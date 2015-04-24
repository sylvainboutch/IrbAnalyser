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
        public static string getFullName(DataRow dr)
        {
            return dr == null ? "" : dr["FirstName"] + " " + dr["LastName"];
        }

        public static string getStudyNumber(string IRBstudyId, string IRBAgency)
        {
            string number = "";
            using (Model.VelosDb db = new Model.VelosDb())
            {
                number = (from stud in db.LCL_V_STUDYSUMM_PLUSMORE
                          where stud.MORE_IRBSTUDYID.ToLower() == IRBstudyId.ToLower()
                       && stud.MORE_IRBAGENCY.ToLower() == IRBAgency.ToLower()
                          select stud.STUDY_NUMBER).FirstOrDefault();
            }
            return number;
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

        /// <summary>
        /// UnZip the files to a temp folder and return the path to the folder
        /// </summary>
        /// <param name="zipFile"></param>
        /// <returns></returns>
        public static string UnZip(string zipFile)
        {
            string directory = Path.GetTempPath();
            directory = directory + "IRBreport\\";

            UnZip(zipFile, directory);

            return directory;
        }

        public static void CleanUpFile(string filepath)
        {
            DirectoryInfo directory = new DirectoryInfo(filepath);
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        /// <summary>
        /// Unzip a file to a specific folder
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="folderPath"></param>
        public static void UnZip(string zipFile, string folderPath)
        {
            //from : http://www.fluxbytes.com/csharp/unzipping-files-using-shell32-in-c/
            if (!File.Exists(zipFile))
                throw new FileNotFoundException();

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Shell32.Shell objShell = new Shell32.Shell();
            Shell32.Folder destinationFolder = objShell.NameSpace(folderPath);
            Shell32.Folder sourceFile = objShell.NameSpace(zipFile);

            foreach (var file in sourceFile.Items())
            {
                destinationFolder.CopyHere(file, 4 | 16);
            }
        }

    }
}
