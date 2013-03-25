using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleEntities
{
    public class Product
    {
        public int Id { get; set; }

        public decimal SaleValue { get; set; }

        public string Name { get; set; }

        public decimal BuyValue { get; set; }
    }
}
