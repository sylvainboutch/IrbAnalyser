using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    public sealed class DbCompare
    {
        public DbCompare()
        {
        }

        /// <summary>
        /// Used for debugging, perform a test read of the database, returning the list of study number.
        /// </summary>
        /// <returns></returns>
        public string testRead()
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                var study = from st in db.VDA_V_STUDY_SUMMARY
                            select st;

                var output = "OUPUT : \r\n";
                foreach (var stu in study)
                {
                    output += stu.STUDY_NUMBER + " \r\n";
                }
                return output;
            }
        }

        /// <summary>
        /// Compare the file with the database, populates the databable for newStudy
        /// </summary>
        /// <param name="data"></param>
        public void isNewStudy(DataTable data)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {

                var study = from st in db.LCL_V_STUDYSUMM_PLUSMORE
                            select st;

                var dat = data.AsEnumerable();
                
                foreach (var row in dat)
                {
                    bool isContained = false;
                    foreach (var stu in study)
                    {
                        isContained = stu.MORE_IRBNUM == row["IRBNumber"].ToString() ? true: isContained;
                    }
                    if (!isContained)
                    {
                        OutputNewStudy.addRowStudy(row["IRBNumber"].ToString(), row["StudyNumber"].ToString());
                    }
                }
            }

        }
    }
}
