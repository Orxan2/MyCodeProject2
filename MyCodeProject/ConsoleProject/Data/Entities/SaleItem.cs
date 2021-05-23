using ConsoleProject.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Data.Entities
{
    class SaleItem : BaseEntity
    {
        private static int _count;

        public Product Product { get; set; }
        public int Quantity { get; set; }
    
        public SaleItem()
        {
            _count++;
            this.ID = _count;
        }
    }
}
