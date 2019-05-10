using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Areas
{
    public class Factor
    {
        public class ViewTblFactor
        {
            public System.Guid id { get; set; }
            public string code { get; set; }
            public Guid customerId { get; set; }
            public Guid? forceId { get; set; }
            public Guid? elctricShopId { get; set; }
            public Guid? opratorId { get; set; }
            public System.Guid bCode { get; set; }
            public bool paymentType { get; set; }
            public bool paymentStatus { get; set; }
            public byte? cancelReason { get; set; }
            public string suspendResaon { get; set; }
            public bool? isPrint { get; set; }
            public string confirmSignature { get; set; }
            public string description { get; set; }
            public DateTime? registerDate { get; set; }
            public DateTime? forceStartDate { get; set; }
            public DateTime? forceEndDate { get; set; }
            public Guid? addressId { get; set; }
            public Guid? offId { get; set; }
            public long? offCount { get; set; }
            public long totalCount { get; set; }
            public byte status { get; set; }
            public virtual ICollection<tblAssignToFactor> tblAssignToFactor { get; set; }
            public virtual tblBranch tblBranch { get; set; }
            public virtual tblCustomer tblCustomer { get; set; }
            public virtual tblCustomerAddress tblCustomerAddress { get; set; }
            public virtual tblElctricShop tblElctricShop { get; set; }
            public virtual tblOff tblOff { get; set; }
            public virtual ICollection<tblForceCancels> tblForceCancels { get; set; }
            public virtual ICollection<tblForceRating> tblForceRating { get; set; }
            public virtual tblForce tblForce { get; set; }
        }

        public class FactorVariable
        {
            public Guid id { get; set; }
            public Guid customerId { get; set; }
            public Guid? forceId { get; set; }
            public Guid? opreatorId { get; set; }
            public bool isPrint { get; set; }
            public string description { get; set; }
            public string registerDate { get; set; }
            public Guid bCode { get; set; }
            public Guid addressId { get; set; }
            public List<Services.OServiceVar> services { get; set; }
        }

        public class ViewOrder
        {
            public Guid id { get; set; }
            public string code { get; set; }
            public string registerDate { get; set; }
            public string customerName { get; set; }
            public Guid customerId { get; set; }
            public string orderTitles { get; set; }
            public string forceName { get; set; }
            public Guid forceId { get; set; }
            public string byWho { get; set; }
            public string  branchName { get; set; }
            public string status { get; set; }
        }

        public class ViewFactorItem
        {
            public Guid? factorId { get; set; }
            public Guid serviceId { get; set; }
            public string title { get; set; }
            public int count { get; set; }
        }

        public class FactorPreview
        {
            public Guid id { get; set; }
            public string code { get; set; }
            public Guid customerId { get; set; }
            public string customerName { get; set; }
            public string customerPhoneNumber { get; set; }
            public string customerPusheId { get; set; }
            public string address { get; set; }
            public float lat { get; set; }
            public float lng { get; set; }
            public bool isPrint { get; set; }
            public string description { get; set; }
            public long totalCount { get; set; }
            public List<ViewFactorItem> tblAssignToFactor { get; set; }
        }
    }
}
