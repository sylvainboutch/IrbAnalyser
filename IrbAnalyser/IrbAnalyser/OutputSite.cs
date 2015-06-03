using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    class OutputSite
    {
        public static DataTable newSites = new DataTable();
        public static DataTable updatedSites = new DataTable();

        public static void initiate()
        {
            if (newSites.Columns.Count == 0)
            {
                newSites.Columns.Add("TYPE", typeof(string));

                newSites.Columns.Add("Study number", typeof(string));

                newSites.Columns.Add("Organization", typeof(string));
                newSites.Columns.Add("Local sample size", typeof(string));
            }

            if (updatedSites.Columns.Count == 0)
            {
                updatedSites.Columns.Add("TYPE", typeof(string));

                updatedSites.Columns.Add("Study number", typeof(string));

                updatedSites.Columns.Add("Organization", typeof(string));
                updatedSites.Columns.Add("Local sample size", typeof(string));
            }
        }

        /// <summary>
        /// Analyse each study row from the import document
        /// </summary>
        /// <param name="studyrow"></param>
        public static void analyseRow(DataRow studyrow, bool newrecord)
        {

            string site = "";

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                site = BranySiteMap.getSite(((string)studyrow["Sitename"]).Replace("(IBC)", ""));
            }

            string irbstudyId = (string)studyrow["StudyId"];
            string size = (string)studyrow["Sitesamplesize"];
            if (!newrecord)
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {
                    if (!String.IsNullOrEmpty(site))
                    {
                        var sites = (from sit in db.VDA_V_STUDYSITES
                                     join stud in db.LCL_V_STUDYSUMM_PLUSMORE on sit.FK_STUDY equals stud.PK_STUDY
                                     where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                        && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                        && sit.SITE_NAME == site
                                     select sit);
                        if (sites.Count() == 0)
                        {
                            addRow("New Site", site, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), true);
                        }
                        else if (sites.FirstOrDefault().STUDYSITE_LSAMPLESIZE != size && !String.IsNullOrEmpty(size))
                        {
                            addRow("Modified site", site, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), false);
                        }
                    }
                }
            }
            else
            {
                addRow("New study", site, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), true);
            }

        }

        /*
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
                            select new { site = sit.SITE_NAME, studyId = stud.MORE_IRBSTUDYID, agency = stud.MORE_IRBAGENCY };

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
        }*/

        public static void addRow(string type, string site, string size, string studyid, string IRBno, bool newrecord)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            { dr = newSites.NewRow(); }
            else
            { dr = updatedSites.NewRow(); }
            dr["Type"] = type;

            dr["Study number"] = Tools.getStudyNumber(studyid,IRBno);

            dr["Organization"] = site;
            dr["Local sample size"] = size;
            if (newrecord)
            { newSites.Rows.Add(dr); }
            else
            { updatedSites.Rows.Add(dr); }
        }
    }
}