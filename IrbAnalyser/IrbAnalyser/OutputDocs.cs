using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    class OutputDocs
    {
        public static DataTable newDocs = new DataTable();
        public static DataTable updatedDocs = new DataTable();

        private static IEnumerable<Model.LCL_V_STUDYVER_APNDX> _versions;
        public static IEnumerable<Model.LCL_V_STUDYVER_APNDX> versions
        {
            get
            {
                if (_versions == null || _versions.Count() == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        var query = (from st in db.LCL_V_STUDYVER_APNDX
                                     where st.IRBIDENTIFIERS != null
                                     //&& st.MORE_IRBAGENCY != null
                                     && st.STUDYAPNDX_URI != null
                                     select st);
                        _versions = query.ToList<Model.LCL_V_STUDYVER_APNDX>();
                    }
                }
                return _versions;
            }
            set
            {
                _versions = value;
            }
        }


        private static void initiate()
        {
            if (newDocs.Columns.Count == 0)
            {
                newDocs.Columns.Add("TYPE", typeof(string));

                newDocs.Columns.Add("Study_number", typeof(string));

                newDocs.Columns.Add("Version date", typeof(string));
                newDocs.Columns.Add("Version number", typeof(string));
                newDocs.Columns.Add("Category", typeof(string));
                newDocs.Columns.Add("URL", typeof(string));
                newDocs.Columns.Add("Short description", typeof(string));
                newDocs.Columns.Add("Version status", typeof(string));
            }

            if (updatedDocs.Columns.Count == 0)
            {
                updatedDocs.Columns.Add("TYPE", typeof(string));

                updatedDocs.Columns.Add("Study_number", typeof(string));

                updatedDocs.Columns.Add("Version date", typeof(string));
                updatedDocs.Columns.Add("Version number", typeof(string));
                updatedDocs.Columns.Add("Category", typeof(string));
                updatedDocs.Columns.Add("URL", typeof(string));
                updatedDocs.Columns.Add("Short description", typeof(string));
                updatedDocs.Columns.Add("Version status", typeof(string));
            }
        }

        /// <summary>
        /// Analyse studyrow and add documents accordingly
        /// </summary>
        /// <param name="studyrow">StudyRow from datatable of study file</param>
        /// <param name="type"></param>
        /// <param name="source"></param>
        public static void analyseRow(DataRow studyrow, bool newrecord)
        {
            string irbstudyId = (string)studyrow["StudyId"];
            string irbno = ((string)studyrow["IRBNumber"]);
            string url = ((string)studyrow["DocumentLink1"]).ToLower();

            if (string.IsNullOrWhiteSpace(url))
            {
                if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                    url = "https://iris.einstein.yu.edu/";
                else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                    url = "https://brany.my.irbmanager.com/projects/" + ((string)studyrow["StudySiteId"]);
            }

            if (!newrecord && !((string)studyrow["IRBNumber"]).Contains("(IBC)"))
            {

                if (!String.IsNullOrEmpty(url))
                {

                    int docs = (from ver in versions
                                where ver.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                   && ver.STUDYAPNDX_URI.ToLower() == url
                                select ver).Count(); ;
                    /*
                    //BRANY look up agency in MSD
                    if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                    {
                        docs = (from ver in versions
                                where ver.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                   && ver.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                   && ver.STUDYAPNDX_URI.ToLower() == url
                                select ver).Count();
                    }
                    //IRIS all other agency in MSD, non IRB studies wont have 
                    else
                    {
                        docs = (from ver in versions
                                where ver.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                   && ver.MORE_IRBAGENCY.ToLower() != Agency.brany
                                   && ver.STUDYAPNDX_URI.ToLower() == url
                                select ver).Count();
                    }
                    */
                }
            }
            else if (!((string)studyrow["IRBNumber"]).Contains("(IBC)"))
            {
                if (!String.IsNullOrEmpty(url))
                {

                    addRow("New study", url, "documents", irbstudyId, irbno, true);
                }
            }


        }

        public static void addRow(string type, string url, string section, string studyid, string irbno, bool newrecord)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            {
                dr = newDocs.NewRow();
            }
            else
            { dr = updatedDocs.NewRow(); }

            dr["TYPE"] = type;

            dr["Study_number"] = Tools.getOldStudyNumber(studyid);

            dr["Version date"] = Tools.parseDate((string)DateTime.Now.ToShortDateString());
            
            dr["Category"] = "External Site Docs";
            dr["URL"] = url;
            if (url.ToLower().Contains("iris"))
            {
                dr["Short description"] = newrecord ? "IRIS IRB Documents" : "";
                dr["Version number"] = "IRIS " + section;
            }
            else
            {
                dr["Short description"] = newrecord ? "BRANY IRB Documents" : "";
                dr["Version number"] = "BRANY " + section;
            }
            dr["Version status"] = "Approved";

            if (newrecord)
            {
                newDocs.Rows.Add(dr);
                updatedDocs.ImportRow(dr);
            }
            else
            { updatedDocs.Rows.Add(dr); }
        }

    }
}
