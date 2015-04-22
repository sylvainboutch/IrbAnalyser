using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Specialized;

namespace IrbAnalyser
{
    class OutputTeam
    {
        //List of team members
        public static DataTable team = new DataTable();

        /// <summary>
        /// Intiate the datatable
        /// </summary>
        private static void initiate()
        {
            if (team.Columns.Count == 0)
            {
                team.Columns.Add("TYPE", typeof(string));
                team.Columns.Add("Email", typeof(string));
                team.Columns.Add("AdditionnalEmails", typeof(string));
                team.Columns.Add("First name", typeof(string));
                team.Columns.Add("Last name", typeof(string));
                team.Columns.Add("Role", typeof(string));
                team.Columns.Add("Group", typeof(string));
                team.Columns.Add("Organization", typeof(string));
            }
        }

        /// <summary>
        /// Add a new row to the team modification datatable
        /// </summary>
        /// <param name="row"></param>
        private static void addRow(DataRow row, string type)
        {
            var role = BranyRoleMap.roleMapBrany[row["Role"].ToString()];
            var group = BranyRoleMap.groupMapBrany[row["Role"].ToString()];
            if (role != "NA" && group != "NA")
            {
                DataRow dr = team.NewRow();
                dr["TYPE"] = type;
                dr["Email"] = row["PrimaryEmailAdress"].ToString();
                dr["AdditionnalEmails"] = row["OtherEmailAdresses"].ToString();
                dr["First name"] = row["FirstName"].ToString();
                dr["Last name"] = row["LastName"].ToString();
                dr["Role"] = role;
                dr["Group"] = group;
                dr["Organization"] = row["Site"].ToString();
                team.Rows.Add(dr);
            }
        }

        /// <summary>
        /// Add a new row to the team modification datatable
        /// </summary>
        /// <param name="row"></param>
        private static void addRow(string type, string email, string name, string role, string site)
        {
            DataRow dr = team.NewRow();
            dr["TYPE"] = type;
            dr["Email"] = email;
            var indexSplit = name.IndexOf(' ');
            dr["First name"] = name.Substring(0, indexSplit);
            dr["Last name"] = name.Substring(indexSplit, name.Length - indexSplit);
            dr["Role"] = role;
            dr["Group"] = "Study TEAM";
            dr["Organization"] = site;
            team.Rows.Add(dr);
        }

        /// <summary>
        /// Analyse the team member file
        /// </summary>
        /// <param name="team"></param>
        public static void analyse(string filename)
        {
            initiate();

            FileParser fpTeam = new FileParser(filename);

            foreach (DataRow user in fpTeam.data.Rows)
            {
                analyseRow(user);
            }

            analyseDelete(fpTeam.data);
        }

        /// <summary>
        /// Analyse study row from the import file
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow userRow)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = userRow["StudyId"].ToString();
                string irbagency = userRow["IRBAgency"].ToString().ToLower();
                string email = (string)userRow["PrimaryEmailAdress"];
                if (!String.IsNullOrEmpty(email))
                {
                    var user = from us in db.VDA_V_STUDYTEAM_MEMBERS
                               join stud in db.LCL_V_STUDYSUMM_PLUSMORE on us.FK_STUDY equals stud.PK_STUDY
                               where stud.MORE_IRBSTUDYID == irbstudyId
                              && stud.MORE_IRBAGENCY.ToLower() == irbagency
                              && us.USER_EMAIL == email
                               select us;
                    if (!user.Any())
                    {
                        addRow(userRow, "New member");
                    }
                    else
                    {
                        var changed = false;
                        if (user.First().USER_NAME != (string)userRow["FirstName"] + " " + (string)userRow["LastName"])
                        {
                            changed = true;
                        }
                        if (user.First().ROLE != BranyRoleMap.roleMapBrany[(string)userRow["Role"]]
                            && BranyRoleMap.roleMapBrany[(string)userRow["Role"]] != "NA")
                        {
                            changed = true;
                        }
                        else { userRow["Role"] = ""; }
                        //todo map sites and check
                        if (!changed) { addRow(userRow, "Modified member"); }
                    }
                }
            }
        }

        /// <summary>
        /// Analyse complete tables to find removed value
        /// </summary>
        /// <param name="studys"></param>
        private static void analyseDelete(DataTable studys)
        {
            var std = from rw in studys.AsEnumerable()
                      select rw.Field<string>("StudyId");

            using (Model.VelosDb db = new Model.VelosDb())
            {
                var team = from users in db.VDA_V_STUDYTEAM_MEMBERS
                           join stud in db.LCL_V_STUDYSUMM_PLUSMORE on users.FK_STUDY equals stud.PK_STUDY
                           where std.Contains(stud.MORE_IRBSTUDYID)
                           select new { site = users, studyId = stud.MORE_IRBSTUDYID, agency = stud.MORE_IRBAGENCY };

                foreach (var user in team)
                {
                    var countEmail = (from DataRow dr in studys.Rows
                                      where (string)dr["Sitename"] == user.site.USER_EMAIL
                                      select dr).Count();
                    var countStudy = (from DataRow dr in studys.Rows
                                      where (string)dr["IRBAgency"] == user.agency
                                      && (string)dr["StudyId"] == user.studyId
                                      select dr).Count();
                    if (countEmail == 0 && countStudy != 0)
                    {
                        addRow("Deleted member", user.site.USER_EMAIL, user.site.USER_NAME, user.site.ROLE, user.site.USER_SITE_NAME);
                    }
                }
            }
        }

    }

}
