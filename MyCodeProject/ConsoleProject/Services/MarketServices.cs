using ConsoleProject.Data.Common;
using ConsoleProject.Data.Entities;
using ConsoleProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Services
{
    class MarketServices : IMarketable
    {
        public List<Product> Products { get; set; }
       public List<Sale> Sales { get; set; }

        List<Categories> categoryList = new();

        

        //public List<Product> Products { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MarketServices()
        {
            Products = new();
            Sales = new();
            
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
            Products.Add(product);
        }

       public void DeleteProduct(int productNo)
        {
            if (productNo == 0)
               throw new ArgumentNullException("productNo", "Məhsulun nömrəsi yanlış daxil edilb");

            int index = Products.FindIndex(i=>i.ID == productNo);
            if (index==-1)
                throw new KeyNotFoundException("Məhsul Tapılmadı");

            Products.RemoveAt(index);

        }

        public IEnumerable<Product> SearchProduct(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("Daxil edilən mətn boşdur!");

            var updatedProdects = Products.Where(i=> i.Name.Contains(text));
            //if (string.IsNullOrEmpty(text))
            //    throw new ArgumentNullException("");

           
            return updatedProdects;
        }

        public void EditProduct(int productNo, Product data)
        {
            foreach (var product in Products)
            {
                if (product.ID == productNo)
                {                    
                     if(!string.IsNullOrEmpty(data.Name))
                            product.Name = data.Name;

                    if (data.Category != null)
                            product.Category = data.Category;

                    if(data.Price != 0)
                            product.Price = data.Price;

                    if (data.Quantity != 0)
                            product.Quantity = data.Quantity;                  
                }               
            }            
        }

        public IEnumerable<Product> SearchProductForPrice(double min,double max)
        {
            
            if (min <= 0)
                throw new ArgumentOutOfRangeException("min", "Minimum Dəyər 0-dan böyük olmalıdır");
            if (max <= 0)
                throw new ArgumentOutOfRangeException("max", "Maksimum Dəyər 0-dan böyük olmalıdır");

            var searchedProducts = Products.Where(i=>i.Price <= max && i.Price >= min);
            //if (string.IsNullOrEmpty(text))
            //    throw new ArgumentNullException("");


            return searchedProducts;
        }

        public IEnumerable<Product> SearchProductForCategory(Categories category)
        {

            if (!categoryList.Contains(category))
                throw new ArgumentException("Məhsulun Kateqoriyası yanlış daxil edilib");

            var searchedProducts = Products.Where(i => i.Category == category);          

            return searchedProducts;
        }

       
    }

}
