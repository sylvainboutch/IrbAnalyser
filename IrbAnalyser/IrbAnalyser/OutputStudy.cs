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

        public static DataTable site = new DataTable();
        /// <summary>
        /// Add the columns to the datatable
        /// </summary>
        public static void initiate()
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
                study.Columns.Add("IND/IDE Information available", typeof(string));
                study.Columns.Add("IND/IDE Number", typeof(string));
                study.Columns.Add("IND holder", typeof(string));
                study.Columns.Add("IND/IDE Grantor*", typeof(string));
                study.Columns.Add("IND/IDE Holder Type*", typeof(string));
                study.Columns.Add("Department", typeof(string));
                study.Columns.Add("Division/Therapeutic area", typeof(string));
                study.Columns.Add("Entire study sample size", typeof(string));
                study.Columns.Add("Phase", typeof(string));
                study.Columns.Add("Research scope", typeof(string));
                study.Columns.Add("Primary funding sponsor, if other :", typeof(string));
                study.Columns.Add("Sponsor contact", typeof(string));
                study.Columns.Add("Sponsor Protocol ID", typeof(string));
                study.Columns.Add("Version date", typeof(string));
                study.Columns.Add("Version number", typeof(string));
                study.Columns.Add("Type", typeof(string));
                study.Columns.Add("Category", typeof(string));
                study.Columns.Add("URL", typeof(string));
                study.Columns.Add("Version status, Work in progress, approved, archived", typeof(string));
                study.Columns.Add("Short description", typeof(string));
            }

            if (site.Columns.Count == 0)
            {
                site.Columns.Add("TYPE", typeof(string));
                site.Columns.Add("IRB Agency name", typeof(string));
                site.Columns.Add("IRB no", typeof(string));
                site.Columns.Add("IRB Study ID", typeof(string));
                //Need mapping for the site
                site.Columns.Add("Sitename",typeof(string));
            }
        }

        /// <summary>
        /// Add a new row to the study table
        /// </summary>
        /// <param name="irbNumber"></param>
        /// <param name="studyNumber"></param>
        public static void addRowStudy(string[] row)
        {            
            initiate();
            study.Rows.Add(row);
        }

        /// <summary>
        /// Add a new row to the study table
        /// </summary>
        /// <param name="irbNumber"></param>
        /// <param name="studyNumber"></param>
        public static void addRowStudy(DataRow row,string type)
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
            dr["IND/IDE Information available"] = row["IND"].ToString();
            dr["IND/IDE Number"] = row["INDnumber"].ToString();
            dr["IND/IDE Grantor*"] = row["INDHolder"].ToString();
            dr["IND/IDE Holder Type*"] = "";
            dr["Department"] = row["Department"].ToString();
            dr["Division/Therapeutic area"] = row["Division"].ToString();
            dr["Entire study sample size"] = row["Studysamplesize"].ToString();
            dr["Phase"] = row["Phase"].ToString() == "" ? "NA" : row["Phase"].ToString() ;
            dr["Research scope"] = row["Multicenter"].ToString() == "TRUE" ? "Multicenter":"Single center";
            dr["Primary funding sponsor, if other :"] = row["Primarysponsorname"].ToString();
            dr["Sponsor contact"] = row["Primarysponsorcontactfirstname"].ToString() + " " + row["Primarysponsorcontactlastname"].ToString() + " : " + row["Primarysponsorcontactemail"].ToString();
            dr["Sponsor Protocol ID"] = row["PrimarysponsorstudyID"].ToString();
            dr["Version date"] = DateTime.Now;
            dr["Version number"] = row["IRBAgency"].ToString() + " documents";
            dr["Type"] = "";
            dr["Category"] = "";
            dr["URL"] = row["Documentlink"].ToString();
            dr["Version status, Work in progress, approved, archived"] = "Approved";
            dr["Short description"] = "";

            study.Rows.Add(dr);
        }




        /// <summary>
        /// Verify if a study has changed and add the changes to the list
        /// </summary>
        /// <param name="row"></param>
        public static void changedValue(DataRow row)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = row["StudyId"].ToString();
                string irbagency = row["IRBAgency"].ToString().ToLower();

                var study = from st in db.LCL_V_STUDYSUMM_PLUSMORE
                            where st.MORE_IRBSTUDYID == irbstudyId
                            && st.MORE_IRBAGENCY.ToLower() == irbagency
                            select st;

                bool hasChanged = false;

                foreach (var stu in study)
                {
                    if (!Tools.compareStr(stu.STUDY_TITLE, row["Studytitle"]))
                    {
                        row["Studytitle"] = row["Studytitle"];
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    if (Tools.compareStr(stu.STUDY_SUMMARY, row["Studysummary"]))
                    {
                        row["Studysummary"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    var indide = from sd in db.LCL_V_STUDY_INDIDE
                                 where stu.PK_STUDY == sd.FK_STUDY
                                 select sd;

                    if (indide.Count() == 0 && row["IND"].ToString().ToUpper() == "TRUE")
                    {
                        hasChanged = true;
                    }
                    if (indide.Count() == 0 && row["IND"].ToString().ToUpper() == "FALSE")
                    {
                        row["IND"] = "";
                    }


                    bool hasntchanged =  false;
                    
                    foreach (var ind in indide)
                    {
                        if (Tools.compareStr(ind.INDIDE_NUMBER, row["INDnumber"]))
                        {
                            row["INDnumber"] = "";
                            hasntchanged = true;
                        }                               
                    }

                    hasChanged = hasntchanged ? hasChanged : true;

                    //TODO What to do with IND Holder

                    if (Tools.compareStr(stu.STUDY_DIVISION, row["Department"]))
                    {
                        row["Department"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    if (Tools.compareStr(stu.STUDY_TAREA, row["Division"]))
                    {
                        row["Division"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    //TODO  Factor in the unit (week, days)
                    if (Tools.compareStr(stu.STUDY_DURATION, row["Studyduration"]))
                    {
                        row["Division"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }


                    if (Tools.compareStr(stu.STUDY_EST_BEGIN_DATE, row["Begindate"]))
                    {
                        row["Begindate"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    if (Tools.compareStr(stu.STUDY_NATSAMPSIZE, row["Studysamplesize"]))
                    {
                        row["Studysamplesize"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    bool cmp = (stu.STUDY_SCOPE == "Multi Center Study" && row["Multicenter"].ToString().ToLower() == "true") ||
                        (stu.STUDY_SCOPE == "Single Center Study" && row["Multicenter"].ToString().ToLower() == "false") ||
                        //TODO  check null
                        (stu.STUDY_SCOPE == null && row["Multicenter"].ToString().ToLower() == "");

                    if (cmp)
                    {
                        row["Multicenter"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    //TODO Phase need mapping from IRIS, BRANY doesnt have
                    //TODO This should also use a map
                    if (Tools.compareStr(stu.STUDY_SPONSOR, row["Primarysponsorname"]))
                    {
                        row["Primarysponsorname"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    string[] strs = {row["Primarysponsorcontactfirstname"].ToString(),
                    row["Primarysponsorcontactfirstname"].ToString(),
                    row["Primarysponsorcontactfirstname"].ToString()};

                    if (Tools.containStr(stu.STUDY_SPONSOR, strs))
                    {
                        row["Primarysponsorname"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    if (Tools.compareStr(stu.STUDY_SPONSORID, row["PrimarysponsorstudyID"]))
                    {
                        row["PrimarysponsorstudyID"] = "";
                    }
                    else
                    {
                        hasChanged = true;
                    }

                    var documents = from sd in db.ER_STUDYAPNDX
                                 where stu.PK_STUDY == sd.FK_STUDY
                                 select sd;


                    if (documents.Count() == 0 && !String.IsNullOrEmpty(row["Documentlink"].ToString()))
                    {
                        hasChanged = true;
                    }

                    hasntchanged =  false;
                    
                    foreach (var doc in documents)
                    {
                        if (Tools.compareStr(doc.STUDYAPNDX_URI, row["Documentlink"]))
                        {
                            row["Documentlink"] = "";
                            hasntchanged = true;
                        }                               
                    }

                    hasChanged = hasntchanged ? hasChanged : true;
                }

                if (hasChanged)
                {
                    OutputStudy.addRowStudy(row,"Modified study");
                }
            }        
        }


        /// <summary>
        /// Print the study table
        /// </summary>
        /// <returns></returns>
        public static string printStudy()
        {
            string ret = "";
            foreach (DataRow dataRow in study.Rows)
            {
                ret += "\r\n";
                foreach (var item in dataRow.ItemArray)
                {
                    ret += "  |  " + item;
                }
            }
            return ret;
        }

    }
}
