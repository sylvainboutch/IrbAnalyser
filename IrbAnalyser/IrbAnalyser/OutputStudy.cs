﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    /// <summary>
    /// Class for handling study information
    /// </summary>
    public static class OutputStudy
    {

        public static List<string> closedStatusInVelos = new List<string>()
        {
            "Administratively Complete",
            "Archived",
            "Complete",
            "Completely Closed - Site",
            "Withdrawn"

        };

        //List of newly created study, with more study detail
        public static DataTable newStudy = new DataTable();
        //List of updated study, with more study detail
        public static DataTable updatedStudy = new DataTable();

        //Contains all study from the database, ensure that the whole list is not being pulled more then once
        private static IEnumerable<Model.LCL_V_STUDYSUMM_PLUSMORE> _studys;
        public static IEnumerable<Model.LCL_V_STUDYSUMM_PLUSMORE> studys
        {
            get
            {
                if (_studys == null || _studys.Count() == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {
                        IQueryable<Model.LCL_V_STUDYSUMM_PLUSMORE> query;

                        query = (from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                 where st.IRBIDENTIFIERS != null
                                 /*where st.MORE_IRBAGENCY != null
                                 && st.IRBIDENTIFIERS != null*/
                                 select st);
                        /*
                        //BRANY look up agency in MSD
                        if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                        {
                            query = (from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                     where st.MORE_IRBAGENCY != null
                                     && st.IRBIDENTIFIERS != null
                                     select st);
                        }
                        else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                        {
                            query = (from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                     where st.MORE_IRBAGENCY == Agency.agencyStrLwr
                                     && st.IRBIDENTIFIERS != null
                                     select st);
                        }
                        //IRIS all other agency in MSD, non IRB studies wont have 
                        else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                        {
                            query = (from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                     where st.MORE_IRBAGENCY != Agency.brany
                                     && st.IRBIDENTIFIERS != null
                                     select st);
                        }
                        else
                        {
                            query = (from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                     where st.MORE_IRBAGENCY != null
                                     && st.IRBIDENTIFIERS != null
                                     select st);
                        }
                        */
                        _studys = query.ToList<Model.LCL_V_STUDYSUMM_PLUSMORE>();
                    }
                }
                return _studys;
            }
            set
            {
                _studys = value;
            }
        }

        //FileParser object for the study data
        private static FileParser _fpstudys;
        public static FileParser fpstudys
        {
            get
            {
                if (_fpstudys == null || _fpstudys.data == null || _fpstudys.data.Rows.Count == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        _fpstudys = new FileParser(Tools.filename + "Study.txt", FileParser.type.Study);
                    }
                }
                return _fpstudys;
            }
            set
            {
                _fpstudys = value;
            }
        }



        /// <summary>
        /// Add the columns to the datatable
        /// </summary>
        private static void initiate()
        {
            if (newStudy.Columns.Count == 0)
            {
                /*newStudy.Columns.Add("IRB Agency name", typeof(string));
                newStudy.Columns.Add("IRB no", typeof(string));
                newStudy.Columns.Add("IRB Study ID", typeof(string));
                newStudy.Columns.Add("IRB Identifiers", typeof(string));
                newStudy.Columns.Add("Study_number", typeof(string));
                newStudy.Columns.Add("Regulatory_coordinator", typeof(string));
                newStudy.Columns.Add("Study_coordinator", typeof(string));
                newStudy.Columns.Add("Principal Investigator", typeof(string));
                newStudy.Columns.Add("Official title", typeof(string));
                newStudy.Columns.Add("Study Summary", typeof(string));
                newStudy.Columns.Add("Department", typeof(string));
                newStudy.Columns.Add("Division/Therapeutic area", typeof(string));
                newStudy.Columns.Add("Entire study sample size", typeof(string));
                newStudy.Columns.Add("Phase", typeof(string));
                newStudy.Columns.Add("Study scope", typeof(string));
                newStudy.Columns.Add("Primary funding sponsor", typeof(string));
                newStudy.Columns.Add("Sponsor information other", typeof(string));
                newStudy.Columns.Add("Sponsor contact", typeof(string));
                newStudy.Columns.Add("Sponsor Protocol ID", typeof(string));
                newStudy.Columns.Add("CRO", typeof(string));
                newStudy.Columns.Add("Cancer", typeof(string));
                newStudy.Columns.Add("Device", typeof(string));
                newStudy.Columns.Add("Drug", typeof(string));
                newStudy.Columns.Add("Consent", typeof(string));*/
                //newStudy.Columns.Add("Accronym", typeof(string));
                newStudy.Columns.Add("Study_number", typeof(string));
                newStudy.Columns.Add("Regulatory_coordinator", typeof(string));
                newStudy.Columns.Add("Division/Therapeutic area", typeof(string));
                newStudy.Columns.Add("Phase", typeof(string));
                newStudy.Columns.Add("Official title", typeof(string));
                newStudy.Columns.Add("Study Summary", typeof(string));
                newStudy.Columns.Add("Principal Investigator", typeof(string));
                newStudy.Columns.Add("Study_coordinator", typeof(string));
                newStudy.Columns.Add("Department", typeof(string));
                newStudy.Columns.Add("Entire study sample size", typeof(string));
                newStudy.Columns.Add("Study scope", typeof(string));
                newStudy.Columns.Add("Primary funding sponsor", typeof(string));
                newStudy.Columns.Add("Sponsor Protocol ID", typeof(string));
                newStudy.Columns.Add("Sponsor contact", typeof(string));
                newStudy.Columns.Add("Sponsor information other", typeof(string));
                newStudy.Columns.Add("IRB Agency name", typeof(string));
                newStudy.Columns.Add("IRB no", typeof(string));
                newStudy.Columns.Add("IRB Study ID", typeof(string));
                newStudy.Columns.Add("IRB Identifiers", typeof(string));
                newStudy.Columns.Add("CRO", typeof(string));
                newStudy.Columns.Add("Cancer", typeof(string));
                newStudy.Columns.Add("Device", typeof(string));
                newStudy.Columns.Add("Drug", typeof(string));
                newStudy.Columns.Add("Consent", typeof(string));
            }

            if (updatedStudy.Columns.Count == 0)
            {
                updatedStudy.Columns.Add("IRB Agency name", typeof(string));
                updatedStudy.Columns.Add("IRB no", typeof(string));
                updatedStudy.Columns.Add("IRB Study ID", typeof(string));
                updatedStudy.Columns.Add("IRB Identifiers", typeof(string));
                updatedStudy.Columns.Add("Study_number", typeof(string));
                //updatedStudy.Columns.Add("Old_study_number", typeof(string));
                updatedStudy.Columns.Add("New_Number", typeof(string));
                updatedStudy.Columns.Add("Regulatory_coordinator", typeof(string));
                updatedStudy.Columns.Add("Study_coordinator", typeof(string));
                updatedStudy.Columns.Add("Principal Investigator", typeof(string));
                updatedStudy.Columns.Add("Official title", typeof(string));
                updatedStudy.Columns.Add("Study summary", typeof(string));
                updatedStudy.Columns.Add("Department", typeof(string));
                updatedStudy.Columns.Add("Division/Therapeutic area", typeof(string));
                updatedStudy.Columns.Add("Entire study sample size", typeof(string));
                updatedStudy.Columns.Add("Phase", typeof(string));
                updatedStudy.Columns.Add("Study scope", typeof(string));
                updatedStudy.Columns.Add("Primary funding sponsor", typeof(string));
                updatedStudy.Columns.Add("Sponsor information other", typeof(string));
                updatedStudy.Columns.Add("Sponsor contact", typeof(string));
                updatedStudy.Columns.Add("Sponsor Protocol ID", typeof(string));
                updatedStudy.Columns.Add("Cancer", typeof(string));
                updatedStudy.Columns.Add("CRO", typeof(string));
                updatedStudy.Columns.Add("Device", typeof(string));
                updatedStudy.Columns.Add("Drug", typeof(string));
                updatedStudy.Columns.Add("Consent", typeof(string));
                //updatedStudy.Columns.Add("Accronym", typeof(string));
            }
        }


        /// <summary>
        /// Analyse the study report
        /// </summary>
        public static void analyse(string filepath)
        {
            initiate();
            FileParser fpStudy = new FileParser(filepath + "Study.txt", FileParser.type.Study);
            foreach (DataRow study in fpStudy.data.Rows)
            {
                analyseRow(study);
            }
        }


        /// <summary>
        /// Analyse a row of the study report
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow dr)
        {
            string irbstudyId = (string)dr["StudyId"];

            if (!String.IsNullOrEmpty((string)dr["IRBAgency"]))
            {
                if (((string)dr["IRBAgency"]).ToLower() == "brany")
                    Agency.AgencyVal = Agency.AgencyList.BRANY;
                else if (((string)dr["IRBAgency"]).ToLower() == "einstein")
                    Agency.AgencyVal = Agency.AgencyList.EINSTEIN;
                else
                {
                    Agency.AgencyVal = Agency.AgencySetupVal;
                    dr["ExternalIRB"] = dr["IRBAgency"];
                    dr["ExternalIRBnumber"] = dr["IRBNumber"];
                }
            }

            string irbagency = "";
            string irbnumber = "";
            /*dr["IRBAgency"]
                dr["IRBNumber"]*/
            if (!string.IsNullOrWhiteSpace(((string)dr["ExternalIRB"])))
            {
                irbnumber = (string)dr["ExternalIRBnumber"];
                if (((string)dr["ExternalIRB"]).ToLower().Contains("neuronext"))
                    irbagency = "NeuroNext CIRB";
                else if (((string)dr["ExternalIRB"]).ToLower().Contains("strokenet"))
                    irbagency = "StrokeNet CIRB";
                else if (((string)dr["ExternalIRB"]).ToLower().Contains("nci") || ((string)dr["ExternalIRB"]).ToLower().Contains("cirb"))
                    irbagency = "NCI CIRB";
                else// if (((string)row["ExternalIRB"]).ToLower().Contains("external"))
                    irbagency = "";
            }
            else
            {
                irbagency = Agency.AgencyVal == Agency.AgencyList.EINSTEIN ? "Einstein IRB" : irbagency;
                irbagency = Agency.AgencyVal == Agency.AgencyList.BRANY ? "BRANY" : irbagency;
                irbnumber = ((string)dr["IRBNumber"]).Replace("(IBC)", "");
            }


            if (shouldStudyBeAdded(irbstudyId))
            {
                string identifiers = Tools.generateStudyIdentifiers((string)dr["StudyId"]);
                string number = Tools.getOldStudyNumber((string)dr["StudyId"]);

                OutputIRBForm.addIds(number, identifiers);

                //external IRB only available in IRIS, we ignore studies with external IRB = BRANY since we have them in the BRANY file. Corrupted studies are in IRIS and shouldnt be pull in, they are a result of their data migration.
                //empty study ID shouldnt happen but could indicate an empty line in the file.
                if (!String.IsNullOrEmpty(irbstudyId) && !((string)dr["StudyId"]).ToLower().Contains("corrupted") && !((string)dr["ExternalIRB"]).ToLower().Contains("brany"))
                {
                    var study = from st in studys
                                where st.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                select st;

                    if (!study.Any())
                    {
                        bool dtStudy = (from st in OutputStudy.newStudy.AsEnumerable()
                                        where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                        select st).Any();
                        OutputSite.analyseRow(dr, true);


                        if (!dtStudy)
                        {
                            OutputStatus.analyseRowStudy(dr, true);

                            string newNumber = Tools.getOldStudyNumber((string)dr["StudyId"]);

                            if (!dr.Table.Columns.Contains("oldNumber"))
                            {
                                dr.Table.Columns.Add("oldNumber");
                            }
                            if (!dr.Table.Columns.Contains("newNumber"))
                            {
                                dr.Table.Columns.Add("newNumber");
                            }

                            dr["oldNumber"] = newNumber;

                            dr["IRBAgency"] = irbagency;
                            dr["IRBNumber"] = irbnumber;

                            addRowStudy(dr, true);
                            //Add all related values for that study                            
                            OutputDocs.analyseRow(dr, true);
                            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                                OutputTeam.addRowMigration(dr, number);
                        }
                    }
                    else
                    {
                        bool dtStudy = (from st in OutputStudy.updatedStudy.AsEnumerable()
                                        where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                        select st).Any();

                        OutputSite.analyseRow(dr, false);


                        if (!dtStudy)
                        {

                            OutputStatus.analyseRowStudy(dr, false);

                            OutputDocs.analyseRow(dr, false);

                            bool hasChanged = false;
                            string newpi = "";
                            string newrc = "";
                            string newsc = "";
                            string newcro = "";

                            foreach (var stu in study)
                            {

                                string newNumber = Tools.getNewStudyNumber((string)dr["StudyId"], irbnumber, (string)dr["StudyAcronym"], (string)dr["StudyTitle"], (string)dr["PrimarySponsorStudyId"]);
                                string oldNumber = Tools.getDBStudyNumber((string)dr["StudyId"]);

                                if (!dr.Table.Columns.Contains("oldNumber"))
                                {
                                    dr.Table.Columns.Add("oldNumber");
                                }
                                if (!dr.Table.Columns.Contains("newNumber"))
                                {
                                    dr.Table.Columns.Add("newNumber");
                                }

                                dr["oldNumber"] = oldNumber;
                                if (newNumber != oldNumber && !string.IsNullOrWhiteSpace((string)dr["StudyAcronym"]))
                                {
                                    hasChanged = true;
                                    dr["newNumber"] = newNumber;
                                }
                                else
                                {
                                    dr["newNumber"] = "";
                                }


                                if (!string.IsNullOrWhiteSpace(stu.MORE_IRBNUM) && stu.MORE_IRBNUM.ToLower().Trim() != irbnumber.ToLower().Trim())
                                {
                                    hasChanged = true;
                                    dr["IRBNumber"] = irbnumber;
                                }
                                else
                                {
                                    dr["IRBNumber"] = "";
                                }


                                if (!string.IsNullOrWhiteSpace(stu.MORE_IRBAGENCY) && stu.MORE_IRBAGENCY.ToLower().Trim() != irbagency.ToLower().Trim())
                                {
                                    hasChanged = true;
                                    dr["IRBAgency"] = irbagency;
                                }
                                else
                                {
                                    dr["IRBAgency"] = "";
                                }

                                newpi = getPI((string)dr["StudyId"]);
                                newrc = getRC((string)dr["StudyId"]);


                                //newsc = getSC((string)dr["StudyId"]);
                                //newcro = getCRO((string)dr["StudyId"]);

                                RCSCPI rcscpi = new RCSCPI();
                                rcscpi = SpecialStudys.getRCSCPI((string)dr["StudyId"]);

                                string cancer = isStudyCancer(dr) ? "Y" : "";

                                if (stu.MORE_CANCER == "Y" && cancer == "")
                                {
                                    hasChanged = true;
                                    dr["Cancer"] = "N";
                                }


                                /*if ((stu.MORE_CANCER == "N" || stu.MORE_CANCER == null) && cancer == "Y")
                                {
                                    hasChanged = true;
                                    dr["Cancer"] = "Y";
                                }*/

                                
                                if (stu.MORE_SC_AGENT == "Y" && ((string)dr["Drug"] != "Y"))
                                {
                                    hasChanged = true;
                                    dr["Drug"] = "N";
                                }
                                else if ((string.IsNullOrWhiteSpace(stu.MORE_SC_AGENT) || stu.MORE_SC_AGENT == "N") && ((string)dr["Drug"] == "Y"))
                                {
                                    hasChanged = true;
                                    dr["Drug"] = "Y";
                                }
                                else
                                {
                                    dr["Drug"] = "";
                                }

                                if (stu.MORE_SC_DEVICE == "Y" && ((string)dr["Device"] != "Y"))
                                {
                                    hasChanged = true;
                                    dr["Device"] = "N";
                                }
                                else if ((string.IsNullOrWhiteSpace(stu.MORE_SC_DEVICE) || stu.MORE_SC_DEVICE == "N") && ((string)dr["Device"] == "Y"))
                                {
                                    hasChanged = true;
                                    dr["Device"] = "Y";
                                }
                                else
                                {
                                    dr["Device"] = "";
                                }
                                

                                if (stu.MORE_INFORMEDCONSENT == "Y" && ((string)dr["HasConsentForm"] != "Y"))
                                {
                                    hasChanged = true;
                                    dr["HasConsentForm"] = "N";
                                }
                                else if ((string.IsNullOrWhiteSpace(stu.MORE_INFORMEDCONSENT) || stu.MORE_INFORMEDCONSENT == "N") && ((string)dr["HasConsentForm"] == "Y"))
                                {
                                    hasChanged = true;
                                    dr["HasConsentForm"] = "Y";
                                }
                                else
                                {
                                    dr["HasConsentForm"] = "";
                                }
                                

                                if (stu.STUDY_PI != newpi && !String.IsNullOrEmpty(newpi) && newpi != rcscpi.PI)
                                {
                                    hasChanged = true;
                                }
                                else { newpi = ""; }

                                if (stu.STUDY_ENTERED_BY != newrc && !String.IsNullOrEmpty(newrc) && newrc != rcscpi.RC)
                                {
                                    hasChanged = true;
                                }
                                else { newrc = ""; }

                                if (stu.STUDY_COORDINATOR != newsc && !String.IsNullOrEmpty(newsc) && newsc != rcscpi.SC)
                                {
                                    hasChanged = true;
                                }
                                else { newsc = ""; }


                                if (Tools.compareStr(stu.STUDY_TITLE, Tools.removeHtml((string)dr["StudyTitle"])) && !String.IsNullOrWhiteSpace(Tools.removeHtml((string)dr["StudyTitle"])))
                                {
                                    dr["Studytitle"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace((string)dr["StudyTitle"]))
                                {
                                    hasChanged = true;
                                }

                                /*if (Tools.compareStr(stu.STUDY_SUMMARY, Tools.removeHtml((string)dr["Studysummary"])) && !String.IsNullOrWhiteSpace(Tools.removeHtml((string)dr["Studysummary"])))
                                {
                                    dr["Studysummary"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace(Tools.removeHtml((string)dr["Studysummary"])))
                                {
                                    hasChanged = true;
                                }*/


                                dr["Studysummary"] = "";
                                dr["Department"] = "";
                                dr["Division"] = "";
                                dr["Cancer"] = "";
                                dr["Studysamplesize"] = "";


                                /*if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                                {
                                    string newdep = string.IsNullOrWhiteSpace((string)dr["Department"]) ? "" : IRISMap.Department.getDepartment((string)dr["Department"]);
                                    if (Tools.compareStr(newdep, stu.STUDY_DIVISION) && !string.IsNullOrWhiteSpace((string)dr["Department"]))
                                    {
                                        dr["Department"] = "";

                                    }
                                    else if (!string.IsNullOrWhiteSpace((string)dr["Department"]))
                                    {
                                        hasChanged = true;
                                        newdep = null;
                                    }

                                    string newDiv = string.IsNullOrWhiteSpace((string)dr["Department"]) ? "" : IRISMap.Department.getDivision((string)dr["Department"]);
                                    if (Tools.compareStr(newDiv, stu.STUDY_TAREA) && !string.IsNullOrWhiteSpace((string)dr["Department"]) && newdep != null)
                                    {
                                        dr["Department"] = "";
                                    }
                                    else if (!string.IsNullOrWhiteSpace((string)dr["Department"]))
                                    {
                                        hasChanged = true;
                                    }
                                }*/


                                /*int samplesize = 0;
                                Int32.TryParse((string)dr["Studysamplesize"], out samplesize);

                                if (stu.STUDY_NATSAMPSIZE == samplesize)
                                {
                                    dr["Studysamplesize"] = "";
                                }
                                else if (samplesize != 0)
                                {
                                    hasChanged = true;
                                }*/

                                bool cmp = (stu.STUDY_SCOPE == "Multi Center Study" && dr["Multicenter"].ToString().ToLower() == "yes") ||
                                    (stu.STUDY_SCOPE == "Single Center Study" && dr["Multicenter"].ToString().ToLower() == "no") ||
                                    (stu.STUDY_SCOPE == null && dr["Multicenter"].ToString().ToLower() == "");

                                if (cmp)
                                {
                                    dr["Multicenter"] = "";
                                }
                                else if (dr["Multicenter"].ToString() != "")
                                {
                                    hasChanged = true;
                                }

                                if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                                {
                                    string newphase = IRISMap.Phase.getPhase((string)dr["Phase"]);
                                    if (Tools.compareStr(newphase, stu.STUDY_PHASE) && !string.IsNullOrWhiteSpace((string)dr["Phase"]))
                                    {
                                        dr["Phase"] = "";
                                    }
                                    else if (!string.IsNullOrWhiteSpace((string)dr["Phase"]))
                                    {
                                        hasChanged = true;
                                    }
                                }

                                string newSponsor = "";
                                if (Agency.AgencyVal == Agency.AgencyList.BRANY || Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                                {
                                    newSponsor = BranySponsorMap.getSponsor((string)dr["Primarysponsorname"]);
                                }

                                if (Tools.compareStr(stu.SPONSOR_DD, newSponsor) && !String.IsNullOrWhiteSpace(newSponsor))
                                {
                                    dr["Primarysponsorname"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace(newSponsor))
                                {
                                    hasChanged = true;
                                }
                                else if (String.IsNullOrWhiteSpace(newSponsor) && !String.IsNullOrWhiteSpace((string)dr["Primarysponsorname"]))
                                {
                                    string otherSponsor = "Per IRB System: " + (string)dr["Primarysponsorname"];
                                    if (Tools.compareStr(stu.STUDY_INFO, otherSponsor) && !String.IsNullOrWhiteSpace(otherSponsor))
                                    {
                                        dr["Primarysponsorname"] = "";
                                    }
                                    else if (!String.IsNullOrWhiteSpace(otherSponsor))
                                    {
                                        hasChanged = true;
                                    }

                                }


                                string[] strs = {dr["Primarysponsorcontactfirstname"].ToString(),
                                dr["Primarysponsorcontactlastname"].ToString()};

                                if (Tools.containStr(stu.SPONSOR_CONTACT, strs) && !String.IsNullOrWhiteSpace((string)dr["Primarysponsorcontactfirstname"]) && !String.IsNullOrWhiteSpace((string)dr["Primarysponsorcontactlastname"]))
                                {
                                    dr["Primarysponsorcontactfirstname"] = "";
                                    dr["Primarysponsorcontactlastname"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace((string)dr["Primarysponsorcontactfirstname"]) && !String.IsNullOrWhiteSpace((string)dr["Primarysponsorcontactlastname"]))
                                {
                                    hasChanged = true;
                                }

                                if (Tools.compareStr(stu.STUDY_SPONSORID, dr["PrimarysponsorstudyID"]) && !String.IsNullOrWhiteSpace((string)dr["PrimarysponsorstudyID"]))
                                {
                                    //dr["PrimarysponsorstudyID"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace((string)dr["PrimarysponsorstudyID"]))
                                {
                                    hasChanged = true;
                                }

                                for (int i = fpstudys.initColumnCount - 1; i < fpstudys.data.Columns.Count - 1; i++)
                                {
                                    dr[i] = "";
                                }

                                //TEMPORARY
                                //hasChanged = true;


                            }

                            if (hasChanged)
                            {
                                addRowStudy(dr, false, newpi, newrc, newsc, newcro);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add a new row to the study table
        /// </summary>
        /// <param name="irbNumber"></param>
        /// <param name="studyNumber"></param>
        private static void addRowStudy(DataRow row, bool newentry, string newpi = null, string newrc = null, string newsc = null, string newcro = null)
        {
            initiate();
            DataRow dr;
            if (newentry)
            { dr = newStudy.NewRow(); }
            else
            { dr = updatedStudy.NewRow(); }


            dr["IRB Agency name"] = (string)row["IRBAgency"];
            dr["IRB no"] = (string)row["IRBNUMBER"];

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Cancer"] = (string)row["MSDCANCER_RELATED_STUDY"];
            }

            /*
            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["IRB Agency name"] = (string)row["IRBAgency"];
                dr["IRB no"] = (string)row["IRBNUMBER"];
                dr["Cancer"] = (string)row["MSDCANCER_RELATED_STUDY"];
            }
            if (!string.IsNullOrWhiteSpace(((string)row["ExternalIRB"])))
            {
                dr["IRB no"] = (string)row["ExternalIRBnumber"];
                if (((string)row["ExternalIRB"]).ToLower().Contains("neuronext"))
                    dr["IRB Agency name"] = "neuronext";
                else if (((string)row["ExternalIRB"]).ToLower().Contains("strokenet"))
                    dr["IRB Agency name"] = "strokenet";
                else if (((string)row["ExternalIRB"]).ToLower().Contains("nci") || ((string)row["ExternalIRB"]).ToLower().Contains("cirb"))
                    dr["IRB Agency name"] = "nci";
                else// if (((string)row["ExternalIRB"]).ToLower().Contains("external"))
                    dr["IRB Agency name"] = "other";
            }
            else
            {
                dr["IRB Agency name"] = Agency.agencyStrLwr;
                dr["IRB no"] = ((string)row["IRBNumber"]).Replace("(IBC)", "");
            }*/

            dr["IRB Study ID"] = (string)row["StudyId"];
            dr["IRB Identifiers"] = Tools.generateStudyIdentifiers((string)row["StudyId"]);

            dr["Device"] = (string)row["Device"];
            dr["Drug"] = (string)row["Drug"];
            dr["Consent"] = (string)row["HasConsentForm"];

            if (!string.IsNullOrWhiteSpace((string)row["newNumber"]))
            {
                dr["Study_number"] = row["oldNumber"];
                dr["New_Number"] = row["newNumber"];
            }
            else
            {
                dr["Study_number"] = row["oldNumber"];
                if (dr.Table.Columns.Contains("New_Number"))
                {
                    dr["New_Number"] = "";
                }
            }

            //TEMPORARY
            //dr["Accronym"] = row["StudyAcronym"];

            if (newpi == null)
            {
                dr["Principal Investigator"] = getPI((string)row["StudyId"]);
            }
            else
            {
                dr["Principal Investigator"] = newpi;
            }

            if (newrc == null)
            {
                dr["Regulatory_coordinator"] = getRC((string)row["StudyId"]);
            }
            else
            {
                dr["Regulatory_coordinator"] = newrc;
            }

            if (newsc == null)
            {
                dr["Study_coordinator"] = getSC((string)row["StudyId"]);
            }
            else
            {
                dr["Study_coordinator"] = newsc;
            }

            /*if (newcro == null)
            {
                dr["CRO"] = getCRO((string)row["StudyId"]);
            }
            else
            {
                dr["CRO"] = newcro;
            }*/

            dr["Official title"] = Tools.removeHtml((string)row["StudyTitle"]);
            dr["Study summary"] = Tools.removeHtml((string)row["Studysummary"]);

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Department"] = String.IsNullOrEmpty((string)row["Department"]) && newentry ? "Please specify" : (string)row["Department"];
                dr["Division/Therapeutic area"] = String.IsNullOrEmpty((string)row["Division"]) && newentry ? "N/A" : (string)row["Division"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Department"] = String.IsNullOrEmpty((string)row["Department"]) && newentry ? "Please specify" : (string)row["Department"];
                dr["Division/Therapeutic area"] = String.IsNullOrEmpty((string)row["Division"]) && newentry ? "N/A" : (string)row["Division"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                dr["Department"] = String.IsNullOrWhiteSpace((string)row["Department"]) ? "" : IRISMap.Department.getDepartment((string)row["Department"]);
                dr["Division/Therapeutic area"] = String.IsNullOrWhiteSpace((string)row["Department"]) ? "" : IRISMap.Department.getDivision((string)row["Department"]);
            }

            if (fpstudys.initColumnCount < fpstudys.data.Columns.Count)
            {
                for (int i = fpstudys.initColumnCount; i <= fpstudys.data.Columns.Count; i++)
                {
                    string colname = fpstudys.data.Columns[i - 1].ColumnName;
                    if (!dr.Table.Columns.Contains(colname)) dr.Table.Columns.Add(colname);
                    dr[colname] = row[colname];
                }
            }

            int size = 0;
            int.TryParse((string)row["Studysamplesize"], out size);
            dr["Entire study sample size"] = size == 0 ? "" : size.ToString();

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Phase"] = String.IsNullOrEmpty((string)row["Phase"]) && newentry ? "Please Specify" : (string)row["Phase"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Phase"] = String.IsNullOrEmpty((string)row["Phase"]) && newentry ? "Please Specify" : (string)row["Phase"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                dr["Phase"] = String.IsNullOrWhiteSpace((string)row["Phase"]) ? "" : IRISMap.Phase.getPhase((string)row["Phase"]);
            }


            if (Tools.compareStr(row["Multicenter"].ToString(), "true") || Tools.compareStr(row["Multicenter"].ToString(), "yes") || Tools.compareStr(row["Multicenter"].ToString(), "y") || ((string)row["Multicenter"]).ToLower().Contains("multi"))
                dr["Study scope"] = "Multi Center Study";
            else if (Tools.compareStr(row["Multicenter"].ToString(), "false") || Tools.compareStr(row["Multicenter"].ToString(), "no") || Tools.compareStr(row["Multicenter"].ToString(), "n") || ((string)row["Multicenter"]).ToLower().Contains("single"))
                dr["Study scope"] = "Single Center Study";
            else
                dr["Study scope"] = "";

            //dr["Primary funding sponsor, if other :"] = row["Primarysponsorname"].ToString();

            if ((Agency.AgencyVal == Agency.AgencyList.BRANY && Agency.AgencySetupVal == Agency.AgencyList.BRANY) || (Agency.AgencyVal == Agency.AgencyList.EINSTEIN && Agency.AgencySetupVal == Agency.AgencyList.EINSTEIN))
            {
                string sponsor = BranySponsorMap.getSponsor((string)row["Primarysponsorname"]);
                if (String.IsNullOrWhiteSpace(sponsor) && !string.IsNullOrWhiteSpace((string)row["Primarysponsorname"]))
                {
                    dr["Sponsor information other"] = "Per IRB System: " + (string)row["Primarysponsorname"];
                }
                else if (!String.IsNullOrWhiteSpace(sponsor))
                {
                    dr["Primary funding sponsor"] = sponsor;
                }
            }
            else if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Primary funding sponsor"] = (string)row["Primarysponsorname"];
            }
            else if (!String.IsNullOrWhiteSpace((string)row["Primarysponsorname"]))
            {
                dr["Sponsor information other"] = "Per IRB System: " + (string)row["Primarysponsorname"];
            }

            dr["Sponsor contact"] = row["PrimarySponsorContactFirstName"].ToString() + " " + row["PrimarySponsorContactLastName"].ToString();
            dr["Sponsor Protocol ID"] = row["PrimarySponsorStudyId"].ToString();

            if (newentry)
            {
                dr["Cancer"] = isStudyCancer(row) ? "Y" : "";
            }
            else
            {
                dr["Cancer"] = row["Cancer"];
            }

            string[] labels = new string[] { };
            string[] values = new string[] { };

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                labels = new string[17] { 
                    "Study Financials Managed By**", 
                    //"CRO, if any*", 
                    "IRB agency name", 
                    "IRB No.", 
                    "Is this a cancer related study ?", 
                    "PI Program Code (Cancer Center Studies Only)*", 
                    "Primary Purpose*", 
                    "&nbsp;&nbsp;&nbsp;Agent", 
                    "&nbsp;&nbsp;&nbsp;Ancillary", 
                    "&nbsp;&nbsp;&nbsp;Correlative (Laboratary based specimen studies &nbsp;&nbsp;&nbsp;to access risks)",
                    "&nbsp;&nbsp;&nbsp;Device",
                    "&nbsp;&nbsp;&nbsp;Epidemiologic",
                    "&nbsp;&nbsp;&nbsp;In Vitro",
                    "&nbsp;&nbsp;&nbsp;Other Observational Studies",
                    "&nbsp;&nbsp;&nbsp;Retrospective chart review",
                    "&nbsp;&nbsp;&nbsp;Tissue Banking",
                    "&nbsp;&nbsp;&nbsp;Trials Involving Interventions",
                    "Responsible Party*"  
                };

                values = new string[17] { 
                    (string)dr["STUDY_MANAGED_BY_IMPORT"],
                    //(string)dr["CRO"], 
                    (string)dr["IRB Agency name"], 
                    (string)dr["IRB no"], 
                    (string)dr["Cancer"],
                    (string)dr["Program_Code_Mapped"],
                    (string)dr["PRIMARY_PURPOSE"],
                    (string)dr["MSD_AGENT"],
                    (string)dr["ANCILLARY"],
                    (string)dr["CORRELATIVE"],
                    (string)dr["DEVICE"],
                    (string)dr["EPIDEMIOLOGIC"],
                    (string)dr["IN_VITRO"],
                    (string)dr["OTHER_Observational_STUDIES"],
                    (string)dr["RETROSPECTIVE_CHART_REVIEW"],
                    (string)dr["TISSUE BANKING"],
                    (string)dr["TRIALS_Involving_INTERVENTIONS"],
                    (string)dr["RESPONSIBLE_PARTY"]
                    

                };
            }
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                labels = new string[4] { "Study Financials Managed By*", 
                    //"CRO, if any*", 
                    "IRB agency name", "IRB No.", "Is this a cancer related study ?" };

                values = new string[4] { "BRY", 
                    //(string)dr["CRO"], 
                    Agency.agencyStrLwr, (string)dr["IRB no"], (string)dr["Cancer"] };
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                labels = new string[6] { "IRB agency name", "IRB No.", "Is this a cancer related study ?", "Is there an Informed Consent associated to study? ", "   Agent ", "   Device" };

                values = new string[6] { (string)dr["IRB Agency name"], (string)dr["IRB no"], (string)dr["Cancer"], (string)dr["Consent"], (string)dr["Drug"], (string)dr["Device"] };
            }

            OutputMSD.initiate();

            for (int i = 0; i < labels.Count(); i++)
            {
                if (!string.IsNullOrWhiteSpace(values[i]))
                {
                    OutputMSD.addRow(labels[i], values[i], (string)row["StudyId"], (string)dr["IRB no"], (string)row["StudyAcronym"], newentry);
                }
            }


            OutputIRBForm.addIds((string)dr["Study_number"], (string)dr["IRB Identifiers"]);

            if (fpstudys.initColumnCount < row.Table.Columns.Count)
            {
                for (int i = fpstudys.initColumnCount; i < row.Table.Columns.Count; i++)
                {
                    if (!dr.Table.Columns.Contains(row.Table.Columns[i].ColumnName))
                    {
                        dr.Table.Columns.Add(row.Table.Columns[i].ColumnName);
                    }
                    dr[row.Table.Columns[i].ColumnName] = row[i];
                }
            }


            if (newentry)
            { newStudy.Rows.Add(dr); }
            else
            { updatedStudy.Rows.Add(dr); }

        }

        /// <summary>
        /// Returns the PI for that study
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static string getPI(string studyId)
        {
            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                var value = fpstudys.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId).FirstOrDefault();
                if (value != null)
                    return (string)value["PI"];
            }
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                return getRole(studyId, BranyRoleMap.PI);
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                //string rcemail = OutputTeam.team.FirstOrDefault(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower()) && x.ROLE == Role).USER_EMAIL;
                var study = studys.FirstOrDefault(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower()));
                string piname = study == null ? "" : study.STUDY_PI;
                piname = String.IsNullOrWhiteSpace(piname) ? "" : piname;
                string piemail = piname == "" ? "" : OutputTeam.accounts.FirstOrDefault(x => x.USER_NAME == piname).USER_EMAIL;
                if (getRoleNotChange(studyId, IRISMap.RoleMap.PI, piname, piemail))
                {
                    return piname;
                }
                else
                {
                    return getRole(studyId, IRISMap.RoleMap.PI);
                }
            }
            else
            {
                return getRole(studyId, IRISMap.RoleMap.PI);
            }
        }

        /*/// <summary>
        /// returns the PI email
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static string getPIeMail(string studyId)
        {
            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                return "";
            }
            var studyteam = OutputTeam.fpTeam.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId);
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                return (string)studyteam.FirstOrDefault(x => (string)x["Role"] == BranyRoleMap.PI && (string)x["Primary"] == "Y")["PrimaryEMailAddress"];
            }
            else
            {
                return (string)studyteam.FirstOrDefault(x => (string)x["Role"] == IRISMap.RoleMap.PI)["PrimaryEMailAddress"];
            }

        }*/


        /// <summary>
        /// Returns the RC for that study
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static string getRC(string studyId)
        {
            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                var value = fpstudys.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId).FirstOrDefault();
                if (value != null)
                    return (string)value["RC"];
            }
            string retstr = "";
            var study = studys.FirstOrDefault(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower()));
            string rcname = study == null ? "" : study.STUDY_ENTERED_BY;
            string rcemail = String.IsNullOrWhiteSpace(rcname) ? "" : OutputTeam.accounts.FirstOrDefault(x => x.USER_NAME == rcname).USER_EMAIL;
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                if (getRoleNotChange(studyId, BranyRoleMap.RC, rcname, rcemail))
                {
                    retstr = rcname;
                }
                else
                {
                    retstr = getRole(studyId, BranyRoleMap.RC);
                }
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                //string rcemail = OutputTeam.team.FirstOrDefault(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower()) && x.ROLE == Role).USER_EMAIL;

                if (getRoleNotChange(studyId, IRISMap.RoleMap.RC1, rcname, rcemail) || getRoleNotChange(studyId, IRISMap.RoleMap.RC2, rcname, rcemail))
                {
                    retstr = rcname;
                }
                else
                {
                    retstr = getRole(studyId, IRISMap.RoleMap.RC1);
                    if (String.IsNullOrWhiteSpace(retstr))
                        retstr = getRole(studyId, IRISMap.RoleMap.RC2);
                }
            }
            else
            {
                retstr = getRole(studyId, IRISMap.RoleMap.RC1);
                if (String.IsNullOrWhiteSpace(retstr))
                    retstr = getRole(studyId, IRISMap.RoleMap.RC2);
            }

            retstr = String.IsNullOrWhiteSpace(retstr) ? getPI(studyId) : retstr;

            return retstr;
        }



        /// <summary>
        /// Returns the SC for that study
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static string getSC(string studyId)
        {
            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                var value = fpstudys.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId).FirstOrDefault();
                if (value != null)
                    return (string)value["SC"];
            }
            string retstr = "";
            if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                retstr = getRole(studyId, IRISMap.RoleMap.SC);
            }
            else
            {
                retstr = getRole(studyId, BranyRoleMap.RC, true);
            }

            return retstr;
        }


        /// <summary>
        /// Returns the CRO for that study
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        private static string getCRO(string studyId)
        {
            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                var value = fpstudys.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId).FirstOrDefault();
                return (string)value["CRO"];
            }
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                return getRole(studyId, BranyRoleMap.CRO);
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// General function to return a specific role for that study, for brany it will only look at roles where the primary flag is true
        /// </summary>
        /// <param name="studyId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private static string getRole(string studyId, string role, bool getSecond = false)
        {
            var studyteam = OutputTeam.fpTeam.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId);
            if (Agency.AgencyVal == Agency.AgencyList.BRANY && !getSecond)
            {
                return Tools.getFullName(studyteam.FirstOrDefault(x => (string)x["Role"] == role && (string)x["Primary"] == "Y"));
            }
            if (Agency.AgencyVal == Agency.AgencyList.BRANY && getSecond)
            {
                return Tools.getFullName(studyteam.FirstOrDefault(x => (string)x["Role"] == role && (string)x["Primary"] == "N"));
            }
            else if (getSecond)
            {
                string output = "";
                var studPeop = studyteam.Where(x => (string)x["Role"] == role);
                if (studPeop.Count() > 1)
                {
                    var peop = studPeop.ElementAt(1);
                    output = Tools.getFullName(peop);
                }
                return output;
            }
            else
            {
                return Tools.getFullName(studyteam.FirstOrDefault(x => (string)x["Role"] == role));
            }
        }

        /// <summary>
        /// Check if the datasource has the following name/email has the specified roles, used for multiple member per role
        /// </summary>
        /// <param name="studyId"></param>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private static bool getRoleNotChange(string studyId, string role, string name, string email)
        {
            if (name == "" && email == "")
            {
                return false;
            }
            else
            {
                return OutputTeam.fpTeam.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId
                    && (string)x["Role"] == role
                    && ((Tools.compareStr((string)x["FirstName"] + " " + (string)x["LastName"], name))
                    || Tools.compareStr((string)x["PrimaryEMailAddress"], email))
                    ).Any();
            }
        }

        /// <summary>
        /// Verify if a studyId is present in the datasource
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static bool isStudyInDataSource(string studyId)
        {
            return (from st in fpstudys.data.AsEnumerable()
                    where st.Field<string>("StudyId").Trim().ToLower() == studyId.Trim().ToLower()
                    select st).Any();
        }


        /*/// <summary>
        /// Check if a study was already added in the modified or new study list
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static bool isStudyInOuput(string studyId)
        {
            bool ret = false;
            ret = (from st in newStudy.AsEnumerable()
                   where st.Field<string>("StudyId").Trim().ToLower() == studyId.Trim().ToLower()
                       select st).Any()
                    ||
                    (from st in updatedStudy.AsEnumerable()
                   where st.Field<string>("StudyId").Trim().ToLower() == studyId.Trim().ToLower()
                   select st).Any();

            return ret;


            return (from st in newStudy.AsEnumerable()
                where newStudy.Field<string>("StudyId").Trim().ToLower() == studyId.Trim().ToLower()
                select st).Any();
        }*/


        /// <summary>
        /// Return true if the study should be analysed / added
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static bool shouldStudyBeAdded(string studyId)
        {
            /*if (studyId.Contains("6885e452-44ed-4306-8d6a-27e67f31f4aa"))
            {
                int a = 1;
                a = a + 1;
            }*/

            if (String.IsNullOrWhiteSpace(studyId) && Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                return true;
            }

            var study = (from st in fpstudys.data.AsEnumerable()
                         where studyId.Trim().ToLower() == st.Field<string>("StudyId").Trim().ToLower()
                         && !st.Field<string>("IRBNumber").Trim().ToLower().Contains("ibc")
                         select st).FirstOrDefault();


            if (study != null && !String.IsNullOrWhiteSpace((string)study["IRBNumber"]))
            {
                return shouldStudyBeAdded(study);
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Return true if the study should be analysed / added
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static bool shouldStudyBeAdded(DataRow dr)
        {
            /*if (((string)dr["IRBNumber"]).Contains("12-06-130-01"))
            {
                int a = 1;
                a = a + 1;
            }*/
            if (SpecialStudys.forceInclude.Any(x => Tools.compareStr(x.number, (string)dr["IRBNumber"])))
            {
                var a = Tools.getOldStudy((string)dr["StudyId"]);
                return true;
            }

            if (Tools.getOldStudy((string)dr["StudyId"]))
            {
                return true;
            }



            if (SpecialStudys.checkConsentAgentAndDevice && dr.Table.Columns.Contains("HasConsentForm") && dr.Table.Columns.Contains("PhaseDrugDevice"))
            {
                if ((string)dr["HasConsentForm"] == "N" && (string)dr["PhaseDrugDevice"] == "N")
                {
                    return false;
                }
            }

            if (SpecialStudys.ignoredIrbNumbers.Count >= 1 && SpecialStudys.ignoredIrbNumbers.Contains((string)dr["IRBNumber"]))
            {
                return false;
            }

            bool ignoreLatestStatus = false;

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                ignoreLatestStatus = (SpecialStudys.ignoredStatusBRANY.Count >= 1 && SpecialStudys.ignoredStatusBRANY.Any(x => Tools.compareStr(x, OutputStatus.getLatestStatusFP((string)dr["StudyId"]))));
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                ignoreLatestStatus = (SpecialStudys.ignoredStatusIRIS.Count >= 1 && SpecialStudys.ignoredStatusIRIS.Any(x => Tools.compareStr(x, OutputStatus.getLatestStatusFP((string)dr["StudyId"]))));
            }
            else
            {
                ignoreLatestStatus = (SpecialStudys.ignoredStatus.Count >= 1 && SpecialStudys.ignoredStatus.Any(x => Tools.compareStr(x, OutputStatus.getLatestStatusFP((string)dr["StudyId"]))));
            }

            if (ignoreLatestStatus)
                return false;

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                if (SpecialStudys.studyToInclude.Count >= 1 && SpecialStudys.studyToInclude.Any(x => Tools.compareStr(x.number, (string)dr["IRBNumber"])))
                {
                    return true;
                }
                else if (SpecialStudys.studyToInclude.Count >= 1)
                {
                    return false;
                }
                else return true;
            }

            if (!((string)dr["ExternalIRB"]).Trim().ToLower().Contains("brany") && !((string)dr["StudyId"]).Trim().ToLower().Contains("corrupted"))
            {

                if (SpecialStudys.ignoredStudys.Any(x => x.IRB == Agency.agencyStrLwr && Tools.compareStr(x.number, (string)dr["IRBNumber"])))
                {
                    return false;
                }



                if (SpecialStudys.studyToInclude.Count >= 1 && SpecialStudys.studyToInclude.Any(x => x.IRB == Agency.agencyStrLwr && Tools.compareStr(x.number, (string)dr["IRBNumber"])))
                {
                    return true;
                }
                else if (SpecialStudys.studyToInclude.Count >= 1)
                {
                    return false;
                }

                if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    if (closedStatusInVelos.Contains(OutputStatus.getLatestStatus((string)dr["StudyId"])))
                    {
                        return false;
                    }

                    var irbno = ((string)dr["IRBNumber"]).Split('-');
                    if (irbno.Count() >= 2 && irbno[1] == "06") { return true; }


                    if (SpecialStudys.cancerTerms.Count >= 1)
                    {
                        foreach (var str in SpecialStudys.cancerTerms)
                        {
                            if (((string)dr["StudyTitle"]).ToLower().Contains(str) ||
                                ((string)dr["PrimarySponsorName"]).ToLower().Contains(str) ||
                                ((string)dr["StudySummary"]).ToLower().Contains(str))
                            {
                                return true;
                            }
                            /*cancerfilter = ((string)dr["StudyTitle"]).ToLower().Contains(str) || cancerfilter;
                            cancerfilter = ((string)dr["PrimarySponsorName"]).ToLower().Contains(str) || cancerfilter;*/
                        }
                        return false;
                    }
                    else
                    {
                        //cancerfilter = true;
                        return true;
                    }
                }
                else
                {
                    if (((string)dr["Division"]).ToLower().Contains("oncol") ||
                        ((string)dr["Department"]).ToLower().Contains("oncol") ||
                        ((string)dr["Cancer"]).ToLower().Contains("yes"))
                    {
                        return true;
                    }
                    if (SpecialStudys.cancerTerms.Count >= 1)
                    {
                        foreach (var str in SpecialStudys.cancerTerms)
                        {
                            if (((string)dr["StudyTitle"]).ToLower().Contains(str) || ((string)dr["PrimarySponsorName"]).ToLower().Contains(str))
                            {
                                return true;
                            }
                            /*cancerfilter = ((string)dr["StudyTitle"]).ToLower().Contains(str) || cancerfilter;
                            cancerfilter = ((string)dr["PrimarySponsorName"]).ToLower().Contains(str) || cancerfilter;*/
                        }
                        return false;
                    }
                    else
                    {
                        //cancerfilter = true;
                        return true;
                    }

                }


            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Return 'Y' if it is cancer related (cancer term search and 06 in BRANY IRB no), N else
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static bool isStudyCancer(DataRow dr)
        {
            if ((string)dr["Cancer"] == "Y")
            {
                return true;
            }
            if (SpecialStudys.cancerTerms.Count >= 1)
            {

                foreach (var str in SpecialStudys.cancerTerms)
                {
                    if (((string)dr["StudyTitle"]).ToLower().Contains(str) ||
                        ((string)dr["PrimarySponsorName"]).ToLower().Contains(str) ||
                        ((string)dr["StudySummary"]).ToLower().Contains(str))
                    {
                        return true;
                    }
                    /*cancerfilter = ((string)dr["StudyTitle"]).ToLower().Contains(str) || cancerfilter;
                    cancerfilter = ((string)dr["PrimarySponsorName"]).ToLower().Contains(str) || cancerfilter;*/
                }
            }

            string cancer = (string)dr["Cancer"];
            if (Agency.AgencySetupVal == Agency.AgencyList.NONE || Agency.AgencySetupVal == Agency.AgencyList.BOTH)
            {
                return true;
            }
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                string[] irbno = ((string)dr["IRBNumber"]).Split('-');
                if (irbno.Count() >= 2 && irbno[1].Trim() == "06")
                    return true;
            }

            return false;
        }

    }
}
