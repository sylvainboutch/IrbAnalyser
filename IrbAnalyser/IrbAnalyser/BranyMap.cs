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
        private static string velosEM = "Einstein Montefiore (non-treating site)";
        public static readonly Dictionary<string, string> siteMapBrany = new Dictionary<string, string>()
            {
                {"",""},
                { "Montefiore Medical Center", velosEM },
                { "Albert Einstein Col. of Med. of Yeshiva University", velosEM },
                { "Westchester Cardiology", velosEM },
                { "Montefiore (Weiler Division)", velosEM },
                { "Montefiore (Moses Division)", velosEM },
                { "Montefiore (Children's Hosp)", velosEM },
                { "Montefiore (Einstein Liver Center)", velosEM }
            };

        public static string getSite(string key)
        {
            try
            {
                return siteMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New site", key);
                return key + "  (NEW !!!)";
            }
        }
    }


    /// <summary>
    /// Class for Role mapping, contains string dictionnary for role and group
    /// </summary>
    public static class BranyRoleMap
    {
        public static string PI = "Investigator";
        public static string RC = "Coordinator";
        public static string CRO = "CRO";

        public static readonly Dictionary<string, string> roleMapBrany = new Dictionary<string, string>()
            {
                { RC, "No privilege" },
                { PI, "No privilege" },
                { "Faculty Advisor", "No privilege" },
                { "Research Assistant", "No privilege" },
                { "Auditor", "No privilege" },
                { "CC Recipient", "No privilege" },
                { "Co-Investigator", "No privilege" },
                { "Sponsor", "No privilege" },
                { "Consultant", "No privilege" },
                {"",""},
                { CRO, "NA" }
            };

        public static readonly Dictionary<string, string> roleMapBranyPrimary = new Dictionary<string, string>()
            {
                { PI, OutputTeam.RC },
                { RC, OutputTeam.PI },
                { "Faculty Advisor", "No privilege" },
                { "Research Assistant", "No privilege" },
                { "Auditor", "No privilege" },
                { "CC Recipient", "No privilege" },
                { "Co-Investigator", "No privilege" },
                { "Sponsor", "No privilege" },
                { "Consultant", "No privilege" },
                {"",""},
                { CRO, "NA" }
            };

        public static string getRole(string key, bool primary)
        {
            try
            {
                if (primary)
                    return roleMapBranyPrimary[key.Trim()];
                else
                    return roleMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New role", key);
                return key + "  (NEW !!!)";
            }
        }
    }

    /// <summary>
    /// Class to hold mapping, 2 dictionnary for status and status type
    /// </summary>
    public static class BranyStatusMap
    {
        public static readonly Dictionary<string, string> statusMapBrany = new Dictionary<string, string>()
            {
                {"",""},
                { "Approved", "IRB Initial Approved" },
                { "Approved IRB only", "IRB Initial Approved" },
                { "Closed", "Complete " },
                { "Closed IRB only", "Complete " },
                { "Closed to Enrollment", "Closed to accrual" },
                { "Closed to enrollment IRB only", "Closed to accrual" },
                { "Deferred", "IRB Deferred" },
                { "Deferred IRB only", "IRB Deferred" },
                { "Disapproved", "IRB Disapproved " },
                { "Emergency Use ONLY", "" },
                { "Exempt", "IRB Exempt" },
                { "Exempt Closed/Completed", "Complete " },
                { "Expired", "NA" },
                { "Not Engaged in Human Sub Rsrch", "Archived" },
                { "Other than Human Subject Rsrch", "Archived" },
                { "Pending Approval", "IRB INITIAL Submitted" },
                { "Pending Approval IRB only", "IRB INITIAL Submitted" },
                { "Pending Closure", "" },
                { "Pending closure IRB only", "" },
                { "Pending Review", "IRB INITIAL Submitted" },
                { "Pending Review IRB only", "IRB INITIAL Submitted" },
                { "PENDING VETTING", "Draft" },
                { "Submission pending", "Draft" },
                { "Submission pending IRB only", "Draft" },
                { "Suspended by entity other than IRB", "Temporarily Closed to Accrual and Intervention" },
                { "Suspended by entity other than IRB IRB only", "Temporarily Closed to Accrual and Intervention" },
                { "Suspended by IRB", "Temporarily Closed to Accrual and Intervention" },
                { "Suspended by IRB IRB only", "Temporarily Closed to Accrual and Intervention" },
                { "Terminated by IRB", "IRB Disapproved " },
                { "Terminated by IRB IRB only", "IRB Disapproved " },
                { "Transferred to Another Record", "" },
                { "Withdrawn by PI/Institution", "Withdrawn" },
                { "Withdrawn by Sponsor", "Withdrawn" },
                { "Pulled", "" },
                { "Approved (IBC)", "" },
                { "Approved IBC Only", "" },
                { "Pending Review (IBC)", "" },
                { "Closed to Enrollment (IBC)", "Closed to accrual" },
                { "Dropped by Investigator", "" },
                { "Administrative - Translation", "" }
            };

        public static string getStatus(string key)
        {
            try
            {
                return statusMapBrany[Tools.cleanMap(key)];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status", key);
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, string> typeMapBrany = new Dictionary<string, string>()
            {
                {"",""},
                { "Approved", "Pre Activation" },
                { "Approved IRB only", "Pre Activation" },
                { "Closed", "Study Status" },
                { "Closed IRB only", "Study Status" },
                { "Closed to Enrollment", "Study Status" },
                { "Closed to enrollment IRB only", "Study Status" },
                { "Deferred", "Pre Activation" },
                { "Deferred IRB only", "Pre Activation" },
                { "Disapproved", "Pre Activation" },
                { "Emergency Use ONLY", "Pre Activation" },
                { "Exempt", "Pre Activation" },
                { "Exempt ClosedCompleted", "Study Status" },
                { "Expired", "NA" },
                { "Not Engaged in Human Sub Rsrch", "Study Status" },
                { "Other than Human Subject Rsrch", "Study Status" },
                { "Pending Approval", "Pre Activation" },
                { "Pending Approval IRB only", "Pre Activation" },
                { "Pending Closure", "Pre Activation" },
                { "Pending closure IRB only", "Pre Activation" },
                { "Pending Review", "Pre Activation" },
                { "Pending Review IRB only", "Pre Activation" },
                { "PENDING VETTING", "Pre Activation" },
                { "Submission pending", "Pre Activation" },
                { "Submission pending IRB only", "Pre Activation" },
                { "Suspended by entity other than IRB", "Study Status" },
                { "Suspended by entity other than IRB IRB only", "Study Status" },
                { "Suspended by IRB", "Study Status" },
                { "Suspended by IRB IRB only", "Study Status" },
                { "Terminated by IRB", "Pre Activation" },
                { "Terminated by IRB IRB only", "Pre Activation" },
                { "Transferred to Another Record trigger an alert to ric", "Pre Activation" },
                { "Withdrawn by PI/Institution", "Study Status" },
                { "Withdrawn by Sponsor", "Study Status" },
                { "Pulled", "Pre Activation" },
                { "Approved IBC Only", "Pre Activation" },
                { "Approved (IBC)", "Pre Activation" },
                { "Pending Review (IBC)", "Pre Activation" },
                { "Dropped by Investigator", "Pre Activation" },
                { "Closed to Enrollment (IBC)", "Study Status" },
                { "Administrative - Translation", "Pre Activation" }
            };

        public static string getType(string key)
        {
            try
            {
                return typeMapBrany[Tools.cleanMap(key)];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status (from status type mapping)", key);
                return key + "  (NEW !!!)";
            }
        }

    }

    /// <summary>
    /// Class to hold mapping, 2 dictionnary for status and status type
    /// </summary>
    public static class BranyEventsMap
    {
        public static readonly Dictionary<string, string> eventsMapBrany = new Dictionary<string, string>()
            {
                {"1572 Modification",""},
                {"Additional PI",""},
                {"Administrative (Translation)",""},
                {"Administrative - Translation",""},
                {"Administrative Review",""},
                {"Advertisements",""},
                {"Amendment","IRB Amendment Submitted**"},
                {"Continuing Review",""},
                {"Expedited Continuing Review",""},
                {"Expedited Initial Review",""},
                {"Full Board Reconsideration",""},
                {"New Application for IRB Review",""},
                {"Noncompliance",""},
                {"Protocol Exception",""},
                {"QA Review",""},
                {"Replacement PI",""},
                {"Reportable Event",""},
                {"Revised Letter",""},
                {"SAE",""},
                {"Study Closure/Expiration",""},
                {"Study Enrollment Closure Report",""},
                {"Unanticipated Problem",""},
                {"Re-Review of IRB-Requested Changes",""},
                {"IND Safety Report",""},
                {"Incomplete Submission",""},
                {"Protocol Deviation",""},
                {"Safety Report(s) - No Review",""},
                {"Personnel Change",""},
                {"COI Review",""},
                {"PI Signature Needed",""},
                {"Emergency Use of a Test Article",""},
                {"xForm Request Form Only",""},
                {"IND Safety Report with ICF Changes",""},
                {"IBC Screening",""},
                {"IBC Review",""},
                {"IBC Continuing/Periodic Review",""},
                {"Advertisements Re-Review",""},
                {"New Protocol Event (Not ready for IRB Review)",""},
                {"Initial Submission Documents",""},
                {"FYI to Full Board",""},
                {"Subject Complaint",""},
                {"Exempt Status Determination",""},
                {"Compassionate Use Request",""}

            };

        public static string getStatus(string key)
        {
            try
            {
                return eventsMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status (from events)", key);
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, string> eventsTypeMapBrany = new Dictionary<string, string>()
            {
                {"1572 Modification","Pre Activation"},
                {"Additional PI","Pre Activation"},
                {"Administrative (Translation)","Pre Activation"},
                {"Administrative - Translation","Pre Activation"},
                {"Administrative Review","Pre Activation"},
                {"Advertisements","Pre Activation"},
                {"Amendment","Pre Activation"},
                {"Continuing Review","Pre Activation"},
                {"Expedited Continuing Review","Pre Activation"},
                {"Expedited Initial Review","Pre Activation"},
                {"Full Board Reconsideration","Pre Activation"},
                {"New Application for IRB Review","Pre Activation"},
                {"Noncompliance","Pre Activation"},
                {"Protocol Exception","Pre Activation"},
                {"QA Review","Pre Activation"},
                {"Replacement PI","Pre Activation"},
                {"Reportable Event","Pre Activation"},
                {"Revised Letter","Pre Activation"},
                {"SAE","Pre Activation"},
                {"Study Closure/Expiration","Pre Activation"},
                {"Study Enrollment Closure Report","Pre Activation"},
                {"Unanticipated Problem","Pre Activation"},
                {"Re-Review of IRB-Requested Changes","Pre Activation"},
                {"IND Safety Report","Pre Activation"},
                {"Incomplete Submission","Pre Activation"},
                {"Protocol Deviation","Pre Activation"},
                {"Safety Report(s) - No Review","Pre Activation"},
                {"Personnel Change","Pre Activation"},
                {"COI Review","Pre Activation"},
                {"PI Signature Needed","Pre Activation"},
                {"Emergency Use of a Test Article","Pre Activation"},
                {"xForm Request Form Only","Pre Activation"},
                {"IND Safety Report with ICF Changes","Pre Activation"},
                {"IBC Screening","Pre Activation"},
                {"IBC Review","Pre Activation"},
                {"IBC Continuing/Periodic Review","Pre Activation"},
                {"Advertisements Re-Review","Pre Activation"},
                {"New Protocol Event (Not ready for IRB Review)","Pre Activation"},
                {"Initial Submission Documents","Pre Activation"},
                {"FYI to Full Board","Pre Activation"},
                {"Subject Complaint","Pre Activation"},
                {"Exempt Status Determination","Pre Activation"},
                {"Compassionate Use Request","Pre Activation"}
            };

        public static string getType(string key)
        {
            try
            {
                return eventsTypeMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status (from events status type map)", key);
                return key + "  (NEW !!!)";
            }
        }

    }


}
