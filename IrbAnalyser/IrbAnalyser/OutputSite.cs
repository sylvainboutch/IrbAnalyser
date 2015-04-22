using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    class OutputSite
    {
        public static DataTable sites = new DataTable();

        public static void initiate()
        {
            if (sites.Columns.Count == 0)
            {
                sites.Columns.Add("TYPE", typeof(string));
                sites.Columns.Add("Organisation", typeof(string));
                sites.Columns.Add("Local sample size", typeof(string));
            }
        }

        /// <summary>
        /// Analyse each study row from the import document
        /// </summary>
        /// <param name="studyrow"></param>
        public static void analyseRow(DataRow studyrow)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = studyrow["StudyId"].ToString();
                string irbagency = studyrow["IRBAgency"].ToString().ToLower();
                string site = studyrow["Sitename"].ToString();
                string size = studyrow["Sitesamplesize"].ToString();

                if (!String.IsNullOrEmpty(site))
                {
                    var docs = (from sit in db.VDA_V_STUDYSITES
                                join stud in db.LCL_V_STUDYSUMM_PLUSMORE on sit.FK_STUDY equals stud.PK_STUDY
                                where stud.MORE_IRBSTUDYID == irbstudyId
                                   && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                   && sit.SITE_NAME.ToLower() == site
                                select sit).Count();
                    if (docs == 0)
                    {
                        addRow("New Site", site, size);
                    }
                }
            }
        }

        /// <summary>
        /// Analyse complete tables to find removed value
        /// </summary>
        /// <param name="studys"></param>
        public static void analyseDelete(DataTable studys)
        {
            var std = from rw in studys.AsEnumerable()
                          select rw.Field<string>("StudyId");

            using (Model.VelosDb db = new Model.VelosDb())
            {
                var sites = from sit in db.VDA_V_STUDYSITES
                            join stud in db.LCL_V_STUDYSUMM_PLUSMORE on sit.FK_STUDY equals stud.PK_STUDY
                            where std.Contains(stud.MORE_IRBSTUDYID)
                            select new {site = sit.SITE_NAME, studyId = stud.MORE_IRBSTUDYID, agency = stud.MORE_IRBAGENCY} ;

                foreach (var site in sites)
                {
                    var countSite = (from DataRow dr in studys.Rows
                                     where (string)dr["Sitename"] == site.site
                                     select dr).Count();
                    var countStudy = (from DataRow dr in studys.Rows
                                      where (string)dr["IRBAgency"] == site.agency
                                      && (string)dr["StudyId"] == site.studyId
                                      select dr).Count();
                    if (countSite == 0 && countStudy != 0)
                    {
                        addRow("Deleted site", site.site, "");
                    }
                }
            }
        }

        public static void addRow(string type, string site, string size)
        {
            initiate();
            DataRow dr = sites.NewRow();
            dr["Type"] = type;
            dr["Organisation"] = site;
            dr["Local sample size"] = size;
            sites.Rows.Add(dr);
        }
    }
}
