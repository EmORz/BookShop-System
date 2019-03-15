using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BookShop
{
    using BookShop.Data;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                var str = Console.ReadLine();
               // DbInitializer.ResetDatabase(db);
                //var command = int.Parse(Console.ReadLine());
                var result = GetAuthorNamesEndingIn(db, str);
                //Console.WriteLine(result+ " books were deleted");
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
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            //Write a GetAuthorNamesEndingIn(BookShopContext context, string input) method that returns the full names of authors,
            //whose first name ends with a given string.
            // Return all names in a single string, each on a new row, ordered alphabetically.
            var result = context.Authors
                .Where(a => a.FirstName.EndsWith(input) && a.FirstName!=null)
                .Select(x => x.FirstName == null? $"{x.LastName}": $"{x.FirstName} {x.LastName}")
                .OrderBy(x => x);
            return String.Join(Environment.NewLine, result);
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            /*8.	Book Search
               Write a GetBookTitlesContaining(BookShopContext context, string input) method that returns the titles of book, which contain a given string.
               Ignore casing.
               Return all titles in a single string, each on a new row, ordered alphabetically.
               */
            var result = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x);

            return String.Join(Environment.NewLine, result);
        }
        public static string GetBooksByAuthor(BookShopContext context, string command)
        {
            //Write a GetBooksByAuthor(BookShopContext context, string input) method that returns all titles of books and
            //their authors’ names for books, which are written by authors whose last names start with the given string.
            // Return a single string with each title on a new row. Ignore casing. Order by book id ascending.
            var titlesAuthors = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(command.ToLower()))
                    .OrderBy(x => x.BookId)
                .Select(x => $"{x.Title} ({x.Author.FirstName} {x.Author.LastName})");
            return String.Join(Environment.NewLine, titlesAuthors);
        }
        public static int CountBooks(BookShopContext context, int command)
        {
           // Write a CountBooks(BookShopContext context, int lengthCheck) method that returns the number of books, which have a title
           // longer than the number given as an input.
            var countBooks = context.Books
                .Where(x => x.Title.Length > command)
                .Select(x => x.Title.Length);
            var temp = (int)countBooks.Count();
            return temp;
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            //Write a method CountCopiesByAuthor(BookShopContext context) that returns the total number of book copies for each author.
            //Order the results descending by total book copies.
            //Return all results in a single string, each on a new line.
            var temp = context.Authors
                .Select(x => new
                {
                    FullName = $"{x.FirstName} {x.LastName}",
                    TotalCopies = x.Books.Sum(cop => cop.Copies )
                })
                .OrderByDescending(x => x.TotalCopies)
                .Select(x => $"{x.FullName} - {x.TotalCopies}");
            return string.Join(Environment.NewLine, temp);
        }
     
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            //Write a method GetTotalProfitByCategory(BookShopContext context) that returns the total profit of all books by category.
            //Profit for a book can be calculated by multiplying its number of copies by the price per single book.
            //Order the results by descending by total profit for category and ascending by category name.
            var temp = context.Categories
                .Select(x => new
                {
                    TypeCatecories = x.Name,
                    TotalProfit = x.CategoryBooks.Select(a => a.Book.Price*a.Book.Copies).Sum()
                })
                .OrderByDescending( x => x.TotalProfit)
                .ThenBy(x => x.TypeCatecories)
                .Select(b => $"{b.TypeCatecories} ${b.TotalProfit}" );
            return string.Join(Environment.NewLine, temp);
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            // Get the most recent books by categories in a GetMostRecentBooks(BookShopContext context) method.The categories should
            //be ordered by name alphabetically. Only take the top 3 most recent books from each category -ordered by release date(descending).
            //Select and print the category name, and for each book – its title and release year.
            var temp = context.Categories
                .Select(x => new
                {
                    Name = x.Name,
                    BookCount = x.CategoryBooks
                                 .Select(b => b.Book)
                                 .Count(),
                    TopThreee = string.Join(Environment.NewLine, x.CategoryBooks
                        .Select(sb => sb.Book)
                        .OrderByDescending(c => c.ReleaseDate)
                        .Take(3)
                        .Select(s => $"{s.Title} {s.ReleaseDate.Value.Year}"))
                })
                .OrderBy(a => a.Name)
                .Select(d => $"--{d.Name}{Environment.NewLine}{d.TopThreee}");
            return string.Join(Environment.NewLine, temp);
        }
        public static void IncreasePrices(BookShopContext context)
        {
            //Write a method IncreasePrices(BookShopContext context) that increases the prices of all books released before 2010 by 5.
            var increasePrice = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010);
            foreach (var inc in increasePrice)
            {
                inc.Price += 5;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            //Write a method RemoveBooks(BookShopContext context) that removes from the database those books, which have less than 4200 copies.
            //Return an int -the number of books that were deleted from the database.
            var getBooksWihLessCopies = context.Books
                .Where(x => x.Copies < 4200).ToArray();

            context.RemoveRange(getBooksWihLessCopies);
            context.SaveChanges();

            return getBooksWihLessCopies.Length;
        }



    }
}
