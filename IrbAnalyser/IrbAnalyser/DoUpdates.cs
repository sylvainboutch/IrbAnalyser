using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using System.Data.SqlClient;
//using Oracle.DataAccess.Client;
using System.Configuration;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using Oracle.ManagedDataAccess.Client;

namespace IrbAnalyser
{
    static class DoUpdates
    {
        //THESE TOO STRING SHOULD ALWAYS BE USED, KEEPS THE AUDIT TRAILS POPULATED.
        private static string lastModifiedBy = @" LAST_MODIFIED_BY = (select PK_USER from eres.er_user where USR_FIRSTNAME = 'IRB' AND USR_LASTNAME = 'Interface') ";
        private static string lastModifiedOn = @" LAST_MODIFIED_DATE = CURRENT_DATE ";

        //Dictionnary containing all updates (by table name) and then by unique key
        //private static Dictionary<string, Dictionary<string, string>> allUpdates = new Dictionary<string, Dictionary<string, string>>();


        //All function relating to status updates
        #region Status


        static Dictionary<string, string> statusUpdates = new Dictionary<string, string>();

        private static string statusUpdateP1 = @"update eres.er_studystat set STUDYSTAT_NOTE = ";
        private static string statusUpdateP2 = @" where pk_studystat = ";

        /// <summary>
        /// Generate all the script for all status updates
        /// </summary>
        private static void populateStatusUpdate()
        {
            //Dictionary<string, string> updates;

            /*if (allUpdates.ContainsKey("Status"))
            {
                updates = allUpdates["Status"];
            }
            else
            {
                updates = new Dictionary<string, string>();
                allUpdates.Add("Status", updates);
            }*/


            foreach (DataRow dr in OutputStatus.updatedStatus.Rows)
            {
                int pkstudystat = 0;
                Int32.TryParse((string)dr["pk_studystat"], out pkstudystat);
                if (!statusUpdates.ContainsKey((string)dr["pk_studystat"]))
                {
                    statusUpdates.Add((string)dr["pk_studystat"], generateStatusUpdate(pkstudystat, (string)dr["Comment"]));
                }
            }

            //allUpdates["Status"] = updates;
        }

        /// <summary>
        /// Generates the script to update one status row
        /// </summary>
        /// <param name="pk_studystat"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        private static string generateStatusUpdate(int pk_studystat, string notes)
        {
            if (pk_studystat == 0)
            {
                return "";
            }

            return statusUpdateP1 + "'" + notes + "'," + lastModifiedBy + "," + lastModifiedOn + statusUpdateP2 + pk_studystat + "";
        }


        #endregion

        //All function relating to study updates
        #region Study

        private static string updateStudyStart = @"update eres.er_study set ";

        private static string updateStudyTitle = @" STUDY_TITLE = ";
        private static string updateStudySummary = @" STUDY_SUM_CLOB = ";
        private static string updateStudyNumber = @" STUDY_NUMBER = ";
        private static string updateStudyAgentDevice = @" STUDY_NUMBER = ";
        private static string updateStudyKeywords = @" STUDY_KEYWRDS = ";
        private static string updateStudySponsorId = @" STUDY_SPONSORID = ";
        private static string updateStudySponsorContact = @" STUDY_CONTACT = ";
        private static string updateStudySponsorOther = @" STUDY_INFO = ";
        private static string updateStudySample = @" STUDY_NSAMPLSIZE = ";
        private static string updateStudyNCT = @" NCT_NUMBER = ";

        //STUDY_MAJ_AUTH
        //
        //
        //NCT_NUMBER
        //NCI_TRIAL_IDENTIFIER

        private static string updateStudyDivision1 = @" FK_CODELST_TAREA = (SELECT PK_CODELST FROM ERES.ER_CODELST WHERE CODELST_TYPE = 'tarea' AND CODELST_DESC = ";
        private static string updateStudyTarea1 = @" STUDY_DIVISION = (SELECT PK_CODELST FROM ERES.ER_CODELST WHERE CODELST_TYPE = 'study_division' AND CODELST_DESC = ";
        private static string updateStudyPhase1 = @" FK_CODELST_PHASE = (SELECT PK_CODELST FROM ERES.ER_CODELST WHERE CODELST_TYPE = 'phase' AND CODELST_DESC = ";
        private static string updateStudySponsors1 = @" FK_CODELST_SPONSOR = (SELECT PK_CODELST FROM ERES.ER_CODELST WHERE CODELST_TYPE = 'sponsor' AND CODELST_DESC = ";
        private static string updateStudyPI1 = @" STUDY_PRINV = (SELECT CAST(PK_USER as varchar2(1000 BYTE) FROM ERES.ER_USER WHERE USR_FIRSTNAME + ' ' + USR_LASTNAME = ";
        private static string updateStudyRC1 = @" FK_AUTHOR = (SELECT PK_USER FROM ERES.ER_USER WHERE USR_FIRSTNAME + ' ' + USR_LASTNAME = ";
        private static string updateStudySC1 = @" STUDY_COORDINATOR = (SELECT PK_USER FROM ERES.ER_USER WHERE USR_FIRSTNAME + ' ' + USR_LASTNAME = ";

        private static string closePart = @") ";

