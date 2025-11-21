namespace CRSmission
{
    public class Program
    {
        static void Main(string[] args)
        {
            RunCashRegisterSystem();
        }
        static void RunCashRegisterSystem()
        {
            // Skapa instanser av menyklasserna
            CRSHomeMenu homeMenuHandler = new CRSHomeMenu();
            SaleMenu saleMenuHandler = new SaleMenu();

            while (true)
            {
                Console.Clear();

                // Hämta användarens val
                int choice = homeMenuHandler.DisplayHomeMenu();
                
                // Hantera användarens val
                switch (choice)
                {
                    case 1:
                        saleMenuHandler.NewCustomer();
                        break;
                    case 0:
                        Console.WriteLine("Avslutar programmet...");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }



            }

        }
    }
}
