//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IrbAnalyser.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ER_STUDYVER
    {
        public ER_STUDYVER()
        {
            this.ER_STUDYAPNDX = new HashSet<ER_STUDYAPNDX>();
        }
    
        public decimal PK_STUDYVER { get; set; }
        public Nullable<decimal> FK_STUDY { get; set; }
        public string STUDYVER_NUMBER { get; set; }
        public string STUDYVER_STATUS { get; set; }
        public string STUDYVER_NOTES { get; set; }
        public Nullable<decimal> ORIG_STUDY { get; set; }
        public Nullable<decimal> RID { get; set; }
        public Nullable<decimal> CREATOR { get; set; }
        public Nullable<decimal> LAST_MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> LAST_MODIFIED_DATE { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public string IP_ADD { get; set; }
        public Nullable<System.DateTime> STUDYVER_DATE { get; set; }
        public Nullable<decimal> STUDYVER_CATEGORY { get; set; }
        public Nullable<decimal> STUDYVER_TYPE { get; set; }
    
        public virtual ICollection<ER_STUDYAPNDX> ER_STUDYAPNDX { get; set; }
    }
}
