using ConsoleProject.Data.Entities;
using ConsoleProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Services
{
    class MarketServices
    {
       public List<Product> products;
       public List<Sale> sales;

        public MarketServices()
        {
            products = new();
            sales = new();
        }

       public void AddProduct(string name, double price, Categories category)
        {

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Məhsul Adı Boşdur");
                       
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", "Məhsulun Qiyməti Yanlış daxil edilib");

            //if (category == null)
            //    throw new KeyNotFoundException();
            Product product = new();
            product.Name = name;
            product.Price = price;
            product.Category = category;
            product.Quantity = products.Where(i=>i.Name==product.Name).Count()+1;

            products.Add(product);
        }

    }
}
