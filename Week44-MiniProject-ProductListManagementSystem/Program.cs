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

// --- Main menu loop ---
// The loop runs until the user chooses to quit.
bool running = true;

while (running)
{
    ShowMenu();

    string choice = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

    // switch picks the code to run depending on the menu choice.
    // "break" ends each case and sends us back to the top of the loop.
    switch (choice)
    {
        case "1":
            AddSeedFromInput(manager);
            break;

        case "2":
            RunNameSearch(manager);
            break;

        case "3":
            RunCategorySearch(manager);
            break;

        case "4":
            manager.ShowSeeds();
            break;

        // Two labels share one block: both "5" and "q" quit the program
        case "5":
        case "q":
            running = false;
            break;

        // Runs for anything that did not match a case above
        default:
            ShowError("Ogiltigt val. Välj ett nummer mellan 1 och 5, eller q för att avsluta.");
            break;
    }
}

Console.WriteLine();
Console.WriteLine("Hej då!");

// ------------------------------------------------------------------
// Helper methods. In top-level statements they are written below the
// main code and work as local functions inside the generated Main.
// ------------------------------------------------------------------

// Prints the main menu.
static void ShowMenu()
{
    Console.WriteLine();
    Console.WriteLine("===================================");
    Console.WriteLine(" FRÖSAMLING");
    Console.WriteLine("===================================");
    Console.WriteLine();
    Console.WriteLine("1. Lägg till frö");
    Console.WriteLine("2. Sök efter namn");
    Console.WriteLine("3. Sök efter kategori");
    Console.WriteLine("4. Visa sorterad lista");
    Console.WriteLine("5. Avsluta (eller q)");
    Console.WriteLine();
    Console.Write("Välj alternativ: ");
}

// Menu option 1: asks for all values and adds a new seed.
static void AddSeedFromInput(SeedManager manager)
{
    Console.WriteLine();

    // Each Read... helper repeats its question until the answer is valid
    string category = ReadRequiredText("Ange kategori: ", "Kategorin får inte vara tom.");
    string name = ReadRequiredText("Ange namn: ", "Namnet får inte vara tomt.");
    bool edible = ReadEdible();
    decimal price = ReadPrice();

    // try/catch: if AddSeed rejects the seed (empty value, negative price
    // or a duplicate) it throws an ArgumentException. We catch it, show the
    // message and go back to the menu, so the program never crashes on bad data.
    try
    {
        // The argument order must match the constructor: name, price, edible, category.
        manager.AddSeed(new Seed(name, price, edible, category));

        Console.WriteLine();
        Console.WriteLine("Fröet har lagts till!");
    }
    catch (ArgumentException ex)
    {
        ShowError(ex.Message);
    }
}

// Menu option 2: searches for seeds by (part of) the name.
static void RunNameSearch(SeedManager manager)
{
    Console.WriteLine();

    string searchText = ReadRequiredText("Sök efter namn: ", "Sökordet får inte vara tomt.");

    // Get the matching seeds from the manager and show them in green
    List<Seed> results = manager.SearchByName(searchText);
    manager.ShowSearchResults(results);
}

// Menu option 3: lists all categories with numbers, the user picks a number,
// and all seeds in that category are shown in green.
static void RunCategorySearch(SeedManager manager)
{
    List<string> categories = manager.GetCategories();

    Console.WriteLine();

    if (categories.Count == 0)
    {
        Console.WriteLine("Inga frön har lagts till ännu, så det finns inga kategorier.");
        return;
    }

    Console.WriteLine("===== KATEGORIER =====");
    Console.WriteLine();

    // Print the categories numbered from 1.
    // The list index starts at 0, so we print i + 1.
    for (int i = 0; i < categories.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {categories[i]}");
    }

    Console.WriteLine();

    int number = ReadNumber(categories.Count);

    // 0 means "go back to the menu"
    if (number == 0)
    {
        return;
    }

    // Convert the number back to a list index (number - 1)
    string chosenCategory = categories[number - 1];

    List<Seed> results = manager.SearchByCategory(chosenCategory);
    manager.ShowSearchResults(results);
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

// Reads a text value. Repeats until the user types something other than
// empty text. Used for category, name and the search word.
static string ReadRequiredText(string prompt, string errorMessage)
{
    while (true)
    {
        Console.Write(prompt);
        string input = (Console.ReadLine() ?? string.Empty).Trim();

        if (input.Length > 0)
        {
            return input;
        }

        ShowError(errorMessage);
    }
}

// Reads a number between 0 and max. Repeats until the input is valid.
// 0 is used as "go back".
static int ReadNumber(int max)
{
    while (true)
    {
        Console.Write("Välj nummer (0 för att gå tillbaka): ");
        string input = (Console.ReadLine() ?? string.Empty).Trim();

        // TryParse returns false instead of throwing an exception
        // when the text is not a whole number.
        if (int.TryParse(input, out int number) && number >= 0 && number <= max)
        {
            return number;
        }

        ShowError($"Ange ett nummer mellan 0 och {max}.");
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
