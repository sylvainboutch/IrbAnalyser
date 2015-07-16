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

                        var query = (from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                     where st.MORE_IRBAGENCY == Agency.agencyStrLwr
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
                newStudy.Columns.Add("IRB Agency name", typeof(string));
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
                updatedStudy.Columns.Add("Study_number", typeof(string));
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
        }


        /// <summary>
        /// Analyse a row of the study report
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow dr)
        {
            string irbstudyId = (string)dr["StudyId"];

            string identifiers = Tools.generateStudyIdentifiers((string)dr["StudyId"]);
            string number = Tools.getStudyNumber((string)dr["StudyId"], ((string)dr["IRBNumber"]).Replace("(IBC)", ""));

            OutputIRBForm.addIds(number, identifiers);

            if (!String.IsNullOrEmpty(irbstudyId) && !((string)dr["StudyId"]).ToLower().Contains("corrupted"))
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

                    OutputStatus.analyseRowStudy(dr, true);
                    if (!dtStudy)
                    {
                        addRowStudy(dr, true);
                        //Add all related values for that study                            
                        OutputDocs.analyseRow(dr, true);
                    }
                }
                else
                {
                    bool dtStudy = (from st in OutputStudy.updatedStudy.AsEnumerable()
                                    where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                    select st).Any();

                    OutputSite.analyseRow(dr, false);

                    OutputStatus.analyseRowStudy(dr, false);
                    if (!dtStudy)
                    {


                        OutputDocs.analyseRow(dr, false);

                        bool hasChanged = false;
                        string newpi = "";
                        string newrc = "";
                        string newsc = "";
                        string newcro = "";

                        foreach (var stu in study)
                        {
                            newpi = getPI((string)dr["StudyId"]);
                            newrc = getRC((string)dr["StudyId"]);
                            newcro = getCRO((string)dr["StudyId"]);

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

                            if (stu.STUDY_COORDINATOR != newsc && !String.IsNullOrEmpty(newsc))
                            {
                                hasChanged = true;
                            }
                            else { newsc = ""; }

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

                            if (Tools.compareStr(stu.STUDY_SUMMARY, Tools.removeHtml((string)dr["Studysummary"])) && !String.IsNullOrWhiteSpace(Tools.removeHtml((string)dr["Studysummary"])))
                            {
                                dr["Studysummary"] = "";
                            }
                            else if (!String.IsNullOrWhiteSpace(Tools.removeHtml((string)dr["Studysummary"])))
                            {
                                hasChanged = true;
                            }


                            if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
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
                                if (Tools.compareStr(newDiv, stu.STUDY_TAREA) && !string.IsNullOrWhiteSpace((string)dr["Department"]) && newdep!=null)
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

                            string cancer;
                            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                            {
                                cancer = "N";
                                if (irbno.Count() >= 2 && irbno[1] == "06")
                                    cancer = "Y";
                            }
                            else
                            {
                                cancer = (string)dr["Cancer"];
                            }

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
                            addRowStudy(dr, false, newpi, newrc, newsc, newcro);
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

            dr["IRB Agency name"] = Agency.agencyStrLwr.ToUpper();
            dr["IRB no"] = ((string)row["IRBNumber"]).Replace("(IBC)", "");
            dr["IRB Study ID"] = (string)row["StudyId"];
            dr["IRB Identifiers"] = Tools.generateStudyIdentifiers((string)row["StudyId"]);

            dr["Study_number"] = Tools.getStudyNumber((string)row["StudyId"], (string)dr["IRB no"], (string)row["StudyAcronym"], (string)row["StudyTitle"]); 
                //Tools.getStudyNumber((string)row["StudyId"], (string)dr["IRB no"], (string)row["StudyAcronym"]);

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

            if (newcro == null)
            {
                dr["CRO"] = getCRO((string)row["StudyId"]);
            }
            else
            {
                dr["CRO"] = newcro;
            }

            dr["Official title"] = (string)row["StudyTitle"];
            dr["Study summary"] = Tools.removeHtml((string)row["Studysummary"]);


            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Department"] = String.IsNullOrEmpty((string)row["Department"]) && newentry ? "Please specify" : (string)row["Department"];
                dr["Division/Therapeutic area"] = String.IsNullOrEmpty((string)row["Division"]) && newentry ? "N/A" : (string)row["Division"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                dr["Department"] = String.IsNullOrWhiteSpace((string)row["Department"]) ? "" : IRISMap.Department.getDepartment((string)row["Department"]);
                dr["Division/Therapeutic area"] = String.IsNullOrWhiteSpace((string)row["Department"]) ? "" : IRISMap.Department.getDivision((string)row["Department"]);
            }

            int size = 0;
            int.TryParse((string)row["Studysamplesize"], out size);
            dr["Entire study sample size"] = size == 0 ? "" : size.ToString();

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                dr["Phase"] = String.IsNullOrEmpty((string)row["Phase"]) && newentry ? "Please Specify" : (string)row["Phase"];
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                dr["Phase"] = String.IsNullOrWhiteSpace((string)row["Phase"]) ? "" : IRISMap.Phase.getPhase((string)row["Phase"]);
            }


            if (Tools.compareStr(row["Multicenter"].ToString(), "true") || Tools.compareStr(row["Multicenter"].ToString(), "yes") || Tools.compareStr(row["Multicenter"].ToString(), "y"))
                dr["Study scope"] = "Multi Center Study";
            else if (Tools.compareStr(row["Multicenter"].ToString(), "false") || Tools.compareStr(row["Multicenter"].ToString(), "no") || Tools.compareStr(row["Multicenter"].ToString(), "n"))
                dr["Study scope"] = "Single Center Study";
            else
                dr["Study scope"] = "";

            dr["Primary funding sponsor, if other :"] = row["Primarysponsorname"].ToString();
            dr["Sponsor contact"] = row["PrimarySponsorContactFirstName"].ToString() + " " + row["PrimarySponsorContactLastName"].ToString();
            dr["Sponsor Protocol ID"] = row["PrimarySponsorStudyId"].ToString();

            string[] labels = new string[] { };
            string[] values = new string[] { };
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                labels = new string[5] { "Study Managed by*", "CRO, if any*", "IRB agency name", "IRB No.", "Is this a cancer related study ?" };

                dr["Cancer"] = row["Cancer"];

                values = new string[5] { "BRY", (string)dr["CRO"], Agency.agencyStrLwr, (string)dr["IRB no"], (string)dr["Cancer"] };
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                labels = new string[3] { "IRB agency name", "IRB No.", "Is this a cancer related study ?" };

                dr["Cancer"] = row["Cancer"];

                values = new string[3] { "einstein", (string)dr["IRB no"], (string)dr["Cancer"] };
            }

            OutputMSD.initiate();

            for (int i = 0; i < labels.Count(); i++)
            {
                OutputMSD.addRow(labels[i], values[i], (string)row["StudyId"], (string)dr["IRB no"], (string)row["StudyAcronym"], newentry);
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
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                return getRole(studyId, BranyRoleMap.PI);
            }
            else
            {
                return getRole(studyId, IRISMap.RoleMap.PI);
            }
        }

        /// <summary>
        /// returns the PI email
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static string getPIeMail(string studyId)
        {
            var studyteam = OutputTeam.fpTeam.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId);
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                return (string)studyteam.FirstOrDefault(x => (string)x["Role"] == BranyRoleMap.PI && (string)x["Primary"] == "Y")["PrimaryEMailAddress"];
            }
            else
            {
                return (string)studyteam.FirstOrDefault(x => (string)x["Role"] == IRISMap.RoleMap.PI)["PrimaryEMailAddress"];
            }

        }


        /// <summary>
        /// Returns the RC for that study
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static string getRC(string studyId)
        {

            string retstr = "";
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                retstr = getRole(studyId, BranyRoleMap.RC);
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

            string retstr = "";
            if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                retstr = getRole(studyId, IRISMap.RoleMap.SC);
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
        private static string getRole(string studyId, string role)
        {
            var studyteam = OutputTeam.fpTeam.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId);
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                return Tools.getFullName(studyteam.FirstOrDefault(x => (string)x["Role"] == role && (string)x["Primary"] == "Y"));
            }
            else
            {
                return Tools.getFullName(studyteam.FirstOrDefault(x => (string)x["Role"] == role));
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
    }
}
