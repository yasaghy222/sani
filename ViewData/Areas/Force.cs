using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ViewModel.Areas
{
    public class Force
    {
        public class ViewTblForce
        {
            public Guid id { get; set; }
            public string image { get; set; }
            public string name { get; set; }
            public string phoneNumber { get; set; }
            public string telNumber { get; set; }
            public string nationalCode { get; set; }
            public string personalCode { get; set; }
            public string pass { get; set; }
            public string address { get; set; }
            public double lat { get; set; }
            public double lng { get; set; }
            public System.Guid bCode { get; set; }
            public string mIntrodouceCode { get; set; }
            public System.DateTime dateRegister { get; set; }
            public long creditor { get; set; }
            public long debtor { get; set; }
            public byte benefitPercent { get; set; }
            public byte cancelRequestCount { get; set; }
            public string birthDate { get; set; }
            public byte rate { get; set; }
            public TimeSpan? timeOfBlock { get; set; }
            public string shabaNumber { get; set; }
            public string pusheId { get; set; }
            public string token { get; set; }
            public byte status { get; set; }
            public virtual tblBranch tblBranch { get; set; }
            public virtual ICollection<tblFactor> tblFactor { get; set; }
            public virtual ICollection<tblForceCancels> tblForceCancels { get; set; }
            public virtual ICollection<tblForceExpertise> tblForceExpertise { get; set; }
            public virtual ICollection<tblForceRating> tblForceRating { get; set; }
            public virtual ICollection<tblPaymentLogs> tblPaymentLogs { get; set; }
        }

        public class ViewForce
        {
            public Guid id { get; set; }
            public string name { get; set; }
            public string phoneNumber { get; set; }
            public string creditor { get; set; }
            public string debtor { get; set; }
            public string branchName { get; set; }
            public string status { get; set; }
        }

        public class ViewForceExp
        {
            public Guid forceId { get; set; }
            public Guid catId { get; set; }
            public string expTitle { get; set; }
            public byte cent { get; set; }
        }

        public class SelectAForce
        {
            public string Text { get; set; }
            public Guid Value { get; set; }
        }
    }
}
