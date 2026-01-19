using System;
using System.Collections.Generic;
using System.Text;

namespace ManageProductsLib
{
    public class Product
    {
        public string Barcode { get; set; }
        public string Country { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
    }
}
