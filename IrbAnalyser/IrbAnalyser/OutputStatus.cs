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

                        IQueryable<Model.VDA_V_STUDYSTAT> query;
                        //BRANY look up agency in MSD
                        if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                        {
                            query = (from st in db.VDA_V_STUDYSTAT
                                     where st.MORE_IRBAGENCY != null
                                     && st.IRBIDENTIFIERS != null
                                     select st);
                        }
                        else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                        {
                            query = (from st in db.VDA_V_STUDYSTAT
                                     where st.MORE_IRBAGENCY.ToLower().Trim() == Agency.agencyStrLwr
                                     && st.IRBIDENTIFIERS != null
                                     select st);
                        }
                        //IRIS all other agency in MSD, non IRB studies wont have 
                        else
                        {
                            query = (from st in db.VDA_V_STUDYSTAT
                                     where st.MORE_IRBAGENCY.ToLower().Trim() != Agency.brany
                                     && st.IRBIDENTIFIERS != null
                                     select st);
                        }


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

        private static FileParser _fpstatus;
        public static FileParser fpstatus
        {
            get
            {
                if (_fpstatus == null || _fpstatus.data == null || _fpstatus.data.Rows.Count == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        _fpstatus = new FileParser(Tools.filename + "Status.txt", FileParser.type.Status);
                        DataView dv = _fpstatus.data.DefaultView;
                        dv.Sort = "StudyId desc, ValidOn asc";
                        _fpstatus.data = dv.ToTable();
                    }
                }
                return _fpstatus;
            }
            set
            {
                _fpstatus = value;
                DataView dv = _fpstatus.data.DefaultView;
                dv.Sort = "StudyId desc, ValidOn asc";
                _fpstatus.data = dv.ToTable();
            }
        }


        private static FileParser _fpevent;
        public static FileParser fpevent
        {
            get
            {
                if (_fpevent == null || _fpevent.data == null || _fpevent.data.Rows.Count == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        _fpevent = new FileParser(Tools.filename + "Event.txt", FileParser.type.Event);
                    }
                }
                return _fpevent;
            }
            set
            {
                _fpevent = value;
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
                newStatus.Columns.Add("Study_number", typeof(string));
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
                updatedStatus.Columns.Add("Study_number", typeof(string));
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

            foreach (DataRow dr in fpstatus.data.Rows)
            {
                analyseRowStatus(dr);
            }

            foreach (DataRow dr in fpevent.data.Rows)
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
            int abc = 0;
            bool isIRIS = Int32.TryParse(irbstudyId, out abc);
            if (irbstudyId.Contains("-"))
                Agency.AgencyVal = Agency.AgencyList.BRANY;
            else if (isIRIS && abc != 0)
                Agency.AgencyVal = Agency.AgencyList.EINSTEIN;
            else Agency.AgencyVal = Agency.AgencySetupVal;

            if (OutputStudy.shouldStudyBeAdded(irbstudyId))
            {
                string sitename = "";
                if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                    sitename = OutputSite.EMmainsite;
                else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    sitename = BranySiteMap.getSite(((string)statusRow["Sitename"]).Replace("(IBC)", ""));
                }
                else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                {
                    sitename = IRISMap.SiteMap.getSite((string)statusRow["Sitename"]);
                }

                DateTime start = DateTime.Now;
                DateTime.TryParse((string)statusRow["ValidOn"], out start);
                start = start == DateTime.MinValue ? DateTime.Now : start;
                string status1 = "";
                if (Agency.AgencyVal == Agency.AgencyList.BRANY) status1 = BranyStatusMap.getStatus((string)statusRow["Status"]);
                else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN) status1 = IRISMap.StatusMap.getStatus((string)statusRow["Status"]);
                // todo einstein status map


                /*if (string.IsNullOrEmpty(status1))
                {
                    addRowStatus(statusRow, "", true);
                }
                else
                {*/
                if (Tools.getOldStudy(irbstudyId) && !String.IsNullOrEmpty(irbstudyId) && !String.IsNullOrEmpty(sitename) && status1 != "NA" && status1 != "")
                {


                    var statuses = from stat in allstatus
                                   where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                   && stat.SSTAT_STUDY_STATUS != null
                                   && stat.SSTAT_VALID_FROM != null
                                   && stat.SSTAT_SITE_NAME != null
                                   select stat;

                    var statusesDB = from stat in statuses
                                     where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                     && stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status1.Trim().ToLower()
                                     && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                     && stat.SSTAT_SITE_NAME.Trim().ToLower() == sitename.Trim().ToLower()
                                     select stat;

                    var statusFP = !(from st in OutputStatus.newStatus.AsEnumerable()
                                     where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                     && st.Field<string>("Study status").Trim().ToLower() == status1.Trim().ToLower()
                                     && st.Field<string>("Status Valid From").Trim().ToLower() == Tools.parseDate((string)statusRow["ValidOn"]).Trim().ToLower()
                                     select st).Any();

                    if (!statusesDB.Any() && statusFP)
                    {
                        addRowStatus(statusRow, "New status", true);
                    }
                }
                else if (status1 != "NA" && status1 != "")
                {
                    addRowStatus(statusRow, "New study", true);
                }
                //}
            }
        }

        /// <summary>
        /// Analyse the status row from the import file
        /// </summary>
        /// <param name="userRow"></param>
        private static void analyseRowEvent(DataRow eventRow)
        {
            string irbstudyId = eventRow["StudyId"].ToString();
            int abc = 0;
            bool isIRIS = Int32.TryParse(irbstudyId, out abc);
            if (irbstudyId.Contains("-"))
                Agency.AgencyVal = Agency.AgencyList.BRANY;
            else if (isIRIS && abc != 0)
                Agency.AgencyVal = Agency.AgencyList.EINSTEIN;
            else Agency.AgencyVal = Agency.AgencySetupVal;


            if (OutputStudy.shouldStudyBeAdded(irbstudyId))
            {
                string status = "";
                string type = "";
                string sitename = "";
                string notes = "";

                if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    status = BranyEventsMap.getStatus((string)eventRow["Event"]);
                    type = BranyEventsMap.getType((string)eventRow["Event"]);
                    if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                        sitename = OutputSite.EMmainsite;
                    else
                        sitename = BranySiteMap.getSite(((string)eventRow["Sitename"]).Replace("(IBC)", ""));
                    notes = (string)eventRow["Event"];
                    if (BranyEventsMap.teamChangedEvents.Contains((string)eventRow["Event"]))
                    {
                        NewValueOuput.appendString("Study personnel changed - event name : ", (string)eventRow["Event"]);
                    }
                }

                else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                {
                    if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                        sitename = OutputSite.EMmainsite;
                    else
                        sitename = IRISMap.SiteMap.getSite(((string)eventRow["Sitename"]).Replace("(IBC)", ""));
                    status = IRISMap.EventsMap.getStatus((string)eventRow["Event"]);
                    type = IRISMap.EventsMap.getType((string)eventRow["Event"]);
                    notes = Tools.removeHtml((string)eventRow["Amendment"]);
                }

                if (string.IsNullOrEmpty(status))
                {
                    addRowEvent(eventRow, "", "New status", true, (string)eventRow["EventCreationDate"]);
                }
                else
                {

                    string status1 = "IRB Amendment Submitted**";
                    string status2 = "IRB Amendment Approved**";

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
                            IEnumerable<Model.VDA_V_STUDYSTAT> statusesDB1;
                            //BRANY look up agency in MSD
                            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                            {
                                statusesDB1 = from stat in allstatus
                                              where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                           && stat.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                              && stat.SSTAT_STUDY_STATUS == status1
                                              && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                              && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                              && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                              //&& stat.SSTAT_NOTES == ((string)eventRow["Event"]).Trim().ToLower()
                                              select stat;
                            }
                            //IRIS all other agency in MSD, non IRB studies wont have 
                            else
                            {
                                statusesDB1 = from stat in allstatus
                                              where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                           && stat.MORE_IRBAGENCY.ToLower() != Agency.brany
                                              && stat.SSTAT_STUDY_STATUS == status1
                                              && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                              && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                              && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                              //&& stat.SSTAT_NOTES == ((string)eventRow["Event"]).Trim().ToLower()
                                              select stat;
                            }

                            if (!statusesDB1.Any())
                            {
                                addRowEvent(eventRow, status1, "New status", true, start.ToString());
                            }

                            if (end != DateTime.MinValue)
                            {
                                IEnumerable<Model.VDA_V_STUDYSTAT> statusesDB2;
                                //BRANY look up agency in MSD
                                if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                                {
                                    statusesDB2 = from stat in allstatus
                                                  where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                               && stat.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                                  && stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status2.Trim().ToLower()
                                                  && stat.SSTAT_VALID_FROM.Value.Year == end.Year
                                                  && stat.SSTAT_VALID_FROM.Value.Month == end.Month
                                                  && stat.SSTAT_VALID_FROM.Value.Day == end.Day
                                                  select stat;
                                }
                                //IRIS all other agency in MSD, non IRB studies wont have 
                                else
                                {
                                    statusesDB2 = from stat in allstatus
                                                  where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                               && stat.MORE_IRBAGENCY.ToLower() != Agency.brany
                                                  && stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status2.Trim().ToLower()
                                                  && stat.SSTAT_VALID_FROM.Value.Year == end.Year
                                                  && stat.SSTAT_VALID_FROM.Value.Month == end.Month
                                                  && stat.SSTAT_VALID_FROM.Value.Day == end.Day
                                                  select stat;
                                }

                                if (!statusesDB2.Any())
                                {
                                    addRowEvent(eventRow, "IRB Amendment Approved**", "New status", true, end.ToString());
                                }
                            }
                        }
                        else
                        {
                            addRowEvent(eventRow, "", "New status", true, (string)eventRow["EventCreationDate"]);
                        }

                    }
                    else
                    {
                        if (status == status1)
                        {
                            addRowEvent(eventRow, "IRB Amendment Submitted**", "New study", true, (string)eventRow["EventCreationDate"]);
                            if (end != DateTime.MinValue)
                            {
                                addRowEvent(eventRow, "IRB Amendment Approved**", "New study", true, end.ToString());
                            }
                        }
                        else
                        {
                            addRowEvent(eventRow, status, "New study", true, (string)eventRow["EventCreationDate"]);
                        }

                    }
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
            if (OutputStudy.shouldStudyBeAdded(irbstudyId))
            {
                string sitename = "";
                if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                {
                    sitename = OutputSite.EMmainsite;
                }
                else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    sitename = BranySiteMap.getSite(((string)studyrow["Sitename"]).Replace("(IBC)", ""));
                }
                else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                {
                    sitename = IRISMap.SiteMap.getSite((string)studyrow["Sitename"]);
                }

                DateTime initial = DateTime.MinValue;
                DateTime.TryParse((string)studyrow["InitialApprovalDate"], out initial);

                DateTime renew = DateTime.MinValue;
                DateTime.TryParse((string)studyrow["MostRecentApprovalDate"], out renew);

                if (newrecord)
                {
                    if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                    {

                        var status = (from st in fpstatus.data.AsEnumerable()
                                      where irbstudyId.Trim().ToLower() == st.Field<string>("StudyId").Trim().ToLower()
                                     && !st.Field<string>("IRBNumber").Trim().ToLower().Contains("ibc")
                                      select st);

                        //status.Columns.Add("parsedDate", typeof(DateTime));
                        if (status.Any())
                        {
                            string latestStatus = (string)status.First().Field<string>("Status");
                            DateTime date1 = DateTime.MinValue;
                            DateTime date2 = DateTime.MinValue;
                            foreach (var row in status)
                            {
                                DateTime.TryParse((string)row.Field<string>("ValidOn"), out date2);
                                if (date2 > date1)
                                {
                                    latestStatus = (string)row.Field<string>("Status");
                                    date1 = date2;
                                }
                            }

                            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                            {
                                latestStatus = BranyStatusMap.getStatus(latestStatus);
                            }
                            else
                            {
                                latestStatus = IRISMap.StatusMap.getStatus(latestStatus);
                            }

                            if (latestStatus != (string)studyrow["Status"])
                            {
                                addRowStudy(studyrow, (string)studyrow["Status"], "New study", true, Tools.parseDate(DateTime.Now.ToString()));
                            }
                        }
                        else
                        {
                            addRowStudy(studyrow, (string)studyrow["Status"], "New study", true, Tools.parseDate(DateTime.Now.ToString()));
                        }

                    }

                    bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                      where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                      && st.Field<string>("Study status").Trim().ToLower() == "irb initial approved"
                                      //&& st.Field<string>("Status Valid From").Trim().ToLower() == Tools.parseDate(((string)studyrow["InitialApprovalDate"])).Trim().ToLower()
                                      select st).Any();

                    if (dtStatus && initial != DateTime.MinValue)
                    {
                        addRowStudy(studyrow, "IRB Initial Approved", "New study", true, Tools.parseDate((string)studyrow["InitialApprovalDate"]).Trim().ToLower());
                    }

                    dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                 where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                 && st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved**"
                                 && st.Field<string>("Status Valid From").Trim().ToLower() == Tools.parseDate((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower()
                                 select st).Any();

                    if (dtStatus && renew != DateTime.MinValue && renew.Date != initial.Date)
                    {
                        addRowStudy(studyrow, "IRB Renewal Approved**", "New study", true, Tools.parseDate((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower());
                    }

                    if (SpecialStudys.closedStudys.Any(x => x.IRB == Agency.agencyStrLwr && Tools.compareStr(x.number, (string)studyrow["IRBNumber"])))
                    {
                        addRowStudy(studyrow, "Complete", "New study", true, Tools.parseDate(DateTime.Now.ToString()));
                    }


                }
                else
                {

                    bool isNotStatusDb;
                    //BRANY look up agency in MSD
                    if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                    {
                        isNotStatusDb = !(from stat in allstatus
                                          where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                  && stat.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                     && (stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb renewal approved**"
                                     || stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb initial approved")
                                          select stat).Any();
                    }
                    //IRIS all other agency in MSD, non IRB studies wont have 
                    else
                    {
                        isNotStatusDb = !(from stat in allstatus
                                          where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                  && stat.MORE_IRBAGENCY.ToLower() != Agency.brany
                                     && (stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb renewal approved**"
                                     || stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb initial approved")
                                          select stat).Any();
                    }

                    bool isNotStatusDt = !(from st in OutputStatus.newStatus.AsEnumerable()
                                           where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                           && (st.Field<string>("Study status").Trim().ToLower() == "irb initial approved"
                                           || st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved**")
                                           select st).Any();

                    if (initial != DateTime.MinValue && isNotStatusDt && isNotStatusDb)
                    {
                        addRowStudy(studyrow, "IRB Initial Approved", "New status", true, Tools.parseDate((string)studyrow["InitialApprovalDate"]).Trim().ToLower());
                    }


                    //BRANY look up agency in MSD
                    if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                    {

                        isNotStatusDb = !(from stat in allstatus
                                          where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                  && stat.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                     && (stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb renewal approved**"
                                     || stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb initial approved")
                                     && stat.SSTAT_VALID_FROM.Value.Year == renew.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == renew.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == renew.Day
                                          select stat).Any();
                    }
                    //IRIS all other agency in MSD, non IRB studies wont have 
                    else
                    {

                        isNotStatusDb = !(from stat in allstatus
                                          where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                  && stat.MORE_IRBAGENCY.ToLower() != Agency.brany
                                     && (stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb renewal approved**"
                                     || stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb initial approved")
                                     && stat.SSTAT_VALID_FROM.Value.Year == renew.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == renew.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == renew.Day
                                          select stat).Any();
                    }


                    isNotStatusDt = !(from st in OutputStatus.newStatus.AsEnumerable()
                                      where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                      && (st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved**"
                                      || st.Field<string>("Study status").Trim().ToLower() == "irb initial approved")
                                      && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower()
                                      select st).Any();

                    if (renew != DateTime.MinValue && isNotStatusDt && isNotStatusDb)
                    {
                        addRowStudy(studyrow, "IRB Renewal Approved**", "New status", true, Tools.parseDate((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower());
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
            string status = "";
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                status = BranyStatusMap.getStatus((string)statusRow["Status"]);
            }

            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                status = IRISMap.StatusMap.getStatus((string)statusRow["Status"]);
            }

            if (string.IsNullOrEmpty(status))
            {
                string undefinedEvent = "Organization : " + statusRow["Sitename"] + " - Status : " + statusRow["Status"] + " - Date : " + statusRow["ValidOn"];
                bool alreadyAdded = (from events in OutputIRBForm.newIRBForm.AsEnumerable()
                                     where events.Field<string>("IRB_Status").Contains(undefinedEvent)
                                     & events.Field<string>("Study_number").ToLower().Trim().Contains(((string)statusRow["StudyId"]).Trim().ToLower())
                                     select events).Any();
                if (!alreadyAdded)
                {

                    OutputIRBForm.addStatus(Tools.getStudyNumber((string)statusRow["StudyId"], ((string)statusRow["IRBNumber"]).Replace("(IBC)", "")), undefinedEvent);
                }
            }
            else
            {

                DataRow dr;
                if (newrecord)
                {
                    dr = newStatus.NewRow();
                }
                else
                {
                    dr = updatedStatus.NewRow();
                }

                dr["TYPE"] = type;

                dr["Study_number"] = Tools.getStudyNumber((string)statusRow["StudyId"], ((string)statusRow["IRBNumber"]).Replace("(IBC)", ""));

                dr["IRB Study ID"] = (string)statusRow["StudyId"];
                if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                {
                    if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                    {
                        dr["Organization"] = OutputSite.EMmainsite;
                        dr["Study status"] = BranyStatusMap.getStatus((string)statusRow["Status"]);
                        dr["status type"] = BranyStatusMap.getType((string)statusRow["Status"]);
                    }

                    else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                    {
                        dr["Organization"] = OutputSite.EMmainsite;
                        dr["Study status"] = IRISMap.StatusMap.getStatus((string)statusRow["Status"]);
                        dr["status type"] = IRISMap.StatusMap.getType((string)statusRow["Status"]);
                    }
                }
                else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    dr["Organization"] = BranySiteMap.getSite(((string)statusRow["Sitename"]).Replace("(IBC)", ""));
                    dr["Study status"] = BranyStatusMap.getStatus((string)statusRow["Status"]);
                    dr["status type"] = BranyStatusMap.getType((string)statusRow["Status"]);
                }

                else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                {
                    dr["Organization"] = IRISMap.SiteMap.getSite((string)statusRow["Sitename"]);
                    dr["Study status"] = IRISMap.StatusMap.getStatus((string)statusRow["Status"]);
                    dr["status type"] = IRISMap.StatusMap.getType((string)statusRow["Status"]);
                }
                //TODO MAP the IRIS status
                dr["Comment"] = (string)statusRow["Status"] + " " + (string)statusRow["Comments"];

                dr["Documented by"] = "IRB interface";
                dr["Status Valid From"] = Tools.parseDate((string)statusRow["ValidOn"]);


                if (fpstatus.initColumnCount < statusRow.Table.Columns.Count)
                {
                    for (int i = fpstatus.initColumnCount; i < statusRow.Table.Columns.Count; i++)
                    {
                        if (!dr.Table.Columns.Contains(statusRow.Table.Columns[i].ColumnName))
                        {
                            dr.Table.Columns.Add(statusRow.Table.Columns[i].ColumnName);
                        }
                        dr[statusRow.Table.Columns[i].ColumnName] = statusRow[i];
                    }
                }

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
        }


        /// <summary>
        /// Add new status from event report
        /// </summary>
        /// <param name="statusRow"></param>
        private static void addRowEvent(DataRow eventRow, string status, string type, bool newrecord, string date)
        {
            DataRow dr;
            if (newrecord)
            { dr = newStatus.NewRow(); }
            else
            { dr = updatedStatus.NewRow(); }

            dr["TYPE"] = type;

            dr["Study_number"] = Tools.getStudyNumber((string)eventRow["StudyId"], ((string)eventRow["IRBNumber"]).Replace("(IBC)", ""));

            dr["IRB Study ID"] = (string)eventRow["StudyId"];

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Organization"] = OutputSite.EMmainsite;
            }
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Organization"] = BranySiteMap.getSite(((string)eventRow["Sitename"]).Replace("(IBC)", ""));
            }

            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                dr["Organization"] = IRISMap.SiteMap.getSite((string)eventRow["Sitename"]);
            }

            dr["Study status"] = status;
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = Tools.parseDate(date);


            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Comment"] = (string)eventRow["Event"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                dr["Comment"] = Tools.removeHtml((string)eventRow["Amendment"]);
            }
            //Change to rule, now we create a new IRB Approved status
            dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["TaskCompletionDate"])
                ? "Approved" : "";

            /*dr["Outcome"] = !string.IsNullOrEmpty((string)eventRow["EventCompletionDate"])
                && string.IsNullOrEmpty((string)eventRow["TaskCompletionDate"])
                ? "Disapproved" : dr["Outcome"];*/

            /*We don't want to have this anymore
             * DateTime end = DateTime.MinValue;
            DateTime.TryParse((string)eventRow["TaskCompletionDate"], out end);
            if (end == DateTime.MinValue)
                DateTime.TryParse((string)eventRow["EventCompletionDate"], out end);
            if (end != DateTime.MinValue)
            {
                dr["Status Valid Until"] = Tools.parseDate((string)end.Date.ToString("o"));
            }*/

            if (fpevent.initColumnCount < eventRow.Table.Columns.Count)
            {
                for (int i = fpevent.initColumnCount; i < eventRow.Table.Columns.Count; i++)
                {
                    if (!dr.Table.Columns.Contains(eventRow.Table.Columns[i].ColumnName))
                    {
                        dr.Table.Columns.Add(eventRow.Table.Columns[i].ColumnName);
                    }
                    dr[eventRow.Table.Columns[i].ColumnName] = eventRow[i];
                }
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                string undefinedEvent = "Organization : " + dr["Organization"] + " - Event : " + dr["Comment"] + " - Date : " + dr["Status Valid From"];
                bool alreadyAdded = (from events in OutputIRBForm.newIRBForm.AsEnumerable()
                                     where events.Field<string>("IRB_Event").Contains(undefinedEvent)
                                     & events.Field<string>("Study_number").ToLower().Trim().Contains(((string)eventRow["StudyId"]).Trim().ToLower())
                                     select events).Any();
                if (!alreadyAdded)
                {
                    OutputIRBForm.addEvents((string)dr["Study_number"], undefinedEvent);
                }
            }

            else if (newrecord)
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
            dr["Study_number"] = Tools.getStudyNumber((string)studyRow["StudyId"], ((string)studyRow["IRBNumber"]).Replace("(IBC)", ""));

            dr["IRB Study ID"] = (string)studyRow["StudyId"];

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Organization"] = OutputSite.EMmainsite;
            }
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Organization"] = BranySiteMap.getSite(((string)studyRow["Sitename"]).Replace("(IBC)", ""));
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                dr["Organization"] = IRISMap.SiteMap.getSite((string)studyRow["Sitename"]);
            }

            dr["Study status"] = status;
            dr["status type"] = "Pre Activation";
            dr["Documented by"] = "IRB interface";
            dr["Status Valid From"] = startdate;
            /*dr["Status Valid From"] = Tools.parseDate((string)studyRow["MostRecentApprovalDate"]);
            dr["Status Valid From"] = (string)dr["Status Valid From"] == "" ? Tools.parseDate((string)studyRow["InitialApprovalDate"]) : "";*/
            if (Agency.AgencySetupVal != Agency.AgencyList.NONE && !(status.Contains("Approved") && status.Contains("IRB")))
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


        static public void removeDuplicateStatus()
        {
            newStatus = Tools.removeDuplicate(newStatus);
            updatedStatus = Tools.removeDuplicate(updatedStatus);
        }

        /// <summary>
        /// Return the latest active status from Velos
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        static public string getLatestStatus(string studyId)
        {
            string status = "";

            status = (from stat in allstatus
                      where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower())
                      && stat.SSTAT_STUDY_STATUS != null
                      && stat.SSTAT_CURRENT_STAT == "Yes"
                      orderby stat.SSTAT_VALID_FROM descending
                      select stat.SSTAT_STUDY_STATUS).FirstOrDefault();

            return status;
        }

        /// <summary>
        /// Get the latest status in the file for the specified study
        /// </summary>
        public static string getLatestStatusFP(string studyId)
        {

            string currentstat = (from st in OutputStatus.fpstatus.data.AsEnumerable()
                                  where st.Field<string>("StudyId").Trim().ToLower() == studyId.Trim().ToLower()
                            select st.Field<string>("Status")
                            ).LastOrDefault();
            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                return currentstat;
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                return BranyStatusMap.getStatus(currentstat);
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                return IRISMap.StatusMap.getStatus(currentstat);
            else
                return currentstat;
        }

    }


}
