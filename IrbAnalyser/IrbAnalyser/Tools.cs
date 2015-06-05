using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace IrbAnalyser
{

    static class Tools
    {
        public static string filename = "";

        private static FileParser fpStudy = new FileParser();

        public static string getFullName(DataRow dr)
        {
            return dr == null ? "" : dr["FirstName"] + " " + dr["LastName"];
        }

        public static string generateStudyIdentifiers(DataTable study, string studyID)
        {
            string result = studyID;
            var stud = (from st in study.AsEnumerable()
                       where st.Field<string>("IRB Study ID").Trim().ToLower() == studyID.Trim().ToLower()
                       && st.Field<string>("IRB Agency name").Trim().ToLower() == Agency.agencyStrLwr
                        select (st.Field<string>("SiteName").Replace("(IBC)", "") + "::" + st.Field<string>("StudySiteId"))).ToArray();
            foreach (var stu in stud)
            {
                result += "&&" + stu;
            }
            return result;
        }

        public static string parseDate(string date)
        {
            DateTime dateparsed = DateTime.MinValue;
            DateTime.TryParse(date,out dateparsed);
            return dateparsed == DateTime.MinValue ? "" : dateparsed.Date.ToString("MM/dd/yyyy");
        }

        public static string getStudyNumber(string IRBstudyId, string IRBnumber)
        {
            if (fpStudy.data.Rows.Count == 0)
            { 
                fpStudy = new FileParser(filename + "Study.txt",FileParser.type.Study);
            }

            /*var stud = (from st in fpStudy.data.AsEnumerable()
                        where st.Field<string>("StudyId").Trim().ToLower() == IRBstudyId.Trim().ToLower()
                        select (st.Field<string>("StudyAcronym"))).ToArray();*/

            var stud = (from st in OutputStudy.fpstudys.data.AsEnumerable()
                        where st.Field<string>("StudyId").Trim().ToLower() == IRBstudyId.Trim().ToLower()
                        select (st.Field<string>("StudyAcronym"))).ToArray();

            
            string accronym = stud.Count() > 0 ? stud[0] : "";

            return getStudyNumber(IRBstudyId, IRBnumber, accronym);
        }

        public static string getStudyNumber(string IRBstudyId, string IRBnumber, string accronym)
        {
            string number = "";
            /*using (Model.VelosDb db = new Model.VelosDb())
            {
                number = (from stud in db.LCL_V_STUDYSUMM_PLUSMORE
                          where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(IRBstudyId.Trim().ToLower())
                       && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                          select stud.STUDY_NUMBER).FirstOrDefault();
            }*/

            number = (from stud in OutputStudy.studys
                      where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(IRBstudyId.Trim().ToLower())
                   && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                      select stud.STUDY_NUMBER).FirstOrDefault();

            if (number == null || number.Trim() == "")
            {
                accronym = string.IsNullOrWhiteSpace(accronym) ? "Please complete" : accronym;
                number = generateStudyNumber(IRBnumber, accronym);
            }
            return number;
        }

        private static string generateStudyNumber(string irbnumber, string accronym)
        {
            //string output = DateTime.Now.Year.ToString().Substring(2, 2);
            //string output = irbnumber.Substring(0, 2);
            string output = irbnumber.Replace("-", "");
            output += "_" + accronym + "_";
            output += Agency.AgencyVal == Agency.AgencyList.BRANY ? "B" : "OCT";//OR MSA ? since apperently OCT enters brany CDA
            //output += irbnumber;
            return output;
        }


        public static bool getOldStudy(string IRBstudyId)
        {
            bool ret;
            /*using (Model.VelosDb db = new Model.VelosDb())
            {
                ret = (from stud in db.LCL_V_STUDYSUMM_PLUSMORE
                       where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(IRBstudyId.Trim().ToLower())
                    && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                       select stud.STUDY_NUMBER).Any();
            }*/

            ret = OutputStudy.studys.Any(x => x.MORE_IRBSTUDYID.Trim().ToLower().Contains(IRBstudyId.Trim().ToLower())
                && x.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr);

            /*ret = (from stud in OutputStudy.studys
                   where stud.MORE_IRBSTUDYID.Trim().ToLower().Contains(IRBstudyId.Trim().ToLower())
                && stud.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                   select stud.STUDY_NUMBER).Any();*/

            return ret;
        }

        public static string cleanMap(string input)
        {
            input = input.Trim();       
            string output = "";
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ' || c == '/' || c == '(' || c == ')')
                {
                    sb.Append(c);
                }
            }
            output = sb.ToString();
            output = output.Replace("  ", " ");
            return output;
        }

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

        public static string parse(string input)
        {
            input = input.Trim('"');
            input = input == "NULL" ? "" : input;

            return input;
        }

        public static string removeQuote(string input)
        {
            return input.Trim('"');
        }

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

    }
}
