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

       public void AddProduct(string name, double price, Categories category,int quantity)
        {

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Məhsul adı boşdur");
            //if (products.Exists(i=>i.Name == name))
            //    throw new DuplicateWaitObjectException("name", "Bu məhsul artıq bazada var");
            //mehsulun artiq olmasini yoxlayir
           
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", "Məhsulun qiyməti yanlış daxil edilib");
            if (!categoryList.Contains(category))
                throw new ArgumentException("Məhsulun Kateqoriyası yanlış daxil edilib");
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("price", "Məhsulun sayı 0-dan böyük olmalıdır!");

            Product product = new();
            product.Name = name;
            product.Price = price;
            product.Category = category;
            product.Quantity = quantity;
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

        public IEnumerable<Product> SearchProduct(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("Daxil edilən mətn boşdur!");

            var updatedProdects = products.Where(i=> i.Name.Contains(text));
            //if (string.IsNullOrEmpty(text))
            //    throw new ArgumentNullException("");

           
            return updatedProdects;
        }

        public void EditProduct(int productNo,string name,Categories category,double price)
        {
            if (productNo == 0)
                throw new ArgumentNullException("productNo", "Məhsulun nömrəsi yanlış daxil edilib");
            if (!products.Exists(i => i.ID == productNo))
                throw new KeyNotFoundException("Məhsul Tapılmadı");
            

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Məhsul adı boşdur");
            if (!categoryList.Contains(category))
                throw new ArgumentException("Məhsulun Kateqoriyası yanlış daxil edilib");
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", "Məhsulun qiyməti yanlış daxil edilib");

           
            foreach (var product in products)
            {
                if (product.ID == productNo)
                {
                    //product.Quantity = product.Quantity - 1;// evvelkinin saylini asaltmaq
                    product.Name = name;
                    product.Category = category;
                    product.Price = price;

                    //product.Quantity = products.Where(i => i.Name == product.Name).Count();
                }
                product.Quantity = products.Where(i => i.Name == product.Name).Count();
            }
            
        }

        public IEnumerable<Product> SearchProductForPrice(double min,double max)
        {
            
            if (min <= 0)
                throw new ArgumentOutOfRangeException("min", "Minimum Dəyər 0-dan böyük olmalıdır");
            if (max <= 0)
                throw new ArgumentOutOfRangeException("max", "Maksimum Dəyər 0-dan böyük olmalıdır");

            var searchedProducts = products.Where(i=>i.Price < max && i.Price > min);
            //if (string.IsNullOrEmpty(text))
            //    throw new ArgumentNullException("");


            return searchedProducts;
        }

        public IEnumerable<Product> SearchProductForCategory(Categories category)
        {

            if (!categoryList.Contains(category))
                throw new ArgumentException("Məhsulun Kateqoriyası yanlış daxil edilib");

            var searchedProducts = products.Where(i => i.Category == category);
            //if (string.IsNullOrEmpty(text))
            //    throw new ArgumentNullException("");


            return searchedProducts;
        }
    }


}
