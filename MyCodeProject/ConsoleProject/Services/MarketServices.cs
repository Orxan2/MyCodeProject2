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

        public MarketServices()
        {
            Products = new();
            Sales = new();
            
            categoryList.AddRange(Enum.GetValues<Categories>());
        }

        #region Adding Operations

        /// <summary>
        /// Bu Metod anbara(Məhsullar siyahısına) <c>verilən kriterilərə sahib</c> yeni bir məhsul əlavə etmək üçün istifadə edilir.
        /// </summary>
        /// <param name="name">Məhsulun adıdır. Boş olmamalıdır.</param>
        /// <param name="price">Məhsulun qiymətidir.Müsbət həqiqi ədəd(double) olmalıdır.</param>
        /// <param name="category">Məhsulun kateqoriyasıdır string tipində qəbul edilir və Enum cinsinə çevriləcək.Boş olmamalıdır. </param>
        /// <param name="quantity">Məhsulun sayıdır.Tam müsbət ədəd(int) olmalıdır.</param>
        /// <returns> Geriyə bir dəyər qaytarmır.Sadəcə anbara yeni bir məhsul əlavə edir. </returns>
        /// <exception cref="System.ArgumentNullException">Data boş olarsa, meydana gəlir.</exception>
        /// <exception cref="System.DuplicateWaitObjectException">Əgər VB-da eyniadlı başqa bir obyekt varsa,meydana gəlir</exception>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        public void AddProduct(string name, double price, string category, int quantity)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The product's name is empty");

            if (Products.Exists(i => i.Name == name))
                throw new DuplicateWaitObjectException("name", "This product is already in the database");

            if (price <= 0)
                throw new FormatException("The price of the product was entered incorrectly");

            if (string.IsNullOrEmpty(category))
                throw new ArgumentNullException("category", "The product's category is empty");

            if (!categoryList.Exists(i => i.ToString() == category))
                throw new FormatException("The product's category was entered incorrectly");

            if (quantity <= 0)
                throw new FormatException("The product's quantity must be number and greater than 0!");

            //Product sinfindən bir obyekt yaradılır,ona dataları mənimsədilir və Products listinə əlavə edilir.
            Product product = new();
            product.Name = name;
            product.Price = price;
            product.Category = Enum.Parse<Categories>(category);
            product.Quantity = quantity;
            product.IsDeleted = false;
            Products.Add(product);

        }

        /// <summary>
        /// Bu Metod Satışa <c>anbarda olan</c> hər hansı bir məhsulu əlavə etmək üçün istifadə edilir.
        /// </summary>        
        /// <param name="datas">Bir Kolleksiyasıdır. Müsbət tam ədəd cinsindən İD və say tələb edir.</param>
        /// /// <returns> Geriyə bir dəyər qaytarmır.Sadəcə satışa yeni bir məhsul əlavə edir. </returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        /// <exception cref="System.ArgumentNullException">Data boş olarsa, meydana gəlir.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Datanın sahib olduğu intervaldan kənarda bir dəyər əlavə edilərsə, meydana gəlir.</exception>
        public void AddSale(Dictionary<int, int> datas)
        {
            Sale sale = new();
            foreach (KeyValuePair<int, int> data in datas)
            {
                if (data.Key <= 0)
                    throw new FormatException("The Product's ID was entered incorrectly");

                Product product = Products.FirstOrDefault(i => i.ID == data.Key && i.IsDeleted == false);

                if (product == null)
                    throw new ArgumentNullException("product","The product was not found");

                if (data.Value > product.Quantity)
                    throw new ArgumentOutOfRangeException("data.Value", "There are not so many products in the database");
               
                SaleItem saleItem = new();
                saleItem.Product = product;
                saleItem.Quantity = data.Value;

                sale.Price += saleItem.Product.Price * saleItem.Quantity;
                //Products.FirstOrDefault(i => i.ID == saledProduct.ID).Quantity -= saledProduct.Quantity;
                sale.SaleItems.Add(saleItem);
            }

            //2-ci və ya sonrakı satış məhsullarını əlavə edəndə Exception olsa,məhsul sayında dəyişiklik olamasın deyə burda yazdım
            foreach (KeyValuePair<int, int> data in datas)
            {
                Products.FirstOrDefault(i => i.ID == data.Key).Quantity -= data.Value;
            }
            Sales.Add(sale);
        }

        #endregion

        #region Removing Operations

        /// <summary>
        ///  Bu Metod anbardan(Məhsullar siyahısı) <c>verilən kriterilərə uyğun</c> bir məhsulu silmək üçün istifadə edilir.
        /// </summary>
        /// <param name="productNo">Məhsulun ID-dir.Müsbət tam ədəd olmalıdır.</param>
        /// <returns> Geriyə bir dəyər qaytarmır.Anbardakı uyğun məhsulu silir.<c>(isdeleted = true)</c> </returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        /// <exception cref="System.ArgumentNullException">Data boş olarsa, meydana gəlir.</exception>
        public void DeleteProduct(int productNo)
        {
            if (productNo == 0)
                throw new FormatException("The product's ID was entered incorrectly");

            //Siyahıdan verilən ID-ə uyğun məhsul seçılir.
            Product product = Products.FirstOrDefault(i => i.ID == productNo);
            if (product == null || product.IsDeleted == true)
                throw new ArgumentNullException("product","The product was not found or has already been deleted");

            product.IsDeleted = true;
        }

        /// <summary>
        /// Bu Metod satışdan <c>verilən kriterilərə uyğun</c> bir satışı silmək üçün istifadə edilir.
        /// </summary>
        /// <param name="saleId">Satışın ID-dir.Müsbət tam ədəd olmalıdır.</param>
        /// <returns> Geriyə bir dəyər qaytarmır.Sadəcə satışı silir. <c>(isdeleted = true)</c> </returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">VB-da daxil edilən dataya sahib bir obyekt tapılmasa meydana gəlir</exception>
        public void DeleteSale(int saleId)
        {
            if (saleId == 0)
                throw new FormatException("The sale's ID was entered incorrectly");

            if (Sales.Exists(i => i.ID != saleId || i.IsDeleted == true))
                throw new KeyNotFoundException("The ID you entered does not match the sale or sale has already been deleted");

            //Siyahıdan verilən ID-ə uyğun satış seçılir.
            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId);

            foreach (var saleItem in sale.SaleItems)
            {
                Product product = (Products.FirstOrDefault(i => i.ID == saleItem.Product.ID));

                //Məhsul silinibsə belə yenidən anbara daxil edilir.
                if (product.IsDeleted == true)
                    product.IsDeleted = false;

                product.Quantity += saleItem.Quantity;
            }

            sale.IsDeleted = true;

        }

        #endregion

        #region Editing Operations

        /// <summary>
        /// Bu Metod anbardan(Məhsullar siyahısı) <c>silinmiş uyğun</c> məhsulu bərpa etmək üçün istifadə edilir.
        /// </summary>
        /// <param name="productNo">Məhsulun ID-dir.Müsbət tam ədəd olmalıdır.</param>
        /// <returns> Geriyə bir dəyər qaytarmır.Sadəcə silinmiş uyğun məhsulu bərpa edir.<c>(isdeleted = false)</c> </returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        /// <exception cref="System.ArgumentNullException">Data boş olarsa, meydana gəlir.</exception>
        public void RestoreProduct(int productNo)
        {
            if (productNo == 0)
                throw new FormatException("The product's ID was entered incorrectly");

            //Siyahıdan verilən ID-ə uyğun məhsul seçılir.
            Product product = Products.FirstOrDefault(i => i.ID == productNo);

            if (product == null || product.IsDeleted == false)
                throw new ArgumentNullException("product","The product was not found or hasn't been deleted");

            product.IsDeleted = false;
        }

        /// <summary>
        ///Bu metod məhsulun hər hansı bir datasını editləmək üçün istifadə edilir.
        /// </summary>
        /// <param name="productNo">Məhsulun ID-dir.Müsbət tam ədəd olmalıdır.</param>
        /// <param name="data">Product sinfindən bir məhsul obyektidir.</param>
        /// <returns> Geriyə bir dəyər qaytarmır.Sadəcə məhsulun datalarını yeniləyir.</returns>       
        public void EditProduct(int productNo, Product data)
        {
           
            foreach (var product in Products)
            {
                if (product.ID == productNo && product.IsDeleted == false)
                {
                    if (!string.IsNullOrEmpty(data.Name))
                        product.Name = data.Name;

                    if (data.Price != 0)
                        product.Price = data.Price;

                    if (data.Quantity != 0)
                        product.Quantity = data.Quantity;
                }
            }
        }
               
        /// <summary>
        /// Bu Metod <c>silinmiş uyğun</c> satışı bərpa etmək üçün istifadə edilir.
        /// </summary>
        /// <param name="saleId">Satışın ID-dir.Müsbət tam ədəd olmalıdır.</param>
        /// <returns> Geriyə bir dəyər qaytarmır.Sadəcə satışı silir. <c>(isdeleted = false)</c> </returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">VB-da daxil edilən dataya sahib bir obyekt tapılmasa meydana gəlir</exception>
        public void RestoreSale(int saleId)
        {
            if (saleId == 0)
                throw new FormatException("The sale's ID is not entered correctly");

            if (Sales.Exists(i => i.ID != saleId || i.IsDeleted == false))
                throw new KeyNotFoundException("The ID you entered does not match the sale or sale hasn't been deleted");

            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId);

            foreach (var saleItem in sale.SaleItems)
            {
                //Siyahıdan verilən ID-ə uyğun məhsul seçılir və sayı azaldılır.
                Product product = (Products.FirstOrDefault(i => i.ID == saleItem.Product.ID));

                if (product.IsDeleted == true)
                    product.IsDeleted = false;

                product.Quantity -= saleItem.Quantity;
            }

            sale.IsDeleted = false;
        }

        /// <summary>
        /// Bu metod satılmış hər hansı məhsulu anbara(məhsullar siyahısı) geri qaytarmaq üçün istifadə edilir
        /// </summary>
        /// <param name="name">Satışdan qayıdan məhsulun adıdır.Boş olmamalıdır</param>
        /// <param name="saleId">Satışın ID-dir.Müsbət tam ədəd olmalıdır</param>
        /// <param name="quantity">Satışdan qayıdan məhsulun sayıdır.Müsbət tam ədəd olmalıdır</param>
        /// <returns> Geriyə bir dəyər qaytarmır.Sadəcə satılmış bir məhsulu anbara geri qaytarır.</returns>
        /// <exception cref="System.ArgumentNullException">Data boş olarsa, meydana gəlir</exception>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">VB-da daxil edilən dataya sahib bir obyekt tapılmasa meydana gəlir</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Datanın sahib olduğu intervaldan kənarda bir dəyər əlavə edilərsə, meydana gəlir</exception>
        public void ReturnProductFromSale(string name, int saleId, int quantity)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The product's name was entered incorrectly");

            if (saleId == 0)
                throw new FormatException("The sale's ID was entered incorrectly");

            if (!Sales.Exists(i => i.ID == saleId && i.IsDeleted == false))
                throw new KeyNotFoundException("The ID you entered does not match the sales or sale has already been deleted");

            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId);

            if (sale.SaleItems.Exists(i => i.Product.Name != name))
                throw new KeyNotFoundException("There is no such product among the sold products");

            if (quantity == 0)
                throw new FormatException("Qebzdeki satilmis Məhsulun sayı doğru daxil edilməyib");

            if (quantity > sale.SaleItems.Where(i => i.Product.Name == name).Sum(i => i.Quantity))
                throw new ArgumentOutOfRangeException("quantity", "There are not so many products in the sale list");

            foreach (var saleItem in sale.SaleItems)
            {
                //uyğun addakı məhsulun satışdan geri qayıtması
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

        #endregion

        #region Searching Operations

        /// <summary>
        /// Məhsulun ada görə axtarışını icra edən bir metoddur.Əgər məhsulun adında daxil edilən mətn varsa, həmin məhsulu və ya məhsulları qaytaracaq
        /// </summary>
        /// <param name="text">String tipində bir mətndir.</param>
        /// <returns>IEnumerable tipində generic kolleksiya geri qaytarır</returns>
        /// <exception cref="System.ArgumentNullException">Data boş olarsa, meydana gəlir.Bu Kolleksiyada ada görə axtarışda tapılan məhsullar olur.</exception>
        public IEnumerable<Product> SearchProductForName(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text", "The entered text is empty!");

            var updatedProdects = Products.Where(i => i.Name.Contains(text) && i.IsDeleted == false);

            return updatedProdects;
        }

        /// <summary>
        /// Məhsulun qiymətə görə axtarışını icra edən bir metoddur.
        /// </summary>
        /// <param name="min">Məhsulun axtarılan minimum qiyməti. Müsbət həqiqi ədəd olmalıdır.</param>
        /// <param name="max">Məhsulun axtarılan maksimum qiyməti. Müsbət həqiqi ədəd olmalıdır.</param>
        /// <returns>IEnumerable tipində generic kolleksiya geri qaytarır. Bu Kolleksiyada qiymət intervalına görə axtarışda tapılan məhsullar olur</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Datanın sahib olduğu intervaldan kənarda bir dəyər əlavə edilərsə, meydana gəlir.</exception>
        public IEnumerable<Product> SearchProductForPrice(double min, double max)
        {
            if (min <= 0)
                throw new ArgumentOutOfRangeException("min", "Minimum price must be number and greater than 0");
            if (max <= 0)
                throw new ArgumentOutOfRangeException("max", "Maximum price must be number and greater than 0");

            var searchedProducts = Products.Where(i => i.Price <= max && i.Price >= min && i.IsDeleted == false);

            return searchedProducts;
        }

        /// <summary>
        /// Məhsulun kateqoriyaya görə axtarışını icra edən bir metoddur.
        /// </summary>
        /// <param name="category">Məhsulun kateqoriyasıdır. String tipində qəbul edilir və Enum cinsinə çevriləcək.Boş olmamalıdır. </param>
        /// <returns>IEnumerable tipində generic kolleksiya geri qaytarır.Bu Kolleksiyada kateqoriyaya görə axtarışda tapılan məhsullar olur.</returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        public IEnumerable<Product> SearchProductForCategory(string category)
        {
            if (!categoryList.Exists(i => i.ToString() == category))
                throw new FormatException("The category of the product was entered incorrectly");

            var searchedProducts = Products.Where(i => i.Category.ToString() == category && i.IsDeleted == false);

            return searchedProducts;
        }

        /// <summary>
        /// Satışın qiymətə görə axtarışını icra edən bir metoddur.
        /// </summary>
        /// <param name="minValue">Satışın axtarılan minimum qiyməti. Müsbət həqiqi ədəd olmalıdır</param>
        /// <param name="maxValue">Satışın axtarılan minimum qiyməti. Müsbət həqiqi ədəd olmalıdır</param>
        /// <returns>IEnumerable tipində generic kolleksiya geri qaytarır. Bu Kolleksiyada qiymət intervalına görə axtarışda tapılan satışlar olur</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Datanın sahib olduğu intervaldan kənarda bir dəyər əlavə edilərsə, meydana gəlir</exception>
        public IEnumerable<Sale> SearchSalesForPrice(double minValue, double maxValue)
        {
            if (minValue <= 0)
                throw new ArgumentOutOfRangeException("minValue", "Minimum price must be number and greater than 0");

            if (maxValue <= 0)
                throw new ArgumentOutOfRangeException("maxValue", "Maximum price must be number and greater than 0");

            var searchedSales = Sales.Where(i => i.Price <= maxValue && i.Price >= minValue && i.IsDeleted == false);

            return searchedSales;

        }

        /// <summary>
        /// Satışın müəyyən bir tarix intervalına görə axtarışını icra edən bir metoddur.
        /// </summary>
        /// <param name="startDate">Axtarılan başlanğıc tarixi</param>
        /// <param name="lastDate">Axtarılan son tarix</param>
        /// <returns>IEnumerable tipində generic kolleksiya geri qaytarır. Bu Kolleksiyada tarix intervalına görə axtarışda tapılan satışlar olur.</returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">VB-da daxil edilən dataya sahib bir obyekt tapılmasa meydana gəlir</exception>
        public IEnumerable<Sale> SearchSalesForDateInterval(DateTime startDate, DateTime lastDate)
        {
            if (startDate.Year == 1)
                throw new FormatException("The start date was entered incorrectly");

            if (lastDate.Year == 1)
                throw new FormatException("The last date was entered incorrectly");

            var searhedSales = Sales.Where(i => i.Date >= startDate && i.Date <= lastDate && i.IsDeleted == false);
            if (searhedSales.Count() == 0)
                throw new KeyNotFoundException("No sales on these dates");

            return searhedSales;
        }
       
        /// <summary>
        /// Satışın müəyyən bir tarixə görə axtarışını icra edən bir metoddur.
        /// </summary>
        /// <param name="date">Axtarılan tarix</param>
        /// <returns>IEnumerable tipində generic kolleksiya geri qaytarır. Bu Kolleksiyada tarix intervalına görə axtarışda tapılan satışlar olur</returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">VB-da daxil edilən dataya sahib bir obyekt tapılmasa meydana gəlir</exception>
        public IEnumerable<Sale> SearchSalesForDate(DateTime date)
        {
            if (date.Year == 1)
                throw new FormatException("Date entered incorrectly");

            var searhedSales = Sales.Where(i => i.Date.Day == date.Day && i.IsDeleted == false);
            if (searhedSales.Count() == 0)
                throw new KeyNotFoundException("No sales on these dates");

            return searhedSales;

        }

        /// <summary>
        /// İstifadəçidən qebul edilmis id-ə esasen hemin nomreli satisin melumatlarinin gosterilmesini icra edən bir metoddur
        /// </summary>
        /// <param name="saleId">Satışın ID-dir. Müsbət tam ədəd olmalıdır</param>
        /// <returns>Sale tipində bir obyekt geri qaytarır.</returns>
        /// <exception cref="System.FormatException">Data yanlış daxil edilərsə, meydana gəlir</exception>
        /// <exception cref="System.ArgumentNullException">Data boş olarsa, meydana gəlir</exception>
        public Sale SearchSaleForID(int saleId)
        {
            if (saleId == 0)
                throw new FormatException("The sale's ID was entered incorrectly");

            Sale sale = Sales.FirstOrDefault(i => i.ID == saleId && i.IsDeleted == false);

            if (sale == null)
                throw new ArgumentNullException("sale","Searched sales not found");

            return sale;
        }

        #endregion
}
}
