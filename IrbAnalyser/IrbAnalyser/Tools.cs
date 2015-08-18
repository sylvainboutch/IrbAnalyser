using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;

namespace IrbAnalyser
{
    /// <summary>
    /// Class containing various tools used by the application
    /// </summary>
    static class Tools
    {
        //Saves the local temporary path to extracted files
        public static string filename = "";

        /// <summary>
        /// Get full name from the first name and last name of a datarow
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string getFullName(DataRow dr)
        {
            return dr == null ? "" : dr["FirstName"] + " " + dr["LastName"];
        }

        /// <summary>
        /// Generate the study identifier string to be stored in the IRB Form
        /// </summary>
        /// <param name="studyID"></param>
        /// <returns></returns>
        public static string generateStudyIdentifiers(string studyID)
        {
            string result = studyID;
            var stud = (from st in OutputStudy.fpstudys.data.AsEnumerable()
                        where st.Field<string>("StudyId").Trim().ToLower() == studyID.Trim().ToLower()
                        select (st.Field<string>("SiteName").Replace("(IBC)", "") + ":" + st.Field<string>("StudySiteId"))).ToArray();
            foreach (var stu in stud)
            {
                if (!stu.Contains("(IBC)"))
                    result += ">" + stu;
            }
            return result;
        }

        /// <summary>
        /// Gets the studyId from the study identifiers IRB Form field
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static string getStudyIdentifiers(string studyId)
        {
            var strplit = studyId.Split(new string[] { ">" }, StringSplitOptions.None);
            return strplit[0];
        }

        /// <summary>
        /// Parse a date using DateTime.TryParse (no format or local) and returns a string of format "MM/dd/yyyy"
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string parseDate(string date)
        {
            DateTime dateparsed = DateTime.MinValue;
            DateTime.TryParse(date,out dateparsed);
            return dateparsed == DateTime.MinValue ? "" : dateparsed.Date.ToString("MM/dd/yyyy");
        }


        /// <summary>
        /// Gets the study number for that study, looks for the study in the database, in previously added new study.
        /// If no study is found, creates the study number.
        /// The acronym is read from the datasource
        /// </summary>
        /// <param name="IRBstudyId"></param>
        /// <param name="IRBnumber"></param>
        /// <returns></returns>
        public static string getStudyNumber(string IRBstudyId, string IRBnumber)
        {
            var stud = (from st in OutputStudy.fpstudys.data.AsEnumerable()
                        where st.Field<string>("StudyId").Trim().ToLower() == IRBstudyId.Trim().ToLower()
                        select st).ToArray();

            string accronym = stud.Count() > 0 ? stud[0].Field<string>("StudyAcronym") : "";
            string title = stud.Count() > 0 ? stud[0].Field<string>("StudyTitle") : "";

            return getStudyNumber(IRBstudyId, IRBnumber, accronym, title);
        }

        /// <summary>
        /// Gets the study number for that study, looks for the study in the database, in previously added new study.
        /// If no study is found, creates the study number.
        /// The acronym is provided
        /// </summary>
        /// <param name="IRBstudyId"></param>
        /// <param name="IRBnumber"></param>
        /// <param name="accronym"></param>
        /// <returns></returns>
        public static string getStudyNumber(string IRBstudyId, string IRBnumber, string accronym, string title)
        {
            string number = "";

            number = (from stud in OutputStudy.studys
                      where stud.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (IRBstudyId.Trim().ToLower())
                   && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                      select stud.STUDY_NUMBER).FirstOrDefault();

            if (number == null || number.Trim() == "")
            {
                string pattern = @"^(19|20)\d{2}";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                IRBnumber = rgx.IsMatch(IRBnumber) ? IRBnumber.Substring(2) : IRBnumber;
                accronym = string.IsNullOrWhiteSpace(accronym) ? cleanTitle(title) : cleanTitle(accronym);
                number = generateStudyNumber(IRBnumber, accronym);
            }
            return number;
        }

        /// <summary>
        /// Generate the study number
        /// </summary>
        /// <param name="irbnumber"></param>
        /// <param name="accronym"></param>
        /// <returns></returns>
        private static string generateStudyNumber(string irbnumber, string accronym)
        {
            string output = irbnumber.Replace("-", "");
            output += "-" + accronym.Substring(0, accronym.Length > 20 ? 20:accronym.Length);
            return output.Trim();
        }

