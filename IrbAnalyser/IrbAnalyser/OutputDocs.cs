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
                                     where st.MORE_IRBAGENCY != null
                                     && st.IRBIDENTIFIERS != null
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

                newDocs.Columns.Add("Study number", typeof(string));

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

                updatedDocs.Columns.Add("Study number", typeof(string));

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
            string url = ((string)studyrow["DocumentLink"]).ToLower();

            if (!newrecord && !((string)studyrow["IRBNumber"]).Contains("(IBC)"))
            {

                if (!String.IsNullOrEmpty(url))
                {
                    var docs = (from ver in versions
                                where ver.IRBIDENTIFIERS.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                   && ver.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                   && ver.STUDYAPNDX_URI.ToLower() == url
                                select ver).Count();
                    if (docs == 0)
                    {
                        addRow("New URL", url, "documents", irbstudyId, irbno, false);
                    }
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

            dr["Study number"] = Tools.getStudyNumber(studyid, irbno);

            dr["Version date"] = Tools.parseDate((string)DateTime.Now.ToShortDateString());
            dr["Version number"] = Agency.agencyStrLwr.ToUpper() + " " + section;
            dr["Category"] = "External Site Docs";
            dr["URL"] = url;
            dr["Short description"] = newrecord ? "BRANY IRB Documents" : "";
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
