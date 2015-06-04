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
        public static DataTable newTeam = new DataTable();
        public static DataTable updatedTeam = new DataTable();

        private static IEnumerable<Model.VDA_V_STUDYTEAM_MEMBERS> _team;
        public static IEnumerable<Model.VDA_V_STUDYTEAM_MEMBERS> team
        {
            get
            {
                if (_team == null || _team.Count() == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        var query = (from st in db.VDA_V_STUDYTEAM_MEMBERS
                                     where st.MORE_IRBAGENCY != null
                                     && st.MORE_IRBSTUDYID != null
                                     select st);
                        _team = query.ToList<Model.VDA_V_STUDYTEAM_MEMBERS>();
                    }
                }
                return _team;
            }
            set
            {
                _team = value;
            }
        }


        /// <summary>
        /// Intiate the datatable
        /// </summary>
        private static void initiate()
        {
            if (newTeam.Columns.Count == 0)
            {
                newTeam.Columns.Add("TYPE", typeof(string));

                newTeam.Columns.Add("Study number", typeof(string));

                newTeam.Columns.Add("Email", typeof(string));
                newTeam.Columns.Add("AdditionnalEmails", typeof(string));
                newTeam.Columns.Add("First name", typeof(string));
                newTeam.Columns.Add("Last name", typeof(string));
                newTeam.Columns.Add("Full name", typeof(string));
                newTeam.Columns.Add("Role", typeof(string));
                //newTeam.Columns.Add("Group", typeof(string));
                newTeam.Columns.Add("Organization", typeof(string));
                newTeam.Columns.Add("Primary", typeof(string));
            }
            if (updatedTeam.Columns.Count == 0)
            {
                updatedTeam.Columns.Add("TYPE", typeof(string));

                updatedTeam.Columns.Add("Study number", typeof(string));

                updatedTeam.Columns.Add("Email", typeof(string));
                updatedTeam.Columns.Add("AdditionnalEmails", typeof(string));
                updatedTeam.Columns.Add("First name", typeof(string));
                updatedTeam.Columns.Add("Last name", typeof(string));
                updatedTeam.Columns.Add("Full name", typeof(string));
                updatedTeam.Columns.Add("Role", typeof(string));
                //updatedTeam.Columns.Add("Group", typeof(string));
                updatedTeam.Columns.Add("Organization", typeof(string));
                updatedTeam.Columns.Add("Primary", typeof(string));
            }
        }

        /// <summary>
        /// Add a new row to the team modification datatable
        /// </summary>
        /// <param name="row"></param>
        private static void addRow(DataRow row, string type, bool newrecord)
        {
            string role = "";
            //string group = "";
            string site = "";
            bool primary = Tools.compareStr(row["Primary"], "Y");

            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                role = BranyRoleMap.getRole((string)row["Role"], primary);
                //group = BranyRoleMap.getGroup((string)row["Role"]);
                site = BranySiteMap.getSite(((string)row["SiteName"]).Replace("(IBC)", ""));
            }

            if (role != "NA")
            {
                initiate();
                DataRow dr;
                if (newrecord)
                { dr = newTeam.NewRow(); }
                else
                { dr = updatedTeam.NewRow(); }

                dr["TYPE"] = type;

                dr["Study number"] = Tools.getStudyNumber((string)row["StudyId"], ((string)row["IRBNumber"]).Replace("(IBC)", ""));

                dr["Email"] = row["PrimaryEMailAddress"].ToString();
                dr["AdditionnalEmails"] = row["OtherEmailAdresses"].ToString();
                dr["First name"] = row["FirstName"].ToString();
                dr["Last name"] = row["LastName"].ToString();
                dr["Full name"] = row["FirstName"].ToString() + " " + row["LastName"].ToString();
                dr["Role"] = role;
                //dr["Group"] = group;
                dr["Organization"] = site;

                if (newrecord)
                { newTeam.Rows.Add(dr); }
                else
                { updatedTeam.Rows.Add(dr); }
            }
        }

        /// <summary>
        /// Analyse the team member file
        /// </summary>
        /// <param name="team"></param>
        public static void analyse(string filename)
        {
            initiate();

            FileParser fpTeam = new FileParser(filename, FileParser.type.Team);

            foreach (DataRow user in fpTeam.data.Rows)
            {
                analyseRow(user);
            }

            //analyseDelete(fpTeam.data);
        }

        /// <summary>
        /// Analyse study row from the import file
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow userRow)
        {
            string irbstudyId = userRow["StudyId"].ToString();

            string email = (string)userRow["PrimaryEMailAddress"];

            if (Tools.getOldStudy((string)userRow["StudyId"]))
            {
                //using (Model.VelosDb db = new Model.VelosDb())
                //{

                if (!String.IsNullOrEmpty(email))
                {
                    var user = from us in team
                               where us.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId)
                              && us.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                              && us.USER_EMAIL == email
                               select us;
                    if (!user.Any())
                    {
                        addRow(userRow, "New member", true);
                    }
                    else
                    {
                        var changed = false;

                        bool primary = Tools.compareStr(userRow["Primary"], "true");

                        if (user.First().USER_NAME != (string)userRow["FirstName"] + " " + (string)userRow["LastName"])
                        {
                            changed = true;
                        }
                        if (user.First().ROLE != BranyRoleMap.getRole((string)userRow["Role"], primary)
                            && BranyRoleMap.getRole((string)userRow["Role"], primary) != "NA")
                        {
                            changed = true;
                        }
                        else { userRow["Role"] = ""; }
                        if (user.First().USER_SITE_NAME != BranySiteMap.getSite((string)userRow["SiteName"]).Replace("(IBC)", ""))
                        {
                            changed = true;
                        }
                        else { userRow["SiteName"] = ""; }
                        //todo map sites and check
                        if (changed) { addRow(userRow, "Modified member", false); }
                    }
                }
                //}
            }
            else
            {
                addRow(userRow, "New study", true);
            }
        }

        /*/// <summary>
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
                                      where (string)dr["SiteName"] == user.site.USER_EMAIL
                                      select dr).Count();
                    var countStudy = (from DataRow dr in studys.Rows
                                      where (string)dr["IRBAgency"] == user.agency
                                      && (string)dr["StudyId"] == user.studyId
                                      select dr).Count();
                    if (countEmail == 0 && countStudy != 0)
                    {
                        addRow("Deleted member", user.site.USER_EMAIL, user.site.USER_NAME, user.site.ROLE, user.site.USER_SITE_NAME, user.studyId, user.agency);
                    }
                }
            }
        }*/

    }

}