        /// <summary>
        /// Check if a study exist in velos for that study id
        /// </summary>
        /// <param name="IRBstudyId"></param>
        /// <returns></returns>
        public static bool getOldStudy(string IRBstudyId)
        {
            bool ret;

            ret = OutputStudy.studys.Any(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (IRBstudyId.Trim().ToLower())
                && x.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr);

            return ret;
        }

        /// <summary>
        /// Clean a string of many special characters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string cleanMap(string input)
        {
            input = input.Trim();       
            string output = "";
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ' || c == '/' || c == '(' || c == ')' || c == '-'
                    || c == '.' || c == '?' || c == '!' || c == '@' || c == ',')
                {
                    sb.Append(c);
                }
            }
            output = sb.ToString();
            output = output.Replace("  ", " ");
            return output;
        }

        /// <summary>
        /// Remove all HTML tag and multiple spaces from a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string removeHtml(string input)
        {
            input = Regex.Replace(input, @"<[^>]+>|&nbsp;", "").Trim();
            input = Regex.Replace(input, @"\s{2,}", " ");
            return input;
        }

        /// <summary>
        /// Remove all non alphanumeric character from a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string cleanTitle(string input)
        {
            input = input.Trim();
            string output = "";
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ')
                {
                    sb.Append(c);
                }
            }
            output = sb.ToString();
            output = output.Replace("  ", " ");
            return output;
        }

        
        /// <summary>
        /// Remove all non alphanumeric character from a string except . and _
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string cleanStr(string input)
        {
            input = input.ToLowerInvariant();
            input = input.Trim();
            input = input.Replace(" ", "");
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// Trims double quotes from a string and change the string "NULL" to an empty string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string parse(string input)
        {
            input = input.Trim('"');
            input = input == "NULL" ? "" : input;

            return input;
        }

        /// <summary>
        /// Trims double quotes from a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string removeQuote(string input)
        {
            return input.Trim('"');
        }

        /// <summary>
        /// Trims quote for all element of a string array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] removeQuote(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = removeQuote(input[i]);
            }
            return input;
        }


        /// <summary>
        /// Return true if the string a equals
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool compareStr(object str1, object str2)
        {
            if (str1 == null)
            {
                if (str2 == null) return true;
                else if (String.IsNullOrEmpty(str2.ToString())) return true;
                else return false;
            }
            else if (str2 == null)
            {
                if (str1 == null) return true;
                else if (String.IsNullOrEmpty(str1.ToString())) return true;
                else return false;
            }
            else
            {
                return cleanStr(str1.ToString()) == cleanStr(str2.ToString());
            }
        }

        /// <summary>
        /// Verify if str1 contains any string from the array str2
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool containStr(object str1, string[] str2)
        {
            bool contain = false;
            if (str1 == null)
            {
                if (str2.Count() == 0) return true;
                else if (str2.All(x => String.IsNullOrEmpty(x))) return true;
                else return false;
            }
            else if (String.IsNullOrEmpty(str1.ToString()))
            {
                if (str2.Count() == 0) return true;
                else if (str2.All(x => String.IsNullOrEmpty(x))) return true;
                else return false;
            }

            foreach (var str in str2)
            {
                contain = cleanStr(str1.ToString()).Contains(cleanStr(str)) ? true : contain;
            }
            return contain;
        }

        /// <summary>
        /// Delete duplicate row based on single column from a data table
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static DataTable removeDuplicate(DataTable Table, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in Table.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                Table.Rows.Remove(dRow);

            return Table;
        }

        /// <summary>
        /// Delete duplicate row based on all columns
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static DataTable removeDuplicate(DataTable Table)
        {

            //Returns just 5 unique rows
            //var UniqueRows = Table.AsEnumerable().Distinct(DataRowComparer.Default);
            //DataTable dt2 = UniqueRows.CopyToDataTable();
            //return dt2;
            DataTable tempTable = Table.Clone();

            IEnumerable<DataRow> unique = Table.AsEnumerable().Distinct(DataRowComparer.Default);
            foreach (DataRow dr in unique)
            {
                tempTable.Rows.Add(dr.ItemArray);
            }
            Table.Clear();
            Table = tempTable.Copy();
            return Table;
        }

    }
}
