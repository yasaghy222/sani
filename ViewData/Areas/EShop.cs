using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Areas
{
    public class EShop
    {
        public class ViewTblEShop
        {
            public Guid id { get; set; }
            public string code { get; set; }
            public string pass { get; set; }
            public string title { get; set; }
            public string shopTel { get; set; }
            public string managerName { get; set; }
            public string managerTel { get; set; }
            public string managerPhoneNumber { get; set; }
            public string managerNationalCode { get; set; }
            public Guid bCode { get; set; }
            public DateTime dateRegister { get; set; }
            public string address { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public long creditor { get; set; }
            public byte benefitPercent { get; set; }
            public string shabaNumber { get; set; }
            public string pusheId { get; set; }
            public bool status { get; set; }
            public string token { get; set; }
            public virtual ICollection<tblAssignToOff> tblAssignToOff { get; set; }
            public virtual tblBranch tblBranch { get; set; }
            public virtual ICollection<tblDocuments> tblDocuments { get; set; }
            public virtual ICollection<tblElectricShopCustomers> tblElectricShopCustomers { get; set; }
            public virtual ICollection<tblFactor> tblFactor { get; set; }
            public virtual ICollection<tblPaymentLogs> tblPaymentLogs { get; set; }
        }
        public class ViewEShop
        {
            public Guid id { get; set; }
            public string title { get; set; }
            public string managerName { get; set; }
            public string managerPhoneNumber { get; set; }
            public string creditor { get; set; }
            public string branchName { get; set; }
            public bool? status { get; set; }
        }
    }
}
