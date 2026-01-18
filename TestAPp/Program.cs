using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}
