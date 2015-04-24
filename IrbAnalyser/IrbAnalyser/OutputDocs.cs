using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    class OutputDocs
    {
        public static DataTable docs = new DataTable();

        private static void initiate()
        {
            if (docs.Columns.Count == 0)
            {
                docs.Columns.Add("TYPE", typeof(string));

                docs.Columns.Add("IRB Agency name", typeof(string));
                docs.Columns.Add("IRB no", typeof(string));
                docs.Columns.Add("IRB Study ID", typeof(string));
                docs.Columns.Add("Study name", typeof(string));

                docs.Columns.Add("Version date", typeof(string));
                docs.Columns.Add("Version number", typeof(string));
                docs.Columns.Add("Category", typeof(string));
                docs.Columns.Add("URL", typeof(string));
                docs.Columns.Add("Version status", typeof(string));
            }
        }

        /// <summary>
        /// Analyse studyrow and add documents accordingly
        /// </summary>
        /// <param name="studyrow">StudyRow from datatable of study file</param>
        /// <param name="type"></param>
        /// <param name="source"></param>
        public static void analyseRow(DataRow studyrow)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = studyrow["StudyId"].ToString();
                string irbagency = studyrow["IRBAgency"].ToString().ToLower();
                string url1 = studyrow["DocumentLink1"].ToString().ToLower();
                string url2 = studyrow["DocumentLink2"].ToString().ToLower();

                if (!String.IsNullOrEmpty(url1))
                {
                    var docs = (from ver in db.ER_STUDYVER
                               join apdx in db.ER_STUDYAPNDX on ver.PK_STUDYVER equals apdx.FK_STUDYVER
                               join stud in db.LCL_V_STUDYSUMM_PLUSMORE on ver.FK_STUDY equals stud.PK_STUDY
                               where stud.MORE_IRBSTUDYID == irbstudyId
                                  && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                  && apdx.STUDYAPNDX_URI.ToLower() == url1
                               select ver).Count();
                    if (docs == 0)
                    {
                        addRow("New URL", irbagency, url1, "documents",irbstudyId,irbagency);
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
                        addRow("New URL", irbagency, url2, "informed consent", irbstudyId, irbagency);
                    }
                }

            }
        }

        public static void addRow(string type, string source, string url, string section, string studyid, string agency)
        {
            initiate();
            DataRow dr = docs.NewRow();

            dr["TYPE"] = type;
            dr["IRB Agency name"] = agency;
            dr["IRB no"] = "";
            dr["IRB Study ID"] = studyid;
            dr["Study name"] = Tools.getStudyNumber(studyid, agency);
            dr["Version date"] = DateTime.Now.ToShortDateString();
            dr["Version number"] = source.ToUpper() + " " + section;
            dr["Category"] = "External Site Docs";
            dr["URL"] = url;
            dr["Version status"] = "Approved";

            docs.Rows.Add(dr);
        }
    }
}
