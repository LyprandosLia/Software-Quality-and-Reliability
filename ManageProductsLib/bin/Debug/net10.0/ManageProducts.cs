using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
    //Βοηθητικό class για την αποθήκευση των εύρων ΤΚ και των αντίστοιχων περιοχών
    public class ZipRange
    {
        public int From { get; set; }
        public int To { get; set; }
        public string Region { get; set; }

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
            //Καταρχάς ελέγχουμε αν το barcode είναι έγκυρο
            if (!CheckBarcode(Barcode))
            {
                return "Invalid Barcode";
            }
            //Παίρνουμε τα 3 πρώτα ψηφία του barcode ως πρόθεμα
            int prefix = int.Parse(Barcode.Substring(0, 3));

            //Αναζήτηση της χώρας με βάση το πρόθεμα
            foreach (var item in _countryList)
            {
                if (prefix >= item.Min && prefix <= item.Max)
                {
                    return item.Country; //Επιστροφή της χώρας αν το πρόθεμα ανήκει στο εύρος
                }
            }
            //Αν δεν βρεθεί η χώρα
            return "Unknown Country";
        }


        //3. Η συνάρτηση αυτή δέχεται ως κείμενο έναν ΤΚ, ο οποίος αφορά κυπριακή διεύθυνση,
        //και επιστρέφει αν είναι έγκυρος.Επιπλέον, στην περίπτωση που είναι έγκυρος,
        //επιστρέφει στην by ref παράμετρο την περιοχή στην οποία αντιστοιχεί ο
        //συγκεκριμένος ΤΚ.Ο έλεγχος των ΤΚ γίνεται βάσει του συνδέσμου: ΤΚ Κύπρου
        public static bool CheckZipCode(string ZipCode, ref string Region)
        {
            //Λίστα με τους έγκυρους ΤΚ και τις αντίστοιχες περιοχές
            var zipCodeData = new List<ZipRange>
            {
                new ZipRange { From = 1000, To = 2999, Region = "Nicosia District" },
                new ZipRange { From = 3000, To = 4999, Region = "Limassol District" },
                new ZipRange { From = 5000, To = 5999, Region = "Famagusta District" },
                new ZipRange { From = 6000, To = 6999, Region = "Larnaca District" },
                new ZipRange { From = 8000, To = 8999, Region = "Paphos District" },
                new ZipRange { From = 9000, To = 9999, Region = "Kyrenia District" }
            };
            //Έλεγχος αν ο ΤΚ είναι έγκυρος
            var match = zipCodeData.FirstOrDefault(z => int.TryParse(ZipCode, out int code) && code >= z.From && code <= z.To);

            //Έλεγχος αν είναι έγκυρος και επιστροφή του Region
            if (match != null)
            {
                Region = match.Region;
                return true;
            }
            //Αν δεν είναι έγκυρος
            else
            {
                Region = string.Empty;
                return false;
            }
        }


        //4.Η μέθοδος αυτή δέχεται ως παράμετρο τα στοιχεία ενός προϊόντος, και επιστρέφει
        //στις by ref παραμέτρους, την τιμή του προϊόντος με ΦΠΑ, την έκπτωση του προϊόντος
        //επί της τιμής με ΦΠΑ, και την τελική τιμή του προϊόντος.O υπολογισμός του ΦΠΑ
        //γίνεται σύμφωνα με το παράρτημα.
        public static void CalculateCost(Product Prd, ref double PriceWithTaxes, ref double Discount, ref double FinalPrice)
        {

            double vat;

            switch (Prd.Category)
            {
                case "Ηλεκτρονικά":
                case "Καλλυντικά":
                case "Αλκοόλ":
                    vat = 0.24; //Κανονικός
                    break;

                case "Τρόφιμα":
                case "Αναψυκτικά":
                    vat = 0.13; //Μειωμένος
                    break;

                case "Βιβλία":
                    vat = 0.06; //Υπερμειωμένος
                    break;

                default:
                    vat = 0; //Unknown κατηγορία
                    break;
            }


            //Υπολογισμοί
            PriceWithTaxes = Prd.Price * (1 + vat);

            Discount = PriceWithTaxes * Prd.Discount / 100.0;

            FinalPrice = PriceWithTaxes - Discount;

        }

        //5. Η συνάρτηση αυτή δέχεται ως παράμετρο μια λίστα προϊόντων, και επιστρέφει το
        // συνολικό τους κόστος με ΦΠΑ.O υπολογισμός του ΦΠΑ γίνεται σύμφωνα με το
        //παράρτημα
        public static double CalculateTotalCost(Product[] Prds)
        {

            //Ελέγχος αν ο πίνακας είναι null
            if (Prds == null)
            {
                throw new ArgumentNullException(nameof(Prds));
            }
            double totalCost = 0.0;

            foreach (var prd in Prds)
            {
                if (prd == null) //Αν κάποιο προϊόν στον πίνακα είναι null
                {
                    throw new ArgumentNullException("Product in the array is null");
                }
                double priceWithTaxes = 0.0;
                double discount = 0.0;
                double finalPrice = 0.0;
                //Κλήση της CalculateCost για κάθε προϊόν που υλοποιήθηκε στο 4.
                CalculateCost(prd, ref priceWithTaxes, ref discount, ref finalPrice);
                totalCost += finalPrice;
            }
            return totalCost;
        }

        //6.Η συνάρτηση αυτή δέχεται ως παράμετρο μια λίστα προϊόντων και επιστρέφει το
        //πλήθος των προϊόντων(δηλαδή κωδικών barcode) της συγκεκριμένης χώρας
        //προέλευσης που δέχεται ως παράμετρο.
        public static int AmountOfProducts(Product[] Prds, string Country)
        {
            int count = 0;
            //Ελέγχος αν ο πίνακας είναι null
            if (Prds == null)
            {
                throw new ArgumentNullException(nameof(Prds));
            }

            foreach (var prd in Prds)
            {
                //Αν κάποιο προϊόν στον πίνακα είναι null
                if (prd == null)
                {
                    throw new ArgumentNullException("Product in the array is null");
                }
                //Κλήση της FindCountry για κάθε προϊόν που υλοποιήθηκε στο 2.
                string findCountry = FindCountry(prd.Barcode);

                //Αν κάνουν match, αυξανουμε τον μετρητή κατά 1
                if (Country == findCountry)
                {
                    count++;
                }
            }
            return count;
        }


        //7. Η συνάρτηση αυτή δέχεται ως παράμετρο μια λίστα προϊόντων, και επιστρέφει αν
        //υπάρχουν προϊόντα που το απόθεμά τους είναι κάτω από το όριο αποθέματος, και
        //ποια προϊόντα είναι αυτά στην by ref παράμετρο.
        public static bool ProductsTopOrder(Product[] Prds, int StockLimit, ref Product[] PrdsToOrder)
        {
            if (Prds == null)
            {
                throw new ArgumentNullException(nameof(Prds));
            }

            List<Product> productsToOrderList = new List<Product>();
            foreach (var prd in Prds)
            {
                if (prd == null)
                {
                    throw new ArgumentNullException("Product in the array is null");

                }
                if (prd.Stock < StockLimit)
                {
                    productsToOrderList.Add(prd);
                }


            }
            PrdsToOrder = productsToOrderList.ToArray();
            return PrdsToOrder.Length > 0;
        }
    }
}