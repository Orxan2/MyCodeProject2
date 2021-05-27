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
                throw new ArgumentNullException("name", "The product's name is empty");
            //if (products.Exists(i=>i.Name == name))
            //    throw new DuplicateWaitObjectException("name", "Bu məhsul artıq bazada var");
            //mehsulun artiq olmasini yoxlayir
           
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", "The price of the product was entered incorrectly");
            if (!categoryList.Contains(category))
                throw new ArgumentException("The category of the product was entered incorrectly");
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("price", "The product's quantity must be greater than 0!");

            Product product = new();
            product.Name = name;
            product.Price = price;
            product.Category = category;
            product.Quantity = quantity;
            product.IsDeleted = false;
            Products.Add(product);
            
        }

       public void DeleteProduct(int productNo)
        {
            if (productNo == 0)
               throw new ArgumentNullException("productNo", "The product's ID was entered incorrectly");

            int index = Products.FindIndex(i=>i.ID == productNo);
            if (index==-1)
                throw new KeyNotFoundException("The product was not found");

            //Products.RemoveAt(index);
            foreach (var product in Products)
            {
                if (product.ID == productNo)
                    product.IsDeleted = true;
            }

        }
        public void ReturnProduct(int productNo)
        {
            if (productNo == 0)
                throw new ArgumentNullException("productNo", "The product's ID you want to return is incorrect");

            int index = Products.FindIndex(i => i.ID == productNo);
            if (index == -1)
                throw new KeyNotFoundException("The product was not found");

            //Products.RemoveAt(index);
            foreach (var product in Products)
            {
                if (product.ID == productNo)
                    product.IsDeleted = false;
            }

        }
        public IEnumerable<Product> SearchProduct(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("The entered text is empty!");

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
                throw new ArgumentOutOfRangeException("min", "Minimum price must be greater than 0");
            if (max <= 0)
                throw new ArgumentOutOfRangeException("max", "Maximum price must be greater than 0");

            var searchedProducts = Products.Where(i=>i.Price <= max && i.Price >= min);
            //if (string.IsNullOrEmpty(text))
            //    throw new ArgumentNullException("");


            return searchedProducts;
        }

        public IEnumerable<Product> SearchProductForCategory(Categories category)
        {

            if (!categoryList.Contains(category))
                throw new ArgumentException("The category of the product was entered incorrectly");

            var searchedProducts = Products.Where(i => i.Category == category);          

            return searchedProducts;
        }

        #region Bu Metodu Duzelt
        public void AddSale(List<Product> saledProducts)
        {
            Sale sale = new();

            foreach (Product saledProduct in saledProducts)
            {
                if (saledProduct.ID<=0)
                    throw new ArgumentNullException("The Product's ID was entered incorrectly");


                Product product = Products.FirstOrDefault(i=>i.ID == saledProduct.ID);

                if (product == null || product.IsDeleted == true)
                    throw new KeyNotFoundException("The product was not found");

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
        #endregion

        public void DeleteSale(int saleId)
        {
            if (saleId == 0)
                throw new ArgumentNullException("saleId", "Satisin kodu dogru daxil edilmeyib");

            if (Sales.Exists(i => i.ID != saleId))
                throw new KeyNotFoundException("daxil etdiyiniz koda uygun satis yoxdur");

            Sale sale = Sales.FirstOrDefault(i=>i.ID == saleId);

            //if (sale == null)
            //    throw new KeyNotFoundException("Satış Tapılmadı");

            foreach (var saleItem in sale.SaleItems)
            {
                Product product = (Products.FirstOrDefault(i => i.ID == saleItem.Product.ID));

                if (product.IsDeleted == true)
                    product.IsDeleted = false;

                product.Quantity += saleItem.Quantity;// silinmeyibse mehsul
            }


            Sales.Remove(sale);
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
            
            var searhedSales = Sales.Where(i => i.Date >= startDate && i.Date <= lastDate);
            if (searhedSales.Count() == 0)
                throw new KeyNotFoundException("Bu Tarixlərdə Satış Olmayıb");

            return searhedSales;
        }

        public Sale DisplaySaleİtems(int saleId)
        {
            if (saleId == 0)
                throw new ArgumentNullException("saleId","Satışın kodu yanlış daxil edilib");

            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId);

            if (sale == null)
                throw new ArgumentNullException("sale", "Axtarılan satış tapılmadı");

            return sale;


        }

        public IEnumerable<Sale> SearchSalesForDate(DateTime date)
        {
            if (date.Year == 1)
                throw new ArgumentNullException("date", "Tarix yanlış daxil edilib");

            var searhedSales = Sales.Where(i => i.Date.Day == date.Day);
            if (searhedSales.Count() == 0)
                throw new KeyNotFoundException("Bu Tarixdə Satış Olmayıb");

            return searhedSales;

        }

        public void ReturnProductFromSale(string name,int saleId,int quantity)
        {
            if (saleId == 0)
                throw new ArgumentNullException("saleId", "Satisin kodu dogru daxil edilmeyib");

            if (Sales.Exists(i => i.ID != saleId))
                throw new KeyNotFoundException("daxil etdiyiniz koda uygun satis yoxdur");

            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId);
          
            if (string.IsNullOrEmpty(name)) 
                throw new ArgumentNullException("name", "Məhsulun adı doğru daxil edilməyib");

            if (sale.SaleItems.Exists(i => i.Product.Name != name))
                throw new KeyNotFoundException("Qebzde satilmis mehsullar arasinda bu mehsul yoxdur");

            if (quantity==0)
                throw new ArgumentNullException("quantity", "Qebzdeki satilmis Məhsulun sayı doğru daxil edilməyib");     
            if(quantity > sale.SaleItems.Where(i => i.Product.Name == name).Sum(i => i.Quantity))

                throw new ArgumentOutOfRangeException("quantity", "Qebzdeki mehsul bu qeder cox deyil");

            //if (sale == null)
            //    throw new ArgumentException("Satış tapılmadı");

            foreach (var saleItem in sale.SaleItems)
            {
                if (saleItem.Product.Name == name)
                {
                    if (saleItem.Product.IsDeleted == true)
                        saleItem.Product.IsDeleted = false;

                    saleItem.Quantity -= quantity; 
                    saleItem.Product.Quantity += quantity;
                    sale.Price -= quantity * saleItem.Product.Price;
                  
                }
            }


            
        }

    }

}
