using ConsoleTables;
using System;
using ConsoleProject.Data.Enums;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
            if (operations.products.Exists(i => i.Name == name))
            {
                Console.WriteLine("Bu məhsul artıq bazada var");
                return;
            }
            Console.Write("Məhsulun qiymətini daxil edin : ");
            //Əgər yanlış daxil edilsə price = 0 olacaq və ArgumentOutOfRangeException yaranacaq
            double.TryParse(Console.ReadLine(), out double price);

            Console.Write("Məhsulun kateqoriyasını daxil edin : ");
            string category = Console.ReadLine();

            Console.Write("Məhsulun sayını daxil edin : ");
            int.TryParse(Console.ReadLine(), out int quantity);

            try
            {            
                operations.AddProduct(name, price, Enum.Parse<Categories>(category), quantity);
                Console.WriteLine("Product Inserted");
            }

            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DuplicateWaitObjectException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {             
             //Console.ForegroundColor =  ConsoleColor.Red;
                Console.WriteLine("Məhsulun Kateqoriyası yanlış daxil edilib");
               
                //Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gözlənilməz bir xəta baş verdi");
                //Console.WriteLine($"Message : {ex.Message} \n Type : {ex.GetType()}");
            }

        }
      
        public static void DeleteProductMenu()
        {
            Console.Write("Silinəcək məhsulun nömrəsini daxil edin : ");
            int.TryParse(Console.ReadLine(), out int index);
            
            try
            {
                operations.DeleteProduct(index);
                Console.WriteLine("Məhsul silindi");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        public static void SearchProductMenu()
        {
            Console.Write("Məhsulun adını daxil edin : ");
            string search = Console.ReadLine();

            try
            {
               var searchedProducts = operations.SearchProduct(search);
                var x = new ConsoleTable("Nömrəsi", "Adı", "Kateqoriyası", "Qiyməti (AZN)", "Sayı");
                foreach (var searchedProduct in searchedProducts)
                {
                    x.AddRow(searchedProduct.ID, searchedProduct.Name, searchedProduct.Category, searchedProduct.Price, searchedProduct.Quantity);
                }
                x.Write();
                //Console.WriteLine();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void EditProductMenu()
        {
            Console.Write("Düzəliş ediləcək məhsulun nömrəsini daxil edin : ");
            int.TryParse(Console.ReadLine(), out int index);
            if (!operations.products.Exists(i => i.ID == index))
            {
                Console.WriteLine("Nömrə Yanlışdır");
                return;
            }
          
            Console.Write("Məhsulun adını : ");
            string name = Console.ReadLine();          

            Console.Write("Məhsulun kateqoriyası : ");
            string category = Console.ReadLine();

            Console.Write("Məhsulun qiyməti : ");
            //Əgər yanlış daxil edilsə price = 0 olacaq və ArgumentOutOfRangeException yaranacaq
            double.TryParse(Console.ReadLine(), out double price);

            try
            {
                operations.EditProduct(index,name, Enum.Parse<Categories>(category),price);
                Console.WriteLine("Məhsul yeniləndi");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Məhsulun Kateqoriyası yanlış daxil edilib");//duselt bu hissəni exception hissəsini
            }
            
            catch (Exception)
            {
                Console.WriteLine("Xeta oldu");
            }
        }

        public static void SearchProductMenuForPrice()
        {
            Console.Write("Minimum dəyər daxil edin : ");
            double.TryParse(Console.ReadLine(), out double minimum);

            Console.Write("Maksimum dəyər daxil edin : ");
            double.TryParse(Console.ReadLine(), out double maximum);

            try
            {
                var searchedProducts = operations.SearchProductForPrice(minimum,maximum);
                var x = new ConsoleTable("Nömrəsi", "Adı", "Kateqoriyası", "Qiyməti (AZN)", "Sayı");
                foreach (var searchedProduct in searchedProducts)
                {
                    x.AddRow(searchedProduct.ID, searchedProduct.Name, searchedProduct.Category, searchedProduct.Price, searchedProduct.Quantity);
                }
                x.Write();
                //Console.WriteLine();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SearchProductMenuForCategory()
        {
            Console.Write("Kateqoriya daxil edin : ");
            string category = Console.ReadLine();                     

            try
            {
                var searchedProducts = operations.SearchProductForCategory(Enum.Parse<Categories>(category));
                var x = new ConsoleTable("Nömrəsi", "Adı", "Kateqoriyası", "Qiyməti (AZN)", "Sayı");
                foreach (var searchedProduct in searchedProducts)
                {
                    x.AddRow(searchedProduct.ID, searchedProduct.Name, searchedProduct.Category, searchedProduct.Price, searchedProduct.Quantity);
                }
                x.Write();
                //Console.WriteLine();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
