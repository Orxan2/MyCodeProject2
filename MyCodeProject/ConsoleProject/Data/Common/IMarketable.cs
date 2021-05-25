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
                  
        public void AddProduct(string name, double price, Categories category, int quantity);
        public void DeleteProduct(int productNo);
        public IEnumerable<Product> SearchProduct(string text);
        public void EditProduct(int productNo, Product data);
        public IEnumerable<Product> SearchProductForPrice(double min, double max);
        public IEnumerable<Product> SearchProductForCategory(Categories category);


        public void AddSale(List<Product> saledProducts);
        //public void TakeFromSale();
        //public void DeleteSale();
        //public void SearchSaleForDateInterval();
        //public void SearchSaleForPrice();
        //public void SearchSaleForDate();
        //public void SearchSaleForID();


    }
}
