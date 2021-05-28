using ConsoleProject.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Data.Entities
{
    class Sale : BaseEntity
    {
        private static int _count;

        public double Price { get; set; }
        public List<SaleItem> SaleItems { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
        public Sale()
        {
            _count++;
            this.ID = _count;
            Date = DateTime.Now;
            IsDeleted = false;
            SaleItems = new();
        }
    }
}
