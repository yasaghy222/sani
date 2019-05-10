using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Areas
{
    public class Customer
    {
        public class ViewCustomerList
        {
            public Guid id { get; set; }
            public string name { get; set; }
            public string phoneNumber { get; set; }
            public long? creditor { get; set; }
            public string cityName { get; set; }
            public bool? status { get; set; }
        }

        public class OrderCustomer
        {
            public Guid id { get; set; }
            public string name { get; set; }
            public string phoneNumber { get; set; }
            public Guid bCode { get; set; }
        }

        public class OrderAddress
        {
            public Guid id { get; set; }
            public int locationId { get; set; }
            public string cityName { get; set; }
            public string address { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public bool isSelected { get; set; }
        }

        public class OAddressVar
        {
            public Guid id { get; set; }
            public Guid customerId { get; set; }
            public int locationId { get; set; }
            public string address { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
        }
    }
}
