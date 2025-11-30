using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CRSmission
{
    public class Sales
    {
        // deklarerar en lista för försäljningsobjekt (anrop av CartItem klassen fr Products)
        public List<CartItem> Items { get; private set; } = new List<CartItem>();

        
        // Lägger till eller uppdaterar en produkt i försäljningen.
        public void AddProduct(Products product, decimal quantity)
        {
            if (quantity <= 0)
            {
                return;
            }

            // Försök hitta om produkten redan finns (med samma ProductId)
            CartItem existingItem = Items.FirstOrDefault(item => item.Product.ProductId == product.ProductId);

            if (existingItem != null)
            {
                // Uppdatera antalet
                existingItem.Quantity += quantity;
            }
            else
            {
                // Lägg till som en ny post
                Items.Add(new CartItem(product, quantity));
            }
        }

        
        // Beräknar den totala summan av hela försäljningen.
        public decimal GetCartTotal()
        {
            return Items.Sum(item => item.TotalItemPrice);
        }


        // Översikt av nuvarande produkter i kundvagnen
        public void DisplayCart()
        {
            Console.WriteLine("\n--- Aktuell Försäljning/Kundvagn ---");
            Console.WriteLine($"{"ID",-7} | {"Namn",-24} | {"Antal/Enhet",-20} | {"Pris totalt"}");
            Console.WriteLine("-------------------------------------------------------------------");

            if (!Items.Any())
            {
                Console.WriteLine("Finns inga produkter för tillfället");
            }
            else
            {
                foreach (var item in Items)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            //Finns inget behov
            //Console.WriteLine("-------------------------------------------------------------------");
            //Console.WriteLine($"** Totalsumma: {GetCartTotal():C2} **");
            //Console.WriteLine("-------------------------------------------------------------------");
        }

        // Under konstruktion för borttagning av produkter, returer, lägga till kampanjer etc.
    }
}
