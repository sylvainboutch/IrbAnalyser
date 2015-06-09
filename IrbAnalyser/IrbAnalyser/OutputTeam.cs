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
        public static string PI = "PI-Full Access";
        public static string RC = "Regulatory Coordinator Full";
        public static string defaultDisabledRole = "No Privilege";
        public static string enabledGroup = "Study Member";
        public static string disabledGroup = "NO_PRIVILEGE";

        //List of team members
        public static DataTable newTeam = new DataTable();
        public static DataTable updatedTeam = new DataTable();
        public static DataTable trainingNeededTeam = new DataTable();

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
                                     && st.USER_EMAIL != null
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

        private static IEnumerable<Model.LCL_V_USER> _accounts;
        public static IEnumerable<Model.LCL_V_USER> accounts
        {
            get
            {
                if (_accounts == null || _accounts.Count() == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {

                        var query = (from st in db.LCL_V_USER
                                    where st.USER_EMAIL != null
                                     select st);
                        _accounts = query.ToList<Model.LCL_V_USER>();
                    }
                }
                return _accounts;
            }
            set
            {
                _accounts = value;
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
            if (trainingNeededTeam.Columns.Count == 0)
            {
                trainingNeededTeam.Columns.Add("TYPE", typeof(string));

                trainingNeededTeam.Columns.Add("Study number", typeof(string));

                trainingNeededTeam.Columns.Add("Email", typeof(string));
                trainingNeededTeam.Columns.Add("AdditionnalEmails", typeof(string));
                trainingNeededTeam.Columns.Add("First name", typeof(string));
                trainingNeededTeam.Columns.Add("Last name", typeof(string));
                trainingNeededTeam.Columns.Add("Full name", typeof(string));
                trainingNeededTeam.Columns.Add("Role", typeof(string));
                //newTeam.Columns.Add("Group", typeof(string));
                trainingNeededTeam.Columns.Add("Organization", typeof(string));
                trainingNeededTeam.Columns.Add("Primary", typeof(string));
            }

        }

        /// <summary>
        /// Add a new row to the team modification datatable
        /// </summary>
        /// <param name="row"></param>
        private static void addRow(DataRow row, string type, DataTable records)
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

                dr = records.NewRow();

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

                var dtUser = from user in records.AsEnumerable()
                             where user.Field<string>("Email").Trim().ToLower() == ((string)row["PrimaryEMailAddress"]).Trim().ToLower()
                             && user.Field<string>("Study number").Trim().ToLower() == ((string)dr["Study number"]).Trim().ToLower()
                             select user;
                if (dtUser.Count() > 0)
                {
                    foreach (DataRow rw in dtUser)
                    {
                        if (rw.Field<string>("Role") != role && role == RC)
                        {
                            rw["Role"] = role;
                        }
                        else if (rw.Field<string>("Role") != role && role == PI && rw.Field<string>("Role") != RC)
                        {
                            rw["Role"] = role;
                        }
                    }
                }
                else
                {
                    records.Rows.Add(dr);
                }
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

            string email = ((string)userRow["PrimaryEMailAddress"]).ToLower();

            var issuperuser = (from us in accounts
                               where us.USER_EMAIL.ToLower() == email
                               && us.GRP_SUPUSR_FLAG == 1
                               && us.USER_STATUS == "Active"
                               select us).Any();

            var isactiveuser = (from us in accounts
                               where us.USER_EMAIL.ToLower() == email
                               && us.USER_STATUS == "Active"
                               select us).Any();

            var currentuser = ((from us in accounts
                                where us.USER_EMAIL.ToLower() == email
                                select us).FirstOrDefault());

            if (currentuser != null)
            {
                if (!issuperuser && currentuser.USER_DEFAULTGRP == enabledGroup && isactiveuser)
                {
                    if (Tools.getOldStudy((string)userRow["StudyId"]))
                    {
                        //using (Model.VelosDb db = new Model.VelosDb())
                        //{

                        if (!String.IsNullOrEmpty(email))
                        {
                            var user = from us in team
                                       where us.MORE_IRBSTUDYID.Trim().ToLower().Contains(irbstudyId)
                                      && us.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                      && us.USER_EMAIL.ToLower() == email
                                       select us;
                            if (!user.Any())
                            {
                                addRow(userRow, "New member", newTeam);
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
                                if (changed) { addRow(userRow, "Modified member", updatedTeam); }
                            }
                        }
                        //}
                    }
                    else
                    {
                        addRow(userRow, "New study", newTeam);
                    }
                }
                else if (currentuser.USER_DEFAULTGRP != enabledGroup && currentuser.USER_EMAIL == email && isactiveuser && !issuperuser)
                {
                    addRow(userRow, "User needs training", trainingNeededTeam);
                }
                else if (!isactiveuser)
                {
                    addRow(userRow, "Inactive User", trainingNeededTeam);
                }
            }
            else
            {
                addRow(userRow, "User needs access", trainingNeededTeam);
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
