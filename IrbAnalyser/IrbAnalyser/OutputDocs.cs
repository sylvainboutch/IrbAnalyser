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

        private static void initiate()
        {
            if (newDocs.Columns.Count == 0)
            {
                newDocs.Columns.Add("TYPE", typeof(string));

                newDocs.Columns.Add("IRB Agency name", typeof(string));
                newDocs.Columns.Add("IRB no", typeof(string));
                newDocs.Columns.Add("IRB Study ID", typeof(string));
                newDocs.Columns.Add("Study name", typeof(string));

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

                updatedDocs.Columns.Add("IRB Agency name", typeof(string));
                updatedDocs.Columns.Add("IRB no", typeof(string));
                updatedDocs.Columns.Add("IRB Study ID", typeof(string));
                updatedDocs.Columns.Add("Study name", typeof(string));

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
            string irbagency = ((string)studyrow["IRBAgency"]).ToLower();
            string irbno = ((string)studyrow["IRBNumber"]).Replace("(IBC)", "");
            string url1 = ((string)studyrow["DocumentLink1"]).ToLower();
            string url2 = ((string)studyrow["DocumentLink2"]).ToLower();

            if (!newrecord)
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {

                    if (!String.IsNullOrEmpty(url1))
                    {
                        var docs = (from ver in db.ER_STUDYVER
                                    join apdx in db.ER_STUDYAPNDX on ver.PK_STUDYVER equals apdx.FK_STUDYVER
                                    join stud in db.LCL_V_STUDYSUMM_PLUSMORE on ver.FK_STUDY equals stud.PK_STUDY
                                    where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                       && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                       && apdx.STUDYAPNDX_URI.ToLower() == url1
                                    select ver).Count();
                        if (docs == 0)
                        {
                            addRow("New URL", irbagency, url1, "documents", irbstudyId, irbagency, irbno, false);
                        }
                    }

                    if (!String.IsNullOrEmpty(url2))
                    {
                        var docs = (from ver in db.ER_STUDYVER
                                    join apdx in db.ER_STUDYAPNDX on ver.PK_STUDYVER equals apdx.FK_STUDYVER
                                    join stud in db.LCL_V_STUDYSUMM_PLUSMORE on ver.FK_STUDY equals stud.PK_STUDY
                                    where stud.MORE_IRBSTUDYID == irbstudyId
                                       && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                       && apdx.STUDYAPNDX_URI.ToLower() == url2
                                    select ver).Count();
                        if (docs == 0)
                        {
                            addRow("New URL", irbagency, url2, "informed consent", irbstudyId, irbagency, irbno, false);
                        }
                    }

                }
            }
            else
            {

                if (!String.IsNullOrEmpty(url1))
                {

                    addRow("New URL", irbagency, url1, "documents", irbstudyId, irbagency, irbno, true);
                }

                if (!String.IsNullOrEmpty(url2))
                {
                    addRow("New URL", irbagency, url2, "informed consent", irbstudyId, irbagency, irbno, true);
                }
            }


        }

        public static void addRow(string type, string source, string url, string section, string studyid, string agency, string irbno, bool newrecord)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            { dr = newDocs.NewRow(); }
            else
            { dr = updatedDocs.NewRow(); }

            dr["TYPE"] = type;
            dr["IRB Agency name"] = agency;
            dr["IRB no"] = irbno;
            dr["IRB Study ID"] = studyid;
            dr["Study name"] = Tools.studyNumber(studyid, agency, irbno, "Please complete");
            dr["Version date"] = Tools.parseDate((string)DateTime.Now.ToShortDateString());
            dr["Version number"] = source.ToUpper() + " " + section;
            dr["Category"] = "External Site Docs";
            dr["URL"] = url;
            dr["Short description"] = newrecord ? "BRANY IRB Documents":"";
            dr["Version status"] = "Approved";

            if (newrecord)
            { newDocs.Rows.Add(dr); }
            else
            { updatedDocs.Rows.Add(dr); }
        }
    }
}
