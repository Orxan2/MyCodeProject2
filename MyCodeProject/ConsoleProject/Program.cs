using System;
using ConsoleProject.Services;
using System.Text;
namespace ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = new MarketServices();


            Console.OutputEncoding = UTF8Encoding.UTF8;
            int selection = 0;

            do
            {
                //Console.WriteLine("1. Məhsullar Üzərində Əməliyyat Aparmaq");
                //Console.WriteLine("2. Satışlar Üzərində Əməliyyat Aparmaq");
                //Console.WriteLine("0. Sistemdən Çıxmaq");

                Console.WriteLine("1. Display");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. Search");

                Console.WriteLine("0. Sistemdən Çıxmaq");

                Console.Write("Bir əmr seçin : ");

                string selectionStr = Console.ReadLine();

                while (!int.TryParse(selectionStr, out selection))
                {
                    Console.WriteLine("Again");
                    selectionStr = Console.ReadLine();
                }

                switch (selection)
                {
                    case 1:
                        MenuServices.DisplayProductList();
                        break;
                    case 2:
                        MenuServices.AddProductMenu();
                        break;
                    case 3:
                        MenuServices.DeleteProductMenu();
                        break;
                    case 4:
                        MenuServices.SearchProductMenu();
                        break;
                    case 0:
                        Console.WriteLine("Exit");
                        break;
                    default:
                        Console.WriteLine("Wrong Assignment");
                        break;
                }
            } while (selection != 0);

            Console.ReadKey();
        }
    }
}
