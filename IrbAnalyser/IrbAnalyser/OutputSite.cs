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

        private static IEnumerable<Model.VDA_V_STUDYSITES> _sites;
        public static IEnumerable<Model.VDA_V_STUDYSITES> sites
        {
            get
            {
                if (_sites == null || _sites.Count() == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        var query = (from st in db.VDA_V_STUDYSITES
                                     where st.MORE_IRBAGENCY != null
                                     && st.IRBIDENTIFIERS != null
                                     select st);
                        _sites = query.ToList<Model.VDA_V_STUDYSITES>();
                    }
                }
                return _sites;
            }
            set
            {
                _sites = value;
            }
        }


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
            else if (Agency.AgencyVal == Agency.AgencyList.IRIS)
            {
                site = IRISMap.SiteMap.getSite((string)studyrow["Sitename"]);
            }
            string irbstudyId = (string)studyrow["StudyId"];
            string size = (string)studyrow["Sitesamplesize"];
            if (!newrecord)
            {
                if (!String.IsNullOrEmpty(site))
                {
                    var sites2 = (from sit in sites
                                  where sit.IRBIDENTIFIERS.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                    && sit.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                    && sit.SITE_NAME == site
                                 select sit);
                    if (sites2.Count() == 0 && !site.Contains("(IBC)"))
                    {
                        addRow("New Site", site, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), true);
                    }
                    else if (sites2.FirstOrDefault().STUDYSITE_LSAMPLESIZE != size && !String.IsNullOrEmpty(size) && !site.Contains("(IBC)"))
                    {
                        addRow("Modified site", site, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), false);
                    }
                }

            }
            else if (!site.Contains("(IBC)"))
            {
                addRow("New study", site, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), true);
            }

        }

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