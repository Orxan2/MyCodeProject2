using ConsoleProject.Data.Entities;
using ConsoleProject.Data.Enums;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Data.Common
{
    interface IMarketable
    {
        public List<Product> Products { get; set; }
        public List<Sale> Sales { get; set; }
                  
        public void AddProduct(string name, double price, string category, int quantity);
        public void DeleteProduct(int productNo);
        public IEnumerable<Product> SearchProductForName(string text);
        public void EditProduct(int productNo, Product data);
        public IEnumerable<Product> SearchProductForPrice(double min, double max);
        public IEnumerable<Product> SearchProductForCategory(string category);


        public void AddSale(Dictionary<int, int> datas);
        public void ReturnProductFromSale(string name, int saleId, int quantity);
        public void DeleteSale(int saleİd);
        public IEnumerable<Sale> SearchSalesForDateInterval(DateTime first,DateTime last);
        public IEnumerable<Sale> SearchSalesForPrice(double minValue, double maxValue);
        public IEnumerable<Sale> SearchSalesForDate(DateTime date);
        public Sale SearchSaleForID(int saleId);
        public void RestoreProduct(int productNo);
        public void RestoreSale(int saleId);

    }
}
