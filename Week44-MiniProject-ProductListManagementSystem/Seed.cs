using System;
using System.Collections.Generic;
using System.Text;

namespace Week44_MiniProject_ProductListManagementSystem
{
    internal class Seed
    {
        //Constructor
        public Seed(string productName, decimal productPrice, bool edible, string productCategory)
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
        //for now, this is a string, but in the future, we can make it an enum
        public string ProductCategory { get; set; }

    }
}
