using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleEntities
{
    public class Address
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }
        
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public string ZipCode { get; set; }

        public IEnumerable<SalesOrder> Sales { get; set; }

        public Address()
        {
            this.Sales = new List<SalesOrder>();
        }
    }
}
