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
        private static string updateStudyAgentDevice = @" STUDY_PRODNAME = ";
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
        private static string updateStudyScope1 = @" FK_CODELST_SCOPE = (SELECT PK_CODELST FROM ERES.ER_CODELST WHERE CODELST_TYPE = 'studyscope' AND CODELST_DESC = ";
        private static string updateStudyPI1 = @" STUDY_PRINV = (SELECT CAST(PK_USER as varchar2(1000 BYTE)) FROM ERES.ER_USER WHERE USR_FIRSTNAME || ' ' || USR_LASTNAME = ";
        private static string updateStudyRC1 = @" FK_AUTHOR = (SELECT PK_USER FROM ERES.ER_USER WHERE USR_FIRSTNAME || ' ' || USR_LASTNAME = ";
        private static string updateStudySC1 = @" STUDY_COORDINATOR = (SELECT PK_USER FROM ERES.ER_USER WHERE USR_FIRSTNAME || ' ' || USR_LASTNAME = ";

        private static string closePart = @") ";

        private static string updateStudyEnd = @" where pk_study = ";

        private static string updateMSD = @"update eres.er_studyid set STUDYID_ID = :value WHERE FK_STUDY = :pkstudy and FK_CODELST_IDTYPE = :fkcodelst";

        private static Dictionary<string, int> msdDict = new Dictionary<string, int>()
        {
            {"RecordCategory",12067},
            {"FinancialBy",12069},
            {"SignOffBy",24837},
            {"HasConsentForm",24242},
            {"Cancer",12065},
            {"Agent",12036},

            {"Biological",25336},
            {"BLood_Draw",25337},
            {"Data_Collection",25344},
            {"Device",12042},
            {"EMERGENCY_INVESTIGATIONAL",25338},
            {"HUMANITARIAN_USE",25339},
            {"QI_STUDY",25340},
            {"RETROSPECTIVE_CHART_REVIEW",12048},
            {"TISSUE_BANKING",24245},
            {"TRIALS_Involving_INTERVENTIONS",12051},
            {"Survey",25341},

            {"CT_FDA",25348},
            {"CT_ICMJE",25349},
            {"CT_NIH",25350},

            {"SpecimenDataAnalysis",25342},

            {"IRB no",6423},
            {"IRB Agency name",12064}
        };



        private static void updateStudys()
        {
            foreach (DataRow dr in OutputStudy.updatedStudy.Rows)
            {
                bool isUpdated = false;
                List<Tuple<string, object, OracleDbType>> values = new List<Tuple<string, object, OracleDbType>>();
                //Dictionary<string, string[]> values = new Dictionary<string, string[]>();
                string updateStr = updateStudyStart;

                if (OutputStudy.updatedStudy.Columns.Contains("Principal Investigator") && dr["Principal Investigator"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Principal Investigator"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("pi", (string)dr["Principal Investigator"], OracleDbType.Varchar2));
                    //values.Add("nctno", (string)dr["NCT_NUMBER"]);
                    updateStr += updateStudyPI1 + " :pi " + closePart;
                }

                if (OutputStudy.updatedStudy.Columns.Contains("Regulatory_coordinator") && dr["Regulatory_coordinator"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Regulatory_coordinator"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("rc", (string)dr["Regulatory_coordinator"], OracleDbType.Varchar2));
                    //values.Add("nctno", (string)dr["NCT_NUMBER"]);
                    updateStr += updateStudyRC1 + " :rc " + closePart;
                }


                if (OutputStudy.updatedStudy.Columns.Contains("Official title") && dr["Official title"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Official title"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("title", (string)dr["Official title"], OracleDbType.Varchar2));
                    //values.Add("title", (string)dr["Official title"], OracleDbType.Varchar2);
                    updateStr += updateStudyTitle + " :title ";
                }
                if (OutputStudy.updatedStudy.Columns.Contains("Study Summary") && dr["Study Summary"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Study Summary"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("summary", (string)dr["Study Summary"], OracleDbType.Clob));
                    //values.Add("summary", (string)dr["Study Summary"]);
                    updateStr += updateStudySummary + " :summary ";
                }

                if (OutputStudy.updatedStudy.Columns.Contains("KeyWords") && dr["KeyWords"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["KeyWords"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("keywords", (string)dr["KeyWords"], OracleDbType.Varchar2));
                    //values.Add("keywords", (string)dr["KeyWords"]);
                    updateStr += updateStudyKeywords + " :keywords ";
                }
                
                if (OutputStudy.updatedStudy.Columns.Contains("Study scope") && dr["Study scope"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Study scope"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("scope", (string)dr["Study scope"], OracleDbType.Varchar2));
                    //values.Add("nctno", (string)dr["NCT_NUMBER"]);
                    updateStr += updateStudyScope1 + " :scope " + closePart;
                }

                
                if (OutputStudy.updatedStudy.Columns.Contains("Primary funding sponsor") && dr["Primary funding sponsor"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Primary funding sponsor"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("sponsor", (string)dr["Primary funding sponsor"], OracleDbType.Varchar2));
                    //values.Add("sponsorid", (string)dr["Sponsor Protocol ID"]);
                    updateStr += updateStudySponsors1 + " :sponsor " + closePart;
                }

                if (OutputStudy.updatedStudy.Columns.Contains("Sponsor information other") && dr["Sponsor information other"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Sponsor information other"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("sponsorother", (string)dr["Sponsor information other"], OracleDbType.Varchar2));
                    //values.Add("sponsorid", (string)dr["Sponsor Protocol ID"]);
                    updateStr += updateStudySponsorOther + " :sponsorother ";
                }

                if (OutputStudy.updatedStudy.Columns.Contains("Sponsor contact") && dr["Sponsor contact"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Sponsor contact"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("sponsorcontact", (string)dr["Sponsor contact"], OracleDbType.Varchar2));
                    //values.Add("sponsorid", (string)dr["Sponsor Protocol ID"]);
                    updateStr += updateStudySponsorContact + " :sponsorcontact ";
                }


                if (OutputStudy.updatedStudy.Columns.Contains("Sponsor Protocol ID") && dr["Sponsor Protocol ID"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Sponsor Protocol ID"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("sponsorid", (string)dr["Sponsor Protocol ID"], OracleDbType.Varchar2));
                    //values.Add("sponsorid", (string)dr["Sponsor Protocol ID"]);
                    updateStr += updateStudySponsorId + " :sponsorid ";
                }

                if (OutputStudy.updatedStudy.Columns.Contains("NCT_NUMBER") && dr["NCT_NUMBER"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["NCT_NUMBER"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("nctno", (string)dr["NCT_NUMBER"], OracleDbType.Varchar2));
                    //values.Add("nctno", (string)dr["NCT_NUMBER"]);
                    updateStr += updateStudyNCT + " :nctno ";
                }

                if (OutputStudy.updatedStudy.Columns.Contains("Phase") && dr["Phase"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Phase"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("phase", (string)dr["Phase"], OracleDbType.Varchar2));
                    //values.Add("nctno", (string)dr["NCT_NUMBER"]);
                    updateStr += updateStudyPhase1 + " :phase " + closePart;
                }
                if (OutputStudy.updatedStudy.Columns.Contains("Entire study sample size") && dr["Entire study sample size"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["Entire study sample size"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("sample", (string)dr["Entire study sample size"], OracleDbType.Varchar2));
                    //values.Add("nctno", (string)dr["NCT_NUMBER"]);
                    updateStr += updateStudySample + " :sample ";
                }
                if (OutputStudy.updatedStudy.Columns.Contains("AgentDevice") && dr["AgentDevice"] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr["AgentDevice"]))
                {
                    if (isUpdated) updateStr += ", ";
                    isUpdated = true;
                    values.Add(new Tuple<string, object, OracleDbType>("agent", (string)dr["AgentDevice"], OracleDbType.Varchar2));
                    //values.Add("nctno", (string)dr["NCT_NUMBER"]);
                    updateStr += updateStudyAgentDevice + " :agent ";
                }
                

                
                

                updateStr += ", " + lastModifiedBy + ", " + lastModifiedOn + " " + updateStudyEnd + (string)dr["pk_study"] + "";

                if (isUpdated) executeSQL(updateStr, values);

                foreach (KeyValuePair<string, int> keypair in msdDict)
                {
                    if (OutputStudy.updatedStudy.Columns.Contains(keypair.Key) && dr[keypair.Key] != DBNull.Value && !String.IsNullOrWhiteSpace((string)dr[keypair.Key]))
                    {
                        values.Clear();
                        values.Add(new Tuple<string, object, OracleDbType>("value", (string)dr[keypair.Key], OracleDbType.Varchar2));
                        values.Add(new Tuple<string, object, OracleDbType>("pkstudy", (string)dr["pk_study"], OracleDbType.Varchar2));
                        values.Add(new Tuple<string, object, OracleDbType>("fkcodelst", keypair.Value.ToString(), OracleDbType.Varchar2));
                        executeSQL(updateMSD, values);
                    }
                }
                

            }
        }


        #endregion

        //Setting the current status
        #region CurrentStatus

        private static string setCurrentString = @"DECLARE counter INTEGER; BEGIN counter := 0; for stat IN (select setNotCurrent, setCurrent, pk_study from VDA.LCL_V_SETCURRENTSTATUS) LOOP counter := counter + 1; update ERES.ER_STUDYSTAT set CURRENT_STAT = 0, LAST_MODIFIED_BY = (select PK_USER from eres.er_user where USR_FIRSTNAME = 'IRB' AND USR_LASTNAME = 'Interface'), LAST_MODIFIED_DATE = CURRENT_DATE where PK_STUDYSTAT = stat.setNotCurrent;    update ERES.ER_STUDYSTAT set CURRENT_STAT = 1, LAST_MODIFIED_BY = (select PK_USER from eres.er_user where USR_FIRSTNAME = 'IRB' AND USR_LASTNAME = 'Interface'), LAST_MODIFIED_DATE = CURRENT_DATE where PK_STUDYSTAT = stat.setCurrent; END LOOP; END;";
        private static string setDraft = @"update eres.er_studystat set STUDYSTAT_DATE = '01-SEP-00',  LAST_MODIFIED_BY = (select PK_USER from eres.er_user where USR_FIRSTNAME = 'IRB' AND USR_LASTNAME = 'Interface'), LAST_MODIFIED_DATE = CURRENT_DATE where pk_studystat in (select st1.pk_studystat from eres.er_studystat st1 left join eres.er_study su on su.PK_STUDY = st1.fk_study left join eres.er_codelst cd1 on cd1.PK_CODELST = st1.FK_CODELST_STUDYSTAT where st1.FK_CODELST_STUDYSTAT = 4836 and st1.studystat_note is null and STUDYSTAT_DATE != '01-SEP-00' and su.creator = 135 or su.creator = (select PK_USER from eres.er_user where USR_FIRSTNAME = 'IRB' AND USR_LASTNAME = 'Interface'))";

        private static void fixStatuses()
        {
            executeSQL(setDraft);
            executeSQL(setCurrentString);
        }

        #endregion

        //Updatign version links when necessary
        #region version
        private static string updateLinks1 = @" update ERES.ER_STUDYAPNDX set STUDYAPNDX_URI = ";
        private static string updateLinks2 = @" where PK_STUDYAPNDX = ";

        private static void doLinkUpdate()
        {
            string updateLinks = updateLinks1 + " :uri" + updateLinks2 + ":pkapndx";
            foreach (DataRow dr in OutputDocs.updatedDocs.Rows)
            {
                if (!String.IsNullOrEmpty((string)dr["PK_STUDYAPNDX"]) && ((string)dr["PK_STUDYAPNDX"]).Trim() != "0")
                {
                    List<Tuple<string, object, OracleDbType>> values = new List<Tuple<string, object, OracleDbType>>();
                    values.Add(new Tuple<string, object, OracleDbType>("pkapndx", (string)dr["PK_STUDYAPNDX"], OracleDbType.Decimal));
                    values.Add(new Tuple<string, object, OracleDbType>("uri", (string)dr["URL"], OracleDbType.Varchar2));
                    executeSQL(updateLinks, values);
                }
            }
        }


        #endregion

        //Updating the VELOS_REPORT.LCL_IRB_MISSING_USERS table
        #region Missing_users
        private static string insertIRB = @"INSERT INTO VELOS_REPORT.LCL_IRB_MISSING_USERS (FK_STUDYTEAM, DATE_ADDED, DATE_NEWEST) VALUES (:fkstudyteam1, :date1, :date1) where not exists (select FK_STUDYTEAM from VELOS_REPORT.LCL_IRB_MISSING_USERS where FK_STUDYTEAM = :fkstudyteam1)";
        private static string updateIRB = @"UPDATE VELOS_REPORT.LCL_IRB_MISSING_USERS set DATE_NEWEST = :date1) where FK_STUDYTEAM = :fkstudyteam1";

        private static string updateInsertIRB = @"MERGE INTO VELOS_REPORT.LCL_IRB_MISSING_USERS ta USING DUAL ON (ta.FK_STUDYTEAM = :fkstudyteam1) WHEN MATCHED THEN UPDATE set DATE_NEWEST = :date1 WHEN NOT MATCHED THEN INSERT(FK_STUDYTEAM, DATE_ADDED, DATE_NEWEST) VALUES (:fkstudyteam1, :date1, :date1)";

        private static void populateMissingUsers()
        {
            foreach (DataRow dr in OutputTeam.triggerTeam.Rows)
            {
                int pk = 0;
                int.TryParse((string)dr["PK_STUDYTEAM"],out pk);
                if (pk != 0)
                {
                    List<Tuple<string, object, OracleDbType>> values = new List<Tuple<string, object, OracleDbType>>();
                    values.Add(new Tuple<string, object, OracleDbType>("fkstudyteam1", pk, OracleDbType.Int32));
                    values.Add(new Tuple<string, object, OracleDbType>("date1", DateTime.Now.Date, OracleDbType.Date));
                    executeSQL(updateInsertIRB, values);
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
            fixStatuses();
        }

        /// <summary>
        /// Execute all commands stored in the allUpdates dictionnary
        /// </summary>
        private static void executeAll()
        {
            /*foreach (var x in statusUpdates.Values)
            {
                executeSQL(x);
            }

            updateStudys();*/

            populateMissingUsers();
        }


        /// <summary>
        /// Execute a simple non-query SQL command
        /// </summary>
        /// <param name="sqlQuery"></param>
        private static void executeSQL(string sqlQuery, List<Tuple<string, object, OracleDbType>> values = null)//Dictionary<string, string> values = null)
        {
            //string constr = "User Id=SBOUCHAR;Password=createticket;Data Source=192.168.199.50:1521/velosprd";

            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["VelosSql"].ConnectionString))
            //using (OracleConnection connection = new OracleConnection(constr))
            {
                OracleCommand command = new OracleCommand(sqlQuery, connection);
                if (values != null && values.Count() > 0)
                {
                    //foreach (KeyValuePair<string, string> keypair in values)
                    foreach(Tuple<string, object, OracleDbType> value in values)
                    {
                        //command.Parameters.Add(keypair.Key, keypair.Value);
                        //command.Parameters.Add(new OracleParameter(keypair.Key, keypair.Value));
                        OracleParameter param = new OracleParameter(value.Item1, value.Item3, ParameterDirection.InputOutput);
                        param.Value = value.Item2;
                        command.Parameters.Add(param);
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
