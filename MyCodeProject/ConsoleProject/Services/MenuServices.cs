using ConsoleTables;
using System;
using ConsoleProject.Data.Enums;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ConsoleProject.Data.Entities;
using System.Text;
using System.Globalization;

namespace ConsoleProject.Services
{
    
   public static class MenuServices
    {
       
        static MarketServices operations = new();
        //static List<Categories> categories = new();


        //public MenuServices() => categories.AddRange(categories);

        public static void DisplayProductList()
        {

            var table = new ConsoleTable("Nömrəsi", "Adı", "Kateqoriyası", "Qiyməti (AZN)", "Sayı");
            foreach (var product in operations.Products)
            {
                table.AddRow(product.ID, product.Name, product.Category, product.Price, product.Quantity);
            }
            table.Write();          
        }
        public static void DisplaySaleList()
        {

            var table = new ConsoleTable("Nömrəsi", "Məbləği", "Məhsul Sayı", "Tarixi");
            foreach (var sale in operations.Sales)
            {
                table.AddRow(sale.ID, sale.Price, sale.SaleItems.Count(), sale.Date);
            }
            table.Write();
            //Console.WriteLine();
        }





        public static void AddProductMenu()             
        {          
            Console.Write("Məhsulun adını daxil edin : ");
            string name = Console.ReadLine();
            if (operations.Products.Exists(i => i.Name == name))
            {
                Console.WriteLine("Bu məhsul artıq bazada var");
                return;
            }
            

            Console.Write("Məhsulun kateqoriyasını daxil edin : ");
            string category = Console.ReadLine();

            Console.Write("Məhsulun sayını daxil edin : ");
            int.TryParse(Console.ReadLine(), out int quantity);

            Console.Write("Məhsulun qiymətini daxil edin : ");
            double price = double.Parse(Console.ReadLine());


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
            if (!operations.Products.Exists(i => i.ID == index))
            {
                Console.WriteLine("Nömrə Yanlışdır");
                return;
            }
            int selection = 0;
            Product data = new();
            do
            {
                Console.WriteLine("1.Ad 2.Kateqoriya 3.Qiymət 4.Say 0.Artıq Dəyişmək İstəmirəm");
                Console.Write("Nəyi Dəyişmək istəyirsiniz? : ");

                string selectionStr = Console.ReadLine();
                while (!int.TryParse(selectionStr, out selection))
                {
                    Console.WriteLine("Again");
                    selectionStr = Console.ReadLine();
                }

                switch (selection)
                {
                    case 1:
                        {
                            Console.Write("Məhsulun adını : ");
                            string name = Console.ReadLine();
                            data.Name = name;
                            break;
                        }
                    case 2:
                        {
                            Console.Write("Məhsulun kateqoriyası : ");

                            if (Enum.TryParse<Categories>(Console.ReadLine(), false, out Categories category))
                                data.Category = category;                            
                            else
                                Console.WriteLine("Kateqoriya Yanlış daxil edildi");

                            break;
                        }
                    case 3:
                        {
                            Console.Write("Məhsulun qiyməti : ");
                            if (double.TryParse(Console.ReadLine(), out double price))
                                data.Price = price;
                            else
                                Console.WriteLine("Qiymət yanlış daxil edildi");
                            break;
                        }
                    case 4:
                        {
                            Console.Write("Məhsulun sayı : ");
                            if (int.TryParse(Console.ReadLine(), out int quantity))
                                data.Quantity = quantity;
                            else
                                Console.WriteLine("Say yanlış daxil edildi");
                            break;
                        }                   
                    default:
                        Console.WriteLine("Yanlış düyməyə basdınız");
                        break;
                }
            } while (selection != 0);

            operations.EditProduct(index, data);
            Console.WriteLine("Məhsul yeniləndi");

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
            catch (ArgumentException ex)
            {
                //Console.WriteLine(ex.Message);
                Console.WriteLine("Məhsulun Kateqoriyası yanlış daxil edilib");
            }
        }


        public static void AddSaleMenu()
        {
            List<Product> saledProducts = new();
            string selection = string.Empty;
            
            do
            {
                Product saledProduct = new();               

                Console.Write("Satılan Məhsulun kodunu daxil edin : ");
                int.TryParse(Console.ReadLine(), out int code);

                Console.Write("Məhsulun sayını daxil edin : ");
                int.TryParse(Console.ReadLine(), out int quantity);

                saledProduct.ID = code;
                saledProduct.Quantity = quantity;
                saledProducts.Add(saledProduct);

                Console.WriteLine("Yeni Məhsul Əlavə etmək üçün 1-ə,əks təqdirdə başqa hərhansısa yere toxunun.");
                selection = Console.ReadLine();

            } while (selection == "1");
            

          
            try
            {
                operations.AddSale(saledProducts);
                Console.WriteLine("Sale Inserted");
            }                     
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }

            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }         
            catch (Exception)
            {
                Console.WriteLine("Gözlənilməz bir xəta baş verdi");
                //Console.WriteLine($"Message : {ex.Message} \n Type : {ex.GetType()}");
            }
        }

        public static void DeleteSaleMenu()
        {
            Console.WriteLine("Silinəcək Satışın kodunu daxil edin : ");
            //string saleIdStr = Console.ReadLine();
            int.TryParse(Console.ReadLine(), out int saleId);
            try
            {
                operations.DeleteSale(saleId);
                Console.WriteLine("Satış silindi");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("Gözlənilməz bir xəta baş verdi");
            }
        }

        public static void SearchSalesForPriceMenu()
        {
            Console.Write("Minimum dəyər daxil edin : ");
            double.TryParse(Console.ReadLine(), out double minimum);

            Console.Write("Maksimum dəyər daxil edin : ");
            double.TryParse(Console.ReadLine(), out double maximum);

            try
            {
                var searchedProducts = operations.SearchSalesForPrice(minimum, maximum);
                var x = new ConsoleTable("Nömrəsi", "Məbləği", "Məhsul Sayı", "Tarixi");
                foreach (var searchedProduct in searchedProducts)
                {
                    x.AddRow(searchedProduct.ID, searchedProduct.Price, searchedProduct.SaleItems.Count(), searchedProduct.Date);
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
