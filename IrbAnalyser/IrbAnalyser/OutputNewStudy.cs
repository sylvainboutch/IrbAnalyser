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
    public static class OutputNewStudy
    {
        //List of newly created study, with more study detail
        public static DataTable study = new DataTable();

        /// <summary>
        /// Add the columns to the datatable
        /// </summary>
        public static void initiate()
        {
            if (study.Columns.Count == 0)
            {
                study.Columns.Add("IRB Agency name", typeof(string));
                study.Columns.Add("IRB no", typeof(string));
                study.Columns.Add("Regulatory coordinator", typeof(string));
                study.Columns.Add("Principal Investigator", typeof(string));
                study.Columns.Add("Study number", typeof(string));
                study.Columns.Add("Official title", typeof(string));
                study.Columns.Add("Study summary", typeof(string));
                study.Columns.Add("IND/IDE Information available", typeof(string));
                study.Columns.Add("IND/IDE Number", typeof(string));
                study.Columns.Add("NA", typeof(string));
                study.Columns.Add("IND/IDE Grantor*", typeof(string));
                study.Columns.Add("IND/IDE Holder Type*", typeof(string));
                study.Columns.Add("Department", typeof(string));
                study.Columns.Add("Division/Therapeutic area", typeof(string));
                study.Columns.Add("Entire study sample size", typeof(string));
                study.Columns.Add("Study duration", typeof(string));
                study.Columns.Add("Estimated Begin date", typeof(string));
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
        public static void addRowStudy(DataRow row)
        {
            initiate();
            DataRow dr = study.NewRow();
            dr["IRB Agency name"] = row["IRBAgency"].ToString();
            dr["IRB no"] = row["IRBNumber"].ToString();
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
            dr["Study duration"] = row["Studyduration"].ToString();
            dr["Estimated Begin date"] = row["Begindate"].ToString();
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
