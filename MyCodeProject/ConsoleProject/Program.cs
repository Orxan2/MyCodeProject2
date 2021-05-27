using System;
using ConsoleProject.Services;
using System.Text;
using System.Globalization;
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
               
                Console.WriteLine("1. Work on Products");
                Console.WriteLine("2. Work on Sales");
                Console.WriteLine("0. Exit");              

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
                        SubMenu.ShowProductOperations();
                        break;
                    case 2:
                        SubMenu.ShowSaleOperations();
                        break;                  
                    case 0:
                        //Console.WriteLine("Exit");
                        break;
                    default:
                        Console.WriteLine("You have selected the wrong command");
                        break;
                }
            } while (selection != 0);

            Console.ReadKey();
        }
    }
}
