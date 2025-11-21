using System;
using System.Collections.Generic;
using System.ComponentModel; //Används för förkortningar av enhetstyper
using System.Globalization;
using System.Linq;
using System.Reflection; // Används för att hämta attributinformation och applicera de.
using System.Text;
using System.Threading.Tasks;

namespace CRSmission
{
    
    public class Products
    {
        private static readonly List<Products> MasterProductList = CreateMasterProductList();
        
        //Enhetstyper för produkter
        public enum UnitType
        {
            //Med hjälp av attributet "Description"
            //Definerar vi strängen Styck och Kilogram med förkortningar
            [Description("st")]
            Styck,
            [Description("kg")]
            Kilogram

        
        }
        //Product properties
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal ItemPrice { get; set; }
        public UnitType Unit { get; set; }

        //Constructor för produkter ,pris och enhetstyp
        public Products(int id, string name, decimal price, UnitType unit)
        {
            this.ProductId = id;
            this.Name = name;
            this.ItemPrice = price;
            this.Unit = unit;
        }

        public override string ToString()
        {
            return $"ID: {ProductId, -4}| Namn: {Name, -20}| Pris: {ItemPrice:C2}| Enhet :{Unit.GetDescription()}";
        }

        // Metod för att skapa och fylla med förvalda produkter
        private static List<Products> CreateMasterProductList()
        {
            // Skapa en ny lista av Products-objekt
            List<Products> products = new List<Products>();

            // Lägg till produkter
            products.Add(new Products(101, "Äpple", 7.50m, Products.UnitType.Kilogram));
            products.Add(new Products(102, "Bananer", 11.90m, Products.UnitType.Kilogram));
            products.Add(new Products(201, "Mjölk", 15.90m, Products.UnitType.Styck));
            products.Add(new Products(202, "Fil", 17.50m, Products.UnitType.Styck));
            products.Add(new Products(301, "Lax", 95.90m, Products.UnitType.Kilogram));
            products.Add(new Products(501, "Bröd", 35.00m, Products.UnitType.Styck));
            products.Add(new Products(502, "Tandkräm", 19.95m, Products.UnitType.Styck));

            return products;
        }
        public static List<Products> GetProductList()
        {
            return MasterProductList;
        }
        //Metod för att visa produktlista med ID och namn
        public static void ShowProductList(List<Products> products)
        {
            {
                Console.WriteLine("--- Produktlista ---");
                Console.WriteLine($"{"ID",-4} | {"Namn"}"); 
                Console.WriteLine("--------------------------");

                foreach (var product in products)
                {
                    //skriv ut produkt ID och namn med formatering
                    Console.WriteLine($"{product.ProductId,-4} | {product.Name}");
                }

                Console.WriteLine("--------------------------");
            }

        }
    }
    
    // Klass som representerar en varupost i kundvagn
    public class CartItem
    {
        // Den produkt som varuposten refererar till
        public Products Product { get; set; }

        // Tillagd egenskap: Antal/kvantitet av produkten i kundvagnen
        public decimal Quantity { get; set; }

        // Beräkning av totalpriset för denna varupost
        public decimal TotalItemPrice
        {
            get { return Product.ItemPrice * Quantity; }
        }

        public CartItem(Products product, decimal quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }

        // Överskugga ToString för att ge en snygg formatering för utskrift
        public override string ToString()
        {
            
            return $"ID: {Product.ProductId,-4}| Namn: {Product.Name,-20}| Antal: {Quantity:N2}" +
                $" {Product.Unit.GetDescription(),-4}| Pris totalt: {TotalItemPrice:C2}";
        }
    }
    // Klass för extension-metoder för enum
    public static class EnumExtensions
    {
        // Metod för att hämta beskrivningen från Description-attributet
        public static string GetDescription(this Enum value)
        {
            // Få information om fältet (dvs. enum-värdet)
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field == null)
            {
                // Returnerar standardsträngen om fältet inte hittas
                return value.ToString();
            }

            // Försök att hämta Description-attributet
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            if (attribute != null)
            {
                // Om attributet finns, returnera dess text ("st" eller "kg")
                return attribute.Description;
            }
            else
            {
                // Annars, returnera standardsträngen ("Styck" eller "Kilogram")
                return value.ToString();
            }
        }
    }
}
