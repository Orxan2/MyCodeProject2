using ConsoleTables;
using System;
using ConsoleProject.Data.Enums;
using System.Collections.Generic;

namespace ConsoleProject.Services
{
    
   public static class MenuServices
    {
        
        static MarketServices operations = new();
        //static List<Categories> categories = new();

      
        //public MenuServices() => categories.AddRange(categories);

        public static void DisplayProductList()
        {
            var table = new ConsoleTable("Nömrəsi","Adı","Kateqoriyası", "Qiyməti (AZN)", "Sayı");
            foreach (var product in operations.products)
            {
                table.AddRow(product.ID,product.Name, product.Category, product.Price,product.Quantity);
            }
            table.Write();
            //Console.WriteLine();
        }

        public static void AddProductMenu()
        {
            Console.Write("Məhsulun adını daxil edin : ");
            string name = Console.ReadLine();

            Console.Write("Məhsulun qiymətini daxil edin : ");
            //Əgər yanlış daxil edilsə price = 0 olacaq və ArgumentOutOfRangeException yaranacaq
            double.TryParse(Console.ReadLine(), out double price);

            Console.Write("Məhsulun kateqoriyasını daxil edin : ");
            string category = Console.ReadLine();


            try
            {
                operations.AddProduct(name, price, Enum.Parse<Categories>(category));
                Console.WriteLine("Product Inserted");
            }

            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }           
            catch (Exception ex)
            {
                Console.WriteLine("Gözlənilməz bir xəta baş verdi");
            }
        }
    }
}
