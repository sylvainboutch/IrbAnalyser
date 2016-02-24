using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace IrbAnalyser
{
    static class DoUpdates
    {
        //THESE TOO STRING SHOULD ALWAYS BE USED, KEEPS THE AUDIT TRAILS POPULATED.
        private static string lastModifiedBy = @" LAST_MODIFIED_BY = (select PK_USER from eres.er_user where USR_FIRSTNAME = 'IRB' AND USR_LASTNAME = 'Interface') ";
        private static string lastModifiedOn = @" LAST_MODIFIED_DATE = CURRENT_DATE ";

        //Dictionnary containing all updates (by table name) and then by unique key
        private static Dictionary<string, Dictionary<string, string>> allUpdates = new Dictionary<string, Dictionary<string, string>>();


        private static string statusUpdateP1 = @"update eres.er_studystat set SSTAT_NOTES = ";
        private static string statusUpdateP2 = @" where pk_studystat = ";

        /// <summary>
        /// Generate all the script for all status updates
        /// </summary>
        private static void populateStatusUpdate()
        {
            Dictionary<string, string> updates;

            if (allUpdates.ContainsKey("Status"))
            {
                updates = allUpdates["Status"];
            }
            else
            {
                updates = new Dictionary<string, string>();
                allUpdates.Add("Status", updates);
            }


            foreach (DataRow dr in OutputStudy.updatedStudy.Rows)
            {
                int pkstudystat = 0;
                Int32.TryParse((string)dr["pk_studystat"], out pkstudystat);
                if (!updates.ContainsKey((string)dr["pk_studystat"]))
                {
                    updates.Add((string)dr["pk_studystat"], generateStatusUpdate(pkstudystat, (string)dr["Comment"]));
                }
            }

            allUpdates["Status"] = updates;
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

            return statusUpdateP1 + "'" + notes + "'," + lastModifiedBy + "," + lastModifiedOn + statusUpdateP2 + pk_studystat + ";";
        }

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
            foreach (var x in allUpdates.Values)
            {
                foreach (var y in x.Values)
                {
                    executeSQL(y);
                }
            }
        }


        /// <summary>
        /// Execute a simple non-query SQL command
        /// </summary>
        /// <param name="sqlQuery"></param>
        private static void executeSQL(string sqlQuery)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["VelosDb"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

        }
    }
}
