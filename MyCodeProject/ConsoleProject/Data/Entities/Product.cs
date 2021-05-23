using ConsoleProject.Data.Common;
using ConsoleProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Data.Entities
{
    class Product : BaseEntity
    {
        private static int _count;

        public string Name { get; set; }
        public double Price { get; set; }
        public Categories Category { get; set; }
        public int Quantity { get; set; }
        public Product()
        {
             _count++;
            this.ID = _count;
          
        }
    }
}
