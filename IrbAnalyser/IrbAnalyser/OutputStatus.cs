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
        private static void initiate()
        {
            if (status.Columns.Count == 0)
            {
                status.Columns.Add("TYPE", typeof(string));
                status.Columns.Add("IRB Agency name", typeof(string));
                status.Columns.Add("IRB no", typeof(string));
                status.Columns.Add("IRB Study ID", typeof(string));
                status.Columns.Add("Study name", typeof(string));

                status.Columns.Add("Organization", typeof(string));

                status.Columns.Add("Study status", typeof(string));
                status.Columns.Add("status type", typeof(string));
                status.Columns.Add("Comment", typeof(string));
                status.Columns.Add("Documented by", typeof(string));
                status.Columns.Add("Status Valid From", typeof(string));
                status.Columns.Add("Status Valid Until", typeof(string));
                status.Columns.Add("Outcome", typeof(string));
            }
        }




        /// <summary>
        /// Analyse status (only from status and event report, study report needs to be called in study row analysis to prevent looping through study twice.
        /// </summary>
        public static void analyse(string dir)
        {
            initiate();
            FileParser fpStatus = new FileParser(dir + "status.txt");
            FileParser fpEvent = new FileParser(dir + "event.txt");

            foreach (DataRow dr in fpStatus.data.Rows)
            {
                analyseRowStatus(dr);
            }

            foreach (DataRow dr in fpEvent.data.Rows)
            {
                analyseRowEvent(dr);
            }
        }

        /// <summary>
        /// Analyse the status row from the import file
        /// </summary>
        /// <param name="userRow"></param>
        private static void analyseRowStatus(DataRow statusRow)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = (string)statusRow["StudyId"];
                string irbagency = ((string)statusRow["IRBAgency"]).ToLower();
                string sitename = "";
                if ((string)statusRow["IRBAgency"] == "BRANY")
                {
                    sitename = BranySiteMap.siteMapBrany[(string)statusRow["Sitename"]];
                }

                if (!String.IsNullOrEmpty(irbstudyId) && !String.IsNullOrEmpty(irbagency) && !String.IsNullOrEmpty(sitename))
                {
                    string status = "";
                    DateTime start = DateTime.Now;
                    DateTime.TryParse((string)statusRow["ValidOn"], out start);
                    start = start == DateTime.MinValue ? DateTime.Now : start;
                    if (irbagency == "brany") status = BranyStatusMap.statusMapBrany[(string)statusRow["Status"]];
                    // todo einstein status map
                    //if (irbagency == "einstein") status = BranyStatusMap.statusMapBrany[(string)statusRow["StudyId"]];

                    var statusesDB = from stat in db.VDA_V_STUDYSTAT
                                     join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                     where stud.MORE_IRBSTUDYID == irbstudyId
                                  && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                     && stat.SSTAT_STUDY_STATUS == status
                                     && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                     && stat.SSTAT_SITE_NAME == sitename
                                     select stat;
                    if (!statusesDB.Any())
                    {
                        addRowStatus(statusRow, "New status");
                    }
                }
            }
        }

        /// <summary>
        /// Analyse the status row from the import file
        /// </summary>
        /// <param name="userRow"></param>
        private static void analyseRowEvent(DataRow eventRow)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = eventRow["StudyId"].ToString();
                string irbagency = eventRow["IRBAgency"].ToString().ToLower();
                string status = "IRB AMENDMENT Submitted**";
                string sitename = "";
                if ((string)eventRow["IRBAgency"] == "BRANY")
                {
                    sitename = BranySiteMap.siteMapBrany[(string)eventRow["Sitename"]];
                }
                if (!String.IsNullOrEmpty(irbstudyId) && !String.IsNullOrEmpty(irbagency) && !String.IsNullOrEmpty(sitename))
                {
                    DateTime start = DateTime.Now;
                    DateTime.TryParse((string)eventRow["EventCreationDate"], out start);
                    start = start == DateTime.MinValue ? DateTime.Now : start;

                    DateTime end = DateTime.MinValue;
                    DateTime.TryParse((string)eventRow["TaskCompletionDate"], out end);
                    if (end == DateTime.MinValue)
                        DateTime.TryParse((string)eventRow["EventCompletionDate"], out end);



                    var statusesDB = from stat in db.VDA_V_STUDYSTAT
                                     join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                     where stud.MORE_IRBSTUDYID == irbstudyId
                                  && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                     && stat.SSTAT_STUDY_STATUS == status
                                     && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                     && stat.SSTAT_SITE_NAME == sitename
                                     select stat;
                    if (!statusesDB.Any())
                    {
                        addRowEvent(eventRow, "New status");
                    }
                    else if ((end != DateTime.MinValue && statusesDB.FirstOrDefault().SSTAT_VALID_UNTIL == null)
                        || statusesDB.FirstOrDefault().SSTAT_VALID_UNTIL.Value.Date != end.Date)
                    {
                        addRowEvent(eventRow, "Modified status");
                    }
                }
            }
        }



        /// <summary>
        /// Analyse the study row for new IRB approval
        /// </summary>
        /// <param name="studyrow"></param>
        public static void analyseRowStudy(DataRow studyrow)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = studyrow["StudyId"].ToString();
                string irbagency = studyrow["IRBAgency"].ToString().ToLower();
                string status = "IRB Approved";
                string sitename = "";
                if ((string)studyrow["IRBAgency"] == "BRANY")
                {
                    sitename = BranySiteMap.siteMapBrany[(string)studyrow["Sitename"]];
                }

                DateTime start = DateTime.Now;
                DateTime.TryParse((string)studyrow["MostRecentApprovalDate"], out start);

                var statusesDB = from stat in db.VDA_V_STUDYSTAT
                                 join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                 where stud.MORE_IRBSTUDYID == irbstudyId
                              && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                 && stat.SSTAT_STUDY_STATUS == status
                                 && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                 && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                 && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                 && stat.SSTAT_SITE_NAME == sitename
                                 select stat;
                if (!statusesDB.Any() && start != DateTime.MinValue)
                {
                    addRowStudy(studyrow, "New status");
                }
                else if (start != DateTime.MinValue && statusesDB.Any())
                {
                    DateTime end = DateTime.Now;
                    DateTime.TryParse((string)studyrow["Expirationdate"], out end);
                    if ((statusesDB.FirstOrDefault().SSTAT_VALID_UNTIL == null && end != DateTime.MinValue)
                        || (statusesDB.FirstOrDefault().SSTAT_VALID_UNTIL.Value.Date != end.Date))
                    {
                        addRowStudy(studyrow, "Modified status");
                    }
                }
                //todo manage update
            }
        }


        /// <summary>
        /// Add new status from status report
        /// </summary>
        /// <param name="statusRow"></param>
        private static void addRowStatus(DataRow statusRow, string type)
        {
            DataRow dr = status.NewRow();
            dr["TYPE"] = type;
            dr["IRB Agency name"] = statusRow["IRBAgency"];
            dr["IRB no"] = statusRow["IRBNumber"];

            dr["IRB Study ID"] = (string)statusRow["StudyId"];
            dr["Study name"] = Tools.getStudyNumber((string)statusRow["StudyId"], (string)statusRow["IRBAgency"]);

            if (statusRow["IRBAgency"].ToString().ToLower() == "brany")
            {
                dr["Organization"] = BranySiteMap.siteMapBrany[(string)statusRow["Sitename"]];
                dr["Study status"] = BranyStatusMap.statusMapBrany[(string)statusRow["Status"]];
                dr["status type"] = BranyStatusMap.typeMapBrany[(string)statusRow["Status"]];
            }
            //TODO MAP the IRIS status

            dr["Comment"] = (string)statusRow["Status"];

            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = statusRow["ValidOn"];

            status.Rows.Add(dr);
        }


        /// <summary>
        /// Add new status from event report
        /// </summary>
        /// <param name="statusRow"></param>
        private static void addRowEvent(DataRow eventRow, string type)
        {
            DataRow dr = status.NewRow();
            dr["TYPE"] = type;
            dr["IRB Agency name"] = eventRow["IRBAgency"];
            dr["IRB no"] = eventRow["IRBNumber"];

            dr["IRB Study ID"] = (string)eventRow["StudyId"];
            dr["Study name"] = Tools.getStudyNumber((string)eventRow["StudyId"], (string)eventRow["IRBAgency"]);

            if (eventRow["IRBAgency"].ToString().ToLower() == "brany")
            {
                dr["Organization"] = BranySiteMap.siteMapBrany[(string)eventRow["Sitename"]];
            }

            dr["Study status"] = "IRB Amendment submitted**";
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = eventRow["EventCreationDate"];

            dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["TaskCompletionDate"])
                ? "Approved" : "";

            dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["EventCompletionDate"])
                && string.IsNullOrEmpty((string)eventRow["TaskCompletionDate"])
                ? "Disapproved" : dr["Outcome"];

            DateTime end = DateTime.MinValue;
            DateTime.TryParse((string)eventRow["TaskCompletionDate"], out end);
            if (end == DateTime.MinValue)
                DateTime.TryParse((string)eventRow["EventCompletionDate"], out end);
            if (end != DateTime.MinValue)
            {
                dr["Status Valid Until"] = end.Date.ToString("o");
            }

            status.Rows.Add(dr);
        }


        private static void addRowStudy(DataRow studyRow, string type)
        {
            DataRow dr = status.NewRow();
            dr["TYPE"] = type;
            dr["IRB Agency name"] = studyRow["IRBAgency"];
            dr["IRB no"] = studyRow["IRBNumber"];

            dr["IRB Study ID"] = (string)studyRow["StudyId"];
            dr["Study name"] = Tools.getStudyNumber((string)studyRow["StudyId"], (string)studyRow["IRBAgency"]);

            if (studyRow["IRBAgency"].ToString().ToLower() == "brany")
            {
                dr["Organization"] = BranySiteMap.siteMapBrany[(string)studyRow["Sitename"]];
            }

            dr["Study status"] = "IRB Approved";
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = studyRow["Mostrecentapprovaldate"];
            dr["Status Valid Until"] = studyRow["Expirationdate"];

            status.Rows.Add(dr);
        }

    }


}
