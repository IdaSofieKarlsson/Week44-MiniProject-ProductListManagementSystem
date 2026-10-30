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
        public void AddSeed(Seed seed)
        {
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
        public decimal CalculateTotal()
        {
            decimal total = 0;

            // Plain loop for now. In Level 3 this can be replaced by LINQ's Sum().
            foreach (Seed seed in seeds)
            {
                total += seed.ProductPrice;
            }

            return total;
        }

        // Converts a price to text with exactly two decimals.
        // CurrentCulture is sv-SE (set in Program.cs), so the result is "39,00".
        private static string FormatPrice(decimal price)
        {
            return price.ToString("F2", CultureInfo.CurrentCulture);
        }
    }
}
