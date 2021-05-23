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
       List<Categories> categoryList = new();
       

        public MarketServices()
        {
            products = new();
            sales = new();
            categoryList.AddRange(Enum.GetValues<Categories>());
        }

       public void AddProduct(string name, double price, Categories category)
        {

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Məhsul adı boşdur");
                       
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", "Məhsulun qiyməti yanlış daxil edilib");
            if (!categoryList.Contains(category))
                throw new ArgumentException("Məhsulun Kateqoriyası yanlış daxil edilib");
           
            Product product = new();
            product.Name = name;
            product.Price = price;
            product.Category = category;
            product.Quantity = products.Where(i=>i.Name==product.Name).Count()+1;

            products.Add(product);
        }

       public void DeleteProduct(int productNo)
        {
            if (productNo == 0)
               throw new ArgumentNullException("productNo", "Məhsulun nömrəsi yanlış daxil edilb");

            int index = products.FindIndex(i=>i.ID == productNo);
            if (index==-1)
                throw new KeyNotFoundException("Məhsul Tapılmadı");

            products.RemoveAt(index);

        }
    }
}
