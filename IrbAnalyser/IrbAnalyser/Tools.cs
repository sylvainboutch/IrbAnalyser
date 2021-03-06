﻿using System;
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
            if (dr == null) return "";

            string email = ((string)dr["PrimaryEMailAddress"]).ToLower().Trim();

            string[] split = ((string)dr["FirstName"]).Split(' ');
            string firstnameLonguest = split[0];
            foreach (string part in split)
            {
                firstnameLonguest = firstnameLonguest.Length > part.Length ? firstnameLonguest : part;
            }

            split = ((string)dr["LastName"]).Split(' ');
            string lastnameLonguest = split[0];
            foreach (string part in split)
            {
                lastnameLonguest = lastnameLonguest.Length > part.Length ? lastnameLonguest : part;
            }

            var user = (from us in OutputTeam.accounts
                        where ((!string.IsNullOrWhiteSpace(us.USER_EMAIL) && us.USER_EMAIL.ToLower() == email)
                        || (us.USER_NAME.ToLower().Contains(firstnameLonguest.ToLower().Trim())
                        & us.USER_NAME.ToLower().Contains(lastnameLonguest.ToLower().Trim())
                        ))
                        select us);

            if (user == null || user.Count() == 0)
            {
                return dr["FirstName"] + " " + dr["LastName"];
            }

            return user.First().USER_NAME;

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
        public static string getStudyIdentifiers(string IrbIdentifier)
        {
            var strplit = IrbIdentifier.Split(new string[] { ">" }, StringSplitOptions.None);
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
            DateTime.TryParse(date, out dateparsed);
            return dateparsed == DateTime.MinValue ? "" : dateparsed.Date.ToString("MM/dd/yyyy");
        }

        /// <summary>
        /// Parse a date using DateTime.TryParse (no format or local) and the date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime parseDateDate(string date)
        {
            DateTime dateparsed = DateTime.MinValue;
            DateTime.TryParse(date, out dateparsed);
            dateparsed.AddHours(-dateparsed.Hour);
            dateparsed.AddMinutes(-dateparsed.Minute);
            dateparsed.AddSeconds(-dateparsed.Second);
            dateparsed.AddMilliseconds(-dateparsed.Millisecond);
            return dateparsed;
        }


        /// <summary>
        /// If the study contains one of these status then we shouldnt update the IRB
        /// </summary>
        public static string[] nochangingstatus = new String[]{
            //"IRB Initial Approved"
            //"Blabla"
            //"Active/Enrolling",
            "IRB Initial Approved"
        };

        /// <summary>
        /// Check if study has the specified status in velos
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        static public bool doesStudyHaveStatus(string[] status, string studyId)
        {
            bool hasstatus = (from stat in OutputStatus.allstatus
                              where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower())
                              && status.Contains(stat.SSTAT_STUDY_STATUS.Trim().ToLower(), StringComparer.OrdinalIgnoreCase)
                              select stat).Any();

            return hasstatus;
        }


        /// <summary>
        /// Check if the studyNumber needs to change
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        static public bool shouldStudyNumberChange(string studyId, string IRBnumber, string accronym, string title, string sponsorId, string numberDB)
        {
            if (doesStudyHaveStatus(nochangingstatus, studyId))
            {
                return false;
            }

            //string numberDB = getDBStudyNumber(studyId);

            /*string usetitle = accronym;
            if (string.IsNullOrWhiteSpace(usetitle))
            {
                usetitle = string.IsNullOrWhiteSpace(sponsorId) ? cleanTitle(title) : cleanTitle(sponsorId);
            }*/


            string usetitle = numberDB.Substring(numberDB.IndexOf("-"));


            string pattern = @"^(19|20)\d{2}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            string irbnumber = rgx.IsMatch(IRBnumber) ? IRBnumber.Substring(2) : IRBnumber;
            irbnumber = irbnumber.Replace("-", "").ToLower();


            string numberActual = "";
            if (!numberDB.ToLower().Contains(irbnumber))
            {
                numberActual = generateStudyNumber(IRBnumber, usetitle);
            }
            /*if (!numberDB.ToLower().Contains(irbnumber) && !string.IsNullOrWhiteSpace(accronym) && accronym.Length < 20)
            {
                numberActual = generateStudyNumber(IRBnumber, usetitle);
            }
            else if (!numberDB.ToLower().Contains(irbnumber) && (string.IsNullOrWhiteSpace(accronym) || accronym.Length >= 20))
            {
                numberActual = generateStudyNumber(IRBnumber, numberDB.Substring(numberDB.IndexOf("-")));
            }
            else if (!string.IsNullOrWhiteSpace(accronym) && accronym.Length < 20)
            {
                numberActual = generateStudyNumber(IRBnumber, usetitle);
            }*/

            return numberActual == numberDB && !string.IsNullOrWhiteSpace(numberActual);
        }


        /// <summary>
        /// Get the study number from the Velos DB
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static string getDBStudyNumber(string studyId)
        {
            string retour = (from stud in OutputStudy.studys
                             where stud.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower())
                             select stud.STUDY_NUMBER).FirstOrDefault();
            return retour;
        }

        private static Dictionary<string, string> studyNumberCache;

        /// <summary>
        /// Gets the study number for that study, looks for the study in the database, in previously added new study.
        /// If no study is found, creates the study number.
        /// The acronym is read from the datasource
        /// </summary>
        /// <param name="IRBstudyId"></param>
        /// <param name="IRBnumber"></param>
        /// <returns></returns>
        public static string getOldStudyNumber(string IRBstudyId)
        {
            if (studyNumberCache.ContainsKey(IRBstudyId))
            {
                return studyNumberCache[IRBstudyId];
            }
            else
            {
                var stud = (from st in OutputStudy.fpstudys.data.AsEnumerable()
                            where st.Field<string>("StudyId").Trim().ToLower() == IRBstudyId.Trim().ToLower()
                            select st).ToArray();

                string accronym = stud.Count() > 0 ? stud[0].Field<string>("StudyAcronym") : "";

                string IRBnumber = "";
                if (!string.IsNullOrWhiteSpace((stud[0].Field<string>("ExternalIRB"))))
                {
                    IRBnumber = stud[0].Field<string>("ExternalIRBnumber");
                }
                else
                {
                    IRBnumber = (stud[0].Field<string>("IRBNumber")).Replace("(IBC)", "");
                }


                string title = stud.Count() > 0 ? stud[0].Field<string>("StudyTitle") : "";
                string sponsorId = stud.Count() > 0 ? stud[0].Field<string>("PrimarySponsorStudyId") : "";

                string oldNumber = getDBStudyNumber(IRBstudyId);
                if (!string.IsNullOrWhiteSpace(oldNumber))
                {
                    studyNumberCache.Add(IRBstudyId, oldNumber);
                    return oldNumber;
                }
                else
                {
                    oldNumber = getNewStudyNumber(IRBstudyId, IRBnumber, accronym, title, sponsorId);
                    studyNumberCache.Add(IRBstudyId, oldNumber);
                    return oldNumber;
                }
            }
        }


        /// <summary>
        /// Gets the study number for that study, looks for the study in the database, in previously added new study.
        /// If no study is found, creates the study number.
        /// The acronym is read from the datasource
        /// </summary>
        /// <param name="IRBstudyId"></param>
        /// <param name="IRBnumber"></param>
        /// <returns></returns>
        public static string getNewStudyNumber(string IRBstudyId, string IRBnumber)
        {
            var stud = (from st in OutputStudy.fpstudys.data.AsEnumerable()
                        where st.Field<string>("StudyId").Trim().ToLower() == IRBstudyId.Trim().ToLower()
                        select st).ToArray();

            string accronym = stud.Count() > 0 ? stud[0].Field<string>("StudyAcronym") : "";
            string title = stud.Count() > 0 ? stud[0].Field<string>("StudyTitle") : "";
            string sponsorId = stud.Count() > 0 ? stud[0].Field<string>("PrimarySponsorStudyId") : "";

            return getNewStudyNumber(IRBstudyId, IRBnumber, accronym, title, sponsorId);
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
        public static string getNewStudyNumber(string IRBstudyId, string IRBnumber, string accronym, string title, string sponsorId)
        {
            string number = getDBStudyNumber(IRBstudyId);



            //string usetitle = accronym;
            string usetitle = string.IsNullOrWhiteSpace(number) ? "" : number.Substring(number.IndexOf("-"));

            usetitle = string.IsNullOrWhiteSpace(usetitle) ? accronym : usetitle;

            if (string.IsNullOrWhiteSpace(usetitle))
            {
                usetitle = string.IsNullOrWhiteSpace(sponsorId) ? cleanTitle(title) : cleanTitle(sponsorId);
            }

            if (number == null || number.Trim() == "")
            {
                return generateStudyNumber(IRBnumber, usetitle);
            }

            if (doesStudyHaveStatus(nochangingstatus, IRBstudyId))
            {
                return number;
            }

            string pattern = @"^(19|20)\d{2}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            string irbnumber = rgx.IsMatch(IRBnumber) ? IRBnumber.Substring(2) : IRBnumber;
            irbnumber = irbnumber.Replace("-", "").ToLower();

            if (!number.ToLower().Contains(irbnumber))
            {
                return generateStudyNumber(IRBnumber, usetitle);
            }

            /*if (!string.IsNullOrWhiteSpace(accronym) && accronym.Length <= 20)
            {
                return generateStudyNumber(IRBnumber, usetitle);
            }

            if (!number.ToLower().Contains(irbnumber) && !string.IsNullOrWhiteSpace(accronym) && accronym.Length <= 20)
            {
                return generateStudyNumber(IRBnumber, usetitle);
            }
            else if (!number.ToLower().Contains(irbnumber) && (string.IsNullOrWhiteSpace(accronym) || accronym.Length > 20))
            {
                return generateStudyNumber(IRBnumber, number.Substring(number.IndexOf("-")));
            }
            else if (!string.IsNullOrWhiteSpace(accronym) && accronym.Length <= 20)
            {
                return generateStudyNumber(IRBnumber, usetitle);
            }*/

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
            string pattern = @"^(19|20)\d{2}";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            irbnumber = rgx.IsMatch(irbnumber) ? irbnumber.Substring(2) : irbnumber;
            string output = irbnumber.Replace("-", "");
            accronym = cleanTitle(accronym);
            accronym = accronym.Replace(" ", "");
            output += "-" + accronym.Substring(0, accronym.Length > 20 ? 20 : accronym.Length);
            return output.Trim();
        }


        private static Dictionary<string, bool> isApprovedCache;

        /// <summary>
        /// Check in the database if the study exist and in the file to see if a study is approved, it will return true if the study has an approval with the date part greater then the input date date part.
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public static bool isStudyApproved(string studyId, DateTime date)
        {
            if (isApprovedCache.ContainsKey(studyId))
                return isApprovedCache[studyId];
            else
            {

                bool isStudyApprovedVelos = (from stat in OutputStatus.allstatus
                                             where stat.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (studyId.Trim().ToLower())
                                             && (stat.SSTAT_STUDY_STATUS == OutputStatus.approvedStatus || stat.SSTAT_STUDY_STATUS == OutputStatus.renewalStatus)
                                             && (stat.SSTAT_VALID_FROM.HasValue && stat.SSTAT_VALID_FROM.Value.Date <= date.Date)
                                             select stat).Any();

                if (isStudyApprovedVelos)
                {

                    isApprovedCache.Add(studyId, true);
                    return true;
                }

                var studyIRBs = (from st in OutputStudy.fpstudys.data.AsEnumerable()
                                 where st.Field<string>("StudyId").Trim().ToLower() == studyId.Trim().ToLower()
                                 && (!string.IsNullOrWhiteSpace(st.Field<string>("InitialApprovalDate").Trim().ToLower())
                                 || !string.IsNullOrWhiteSpace(st.Field<string>("MostRecentApprovalDate").Trim().ToLower())
                                 )
                                 select st);

                bool isApprovedBool = false;
                foreach (var study in studyIRBs)
                {
                    DateTime approvDate = DateTime.MaxValue;
                    if (!String.IsNullOrWhiteSpace(study.Field<string>("InitialApprovalDate")))
                    {
                        approvDate = DateTime.ParseExact((study.Field<string>("InitialApprovalDate")).Trim(), new string[] { "s", "u", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal);
                    }
                    if (approvDate == DateTime.MaxValue && !String.IsNullOrWhiteSpace(study.Field<string>("MostRecentApprovalDate")))
                    {
                        approvDate = DateTime.ParseExact((study.Field<string>("MostRecentApprovalDate")).Trim(), new string[] { "s", "u", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal);
                    }
                    if (approvDate == DateTime.MaxValue || approvDate.Date > date.Date && !isApprovedBool) isApprovedBool = false;
                    else if (approvDate != DateTime.MaxValue || approvDate.Date <= date.Date) isApprovedBool = true;

                }

                isApprovedCache.Add(studyId, isApprovedBool);
                return isApprovedBool;
            }
        }



        private static Dictionary<string, bool> oldStudyCache;

        /// <summary>
        /// Check if a study exist in velos for that study id
        /// </summary>
        /// <param name="IRBstudyId"></param>
        /// <returns></returns>
        public static bool getOldStudy(string IRBstudyId)
        {
            if (oldStudyCache.ContainsKey(IRBstudyId))
            {
                return oldStudyCache[IRBstudyId];
            }
            else
            {
                bool oldStudy = OutputStudy.studys.Any(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (IRBstudyId.Trim().ToLower()));
                oldStudyCache.Add(IRBstudyId, oldStudy);
                return oldStudy;
            }
            /*bool ret; 
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                ret = OutputStudy.studys.Any(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (IRBstudyId.Trim().ToLower())
                    && x.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr);
            }
            else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                ret = OutputStudy.studys.Any(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (IRBstudyId.Trim().ToLower())
                    && x.MORE_IRBAGENCY.ToLower() != Agency.brany);
            }
            else
            {
                ret = OutputStudy.studys.Any(x => x.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (IRBstudyId.Trim().ToLower()));
            }
            return ret;*/
        }


        /// <summary>
        /// Gets the RC from Velos for that study, looks for the study in the database, 
        /// </summary>
        /// <param name="IRBstudyId"></param>
        /// <param name="IRBnumber"></param>
        /// <param name="accronym"></param>
        /// <returns></returns>
        public static string getRC(decimal? fkstudy)
        {
            string rc = "";


            rc = (from stud in OutputStudy.studys
                  where stud.PK_STUDY == fkstudy
                  select stud.STUDY_ENTERED_BY).FirstOrDefault();

            return rc;
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
            input = Regex.Replace(input, @"<[^>]+|&nbsp;", "").Trim();
            input = Regex.Replace(input, "\"", "").Trim();
            input = Regex.Replace(input, @"\s{2,}", " ");
            input = cleanMap(input);
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
            input = input.Replace('}', ')');
            input = input.Replace('{', '(');
            input = input.ToLower().Trim() == "null" ? "" : input;
            input = input.Length >= 1990 ? input.Substring(0, 1990) : input;
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
        /// Fix the format of the NCT number
        /// </summary>
        /// <param name="nct"></param>
        /// <returns></returns>
        public static string fixNCT(string nct)
        {
            if (String.IsNullOrWhiteSpace(nct))
            {
                return "";
            }
            string nctnumberpart = Regex.Match(nct, @"\d+").Value;
            int nctnumbercount = nctnumberpart.Length;
            if (nctnumbercount == 0)
            {
                return "";
            }
            else if (nctnumbercount < 8)
            {
                nctnumberpart = nctnumberpart.PadLeft(8, '0');
                return "NCT" + nctnumberpart;
            }
            else if (nctnumbercount == 8)
            {
                return "NCT" + nctnumberpart;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Fix the format of the ind number
        /// </summary>
        /// <param name="ind"></param>
        /// <returns></returns>
        public static string fixIND(string ind)
        {
            if (String.IsNullOrWhiteSpace(ind))
            {
                return "";
            }
            return Regex.Match(ind, @"\d +").Value;
        }

        /// <summary>
        /// Remove empty columns from a datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable removeEmptyColumns(DataTable table)
        {
            foreach (var column in table.Columns.Cast<DataColumn>().ToArray())
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(column) || String.IsNullOrWhiteSpace((string)dr[column.ColumnName])))
                    table.Columns.Remove(column);
            }

            return table;
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
        /// Delete duplicate row based on multiple columns from a data table
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static DataTable removeDuplicate(DataTable Table, string[] colNames)
        {
            /*
            // note that strongly typed dictionary has replaced the hash table + it uses custom comparer 
            var hTable = new Dictionary<DataRowInfo, string>();
            var duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in Table.Rows)
            {
                var dataRowInfo = new DataRowInfo(drow, colNames);

                if (hTable.ContainsKey(dataRowInfo))
                    duplicateList.Add(drow);
                else
                    hTable.Add(dataRowInfo, string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                Table.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return Table;*/

            //AsEnumerable().Where(x => (string)x["StudyId"] == studyId && formEvents.Contains((string)x["Event"])).AsEnumerable();

            var newTable = Table.Clone();
            foreach (DataRow dr in Table.Rows)
            {
                bool exist = true;
                foreach (string columnName in colNames)
                {
                    exist = exist && (from newrow in newTable.AsEnumerable()
                                      where ((string)newrow[columnName]).Trim().ToLower() == ((string)dr[columnName]).Trim().ToLower()
                                      select newrow).Any();
                }
                if (!exist)
                {
                    newTable.Rows.Add(dr.ItemArray);
                }
            }
            Table.Clear();
            Table = newTable.Copy();
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

        // contains values of specified columns
        internal sealed class DataRowInfo
        {
            public object[] Values { get; private set; }

            public DataRowInfo(DataRow dataRow, string[] columns)
            {
                Values = columns.Select(c => dataRow[c]).ToArray();
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                    return true;

                var other = obj as DataRowInfo;
                if (other == null)
                    return false;

                return Equals(other);
            }

            private bool Equals(DataRowInfo other)
            {
                if (this.Values.Length != other.Values.Length)
                    return false;
                for (int i = 0; i < this.Values.Length; i++)
                {
                    if (AreObjectsEqual(this.Values[i], other.Values[i]))
                        return false;
                }

                return true;
            }

            private static bool AreObjectsEqual(object left, object right)
            {
                if (ReferenceEquals(left, right))
                    return true;

                if (ReferenceEquals(left, null))
                    return false;

                if (ReferenceEquals(right, null))
                    return false;

                if (left.GetType() != right.GetType())
                    return false;

                return left.Equals(right);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = 0;
                    foreach (var value in this.Values)
                    {
                        hashCode = hashCode ^ ((value != null ? value.GetHashCode() : 0) * 397);
                    }
                    return hashCode;
                }
            }
        }

        /// <summary>
        /// Clears cache
        /// </summary>
        public static void reset()
        {
            if (isApprovedCache == null)
            {
                isApprovedCache = new Dictionary<string, bool>();
            }
            if (oldStudyCache == null)
            {
                oldStudyCache = new Dictionary<string, bool>();
            }
            if (studyNumberCache == null)
            {
                studyNumberCache = new Dictionary<string, string>();
            }
            isApprovedCache.Clear();
            oldStudyCache.Clear();
            studyNumberCache.Clear();
            OutputStudy.reset();
            OutputTeam.fpTeam.reset();
            OutputStatus.fpevent.reset();
            OutputStatus.fpstatus.reset();
        }

    }
}
