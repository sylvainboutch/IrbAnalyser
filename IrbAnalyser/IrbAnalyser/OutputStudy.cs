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
        public static DataTable study = new DataTable();

        /// <summary>
        /// Add the columns to the datatable
        /// </summary>
        private static void initiate()
        {
            if (study.Columns.Count == 0)
            {
                study.Columns.Add("TYPE", typeof(string));
                study.Columns.Add("IRB Agency name", typeof(string));
                study.Columns.Add("IRB no", typeof(string));
                study.Columns.Add("IRB Study ID", typeof(string));
                study.Columns.Add("Study number", typeof(string));
                study.Columns.Add("Regulatory coordinator", typeof(string));
                study.Columns.Add("Principal Investigator", typeof(string));
                study.Columns.Add("Official title", typeof(string));
                study.Columns.Add("Study summary", typeof(string));
                study.Columns.Add("Department", typeof(string));
                study.Columns.Add("Division/Therapeutic area", typeof(string));
                study.Columns.Add("Entire study sample size", typeof(string));
                study.Columns.Add("Phase", typeof(string));
                study.Columns.Add("Research scope", typeof(string));
                study.Columns.Add("Primary funding sponsor, if other :", typeof(string));
                study.Columns.Add("Sponsor contact", typeof(string));
                study.Columns.Add("Sponsor Protocol ID", typeof(string));
            }
        }


        /// <summary>
        /// Analyse the study report
        /// </summary>
        public static void analyse(string filepath)
        {
            initiate();
            FileParser fpStudy = new FileParser(filepath + "studysite.txt");

            foreach (DataRow study in fpStudy.data.Rows)
            {
                analyseRow(study, filepath + "team.txt");
            }

            OutputSite.analyseDelete(fpStudy.data);
        }


        /// <summary>
        /// Analyse a row of the study report
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow dr, string teamfile)
        {
            string irbstudyId = (string)dr["StudyId"];
            string irbagency = ((string)dr["IRBAgency"]).ToLower();
            if (!String.IsNullOrEmpty(irbagency) && !String.IsNullOrEmpty(irbstudyId))
            {
                OutputStatus.analyseRowStudy(dr);
                OutputSite.analyseRow(dr);
                OutputDocs.analyseRow(dr);
                using (Model.VelosDb db = new Model.VelosDb())
                {

                    var study = from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                where st.MORE_IRBSTUDYID == irbstudyId
                                && st.MORE_IRBAGENCY.ToLower() == irbagency
                                select st;
                    if (!study.Any())
                    {
                        addRowStudy(dr, "New study", true, teamfile);
                    }
                    else
                    {
                        bool hasChanged = false;
                        string newpi = "";
                        string newrc = "";
                        foreach (var stu in study)
                        {
                            newpi = getPI(teamfile, (string)dr["IRBAgency"], (string)dr["StudyId"]);
                            newrc = getRC(teamfile, (string)dr["IRBAgency"], (string)dr["StudyId"]);

                            if (stu.STUDY_PI != newpi && !String.IsNullOrEmpty(newpi))
                            {
                                hasChanged = true;
                            }
                            else { newpi = ""; }

                            if (stu.STUDY_COORDINATOR != newrc && !String.IsNullOrEmpty(newrc))
                            {
                                hasChanged = true;
                            }
                            else { newrc = ""; }

                            if (!Tools.compareStr(stu.STUDY_TITLE, dr["Studytitle"]))
                            {
                                dr["Studytitle"] = dr["Studytitle"];
                            }
                            else
                            {
                                hasChanged = true;
                            }

                            if (Tools.compareStr(stu.STUDY_SUMMARY, dr["Studysummary"]))
                            {
                                dr["Studysummary"] = "";
                            }
                            else
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

                            int samplesize = 0;
                            Int32.TryParse((string)dr["Studysamplesize"], out samplesize);

                            if (stu.STUDY_NATSAMPSIZE == samplesize)
                            {
                                dr["Studysamplesize"] = "";
                            }
                            else
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
                            else
                            {
                                hasChanged = true;
                            }

                            //TODO Phase need mapping from IRIS, BRANY doesnt have
                            //TODO This should also use a map
                            if (Tools.compareStr(stu.STUDY_SPONSOR, dr["Primarysponsorname"]))
                            {
                                dr["Primarysponsorname"] = "";
                            }
                            else
                            {
                                hasChanged = true;
                            }

                            string[] strs = {dr["Primarysponsorcontactfirstname"].ToString(),
                    dr["Primarysponsorcontactlastname"].ToString()};

                            if (Tools.containStr(stu.STUDY_SPONSOR, strs))
                            {
                                dr["Primarysponsorname"] = "";
                            }
                            else
                            {
                                hasChanged = true;
                            }

                            if (Tools.compareStr(stu.STUDY_SPONSORID, dr["PrimarysponsorstudyID"]))
                            {
                                dr["PrimarysponsorstudyID"] = "";
                            }
                            else
                            {
                                hasChanged = true;
                            }

                        }

                        if (hasChanged)
                        {
                            OutputStudy.addRowStudy(dr, "Modified study", false, teamfile, newpi, newrc);
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
        private static void addRowStudy(DataRow row, string type, bool newStudy, string teamfile, string newpi = "", string newrc = "")
        {
            initiate();
            DataRow dr = study.NewRow();
            dr["TYPE"] = type;
            dr["IRB Agency name"] = (string)row["IRBAgency"];
            dr["IRB no"] = (string)row["IRBNumber"];
            dr["IRB Study ID"] = (string)row["StudyId"];

            if (newStudy)
            {
                dr["Study number"] = DateTime.Now.Year.ToString().Substring(2,2);
                dr["Study number"] += ((string)row["IRBAgency"]).ToLower() == "brany" ? "BRANY" : "IRIS";//OR MSA ? since apperently OCT enters brany CDA
                dr["Study number"] += " PLEASE COMPLETE ";
                dr["Regulatory coordinator"] = getRC(teamfile, (string)row["IRBAgency"], (string)row["StudyId"]);
                dr["Principal Investigator"] = getPI(teamfile, (string)row["IRBAgency"], (string)row["StudyId"]);
            }
            else
            {
                dr["Study number"] = Tools.getStudyNumber((string)row["StudyId"], (string)row["IRBAgency"]);
                dr["Regulatory coordinator"] = newrc;
                dr["Principal Investigator"] = newpi;
            }
            dr["Official title"] = row["Studytitle"].ToString();
            dr["Study summary"] = row["Studysummary"].ToString();
            dr["Department"] = row["Department"].ToString();
            dr["Division/Therapeutic area"] = row["Division"].ToString();
            dr["Entire study sample size"] = row["Studysamplesize"].ToString();
            dr["Phase"] = row["Phase"].ToString() == "" ? "NA" : row["Phase"].ToString();
            dr["Research scope"] = row["Multicenter"].ToString() == "TRUE" ? "Multicenter" : "Single center";
            dr["Primary funding sponsor, if other :"] = row["Primarysponsorname"].ToString();
            dr["Sponsor contact"] = row["PrimarySponsorContactFirstName"].ToString() + " " + row["PrimarySponsorContactLastName"].ToString();
            dr["Sponsor Protocol ID"] = row["PrimarySponsorStudyId"].ToString();

            study.Rows.Add(dr);
        }

        private static string getPI(string teamfile, string agency, string studyId)
        {
            return getRole(teamfile, agency, studyId, "PI");
        }

        private static string getRC(string teamfile, string agency, string studyId)
        {
            return getRole(teamfile, agency, studyId, "Study Coordinator");
        }

        private static string getRole(string teamfile, string agency, string studyId, string roleStr)
        {
            FileParser fpTeam = new FileParser(teamfile);

            var studyteam = fpTeam.data.AsEnumerable().Where(x => (string)x["IRBAgency"] == agency && (string)x["StudyId"] == studyId);
            string role = "";
            if (agency.ToLower() == "brany")
            {
                role = BranyRoleMap.roleMapBrany.FirstOrDefault(x => x.Value == roleStr).Key;
            }
            return Tools.getFullName(studyteam.FirstOrDefault(x => (string)x["Role"] == role));
        }

    }
}
