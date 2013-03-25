using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleEntities
{
    public class SalesOrder
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public ICollection<SalesOrderItem> Items { get; set; }

        public SalesOrder()
        {
            this.Date = DateTime.Now;
            this.Items = new List<SalesOrderItem>();
        }
    }
}
