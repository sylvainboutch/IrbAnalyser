using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    /// <summary>
    /// Contains the list of newly created study
    /// </summary>
    public static class OutputStudy
    {

        //List of newly created study, with more study detail
        public static DataTable newStudy = new DataTable();
        public static DataTable updatedStudy = new DataTable();

        private static IEnumerable<Model.LCL_V_STUDYSUMM_PLUSMORE> _studys;
        public static IEnumerable<Model.LCL_V_STUDYSUMM_PLUSMORE> studys
        {
            get
            {
                if (_studys == null || _studys.Count() == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        var query = (from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                     where st.MORE_IRBAGENCY != null
                                     && st.IRBIDENTIFIERS != null
                                 select st);
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
                newStudy.Columns.Add("IRB Agency name", typeof(string));
                newStudy.Columns.Add("IRB no", typeof(string));
                newStudy.Columns.Add("IRB Study ID", typeof(string));
                newStudy.Columns.Add("IRB Identifiers", typeof(string));
                newStudy.Columns.Add("Study number", typeof(string));
                newStudy.Columns.Add("Study coordinator", typeof(string));
                newStudy.Columns.Add("Principal Investigator", typeof(string));
                newStudy.Columns.Add("Official title", typeof(string));
                newStudy.Columns.Add("Study Summary", typeof(string));
                newStudy.Columns.Add("Department", typeof(string));
                newStudy.Columns.Add("Division/Therapeutic area", typeof(string));
                newStudy.Columns.Add("Entire study sample size", typeof(string));
                newStudy.Columns.Add("Phase", typeof(string));
                newStudy.Columns.Add("Study scope", typeof(string));
                newStudy.Columns.Add("Primary funding sponsor, if other :", typeof(string));
                newStudy.Columns.Add("Sponsor contact", typeof(string));
                newStudy.Columns.Add("Sponsor Protocol ID", typeof(string));
                newStudy.Columns.Add("CRO", typeof(string));
                newStudy.Columns.Add("Cancer", typeof(string));
            }

            if (updatedStudy.Columns.Count == 0)
            {
                updatedStudy.Columns.Add("IRB Agency name", typeof(string));
                updatedStudy.Columns.Add("IRB no", typeof(string));
                updatedStudy.Columns.Add("IRB Study ID", typeof(string));
                updatedStudy.Columns.Add("IRB Identifiers", typeof(string));
                updatedStudy.Columns.Add("Study number", typeof(string));
                updatedStudy.Columns.Add("Study coordinator", typeof(string));
                updatedStudy.Columns.Add("Principal Investigator", typeof(string));
                updatedStudy.Columns.Add("Official title", typeof(string));
                updatedStudy.Columns.Add("Study summary", typeof(string));
                updatedStudy.Columns.Add("Department", typeof(string));
                updatedStudy.Columns.Add("Division/Therapeutic area", typeof(string));
                updatedStudy.Columns.Add("Entire study sample size", typeof(string));
                updatedStudy.Columns.Add("Phase", typeof(string));
                updatedStudy.Columns.Add("Study scope", typeof(string));
                updatedStudy.Columns.Add("Primary funding sponsor, if other :", typeof(string));
                updatedStudy.Columns.Add("Sponsor contact", typeof(string));
                updatedStudy.Columns.Add("Sponsor Protocol ID", typeof(string));
                updatedStudy.Columns.Add("CRO", typeof(string));
                updatedStudy.Columns.Add("Cancer", typeof(string));
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
            //OutputSite.analyseDelete(fpStudy.data);
        }


        /// <summary>
        /// Analyse a row of the study report
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow dr)
        {
            string irbstudyId = (string)dr["StudyId"];

            if (!String.IsNullOrEmpty(irbstudyId))
            {

                //using (Model.VelosDb db = new Model.VelosDb())
                //{

                    var study = from st in studys
                                where st.IRBIDENTIFIERS.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                && st.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                select st;



                    if (!study.Any())
                    {
                        bool dtStudy = (from st in OutputStudy.newStudy.AsEnumerable()
                                        where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                        && st.Field<string>("IRB Agency name").Trim().ToLower() == Agency.agencyStrLwr
                                        select st).Any();

                        OutputStatus.analyseRowStudy(dr, true);
                        if (!dtStudy)
                        {
                            addRowStudy(dr, true);
                            //Add all related values for that study                            
                            OutputSite.analyseRow(dr, true);
                            OutputDocs.analyseRow(dr, true);
                        }
                    }
                    else
                    {
                        bool dtStudy = (from st in OutputStudy.updatedStudy.AsEnumerable()
                                        where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                        && st.Field<string>("IRB Agency name").Trim().ToLower() == Agency.agencyStrLwr
                                        select st).Any();

                        OutputStatus.analyseRowStudy(dr, false);
                        if (!dtStudy)
                        {

                            OutputSite.analyseRow(dr, false);
                            OutputDocs.analyseRow(dr, false);

                            bool hasChanged = false;
                            string newpi = "";
                            string newrc = "";
                            string newcro = "";

                            foreach (var stu in study)
                            {
                                newpi = getPI((string)dr["StudyId"]);
                                newrc = getRC((string)dr["StudyId"]);
                                newcro = getCRO((string)dr["StudyId"]);


                                //BE CAREFULL IN THE VIEW :
                                //STUDY_ENTERED_BY = Regulatory Coordinator
                                //STUDY_PI = PRINCIPAL INVESTIGATOR
                                //STUDY_COORDINATOR = STUDY_COORDINATOR

                                if (stu.STUDY_PI != newpi && !String.IsNullOrEmpty(newpi))
                                {
                                    hasChanged = true;
                                }
                                else { newpi = ""; }

                                if (stu.STUDY_ENTERED_BY != newrc && !String.IsNullOrEmpty(newrc))
                                {
                                    hasChanged = true;
                                }
                                else { newrc = ""; }

                                if (stu.MORE_CRO != newcro && !String.IsNullOrEmpty(newcro))
                                {
                                    hasChanged = true;
                                }
                                else { newcro = ""; }

                                if (Tools.compareStr(stu.STUDY_TITLE, dr["StudyTitle"]) && !String.IsNullOrWhiteSpace((string)dr["StudyTitle"]))
                                {
                                    dr["Studytitle"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace((string)dr["StudyTitle"]))
                                {
                                    hasChanged = true;
                                }

                                if (Tools.compareStr(stu.STUDY_SUMMARY, dr["Studysummary"]) && !String.IsNullOrWhiteSpace((string)dr["Studysummary"]))
                                {
                                    dr["Studysummary"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace((string)dr["Studysummary"]))
                                {
                                    hasChanged = true;
                                }
                                /*
                                var indide = from sd in db.LCL_V_STUDY_INDIDE
                                             where stu.PK_STUDY == sd.FK_STUDY
                                             select sd;

                                if (indide.Count() == 0 && dr["IND"].ToString().ToUpper() == "TRUE")
                                {
                                    hasChanged = true;
                                }
                                if (indide.Count() == 0 && dr["IND"].ToString().ToUpper() == "FALSE")
                                {
                                    dr["IND"] = "";
                                }


                                bool hasntchanged = false;

                                foreach (var ind in indide)
                                {
                                    if (Tools.compareStr(ind.INDIDE_NUMBER, dr["INDnumber"]))
                                    {
                                        dr["INDnumber"] = "";
                                        hasntchanged = true;
                                    }
                                }

                                hasChanged = hasntchanged ? hasChanged : true;

                                //TODO What to do with IND Holder
                                */

                                //TODO This would come from IRIS mapping
                                /*
                                if (Tools.compareStr(stu.STUDY_DIVISION, dr["Department"]))
                                {
                                    dr["Department"] = "";
                                }
                                else
                                {
                                    hasChanged = true;
                                }

                                if (Tools.compareStr(stu.STUDY_TAREA, dr["Division"]))
                                {
                                    dr["Division"] = "";
                                }
                                else
                                {
                                    hasChanged = true;
                                }
                                */


                                if (Agency.AgencyVal == Agency.AgencyList.IRIS)
                                {
                                    string newdep = IRISMap.Department.getDepartment((string)dr["Department"]);
                                    if (Tools.compareStr(newdep, stu.STUDY_DIVISION) && !string.IsNullOrWhiteSpace((string)dr["Department"]))
                                    {
                                        dr["Department"] = "";
                                    }
                                    else if (!string.IsNullOrWhiteSpace((string)dr["Department"]))
                                    {
                                        hasChanged = true;
                                    }

                                    string newDiv = IRISMap.Department.getDivision((string)dr["Department"]);
                                    if (Tools.compareStr(newDiv, stu.STUDY_TAREA) && !string.IsNullOrWhiteSpace((string)dr["Department"]))
                                    {
                                        dr["Department"] = "";
                                    }
                                    else if (!string.IsNullOrWhiteSpace((string)dr["Department"]))
                                    {
                                        hasChanged = true;
                                    }
                                }



                                int samplesize = 0;
                                Int32.TryParse((string)dr["Studysamplesize"], out samplesize);

                                if (stu.STUDY_NATSAMPSIZE == samplesize)
                                {
                                    dr["Studysamplesize"] = "";
                                }
                                else if (samplesize != 0)
                                {
                                    hasChanged = true;
                                }

                                bool cmp = (stu.STUDY_SCOPE == "Multi Center Study" && dr["Multicenter"].ToString().ToLower() == "true") ||
                                    (stu.STUDY_SCOPE == "Single Center Study" && dr["Multicenter"].ToString().ToLower() == "false") ||
                                    (stu.STUDY_SCOPE == null && dr["Multicenter"].ToString().ToLower() == "");

                                if (cmp)
                                {
                                    dr["Multicenter"] = "";
                                }
                                else if (dr["Multicenter"].ToString() != "")
                                {
                                    hasChanged = true;
                                }

                                if (Agency.AgencyVal == Agency.AgencyList.IRIS)
                                {
                                    string newphase = IRISMap.Phase.getPhase((string)dr["Phase"]);
                                    if (Tools.compareStr(newphase,stu.STUDY_PHASE) && !string.IsNullOrWhiteSpace((string)dr["Phase"]))
                                    {
                                        dr["Phase"] = "";
                                    }
                                    else if ( !string.IsNullOrWhiteSpace((string)dr["Phase"]))
                                    {
                                        hasChanged = true;
                                    }
                                }



                                if (Tools.compareStr(stu.STUDY_SPONSOR, dr["Primarysponsorname"]) && !String.IsNullOrWhiteSpace((string)dr["Primarysponsorname"]))
                                {
                                    dr["Primarysponsorname"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace((string)dr["Primarysponsorname"]))
                                {
                                    hasChanged = true;
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
                                    dr["PrimarysponsorstudyID"] = "";
                                }
                                else if (!String.IsNullOrWhiteSpace((string)dr["PrimarysponsorstudyID"]))
                                {
                                    hasChanged = true;
                                }

                                string[] irbno = ((string)dr["IRBNumber"]).Replace("(IBC)", "").Split('-');

                                string cancer = "N";
                                if (irbno.Count() >= 2 && irbno[1] == "06")
                                cancer = "Y";

                                if (stu.MORE_CANCER == "Y" && cancer == "N")
                                {
                                    hasChanged = true;
                                    dr["Cancer"] = "N";
                                }

                                if ((stu.MORE_CANCER == "N" || stu.MORE_CANCER == null) && cancer == "Y")
                                {
                                    hasChanged = true;
                                    dr["Cancer"] = "Y";
                                }



                            }

                            if (hasChanged)
                            {
                                addRowStudy(dr, false, newpi, newrc, newcro);
                            }
                        }
                    //}
                }
            }
        }

        /// <summary>
        /// Add a new row to the study table
        /// </summary>
        /// <param name="irbNumber"></param>
        /// <param name="studyNumber"></param>
        private static void addRowStudy(DataRow row, bool newentry, string newpi = null, string newrc = null, string newcro = null)
        {
            initiate();
            DataRow dr;
            if (newentry)
            { dr = newStudy.NewRow(); }
            else
            { dr = updatedStudy.NewRow(); }

            dr["IRB Agency name"] = Agency.agencyStrLwr.ToUpper();
            dr["IRB no"] = ((string)row["IRBNumber"]).Replace("(IBC)", "");
            dr["IRB Study ID"] = (string)row["StudyId"];
            dr["IRB Identifiers"] = Tools.generateStudyIdentifiers((string)row["StudyId"]);

            dr["Study number"] = Tools.getStudyNumber((string)row["StudyId"], (string)dr["IRB no"], (string)row["StudyAcronym"]);

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
                dr["Study coordinator"] = getRC((string)row["StudyId"]);
            }
            else
            {
                dr["Study coordinator"] = newrc;
            }

            if (newcro == null)
            {
                dr["CRO"] = getCRO((string)row["StudyId"]);
            }
            else
            {
                dr["CRO"] = newcro;
            }

            dr["Official title"] = (string)row["StudyTitle"];
            dr["Study summary"] = row["Studysummary"].ToString();


            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Department"] = String.IsNullOrEmpty((string)row["Department"]) && newentry ? "Please specify" : (string)row["Department"];
                dr["Division/Therapeutic area"] = String.IsNullOrEmpty((string)row["Division"]) && newentry ? "N/A" : (string)row["Division"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.IRIS)
            {
                dr["Department"] = IRISMap.Department.getDepartment((string)row["Department"]);
                dr["Division/Therapeutic area"] = IRISMap.Department.getDivision((string)row["Department"]);
            }


            dr["Entire study sample size"] = row["Studysamplesize"].ToString();

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Phase"] = String.IsNullOrEmpty((string)row["Phase"]) && newentry ? "Please Specify" : (string)row["Phase"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.IRIS)
            {
                dr["Phase"] = IRISMap.Phase.getPhase((string)row["Phase"]);
            }


            if (Tools.compareStr(row["Multicenter"].ToString(), "TRUE"))
                dr["Study scope"] = "Multi Center Study";
            else if (Tools.compareStr(row["Multicenter"].ToString(), "FALSE"))
                dr["Study scope"] = "Single Center Study";
            else
                dr["Study scope"] = "";

            dr["Primary funding sponsor, if other :"] = row["Primarysponsorname"].ToString();
            dr["Sponsor contact"] = row["PrimarySponsorContactFirstName"].ToString() + " " + row["PrimarySponsorContactLastName"].ToString();
            dr["Sponsor Protocol ID"] = row["PrimarySponsorStudyId"].ToString();


            
            string[] labels = new string[5] { "Study Managed by", "CRO", "IRB agency name", "IRB No.", "Is this a cancer related study ?" };

            dr["Cancer"] = row["Cancer"];

            string[] values = new string[5] { "BRY", (string)dr["CRO"], Agency.agencyStrLwr, (string)dr["IRB no"], (string)dr["Cancer"] };

            OutputMSD.initiate();

            for (int i = 0; i < labels.Count(); i++)
            {
                OutputMSD.addRow(labels[i], values[i], (string)row["StudyId"], (string)dr["IRB no"], (string)row["StudyAcronym"], newentry);
            }


            OutputIRBForm.addIds((string)dr["Study number"], (string)dr["IRB Identifiers"]);

            if (newentry)
            { newStudy.Rows.Add(dr); }
            else
            { updatedStudy.Rows.Add(dr); }

        }

        private static string getPI(string studyId)
        {
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                return getRole(studyId, BranyRoleMap.PI);
            }
            else
            {
                return getRole(studyId, IRISMap.RoleMap.PI);
            }
        }

        private static string getRC(string studyId)
        {

            string retstr = "";
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                retstr = getRole(studyId, BranyRoleMap.RC);
            }
            else
            {
                retstr = getRole(studyId, IRISMap.RoleMap.RC);
            }

            retstr = String.IsNullOrWhiteSpace(retstr) ? getPI(studyId) : retstr;

            return retstr;
        }


        private static string getCRO(string studyId)
        {
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                return getRole(studyId, BranyRoleMap.CRO);
            }
            else
            {
                return "";
            }
        }



        private static string getRole(string studyId, string role)
        {
            var studyteam = OutputTeam.fpTeam.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId);

            return Tools.getFullName(studyteam.FirstOrDefault(x => (string)x["Role"] == role && (string)x["Primary"] == "Y"));
        }


    }
}
