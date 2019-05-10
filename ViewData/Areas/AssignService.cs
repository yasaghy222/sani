using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Areas
{
    public class AssignService
    {
        public class ViewTblBranchService
        {
            public Guid bCode { get; set; }

            public Guid serviceId { get; set; }

            public long price { get; set; }

            public bool formMode { get; set; }
        }
    }
}
