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
Console.WriteLine("(skriv q som kategori för att visa listan)");
Console.WriteLine();

// The first category is read before the loop. Inside the loop, the next
// category is always read at the end, so "q" can be typed at any time
// the program asks for a category.
string category = ReadCategory();

while (true)
{
    // "q" shows the list. The user can then type q again to quit,
    // or type a category to keep adding seeds.
    if (IsQuit(category))
    {
        manager.ShowSeeds();

        Console.WriteLine();
        Console.WriteLine("Välj q för att avsluta eller fyll i kategori för att fortsätta lägga till frön.");
        Console.WriteLine();

        category = ReadCategory();

        // q a second time (right after the list) ends the program
        if (IsQuit(category))
        {
            break;
        }
    }

    // Read the rest of the seed. Each helper repeats its question
    // until the answer is valid.
    string name = ReadName();
    bool edible = ReadEdible();
    decimal price = ReadPrice();

    // try/catch: if AddSeed rejects the seed (empty value, negative price
    // or a duplicate) it throws an ArgumentException. We catch it, show the
    // message and carry on, so the program never crashes on bad data.
    try
    {
        // The argument order must match the constructor: name, price, edible, category.
        manager.AddSeed(new Seed(name, price, edible, category));

        Console.WriteLine();
        Console.WriteLine("Fröet har lagts till!");
        Console.WriteLine();
    }
    catch (ArgumentException ex)
    {
        ShowError(ex.Message);
    }

    // Ask for the next category (or q to show the list)
    category = ReadCategory();
}

// ------------------------------------------------------------------
// Helper methods. In top-level statements they are written below the
// main code and work as local functions inside the generated Main.
// ------------------------------------------------------------------

// Returns true if the text is "q" (not case sensitive).
static bool IsQuit(string text)
{
    return text.Equals("q", StringComparison.OrdinalIgnoreCase);
}

// Prints an error in red using the format "FEL:" followed by the message.
static void ShowError(string message)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("FEL:");
    Console.WriteLine(message);
    Console.ResetColor();
    Console.WriteLine();
}

// Reads a category. Repeats until the user types something other than
// empty text. "q" is accepted here because it means "show the list".
static string ReadCategory()
{
    while (true)
    {
        Console.Write("Ange kategori: ");
        string input = (Console.ReadLine() ?? string.Empty).Trim();

        if (input.Length > 0)
        {
            return input;
        }

        ShowError("Kategorin får inte vara tom.");
    }
}

// Reads a seed name. Repeats until the user types something other than empty text.
static string ReadName()
{
    while (true)
    {
        Console.Write("Ange namn: ");
        string input = (Console.ReadLine() ?? string.Empty).Trim();

        if (input.Length > 0)
        {
            return input;
        }

        ShowError("Namnet får inte vara tomt.");
    }
}

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

        ShowError("Svara j eller n.");
    }
}

// Reads a price from the user. Accepts both "12,50" and "12.50".
// Repeats until the price is a valid, non-negative number.
static decimal ReadPrice()
{
    while (true)
    {
        Console.Write("Ange pris: ");
        string input = Console.ReadLine() ?? string.Empty;

        // Replace comma with dot and parse with the invariant culture,
        // so the result is the same on every computer.
        string normalized = input.Trim().Replace(',', '.');

        // TryParse returns false instead of throwing an exception
        // when the text is not a number (for example "abc").
        if (!decimal.TryParse(normalized, NumberStyles.Number,
                              CultureInfo.InvariantCulture, out decimal price))
        {
            ShowError("Ogiltigt pris. Ange ett numeriskt värde.");
            continue;
        }

        // Negative prices make no sense
        if (price < 0)
        {
            ShowError("Priset får inte vara negativt.");
            continue;
        }

        return price;
    }
}
