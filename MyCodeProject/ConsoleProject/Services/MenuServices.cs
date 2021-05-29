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

            var table = new ConsoleTable("ID", "Name", "Category", "Price (AZN)", "Quantity");
            foreach (var product in operations.Products)
            { 
                if (product.IsDeleted == false)
                    table.AddRow(product.ID, product.Name, product.Category, product.Price.ToString("0.00"), product.Quantity);

            }
            table.Write();          
        }
        public static void DisplaySaleList()
        {

            var table = new ConsoleTable("ID", "Total Price", "Product Quantity", "Sell by");
            foreach (var sale in operations.Sales)
            {
                if (sale.IsDeleted == false)
                    table.AddRow(sale.ID, sale.Price.ToString("0.00"), sale.SaleItems.Sum(i=>i.Quantity), sale.Date);
            }
            table.Write();
            //Console.WriteLine();
        }

        public static void AddProductMenu()             
        {          
            Console.Write("Please enter product's name : ");
            string name = Console.ReadLine();

            Console.Write("Please enter product's category : ");
            string category = Console.ReadLine();

            Console.Write("Please enter product's quantity : ");
            int.TryParse(Console.ReadLine(), out int quantity);

            Console.Write("Please enter product's price (x.xx): ");
            double.TryParse(Console.ReadLine(),out double price);

            try
            {            
                operations.AddProduct(name, price, category, quantity);
                Console.WriteLine("Product has been Inserted");
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
            catch (KeyNotFoundException ex)
            {           
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("An unexpected error occurred!");
            }

        }
      
        public static void DeleteProductMenu()
        {
            Console.Write("Please enter the ID of the product you want to delete : ");
            int.TryParse(Console.ReadLine(), out int index);
            
            try
            {
                operations.DeleteProduct(index);
                Console.WriteLine("Product has been deleted");
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
        public static void ReturnProductMenu()
        {
            Console.Write("Please enter the ID of the product you want to restore : ");
            int.TryParse(Console.ReadLine(), out int index);

            try
            {
                operations.ReturnProduct(index);
                Console.WriteLine("Product has been restored");
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
            Console.Write("Please enter product's name : ");
            string search = Console.ReadLine();

            try
            {
               var searchedProducts = operations.SearchProduct(search);
                var table = new ConsoleTable("ID", "Name", "Category", "Price (AZN)", "Quantity");
                foreach (var searchedProduct in searchedProducts)
                {
                    if (searchedProduct.IsDeleted == false)
                        table.AddRow(searchedProduct.ID, searchedProduct.Name, searchedProduct.Category, searchedProduct.Price.ToString("0.00"), searchedProduct.Quantity);
                }
                table.Write();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
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
            //Product data = new(); // instance yaradanda ID artdigi ucun listden bir product goturdum
            Product data = operations.Products.FirstOrDefault();
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
                    case 1:
                        {
                            Console.Write("Product's name : ");
                            string name = Console.ReadLine();
                            data.Name = name;
                            break;
                        }
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
                    case 3:
                        {
                            Console.Write("Product's price (x.xx) : ");
                            if (double.TryParse(Console.ReadLine(), out double price))
                                data.Price = price;
                            else
                                Console.WriteLine("The price was entered incorrectly");
                            break;
                        }
                    case 4:
                        {
                            Console.Write("Product's quantity : ");
                            if (int.TryParse(Console.ReadLine(), out int quantity))
                                data.Quantity = quantity;
                            else
                                Console.WriteLine("The quantity was entered incorrectly");
                            break;
                        }                   
                    default:
                        Console.WriteLine("You pressed the wrong button");
                        break;
                }
            } while (selection != 0);          

        }
        public static void SearchProductMenuForPrice()
        {
            Console.Write("Please enter a minimum price (x.xx) : ");
            double.TryParse(Console.ReadLine(), out double minimum);

            Console.Write("Please enter a maximum price (x.xx) : ");
            double.TryParse(Console.ReadLine(), out double maximum);

            try
            {
                var searchedProducts = operations.SearchProductForPrice(minimum,maximum);
                var table = new ConsoleTable("ID", "Name", "Category", "Price (AZN)", "Quantity");
                foreach (var searchedProduct in searchedProducts)
                {
                    if (searchedProduct.IsDeleted == false)
                        table.AddRow(searchedProduct.ID, searchedProduct.Name, searchedProduct.Category, searchedProduct.Price.ToString("0.00"), searchedProduct.Quantity);
                }
                table.Write();
                //Console.WriteLine();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void SearchProductMenuForCategory()
        {
            Console.Write("Please enter category for search : ");
            string category = Console.ReadLine();                     

            try
            {
                var searchedProducts = operations.SearchProductForCategory(category);
                var table = new ConsoleTable("ID", "Name", "Category", "Price (AZN)", "Quantity");
                foreach (var searchedProduct in searchedProducts)
                {
                    if (searchedProduct.IsDeleted == false)
                        table.AddRow(searchedProduct.ID, searchedProduct.Name, searchedProduct.Category, searchedProduct.Price.ToString("0.00"), searchedProduct.Quantity);
                }
                table.Write();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region Add Sale Menu

        public static void AddSaleMenu()
        {          
           string selection = string.Empty;
            Dictionary<int, int> data = new();
            do
            {             
                Console.Write("Please enter the product ID you want to sell : ");
                int.TryParse(Console.ReadLine(), out int code);

                Console.Write("How many products do you want to add? : ");
                int.TryParse(Console.ReadLine(), out int quantity);

                data.Add(code,quantity);
                Console.WriteLine("Tap 1 to add another new product, otherwise anywhere else : ");
                selection = Console.ReadLine();

            } while (selection == "1");

            try
            {
                operations.AddSale(data);
                Console.WriteLine("Sale has been Inserted");
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
                Console.WriteLine("An unexpected error occurred!");
            }
        }
        #endregion

        public static void DeleteSaleMenu()
        {
            Console.WriteLine("Please enter the ID of the sale you want to delete : ");
            int.TryParse(Console.ReadLine(), out int saleId);

            try
            {
                operations.DeleteSale(saleId);
                Console.WriteLine("Sale has been deleted");
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
                Console.WriteLine("An unexpected error occurred!");
            }
        }
        public static void RestoreSaleMenu()
        {
            Console.WriteLine("Please enter the ID of the sale you want to restore : ");
            int.TryParse(Console.ReadLine(), out int saleId);
            try
            {
                operations.RestoreSale(saleId);
                Console.WriteLine("Sale has been restored");
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
                Console.WriteLine("An unexpected error occurred!");
            }
        }
        public static void SearchSalesForPriceMenu()
        {
            Console.Write("Please enter a minimum price for search (x.xx) : ");
            double.TryParse(Console.ReadLine(), out double minimum);

            Console.Write("Please enter a maximum price for search (x.xx) : ");
            double.TryParse(Console.ReadLine(), out double maximum);

            try
            {
                var searchedSales = operations.SearchSalesForPrice(minimum, maximum);
                var searchTable = new ConsoleTable("ID", "Total Price", "Product Quantity", "Sell by");
                foreach (var searchedSale in searchedSales)
                {
                    searchTable.AddRow(searchedSale.ID, searchedSale.Price.ToString("0.00"), searchedSale.SaleItems.Count(), searchedSale.Date);
                }
                searchTable.Write();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       public static void SearchSalesForDateInterval()
        {
            Console.Write("Please enter a start date for search (mm/dd/yyyy) : ");
            DateTime.TryParse(Console.ReadLine(), out DateTime minimum);

            Console.Write("Please enter a last date for search (mm/dd/yyyy) : ");
            DateTime.TryParse(Console.ReadLine(), out DateTime maximum);

            try
            {
               var searchedSales = operations.SearchSalesForDateInterval(minimum,maximum);
                var searchTable = new ConsoleTable("ID", "Total Price", "Product Quantity", "Sell by");
                foreach (var searchedSale in searchedSales)
                {
                    searchTable.AddRow(searchedSale.ID, searchedSale.Price.ToString("0.00"), searchedSale.SaleItems.Count(), searchedSale.Date);
                }
                searchTable.Write();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()} - {ex.Message}");
                //Console.WriteLine("Gözlənilməz bir xəta baş verdi");
            }

        }

        public static void DisplaySaleİtemsMenu()
        {
            Console.WriteLine("Please enter sale's ID : ");
            int.TryParse(Console.ReadLine(),out int saleId);

            try
            {
                Sale searchedSale = operations.DisplaySaleİtems(saleId);
                var saleTable = new ConsoleTable("ID", "Total Price", "Product Quantity", "Sell by");
                saleTable.AddRow(searchedSale.ID, searchedSale.Price.ToString("0.00"), searchedSale.SaleItems.Count(), searchedSale.Date);
                saleTable.Write();
                
                var saleItemsTable = new ConsoleTable("ID", "Product Name", "Product Quantity");
                foreach (var SaleItem in searchedSale.SaleItems)
                {
                    saleItemsTable.AddRow(SaleItem.ID, SaleItem.Product.Name, SaleItem.Quantity);
                }
                saleItemsTable.Write();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("An unexpected error occurred!");
            }
        }

        public static void SearchSalesForDate()
        {
            Console.Write("Please enter a date for search (mm/dd/yyyy) : ");
            DateTime.TryParse(Console.ReadLine(), out DateTime minimum);           

            try
            {
                var searchedSales = operations.SearchSalesForDate(minimum);
                var searchTable = new ConsoleTable("ID", "Total Price", "Product Quantity", "Sell by");
                foreach (var searchedSale in searchedSales)
                {
                    searchTable.AddRow(searchedSale.ID, searchedSale.Price.ToString("0.00"), searchedSale.SaleItems.Count(), searchedSale.Date);
                }
                searchTable.Write();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()} - {ex.Message}");
                //Console.WriteLine("Gözlənilməz bir xəta baş verdi");
            }
        }

        public static void ReturnProductFromSaleMenu()
        {
            Console.WriteLine("Which product do you want to return:");
            string name = Console.ReadLine();

            Console.WriteLine("Which will be deleted from sale? : ");
            int.TryParse(Console.ReadLine(),out int saleId);

            Console.WriteLine("How many products do you want to return : ");
            int.TryParse(Console.ReadLine(), out int quantity);

            try
            {
                operations.ReturnProductFromSale(name,saleId, quantity);
                Console.WriteLine("the product has been returned");
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
            catch (Exception)
            {
                Console.WriteLine("An unexpected error occurred!");
            }
           
        }
    }
}
