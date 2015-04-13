using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace IrbAnalyser
{
    public static class OutputStatus
    {
        //List of newly created study, with more study detail
        public static DataTable status = new DataTable();

        /// <summary>
        /// Add the columns to the datatable
        /// </summary>
        public static void initiate()
        {
            if (status.Columns.Count == 0)
            {
                status.Columns.Add("TYPE", typeof(string));
                status.Columns.Add("IRB Agency name", typeof(string));
                status.Columns.Add("IRB no", typeof(string));

                status.Columns.Add("Organisation", typeof(string));
                status.Columns.Add("Local sample size", typeof(string));

                status.Columns.Add("Study status", typeof(string));
                status.Columns.Add("status type", typeof(string));
                status.Columns.Add("Documented by", typeof(string));
                status.Columns.Add("Status Valid From", typeof(string));
                status.Columns.Add("Status Valid Until", typeof(string));
                status.Columns.Add("Outcome", typeof(string));
            }
        }

        /// <summary>
        /// Add all status for new studies
        /// </summary>
        /// <param name="studyTable"></param>
        /// <param name="statusTable"></param>
        /// <param name="eventTable"></param>
        public static void addStatus(DataRow studyRow, DataTable statusTable, DataTable eventTable)
        {
            initiate();
            BranyStatusMap bsm = new BranyStatusMap();

            foreach (DataRow statusRow in statusTable.Rows)
            {
                if ((string)statusRow["IRBNumber"] == (string)studyRow["IRBNumber"] && (string)statusRow["IRBAgency"] == (string)studyRow["IRBAgency"])
                {
                    DataRow dr = status.NewRow();
                    dr["TYPE"] = "New study";
                    dr["IRB Agency name"] = statusRow["IRBAgency"];
                    dr["IRB no"] = statusRow["IRBNumber"];
                    dr["Organisation"] = statusRow["Sitename"];
                    dr["Local sample size"] = statusRow["SiteSampleSize"];
                    if (statusRow["IRBAgency"].ToString().ToLower() == "brany")
                    {
                        dr["Study status"] = bsm.statusMapBrany[statusRow["Status"].ToString()];
                        dr["status type"] = bsm.typeMapBrany[statusRow["Status"].ToString()];
                    }
                    //TODO MAP the IRIS status

                    dr["Documented by"] = "IRB interface";
                    dr["Status Valid From"] = statusRow["ValidOn"];
                    dr["Status Valid Until"] = statusRow["ValidUntill"];

                    status.Rows.Add(dr);
                }
            }

            if (studyRow["Initialapprovaldate"] != null)
            {
                if (!string.IsNullOrEmpty(studyRow["Initialapprovaldate"].ToString()))
                {
                    DataRow dr = status.NewRow();
                    dr["TYPE"] = "New study";
                    dr["IRB Agency name"] = studyRow["IRBAgency"];
                    dr["IRB no"] = studyRow["IRBNumber"];
                    dr["Organisation"] = studyRow["Sitename"];
                    dr["Local sample size"] = "0";
                    dr["Study status"] = "IRB Approved";
                    dr["status type"] = "Pre Activation";
                    dr["Documented by"] = "IRB interface";
                    dr["Status Valid From"] = studyRow["Mostrecentapprovaldate"];
                    dr["Status Valid Until"] = studyRow["Expirationdate"];

                    status.Rows.Add(dr);
                }
            }

            foreach (DataRow eventRow in eventTable.Rows)
            {
                if (eventRow["IRBNumber"] == studyRow["IRBNumber"] && eventRow["IRBAgency"] == studyRow["IRBAgency"])
                {
                    DataRow dr = status.NewRow();
                    if (((string)eventRow["event"]).ToLower().Contains("amendment"))
                    {
                        dr["TYPE"] = "New study";
                        dr["IRB Agency name"] = studyRow["IRBAgency"];
                        dr["IRB no"] = studyRow["IRBNumber"];
                        dr["Organisation"] = studyRow["Sitename"];
                        dr["Local sample size"] = "0";
                        dr["Study status"] = "IRB Amendment submitted";
                        dr["status type"] = "Pre Activation";
                        dr["Documented by"] = "IRB interface";
                        dr["Status Valid From"] = eventRow["EventCreationDate"];

                        dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["EventTaskCompletionDate"])
                            ? "Approved" : "";

                        dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["EventCompletionDate"])
                            && string.IsNullOrEmpty((string)eventRow["EventTaskCompletionDate"])
                            ? "Disapproved" : dr["Outcome"];

                        status.Rows.Add(dr);
                    }
                }
            }
        }

        public static void analyseStatus(DataTable studyTable, DataTable statusTable, DataTable eventTable)
        {

        }
    }

    public class BranyStatusMap
    {
        public StringDictionary statusMapBrany = new StringDictionary();
        //public StringDictionary statusMapVelos = new StringDictionary();
        public StringDictionary typeMapBrany = new StringDictionary();

        public BranyStatusMap()
        {
            statusMapBrany.Add("Approved", "IRB Approved");
            statusMapBrany.Add("Approved -IRB only", "IRB Approved");
            statusMapBrany.Add("Closed", "Complete");
            statusMapBrany.Add("Closed - IRB only", "Complete");
            statusMapBrany.Add("Closed to Enrollment", "Closed to accrual");
            statusMapBrany.Add("Closed to Enrollment -IRB only", "Closed to accrual");
            statusMapBrany.Add("Deferred", "IRB Deferred");
            statusMapBrany.Add("Deferred – IRB only", "IRB Deferred");
            statusMapBrany.Add("Disapproved", "IRB Disapproved - Returned to study team");
            statusMapBrany.Add("Emergency Use ONLY", "Undefined IRB Event");
            statusMapBrany.Add("Exempt", "IRB Exempt");
            statusMapBrany.Add("Exempt - Closed/Completed", "Complete");
            statusMapBrany.Add("Expired", "IRB Disaproved");
            statusMapBrany.Add("Not Engaged in Human Sub Rsrch", "IRB Exempt");
            statusMapBrany.Add("Other than Human Subject Rsrch", "IRB Exempt");
            statusMapBrany.Add("Pending Approval", "IRB INITIAL Submitted");
            statusMapBrany.Add("Pending Approval – IRB only", "IRB INITIAL Submitted");
            statusMapBrany.Add("Pending Closure", "Undefined IRB Event");
            statusMapBrany.Add("Pending closure – IRB only", "Undefined IRB Event");
            statusMapBrany.Add("Pending Review", "IRB INITIAL Submitted");
            statusMapBrany.Add("Pending Review – IRB only", "IRB INITIAL Submitted");
            statusMapBrany.Add("PENDING VETTING", "IRB INITIAL Submitted Draft - Record created");
            statusMapBrany.Add("Submission pending", "Draft - Record created");
            statusMapBrany.Add("Submission pending – IRB only", "Draft - Record created");
            statusMapBrany.Add("Suspended by entity other than IRB", "Temporarily Closed to Accrual and Intervention");
            statusMapBrany.Add("Suspended by entity other than IRB - IRB only", "Temporarily Closed to Accrual and Intervention");
            statusMapBrany.Add("Suspended by IRB", "Temporarily Closed to Accrual and Intervention");
            statusMapBrany.Add("Suspended by IRB - IRB only", "Temporarily Closed to Accrual and Intervention");
            statusMapBrany.Add("Terminated by IRB", "IRB Disapproved - Returned to study team");
            statusMapBrany.Add("Terminated by IRB – IRB only", "IRB Disapproved - Returned to study team");
            statusMapBrany.Add("Transferred to Another Record", "Undefined IRB Event");
            statusMapBrany.Add("Withdrawn by PI/Institution", "Withdrawn");
            statusMapBrany.Add("Withdrawn by Sponsor", "Withdrawn");


            typeMapBrany.Add("Approved", "Pre Activation");
            typeMapBrany.Add("Approved -IRB only", "Pre Activation");
            typeMapBrany.Add("Closed", "Study Status");
            typeMapBrany.Add("Closed - IRB only", "Study Status");
            typeMapBrany.Add("Closed to Enrollment", "Study Status");
            typeMapBrany.Add("Closed to Enrollment -IRB only", "Study Status");
            typeMapBrany.Add("Deferred", "Pre Activation");
            typeMapBrany.Add("Deferred – IRB only", "Pre Activation");
            typeMapBrany.Add("Disapproved", "Pre Activation");
            typeMapBrany.Add("Emergency Use ONLY", "Pre Activation");
            typeMapBrany.Add("Exempt", "Pre Activation");
            typeMapBrany.Add("Exempt - Closed/Completed", "Study Status");
            typeMapBrany.Add("Expired", "Pre Activation");
            typeMapBrany.Add("Not Engaged in Human Sub Rsrch", "Pre Activation");
            typeMapBrany.Add("Other than Human Subject Rsrch", "Pre Activation");
            typeMapBrany.Add("Pending Approval", "Pre Activation");
            typeMapBrany.Add("Pending Approval – IRB only", "Pre Activation");
            typeMapBrany.Add("Pending Closure", "Pre Activation");
            typeMapBrany.Add("Pending closure – IRB only", "Pre Activation");
            typeMapBrany.Add("Pending Review", "Pre Activation");
            typeMapBrany.Add("Pending Review – IRB only", "Pre Activation");
            typeMapBrany.Add("PENDING VETTING", "Pre Activation");
            typeMapBrany.Add("Submission pending", "Pre Activation");
            typeMapBrany.Add("Submission pending – IRB only", "Pre Activation");
            typeMapBrany.Add("Suspended by entity other than IRB", "Study Status");
            typeMapBrany.Add("Suspended by entity other than IRB - IRB only", "Study Status");
            typeMapBrany.Add("Suspended by IRB", "Study Status");
            typeMapBrany.Add("Suspended by IRB - IRB only", "Study Status");
            typeMapBrany.Add("Terminated by IRB", "Pre Activation");
            typeMapBrany.Add("Terminated by IRB – IRB only", "Pre Activation");
            typeMapBrany.Add("Transferred to Another Record", "Pre Activation");
            typeMapBrany.Add("Withdrawn by PI/Institution", "Study Status");
            typeMapBrany.Add("Withdrawn by Sponsor", "Study Status");


            /*foreach (DictionaryEntry de in statusMapBrany)
            {
                statusMapVelos.Add(de.Value.ToString(), de.Key.ToString());
            }*/
        }
    }
}
