using System;
using System.Collections.Generic;
using System.Text;
using ManageProductsLib;

namespace ManageProductsLib
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Testing finction CheckBarcode()
           
            Console.WriteLine($"Barcode 1: {4006381333931} is valid: {ManageProducts.CheckBarcode("4006381333931")}"); //Should be true
            Console.WriteLine($"Barcode 2: {4006381333932} is valid: {ManageProducts.CheckBarcode("4006381333932")}"); //Should be false
            Console.WriteLine($"Barcode 3: {6291041500213} is valid: {ManageProducts.CheckBarcode("6291041500213")}"); //Should be true
            Console.WriteLine($"Barcode 4: {5901234123450} is valid: {ManageProducts.CheckBarcode("5901234123450")}"); //Should be false
            Console.WriteLine($"Barcode 5: {5901234123457} is valid: {ManageProducts.CheckBarcode("5901234123457")}"); //Should be true
          



            //Testing function FindCountry()
            Console.WriteLine($"Country 1: {4006381333931} is : {ManageProducts.FindCountry("4006381333931")}"); 
            Console.WriteLine($"Country 2: {4006381333932} is : {ManageProducts.FindCountry("4006381333932")}"); 
            Console.WriteLine($"Country 3: {6291041500213} is : {ManageProducts.FindCountry("6291041500213")}"); 
            Console.WriteLine($"Country 4: {5901234123450} is : {ManageProducts.FindCountry("5901234123450")}"); 
            Console.WriteLine($"Country 5: {5901234123457} is : {ManageProducts.FindCountry("5901234123457")}");

            //Testing function CheckZipCode() and CalculateCost()

            string zipcode = "1010";
            string region = "";
            bool isValidZip = ManageProducts.CheckZipCode(zipcode, ref region);
            Console.WriteLine($"ZipCode {zipcode} is valid: {isValidZip}, Region: {region}"); //Should print: ZipCode 1010 is valid: True, Region: Nicossia

            zipcode = "9999";
            region = "";
            isValidZip = ManageProducts.CheckZipCode(zipcode, ref region);
            Console.WriteLine($"ZipCode {zipcode} is valid: {isValidZip}, Region: {region}"); //Should print: ZipCode 9999 is valid: True, Region: Kyrenia


            Product p1 = new Product
            {
                Barcode = "4006381333931",
                Country = "Γερμανία",
                Stock = 50,
                Category = "Ηλεκτρονικά",
                Price = 100.0,
                Discount = 10
            };

            Product p2 = new Product
            {
                Barcode = "5901234123457",
                Country = "Πολωνία",
                Category = "Τρόφιμα",
                Stock = 100,
                Price = 50.0,
                Discount = 5 // 5%
            };

            double priceWithTaxes1 = 0.0;
            double discount = 0.0;
            double finalPrice1 = 0.0;

            ManageProducts.CalculateCost(p1, ref priceWithTaxes1, ref discount, ref finalPrice1);
            Console.WriteLine($"Product: {p1.Category}, PriceWithVAT: {priceWithTaxes1}, Discount: {discount}, FinalPrice: {finalPrice1}");
            //// Should print: PriceWithVAT = 124, Discount = 12.4, FinalPrice = 111.6

            priceWithTaxes1 = 0.0;
            discount = 0.0;
            finalPrice1 = 0.0;

            ManageProducts.CalculateCost(p2, ref priceWithTaxes1, ref discount, ref finalPrice1);
            Console.WriteLine($"Product: {p2.Category}, PriceWithVAT: {priceWithTaxes1}, Discount: {discount}, FinalPrice: {finalPrice1}");
            // Should print: PriceWithVAT = 56.5, Discount = 2.825, FinalPrice = 53.675



            //Testing function CalculateTotalCost()
            Product[] products = new Product[]
            {
                new Product { Category = "Ηλεκτρονικά", Price = 100, Discount = 10 },
                new Product { Category = "Τρόφιμα", Price = 50, Discount = 5 },
                new Product { Category = "Βιβλία", Price = 200, Discount = 0 }
            };

            double total = ManageProducts.CalculateTotalCost(products);
            Console.WriteLine($"Total cost with VAT & discounts: {total}");

            //Should print: Total cost with VAT & discounts: 377.275


            //Testing function AmountofProducts()
            Product[] products2 = new Product[]
             {
                new Product { Barcode = "4006381333931", Category = "Ηλεκτρονικά", Price = 100 },
                new Product { Barcode = "5901234123457", Category = "Τρόφιμα", Price = 50 },
                new Product { Barcode = "4001234567890", Category = "Βιβλία", Price = 200 }
             };

            int germanProducts = ManageProducts.AmountOfProducts(products2, "Germany");
            Console.WriteLine($"Number of products from Germany: {germanProducts}");

            int polandProducts = ManageProducts.AmountOfProducts(products2, "Poland");
            Console.WriteLine($"Number of products from Poland: {polandProducts}");

            //Testing function ProductsTopOrder()
            Product[] products3 = new Product[]
       {
            new Product { Barcode = "4006381333931", Category = "Ηλεκτρονικά", Price = 100, Stock = 50 },
            new Product { Barcode = "5909876543217", Category = "Τρόφιμα", Price = 50, Stock = 5 },
            new Product { Barcode = "4001234567890", Category = "Βιβλία", Price = 200, Stock = 20 },
            new Product { Barcode = "5901111111118", Category = "Καλλυντικά", Price = 30, Stock = 2 }
       };

            int stockLimit = 10;
            Product[] productsToOrder = null;

            bool needOrder = ManageProducts.ProductsTopOrder(products3, stockLimit, ref productsToOrder);

            Console.WriteLine($"Are there products to order? {needOrder}");
            Console.WriteLine($"Number of products to order: {productsToOrder.Length}");
            Console.WriteLine("Products to order:");

            foreach (var p in productsToOrder)
            {
                Console.WriteLine($"- {p.Category}, Barcode: {p.Barcode}, Stock: {p.Stock}");
            }
        }
    }
            
    }

