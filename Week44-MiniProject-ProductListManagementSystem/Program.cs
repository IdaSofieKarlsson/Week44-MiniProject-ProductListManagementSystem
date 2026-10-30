using System.Globalization;
using System.Text;
using Week44_MiniProject_ProductListManagementSystem;

// --- Swedish language support ---
// UTF-8 makes åäö display and read correctly in the console.
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Use Swedish rules for formatting numbers and comparing text.
CultureInfo.CurrentCulture = new CultureInfo("sv-SE");
CultureInfo.CurrentUICulture = new CultureInfo("sv-SE");

// The list that stores all seeds while the program is running
List<Seed> seeds = new List<Seed>();

Console.WriteLine("==== FRÖSAMLING ====");
Console.WriteLine("(skriv q som kategori för att avsluta)");
Console.WriteLine();

// Keep adding seeds until the user types q
while (true)
{
    Console.Write("Ange kategori: ");
    string category = Console.ReadLine() ?? string.Empty;

    // "q" ends the input loop (case-insensitive, so Q works too)
    if (category.Trim().Equals("q", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    Console.Write("Ange namn: ");
    string name = Console.ReadLine() ?? string.Empty;

    bool edible = ReadEdible();
    decimal price = ReadPrice();

    // Create a new Seed object and add it to the list.
    // The argument order must match the constructor: name, price, edible, category.
    seeds.Add(new Seed(name.Trim(), price, edible, category.Trim()));

    Console.WriteLine();
    Console.WriteLine("Fröet har lagts till!");
    Console.WriteLine();
}

ShowSeeds(seeds);

// ------------------------------------------------------------------
// Helper methods. In top-level statements they are written below the
// main code and work as local functions inside the generated Main.
// ------------------------------------------------------------------

// Asks whether the seed is edible and converts the answer to a bool.
// Repeats the question until the answer is valid.
static bool ReadEdible()
{
    while (true)
    {
        Console.Write("Är den ätbar? (j/n): ");
        string answer = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

        if (answer == "j" || answer == "ja")
        {
            return true;
        }
        if (answer == "n" || answer == "nej")
        {
            return false;
        }

        Console.WriteLine("Svara j eller n.");
    }
}

// Reads a price from the user. Accepts both "12,50" and "12.50".
// Repeats until a number is entered. Full error handling comes in Level 3.
static decimal ReadPrice()
{
    while (true)
    {
        Console.Write("Ange pris: ");
        string input = Console.ReadLine() ?? string.Empty;

        // Replace comma with dot and parse with the invariant culture,
        // so the result is the same on every computer.
        string normalized = input.Trim().Replace(',', '.');

        if (decimal.TryParse(normalized, NumberStyles.Number,
                            CultureInfo.InvariantCulture, out decimal price))
        {
            return price;
        }

        Console.WriteLine("Ogiltigt pris. Ange ett numeriskt värde.");
    }
}

// Prints every seed in the list as a table row.
static void ShowSeeds(List<Seed> seeds)
{
    Console.WriteLine();
    Console.WriteLine("===== FRÖLISTA =====");
    Console.WriteLine();

    if (seeds.Count == 0)
    {
        Console.WriteLine("Inga frön har lagts till.");
        return;
    }

    // Find the longest category and name so the columns line up.
    int categoryWidth = seeds.Max(s => s.ProductCategory.Length);
    int nameWidth = seeds.Max(s => s.ProductName.Length);

    foreach (Seed seed in seeds)
    {
        string edibleText = seed.Edible ? "Ätbar     " : "Inte ätbar";

        // PadRight adds spaces after the text to fill the column width.
        // "0.##" shows 599 as "599" and 12.5 as "12,5" (Swedish format).
        Console.WriteLine(
            $"{seed.ProductCategory.PadRight(categoryWidth)} | " +
            $"{seed.ProductName.PadRight(nameWidth)} | " +
            $"{edibleText} | " +
            $"{seed.ProductPrice.ToString("0.##", CultureInfo.CurrentCulture)} kr");
    }
}