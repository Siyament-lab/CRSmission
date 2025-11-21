using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace CRSmission
{
    public class ReceiptStorage
    {
        //Mappnamn för kvitton
        private const string ReceiptDir = "KassaKvitton";
      

        //Metod för att spara kvitto till fil
        public string SaveReceipt(Sales sales)
        {
            if (sales.Items.Count == 0) 
            {
                return string.Empty;
            }
            //Nuvarande mapp plats
            string currentDirectory = Directory.GetCurrentDirectory();

            //Flytta mappen 3 steg uppåt
            //Använder ".." för att snestreck(/\) har olika funktioner berodende på OP-sys.
            string relativePath = Path.Combine(currentDirectory, "..", "..", "..");
            string threeStepsBack = Path.GetFullPath(relativePath);

            //Skapar mapp om den inte existerar
            string directoryPath = Path.Combine(threeStepsBack, ReceiptDir);
            

            Directory.CreateDirectory(directoryPath);

            // Skapat filnamn till kvitton
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"Kvitto_{timeStamp}.txt";
            string fullPath = Path.Combine(directoryPath, fileName);

            //Bygger utformning av kvittot använder sb=stringBuilder
            //Istället för string.
            StringBuilder receiptText = new StringBuilder();
            receiptText.AppendLine("====================MishMash=======================");
            receiptText.AppendLine("                  Kvitto             ");
            receiptText.AppendLine("===================================================");
            receiptText.AppendLine($"Datum: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            receiptText.AppendLine("___________________________________________________");
            receiptText.AppendLine($"{"Produkt", -15} {"Antal", -10} {"Pris/enhet", -12} {"Totalt",-10}");
            receiptText.AppendLine("___________________________________________________");

            foreach (var item in sales.Items) 
            {
                string unitAndQuantity = $"{item.Quantity.ToString("N2")} {item.Product.Unit.GetDescription()}";

                string line = $"{item.Product.Name,-15} {unitAndQuantity,-10} {item.Product.ItemPrice, -12}{item.TotalItemPrice.ToString("C2"),10}";
                receiptText.AppendLine(line);
            }
            receiptText.AppendLine("----------------------------------------------------");
            receiptText.AppendLine($"TOTAL ATT BETALA: {sales.GetCartTotal():C2}");
            receiptText.AppendLine("====================================================");

            try
            {
                // Spara kvittot till fil
                File.WriteAllText(fullPath, receiptText.ToString(), Encoding.UTF8);

                return fullPath;
            }
            catch (Exception ex)
            {
                // fånga ev. fel och meddela. Undviken program- krasch.
                Console.WriteLine($"Något stämmer inte här, Fel: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
