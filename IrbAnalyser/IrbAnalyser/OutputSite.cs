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

                sites.Columns.Add("IRB Agency name", typeof(string));
                sites.Columns.Add("IRB no", typeof(string));
                sites.Columns.Add("IRB Study ID", typeof(string));
                sites.Columns.Add("Study name", typeof(string));

                sites.Columns.Add("Organization", typeof(string));
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

                string site = "";

                if ((string)studyrow["IRBAgency"] == "BRANY")
                {
                    site = BranySiteMap.siteMapBrany[(string)studyrow["Sitename"]];
                }

                string irbstudyId = (string)studyrow["StudyId"];
                string irbagency = ((string)studyrow["IRBAgency"]).ToLower();
                string size = (string)studyrow["Sitesamplesize"];

                if (!String.IsNullOrEmpty(site))
                {
                    var sites = (from sit in db.VDA_V_STUDYSITES
                                join stud in db.LCL_V_STUDYSUMM_PLUSMORE on sit.FK_STUDY equals stud.PK_STUDY
                                where stud.MORE_IRBSTUDYID == irbstudyId
                                   && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                   && sit.SITE_NAME == site
                                select sit);
                    if (sites.Count() == 0)
                    {
                        addRow("New Site", site, size, irbstudyId, irbagency);
                    }
                    else if (sites.FirstOrDefault().STUDYSITE_LSAMPLESIZE != size && !String.IsNullOrEmpty(size))
                    {
                        addRow("Modified site", site, size, irbstudyId, irbagency);
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
                                     where BranySiteMap.siteMapBrany[(string)dr["Sitename"]] == site.site
                                     select dr).Count();
                    var countStudy = (from DataRow dr in studys.Rows
                                      where (string)dr["IRBAgency"] == site.agency
                                      && (string)dr["StudyId"] == site.studyId
                                      select dr).Count();
                    if (countSite == 0 && countStudy != 0)
                    {
                        addRow("Deleted site", site.site, "", site.studyId, site.agency);
                    }
                }
            }
        }

        public static void addRow(string type, string site, string size, string studyid, string agency)
        {
            initiate();
            DataRow dr = sites.NewRow();
            dr["Type"] = type;

            dr["IRB Agency name"] = agency;
            dr["IRB no"] = "";
            dr["IRB Study ID"] = studyid;
            dr["Study name"] = Tools.getStudyNumber(studyid, agency);

            dr["Organization"] = site;
            dr["Local sample size"] = size;
            sites.Rows.Add(dr);
        }
    }
}
