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
                newStudy.Columns.Add("Study summary", typeof(string));
                newStudy.Columns.Add("Department", typeof(string));
                newStudy.Columns.Add("Division/Therapeutic area", typeof(string));
                newStudy.Columns.Add("Entire study sample size", typeof(string));
                newStudy.Columns.Add("Phase", typeof(string));
                newStudy.Columns.Add("Study scope", typeof(string));
                newStudy.Columns.Add("Primary funding sponsor, if other :", typeof(string));
                newStudy.Columns.Add("Sponsor contact", typeof(string));
                newStudy.Columns.Add("Sponsor Protocol ID", typeof(string));
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
            }
        }


        /// <summary>
        /// Analyse the study report
        /// </summary>
        public static void analyse(string filepath)
        {
            initiate();
            FileParser fpStudy = new FileParser(filepath + "Study.txt",FileParser.type.Study);

            foreach (DataRow study in fpStudy.data.Rows)
            {
                analyseRow(study, filepath + "Team.txt");
            }

            //OutputSite.analyseDelete(fpStudy.data);
        }


        /// <summary>
        /// Analyse a row of the study report
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow dr, string teamfile)
        {
            string irbstudyId = (string)dr["StudyId"];
            //string irbagency = ((string)dr["IRBAgency"]).ToLower();
            string irbagency = "BRANY";
            if (!String.IsNullOrEmpty(irbagency) && !String.IsNullOrEmpty(irbstudyId))
            {

                using (Model.VelosDb db = new Model.VelosDb())
                {
                    
                    var study = from st in db.LCL_V_STUDYSUMM_PLUSMORE
                                where st.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                && st.MORE_IRBAGENCY.ToLower() == irbagency.ToLower()
                                select st;



                    if (!study.Any())
                    {
                        bool dtStudy = (from st in OutputStudy.newStudy.AsEnumerable()
                                        where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                        && st.Field<string>("IRB Agency name").Trim().ToLower() == irbagency.Trim().ToLower()
                                        select st).Any();

                        OutputStatus.analyseRowStudy(dr, true);
                        if (!dtStudy)
                        {
                            addRowStudy(dr, true, teamfile);
                            //Add all related values for that study                            
                            OutputSite.analyseRow(dr, true);
                            OutputDocs.analyseRow(dr, true);
                        }
                    }
                    else
                    {
                        bool dtStudy = (from st in OutputStudy.updatedStudy.AsEnumerable()
                                        where st.Field<string>("IRB Study ID").Trim().ToLower() == irbstudyId.Trim().ToLower()
                                        && st.Field<string>("IRB Agency name").Trim().ToLower() == irbagency.Trim().ToLower()
                                        select st).Any();

                        OutputStatus.analyseRowStudy(dr, false);
                        if (!dtStudy)
                        {

                            OutputSite.analyseRow(dr, false);
                            OutputDocs.analyseRow(dr, false);

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
                                addRowStudy(dr, false, teamfile, newpi, newrc);
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
        private static void addRowStudy(DataRow row, bool newentry, string teamfile, string newpi = "", string newrc = "")
        {
            initiate();
            DataRow dr;
            if (newentry)
            { dr = newStudy.NewRow(); }
            else
            { dr = updatedStudy.NewRow(); }

            dr["IRB Agency name"] = (string)row["IRBAgency"];
            dr["IRB no"] = ((string)row["IRBNumber"]).Replace("(IBC)", "");
            dr["IRB Study ID"] = (string)row["StudyId"];
            dr["IRB Identifiers"] = Tools.generateStudyIdentifiers(dr.Table, (string)row["StudyId"], (string)row["IRBAgency"]);
            //dr["Study number"] = Tools.generateStudyNumber((string)row["IRBAgency"], (string)row["IRBNumber"], "Please complete");
            dr["Study number"] = Tools.studyNumber((string)row["StudyId"], (string)row["IRBAgency"], (string)dr["IRB no"], "Please complete");

            dr["Study coordinator"] = getRC(teamfile, (string)row["IRBAgency"], (string)row["StudyId"]);
            dr["Principal Investigator"] = getPI(teamfile, (string)row["IRBAgency"], (string)row["StudyId"]);
            dr["Official title"] = (string)row["StudyTitle"];
            dr["Study summary"] = row["Studysummary"].ToString();
            dr["Department"] = String.IsNullOrEmpty((string)row["Department"]) && newentry ? "Please specify" : (string)row["Department"];
            dr["Division/Therapeutic area"] = String.IsNullOrEmpty((string)row["Division"]) && newentry ? "N/A" : (string)row["Division"];
            dr["Entire study sample size"] = row["Studysamplesize"].ToString();
            dr["Phase"] = String.IsNullOrEmpty((string)row["Phase"]) && newentry ? "Please Specify" : (string)row["Phase"];
            dr["Study scope"] = row["Multicenter"].ToString() == "TRUE" ? "Multi Center Study" : "Single Center Study";
            dr["Primary funding sponsor, if other :"] = row["Primarysponsorname"].ToString();
            dr["Sponsor contact"] = row["PrimarySponsorContactFirstName"].ToString() + " " + row["PrimarySponsorContactLastName"].ToString();
            dr["Sponsor Protocol ID"] = row["PrimarySponsorStudyId"].ToString();

            string[] labels = new string[3]{"IRB agency name","IRB No.","OFFICE USE ONLY - DO NOT MODIFY - IRB Identifiers"};
            string[] values = new string[3] { (string)row["IRBAgency"], (string)dr["IRB no"], (string)dr["IRB Identifiers"] };

            OutputMSD.initiate();

            for (int i = 0; i < labels.Count(); i++)
            {
                OutputMSD.addRow(labels[i], values[i], (string)row["StudyId"], (string)row["IRBAgency"], (string)dr["IRB no"], newentry);
            }


            if (newentry)
            { newStudy.Rows.Add(dr); }
            else
            { updatedStudy.Rows.Add(dr); }

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
            FileParser fpTeam = new FileParser(teamfile, FileParser.type.Team);

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
