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
                Console.WriteLine("1. Display All Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. Restore Product");
                Console.WriteLine("5. Search Products For Name");
                Console.WriteLine("6. Edit Product");
                Console.WriteLine("7. Search Products For Price");
                Console.WriteLine("8. Search Products For Category");
                

                Console.WriteLine("0. Go Back");

                Console.Write("Please Select a Command : ");

                string selectionStr = Console.ReadLine();

                while (!int.TryParse(selectionStr, out selection))
                {
                    Console.WriteLine("Command was entered incorrectly. Please try again");
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
                        MenuServices.ReturnProductMenu();
                        break;
                    case 5:
                        MenuServices.SearchProductMenu();
                        break;
                    case 6:
                        MenuServices.EditProductMenu();
                        break;
                    case 7:
                        MenuServices.SearchProductMenuForPrice();
                        break;
                    case 8:
                        MenuServices.SearchProductMenuForCategory();
                        break;
                   
                    case 0:
                        //Console.WriteLine("Exit");
                        break;
                    default:
                        Console.WriteLine("You have selected the wrong command");
                        break;
                }
            } while (selection != 0);
        }

        public static void ShowSaleOperations()
        {
            int selection = 0;

            do
            {
               
                Console.WriteLine("1. Add Sale");
                Console.WriteLine("2. Return Product From Sale");
                Console.WriteLine("3. Delete Sale");
                Console.WriteLine("4. Restore Sale");
                Console.WriteLine("5. Display All Sales");
                Console.WriteLine("6. Search Sales For Date Interval");
                Console.WriteLine("7. Search Sales For Price");                
                Console.WriteLine("8. Search Sales For Date");
                Console.WriteLine("9. Search Sale and Sale Items For ID");
                Console.WriteLine("0. Go Back");

                Console.Write("Please Select a Command : ");

                string selectionStr = Console.ReadLine();

                while (!int.TryParse(selectionStr, out selection))
                {
                    Console.WriteLine("Command was entered incorrectly. Please try again");
                    selectionStr = Console.ReadLine();
                }

                switch (selection)
                {
                    case 1:
                        MenuServices.AddSaleMenu();
                        break;
                    case 2:
                        MenuServices.ReturnProductFromSaleMenu();
                        break;
                    case 3:
                        MenuServices.DeleteSaleMenu();
                        break;
                    case 4:
                        MenuServices.RestoreSaleMenu();
                        break;
                    case 5:
                        MenuServices.DisplaySaleList();
                        break;
                    case 6:
                        MenuServices.SearchSalesForDateInterval();
                        break;
                    case 7:
                        MenuServices.SearchSalesForPriceMenu();
                        break;
                    case 8:
                        MenuServices.SearchSalesForDate();
                        break;
                    case 9:
                        MenuServices.DisplaySaleİtemsMenu();
                        break;

                    case 0:
                       
                        break;
                    default:
                        Console.WriteLine("You have selected the wrong command");
                        break;
                }
            } while (selection != 0);
        }
    }
}
