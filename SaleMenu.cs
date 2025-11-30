using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CRSmission
{
    public class SaleMenu
    {
        //Create sale menu
        private readonly string[] saleMenuOption = new string[]
        {
            "Tillbaka (Avbryt försäljning)",
            "Lathund ProduktID och Namn",
            "Lägg till produkter",
            "Returnera/ ta bort produkt/er", //Under konstruktion, ingen funktionalitet ännu
            "Betala"
        };

        public void NewCustomer()
        {
            //Anropar Sales klassen för att skapa en ny försäljning
            Sales currentSale = new Sales();

            Console.Clear();
            Console.WriteLine("Ny kund, påbörja produktregistrering!");
            Console.WriteLine("======================================");

            while (true)
            {
                currentSale.DisplayCart();

                Console.WriteLine("\n--- Meny ---");
                for (int i = 0; i < saleMenuOption.Length; i++)
                {
                    Console.WriteLine($"{i}. {saleMenuOption[i]}");
                }

                Console.Write("Välj åtgärd: ");

                if (int.TryParse(Console.ReadLine(), out int saleChoice) 
                    && saleChoice >= 0 && saleChoice < saleMenuOption.Length)
                {
                    
                    switch (saleChoice)
                    {

                        case 0:
                            Console.Clear();
                            Console.WriteLine("Försäljning avbruten.");
                            return;

                        case 1:
                            Console.Clear();
                            Console.WriteLine("--- Lathund ProduktID och Namn ---");
                            List<Products> productList = Products.GetProductList();
                            Products.ShowProductList(productList);
                            break;

                        case 2: // Lägg till produkter i kundvagnen
                            Console.Clear();
                            // Anropar kontinuerliga metoden
                            ContinuousAddProductsToSale(currentSale);
                            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                            Console.ReadKey();
                            break;

                        case 3:
                            Console.Clear();
                            Console.WriteLine("Returnera/ ta bort produkt/er - Under konstruktion");
                            break;

                        case 4: // Betala
                            Console.Clear();
                            if (currentSale.GetCartTotal() > 0)
                            {
                                Console.WriteLine($"\n** TOTALT ATT BETALA: {currentSale.GetCartTotal():C2} **");

                                
                                ReceiptStorage saver = new ReceiptStorage();
                                string receiptPath = saver.SaveReceipt(currentSale); // Anropar SaveReceipt

                                if (!string.IsNullOrEmpty(receiptPath))
                                {
                                    Console.WriteLine("------------------------------------------");
                                    Console.WriteLine($"Kvittot sparades i fil:");
                                    Console.WriteLine($"'{receiptPath}'");
                                    Console.WriteLine("------------------------------------------");
                                }

                                Console.WriteLine("Betalning lyckades.");
                                return; 
                            }
                            else
                            {
                                Console.WriteLine("Kundvagnen är tom. Vänligen lägg till produkter först.");
                            }
                            break;

                        default:
                            Console.WriteLine("Ogiltigt val. Försök igen.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning. Vänligen ange ett nummer från menyn.");
                }
            }
        }



        // Metod för kontinuerlig produktinmatning tills användaren väljer att avsluta
        private void ContinuousAddProductsToSale(Sales currentSale)
        {
            List<Products> masterList = Products.GetProductList();

            Console.Clear();
            //Console.WriteLine("--- Lägg till produkter (Snabb inmatning) ---");
            Console.WriteLine("Ange ID följd av antal/kvantitet");
            //Console.WriteLine("Skriv 'STOP' eller tryck ENTER utan inmatning för att återgå till menyn.");

            while (true)
            {
                // Visa totalsumman i inmatningsläget
                Console.WriteLine($"\nAktuell summa: {currentSale.GetCartTotal():C2}");
                Console.Write("Produkt > ");

                string input = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(input) || input == "0")
                {
                    Console.WriteLine("\nÅtergår till huvudmenyn.");
                    return; // Gå tillbaka till NewCustomer-loopen
                }

                // Dela upp input-strängen med mellanslag
                string[] parts = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    Console.Clear();
                    Console.WriteLine("Inmatningen måste innehålla ID och Antal, separerade av mellanslag.");
                    continue; // Fortsätt loopen för ny inmatning
                }

                // 2. Omvandla ProduktID till heltal
                if (!int.TryParse(parts[0], out int productId) || productId <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Ogiltigt ProduktID.");
                    continue;
                }

                // Deklarera variabel för kvantitet
                string quantityString = parts[1].Replace(',', '.');
                // Omvandla Kvantitet/Antal fr text till tal
                if (!decimal.TryParse(quantityString,
                                      NumberStyles.Any,
                                      CultureInfo.InvariantCulture,
                                      out decimal quantity) || quantity <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("kvantitet måste vara ett positivt tal.");
                    continue;
                }

                // 4. Hitta produkten
                Products productToAdd = masterList.FirstOrDefault(p => p.ProductId == productId);

                if (productToAdd == null)
                {
                    Console.Clear();
                    Console.WriteLine($"FEL: Produkt med ID {productId} hittades inte.");
                    continue;
                }

                // 5. Lägg till produkten i kundvagnen
                currentSale.AddProduct(productToAdd, quantity);

                Console.WriteLine($"OK: {quantity:N2} {productToAdd.Unit.GetDescription()} av {productToAdd.Name} lades till.");
                // Loopar igen för nästa inmatning
            }
        }
    }
}


