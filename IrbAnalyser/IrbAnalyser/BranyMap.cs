using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace IrbAnalyser
{

    /// <summary>
    /// Class for Site mapping, contains string dictionnary for site
    /// </summary>
    public static class BranySiteMap
    {
        public static readonly StringDictionary siteMapBrany = new StringDictionary()
            {
                { "Montefiore Medical Center", "Eisntein-Montefiore" },
                { "Albert Einstein Col. of Med. of Yeshiva University", "Eisntein-Montefiore" },
                { "Westchester Cardiology", "Eisntein-Montefiore" },
                { "Montefiore (Weiler Division)", "Eisntein-Montefiore" },
                { "Montefiore (Moses Division)", "Eisntein-Montefiore" },
                { "Montefiore (Children's Hosp)", "Eisntein-Montefiore" },
                { "Montefiore (Einstein Liver Center)", "Eisntein-Montefiore" },
                { "Montefiore Medical Center (IBC)", "Eisntein-Montefiore" }
            };
    }


    /// <summary>
    /// Class for Role mapping, contains string dictionnary for role and group
    /// </summary>
    public static class BranyRoleMap
    {
        public static readonly StringDictionary roleMapBrany = new StringDictionary()
            {
                { "CRO", "NA" },
                { "Sponsor", "Limited PI" },
                { "Auditor", "Limited PI" },
                { "CC Recipient", "Limited PI" },
                { "Co-Investigator", "Limited PI" },
                { "Consultant", "NA" },
                { "Coordinator", "Study Coordinator" },
                { "Faculty Advisor", "Limited PI" },
                { "PI", "PI" },
                { "Research Assistant", "Limited PI" }
            };

        public static readonly StringDictionary groupMapBrany = new StringDictionary()
            {
                { "CRO", "NA" },
                { "Sponsor", "Study team" },
                { "Auditor", "Study team" },
                { "CC Recipient", "Study team" },
                { "Co-Investigator", "Study team" },
                { "Consultant", "NA" },
                { "Coordinator", "Study team" },
                { "Faculty Advisor", "Study team" },
                { "PI", "Study team" },
                { "Research Assistant", "Study team" }
            };
    }

    /// <summary>
    /// Class to hold mapping, 2 dictionnary for status and status type
    /// </summary>
    public static class BranyStatusMap
    {
        public static readonly StringDictionary statusMapBrany = new StringDictionary()
            {
                { "Approved", "IRB Approved" },
                { "Approved -IRB only", "IRB Approved" },
                { "Closed", "Complete " },
                { "Closed - IRB only", "Complete " },
                { "Closed to Enrollment", "Closed to accrual" },
                { "Closed to Enrollment -IRB only", "Closed to accrual" },
                { "Deferred", "IRB Deferred" },
                { "Deferred – IRB only", "IRB Deferred" },
                { "Disapproved", "IRB Disapproved " },
                { "Emergency Use ONLY", "Undefined IRB Event" },
                { "Exempt", "IRB Exempt" },
                { "Exempt - Closed/Completed", "Complete " },
                { "Expired - DO NOTHING in the interface to the study information", "NA" },
                { "Not Engaged in Human Sub Rsrch", "NOSTUDY" },
                { "Other than Human Subject Rsrch", "NOSTUDY" },
                { "Pending Approval", "IRB INITIAL Submitted" },
                { "Pending Approval – IRB only", "IRB INITIAL Submitted" },
                { "Pending Closure", "Undefined IRB Event" },
                { "Pending closure – IRB only", "Undefined IRB Event" },
                { "Pending Review", "IRB INITIAL Submitted" },
                { "Pending Review – IRB only", "IRB INITIAL Submitted" },
                { "PENDING VETTING", "IRB INITIAL Submitted Draft - Record created" },
                { "Submission pending", "Draft - Record created" },
                { "Submission pending – IRB only", "Draft - Record created" },
                { "Suspended by entity other than IRB", "Temporarily Closed to Accrual and Intervention" },
                { "Suspended by entity other than IRB - IRB only", "Temporarily Closed to Accrual and Intervention" },
                { "Suspended by IRB", "Temporarily Closed to Accrual and Intervention" },
                { "Suspended by IRB - IRB only", "Temporarily Closed to Accrual and Intervention" },
                { "Terminated by IRB", "IRB Disapproved " },
                { "Terminated by IRB – IRB only", "IRB Disapproved " },
                { "Transferred to Another Record - trigger an alert to ric", "Undefined IRB Event" },
                { "Withdrawn by PI/Institution", "Withdrawn" },
                { "Withdrawn by Sponsor", "Withdrawn" }
            };
        public static readonly StringDictionary typeMapBrany = new StringDictionary()
            {
                { "Approved", "Pre Activation" },
                { "Approved -IRB only", "Pre Activation" },
                { "Closed", "Study Status" },
                { "Closed - IRB only", "Study Status" },
                { "Closed to Enrollment", "Study Status" },
                { "Closed to Enrollment -IRB only", "Study Status" },
                { "Deferred", "Pre Activation" },
                { "Deferred – IRB only", "Pre Activation" },
                { "Disapproved", "Pre Activation" },
                { "Emergency Use ONLY", "Pre Activation" },
                { "Exempt", "Pre Activation" },
                { "Exempt - Closed/Completed", "Study Status" },
                { "Expired", "NA" },
                { "Not Engaged in Human Sub Rsrch", "NOSTUDY" },
                { "Other than Human Subject Rsrch", "NOSTUDY" },
                { "Pending Approval", "Pre Activation" },
                { "Pending Approval – IRB only", "Pre Activation" },
                { "Pending Closure", "Pre Activation" },
                { "Pending closure – IRB only", "Pre Activation" },
                { "Pending Review", "Pre Activation" },
                { "Pending Review – IRB only", "Pre Activation" },
                { "PENDING VETTING", "Pre Activation" },
                { "Submission pending", "Pre Activation" },
                { "Submission pending – IRB only", "Pre Activation" },
                { "Suspended by entity other than IRB", "Study Status" },
                { "Suspended by entity other than IRB - IRB only", "Study Status" },
                { "Suspended by IRB", "Study Status" },
                { "Suspended by IRB - IRB only", "Study Status" },
                { "Terminated by IRB", "Pre Activation" },
                { "Terminated by IRB – IRB only", "Pre Activation" },
                { "Transferred to Another Record - trigger an alert to ric", "Pre Activation" },
                { "Withdrawn by PI/Institution", "Study Status" },
                { "Withdrawn by Sponsor", "Study Status" }
            };
    }


}
