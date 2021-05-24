using System;
using ConsoleProject.Services;
using System.Text;
namespace ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Console.OutputEncoding = UTF8Encoding.UTF8;
            int selection = 0;

            do
            {
                Console.WriteLine("1. Məhsullar Üzərində Əməliyyat Aparmaq");
                Console.WriteLine("2. Satışlar Üzərində Əməliyyat Aparmaq");
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
                        SubMenu.ShowProductOperations();
                        break;
                    case 2:
                        SubMenu.ShowSaleOperations();
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
