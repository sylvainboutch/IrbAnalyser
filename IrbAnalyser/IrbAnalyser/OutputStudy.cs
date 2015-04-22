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
                study.Columns.Add("Regulatory coordinator", typeof(string));
                study.Columns.Add("Principal Investigator", typeof(string));
                study.Columns.Add("Study number", typeof(string));
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
        public static void analyse(string filename)
        {
            initiate();
            FileParser fpStudy = new FileParser(filename);

            foreach (DataRow study in fpStudy.data.Rows)
            {
                analyseRow(study);
            }

            OutputSite.analyseDelete(fpStudy.data);
        }


        /// <summary>
        /// Analyse a row of the study report
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow dr)
        {
            OutputStatus.analyseRowStudy(dr);
            OutputSite.analyseRow(dr);
            OutputDocs.analyseRow(dr);
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = dr["StudyId"].ToString();
                string irbagency = dr["IRBAgency"].ToString().ToLower();

                var study = from st in db.LCL_V_STUDYSUMM_PLUSMORE
                            where st.MORE_IRBSTUDYID == irbstudyId
                            && st.MORE_IRBAGENCY.ToLower() == irbagency
                            select st;
                if (!study.Any())
                {
                    addRowStudy(dr, "New study");
                }
                else
                {
                    bool hasChanged = false;

                    foreach (var stu in study)
                    {
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

                        //TODO  Factor in the unit (week, days)
                        if (Tools.compareStr(stu.STUDY_DURATION, dr["Studyduration"]))
                        {
                            dr["Studyduration"] = "";
                        }
                        else
                        {
                            hasChanged = true;
                        }


                        if (Tools.compareStr(stu.STUDY_EST_BEGIN_DATE, dr["Begindate"]))
                        {
                            dr["Begindate"] = "";
                        }
                        else
                        {
                            hasChanged = true;
                        }

                        if (Tools.compareStr(stu.STUDY_NATSAMPSIZE, dr["Studysamplesize"]))
                        {
                            dr["Studysamplesize"] = "";
                        }
                        else
                        {
                            hasChanged = true;
                        }

                        bool cmp = (stu.STUDY_SCOPE == "Multi Center Study" && dr["Multicenter"].ToString().ToLower() == "true") ||
                            (stu.STUDY_SCOPE == "Single Center Study" && dr["Multicenter"].ToString().ToLower() == "false") ||
                            //TODO  check null
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
                    dr["Primarysponsorcontactfirstname"].ToString(),
                    dr["Primarysponsorcontactfirstname"].ToString()};

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

                        var documents = from sd in db.ER_STUDYAPNDX
                                        where stu.PK_STUDY == sd.FK_STUDY
                                        select sd;


                        if (documents.Count() == 0 && !String.IsNullOrEmpty(dr["Documentlink"].ToString()))
                        {
                            hasChanged = true;
                        }
                    }

                    if (hasChanged)
                    {
                        OutputStudy.addRowStudy(dr, "Modified study");
                    }
                }

            }
        }

        /// <summary>
        /// Add a new row to the study table
        /// </summary>
        /// <param name="irbNumber"></param>
        /// <param name="studyNumber"></param>
        private static void addRowStudy(string[] row)
        {
            initiate();
            study.Rows.Add(row);
        }

        /// <summary>
        /// Add a new row to the study table
        /// </summary>
        /// <param name="irbNumber"></param>
        /// <param name="studyNumber"></param>
        private static void addRowStudy(DataRow row, string type)
        {
            initiate();
            DataRow dr = study.NewRow();
            dr["TYPE"] = type;
            dr["IRB Agency name"] = row["IRBAgency"].ToString();
            dr["IRB no"] = row["IRBNumber"].ToString();
            dr["IRB Study ID"] = row["StudyId"].ToString();
            dr["Regulatory coordinator"] = "";
            dr["Principal Investigator"] = "";
            dr["Study number"] = "";
            dr["Official title"] = row["Studytitle"].ToString();
            dr["Study summary"] = row["Studysummary"].ToString();
            dr["Department"] = row["Department"].ToString();
            dr["Division/Therapeutic area"] = row["Division"].ToString();
            dr["Entire study sample size"] = row["Studysamplesize"].ToString();
            dr["Phase"] = row["Phase"].ToString() == "" ? "NA" : row["Phase"].ToString();
            dr["Research scope"] = row["Multicenter"].ToString() == "TRUE" ? "Multicenter" : "Single center";
            dr["Primary funding sponsor, if other :"] = row["Primarysponsorname"].ToString();
            dr["Sponsor contact"] = row["PrimarySponsorSontactFirstName"].ToString() + " " + row["PrimarySponsorSontactLastName"].ToString();
            dr["Sponsor Protocol ID"] = row["PrimarySponsorStudyId"].ToString();

            study.Rows.Add(dr);
        }
    }
}