        private static string updateStudyEnd = @" where pk_study = ";

        private static string updateMSD = @"update eres.er_studyid set STUDYID_ID = :value WHERE FK_STUDY = :pkstudy and FK_CODELST_IDTYPE = :fkcodelst";

        private static Dictionary<string, int> msdDict = new Dictionary<string, int>()
        {
            {"Cancer",12065},
            {"IRB no",6423},
            {"IRB Agency name",12064},
            {"Device",12042},
            {"CT_FDA",25348},
            {"CT_ICMJE",25349},
            {"SpecimenDataAnalysis",25342},
            {"Agent",12036}
        };



        private static void updateStudys()
        {
            foreach (DataRow dr in OutputStudy.updatedStudy.Rows)
            {
                bool isUpdated = false;
                Dictionary<string, string> values = new Dictionary<string, string>();
                string updateStr = updateStudyStart;

                if (dr["Official title"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Official title"]))
                {
                    isUpdated = true;
                    values.Add("title", (string)dr["Official title"]);
                    updateStr += updateStudyTitle + " :title ";
                }
                if (dr["Study Summary"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Study Summary"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add("summary", (string)dr["Study Summary"]);
                    updateStr += updateStudySummary + " :summary ";
                }

                if (dr["KeyWords"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["KeyWords"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add("keywords", (string)dr["KeyWords"]);
                    updateStr += updateStudyKeywords + " :keywords ";
                }

                if (dr["Sponsor Protocol ID"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Sponsor Protocol ID"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add("sponsorid", (string)dr["Sponsor Protocol ID"]);
                    updateStr += updateStudySponsorId + " :sponsorid ";
                }

                if (dr["NCT_NUMBER"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["NCT_NUMBER"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add("nctno", (string)dr["NCT_NUMBER"]);
                    updateStr += updateStudyNCT + " :nctno ";
                }

                updateStr += ", " + lastModifiedBy + ", " + lastModifiedOn + " " + updateStudyEnd + (string)dr["pk_study"] + "";

                if (isUpdated) executeSQL(updateStr, values);

                foreach (KeyValuePair<string, int> keypair in msdDict)
                {
                    if (dr[keypair.Key] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr[keypair.Key]))
                    {
                        values.Clear();
                        values.Add("value", (string)dr[keypair.Key]);
                        values.Add("pkstudy", (string)dr["pk_study"]);
                        values.Add("fkcodelst", keypair.Value.ToString());
                        executeSQL(updateMSD, values);
                    }
                }
                

            }
        }


        #endregion

        /// <summary>
        /// Do it all!
        /// </summary>
        public static void complete()
        {
            populateStatusUpdate();
            executeAll();
        }

        /// <summary>
        /// Execute all commands stored in the allUpdates dictionnary
        /// </summary>
        private static void executeAll()
        {
            foreach (var x in statusUpdates.Values)
            {
                executeSQL(x);
            }

            updateStudys();
        }


        /// <summary>
        /// Execute a simple non-query SQL command
        /// </summary>
        /// <param name="sqlQuery"></param>
        private static void executeSQL(string sqlQuery, Dictionary<string, string> values = null)
        {
            //string constr = "User Id=SBOUCHAR;Password=createticket;Data Source=192.168.199.50:1521/velosprd";

            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["VelosSql"].ConnectionString))
            //using (OracleConnection connection = new OracleConnection(constr))
            {
                OracleCommand command = new OracleCommand(sqlQuery, connection);
                if (values != null && values.Count() > 0)
                {
                    foreach (KeyValuePair<string, string> keypair in values)
                    {
                        //command.Parameters.Add(keypair.Key, keypair.Value);
                        command.Parameters.Add(new OracleParameter(keypair.Key, keypair.Value));
                    }
                }

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
            }

        }



        public static void loadExcel(string excelFile, DataTable table, string sheet)
        {
            FileInfo file = new FileInfo(excelFile);
            if (!file.Exists) { throw new Exception("Error, file doesn't exists!"); }
            string extension = file.Extension;
            string strConn;
            switch (extension)
            {
                case ".xls":
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFile + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                    break;
                case ".xlsx":
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFile + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
                    break;
                default:
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFile + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                    break;
            }

            OleDbConnection cnnxls = new OleDbConnection(strConn);
            bool sheetExist = false;
            cnnxls.Open();
            var dbSchema = cnnxls.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            cnnxls.Close();
            if (dbSchema != null || dbSchema.Rows.Count > 0)
            {
                for (int i = 0; i < dbSchema.Rows.Count; i++)
                {
                    string sheetname = dbSchema.Rows[i]["TABLE_NAME"].ToString();
                    sheetExist = sheetExist || sheetname == sheet + "$";
                }
            }

            if (sheetExist)
            {

                string query = String.Format("select * from [{0}$]", sheet);
                OleDbDataAdapter oda = new OleDbDataAdapter(query, cnnxls);
                DataTable dt = new DataTable();



                oda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drnew = table.NewRow();
                    foreach (DataColumn column in dt.Columns)
                    {

                        drnew[column.ColumnName] = dr[column.ColumnName];
                    }
                    table.Rows.Add(drnew);
                }
            }

        }


    }


}
