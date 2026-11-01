using System;
using System.Collections.Generic;
using System.Text;

namespace Week44_MiniProject_ProductListManagementSystem
{
    internal class Seed
    {
        //Constructor
        // CHANGED: the last parameter was "string productCategory", now it is a SeedCategory
        public Seed(string productName, decimal productPrice, bool edible, SeedCategory productCategory)
        {
            ProductName = productName;
            ProductPrice = productPrice;
            Edible = edible;
            ProductCategory = productCategory;
        }

        //Class properties
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        //edible, not necessarily a fruit or vegetable, but can be eaten (like sunflower seeds), or Ringblomma in your salad
        public bool Edible { get; set; }
        // CHANGED: the type was "string", now it is the enum SeedCategory.
        // The value can only be one of the seven categories.
        public SeedCategory ProductCategory { get; set; }

    }
}
