using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Services
{
    class SubMenu
    {
        public static void ShowProductOperations()
        {
           
            int selection = 0;

            do
            {
                Console.WriteLine("1. Display");
                Console.WriteLine("2. Add");
                Console.WriteLine("3. Delete");
                Console.WriteLine("4. Search");
                Console.WriteLine("5. Edit");
                Console.WriteLine("6. Search For Price");
                Console.WriteLine("7. Search For Category");

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
                    case 5:
                        MenuServices.EditProductMenu();
                        break;
                    case 6:
                        MenuServices.SearchProductMenuForPrice();
                        break;
                    case 7:
                        MenuServices.SearchProductMenuForCategory();
                        break;
                    case 0:
                        Console.WriteLine("Exit");
                        break;
                    default:
                        Console.WriteLine("Wrong Assignment");
                        break;
                }
            } while (selection != 0);
        }

        public static void ShowSaleOperations()
        {
            int selection = 0;

            do
            {
                //Console.WriteLine("1. Display");
                Console.WriteLine("2. Add");
                //Console.WriteLine("3. Delete");
                //Console.WriteLine("4. Search");
                //Console.WriteLine("5. Edit");
                //Console.WriteLine("6. Search For Price");
                //Console.WriteLine("7. Search For Category");

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
                    //case 1:
                    //    MenuServices.DisplayProductList();
                    //    break;
                    case 2:
                        MenuServices.AddSaleMenu();
                        break;
                    //case 3:
                    //    MenuServices.DeleteProductMenu();
                    //    break;
                    //case 4:
                    //    MenuServices.SearchProductMenu();
                    //    break;
                    //case 5:
                    //    MenuServices.EditProductMenu();
                    //    break;
                    //case 6:
                    //    MenuServices.SearchProductMenuForPrice();
                    //    break;
                    //case 7:
                    //    MenuServices.SearchProductMenuForCategory();
                    //    break;
                    case 0:
                        Console.WriteLine("Exit");
                        break;
                    default:
                        Console.WriteLine("Wrong Assignment");
                        break;
                }
            } while (selection != 0);
        }
    }
}
