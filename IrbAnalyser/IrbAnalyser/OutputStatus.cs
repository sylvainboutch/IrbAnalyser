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
        public static string undefinedStatus = "Undefined IRB Event**";
        public static string undefinedType = "Study Status";
        public static string deletedStatus = "Archived";
        public static string deletedType = "Pre Activation";
        public static string deletedPostActivationStatus = "Undefined IRB Event**";
        public static string deletedPostActivationType = "Study Status";
        public static string approvedStatus = "IRB Initial Approved";
        public static string approvedType = "Pre Activation";
        public static string renewalStatus = "IRB Renewal Approved**";
        public static string renewalType = "Post Activation";
        public static string preActAmendmentSubmitStatus = "IRB Pre-Activation Amendent Submitted**";
        public static string preActAmendmentApprovedStatus = "IRB Pre-Activation Amendment Approved**";
        public static string preActAmendmentSubmitType = "Pre Activation";
        public static string preActAmendmentApprovedType = "Pre Activation";
        public static string amendmentSubmitStatus = "IRB Amendment Submitted**";
        public static string amendmentApprovedStatus = "IRB Amendment Approved**";
        public static string amendmentSubmitType = "Post Activation";
        public static string amendmentApprovedType = "Post Activation";
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

                        query = (from st in db.VDA_V_STUDYSTAT
                                 where //st.MORE_IRBAGENCY != null &&
                                 st.IRBIDENTIFIERS != null
                                 select st);
                        /*
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
                        */

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
        public static void initiate()
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
                newStatus.Columns.Add("Review Type", typeof(string));
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
                updatedStatus.Columns.Add("Review Type", typeof(string));
                updatedStatus.Columns.Add("pk_studystat", typeof(string));
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

            analyseDeletedStudies();
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
                string sitename = OutputSite.EMmainsite;

                DateTime start = DateTime.Now;
                DateTime.TryParse((string)statusRow["ValidOn"], out start);
                start = start == DateTime.MinValue ? DateTime.Now : start;

                string startstring = Tools.parseDate((string)statusRow["ValidOn"]);

                bool allowMultiple = false;
                string status = undefinedStatus;
                string type = undefinedType;

                if (Agency.AgencySetupVal == Agency.AgencyList.BRANY)
                {
                    allowMultiple = BranyStatusMap.getStatusMultipleBrany2((string)statusRow["Status"]);
                    status = BranyStatusMap.getStatus2((string)statusRow["Status"]);
                    type = BranyStatusMap.getType2((string)statusRow["Status"]);
                }
                else if (Agency.AgencySetupVal == Agency.AgencyList.EINSTEIN)
                {
                    allowMultiple = IRISMap.StatusMap.getMultiple2((string)statusRow["Status"]);
                    status = IRISMap.StatusMap.getStatus2((string)statusRow["Status"]);
                    type = IRISMap.StatusMap.getType2((string)statusRow["Status"]);
                }

                if (status != "" && status != "NA" && !String.IsNullOrEmpty(irbstudyId))
                {

                    /*if (irbstudyId == "5532")
                    {
                        Agency.AgencyVal = Agency.AgencyList.EINSTEIN;
                    }*/


                    bool isOtherStatusInDT = (from st in OutputStatus.newStatus.AsEnumerable()
                                              where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                         && st.Field<string>("Study status").Trim().ToLower() == status.Trim().ToLower()
                                              //&& st.Field<string>("Status Valid From").Trim().ToLower() != startstring.Trim().ToLower()
                                              select st).Any();

                    var statuses = from stat in allstatus
                                   where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == irbstudyId.Trim().ToLower()
                                   && stat.SSTAT_STUDY_STATUS != null
                                   && stat.SSTAT_VALID_FROM != null
                                   select stat;


                    bool isOtherStatusInDB = (from stat in statuses
                                              where stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status.Trim().ToLower()
                                              //&& stat.SSTAT_VALID_FROM.Value.Year != start.Year
                                              //&& stat.SSTAT_VALID_FROM.Value.Month != start.Month
                                              //&& stat.SSTAT_VALID_FROM.Value.Day != start.Day
                                              select stat).Any();

                    bool isStatusInDB = (from stat in statuses
                                         where stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status.Trim().ToLower()
                                         && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                         && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                         && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                         && stat.SSTAT_NOTES.Trim().ToLower().Contains(((string)statusRow["Status"]).Trim().ToLower())
                                         select stat).Any();

                    bool isStatusInDT = (from st in OutputStatus.newStatus.AsEnumerable()
                                         where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                         && st.Field<string>("Study status").Trim().ToLower() == status.Trim().ToLower()
                                         && st.Field<string>("Status Valid From").Trim().ToLower() == startstring.Trim().ToLower()
                                         && st.Field<string>("Comment").Trim().ToLower() == ((string)statusRow["Status"]).Trim().ToLower()
                                         select st).Any();


                    if (!allowMultiple && (isOtherStatusInDB || isOtherStatusInDT) && !isStatusInDB && !isStatusInDT)
                    {
                        status = undefinedStatus;
                        type = undefinedType;

                        isStatusInDB = (from stat in statuses
                                        where stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status.Trim().ToLower()
                                        && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                        && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                        && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                        && stat.SSTAT_NOTES.Trim().ToLower().Contains(((string)statusRow["Status"]).Trim().ToLower())
                                        select stat).Any();



                        var undefinedDT = (from st in OutputStatus.newStatus.AsEnumerable()
                                           where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                           && st.Field<string>("Study status").Trim().ToLower() == status.Trim().ToLower()
                                           && st.Field<string>("Status Valid From").Trim().ToLower() == startstring.Trim().ToLower()
                                           select st);



                        var isUndefinedDT = undefinedDT.Any();

                        if (!isUndefinedDT)
                        {
                            undefinedDT = (from st in OutputStatus.updatedStatus.AsEnumerable()
                                           where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                           && st.Field<string>("Study status").Trim().ToLower() == status.Trim().ToLower()
                                           && st.Field<string>("Status Valid From").Trim().ToLower() == startstring.Trim().ToLower()
                                           select st);

                            isUndefinedDT = undefinedDT.Any();
                        }

                        isStatusInDT = (from st in undefinedDT
                                        where st.Field<string>("Comment").Trim().ToLower().Contains(((string)statusRow["Status"]).Trim().ToLower())
                                        select st).Any();

                        var undefinedDB = (from stat in statuses
                                              where stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status.Trim().ToLower()
                                              && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                              && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                              && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                              select stat);

                        bool isUndefinedDB = undefinedDB.Any();

                        if (isUndefinedDT && !isStatusInDT && !isStatusInDB)
                        {
                            DataRow undefinedDR = undefinedDT.First();
                            undefinedDR["Comment"] = undefinedDR["Comment"] + " ; " + (string)statusRow["Status"];
                        }
                        else if (isUndefinedDB && !isStatusInDB && !isStatusInDT && !isUndefinedDT)
                        {
                            string comments = undefinedDB.First().SSTAT_NOTES + " ; " + (string)statusRow["Status"];
                            addRowStatus(irbstudyId, status, type, comments, sitename, startstring, "Modified Undefined IRB Event", true, (int)undefinedDB.First().PK_STUDYSTAT);
                        }
                        else if (!isStatusInDB && !isStatusInDT && statuses.Any() && !isUndefinedDB && !isUndefinedDT)
                        {
                            addRowStatus(irbstudyId, status, type, (string)statusRow["Status"], sitename, startstring, "New Status");
                        }
                        else if (!isStatusInDB && !isStatusInDT && !statuses.Any() && !isUndefinedDB && !isUndefinedDT)
                        {
                            addRowStatus(irbstudyId, status, type, (string)statusRow["Status"], sitename, startstring, "New Study");
                        }

                    }
                    else if (!isStatusInDB && !isStatusInDT && statuses.Any())
                    {
                        addRowStatus(irbstudyId, status, type, (string)statusRow["Status"], sitename, startstring, "New Status");
                    }
                    else if (!isStatusInDT && !isStatusInDB)
                    {
                        addRowStatus(irbstudyId, status, type, (string)statusRow["Status"], sitename, startstring, "New Study");
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

            /*if ((string)eventRow["StudyId"] == "dd0e38f8-2eb4-4b66-8891-d6f4f1b688fa")
            {
                Agency.AgencyVal = Agency.AgencyList.BRANY;
            }*/

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
                string stattype = "";
                //string type = "";
                string sitename = "";
                string notes = "";

                if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    status = BranyEventsMap.getStatus((string)eventRow["Event"]);
                    stattype = BranyEventsMap.getType((string)eventRow["Event"]);
                    sitename = OutputSite.EMmainsite;
                    notes = (string)eventRow["Event"];
                    if (BranyEventsMap.teamChangedEvents.Contains((string)eventRow["Event"]))
                    {
                        NewValueOuput.appendString("Study personnel changed - event name : ", (string)eventRow["Event"]);
                    }
                }

                else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                {
                    sitename = OutputSite.EMmainsite;
                    status = IRISMap.EventsMap.getStatus((string)eventRow["Event"]);
                    stattype = IRISMap.EventsMap.getType((string)eventRow["Event"]);
                    notes = Tools.removeHtml((string)eventRow["Amendment"]);
                }

                if (!string.IsNullOrEmpty(status))
                {

                    string status1 = amendmentSubmitStatus;
                    string status2 = amendmentApprovedStatus;
                    string stattype1 = amendmentApprovedType;

                    DateTime start = DateTime.Now;
                    DateTime.TryParse((string)eventRow["EventCreationDate"], out start);
                    start = start == DateTime.MinValue ? DateTime.Now : start;

                    DateTime end = DateTime.MinValue;
                    //DateTime.TryParse((string)eventRow["TaskCompletionDate"], out end);
                    //if (end == DateTime.MinValue)
                    DateTime.TryParse((string)eventRow["EventCompletionDate"], out end);


                    if (Tools.getOldStudy((string)eventRow["StudyId"]))
                    {
                        if (status == amendmentSubmitStatus)
                        {
                            if (!Tools.isStudyApproved(irbstudyId, start))
                            {
                                status1 = preActAmendmentSubmitStatus;
                                status2 = preActAmendmentApprovedStatus;
                                stattype1 = preActAmendmentApprovedType;
                            }

                            IEnumerable<Model.VDA_V_STUDYSTAT> statusesDB1 = from stat in allstatus
                                                                             where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                                                             && (stat.SSTAT_STUDY_STATUS == preActAmendmentSubmitStatus || stat.SSTAT_STUDY_STATUS == amendmentSubmitStatus)
                                                                             && stat.SSTAT_VALID_FROM.Value.Year == start.Year
                                                                             && stat.SSTAT_VALID_FROM.Value.Month == start.Month
                                                                             && stat.SSTAT_VALID_FROM.Value.Day == start.Day
                                                                             select stat;


                            if (!statusesDB1.Any())
                            {
                                addRowEvent(eventRow, status1, stattype1, "New status", true, start.ToString());
                            }

                            if (end != DateTime.MinValue)
                            {
                                bool isStatusesDB2 = (from stat in allstatus
                                                      where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                                      && (stat.SSTAT_STUDY_STATUS == preActAmendmentApprovedStatus || stat.SSTAT_STUDY_STATUS == amendmentApprovedStatus)
                                                      && stat.SSTAT_VALID_FROM.Value.Year == end.Year
                                                      && stat.SSTAT_VALID_FROM.Value.Month == end.Month
                                                      && stat.SSTAT_VALID_FROM.Value.Day == end.Day
                                                      select stat).Any();


                                if (!isStatusesDB2)
                                {
                                    addRowEvent(eventRow, status2, stattype1, "New status", true, end.ToString());
                                }
                            }
                        }

                    }
                    else
                    {
                        /*
                        if ((string)eventRow["StudyId"] == "1286a741-af55-4a32-8e9a-d85d81c4824a")
                        {
                            Agency.AgencyVal = Agency.AgencyList.BRANY;
                        }*/

                        if (!Tools.isStudyApproved(irbstudyId, start))
                        {
                            status1 = preActAmendmentSubmitStatus;
                            status2 = preActAmendmentApprovedStatus;
                            stattype1 = preActAmendmentApprovedType;
                        }

                        if (status == amendmentSubmitStatus)
                        {


                            addRowEvent(eventRow, status1, stattype1, "New study", true, (string)eventRow["EventCreationDate"]);
                            if (end != DateTime.MinValue)
                            {
                                addRowEvent(eventRow, status2, stattype1, "New study", true, end.ToString());
                            }
                        }
                        else
                        {
                            addRowEvent(eventRow, status, stattype, "New study", true, (string)eventRow["EventCreationDate"]);
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
            if (!((string)studyrow["Sitename"]).Contains("(IBC)"))
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
                                string latestStatus = (string)status.Last().Field<string>("Status");
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
                                    BranyStatusMap.getStatus2(latestStatus);
                                }
                                else
                                {
                                    IRISMap.StatusMap.getStatus2(latestStatus);
                                }

                                if (latestStatus != (string)studyrow["Status"])
                                {
                                    addRowStudy(studyrow, (string)studyrow["Status"], "New study", (string)studyrow["Status"], true, Tools.parseDate(DateTime.Now.ToString()));
                                }
                            }
                            else
                            {
                                addRowStudy(studyrow, (string)studyrow["Status"], "New study", (string)studyrow["Status"], true, Tools.parseDate(DateTime.Now.ToString()));
                            }

                        }

                        bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                          where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                          && st.Field<string>("Study status").Trim().ToLower() == approvedStatus.ToLower()
                                          //&& st.Field<string>("Status Valid From").Trim().ToLower() == Tools.parseDate(((string)studyrow["InitialApprovalDate"])).Trim().ToLower()
                                          select st).Any();

                        if (dtStatus && initial != DateTime.MinValue)
                        {
                            addRowStudy(studyrow, approvedStatus, "New study", approvedType, true, Tools.parseDate((string)studyrow["InitialApprovalDate"]).Trim().ToLower());
                        }

                        dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                     where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                     && st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved**"
                                     && st.Field<string>("Status Valid From").Trim().ToLower() == Tools.parseDate((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower()
                                     select st).Any();

                        if (dtStatus && renew != DateTime.MinValue && renew.Date != initial.Date)
                        {
                            addRowStudy(studyrow, "IRB Renewal Approved**", "New study", "Post Activation", true, Tools.parseDate((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower());
                        }

                        /*if (SpecialStudys.closedStudys.Any(x => x.IRB == Agency.agencyStrLwr && Tools.compareStr(x.number, (string)studyrow["IRBNumber"])))
                        {
                            addRowStudy(studyrow, "Complete", "New study", true, Tools.parseDate(DateTime.Now.ToString()));
                        }*/


                    }
                    else
                    {
                        //FOR TESTING PURPOSE 
                        /*
                        if (irbstudyId.ToLower() == "1b761de8-918b-4756-86ed-49bb5ebac2ed")
                        {
                            Agency.AgencyVal = Agency.AgencyList.BRANY;
                        }*/

                        bool isApprovedDB = (from stat in allstatus
                                             where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                    && (stat.SSTAT_STUDY_STATUS.Trim().ToLower() == renewalStatus.ToLower()
                                    || stat.SSTAT_STUDY_STATUS.Trim().ToLower() == approvedStatus.ToLower())
                                    && stat.SSTAT_VALID_FROM.Value.Year == initial.Year
                                    && stat.SSTAT_VALID_FROM.Value.Month == initial.Month
                                    && stat.SSTAT_VALID_FROM.Value.Day == initial.Day
                                             select stat).Any();


                        bool isRenewDB = (from stat in allstatus
                                          where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                 && (stat.SSTAT_STUDY_STATUS.Trim().ToLower() == renewalStatus.ToLower()
                                 || stat.SSTAT_STUDY_STATUS.Trim().ToLower() == approvedStatus.ToLower())
                                 && stat.SSTAT_VALID_FROM.Value.Year == renew.Year
                                 && stat.SSTAT_VALID_FROM.Value.Month == renew.Month
                                 && stat.SSTAT_VALID_FROM.Value.Day == renew.Day
                                          select stat).Any();

                        bool isApprovedDT = (from st in OutputStatus.newStatus.AsEnumerable()
                                             where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                             && (st.Field<string>("Study status").Trim().ToLower() == approvedStatus.ToLower()
                                             || st.Field<string>("Study status").Trim().ToLower() == approvedStatus.ToLower())
                                             && st.Field<string>("Status Valid From").Trim().ToLower() == Tools.parseDate(((string)studyrow["InitialApprovalDate"]).Trim().ToLower())
                                             select st).Any();

                        bool isrenewDT = (from st in OutputStatus.newStatus.AsEnumerable()
                                          where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                          && (st.Field<string>("Study status").Trim().ToLower() == approvedStatus.ToLower()
                                          || st.Field<string>("Study status").Trim().ToLower() == approvedStatus.ToLower())
                                          && st.Field<string>("Status Valid From").Trim().ToLower() == Tools.parseDate(((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower())
                                          select st).Any();

                        if (initial != DateTime.MinValue && !isApprovedDB && !isApprovedDT)
                        {
                            addRowStudy(studyrow, approvedStatus, "New status", approvedType, true, Tools.parseDate((string)studyrow["InitialApprovalDate"]).Trim().ToLower());
                        }

                        if (renew != DateTime.MinValue && !isrenewDT && !isRenewDB && renew.Date != initial.Date)
                        {
                            addRowStudy(studyrow, renewalStatus, "New status", renewalType, true, Tools.parseDate((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower());
                        }


                        /*

                        bool isNotStatusDb = !(from stat in allstatus
                                               where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                          && (stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb renewal approved**"
                                          || stat.SSTAT_STUDY_STATUS.Trim().ToLower() == approvedStatus.ToLower())
                                               select stat).Any();

                        bool isNotStatusDt = !(from st in OutputStatus.newStatus.AsEnumerable()
                                               where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                               && (st.Field<string>("Study status").Trim().ToLower() == approvedStatus.ToLower()
                                               || st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved**")
                                               && st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved**"
                                               select st).Any();
                    

                        if (initial != DateTime.MinValue && isNotStatusDt && isNotStatusDb)
                        {
                            addRowStudy(studyrow, approvedStatus, "New status", approvedType, true, Tools.parseDate((string)studyrow["InitialApprovalDate"]).Trim().ToLower());
                        }

                        isNotStatusDb = !(from stat in allstatus
                                          where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                     && (stat.SSTAT_STUDY_STATUS.Trim().ToLower() == "irb renewal approved**"
                                     || stat.SSTAT_STUDY_STATUS.Trim().ToLower() == approvedStatus.ToLower())
                                     && stat.SSTAT_VALID_FROM.Value.Year == renew.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month == renew.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day == renew.Day
                                          select stat).Any();


                        isNotStatusDt = !(from st in OutputStatus.newStatus.AsEnumerable()
                                          where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                          && (st.Field<string>("Study status").Trim().ToLower() == "irb renewal approved**"
                                          || st.Field<string>("Study status").Trim().ToLower() == approvedStatus.ToLower())
                                          && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower()
                                          select st).Any();

                        if (renew != DateTime.MinValue && isNotStatusDt && isNotStatusDb && renew.Date != initial.Date)
                        {
                            addRowStudy(studyrow, "IRB Renewal Approved**", "New status", "Post Activation", true, Tools.parseDate((string)studyrow["MostRecentApprovalDate"]).Trim().ToLower());
                        }*/
                    }
                }
            }
        }


        /// <summary>
        /// Add new status from status report
        /// </summary>
        /// <param name="statusRow"></param>
        private static void addRowStatus(string studyId, string status, string statustype, string statusnote, string statussite, string statusDate, string type, bool isupdate = false, int pk_studystat = 0)
        {
            if (string.IsNullOrEmpty(status))
            {
                string undefinedEvent = "Organization : " + statussite + " - Status : " + status + " - Date : " + statusDate;
                bool alreadyAdded = (from events in OutputIRBForm.newIRBForm.AsEnumerable()
                                     where events.Field<string>("IRB_Status").Contains(undefinedEvent)
                                     & events.Field<string>("Study_number").ToLower().Trim().Contains(studyId.Trim().ToLower())
                                     select events).Any();
                if (!alreadyAdded)
                {

                    OutputIRBForm.addStatus(Tools.getOldStudyNumber(studyId), undefinedEvent);
                }
            }
            else
            {

                if (pk_studystat != 0 && isupdate && !updatedStatus.Columns.Contains("pk_studystat"))
                {
                    updatedStatus.Columns.Add("pk_studystat");
                }

                DataRow dr;
                if (isupdate)
                {
                    dr = updatedStatus.NewRow();
                }
                else
                {
                    dr = newStatus.NewRow();
                }



                if (pk_studystat != 0 && isupdate)
                {
                    dr["pk_studystat"] = pk_studystat;
                }

                dr["TYPE"] = type;

                dr["Study_number"] = Tools.getOldStudyNumber(studyId);

                dr["IRB Study ID"] = studyId;
                dr["Organization"] = statussite;
                dr["Study status"] = status;
                dr["status type"] = statustype;

                //TODO MAP the IRIS status
                dr["Comment"] = statusnote;

                dr["Documented by"] = "IRB interface";
                dr["Status Valid From"] = statusDate;

                bool dtStatus = !(from st in OutputStatus.newStatus.AsEnumerable()
                                  where st.Field<string>("IRB Study ID").Trim().ToLower() == studyId
                                  && st.Field<string>("Study status").Trim().ToLower() == statussite
                                  && st.Field<string>("Status Valid From").Trim().ToLower() == ((string)dr["Status Valid From"]).Trim().ToLower()
                                  && st.Field<string>("Comment").Trim().ToLower() == ((string)dr["Comment"]).Trim().ToLower()
                                  select st).Any();

                if (isupdate && dtStatus)
                {
                    updatedStatus.Rows.Add(dr);
                }
                else if (dtStatus)
                {
                    newStatus.Rows.Add(dr);
                }
            }
        }


        /// <summary>
        /// Add new status from event report
        /// </summary>
        /// <param name="statusRow"></param>
        private static void addRowEvent(DataRow eventRow, string status, string statusType, string type, bool newrecord, string date)
        {
            DataRow dr;
            if (newrecord)
            { dr = newStatus.NewRow(); }
            else
            { dr = updatedStatus.NewRow(); }

            dr["TYPE"] = type;

            dr["Study_number"] = Tools.getOldStudyNumber((string)eventRow["StudyId"]);

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
            dr["status type"] = statusType;
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


        private static void addRowStudy(DataRow studyRow, string status, string type, string statustype, bool newrecord, string startdate)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            { dr = newStatus.NewRow(); }
            else
            { dr = updatedStatus.NewRow(); }
            dr["TYPE"] = type;
            dr["Comment"] = ((string)studyRow["IRBNumber"]).Contains("(IBC)") ? "Status from IBC" : "";
            dr["Study_number"] = Tools.getOldStudyNumber((string)studyRow["StudyId"]);

            dr["IRB Study ID"] = (string)studyRow["StudyId"];

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Organization"] = OutputSite.EMmainsite;
            }
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                //dr["Organization"] = BranySiteMap.getSite(((string)studyRow["Sitename"]).Replace("(IBC)", ""));
                dr["Organization"] = OutputSite.EMmainsite;
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                //dr["Organization"] = IRISMap.SiteMap.getSite((string)studyRow["Sitename"]);
                dr["Organization"] = OutputSite.EMmainsite;
            }

            dr["Review Type"] = (string)studyRow["ReviewType"];

            dr["Study status"] = status;
            dr["status type"] = statustype;
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



        private static void analyseDeletedStudies()
        {
            IEnumerable<Model.LCL_V_STUDYSUMM_PLUSMORE> dbStudies = new List<Model.LCL_V_STUDYSUMM_PLUSMORE>();
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dbStudies = (from stud in OutputStudy.studys
                             where
                             stud.MORE_IRBAGENCY != null &&
                             stud.MORE_IRBAGENCY == Agency.brany && stud.IRBIDENTIFIERS != null
                             select stud);
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                dbStudies = (from stud in OutputStudy.studys
                             where
                             stud.MORE_IRBAGENCY != null &&
                             (
                             stud.MORE_IRBAGENCY == Agency.iris
                                 //|| stud.MORE_IRBAGENCY == "NCI CIRB"
                                 //|| stud.MORE_IRBAGENCY == "NeuroNext CIRB"
                                 //|| stud.MORE_IRBAGENCY == "StrokeNet CIRB"
                             )
                             && stud.IRBIDENTIFIERS != null
                             select stud);
            }
            foreach (Model.LCL_V_STUDYSUMM_PLUSMORE dbStudy in dbStudies)
            {
                string studyId = Tools.getStudyIdentifiers(dbStudy.IRBIDENTIFIERS);
                bool irbStudyDoesntExist = !(from DataRow dr in OutputStudy.fpstudys.data.Rows
                                             where (string)dr["StudyId"] == studyId
                                             select dr).Any();



                if (irbStudyDoesntExist)
                {
                    bool velosStudyIsntMarked = !(from dbStatus in OutputStatus.allstatus
                                                  where dbStatus.IRBIDENTIFIERS == dbStudy.IRBIDENTIFIERS
                                                  && (dbStatus.SSTAT_STUDY_STATUS == deletedPostActivationStatus
                                                  || dbStatus.SSTAT_STUDY_STATUS == deletedStatus)
                                                  select dbStatus).Any();

                    if (velosStudyIsntMarked)
                    {

                        bool irbStudyApproved = Tools.isStudyApproved(studyId, DateTime.MinValue);

                        DataRow dr = newStatus.NewRow();
                        if (irbStudyApproved)
                        {
                            dr["Study status"] = deletedPostActivationStatus;
                            dr["status type"] = deletedPostActivationType;
                        }
                        else
                        {
                            dr["Study status"] = deletedStatus;
                            dr["status type"] = deletedType;
                        }



                        dr["TYPE"] = "Deleted Study in IRB";
                        dr["Comment"] = "This study has been deleted from the IRB system";
                        dr["Study_number"] = Tools.getDBStudyNumber(studyId);

                        dr["IRB Study ID"] = studyId;
                        dr["Organization"] = OutputSite.EMmainsite;

                        dr["Review Type"] = "";

                        dr["Documented by"] = "IRB interface";
                        dr["Status Valid From"] = Tools.parseDate(DateTime.Now.ToShortDateString());

                        newStatus.Rows.Add(dr);
                    }
                }
            }

        }


        /// <summary>
        /// Remove duplicates from the newStatus and updatedStatus DT
        /// </summary>
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
        /// Gets the type and name of the status
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static string[] getStatus(DataRow dr)
        {
            string[] statuspair = new string[2] { undefinedStatus, undefinedType };
            bool allowMultiple = false;
            string status = undefinedStatus;
            string type = undefinedType;

            if (Agency.AgencySetupVal == Agency.AgencyList.BRANY || (Agency.AgencySetupVal == Agency.AgencyList.NONE && Agency.AgencyVal == Agency.AgencyList.BRANY))
            {
                allowMultiple = BranyStatusMap.getStatusMultipleBrany2((string)dr["Status"]);
                status = BranyStatusMap.getStatus2((string)dr["Status"]);
                type = BranyStatusMap.getType2((string)dr["Status"]);
            }
            else if (Agency.AgencySetupVal == Agency.AgencyList.EINSTEIN || (Agency.AgencySetupVal == Agency.AgencyList.NONE && Agency.AgencyVal == Agency.AgencyList.EINSTEIN))
            {
                allowMultiple = IRISMap.StatusMap.getMultiple2((string)dr["Status"]);
                status = IRISMap.StatusMap.getStatus2((string)dr["Status"]);
                type = IRISMap.StatusMap.getType2((string)dr["Status"]);
            }

            if (!allowMultiple)
            {
                DateTime start = DateTime.Now;
                DateTime.TryParse((string)dr["ValidOn"], out start);
                start = start == DateTime.MinValue ? DateTime.Now : start;

                bool isStatusInDT = (from st in OutputStatus.newStatus.AsEnumerable()
                                     where st.Field<string>("IRB Study ID").Trim().ToLower() == ((string)dr["StudyId"]).Trim().ToLower()
                                     && st.Field<string>("Study status").Trim().ToLower() == status.Trim().ToLower()
                                     && st.Field<string>("Status Valid From").Trim().ToLower() != Tools.parseDate((string)dr["ValidOn"]).Trim().ToLower()
                                     select st).Any();

                var statuses = from stat in allstatus
                               where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == ((string)dr["StudyId"]).Trim().ToLower()
                               && stat.SSTAT_STUDY_STATUS != null
                               && stat.SSTAT_VALID_FROM != null
                               && stat.SSTAT_SITE_NAME != null
                               select stat;

                bool isStatusInDB = (from stat in statuses
                                     where stat.SSTAT_STUDY_STATUS.Trim().ToLower() == status.Trim().ToLower()
                                     && stat.SSTAT_VALID_FROM.Value.Year != start.Year
                                     && stat.SSTAT_VALID_FROM.Value.Month != start.Month
                                     && stat.SSTAT_VALID_FROM.Value.Day != start.Day
                                     select stat).Any();

                /* bool isSmallerStatusInFP = (from st in fpstatus.data.AsEnumerable()
                                           where ((string)dr["StudyId"]).Trim().ToLower() == st.Field<string>("StudyId").Trim().ToLower()
                                           && ((string)dr["Status"]).Trim().ToLower() == st.Field<string>("Status").Trim().ToLower()
                                          && !st.Field<string>("IRBNumber").Trim().ToLower().Contains("ibc")
                                          && Tools.parseDateDate(st.Field<string>("ValidOn")) < Tools.parseDateDate((string)dr["ValidOn"])
                                           select st).Any();*/

                if (isStatusInDB || isStatusInDT) //&& !isSmallerStatusInFP)
                {
                    status = undefinedStatus;
                    type = undefinedType;
                }
            }

            statuspair[0] = status;
            statuspair[1] = type;

            return statuspair;
        }

    }


}
