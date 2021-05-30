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
       // Bu Class-da MarketServis-dəki Metodları Çağırıb,onlara parametrlər yollayıb,try..catch strukturu ilə xətaları idarə edirik.
        
        static MarketServices operations = new();

        #region Call Display Operations 

        // Məhsullar Siyahısını Göstərəcək
        public static void DisplayProductList()
        {
            try
            {
                DisplayProducts(operations.Products);
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        // Satışlar Siyahısını Göstərəcək
        public static void DisplaySaleList()
        {
            try
            {
                DisplaySales(operations.Sales);
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
            
        }

        #endregion

        #region Call Add Operations

        //AddProduct Metodunu Çağırır
        public static void AddProductMenu()
        {
            Console.Write("Please enter product's name : ");
            string name = Console.ReadLine();

            Console.Write("Please enter product's category : ");
            string category = Console.ReadLine();

            Console.Write("Please enter product's quantity : ");
            int.TryParse(Console.ReadLine(), out int quantity);

            Console.Write("Please enter product's price (x.xx): ");
            double.TryParse(Console.ReadLine(), out double price);

            try
            {
                operations.AddProduct(name,price,category,quantity);
                Console.WriteLine("Product has been Inserted");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"\n {ex.Message}"); 
            }
            catch (DuplicateWaitObjectException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }
       
        //AddSale Metodunu Çağırır
        public static void AddSaleMenu()
        {
            string selection = string.Empty;
            Dictionary<int, int> datas = new();

            //İstifadəçinin istədiyi qədər məhsulu satışa əlavə edə bilməsi üçün dövr qurulur.
            do
            {
                Console.Write("Please enter the product ID you want to sell : ");
                int.TryParse(Console.ReadLine(), out int code);

                Console.Write("How many products do you want to add? : ");
                int.TryParse(Console.ReadLine(), out int quantity);

                datas.Add(code, quantity);
                Console.WriteLine("Tap 1 to add another new product, otherwise anywhere else : ");
                selection = Console.ReadLine();

            } while (selection == "1");

            try
            {
                operations.AddSale(datas);
                Console.WriteLine("Sale has been Inserted");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }

            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        #endregion

        #region Call Remove Operaions

        //DeleteProduct Metodunu Çağırır
        public static void DeleteProductMenu()
        {
            Console.Write("Please enter the ID of the product you want to delete : ");
            int.TryParse(Console.ReadLine(), out int index);

            try
            {
                operations.DeleteProduct(index);
                Console.WriteLine("Product has been deleted");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }

        }

        //DeleteSale Metodunu Çağırır
        public static void DeleteSaleMenu()
        {
            Console.WriteLine("Please enter the ID of the sale you want to delete : ");
            int.TryParse(Console.ReadLine(), out int saleId);

            try
            {
                operations.DeleteSale(saleId);
                Console.WriteLine("Sale has been deleted");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        #endregion

        #region Call Search Operations

        // Bu metod SearchProductForName metodunu çağırır və axtarışın nəticəsini göstərir.
        public static void SearchProductMenuForName()
        {
            Console.Write("Please enter product's name : ");
            string search = Console.ReadLine();

            try
            {
                var searchedProducts = operations.SearchProductForName(search);
                DisplayProducts(searchedProducts.ToList<Product>());
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        // Bu metod SearchSalesForPrice metodunu çağırır və axtarışın nəticəsini göstərir.
        public static void SearchSalesMenuForPrice()
        {
            Console.Write("Please enter a minimum price for search (x.xx) : ");
            double.TryParse(Console.ReadLine(), out double minimum);

            Console.Write("Please enter a maximum price for search (x.xx) : ");
            double.TryParse(Console.ReadLine(), out double maximum);

            try
            {
                var searchedSales = operations.SearchSalesForPrice(minimum, maximum);
                DisplaySales(searchedSales.ToList<Sale>());               
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        // Bu metod SearchSalesForDateInterval metodunu çağırır və axtarışın nəticəsini göstərir.
        public static void SearchSalesMenuForDateInterval()
        {
            Console.Write("Please enter a start date for search (mm/dd/yyyy) : ");
            DateTime.TryParse(Console.ReadLine(), out DateTime minimum);

            Console.Write("Please enter a last date for search (mm/dd/yyyy) : ");
            DateTime.TryParse(Console.ReadLine(), out DateTime maximum);

            try
            {
                var searchedSales = operations.SearchSalesForDateInterval(minimum, maximum);
                DisplaySales(searchedSales.ToList<Sale>());               
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }

        }

        // Bu metod SearchSaleForID metodunu çağırır və axtarışın nəticəsini göstərir.
        public static void SearchSaleMenuForID()
        {
            Console.WriteLine("Please enter sale's ID : ");
            int.TryParse(Console.ReadLine(), out int saleId);

            try
            {
                // İD-ə görə Satışı tapıb göstərir
                Sale searchedSale = operations.SearchSaleForID(saleId);
                var saleTable = new ConsoleTable("ID", "Total Price", "Product Quantity", "Sell by");
                saleTable.AddRow(searchedSale.ID, searchedSale.Price.ToString("0.00"), searchedSale.SaleItems.Sum(i => i.Quantity), searchedSale.Date);
                saleTable.Write();
            
                //hər saleitem başqa bir cədvəldə göstərilir.
                var saleItemsTable = new ConsoleTable("ID", "Product Name", "Product Quantity");
                foreach (var SaleItem in searchedSale.SaleItems)
                {
                    saleItemsTable.AddRow(SaleItem.ID, SaleItem.Product.Name, SaleItem.Quantity);
                }
                saleItemsTable.Write();
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        // Bu metod SearchSalesForDate metodunu çağırır və axtarışın nəticəsini göstərir.
        public static void SearchSalesMenuForDate()
        {
            Console.Write("Please enter a date for search (mm/dd/yyyy) : ");
            DateTime.TryParse(Console.ReadLine(), out DateTime minimum);

            try
            {
                var searchedSales = operations.SearchSalesForDate(minimum);
                DisplaySales(searchedSales.ToList<Sale>());               
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        // Bu metod SearchProductForPrice metodunu çağırır və axtarışın nəticəsini göstərir.
        public static void SearchProductMenuForPrice()
        {
            Console.Write("Please enter a minimum price (x.xx) : ");
            double.TryParse(Console.ReadLine(), out double minimum);

            Console.Write("Please enter a maximum price (x.xx) : ");
            double.TryParse(Console.ReadLine(), out double maximum);

            try
            {
                var searchedProducts = operations.SearchProductForPrice(minimum, maximum);
                DisplayProducts(searchedProducts.ToList<Product>());                
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        // Bu metod SearchProductForCategory metodunu çağırır və axtarışın nəticəsini göstərir.
        public static void SearchProductMenuForCategory()
        {
            Console.Write("Please enter category for search : ");
            string category = Console.ReadLine();

            try
            {
                var searchedProducts = operations.SearchProductForCategory(category);
                DisplayProducts(searchedProducts.ToList<Product>());                
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        #endregion

        #region Call Edit Operations

        //Bu metodda switch..case ilə istifadəçinin seçim ilə edit edə bilməsini,do..while ilə də əməliyyatı təkrarlaya
        //bilməsini təmin etdim     
        public static void EditProductMenu()
        {            
            Console.Write("Please enter the product ID you want to edit : ");
            int.TryParse(Console.ReadLine(), out int index);
            if (!operations.Products.Exists(i => i.ID == index))
            {
                Console.WriteLine("The ID was entered incorrectly");
                return;
            }
            int selection = 0;
            Product data = new();
            //Product data = operations.Products.FirstOrDefault();

            do
            {
                Console.WriteLine("1.Name 2.Category 3.Price(x.xx) 4.Quantity 0.I don't want to change anymore");
                Console.Write("What do you want to change? : ");

                string selectionStr = Console.ReadLine();
                while (!int.TryParse(selectionStr, out selection))
                {
                    Console.WriteLine("The choice is incorrect");
                    selectionStr = Console.ReadLine();
                }

                switch (selection)
                {
                    // Məhsul adı daxil edilir və yoxlanılır
                    case 1:
                        {
                            Console.Write("Product's name : ");
                            string name = Console.ReadLine();
                            if (string.IsNullOrEmpty(name))
                                Console.WriteLine("The product's name is empty");
                            else
                                data.Name = name;

                            break;
                        }
                    // Məhsul Kateqoriyası daxil edilir və yoxlanılır
                    case 2:
                        {
                            Console.Write("Product's category : ");
                            string category = Console.ReadLine();
                            if (operations.categoryList.Exists(i => i.ToString() == category))
                                data.Category = Enum.Parse<Categories>(category);
                            else
                                Console.WriteLine("The category was entered incorrectly");
                            break;
                        }
                    // Məhsul Qiyməti daxil edilir və yoxlanılır
                    case 3:
                        {
                            Console.Write("Product's price (x.xx) : ");
                            double.TryParse(Console.ReadLine(), out double price);
                            if (price <= 0)
                                Console.WriteLine("The price of the product was entered incorrectly");
                            else
                                data.Price = price;

                            break;
                        }
                    // Məhsul Sayı daxil edilir və yoxlanılır
                    case 4:
                        {
                            Console.Write("Product's quantity : ");
                            int.TryParse(Console.ReadLine(), out int quantity);
                            if (quantity <= 0)
                                Console.WriteLine("The quantity of the product was entered incorrectly");
                            else
                                data.Quantity = quantity;                           

                            break;
                        }
                    default:
                        Console.WriteLine("You pressed the wrong button");
                        break;
                }
            } while (selection != 0);

            operations.EditProduct(index, data);

        }

        // Bu metod RestoreProduct metodunu çağırır
        public static void RestoreProductMenu()
        {
            Console.Write("Please enter the ID of the product you want to restore : ");
            int.TryParse(Console.ReadLine(), out int index);

            try
            {
                operations.RestoreProduct(index);
                Console.WriteLine("Product has been restored");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }

        }

        // Bu metod RestoreSale metodunu çağırır
        public static void RestoreSaleMenu()
        {
            Console.WriteLine("Please enter the ID of the sale you want to restore : ");
            int.TryParse(Console.ReadLine(), out int saleId);
            try
            {
                operations.RestoreSale(saleId);
                Console.WriteLine("Sale has been restored");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }
        }

        // Bu metod ReturnProductFromSale metodunu çağırır
        public static void ReturnProductFromSaleMenu()
        {
            Console.WriteLine("Which product do you want to return:");
            string name = Console.ReadLine();

            Console.WriteLine("Which will be deleted from sale? : ");
            int.TryParse(Console.ReadLine(), out int saleId);

            Console.WriteLine("How many products do you want to return : ");
            int.TryParse(Console.ReadLine(), out int quantity);

            try
            {
                operations.ReturnProductFromSale(name, saleId, quantity);
                Console.WriteLine("the product has been returned");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred!");
            }

        }

        #endregion



        #region ConsoleTable Methods
        private static void DisplayProducts(List<Product> products)
        {
            var table = new ConsoleTable("ID", "Name", "Category", "Price (AZN)", "Quantity");
            foreach (var product in products)
            {
                if (product.IsDeleted == false) //Sadəcə silinməmiş məhsulları göstərəcək
                    table.AddRow(product.ID, product.Name, product.Category, product.Price.ToString("0.00"), product.Quantity);
            }
            table.Write();
        }
        private static void DisplaySales(List<Sale> sales)
        {
            var table = new ConsoleTable("ID", "Total Price", "Product Quantity", "Sell by");
            foreach (var sale in sales)
            {
                if (sale.IsDeleted == false)//Sadəcə silinməmiş satışları göstərəcək
                    table.AddRow(sale.ID, sale.Price.ToString("0.00"), sale.SaleItems.Sum(i => i.Quantity), sale.Date);
            }
            table.Write();
        }
        #endregion

    }
}
