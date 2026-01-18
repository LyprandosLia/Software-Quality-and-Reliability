using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ManageProductsLib
{
    //Βοηθητικό class για την ανάκτηση των χωρών από το json 
    public class CountryData
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string Country { get; set; }
    }

    public static class ManageProducts
    {
        
        //Δημιουργία λίστας για την ανάκτηση των χώρων από το json
        //Κάθε χώρα αποτελείται από ένα εύρος prefix ψηφιών στο οποίο θα ανήκει. 
        private static List<CountryData> _countryList = new List<CountryData>();

        
        static ManageProducts()
        {
            try
            {
                //Ανάκτηση και αποθήκευση της λίστας από το json αρχείο
                string json = File.ReadAllText("countries_ean.json");
                //Μετατροπή του περιεχομένου του json σε λίστα αντικειμένων της C#
                _countryList = JsonSerializer.Deserialize<List<CountryData>>(json) ?? new List<CountryData>();

                Console.WriteLine("List imported successfuly");
            }
            catch
            {
                Console.WriteLine("List is not imported successfuly");
            }
        }

        //1. Η συνάρτηση αυτή δέχεται ως κείμενο έναν αριθμό που αντιστοιχεί σε barcode τύπου
        //EAN-13, δηλαδή αποτελείται από 13 ψηφία, και επιστρέφει τιμή true ή false αν ο
        //αριθμός αυτός αντιστοιχεί σε έγκυρο barcode, βάσει του αλγορίθμου που δίνεται στο
        //παράρτημα.
        public static bool CheckBarcode(string Barcode)
        {
            //Ελέγχος πως το barcode έχει 13 ψηφία
            if (Barcode.Length != 13 || Barcode == null)
            {
                Console.WriteLine("Invalid Barcode Length");
                Console.WriteLine("Exiting...");
                return false;
            }
            int check_digit = Barcode[12] - '0'; //Το τελευταίο ψηφίο του barcode
            int sum = 0;
            int weight = 3;
            for (int i = Barcode.Length - 2; i >= 0; i--)
            {
                if (!char.IsDigit(Barcode[i])) //Έλεγχος πως το Barcode[i] είναι ψηφίο και όχι χαρακτήρας
                    return false;

                int digit = Barcode[i] - '0'; //Cast του char Barcode[i] σε int αφαιρώντας το ASCII 0 (48)
                sum += digit * weight;
                weight = (weight == 3) ? 1 : 3; //Εναλλάγη των βαρών από 3 σε 1 και αντίστροφα, χρησιμοποιώντας σύντομο if statemnt
            }
            int result = (10 - (sum % 10)) % 10;

            return result == check_digit; // Επιστρέφει true στην ισότητα, false διαφορετικά
        }


        //2. Η συνάρτηση αυτή δέχεται ως κείμενο τον αριθμό του barcode και επιστρέφει την
        // χώρα που εξέδωσε τον αριθμό, με βάση τα στοιχεία που δίνονται στον σύνδεσμο: https://hdwrglobal.com/el/blog/96545/%CE%B3%CF%81%CE%B1%CE%BC%CE%BC%CF%89%CF%84%CE%BF%CE%AF-%CE%BA%CF%8E%CE%B4%CE%B9%CE%BA%CE%B5%CF%82-%CF%8C%CE%BB%CE%B1-%CF%8C%CF%83%CE%B1-%CF%80%CF%81%CE%AD%CF%80%CE%B5%CE%B9-%CE%BD%CE%B1-%CE%B3%CE%BD/#id3

        public static string FindCountry(string Barcode)
        {

            if (!CheckBarcode(Barcode))
            {
                return "Invalid Barcode";
            }

            int prefix = int.Parse(Barcode.Substring(0, 3));
            
          
            foreach (var item in _countryList)
            {
                if (prefix >= item.Min && prefix<=item.Max)
                {
                    return item.Country;
                }
            }

            return "Unknown Country";
        }

    }
}   
