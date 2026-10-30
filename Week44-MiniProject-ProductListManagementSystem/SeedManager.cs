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

            // Find the widest value in each column so everything lines up.
            // The price texts are formatted first so we can measure them.
            int categoryWidth = sortedSeeds.Max(s => s.ProductCategory.Length);
            int nameWidth = sortedSeeds.Max(s => s.ProductName.Length);
            int priceWidth = sortedSeeds.Max(s => FormatPrice(s.ProductPrice).Length);

            foreach (Seed seed in sortedSeeds)
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

            // Print the total price at the bottom, between two divider lines.
            Console.WriteLine();
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"TOTALT PRIS: {FormatPrice(CalculateTotal())} kr");
            Console.WriteLine("-----------------------------");
        }

        // Adds up the price of every seed and returns the total.
        // LINQ: Sum() replaces the foreach loop from Level 2.
        // On an empty list it simply returns 0.
        public decimal CalculateTotal()
        {
            return seeds.Sum(s => s.ProductPrice);
        }

        // Converts a price to text with exactly two decimals.
        // CurrentCulture is sv-SE (set in Program.cs), so the result is "39,00".
        private static string FormatPrice(decimal price)
        {
            return price.ToString("F2", CultureInfo.CurrentCulture);
        }
    }
}
