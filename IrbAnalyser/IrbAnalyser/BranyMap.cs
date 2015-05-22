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
        public static readonly Dictionary<string, string> siteMapBrany = new Dictionary<string, string>()
            {
                {"",""},
                { "Montefiore Medical Center", "Einstein Montefiore" },
                { "Albert Einstein Col. of Med. of Yeshiva University", "Albert Einstein Col. of Med. of Yeshiva University" },
                { "Westchester Cardiology", "Westchester Cardiology" },
                { "Montefiore (Weiler Division)", "Montefiore (Weiler Division)" },
                { "Montefiore (Moses Division)", "Montefiore (Moses Division)" },
                { "Montefiore (Children's Hosp)", "Children's Hospital at Montefiore" },
                { "Montefiore (Einstein Liver Center)", "Montefiore (Einstein Liver Center)" }
            };

        public static string getSite(string key)
        {
            try
            {
                return siteMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                return key + "  (NEW !!!)";
            }
        }
    }


    /// <summary>
    /// Class for Role mapping, contains string dictionnary for role and group
    /// </summary>
    public static class BranyRoleMap
    {
        public static readonly Dictionary<string, string> roleMapBrany = new Dictionary<string, string>()
            {
                { "Coordinator", "No privilege" },
                { "Investigator", "No privilege" },
                { "Faculty Advisor", "No privilege" },
                { "Research Assistant", "No privilege" },
                { "Auditor", "No privilege" },
                { "CC Recipient", "No privilege" },
                { "Co-Investigator", "No privilege" },
                { "Sponsor", "No privilege" },
                { "Consultant", "No privilege" },
                {"",""},
                { "CRO", "NA" }
            };

        public static readonly Dictionary<string, string> roleMapBranyPrimary = new Dictionary<string, string>()
            {
                { "Coordinator", "Study Coordinator" },
                { "Investigator", "Primary Investigator" },
                { "Faculty Advisor", "No privilege" },
                { "Research Assistant", "No privilege" },
                { "Auditor", "No privilege" },
                { "CC Recipient", "No privilege" },
                { "Co-Investigator", "No privilege" },
                { "Sponsor", "No privilege" },
                { "Consultant", "No privilege" },
                {"",""},
                { "CRO", "NA" }
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
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, string> groupMapBrany = new Dictionary<string, string>()
            {
                {"",""},
                { "CRO", "NA" },
                { "Sponsor", "Study team" },
                { "Auditor", "Study team" },
                { "CC Recipient", "Study team" },
                { "Co-Investigator", "Study team" },
                { "Consultant", "NA" },
                { "Coordinator", "Study team" },
                { "Faculty Advisor", "Study team" },
                { "PI", "Study team" },
                { "Research Assistant", "Study team" },
                { "Investigator", "Study team" }
            };


        public static string getGroup(string key)
        {
            try
            {
                return groupMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
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

        public static string getStatus(string key)
        {
            try
            {
                return statusMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, string> typeMapBrany = new Dictionary<string, string>()
            {
                {"",""},
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

        public static string getType(string key)
        {
            try
            {
                return typeMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
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
                {"1572 Modification","Undefined IRB Event"},
                {"Additional PI","Undefined IRB Event"},
                {"Administrative (Translation)","Undefined IRB Event"},
                {"Administrative Review","Undefined IRB Event"},
                {"Advertisements","Undefined IRB Event"},
                {"Amendment","IRB AMENDMENT Submitted**"},
                {"Continuing Review","Undefined IRB Event"},
                {"Expedited Continuing Review","Undefined IRB Event"},
                {"Expedited Initial Review","Undefined IRB Event"},
                {"Full Board Reconsideration","Undefined IRB Event"},
                {"New Application for IRB Review","Undefined IRB Event"},
                {"Noncompliance","Undefined IRB Event"},
                {"Protocol Exception","Undefined IRB Event"},
                {"QA Review","Undefined IRB Event"},
                {"Replacement PI","Undefined IRB Event"},
                {"Reportable Event","Undefined IRB Event"},
                {"Revised Letter","Undefined IRB Event"},
                {"SAE","Undefined IRB Event"},
                {"Study Closure/Expiration","Undefined IRB Event"},
                {"Study Enrollment Closure Report","Undefined IRB Event"},
                {"Unanticipated Problem","Undefined IRB Event"}
            };

        public static string getStatus(string key)
        {
            try
            {
                return eventsMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, string> eventsTypeMapBrany = new Dictionary<string, string>()
            {
                {"1572 Modification","Pre Activation"},
                {"Additional PI","Pre Activation"},
                {"Administrative (Translation)","Pre Activation"},
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
                {"Unanticipated Problem","Pre Activation"}
            };

        public static string getType(string key)
        {
            try
            {
                return eventsTypeMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                return key + "  (NEW !!!)";
            }
        }

    }


}
