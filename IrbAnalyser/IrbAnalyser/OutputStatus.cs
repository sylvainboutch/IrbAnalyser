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
        public static DataTable newStatus = new DataTable();
        public static DataTable updatedStatus = new DataTable();

        /// <summary>
        /// Add the columns to the datatable
        /// </summary>
        private static void initiate()
        {
            if (newStatus.Columns.Count == 0)
            {
                newStatus.Columns.Add("TYPE", typeof(string));
                newStatus.Columns.Add("IRB Agency name", typeof(string));
                newStatus.Columns.Add("IRB no", typeof(string));
                newStatus.Columns.Add("IRB Study ID", typeof(string));
                newStatus.Columns.Add("Study number", typeof(string));

                newStatus.Columns.Add("Organization", typeof(string));

                newStatus.Columns.Add("Study status", typeof(string));
                newStatus.Columns.Add("status type", typeof(string));
                newStatus.Columns.Add("Comment", typeof(string));
                newStatus.Columns.Add("Documented by", typeof(string));
                newStatus.Columns.Add("Status Valid From", typeof(string));
                newStatus.Columns.Add("Status Valid Until", typeof(string));
                newStatus.Columns.Add("Outcome", typeof(string));
            }

            if (updatedStatus.Columns.Count == 0)
            {
                updatedStatus.Columns.Add("TYPE", typeof(string));
                updatedStatus.Columns.Add("IRB Agency name", typeof(string));
                updatedStatus.Columns.Add("IRB no", typeof(string));
                updatedStatus.Columns.Add("IRB Study ID", typeof(string));
                updatedStatus.Columns.Add("Study number", typeof(string));

                updatedStatus.Columns.Add("Organization", typeof(string));

                updatedStatus.Columns.Add("Study status", typeof(string));
                updatedStatus.Columns.Add("status type", typeof(string));
                updatedStatus.Columns.Add("Comment", typeof(string));
                updatedStatus.Columns.Add("Documented by", typeof(string));
                updatedStatus.Columns.Add("Status Valid From", typeof(string));
                updatedStatus.Columns.Add("Status Valid Until", typeof(string));
                updatedStatus.Columns.Add("Outcome", typeof(string));
            }
        }


        /// <summary>
        /// Analyse status (only from status and event report, study report needs to be called in study row analysis to prevent looping through study twice.
        /// </summary>
        public static void analyse(string dir)
        {
            initiate();
            FileParser fpStatus = new FileParser(dir + "Status.txt", FileParser.type.Status);
            FileParser fpEvent = new FileParser(dir + "Event.txt",FileParser.type.Event);

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
            string irbstudyId = (string)statusRow["StudyId"];
            string irbagency = ((string)statusRow["IRBAgency"]).ToLower();
            string sitename = "";
            if ((string)statusRow["IRBAgency"] == "BRANY")
            {
                sitename = BranySiteMap.getSite(((string)statusRow["Sitename"]).Replace("(IBC)", ""));
            }

            DateTime start = DateTime.Now;
            DateTime.TryParse((string)statusRow["ValidOn"], out start);
            start = start == DateTime.MinValue ? DateTime.Now : start;
            string status = "";
            if (irbagency == "brany") status = BranyStatusMap.getStatus((string)statusRow["Status"]);
            // todo einstein status map
            //if (irbagency == "einstein") status = BranyStatusMap.statusMapBrany[(string)statusRow["StudyId"]];

            if (Tools.getOldStudy(irbstudyId, irbagency))
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {


                    if (!String.IsNullOrEmpty(irbstudyId) && !String.IsNullOrEmpty(irbagency) && !String.IsNullOrEmpty(sitename))
                    {
                        var statusesDB = from stat in db.VDA_V_STUDYSTAT
                                         join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                         where stud.MORE_IRBSTUDYID == irbstudyId
                                      && stud.MORE_IRBAGENCY.ToLower().Trim().Contains(irbagency.ToLower().Trim())
                                         && stat.SSTAT_STUDY_STATUS == status
                                         && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                         && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                         && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                         && stat.SSTAT_SITE_NAME == sitename
                                         select stat;
                        if (!statusesDB.Any())
                        {
                            addRowStatus(statusRow, "New status", true);
                        }
                    }
                }
            }
            else
            {
                addRowStatus(statusRow, "New study", true);
            }
        }

        /// <summary>
        /// Analyse the status row from the import file
        /// </summary>
        /// <param name="userRow"></param>
        private static void analyseRowEvent(DataRow eventRow)
        {
            string irbstudyId = eventRow["StudyId"].ToString();
            string irbagency = eventRow["IRBAgency"].ToString().ToLower();

            string status = BranyEventsMap.getStatus((string)eventRow["Event"]);
            string type = BranyEventsMap.getType((string)eventRow["Event"]);

            string status1 = "IRB AMENDMENT Submitted**";
            string status2 = "IRB Approved";
            string sitename = "";
            if ((string)eventRow["IRBAgency"] == "BRANY")
            {
                sitename = BranySiteMap.getSite(((string)eventRow["Sitename"]).Replace("(IBC)", ""));
            }

            DateTime start = DateTime.Now;
            DateTime.TryParse((string)eventRow["EventCreationDate"], out start);
            start = start == DateTime.MinValue ? DateTime.Now : start;

            DateTime end = DateTime.MinValue;
            DateTime.TryParse((string)eventRow["TaskCompletionDate"], out end);
            /*if (end == DateTime.MinValue)
                DateTime.TryParse((string)eventRow["EventCompletionDate"], out end);*/

            if (Tools.getOldStudy((string)eventRow["StudyId"], (string)eventRow["IRBAgency"]))
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {

                    if (status == status1 && !String.IsNullOrEmpty(irbstudyId) && !String.IsNullOrEmpty(irbagency) && !String.IsNullOrEmpty(sitename))
                    {
                        var statusesDB1 = from stat in db.VDA_V_STUDYSTAT
                                          join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                          where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                       && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                          && stat.SSTAT_STUDY_STATUS == status1
                                          && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                          && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                          && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                          && stat.SSTAT_SITE_NAME == sitename
                                          select stat;



                        if (end != DateTime.MinValue)
                        {

                            var statusesDB2 = from stat in db.VDA_V_STUDYSTAT
                                              join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                              where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                           && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                              && stat.SSTAT_STUDY_STATUS == status2
                                              && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                              && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                              && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                              && stat.SSTAT_SITE_NAME == sitename
                                              select stat;
                            if (!statusesDB2.Any())
                            {
                                addRowEvent(eventRow,"IRB Approved","New Status",true);
                            }
                        }


                        if (!statusesDB1.Any())
                        {
                            addRowEvent(eventRow, "IRB AMENDMENT Submitted**","New status", true);
                        }
                        else if (end != DateTime.MinValue && statusesDB1.FirstOrDefault().SSTAT_VALID_UNTIL == null)
                        {
                            addRowEvent(eventRow,"IRB AMENDMENT Submitted**", "Modified status", false);
                        }
                        else if (statusesDB1.FirstOrDefault().SSTAT_VALID_UNTIL != null && statusesDB1.FirstOrDefault().SSTAT_VALID_UNTIL.Value.Date != end.Date)
                        {
                            addRowEvent(eventRow, "IRB AMENDMENT Submitted**", "Modified status", false);
                        }
                    }

                    else if (!String.IsNullOrEmpty(irbstudyId) && !String.IsNullOrEmpty(irbagency) && !String.IsNullOrEmpty(sitename))
                    {
                        var statusesDB1 = from stat in db.VDA_V_STUDYSTAT
                                          join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                          where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                       && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                          && stat.SSTAT_STUDY_STATUS == status
                                          && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                          && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                          && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                          && stat.SSTAT_SITE_NAME == sitename
                                          select stat;
                        if (!statusesDB1.Any())
                        {
                            addRowEvent(eventRow, status, "New status", true);
                        }
                        else if (end != DateTime.MinValue && statusesDB1.FirstOrDefault().SSTAT_VALID_UNTIL == null)
                        {
                            addRowEvent(eventRow, status, "Modified status", false);
                        }
                        else if (statusesDB1.FirstOrDefault().SSTAT_VALID_UNTIL != null && statusesDB1.FirstOrDefault().SSTAT_VALID_UNTIL.Value.Date != end.Date)
                        {
                            addRowEvent(eventRow, status, "Modified status", false);
                        }
                    }
                }

            }
            else if (status == status1)
            {
                addRowEvent(eventRow, "IRB AMENDMENT Submitted**", "New study", true);
                if (end != DateTime.MinValue)
                {
                    addRowEvent(eventRow, "IRB Approved", "New study", true);
                }
            }
            else
            {
                addRowEvent(eventRow, status, "New study", true);
            }
        }

        /// <summary>
        /// Analyse the study row for new IRB approval
        /// </summary>
        /// <param name="studyrow"></param>
        public static void analyseRowStudy(DataRow studyrow, bool newrecord)
        {
            string irbstudyId = studyrow["StudyId"].ToString();
            string irbagency = studyrow["IRBAgency"].ToString().ToLower();
            string status = "IRB Approved";
            string sitename = "";
            if ((string)studyrow["IRBAgency"] == "BRANY")
            {
                sitename = BranySiteMap.getSite(((string)studyrow["Sitename"]).Replace("(IBC)", ""));
            }

            DateTime start = DateTime.Now;
            DateTime.TryParse((string)studyrow["MostRecentApprovalDate"], out start);
            if (start == DateTime.MinValue)
                DateTime.TryParse((string)studyrow["InitialApprovalDate"], out start);

            if (newrecord)
            {
                bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                && st.Field<string>("IRB Agency name").Trim().ToLower() == irbagency.Trim().ToLower()
                                && st.Field<string>("Study status").Trim().ToLower() == status.Trim().ToLower()
                                && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["Mostrecentapprovaldate"]).Trim().ToLower()
                                select st).Any();
                if (dtStatus && start != DateTime.MinValue)
                {
                    addRowStudy(studyrow, "New study", true);
                }
            }
            else
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {


                    var statusesDB = !(from stat in db.VDA_V_STUDYSTAT
                                     join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                       where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                  && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                     && stat.SSTAT_STUDY_STATUS == status
                                     && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                     && stat.SSTAT_SITE_NAME == sitename
                                     select stat).Any();

                    bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                     where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                     && st.Field<string>("IRB Agency name").Trim().ToLower() == irbagency.Trim().ToLower()
                                     && st.Field<string>("Study status").Trim().ToLower() == status.Trim().ToLower()
                                     && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["Mostrecentapprovaldate"]).Trim().ToLower()
                                     select st).Any();

                    if (start != DateTime.MinValue && dtStatus)
                    {
                        addRowStudy(studyrow, "New status", true);
                    }
                    /*else if (start != DateTime.MinValue && statusesDB.Any())
                    {
                        DateTime end = DateTime.Now;
                        DateTime.TryParse((string)studyrow["Expirationdate"], out end);
                        if ((statusesDB.FirstOrDefault().SSTAT_VALID_UNTIL == null && end != DateTime.MinValue)
                            || (statusesDB.FirstOrDefault().SSTAT_VALID_UNTIL.Value.Date != end.Date))
                        {
                            addRowStudy(studyrow, "Modified status", false);
                        }
                    }*/
                }
            }
        }


        /// <summary>
        /// Add new status from status report
        /// </summary>
        /// <param name="statusRow"></param>
        private static void addRowStatus(DataRow statusRow, string type, bool newrecord)
        {
            DataRow dr;
            if (newrecord)
            { dr = newStatus.NewRow(); }
            else
            { dr = updatedStatus.NewRow(); }

            dr["TYPE"] = type;
            dr["IRB Agency name"] = statusRow["IRBAgency"];
            dr["IRB no"] = ((string)statusRow["IRBNumber"]).Replace("(IBC)", "");

            dr["IRB Study ID"] = (string)statusRow["StudyId"];
            dr["Study number"] = Tools.studyNumber((string)statusRow["StudyId"], (string)statusRow["IRBAgency"], (string)dr["IRB no"], "Please complete");

            if (statusRow["IRBAgency"].ToString().ToLower() == "brany")
            {
                dr["Organization"] = BranySiteMap.getSite(((string)statusRow["Sitename"]).Replace("(IBC)", ""));
                dr["Study status"] = BranyStatusMap.getStatus((string)statusRow["Status"]);
                dr["status type"] = BranyStatusMap.getType((string)statusRow["Status"]);
            }
            //TODO MAP the IRIS status
            dr["Comment"] = (string)statusRow["Status"];

            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = Tools.parseDate((string)statusRow["ValidOn"]);

            if (newrecord)
            { newStatus.Rows.Add(dr); }
            else
            { updatedStatus.Rows.Add(dr); }
        }


        /// <summary>
        /// Add new status from event report
        /// </summary>
        /// <param name="statusRow"></param>
        private static void addRowEvent(DataRow eventRow, string status, string type, bool newrecord)
        {
            DataRow dr;
            if (newrecord)
            { dr = newStatus.NewRow(); }
            else
            { dr = updatedStatus.NewRow(); }

            dr["TYPE"] = type;
            dr["IRB Agency name"] = eventRow["IRBAgency"];
            dr["IRB no"] = ((string)eventRow["IRBNumber"]).Replace("(IBC)", "");

            dr["IRB Study ID"] = (string)eventRow["StudyId"];
            dr["Study number"] = Tools.studyNumber((string)eventRow["StudyId"], (string)eventRow["IRBAgency"], (string)dr["IRB no"], "Please complete");

            if (eventRow["IRBAgency"].ToString().ToLower() == "brany")
            {
                dr["Organization"] = BranySiteMap.getSite(((string)eventRow["Sitename"]).Replace("(IBC)", ""));
            }

            dr["Study status"] = status;
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = Tools.parseDate((string)eventRow["EventCreationDate"]);
            dr["Comment"] = (string)eventRow["Event"];
            //Change to rule, now we create a new IRB Approved status
            dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["TaskCompletionDate"])
                ? "Approved" : "";

            /*dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["EventCompletionDate"])
                && string.IsNullOrEmpty((string)eventRow["TaskCompletionDate"])
                ? "Disapproved" : dr["Outcome"];*/

            DateTime end = DateTime.MinValue;
            DateTime.TryParse((string)eventRow["TaskCompletionDate"], out end);
            if (end == DateTime.MinValue)
                DateTime.TryParse((string)eventRow["EventCompletionDate"], out end);
            if (end != DateTime.MinValue)
            {
                dr["Status Valid Until"] = Tools.parseDate((string)end.Date.ToString("o"));
            }

            if (newrecord)
            { newStatus.Rows.Add(dr); }
            else
            { updatedStatus.Rows.Add(dr); }
        }


        private static void addRowStudy(DataRow studyRow, string type, bool newrecord)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            { dr = newStatus.NewRow(); }
            else
            { dr = updatedStatus.NewRow(); }
            dr["TYPE"] = type;
            dr["IRB Agency name"] = studyRow["IRBAgency"];
            dr["IRB no"] = ((string)studyRow["IRBNumber"]).Replace("(IBC)", "");
            dr["Comment"] = ((string)studyRow["IRBNumber"]).Contains("(IBC)") ? "Status from IBC" : "";

            dr["IRB Study ID"] = (string)studyRow["StudyId"];
            dr["Study number"] = Tools.studyNumber((string)studyRow["StudyId"], (string)studyRow["IRBAgency"], (string)dr["IRB no"], "Please complete");

            if (studyRow["IRBAgency"].ToString().ToLower() == "brany")
            {
                dr["Organization"] = BranySiteMap.getSite(((string)studyRow["Sitename"]).Replace("(IBC)", ""));
            }

            dr["Study status"] = "IRB Approved";
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = Tools.parseDate((string)studyRow["MostRecentApprovalDate"]);
            dr["Status Valid From"] = dr["Status Valid From"] == "" ? Tools.parseDate((string)studyRow["InitialApprovalDate"]) : "";
            dr["Status Valid Until"] = Tools.parseDate((string)studyRow["Expirationdate"]);

            if (newrecord)
            { newStatus.Rows.Add(dr); }
            else
            { updatedStatus.Rows.Add(dr); }
        }

    }


}
