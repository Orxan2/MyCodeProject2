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

       public List<Categories> categoryList = new();

        

        //public List<Product> Products { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MarketServices()
        {
            Products = new();
            Sales = new();
            
            categoryList.AddRange(Enum.GetValues<Categories>());
        }

       public void AddProduct(string name, double price, string category,int quantity)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The product's name is empty");

            if (Products.Exists(i => i.Name == name))
                throw new DuplicateWaitObjectException("name", "Bu məhsul artıq bazada var");
                        
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", "The price of the product was entered incorrectly");

            if (!categoryList.Exists(i => i.ToString() == category))
                throw new ArgumentNullException("category", "The product's category was entered incorrectly");

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("price", "The product's quantity must be number and greater than 0!");

            Product product = new();
            product.Name = name;
            product.Price = price;
            product.Category = Enum.Parse<Categories>(category);
            product.Quantity = quantity;
            product.IsDeleted = false;
            Products.Add(product);
            
        }

       public void DeleteProduct(int productNo)
        {
            if (productNo == 0)
               throw new ArgumentNullException("productNo", "The product's ID was entered incorrectly");

            Product product = Products.FirstOrDefault(i=>i.ID == productNo);
            if (product == null || product.IsDeleted==true)
                throw new KeyNotFoundException("The product was not found or has already been deleted");

            product.IsDeleted = true;           
        }
        public void ReturnProduct(int productNo)
        {
            if (productNo == 0)
                throw new ArgumentNullException("productNo", "The product's ID was entered incorrectly");

            Product product = Products.FirstOrDefault(i => i.ID == productNo);

            if (product == null || product.IsDeleted == false)
                throw new KeyNotFoundException("The product was not found or hasn't been deleted");

            product.IsDeleted = false;
        }

        public IEnumerable<Product> SearchProduct(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("The entered text is empty!");

            var updatedProdects = Products.Where(i=> i.Name.Contains(text) && i.IsDeleted == false);
            //if (string.IsNullOrEmpty(text))
            //    throw new ArgumentNullException("");

           
            return updatedProdects;
        }

        public void EditProduct(int productNo, Product data)
        {
            foreach (var product in Products)
            {               
                if (product.ID == productNo && product.IsDeleted == false)
                {                    
                     if(!string.IsNullOrEmpty(data.Name))
                            product.Name = data.Name;

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
                throw new ArgumentOutOfRangeException("min", "Minimum price must be number and greater than 0");
            if (max <= 0)
                throw new ArgumentOutOfRangeException("max", "Maximum price must be number and greater than 0");

            var searchedProducts = Products.Where(i=>i.Price <= max && i.Price >= min && i.IsDeleted == false);
            //if (string.IsNullOrEmpty(text))
            //    throw new ArgumentNullException("");


            return searchedProducts;
        }

        public IEnumerable<Product> SearchProductForCategory(string category)
        {

            if (!categoryList.Exists(i=>i.ToString() == category))
                throw new ArgumentException("The category of the product was entered incorrectly");

            var searchedProducts = Products.Where(i => i.Category.ToString() == category && i.IsDeleted == false);          

            return searchedProducts;
        }

        #region Add Sale
        public void AddSale(Dictionary<int,int> datas)
        {              
            Sale sale = new();
            foreach (KeyValuePair<int,int> data in datas)
            {
                if (data.Key <= 0)
                    throw new ArgumentNullException("The Product's ID was entered incorrectly");

                Product product = Products.FirstOrDefault(i => i.ID == data.Key && i.IsDeleted == false);

                if (product == null)
                    throw new KeyNotFoundException("The product was not found");

                if (data.Value > product.Quantity)
                    throw new KeyNotFoundException("Bazada Bu QƏdər Məhsul Yoxdur");
                SaleItem saleItem = new();
                saleItem.Product = product;
                Console.WriteLine($"my Product's Price : {saleItem.Product.Price} \n My Product's Name : {saleItem.Product.Name} \n My Product's Isdeleted : {product.IsDeleted}");
                saleItem.Quantity = data.Value;

                sale.Price += saleItem.Product.Price * saleItem.Quantity;
                //Products.FirstOrDefault(i => i.ID == saledProduct.ID).Quantity -= saledProduct.Quantity;
                sale.SaleItems.Add(saleItem);
            }

            foreach (KeyValuePair<int, int> data in datas)//2-ci və ya sonrakı satış məhsullarını əlavə edəndə Exception olsa 1-i silməsin deyə təkrar yazdım
            {
                Products.FirstOrDefault(i => i.ID == data.Key).Quantity -= data.Value;
            }
            Sales.Add(sale);
        }
        #endregion

        public void DeleteSale(int saleId)
        {
            if (saleId == 0)
                throw new ArgumentNullException("saleId", "Satisin kodu dogru daxil edilmeyib");

            if (Sales.Exists(i => i.ID != saleId || i.IsDeleted == true))
                throw new KeyNotFoundException("daxil etdiyiniz koda uygun satis yoxdur ya da artiq silinib");

            Sale sale = Sales.FirstOrDefault(i=>i.ID == saleId);

            foreach (var saleItem in sale.SaleItems)
            {
                Product product = (Products.FirstOrDefault(i => i.ID == saleItem.Product.ID));

                if (product.IsDeleted == true)
                    product.IsDeleted = false;

                product.Quantity += saleItem.Quantity;
            }

            sale.IsDeleted = true;          
            
        }

        public void RestoreSale(int saleId)
        {
            if (saleId == 0)
                throw new ArgumentNullException("saleId", "Satisin kodu dogru daxil edilmeyib");

            if (Sales.Exists(i => i.ID != saleId || i.IsDeleted == false))
                throw new KeyNotFoundException("daxil etdiyiniz koda uygun satis yoxdur yada silinmeyib");

            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId);

            foreach (var saleItem in sale.SaleItems)
            {
                Product product = (Products.FirstOrDefault(i => i.ID == saleItem.Product.ID));

                if (product.IsDeleted == true)
                    product.IsDeleted = false;

                product.Quantity -= saleItem.Quantity; 
            }

            sale.IsDeleted = false;          
        }
        public IEnumerable<Sale> SearchSalesForPrice(double minValue,double maxValue)
        {
            if (minValue <= 0)
                throw new ArgumentOutOfRangeException("minValue", "Minimum Qiymət 0-dan böyük olmalıdır");
            if (maxValue <= 0)
                throw new ArgumentOutOfRangeException("maxValue", "Maksimum Qiymət 0-dan böyük olmalıdır");

            var searchedSales = Sales.Where(i=>i.Price<=maxValue && i.Price>=minValue && i.IsDeleted==false);

            return searchedSales;

        }

        public IEnumerable<Sale> SearchSalesForDateInterval(DateTime startDate,DateTime lastDate)
        {
            if (startDate.Year == 1)
                throw new ArgumentNullException("startDate", "Başlanğıc tarixi yanlış daxil edilib");

            if (lastDate.Year == 1)
                throw new ArgumentNullException("lastDate", "Bitiş tarixi yanlış daxil edilib");
            
            var searhedSales = Sales.Where(i => i.Date >= startDate && i.Date <= lastDate && i.IsDeleted == false);
            if (searhedSales.Count() == 0)
                throw new KeyNotFoundException("Bu Tarixlərdə Satış Olmayıb");

            return searhedSales;
        }

        public Sale DisplaySaleİtems(int saleId)
        {
            if (saleId == 0)
                throw new ArgumentNullException("saleId","Satışın kodu yanlış daxil edilib");

            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId && i.IsDeleted == false);

            if (sale == null)
                throw new ArgumentNullException("sale", "Axtarılan satış tapılmadı");

            return sale;


        }

        public IEnumerable<Sale> SearchSalesForDate(DateTime date)
        {
            if (date.Year == 1)
                throw new ArgumentNullException("date", "Tarix yanlış daxil edilib");

            var searhedSales = Sales.Where(i => i.Date.Day == date.Day && i.IsDeleted == false);
            if (searhedSales.Count() == 0)
                throw new KeyNotFoundException("Bu Tarixdə Satış Olmayıb");

            return searhedSales;

        }

        public void ReturnProductFromSale(string name,int saleId,int quantity)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Məhsulun adı doğru daxil edilməyib");

            if (saleId == 0)
                throw new ArgumentNullException("saleId", "Satisin kodu dogru daxil edilmeyib");

            if (!Sales.Exists(i => i.ID == saleId && i.IsDeleted == false))
                throw new KeyNotFoundException("daxil etdiyiniz koda uygun satis yoxdur ya da silinib");

            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId);

            if (sale.SaleItems.Exists(i => i.Product.Name != name))
                throw new KeyNotFoundException("Qebzde satilmis mehsullar arasinda bu mehsul yoxdur");

            if (quantity==0)
                throw new ArgumentNullException("quantity", "Qebzdeki satilmis Məhsulun sayı doğru daxil edilməyib");     

            if(quantity > sale.SaleItems.Where(i => i.Product.Name == name).Sum(i => i.Quantity))
                throw new ArgumentOutOfRangeException("quantity", "Qebzdeki mehsul bu qeder cox deyil");

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
