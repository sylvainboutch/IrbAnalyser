using System;
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
                newStudy.Columns.Add("New_Number", typeof(string));
                newStudy.Columns.Add("Regulatory_coordinator", typeof(string));
                newStudy.Columns.Add("Division/Therapeutic area", typeof(string));
                newStudy.Columns.Add("Phase", typeof(string));
                newStudy.Columns.Add("Official title", typeof(string));
                newStudy.Columns.Add("Study Summary", typeof(string));
                newStudy.Columns.Add("Principal Investigator", typeof(string));
                newStudy.Columns.Add("PIMajorAuthor", typeof(string));
                newStudy.Columns.Add("Study_coordinator", typeof(string));
                newStudy.Columns.Add("IND_Holder", typeof(string));
                newStudy.Columns.Add("IND_NUMBERS", typeof(string));

                newStudy.Columns.Add("AgentDevice", typeof(string));



                newStudy.Columns.Add("Department", typeof(string));
                newStudy.Columns.Add("Entire study sample size", typeof(string));
                newStudy.Columns.Add("Study scope", typeof(string));
                newStudy.Columns.Add("Primary funding sponsor", typeof(string));
                newStudy.Columns.Add("Sponsor Protocol ID", typeof(string));
                newStudy.Columns.Add("Sponsor contact", typeof(string));
                newStudy.Columns.Add("Sponsor information other", typeof(string));
                newStudy.Columns.Add("KeyWords", typeof(string));
                newStudy.Columns.Add("IRB Agency name", typeof(string));
                newStudy.Columns.Add("IRB no", typeof(string));
                newStudy.Columns.Add("IRB Study ID", typeof(string));
                newStudy.Columns.Add("IRB Identifiers", typeof(string));
                newStudy.Columns.Add("CRO", typeof(string));

                newStudy.Columns.Add("RecordCategory", typeof(string));

                newStudy.Columns.Add("FinancialBy", typeof(string));
                newStudy.Columns.Add("Cancer", typeof(string));
                newStudy.Columns.Add("Consent", typeof(string));

                newStudy.Columns.Add("Agent", typeof(string));
                newStudy.Columns.Add("Biological", typeof(string));
                newStudy.Columns.Add("BLood_Draw", typeof(string));
                newStudy.Columns.Add("Data_Collection", typeof(string));
                newStudy.Columns.Add("Device", typeof(string));
                newStudy.Columns.Add("EMERGENCY_INVESTIGATIONAL", typeof(string));
                newStudy.Columns.Add("HUMANITARIAN_USE", typeof(string));
                newStudy.Columns.Add("QI_STUDY", typeof(string));
                newStudy.Columns.Add("RETROSPECTIVE_CHART_REVIEW", typeof(string));
                newStudy.Columns.Add("Survey", typeof(string));
                newStudy.Columns.Add("TISSUE_BANKING", typeof(string));
                newStudy.Columns.Add("TRIALS_Involving_INTERVENTIONS", typeof(string));

                newStudy.Columns.Add("CT_FDA", typeof(string));
                newStudy.Columns.Add("CT_ICMJE", typeof(string));
                newStudy.Columns.Add("CT_NIH", typeof(string));

                newStudy.Columns.Add("SpecimenDataAnalysis", typeof(string));


                newStudy.Columns.Add("NCT_NUMBER", typeof(string));
                newStudy.Columns.Add("pk_study", typeof(string));

                newStudy.Columns.Add("SignOffBy", typeof(string));

            }

            if (updatedStudy.Columns.Count == 0)
            {/*
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
                updatedStudy.Columns.Add("Agent", typeof(string));
                updatedStudy.Columns.Add("Consent", typeof(string));
                updatedStudy.Columns.Add("NCTNumber", typeof(string));
                //updatedStudy.Columns.Add("Accronym", typeof(string));
              */
                updatedStudy.Columns.Add("Study_number", typeof(string));
                updatedStudy.Columns.Add("New_Number", typeof(string));
                updatedStudy.Columns.Add("Regulatory_coordinator", typeof(string));
                updatedStudy.Columns.Add("Division/Therapeutic area", typeof(string));
                updatedStudy.Columns.Add("Phase", typeof(string));
                updatedStudy.Columns.Add("Official title", typeof(string));
                updatedStudy.Columns.Add("Study Summary", typeof(string));
                updatedStudy.Columns.Add("Principal Investigator", typeof(string));
                updatedStudy.Columns.Add("PIMajorAuthor", typeof(string));
                updatedStudy.Columns.Add("Study_coordinator", typeof(string));
                updatedStudy.Columns.Add("IND_Holder", typeof(string));
                updatedStudy.Columns.Add("IND_NUMBERS", typeof(string));

                updatedStudy.Columns.Add("AgentDevice", typeof(string));



                updatedStudy.Columns.Add("Department", typeof(string));
                updatedStudy.Columns.Add("Entire study sample size", typeof(string));
                updatedStudy.Columns.Add("Study scope", typeof(string));
                updatedStudy.Columns.Add("Primary funding sponsor", typeof(string));
                updatedStudy.Columns.Add("Sponsor Protocol ID", typeof(string));
                updatedStudy.Columns.Add("Sponsor contact", typeof(string));
                updatedStudy.Columns.Add("Sponsor information other", typeof(string));
                updatedStudy.Columns.Add("KeyWords", typeof(string));
                updatedStudy.Columns.Add("IRB Agency name", typeof(string));
                updatedStudy.Columns.Add("IRB no", typeof(string));
                updatedStudy.Columns.Add("IRB Study ID", typeof(string));
                updatedStudy.Columns.Add("IRB Identifiers", typeof(string));
                updatedStudy.Columns.Add("CRO", typeof(string));

                updatedStudy.Columns.Add("RecordCategory", typeof(string));

                updatedStudy.Columns.Add("FinancialBy", typeof(string));
                

                updatedStudy.Columns.Add("Cancer", typeof(string));
                updatedStudy.Columns.Add("Consent", typeof(string));

                updatedStudy.Columns.Add("Agent", typeof(string));
                updatedStudy.Columns.Add("Biological", typeof(string));
                updatedStudy.Columns.Add("BLood_Draw", typeof(string));
                updatedStudy.Columns.Add("Data_Collection", typeof(string));
                updatedStudy.Columns.Add("Device", typeof(string));
                updatedStudy.Columns.Add("EMERGENCY_INVESTIGATIONAL", typeof(string));
                updatedStudy.Columns.Add("HUMANITARIAN_USE", typeof(string));
                updatedStudy.Columns.Add("QI_STUDY", typeof(string));
                updatedStudy.Columns.Add("RETROSPECTIVE_CHART_REVIEW", typeof(string));
                updatedStudy.Columns.Add("Survey", typeof(string));
                updatedStudy.Columns.Add("TISSUE_BANKING", typeof(string));
                updatedStudy.Columns.Add("TRIALS_Involving_INTERVENTIONS", typeof(string));

                updatedStudy.Columns.Add("CT_FDA", typeof(string));
                updatedStudy.Columns.Add("CT_ICMJE", typeof(string));
                updatedStudy.Columns.Add("CT_NIH", typeof(string));

                updatedStudy.Columns.Add("SpecimenDataAnalysis", typeof(string));


                updatedStudy.Columns.Add("NCT_NUMBER", typeof(string));
                updatedStudy.Columns.Add("pk_study", typeof(string));

                updatedStudy.Columns.Add("SignOffBy", typeof(string));
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
                {
                    dr["KeyWords"] = "IRIS=" + (string)dr["IRBNumber"];
                    irbagency = "NeuroNext CIRB";
                }
                else if (((string)dr["ExternalIRB"]).ToLower().Contains("strokenet"))
                {
                    dr["KeyWords"] = "IRIS=" + (string)dr["IRBNumber"];
                    irbagency = "StrokeNet CIRB";
                }
                else if (((string)dr["ExternalIRB"]).ToLower().Contains("nci") || ((string)dr["ExternalIRB"]).ToLower().Contains("cirb"))
                {
                    dr["KeyWords"] = "IRIS=" + (string)dr["IRBNumber"];
                    irbagency = "NCI CIRB";
                }
                else// if (((string)row["ExternalIRB"]).ToLower().Contains("external"))
                {
                    irbagency = "";
                }
            }
            else
            {
                irbagency = Agency.AgencyVal == Agency.AgencyList.EINSTEIN ? "Einstein IRB" : irbagency;
                irbagency = Agency.AgencyVal == Agency.AgencyList.BRANY ? "BRANY" : irbagency;
                irbnumber = ((string)dr["IRBNumber"]).Replace("(IBC)", "");
            }

            dr["IRBAgency"] = irbagency;
            dr["IRBNumber"] = irbnumber;


            if (shouldStudyBeAdded(irbstudyId))
            {
                string identifiers = Tools.generateStudyIdentifiers((string)dr["StudyId"]);
                string number = Tools.getOldStudyNumber((string)dr["StudyId"]);

                dr["RecordCategory"] = "Study";

                OutputIRBForm.addIds(number, identifiers);

                //external IRB only available in IRIS, we ignore studies with external IRB = BRANY since we have them in the BRANY file. Corrupted studies are in IRIS and shouldnt be pull in, they are a result of their data migration.
                //empty study ID shouldnt happen but could indicate an empty line in the file.
                if (!String.IsNullOrEmpty(irbstudyId) && !((string)dr["StudyId"]).ToLower().Contains("corrupted") && !((string)dr["ExternalIRB"]).ToLower().Contains("brany"))
                {
                    var study = from st in studys
                                where st.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                select st;

                    if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                    {
                        OutputTeam.addRowXForm(irbstudyId, number, irbnumber);

                        //XmlTools xmltool = new XmlTools();
                        /*string category = xmltool.getStudyType((string)dr["StudyId"]);
                        if (category == "")
                        {
                            dr["Agent"] = "Yes";
                            dr["Biological"] = "";
                            dr["BLood_Draw"] = "";
                            dr["Data_Collection"] = "";
                            dr["Device"] = "";
                            dr["EMERGENCY_INVESTIGATIONAL"] = "";
                            dr["HUMANITARIAN_USE"] = "";
                            dr["QI_STUDY"] = "";
                            dr["RETROSPECTIVE_CHART_REVIEW"] = "";
                            dr["Survey"] = "";
                            dr["TISSUE_BANKING"] = "";
                            dr["TRIALS_Involving_INTERVENTIONS"] = "";
                            dr["SpecimenDataAnalysis"] = "";
                            dr["NCT_NUMBER"] = "";

                        }*/

                    }

                    XmlTools xmltool = new XmlTools();
                    xmltool.fillDataRow(dr);

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

                            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                            {
                                dr["FinancialBy"] = "BRY";
                                dr["SignOffBy"] = "BRY";                                
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



                                hasChanged = checkChangeOverwriteString("IRBNumber", dr, stu.MORE_IRBNUM, hasChanged);

                                hasChanged = checkChangeOverwriteString("IRBAgency", dr, stu.MORE_IRBAGENCY, hasChanged);

                                /*if (!string.IsNullOrWhiteSpace(stu.MORE_IRBNUM) && stu.MORE_IRBNUM.ToLower().Trim() != irbnumber.ToLower().Trim())
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
                                }*/

                                newpi = getPI((string)dr["StudyId"]);
                                newrc = getRC((string)dr["StudyId"]);


                                //newsc = getSC((string)dr["StudyId"]);
                                //newcro = getCRO((string)dr["StudyId"]);

                                RCSCPI rcscpi = new RCSCPI();
                                rcscpi = SpecialStudys.getRCSCPI((string)dr["StudyId"]);


                                dr["Cancer"] = isStudyCancer(dr) ? "Y" : "";

                                hasChanged = checkChangeOverwriteFalse("Cancer", dr, stu.MORE_CANCER, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("HasConsentForm", dr, stu.MORE_INFORMEDCONSENT, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("Agent", dr, stu.MORE_SC_AGENT, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("Biological", dr, stu.MORE_SC_BIOLOGIC, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("BLood_Draw", dr, stu.MORE_SC_BLOOD_DRAW, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("Data_Collection", dr, stu.MORE_SC_DATA_ROUTINE_CARE, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("Device", dr, stu.MORE_SC_DEVICE, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("EMERGENCY_INVESTIGATIONAL", dr, stu.MORE_SC_EMERGENCY_USE, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("HUMANITARIAN_USE", dr, stu.MORE_SC_HUD, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("QI_STUDY", dr, stu.MORE_SC_QI_PROJECT, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("RETROSPECTIVE_CHART_REVIEW", dr, stu.MORE_SC_RETROCHARTREVIEW, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("TISSUE_BANKING", dr, stu.MORE_SC_TISSUEBANKING, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("TRIALS_Involving_INTERVENTIONS", dr, stu.MORE_SC_TRIALS, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("Survey", dr, stu.MORE_SC_SURVEY_STUDY, hasChanged);

                                hasChanged = checkChangeOverwriteNo("SpecimenDataAnalysis", dr, stu.MORE_ANALYSIS_WO_CONSENT, hasChanged);

                                hasChanged = checkChangeOverwriteFalse("PIMajorAuthor", dr, stu.STUDY_MAJ_AUTH, hasChanged);

                                hasChanged = checkChangeOverwriteString("RecordCategory", dr, stu.MORE_RECCATG, hasChanged);


                                hasChanged = checkChangeOverwriteFalse("CT_FDA", dr, stu.MORE_CT_FDA, hasChanged);
                                hasChanged = checkChangeOverwriteFalse("CT_ICMJE", dr, stu.MORE_CT_ICMJE, hasChanged);
                                hasChanged = checkChangeOverwriteFalse("CT_NIH", dr, stu.MORE_CT_NIH, hasChanged);

                                hasChanged = checkChangeOverwriteNullString("AgentDevice", dr, stu.STUDY_AGENT_DEVICE, hasChanged);

                                //hasChanged = checkChangeOverwriteString("IND_NUMBERS", dr, stu.
                                dr["IND_NUMBERS"] = Tools.fixIND((string)dr["IND_NUMBERS"]);

                                dr["NCT_NUMBER"] = Tools.fixNCT((string)dr["NCT_NUMBER"]);
                                hasChanged = checkChangeOverwriteString("NCT_NUMBER", dr, stu.NCT_NUMBER, hasChanged);

                                //HUMANITARIAN_USE

                                if (!String.IsNullOrWhiteSpace((string)dr["KeyWords"]) && !String.IsNullOrWhiteSpace(stu.STUDY_KEYWRDS))
                                {
                                    string keywords = stu.STUDY_KEYWRDS.Length > 480 ? stu.STUDY_KEYWRDS.Substring(0, 480) : stu.STUDY_KEYWRDS;
                                    
                                    dr["KeyWords"] = stu.STUDY_KEYWRDS.Contains((string)dr["KeyWords"]) ? "" : "update eres.er_study set study_keywrds = '" + keywords + "' || chr(13) || chr(10) || '" + (string)dr["KeyWords"] + "' || chr(13) || chr(10) where pk_study = " + stu.PK_STUDY;
                                    dr["KeyWords"] = ((string)dr["KeyWords"]).Replace("\r\n","' || chr(13) || chr(10) || '");
                                    dr["KeyWords"] = ((string)dr["KeyWords"]).Replace("\n", "' || chr(13) || chr(10) || '");
                                }
                                else if (!String.IsNullOrWhiteSpace((string)dr["KeyWords"]))
                                {
                                    dr["KeyWords"] = "update eres.er_study set study_keywrds = '" + (string)dr["KeyWords"] + "' || chr(13) || chr(10) where pk_study = " + stu.PK_STUDY;
                                }

                                dr["pk_study"] = stu.PK_STUDY;



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

                                hasChanged = checkChangeOverwriteString("Studytitle", dr, stu.STUDY_TITLE, hasChanged);

                                dr["Studysummary"] = Tools.removeHtml((string)dr["Studysummary"]);

                                hasChanged = checkChangeOverwriteNullString("Studysummary", dr, stu.STUDY_SUMMARY, hasChanged);

                                hasChanged = checkChangeOverwriteString("PrimarySponsorStudyId", dr, stu.STUDY_SPONSORID, hasChanged);

                                /*if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                                {
                                    string newdep = string.IsNullOrWhiteSpace((string)dr["Department"]) ? "" : IRISMap.Department.getDepartment((string)dr["Department"]);
                                    hasChanged = checkChangeOverwriteNullString("Department", dr, newdep, hasChanged);
                                    string newDiv = string.IsNullOrWhiteSpace((string)dr["Department"]) ? "" : IRISMap.Department.getDivision((string)dr["Department"]);
                                    hasChanged = checkChangeOverwriteNullString("Department", dr, newDiv, hasChanged);
                                }*/

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
                                    if (!Tools.compareStr(newphase, stu.STUDY_PHASE) && newphase != "Please specify")
                                    {
                                        hasChanged = true;
                                    }
                                    else
                                    {
                                        dr["Phase"] = "";
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
        /// Specific for Y/N  overwrite null or false only
        /// </summary>
        /// <param name="field"></param>
        /// <param name="dr"></param>
        /// <param name="dbValue"></param>
        /// <param name="hasChanged"></param>
        /// <returns></returns>
        private static bool checkChangeOverwriteFalse(string field, DataRow dr, string dbValue, bool hasChanged)
        {
            //bool hasChanged = false;
            if (String.IsNullOrWhiteSpace(dbValue) && (string)dr[field] == "N")
            {
                hasChanged = true;
            }
            else if ((dbValue == "N" || dbValue == null) && (string)dr[field] == "Y")
            {
                hasChanged = true;
            }
            /*else if (dbValue == "Y" && (string)dr[field] == "N")
            {
                hasChanged = true;
                dr[field] = "N";
            }*/
            else
            {
                dr[field] = "";
            }
            return hasChanged;

        }


        /// <summary>
        /// Specific for Yes/No  overwrite null or No only
        /// </summary>
        /// <param name="field"></param>
        /// <param name="dr"></param>
        /// <param name="dbValue"></param>
        /// <param name="hasChanged"></param>
        /// <returns></returns>
        private static bool checkChangeOverwriteNo(string field, DataRow dr, string dbValue, bool hasChanged)
        {
            //bool hasChanged = false;
            if (String.IsNullOrWhiteSpace(dbValue) && (((string)dr[field]).Trim().ToLower() == "n" || ((string)dr[field]).Trim().ToLower() == "no"))
            {
                hasChanged = true;
                dr[field] = "No";
            }
            else if (( dbValue == null || dbValue.ToLower().Trim() == "no") && (((string)dr[field]).Trim().ToLower() == "y" || ((string)dr[field]).Trim().ToLower() == "Yes"))
            {
                hasChanged = true;
                dr[field] = "Yes";
            }
            /*else if (dbValue == "Y" && (string)dr[field] == "N")
            {
                hasChanged = true;
                dr[field] = "N";
            }*/
            else
            {
                dr[field] = "";
            }
            return hasChanged;

        }

        /// <summary>
        /// Overwrite only if null
        /// </summary>
        /// <param name="field"></param>
        /// <param name="dr"></param>
        /// <param name="dbValue"></param>
        /// <param name="hasChanged"></param>
        /// <returns></returns>
        private static bool checkChangeOverwriteNullString(string field, DataRow dr, string dbValue, bool hasChanged)
        {
            //bool hasChanged = false;
            if (!String.IsNullOrWhiteSpace((string)dr[field]) &
                (String.IsNullOrWhiteSpace(dbValue))
                )
            {
                hasChanged = true;
            }
            else
            {
                dr[field] = "";
            }
            return hasChanged;

        }

        /// <summary>
        /// Overwrite if different
        /// </summary>
        /// <param name="field"></param>
        /// <param name="dr"></param>
        /// <param name="dbValue"></param>
        /// <param name="hasChanged"></param>
        /// <returns></returns>
        private static bool checkChangeOverwriteString(string field, DataRow dr, string dbValue, bool hasChanged)
        {
            //bool hasChanged = false;
            if (!String.IsNullOrWhiteSpace((string)dr[field]) &
                (String.IsNullOrWhiteSpace(dbValue)
                || (!Tools.compareStr(dbValue, (string)dr[field]))
                )
                )
            {
                hasChanged = true;
            }
            else
            {
                dr[field] = "";
            }
            return hasChanged;

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
            { 
                dr = newStudy.NewRow(); 
            }
            else
            { 
                dr = updatedStudy.NewRow(); 
            }

            //make sur no dbnull row is in the datarow
            foreach (DataColumn c in row.Table.Columns)
            {
                row[c.ColumnName] = String.IsNullOrWhiteSpace((string)row[c.ColumnName]) ? "" : row[c.ColumnName];
            }

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


            dr["Consent"] = (string)row["HasConsentForm"];

            dr["PIMajorAuthor"] = (string)row["PIMajorAuthor"];

            dr["Agent"] = (string)row["Agent"];
            dr["Biological"] = (string)row["Biological"];
            dr["BLood_Draw"] = (string)row["BLood_Draw"];
            dr["Data_Collection"] = (string)row["Data_Collection"];
            dr["Device"] = (string)row["Device"];
            dr["EMERGENCY_INVESTIGATIONAL"] = (string)row["EMERGENCY_INVESTIGATIONAL"];
            dr["HUMANITARIAN_USE"] = (string)row["HUMANITARIAN_USE"];
            dr["QI_STUDY"] = (string)row["QI_STUDY"];
            dr["RETROSPECTIVE_CHART_REVIEW"] = (string)row["RETROSPECTIVE_CHART_REVIEW"];
            dr["Survey"] = (string)row["Survey"];
            dr["TISSUE_BANKING"] = (string)row["TISSUE_BANKING"];
            dr["TRIALS_Involving_INTERVENTIONS"] = (string)row["TRIALS_Involving_INTERVENTIONS"];
            dr["SpecimenDataAnalysis"] = (string)row["SpecimenDataAnalysis"];
            dr["NCT_NUMBER"] = (string)row["NCT_NUMBER"];
            dr["KeyWords"] = (string)row["KeyWords"];
            dr["pk_study"] = (string)row["pk_study"];
            dr["RecordCategory"] = (string)row["RecordCategory"];

            dr["CT_FDA"] = (string)row["CT_FDA"];
            dr["CT_ICMJE"] = (string)row["CT_ICMJE"];
            dr["CT_NIH"] = (string)row["CT_NIH"];

            //dr["IND_Holder"] = (string)row["IND_Holder"];
            dr["IND_Holder"] = "";
            dr["IND_NUMBERS"] = (string)row["IND_NUMBERS"];
            dr["AgentDevice"] = (string)row["AgentDevice"];
            dr["Primary funding sponsor"] = (string)row["Primarysponsorname"];
            dr["FinancialBy"] = (string)row["FinancialBy"];
            dr["SignOffBy"] = (string)row["SignOffBy"];

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
            dr["Study summary"] = ((string)row["Studysummary"]);

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Department"] = String.IsNullOrEmpty((string)row["Department"]) && newentry ? "Please specify" : (string)row["Department"];
                dr["Division/Therapeutic area"] = String.IsNullOrEmpty((string)row["Division"]) && newentry ? "N/A" : (string)row["Division"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY && newentry)
            {
                dr["Department"] = String.IsNullOrEmpty((string)row["Department"]) && newentry ? "Please specify" : (string)row["Department"];
                dr["Division/Therapeutic area"] = String.IsNullOrEmpty((string)row["Division"]) && newentry ? "N/A" : (string)row["Division"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN && newentry)
            {
                dr["Department"] = String.IsNullOrWhiteSpace((string)row["Department"]) ? "" : IRISMap.Department.getDepartment((string)row["Department"]);
                dr["Division/Therapeutic area"] = String.IsNullOrWhiteSpace((string)row["Department"]) ? "" : IRISMap.Department.getDivision((string)row["Department"]);
            }
            else
            {
                dr["Department"] = "";
                dr["Division/Therapeutic area"] = "";
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

            if (newentry)
            {
                int size = 0;
                int.TryParse((string)row["Studysamplesize"], out size);
                dr["Entire study sample size"] = size == 0 ? "" : size.ToString();
            }
            else { dr["Entire study sample size"] = ""; }


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
                    dr["Primary funding sponsor"] = "";
                }
                else if (!String.IsNullOrWhiteSpace(sponsor))
                {
                    dr["Primary funding sponsor"] = sponsor;
                    dr["Sponsor information other"] = "";
                }
                else
                {
                    dr["Primary funding sponsor"] = "";
                    dr["Sponsor information other"] = "";
                }

            }
            else if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                dr["Primary funding sponsor"] = (string)row["Primarysponsorname"];
                dr["Sponsor information other"] = "";
            }
            else if (!String.IsNullOrWhiteSpace((string)row["Primarysponsorname"]))
            {
                dr["Sponsor information other"] = "Per IRB System: " + (string)row["Primarysponsorname"];
                dr["Primary funding sponsor"] = "";
            }
            else
            {
                dr["Primary funding sponsor"] = "";
                dr["Sponsor information other"] = "";
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

            //make sure no dbnull row is in the datarow
            /*foreach (DataColumn c in dr.Table.Columns)
            {
                dr[c.ColumnName] = DBNull.Value.Equals(dr[c.ColumnName]) || dr[c.ColumnName] == null || String.IsNullOrWhiteSpace((string)dr[c.ColumnName]) ? "" : dr[c.ColumnName];
            }*/


            if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                labels = new string[20] { 
                    "Study Financials Managed By**", 
                    //"CRO, if any*", 
                    "IRB agency name", 
                    "IRB No.", 
                    "Is this a cancer related study ?", 
                    "PI Program Code (Cancer Center Studies Only)*", 
                    "Primary Purpose*", 
                    "Is there an Informed Consent associated to study?",
                    "&nbsp;&nbsp;&nbsp;Agent",
                    "&nbsp;&nbsp&nbsp;Biologic Specimen Research (Only tissue or blood collected)",
                    "&nbsp;&nbsp&nbsp;Blood Draw",
                    "&nbsp;&nbsp;&nbsp;Data Collection during routine clinical care",
                    "&nbsp;&nbsp;&nbsp;Device",
                    "&nbsp;&nbsp&nbsp;Emergency use of an investigational drug or device",
                    "&nbsp;&nbsp&nbsp;Humanitarian Use Device (HUD)",
                    "&nbsp;&nbsp&nbsp;Quality Improvement (QI) project",
                    "&nbsp;&nbsp;&nbsp;Retrospective Chart Review",
                    "&nbsp;&nbsp&nbsp;Survey Study (e.g. questionnaire, etc.)",
                    "&nbsp;&nbsp;&nbsp;Tissue Banking",
                    "&nbsp;&nbsp;&nbsp;Trials Involving Interventions",
                    "Will the ONLY research activity be analysis of specimens or data/medical records obtained without consent? (Note: This means there will be NO interventions with human subjects.)"

                };

                values = new string[20] { 
                    (string)dr["STUDY_MANAGED_BY_IMPORT"],
                    (string)dr["IRB Agency name"], 
                    (string)dr["IRB no"], 
                    (string)dr["Cancer"],
                    (string)dr["Program_Code_Mapped"],
                    (string)dr["PRIMARY_PURPOSE"],

                    (string)dr["Consent"],
                    (string)dr["Agent"],
                    (string)dr["Biological"],
                    (string)dr["BLood_Draw"],
                    (string)dr["Data_Collection"],
                    (string)dr["Device"],
                    (string)dr["EMERGENCY_INVESTIGATIONAL"],
                    (string)dr["HUMANITARIAN_USE"],
                    (string)dr["QI_STUDY"],
                    (string)dr["RETROSPECTIVE_CHART_REVIEW"],
                    (string)dr["Survey"],
                    (string)dr["TISSUE_BANKING"],
                    (string)dr["TRIALS_Involving_INTERVENTIONS"],
                    (string)dr["SpecimenDataAnalysis"] 
                };
            }
            else
            {
                labels = new string[23] { 
                    "Record Category*",
                    "Study Financials Managed By*",
                    "Agreement/Final Administrative Sign-Off Managed By*",
                    "IRB Agency Name",
                    "IRB No.",
                    "Is this a cancer related study?",
                    "Is there an Informed Consent associated to study?",
                    "&nbsp;&nbsp;&nbsp;Agent",
                    "&nbsp;&nbsp&nbsp;Biologic Specimen Research (Only tissue or blood collected)",
                    "&nbsp;&nbsp&nbsp;Blood Draw",
                    "&nbsp;&nbsp;&nbsp;Data Collection during routine clinical care",
                    "&nbsp;&nbsp;&nbsp;Device",
                    "&nbsp;&nbsp&nbsp;Emergency use of an investigational drug or device",
                    "&nbsp;&nbsp&nbsp;Humanitarian Use Device (HUD)",
                    "&nbsp;&nbsp&nbsp;Quality Improvement (QI) project",
                    "&nbsp;&nbsp;&nbsp;Retrospective Chart Review",
                    "&nbsp;&nbsp&nbsp;Survey Study (e.g. questionnaire, etc.)",
                    "&nbsp;&nbsp;&nbsp;Tissue Banking",
                    "&nbsp;&nbsp;&nbsp;Trials Involving Interventions",
                    "&nbsp;&nbsp;&nbsp;Is this an FDA Clinical Trial?",
                    "&nbsp;&nbsp;&nbsp;Is this an ICMJE Clinical Trial?",
                    "&nbsp;&nbsp;&nbsp;Is this an NIH Clinical Trial?",
                    "Will the ONLY research activity be analysis of specimens or data/medical records obtained without consent? (Note: This means there will be NO interventions with human subjects.)"
                };

                values = new string[23] {   
                    (string)dr["RecordCategory"],
                    (string)dr["FinancialBy"],
                    (string)dr["SignOffBy"],
                    (string)dr["IRB Agency name"],
                    (string)dr["IRB no"],
                    (string)dr["Cancer"],
                    (string)dr["Consent"],
                    (string)dr["Agent"],
                    (string)dr["Biological"],
                    (string)dr["BLood_Draw"],
                    (string)dr["Data_Collection"],
                    (string)dr["Device"],
                    (string)dr["EMERGENCY_INVESTIGATIONAL"],
                    (string)dr["HUMANITARIAN_USE"],
                    (string)dr["QI_STUDY"],
                    (string)dr["RETROSPECTIVE_CHART_REVIEW"],
                    (string)dr["Survey"],
                    (string)dr["TISSUE_BANKING"],
                    (string)dr["TRIALS_Involving_INTERVENTIONS"],
                    (string)dr["CT_FDA"],
                    (string)dr["CT_ICMJE"],
                    (string)dr["CT_NIH"],
                    (string)dr["SpecimenDataAnalysis"]                
                };
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


                piemail = String.IsNullOrWhiteSpace(piemail) ? "" : piemail.ToLower().Trim();
                piname = String.IsNullOrWhiteSpace(piname) ? "" : piname;

                string[] split = piname.Split(' ');
                var split2 = from s in split
                             orderby s.Length descending
                             select s;

                string nameLonguest = split2.First().Trim().ToLower();
                string nameSecondLonguest = nameLonguest;


                if (split2.Count() > 1)
                {
                    nameSecondLonguest = split2.ElementAt(1).Trim().ToLower();
                }

                var pis = from pi in OutputTeam.fpTeam.data.AsEnumerable()
                          where (pi.Field<string>("Role") == IRISMap.RoleMap.RC1 || pi.Field<string>("Role") == IRISMap.RoleMap.RC2)
                         && pi.Field<string>("StudyId") == studyId
                         && (
                         (piemail != "" && (string)pi["PrimaryEMailAddress"] == piemail)
                           || (((string)pi["FirstName"] + " " + (string)pi["LastName"]).Trim().ToLower().Contains(nameLonguest) && ((string)pi["FirstName"] + " " + (string)pi["LastName"]).Trim().ToLower().Contains(nameSecondLonguest))
                         )
                          select pi;

                if (pis.Count() > 0)
                {
                    return pis.First().Field<string>("FirstName") + " " + pis.First().Field<string>("LastName");
                }
                else
                {
                    return getRole(studyId, IRISMap.RoleMap.PI);
                }

                /*if (getRoleNotChange(studyId, IRISMap.RoleMap.PI, piname, piemail))
                {
                    return piname;
                }
                else
                {
                    return getRole(studyId, IRISMap.RoleMap.PI);
                }*/
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

            if (study == null)
            {
                if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    return getRole(studyId, BranyRoleMap.RC);
                }
                else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                {
                    string rc1 = getRole(studyId, IRISMap.RoleMap.RC1);
                    return String.IsNullOrWhiteSpace(rc1) ? getRole(studyId, IRISMap.RoleMap.RC2) : rc1;
                }
            }

            string rcname = study == null ? "" : study.STUDY_ENTERED_BY;
            string rcemail = String.IsNullOrWhiteSpace(rcname) ? "" : OutputTeam.accounts.FirstOrDefault(x => x.USER_NAME == rcname).USER_EMAIL;

            rcemail = String.IsNullOrWhiteSpace(rcemail) ? "" : rcemail.ToLower().Trim();
            rcname = String.IsNullOrWhiteSpace(rcname) ? "" : rcname;

            string[] split = rcname.Split(' ');
            var split2 = from s in split
                         orderby s.Length descending
                         select s;

            string nameLonguest = split2.First().Trim().ToLower();
            string nameSecondLonguest = nameLonguest;


            if (split2.Count() > 1)
            {
                nameSecondLonguest = split2.ElementAt(1).Trim().ToLower();
            }


            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                /*string test = "";
                if (rcname.Contains("Barrera"))
                {
                    test = rcname + " s";
                }*/

                var rcs = from rc in OutputTeam.fpTeam.data.AsEnumerable()
                          where rc.Field<string>("Role") == BranyRoleMap.RC
                          && rc.Field<string>("StudyId") == studyId
                          && (
                          (rcemail != "" && (string)rc["PrimaryEMailAddress"] == rcemail)
                            || (((string)rc["FirstName"] + " " + (string)rc["LastName"]).Trim().ToLower().Contains(nameLonguest) && ((string)rc["FirstName"] + " " + (string)rc["LastName"]).Trim().ToLower().Contains(nameSecondLonguest))
                          )
                          select rc;

                if (rcs.Count() > 0)
                {
                    return rcname;//test + rcs.First().Field<string>("FirstName") + " " + rcs.First().Field<string>("LastName");
                }
                else
                {
                    return getRole(studyId, BranyRoleMap.RC);
                }

                /*if (getRoleNotChange(studyId, BranyRoleMap.RC, rcname, rcemail))
                {
                    retstr = rcname;
                }
                else
                {
                    retstr = getRole(studyId, BranyRoleMap.RC);
                }*/
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {

                var rcs = from rc in OutputTeam.fpTeam.data.AsEnumerable()
                          where (rc.Field<string>("Role") == IRISMap.RoleMap.RC1 || rc.Field<string>("Role") == IRISMap.RoleMap.RC2)
                          && rc.Field<string>("StudyId") == studyId
                          && (
                          (rcemail != "" && (string)rc["PrimaryEMailAddress"] == rcemail)
                            || (((string)rc["FirstName"] + " " + (string)rc["LastName"]).Trim().ToLower().Contains(nameLonguest) && ((string)rc["FirstName"] + " " + (string)rc["LastName"]).Trim().ToLower().Contains(nameSecondLonguest))
                          )
                          select rc;

                if (rcs.Count() > 0 && !String.IsNullOrWhiteSpace(rcname))
                {
                    return rcname;//rcs.First().Field<string>("FirstName") + " " + rcs.First().Field<string>("LastName");
                }
                else
                {
                    string rc1 = getRole(studyId, IRISMap.RoleMap.RC1);
                    return String.IsNullOrWhiteSpace(rc1) ? getRole(studyId, IRISMap.RoleMap.RC2) : rc1;
                }

                //string rcemail = OutputTeam.team.FirstOrDefault(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower()) && x.ROLE == Role).USER_EMAIL;

                /*if (getRoleNotChange(studyId, IRISMap.RoleMap.RC1, rcname, rcemail) || getRoleNotChange(studyId, IRISMap.RoleMap.RC2, rcname, rcemail))
                {
                    retstr = rcname;
                }
                else
                {
                    retstr = getRole(studyId, IRISMap.RoleMap.RC1);
                    if (String.IsNullOrWhiteSpace(retstr))
                        retstr = getRole(studyId, IRISMap.RoleMap.RC2);
                }*/
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
            return "";
            /*if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
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

            return retstr;*/
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
            var studyteam = OutputTeam.fpTeam.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId).OrderBy(x => x.Field<string>("LastName")).ThenBy(y => y.Field<string>("FirstName"));
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

                email = String.IsNullOrWhiteSpace(email) ? "" : email.ToLower().Trim();
                name = String.IsNullOrWhiteSpace(name) ? "" : name;

                if (email == "" & name == "") return true;

                string[] split = name.Split(' ');
                var split2 = from s in split
                             orderby s.Length descending
                             select s;

                string nameLonguest = split2.First().Trim().ToLower();
                string nameSecondLonguest = nameLonguest;
                if (split2.Count() > 1)
                {
                    nameSecondLonguest = split2.ElementAt(1).Trim().ToLower();
                }

                return OutputTeam.fpTeam.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId
                    && (string)x["Role"] == role
                    && (email != "" && (string)x["PrimaryEMailAddress"] == email)
                    && (((string)x["FirstName"] + " " + (string)x["LastName"]).Trim().ToLower().Contains(nameLonguest) && ((string)x["FirstName"] + " " + (string)x["LastName"]).Trim().ToLower().Contains(nameSecondLonguest))
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


            if (SpecialStudys.checkConsentAgentAndDeviceDate && dr.Table.Columns.Contains("HasConsentForm") && dr.Table.Columns.Contains("PhaseDrugDevice") && dr.Table.Columns.Contains("CreationDate") && Agency.AgencySetupVal == Agency.AgencyList.EINSTEIN)
            {
                DateTime dateparsed = DateTime.MinValue;
                DateTime.TryParse((string)dr["CreationDate"], out dateparsed);

                if ((string)dr["HasConsentForm"] == "N" && (string)dr["PhaseDrugDevice"] == "N" && dateparsed < SpecialStudys.checkConsentAgentAndDeviceDateDate)
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
