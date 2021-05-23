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
        public SaleItem SaleItem { get; set; }
        public DateTime Date { get; set; }
        public Sale()
        {
            _count++;
            this.ID = _count;
        }
    }
}
