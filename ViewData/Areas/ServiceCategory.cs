using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Areas
{
    public class ServiceCategory
    {
        public class ViewServiceCategory
        {
            public Guid id { get; set; }

            public Guid pid { get; set; }

            public string title { get; set; }

            public string parent { get; set; }

            public int count { get; set; }
        }

        public class ServiceCatSelect
        {
            public Guid id { get; set; }

            public Guid pid { get; set; }

            public string title { get; set; }
        }
    }
}
