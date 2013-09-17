using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleEntities
{
    public class Customer
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int CurrentAddressId { get; set; }

        public virtual Address CurrentAddress { get; set; }

        public IEnumerable<SalesOrder> Sales { get; set; }

        public CustomerDetails Details { get; set; }

        public Customer()
        {
            this.Sales = new List<SalesOrder>();
        }
    }
}
