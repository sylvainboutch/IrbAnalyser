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


        private static IEnumerable<Model.VDA_V_STUDYSTAT> _allstatus;
        public static IEnumerable<Model.VDA_V_STUDYSTAT> allstatus
        {
            get
            {
                if (_allstatus == null || _allstatus.Count() == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        var query = (from st in db.VDA_V_STUDYSTAT
                                     where st.MORE_IRBAGENCY.ToLower().Trim() == Agency.agencyStrLwr
                                     && st.MORE_IRBSTUDYID != null
                                     select st);
                        _allstatus = query.ToList<Model.VDA_V_STUDYSTAT>();
                    }
                }
                return _allstatus;
            }
            set
            {
                _allstatus = value;
            }
        }


        /// <summary>
        /// Add the columns to the datatable
        /// </summary>
        private static void initiate()
        {
            if (newStatus.Columns.Count == 0)
            {
                newStatus.Columns.Add("TYPE", typeof(string));
                newStatus.Columns.Add("Study number", typeof(string));
                newStatus.Columns.Add("IRB Study ID", typeof(string));

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
                updatedStatus.Columns.Add("IRB Study ID", typeof(string));

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
            string status1 = "";
            if (Agency.AgencyVal == Agency.AgencyList.BRANY) status1 = BranyStatusMap.getStatus((string)statusRow["Status"]);
            // todo einstein status map

            if (Tools.getOldStudy(irbstudyId) && !String.IsNullOrEmpty(irbstudyId) && !String.IsNullOrEmpty(sitename) && status1 !="NA")
            {


                var statuses = from stat in allstatus
                               where stat.MORE_IRBSTUDYID == irbstudyId
                               && stat.SSTAT_STUDY_STATUS != null
                               && stat.SSTAT_NOTES != null
                               && stat.SSTAT_VALID_FROM != null
                               && stat.SSTAT_SITE_NAME != null
                               select stat;

                var statusesDB = from stat in statuses
                                 where stat.MORE_IRBSTUDYID == irbstudyId
                                 && stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status1.Trim().ToLower()
                                 && stat.SSTAT_NOTES.Trim().ToLower() == ((string)statusRow["Status"]).Trim().ToLower()
                                 && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                 && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                 && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                 && stat.SSTAT_SITE_NAME.Trim().ToLower() == sitename.Trim().ToLower()
                                 select stat;

                if (!statusesDB.Any())
                {
                    addRowStatus(statusRow, "New status", true);
                }
            }
            else if (status1 !="NA")
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
            string status2 = "IRB Initial Approved";

            DateTime start = DateTime.Now;
            DateTime.TryParse((string)eventRow["EventCreationDate"], out start);
            start = start == DateTime.MinValue ? DateTime.Now : start;

            DateTime end = DateTime.MinValue;
            //DateTime.TryParse((string)eventRow["TaskCompletionDate"], out end);
            //if (end == DateTime.MinValue)
            DateTime.TryParse((string)eventRow["EventCompletionDate"], out end);


            if (Tools.getOldStudy((string)eventRow["StudyId"]))
            {
                if (status == status1)
                {
                    var statusesDB1 = from stat in allstatus
                                      where stat.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                   && stat.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                      && stat.SSTAT_STUDY_STATUS == status1
                                      && stat.SSTAT_NOTES == (string)eventRow["Event"]
                                      && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                      && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                      && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                      && stat.SSTAT_SITE_NAME == sitename
                                      //&& stat.SSTAT_NOTES == ((string)eventRow["Event"]).Trim().ToLower()
                                      select stat;

                    if (!statusesDB1.Any())
                    {
                        addRowEvent(eventRow, status1, "New status", true);
                    }

                    if (end != DateTime.MinValue)
                    {
                        var statusesDB2 = from stat in allstatus
                                          where stat.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                       && stat.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                          && stat.SSTAT_STUDY_STATUS == status2
                                          && stat.SSTAT_NOTES == (string)eventRow["Event"]
                                          && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                          && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                          && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                          && stat.SSTAT_SITE_NAME == sitename
                                          select stat;

                        if (!statusesDB2.Any())
                        {
                            addRowEvent(eventRow, "IRB Amendment Approved", "New status", true);
                        }
                    }
                }
                else
                {
                    var statuses = from stat in allstatus
                                   where stat.MORE_IRBSTUDYID == irbstudyId
                                   && stat.SSTAT_STUDY_STATUS != null
                                   && stat.SSTAT_NOTES != null
                                   && stat.SSTAT_VALID_FROM != null
                                   && stat.SSTAT_SITE_NAME != null
                                   select stat;

                    var statusesDB3 = from stat in statuses
                                      where stat.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                      && stat.SSTAT_STUDY_STATUS.ToString().ToLower() == status.ToString().ToLower()
                                      && stat.SSTAT_NOTES.Trim().ToLower() == ((string)eventRow["Event"]).Trim().ToLower()
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
                                  && st.Field<string>("Study status").Trim().ToLower() == "irb initial approved"
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["InitialApprovalDate"]).Trim().ToLower()
                                  select st).Any();

                if (dtStatus && initial != DateTime.MinValue)
                {
                    addRowStudy(studyrow, "IRB Initial Approved", "New study", true, ((string)studyrow["InitialApprovalDate"]).Trim().ToLower());
                }

                dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                             where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                             && st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved"
                             && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower()
                             select st).Any();

                if (dtStatus && renew != DateTime.MinValue)
                {
                    addRowStudy(studyrow, "IRB Renewal Approved", "New study", true, ((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower());
                }

            }
            else
            {


                var statusesDB = !(from stat in allstatus
                                   where stat.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                              && stat.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                 && stat.SSTAT_STUDY_STATUS == "irb initial approved"
                                 && stat.SSTAT_VALID_FROM.Value.Year == initial.Year
                                 && stat.SSTAT_VALID_FROM.Value.Month == initial.Month
                                 && stat.SSTAT_VALID_FROM.Value.Day == initial.Day
                                 && stat.SSTAT_SITE_NAME == sitename
                                   select stat).Any();

                bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                  && st.Field<string>("Study status").Trim().ToLower() == "irb initial approved"
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["InitialApprovalDate"]).Trim().ToLower()
                                  select st).Any();

                if (initial != DateTime.MinValue && dtStatus)
                {
                    addRowStudy(studyrow, "Irb Initial Approved", "New status", true, ((string)studyrow["InitialApprovalDate"]).Trim().ToLower());
                }


                statusesDB = !(from stat in allstatus
                               where stat.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                          && stat.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                             && stat.SSTAT_STUDY_STATUS == "irb renewal approved"
                             && stat.SSTAT_VALID_FROM.Value.Year == renew.Year
                             && stat.SSTAT_VALID_FROM.Value.Month == renew.Month
                             && stat.SSTAT_VALID_FROM.Value.Day == renew.Day
                             && stat.SSTAT_SITE_NAME == sitename
                               select stat).Any();

                dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                             where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                             && st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved"
                             && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower()
                             select st).Any();

                if (renew != DateTime.MinValue && dtStatus)
                {
                    addRowStudy(studyrow, "irb renewal approved", "New status", true, ((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower());
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


        /// <summary>
        /// Add new status from status report
        /// </summary>
        /// <param name="statusRow"></param>
        private static void addRowStatus(DataRow statusRow, string type, bool newrecord)
        {
            DataRow dr;
            if (newrecord)
            { 
                dr = newStatus.NewRow();            
            }
            else
            { dr = updatedStatus.NewRow(); }

            dr["TYPE"] = type;

            dr["Study number"] = Tools.getStudyNumber((string)statusRow["StudyId"], ((string)statusRow["IRBNumber"]).Replace("(IBC)", ""));

            dr["IRB Study ID"] = (string)statusRow["StudyId"];

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
            { 
                bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                     where st.Field<string>("IRB Study ID").Trim().ToLower() == ((string)statusRow["StudyId"]).Trim().ToLower()
                     && st.Field<string>("Study status").Trim().ToLower() == ((string)dr["Study status"]).Trim().ToLower()
                     && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)dr["Status Valid From"]).Trim().ToLower()
                     && st.Field<string>("Comment").Trim().ToLower() == ((string)dr["Comment"]).Trim().ToLower()
                     select st).Any();
                if (dtStatus) newStatus.Rows.Add(dr);
            }
            else
            {
                bool dtStatus = !(from st in OutputStatus.updatedStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == ((string)statusRow["StudyId"]).Trim().ToLower()
                                  && st.Field<string>("Study status").Trim().ToLower() == ((string)dr["Study status"]).Trim().ToLower()
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)dr["Status Valid From"]).Trim().ToLower()
                                  && st.Field<string>("Comment").Trim().ToLower() == ((string)dr["Comment"]).Trim().ToLower()
                                  select st).Any();
                if (dtStatus) updatedStatus.Rows.Add(dr);
            }
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

            dr["IRB Study ID"] = (string)eventRow["StudyId"];

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
            {
                bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == ((string)eventRow["StudyId"]).Trim().ToLower()
                                  && st.Field<string>("Study status").Trim().ToLower() == ((string)dr["Study status"]).Trim().ToLower()
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)dr["Status Valid From"]).Trim().ToLower()
                                  && st.Field<string>("Comment").Trim().ToLower() == ((string)dr["Comment"]).Trim().ToLower()
                                  select st).Any();
                if (dtStatus) newStatus.Rows.Add(dr);
            }
            else
            {
                bool dtStatus = !(from st in OutputStatus.updatedStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == ((string)eventRow["StudyId"]).Trim().ToLower()
                                  && st.Field<string>("Study status").Trim().ToLower() == ((string)dr["Study status"]).Trim().ToLower()
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)dr["Status Valid From"]).Trim().ToLower()
                                  && st.Field<string>("Comment").Trim().ToLower() == ((string)dr["Comment"]).Trim().ToLower()
                                  select st).Any();
                if (dtStatus) updatedStatus.Rows.Add(dr);
            }
        }


        private static void addRowStudy(DataRow studyRow, string status, string type, bool newrecord, string startdate)
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

            dr["IRB Study ID"] = (string)studyRow["StudyId"];

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Organization"] = BranySiteMap.getSite(((string)studyRow["Sitename"]).Replace("(IBC)", ""));
            }

            dr["Study status"] = status;
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = startdate;
            /*dr["Status Valid From"] = Tools.parseDate((string)studyRow["MostRecentApprovalDate"]);
            dr["Status Valid From"] = (string)dr["Status Valid From"] == "" ? Tools.parseDate((string)studyRow["InitialApprovalDate"]) : "";*/
            dr["Status Valid Until"] = Tools.parseDate((string)studyRow["Expirationdate"]);

            if (newrecord)
            {
                bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == ((string)studyRow["StudyId"]).Trim().ToLower()
                                  && st.Field<string>("Study status").Trim().ToLower() == ((string)dr["Study status"]).Trim().ToLower()
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)dr["Status Valid From"]).Trim().ToLower()
                                  && st.Field<string>("Comment").Trim().ToLower() == ((string)dr["Comment"]).Trim().ToLower()
                                  select st).Any();
                if (dtStatus) newStatus.Rows.Add(dr);
            }
            else
            {
                bool dtStatus = !(from st in OutputStatus.updatedStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == ((string)studyRow["StudyId"]).Trim().ToLower()
                                  && st.Field<string>("Study status").Trim().ToLower() == ((string)dr["Study status"]).Trim().ToLower()
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)dr["Status Valid From"]).Trim().ToLower()
                                  && st.Field<string>("Comment").Trim().ToLower() == ((string)dr["Comment"]).Trim().ToLower()
                                  select st).Any();
                if (dtStatus) updatedStatus.Rows.Add(dr);
            }
        }

    }


}
