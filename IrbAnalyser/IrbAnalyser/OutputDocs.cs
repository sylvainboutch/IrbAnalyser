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

        private static string branydocs = "BRANY Documents";
        private static string irisdocs = "IRIS Documents";

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
                                     //&& st.STUDYAPNDX_URI != null
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
                newDocs.Columns.Add("PK_STUDYVER", typeof(string));
                newDocs.Columns.Add("PK_STUDYAPNDX", typeof(string));
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
                updatedDocs.Columns.Add("PK_STUDYVER", typeof(string));
                updatedDocs.Columns.Add("PK_STUDYAPNDX", typeof(string));
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



                    int docs2 = (from ver in versions
                                where ver != null && ver.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                   && !String.IsNullOrWhiteSpace(ver.STUDYVER_NUMBER) && (ver.STUDYVER_NUMBER.ToLower() == irisdocs.ToLower() || ver.STUDYVER_NUMBER.ToLower() == branydocs.ToLower())
                                select ver).Count();


                    if ((string)studyrow["IRBNumber"] == "2015-5616")
                    {
                        var test = (from ver in versions
                                     where ver != null && ver.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                     select ver);
                        studyrow["DocumentLink1"] = test.ToString();
                    }

                    if (docs2 == 0)
                    {
                        addRow("Updated Study - Missing Version", url, irbstudyId, irbno, true);
                        addRow("Updated Study - Missing Version", url, irbstudyId, irbno, false);
                    }
                    else 
                    {
                        var docs = (from ver in versions
                                    where ver != null && ver.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                       && !String.IsNullOrWhiteSpace(ver.STUDYAPNDX_URI) && ver.STUDYAPNDX_URI.ToLower() == url.ToLower()
                                    select ver);

                        if (docs.Count() == 0)
                        {
                            docs = (from ver in versions
                                    where ver != null && ver.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                       && !String.IsNullOrWhiteSpace(ver.STUDYAPNDX_URI) && (ver.STUDYAPNDX_URI.ToLower().Contains("https://") && (ver.STUDYAPNDX_URI.ToLower().Contains("brany") || ver.STUDYAPNDX_URI.ToLower().Contains("iris")))
                                    select ver);

                            if (docs.Count() == 0)
                            {
                                docs = (from ver in versions
                                        where ver != null && ver.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                           && !String.IsNullOrWhiteSpace(ver.STUDYVER_NUMBER) && (ver.STUDYVER_NUMBER.ToLower() == irisdocs.ToLower() || ver.STUDYVER_NUMBER.ToLower() == branydocs.ToLower())
                                        select ver);
                            }

                            int PK_STUDYVER = docs.FirstOrDefault() == null ||  docs.FirstOrDefault().PK_STUDYVER == null ? 0:(int)docs.FirstOrDefault().PK_STUDYVER;
                            int PK_STUDYAPNDX = docs.FirstOrDefault() == null || docs.FirstOrDefault().PK_STUDYAPNDX == null ? 0 : (int)docs.FirstOrDefault().PK_STUDYAPNDX;

                            addRow("Updated Study - Wrong URL", url, irbstudyId, irbno, false, PK_STUDYVER, PK_STUDYAPNDX);
                        }
                    }

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

                    addRow("New study", url, irbstudyId, irbno, true);
                }
            }


        }

        public static void addRow(string type, string url, string studyid, string irbno, bool newrecord, int PK_STUDYVER = 0, int PK_STUDYAPNDX = 0)
        {
            initiate();
            DataRow dr;
            string studyNumber = Tools.getOldStudyNumber(studyid);

            if (newrecord)
            {
                if (newDocs.AsEnumerable().Where(x => x.Field<string>("Study_number") == studyNumber).Any()) { return; }
                dr = newDocs.NewRow();
            }
            else
            {
                if (updatedDocs.AsEnumerable().Where(x => x.Field<string>("Study_number") == studyNumber).Any()) { return; }
                dr = updatedDocs.NewRow(); 
            }

            dr["TYPE"] = type;

            dr["Study_number"] = studyNumber;

            dr["Version date"] = Tools.parseDate((string)DateTime.Now.ToShortDateString());
            
            dr["Category"] = "External Site Docs";
            dr["URL"] = url;
            if (url.ToLower().Contains("iris"))
            {
                dr["Short description"] = newrecord ? "IRIS IRB Documents" : "";
                dr["Version number"] = irisdocs;
            }
            else
            {
                dr["Short description"] = newrecord ? "BRANY IRB Documents" : "";
                dr["Version number"] = branydocs;
            }
            dr["Version status"] = "Approved";

            dr["PK_STUDYAPNDX"] = PK_STUDYAPNDX;
            dr["PK_STUDYVER"] = PK_STUDYVER;

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
