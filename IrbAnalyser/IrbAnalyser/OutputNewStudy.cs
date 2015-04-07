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
                study.Columns.Add("IRBNumber", typeof(string));
                study.Columns.Add("StudyNumber", typeof(string));
            }
        }

        /// <summary>
        /// Add a new row to the study table
        /// </summary>
        /// <param name="irbNumber"></param>
        /// <param name="studyNumber"></param>
        public static void addRowStudy(string irbNumber, string studyNumber)
        {            
            initiate();
            var newRow = study.NewRow();
            newRow["IRBNumber"] = irbNumber;
            newRow["StudyNumber"] = studyNumber;
            study.Rows.Add(newRow);
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
