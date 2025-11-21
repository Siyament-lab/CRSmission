using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CRSmission
{
    public class CRSHomeMenu
    {
        //Huvudmeny med Array lösning
        public readonly string[] homeMenu = new string[]
        {
            "Avsluta",
            "Ny kund",
            //"Admin 'Under construction'",


        };
        public int DisplayHomeMenu()
        {
            //Deklarera variabeln för användarens val
            int yourChoice;

            while (true) 
            {
                Console.WriteLine("---VÄlkommen till huduvmenyn---");
                //For lopp för menyn och utskrift
                for (int i =0; i < homeMenu.Length; i++)
                {
                    Console.WriteLine($"{i}. {homeMenu[i]}");
                }
                Console.WriteLine("------------***----------------");

                //Uppmana användaren om att göra ett val
                Console.Write("Välj ett alternativ från listan: ");

                //konventera sträng till heltal, om lyckad, utdata blir "yourchoice" med hjälp
                //av "out" -parametern inom ramen för menyns spann.
                if (int.TryParse(Console.ReadLine(), out yourChoice) 
                    && yourChoice >= 0 && yourChoice < homeMenu.Length)
                {
                    return yourChoice;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen..");
                }
            }
        }
    }
}
