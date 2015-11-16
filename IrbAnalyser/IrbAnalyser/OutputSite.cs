using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    class OutputSite
    {
        public static string EMmainsite = "Einstein Montefiore (non-treating site)";
        public static string siteTypeTreating = "Treating Site"; //Treating Site or treatingSite
        public static string siteTypePrimary = "Primary/Responsible"; //Primary/Responsible or primary
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

                newSites.Columns.Add("Study_number", typeof(string));

                newSites.Columns.Add("Organization", typeof(string));
                newSites.Columns.Add("Local sample size", typeof(string));
                newSites.Columns.Add("Site_Type", typeof(string));
            }

            if (updatedSites.Columns.Count == 0)
            {
                updatedSites.Columns.Add("TYPE", typeof(string));

                updatedSites.Columns.Add("Study_number", typeof(string));

                updatedSites.Columns.Add("Organization", typeof(string));
                updatedSites.Columns.Add("Local sample size", typeof(string));
                updatedSites.Columns.Add("Site_Type", typeof(string));
            }
        }

        /// <summary>
        /// Analyse each study row from the import document
        /// </summary>
        /// <param name="studyrow"></param>
        public static void analyseRow(DataRow studyrow, bool newrecord)
        {

            string site = "";
            string siteType = "";
            string irbstudyId = (string)studyrow["StudyId"];

            if (Agency.AgencySetupVal == Agency.AgencyList.NONE && newrecord)
            {
                var siteslist = ((string)studyrow["Sitename"]).Split(',');
                foreach (var sit in siteslist)
                {
                    addRow("New Site", sit, OutputSite.siteTypeTreating, "", irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), true);
                }
                site = OutputSite.EMmainsite;
            }
            else if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
            {
                site = OutputSite.EMmainsite;
            }
            else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                site = BranySiteMap.getSite(((string)studyrow["Sitename"]).Replace("(IBC)", ""));
                siteType = BranySiteMap.getSiteType(((string)studyrow["Sitename"]).Replace("(IBC)", ""));
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                site = IRISMap.SiteMap.getSite((string)studyrow["Sitename"]);
                siteType = IRISMap.SiteMap.getSiteType((string)studyrow["Sitename"]);
            }
            
            if (OutputStudy.shouldStudyBeAdded(irbstudyId) && site != OutputSite.EMmainsite)
            {
                string size = (string)studyrow["Sitesamplesize"];
                if (!newrecord)
                {
                    if (!String.IsNullOrEmpty(site))
                    {
                        IEnumerable<Model.VDA_V_STUDYSITES> sites2;
                        //BRANY look up agency in MSD
                        if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                        {
                            sites2 = (from sit in sites
                                      where sit.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                        && sit.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                        && sit.SITE_NAME == site
                                      select sit);
                        }
                        //IRIS all other agency in MSD, non IRB studies wont have 
                        else
                        {
                            sites2 = (from sit in sites
                                      where sit.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                        && sit.MORE_IRBAGENCY.ToLower() != Agency.brany
                                        && sit.SITE_NAME == site
                                      select sit);
                        }


                        if (sites2.Count() == 0 && !site.Contains("(IBC)"))
                        {
                            addRow("New Site", site, siteType, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), true);
                        }
                        else if (sites2.FirstOrDefault().STUDYSITE_LSAMPLESIZE != size && !String.IsNullOrEmpty(size) && !site.Contains("(IBC)"))
                        {
                            addRow("Modified site", site, siteType, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), false);
                        }
                    }

                }
                else if (!site.Contains("(IBC)"))
                {
                    addRow("New study", site, siteType, size, irbstudyId, ((string)studyrow["IRBNumber"]).Replace("(IBC)", ""), true);
                }
            }
        }

        public static void addRow(string type, string site, string siteType, string size, string studyid, string IRBno, bool newrecord)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            { dr = newSites.NewRow(); }
            else
            { dr = updatedSites.NewRow(); }
            dr["Type"] = type;

            dr["Study_number"] = Tools.getOldStudyNumber(studyid, IRBno);

            dr["Organization"] = site;
            dr["Local sample size"] = size;

            dr["Site_Type"] = siteType;

            if (newrecord)
            { newSites.Rows.Add(dr); }
            else
            { updatedSites.Rows.Add(dr); }
        }
    }
}