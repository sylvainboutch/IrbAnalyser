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

                status.Columns.Add("Organisation", typeof(string));

                status.Columns.Add("Study status", typeof(string));
                status.Columns.Add("status type", typeof(string));
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
                string sitename = BranySiteMap.siteMapBrany[(string)statusRow["Sitename"]];
                string status = "";
                DateTime start = DateTime.Now;
                DateTime.TryParse((string)statusRow["ValidOn"], out start);
                start = start == DateTime.MinValue ? DateTime.Now : start;
                if (irbagency == "brany") status = BranyStatusMap.statusMapBrany[(string)statusRow["StudyId"]];
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
                //todo manage update

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
                string status = "IRB Amendment";
                string sitename = BranySiteMap.siteMapBrany[(string)eventRow["Sitename"]];

                DateTime start = DateTime.Now;
                DateTime.TryParse((string)eventRow["EventCreationDate"], out start);
                start = start == DateTime.MinValue ? DateTime.Now : start;

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
                //todo manage update
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
                string sitename = BranySiteMap.siteMapBrany[(string)studyrow["Sitename"]];

                DateTime start = DateTime.Now;
                DateTime.TryParse((string)studyrow["MostRecentApprovalDate"], out start);
                start = start == DateTime.MinValue ? DateTime.Now : start;

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
                    addRowStudy(studyrow, "New status");
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
            dr["Organisation"] = statusRow["Sitename"];
            if (statusRow["IRBAgency"].ToString().ToLower() == "brany")
            {
                dr["Study status"] = BranyStatusMap.statusMapBrany[statusRow["Status"].ToString()];
                dr["status type"] = BranyStatusMap.typeMapBrany[statusRow["Status"].ToString()];
            }
            //TODO MAP the IRIS status

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
            dr["Organisation"] = eventRow["Sitename"];
            dr["Study status"] = "IRB Amendment submitted";
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = eventRow["EventCreationDate"];

            dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["TaskCompletionDate"])
                ? "Approved" : "";

            dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["EventCompletionDate"])
                && string.IsNullOrEmpty((string)eventRow["TaskCompletionDate"])
                ? "Disapproved" : dr["Outcome"];

            status.Rows.Add(dr);
        }


        private static void addRowStudy(DataRow studyRow, string type)
        {
            DataRow dr = status.NewRow();
            dr["TYPE"] = type;
            dr["IRB Agency name"] = studyRow["IRBAgency"];
            dr["IRB no"] = studyRow["IRBNumber"];
            dr["Organisation"] = studyRow["Sitename"];
            dr["Study status"] = "IRB Approved";
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = studyRow["Mostrecentapprovaldate"];
            dr["Status Valid Until"] = studyRow["Expirationdate"];

            status.Rows.Add(dr);
        }

    }


}
