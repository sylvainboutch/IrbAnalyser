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
                updatedStatus.Columns.Add("TYPE", typeof(string));
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
            FileParser fpEvent = new FileParser(dir + "Event.txt", FileParser.type.Event);

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
            string sitename = "";
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                sitename = BranySiteMap.getSite(((string)statusRow["Sitename"]).Replace("(IBC)", ""));
            }

            DateTime start = DateTime.Now;
            DateTime.TryParse((string)statusRow["ValidOn"], out start);
            start = start == DateTime.MinValue ? DateTime.Now : start;
            string status = "";
            if (Agency.AgencyVal == Agency.AgencyList.BRANY) status = BranyStatusMap.getStatus((string)statusRow["Status"]);
            // todo einstein status map

            if (Tools.getOldStudy(irbstudyId))
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {


                    if (!String.IsNullOrEmpty(irbstudyId) && !String.IsNullOrEmpty(sitename))
                    {
                        var statusesDB = from stat in db.VDA_V_STUDYSTAT
                                         join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                         where stud.MORE_IRBSTUDYID == irbstudyId
                                      && stud.MORE_IRBAGENCY.ToLower().Trim().Contains(Agency.agencyStrLwr)
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

            string status = "";
            string type = "";
            string sitename = "";

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                status = BranyEventsMap.getStatus((string)eventRow["Event"]);
                type = BranyEventsMap.getType((string)eventRow["Event"]);
                sitename = BranySiteMap.getSite(((string)eventRow["Sitename"]).Replace("(IBC)", ""));
            }

            string status1 = "IRB AMENDMENT Submitted**";
            string status2 = "IRB Approved";

            DateTime start = DateTime.Now;
            DateTime.TryParse((string)eventRow["EventCreationDate"], out start);
            start = start == DateTime.MinValue ? DateTime.Now : start;

            DateTime end = DateTime.MinValue;
            DateTime.TryParse((string)eventRow["TaskCompletionDate"], out end);
            if (end == DateTime.MinValue)
                DateTime.TryParse((string)eventRow["EventCompletionDate"], out end);


            if (Tools.getOldStudy((string)eventRow["StudyId"]))
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {
                    if (status == status1)
                    {
                        if (end != DateTime.MinValue)
                        {
                            var statusesDB1 = from stat in db.VDA_V_STUDYSTAT
                                              join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                              where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                           && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                              && stat.SSTAT_STUDY_STATUS == status1
                                              && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                              && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                              && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                              && stat.SSTAT_SITE_NAME == sitename
                                              //&& stat.SSTAT_NOTES == ((string)eventRow["Event"]).Trim().ToLower()
                                              select stat;

                            if (statusesDB1.Any() && statusesDB1.First().SSTAT_VALID_UNTIL != end)
                            {
                                addRowEvent(eventRow, "IRB AMENDMENT Submitted**", "Modified status", false);
                            }

                            var statusesDB2 = from stat in db.VDA_V_STUDYSTAT
                                              join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                              where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                           && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                              && stat.SSTAT_STUDY_STATUS == status2
                                              && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                              && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                              && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                              && stat.SSTAT_SITE_NAME == sitename
                                              select stat;

                            if (!statusesDB2.Any())
                            {
                                addRowEvent(eventRow, "IRB Amendment Approved", "New status", true);
                            }
                            else if (statusesDB2.First().SSTAT_VALID_UNTIL != end)
                            {
                                addRowEvent(eventRow, "IRB Amendment Approved", "Modified status", false);
                            }
                        }
                    }
                    else
                    {
                        var statusesDB3 = from stat in db.VDA_V_STUDYSTAT
                                          join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                          where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                       && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                          && stat.SSTAT_STUDY_STATUS == status
                                          && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                          && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                          && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                          && stat.SSTAT_SITE_NAME == sitename
                                          //&& stat.SSTAT_NOTES == ((string)eventRow["Event"]).Trim().ToLower()
                                          select stat;
                        if (!statusesDB3.Any())
                        {
                            addRowEvent(eventRow, "Undefined IRB Event", "New status", true);
                        }
                        else if (statusesDB3.First().SSTAT_VALID_UNTIL != end)
                        {
                            addRowEvent(eventRow, "Undefined IRB Event", "Modified status", false);
                        }
                    }
                }
            }
            else
            {
                if (status == status1)
                {
                    addRowEvent(eventRow, "IRB AMENDMENT Submitted**", "New study", true);
                    if (end != DateTime.MinValue)
                    {
                        addRowEvent(eventRow, "IRB Amendment Approved", "New study", true);
                    }
                }
                else
                {
                    addRowEvent(eventRow, status, "New study", true);
                }

            }
        }

        /// <summary>
        /// Analyse the study row for new IRB approval
        /// </summary>
        /// <param name="studyrow"></param>
        public static void analyseRowStudy(DataRow studyrow, bool newrecord)
        {
            string irbstudyId = studyrow["StudyId"].ToString();

            string sitename = "";
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                sitename = BranySiteMap.getSite(((string)studyrow["Sitename"]).Replace("(IBC)", ""));
            }

            DateTime initial = DateTime.MinValue;
            DateTime.TryParse((string)studyrow["InitialApprovalDate"], out initial);

            DateTime renew = DateTime.MinValue;
            DateTime.TryParse((string)studyrow["MostRecentApprovalDate"], out initial);

            if (newrecord)
            {
                bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                  && st.Field<string>("IRB Agency name").Trim().ToLower() == Agency.agencyStrLwr
                                  && st.Field<string>("Study status").Trim().ToLower() == "irb approved"
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["InitialApprovalDate"]).Trim().ToLower()
                                  select st).Any();

                if (dtStatus && initial != DateTime.MinValue)
                {
                    addRowStudy(studyrow, "IRB Approved","New study", true);
                }

                dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                  && st.Field<string>("IRB Agency name").Trim().ToLower() == Agency.agencyStrLwr
                                  && st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved"
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower()
                                  select st).Any();

                if (dtStatus && renew != DateTime.MinValue)
                {
                    addRowStudy(studyrow, "IRB Renewal Approved", "New study", true);
                }

            }
            else
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {


                    var statusesDB = !(from stat in db.VDA_V_STUDYSTAT
                                       join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                       where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                  && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                     && stat.SSTAT_STUDY_STATUS == "irb approved"
                                     && stat.SSTAT_VALID_FROM.Value.Year == initial.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == initial.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == initial.Day
                                     && stat.SSTAT_SITE_NAME == sitename
                                       select stat).Any();

                    bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                      where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                      && st.Field<string>("IRB Agency name").Trim().ToLower() == Agency.agencyStrLwr
                                      && st.Field<string>("Study status").Trim().ToLower() == "irb approved"
                                      && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["InitialApprovalDate"]).Trim().ToLower()
                                      select st).Any();

                    if (initial != DateTime.MinValue && dtStatus)
                    {
                        addRowStudy(studyrow, "irb approved", "New status", true);
                    }


                    statusesDB = !(from stat in db.VDA_V_STUDYSTAT
                                       join stud in db.LCL_V_STUDYSUMM_PLUSMORE on stat.FK_STUDY equals stud.PK_STUDY
                                       where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                  && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                     && stat.SSTAT_STUDY_STATUS == "irb renewal approved"
                                     && stat.SSTAT_VALID_FROM.Value.Year == renew.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == renew.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == renew.Day
                                     && stat.SSTAT_SITE_NAME == sitename
                                       select stat).Any();

                    dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                      where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                      && st.Field<string>("IRB Agency name").Trim().ToLower() == Agency.agencyStrLwr
                                      && st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved"
                                      && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower()
                                      select st).Any();

                    if (renew != DateTime.MinValue && dtStatus)
                    {
                        addRowStudy(studyrow, "irb renewal approved", "New status", true);
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

            dr["Study number"] = Tools.getStudyNumber((string)statusRow["StudyId"], ((string)statusRow["IRBNumber"]).Replace("(IBC)", ""));

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
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

            dr["Study number"] = Tools.getStudyNumber((string)eventRow["StudyId"], ((string)eventRow["IRBNumber"]).Replace("(IBC)", ""));

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
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


        private static void addRowStudy(DataRow studyRow, string status, string type, bool newrecord)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            { dr = newStatus.NewRow(); }
            else
            { dr = updatedStatus.NewRow(); }
            dr["TYPE"] = type;
            dr["Comment"] = ((string)studyRow["IRBNumber"]).Contains("(IBC)") ? "Status from IBC" : "";
            dr["Study number"] = Tools.getStudyNumber((string)studyRow["StudyId"], ((string)studyRow["IRBNumber"]).Replace("(IBC)", ""));

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Organization"] = BranySiteMap.getSite(((string)studyRow["Sitename"]).Replace("(IBC)", ""));
            }

            dr["Study status"] = status;
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = Tools.parseDate((string)studyRow["MostRecentApprovalDate"]);
            dr["Status Valid From"] = (string)dr["Status Valid From"] == "" ? Tools.parseDate((string)studyRow["InitialApprovalDate"]) : "";
            dr["Status Valid Until"] = Tools.parseDate((string)studyRow["Expirationdate"]);

            if (newrecord)
            { newStatus.Rows.Add(dr); }
            else
            { updatedStatus.Rows.Add(dr); }
        }

    }


}
