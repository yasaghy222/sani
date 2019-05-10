using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Areas
{
    public class Services
    {
        public class ViewServicesList
        {
            public Guid id { get; set; }

            public string title { get; set; }

            public bool unit { get; set; }

            public string catName { get; set; }

            public string description { get; set; }

            public bool status { get; set; }
        }

        public class OrderService
        {
            public Guid id { get; set; }
            public string title { get; set; }
            public int count { get; set; }
            public long price { get; set; }
            public string catList { get; set; }
        }

        public class OServiceVar
        {
            public Guid id { get; set; }
            public int count { get; set; }
            public long price { get; set; }
        }

        public class ViewServiceAssignList
        {
            public Guid bCode { get; set; }

            public string bTitle { get; set; }

            public Guid serviceId { get; set; }

            public string price { get; set; }
        }
    }
}
