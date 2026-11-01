using System;
using System.Collections.Generic;
using System.Text;

namespace Week44_MiniProject_ProductListManagementSystem
{
    // NEW: the enum that replaces the free-text category.
    // Member names are in English and contain no å, ä, ö or spaces.
    // The Swedish text shown to the user comes from GetDisplayName() below.
    // To add a category later, add one member here and one line in GetDisplayName().
    internal enum SeedCategory
    {
        Brassicas,        // Kålväxter
        RootVegetables,   // Rotfrukter
        Alliums,          // Lökväxter
        Legumes,          // Baljväxter
        OtherVegetables,  // Övriga grönsaker
        SummerFlowers,    // Sommarblommor
        Perennials        // Perenner
    }

    // NEW: extension methods for SeedCategory.
    // An extension class must be static. The "this" before the first parameter
    // makes it possible to write category.GetDisplayName() on any SeedCategory value.
    internal static class SeedCategoryExtensions
    {
        // Converts an enum value to the Swedish text shown in the console.
        public static string GetDisplayName(this SeedCategory category)
        {
            // A switch expression: each line is "value => result".
            return category switch
            {
                SeedCategory.Brassicas => "Kålväxter",
                SeedCategory.RootVegetables => "Rotfrukter",
                SeedCategory.Alliums => "Lökväxter",
                SeedCategory.Legumes => "Baljväxter",
                SeedCategory.OtherVegetables => "Övriga grönsaker",
                SeedCategory.SummerFlowers => "Sommarblommor",
                SeedCategory.Perennials => "Perenner",

                // Reached only if a member was added to the enum but not to this
                // switch. Failing loudly here makes the mistake easy to find.
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };
        }
    }
}
