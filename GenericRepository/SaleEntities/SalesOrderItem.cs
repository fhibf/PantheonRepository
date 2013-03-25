using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleEntities
{
    public class SalesOrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int SalesOrderId { get; set; }

        public SalesOrder SalesOrder { get; set; }

        public int Quantity { get; set; }

        public decimal Value { get; set; }
    }
}
