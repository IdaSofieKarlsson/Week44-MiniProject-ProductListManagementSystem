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

// The manager owns the list of seeds
SeedManager manager = new SeedManager();

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

    // Create a new Seed and hand it to the manager.
    // The argument order must match the constructor: name, price, edible, category.
    manager.AddSeed(new Seed(name.Trim(), price, edible, category.Trim()));

    Console.WriteLine();
    Console.WriteLine("Fröet har lagts till!");
    Console.WriteLine();
}

manager.ShowSeeds();

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
