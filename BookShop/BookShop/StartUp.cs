using System;
using System.Globalization;
using System.Linq;
using System.Text;
using BookShop.Models;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                var command = (Console.ReadLine());
                var result = GetBooksReleasedBefore(db, command);
                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);
            var result = context.Books.Where(x => x.AgeRestriction == ageRestriction)
                .Select(x => x.Title)
                .OrderBy(x => x);
           
            return String.Join(Environment.NewLine, result);
        }


        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenEdition = (EditionType)Enum.Parse(typeof(EditionType), "Gold" , true);
            var result = context.Books.Where(x => x.EditionType == goldenEdition && x.Copies<5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title);

            return String.Join(Environment.NewLine, result);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var result = context.Books.Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new
                {
                    x.Title,
                    x.Price
                });
            StringBuilder sb = new StringBuilder();
            foreach (var r in result)
            {
                sb.AppendLine($"{r.Title} - ${r.Price:F2}");
            }
            return sb.ToString().Trim();
            
        }


        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            //returns in a single string all titles of books that are NOT released on a given year. Order them by book id ascending.
            var booksNotReleased = context.Books.Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title);
            return String.Join(Environment.NewLine, booksNotReleased);
        }

        public static string GetBooksByCategory(BookShopContext context, string command)
        {
           // method that selects and returns in a single string the titles of books by a given list of categories.
           // The list of categories will be given in a single line separated with one or more spaces.Ignore casing.
           // Order by title alphabetically.
            var listOfCategories = command.ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var bookCategories = context.Books
                .Where(x => x.BookCategories
                    .Select(a => a.Category.Name.ToLower())
                    .Intersect(listOfCategories)
                    .Any())
                    .Select(x => x.Title)
                .OrderBy(x => x);
            return String.Join(Environment.NewLine, bookCategories);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            //method that returns the title, edition type and price of all books that are released before a given date. The date will be a string in format dd-MM-yyyy.
            // Return all of the rows in a single string, ordered by release date descending.
            var temp = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var result = context.Books
                .Where(x => x.ReleaseDate < temp)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => $"{x.Title} - {x.EditionType} - ${x.Price:f2}");


            return String.Join(Environment.NewLine, result);
        }



        public static string GetAuthorNamesEndingIn(BookShopContext context, string command)
        {
            return "";
        }
        public static string GetBookTitlesContaining(BookShopContext context, string command)
        {
            return "";
        }
        public static string GetBooksByAuthor(BookShopContext context, string command)
        {
            return "";
        }
        public static string CountBooks(BookShopContext context, string command)
        {
            return "";
        }
        public static string CountCopiesByAuthor(BookShopContext context, string command)
        {
            return "";
        }
     
        public static string GetTotalProfitByCategory(BookShopContext context, string command)
        {
            return "";
        }
        public static string GetMostRecentBooks(BookShopContext context, string command)
        {
            return "";
        }
        public static string IncreasePrices(BookShopContext context, string command)
        {
            return "";
        }

        public static string RemoveBooks(BookShopContext context, string command)
        {
            return "";
        }



    }
}
