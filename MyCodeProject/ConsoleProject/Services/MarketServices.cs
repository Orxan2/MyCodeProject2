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


        public void AddSale(List<Product> saledProducts)
        {

            Sale sale = new();
            
            foreach (var saledProduct in saledProducts)
            {

                if (saledProduct.ID<=0)
                    throw new ArgumentNullException("Məhsulun kodu düzgün daxil edilməyib");


                var product = Products.FirstOrDefault(i=>i.ID == saledProduct.ID);

                if (product == null)
                    throw new KeyNotFoundException("Məhsul Tapılmadı");

                if (saledProduct.Quantity > product.Quantity)
                    throw new KeyNotFoundException("Bazada Bu QƏdər Məhsul Yoxdur");
                SaleItem saleItem = new();
                saleItem.Product = product;
                saleItem.Quantity = saledProduct.Quantity;
                sale.Price += saleItem.Product.Price * saleItem.Quantity;               
                //Products.FirstOrDefault(i => i.ID == saledProduct.ID).Quantity -= saledProduct.Quantity;
                sale.SaleItems.Add(saleItem);
            }
            foreach (var saledProduct in saledProducts)//2-ci və ya sonrakı satış məhsullarını əlavə edəndə Exception olsa 1-i silməsin deyə təkrar yazdım
            {
                Products.FirstOrDefault(i => i.ID == saledProduct.ID).Quantity -= saledProduct.Quantity;
            }
            Sales.Add(sale);           
          
        }

        public void DeleteSale(int saleId)
        {
            if (saleId <= 0)
                throw new ArgumentNullException("saleId", "Satışın kodu düzgün daxil edilməyib");

            int index = Sales.FindIndex(i=>i.ID == saleId);

            if (index==-1)
                throw new KeyNotFoundException("Satış Tapılmadı");

            Sales.RemoveAt(index);
        }

        public IEnumerable<Sale> SearchSalesForPrice(double minValue,double maxValue)
        {
            if (minValue <= 0)
                throw new ArgumentOutOfRangeException("minValue", "Minimum Qiymət 0-dan böyük olmalıdır");
            if (maxValue <= 0)
                throw new ArgumentOutOfRangeException("maxValue", "Maksimum Qiymət 0-dan böyük olmalıdır");

            var searchedSales = Sales.Where(i=>i.Price<=maxValue && i.Price>=minValue);

            return searchedSales;

        }

        public IEnumerable<Sale> SearchSalesForDateInterval(DateTime startDate,DateTime lastDate)
        {
            if (startDate.Year == 1)
                throw new ArgumentNullException("startDate", "Başlanğıc tarixi yanlış daxil edilib");

            if (lastDate.Year == 1)
                throw new ArgumentNullException("lastDate", "Bitiş tarixi yanlış daxil edilib");

            Console.WriteLine("FIRST DATE : {0} \n last Date : {1}",startDate,lastDate);
            var searhedSales = Sales.Where(i => i.Date >= startDate && i.Date <= lastDate);
            if (searhedSales.Count() == 0)
                throw new KeyNotFoundException("Bu Tarixlərdə Satış Olmayıb");

            return searhedSales;
        }
    }

}
