using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Week44_MiniProject_ProductListManagementSystem
{
    /// <summary>
    /// Manages the seed list: adding, showing and calculating the total price.
    /// Program.cs handles user input, this class handles the data.
    /// </summary>
    internal class SeedManager
    {
        // The list that stores all seeds while the program is running.
        // "private" means only this class can change the list directly.
        private readonly List<Seed> seeds = new List<Seed>();

        // Adds a seed to the list.
        // The checks below are a safety net: even if some other code forgets to
        // validate the input, the manager never stores an invalid seed.
        // An invalid seed makes the method throw an ArgumentException,
        // which the caller catches with try/catch.
        public void AddSeed(Seed seed)
        {
            if (string.IsNullOrWhiteSpace(seed.ProductName))
            {
                throw new ArgumentException("Namnet får inte vara tomt.");
            }

            if (string.IsNullOrWhiteSpace(seed.ProductCategory))
            {
                throw new ArgumentException("Kategorin får inte vara tom.");
            }

            if (seed.ProductPrice < 0)
            {
                throw new ArgumentException("Priset får inte vara negativt.");
            }

            // LINQ: Where() narrows the list down to the same category,
            // FirstOrDefault() then returns the first seed with the same name,
            // or null if there is none. Comparison ignores upper/lower case
            // and follows Swedish rules (so "Ärtor" equals "ärtor").
            Seed? existing = seeds
                .Where(s => string.Equals(s.ProductCategory, seed.ProductCategory,
                                          StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault(s => string.Equals(s.ProductName, seed.ProductName,
                                                   StringComparison.CurrentCultureIgnoreCase));

            if (existing != null)
            {
                throw new ArgumentException(
                    $"'{seed.ProductName}' finns redan i kategorin '{seed.ProductCategory}'.");
            }

            seeds.Add(seed);
        }

        // Returns every distinct category in the list, sorted alphabetically.
        // Distinct() removes duplicates (ignoring upper/lower case), and
        // OrderBy() with the current culture (sv-SE) puts å, ä, ö after z.
        public List<string> GetCategories()
        {
            return seeds
                .Select(s => s.ProductCategory)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(c => c, StringComparer.CurrentCultureIgnoreCase)
                .ToList();
        }

        // Returns all seeds whose name contains the search text.
        // Contains() makes it a partial match: "ros" finds "Ringblomma ros" and
        // "Rosenkål". The comparison ignores upper/lower case.
        public List<Seed> SearchByName(string searchText)
        {
            return seeds
                .Where(s => s.ProductName.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        // Returns all seeds that belong to the given category.
        public List<Seed> SearchByCategory(string category)
        {
            return seeds
                .Where(s => string.Equals(s.ProductCategory, category,
                                          StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        // Prints all seeds sorted from lowest to highest price,
        // followed by the total price at the bottom.
        public void ShowSeeds()
        {
            Console.WriteLine();
            Console.WriteLine("===== FRÖLISTA =====");
            Console.WriteLine();

            if (seeds.Count == 0)
            {
                Console.WriteLine("Inga frön har lagts till.");
                return;
            }

            // OrderBy sorts by price (lowest first). It returns a new sorted
            // sequence, so the original list keeps its insertion order.
            List<Seed> sortedSeeds = seeds.OrderBy(s => s.ProductPrice).ToList();

            // null = print in the normal console color
            PrintTable(sortedSeeds, null);

            // Print the total price at the bottom, between two divider lines.
            Console.WriteLine();
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"TOTALT PRIS: {FormatPrice(CalculateTotal())} kr");
            Console.WriteLine("-----------------------------");
        }

        // Prints search results highlighted in green.
        // Called with the list that SearchByName() or SearchByCategory() returned.
        public void ShowSearchResults(List<Seed> results)
        {
            Console.WriteLine();

            if (results.Count == 0)
            {
                Console.WriteLine("Inga frön hittades.");
                return;
            }

            // Heading and rows are printed in green so the hits stand out.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("HITTADE FRÖN:");
            Console.ResetColor();
            Console.WriteLine();

            PrintTable(results.OrderBy(s => s.ProductPrice).ToList(), ConsoleColor.Green);
        }

        // Adds up the price of every seed and returns the total.
        // LINQ: Sum() adds up the prices. On an empty list it returns 0.
        public decimal CalculateTotal()
        {
            return seeds.Sum(s => s.ProductPrice);
        }

        // Prints a list of seeds as an aligned table.
        // If a color is given, every row is printed in that color.
        // Used by both ShowSeeds() and ShowSearchResults().
        private static void PrintTable(List<Seed> list, ConsoleColor? color)
        {
            // Find the widest value in each column so everything lines up.
            // The price texts are formatted first so we can measure them.
            int categoryWidth = list.Max(s => s.ProductCategory.Length);
            int nameWidth = list.Max(s => s.ProductName.Length);
            int priceWidth = list.Max(s => FormatPrice(s.ProductPrice).Length);

            // Switch to the highlight color (if any) before printing the rows
            if (color != null)
            {
                Console.ForegroundColor = color.Value;
            }

            foreach (Seed seed in list)
            {
                string edibleText = seed.Edible ? "Ätbar     " : "Inte ätbar";

                // PadRight fills the column with spaces after the text,
                // PadLeft fills with spaces before the text (right-aligned).
                Console.WriteLine(
                    $"{seed.ProductCategory.PadRight(categoryWidth)} | " +
                    $"{seed.ProductName.PadRight(nameWidth)} | " +
                    $"{edibleText} | " +
                    $"{FormatPrice(seed.ProductPrice).PadLeft(priceWidth)} kr");
            }

            // Always go back to the normal color afterwards
            Console.ResetColor();
        }

        // Converts a price to text with exactly two decimals.
        // CurrentCulture is sv-SE (set in Program.cs), so the result is "39,00".
        private static string FormatPrice(decimal price)
        {
            return price.ToString("F2", CultureInfo.CurrentCulture);
        }
    }
}