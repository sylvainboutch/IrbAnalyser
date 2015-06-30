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
        public static string PI = "PI-View Access";
        public static string RC = "Regulatory Coordinator Full";
        public static string SC = "Study Coordinator";
        public static string defaultDisabledRole = "No Privilege";
        public static string enabledGroup = "Study Member";
        public static string disabledGroup = "NO_PRIVILEGE";

        //List of team members
        public static DataTable newTeam = new DataTable();
        public static DataTable updatedTeam = new DataTable();
        public static DataTable triggerTeam = new DataTable();
        public static DataTable newNonSystemUser = new DataTable();

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
                                     && st.IRBIDENTIFIERS != null
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
                                     & st.USER_USRNAME != null
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


        private static FileParser _fpteam;
        public static FileParser fpTeam
        {
            get
            {
                if (_fpteam == null || _fpteam.data == null || _fpteam.data.Rows.Count == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {
                        _fpteam = new FileParser(Tools.filename + "Team.txt", FileParser.type.Team);
                    }
                }
                return _fpteam;
            }
            set
            {
                _fpteam = value;
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
            if (triggerTeam.Columns.Count == 0)
            {
                triggerTeam.Columns.Add("TYPE", typeof(string));

                triggerTeam.Columns.Add("Study number", typeof(string));

                triggerTeam.Columns.Add("Email", typeof(string));
                triggerTeam.Columns.Add("AdditionnalEmails", typeof(string));
                triggerTeam.Columns.Add("First name", typeof(string));
                triggerTeam.Columns.Add("Last name", typeof(string));
                triggerTeam.Columns.Add("Full name", typeof(string));
                triggerTeam.Columns.Add("Role", typeof(string));
                //newTeam.Columns.Add("Group", typeof(string));
                triggerTeam.Columns.Add("Organization", typeof(string));
                triggerTeam.Columns.Add("Primary", typeof(string));
            }
            if (newNonSystemUser.Columns.Count == 0)
            {
                newNonSystemUser.Columns.Add("TYPE", typeof(string));

                newNonSystemUser.Columns.Add("Study number", typeof(string));

                newNonSystemUser.Columns.Add("Email", typeof(string));
                newNonSystemUser.Columns.Add("AdditionnalEmails", typeof(string));
                newNonSystemUser.Columns.Add("First name", typeof(string));
                newNonSystemUser.Columns.Add("Last name", typeof(string));
                newNonSystemUser.Columns.Add("Full name", typeof(string));
                newNonSystemUser.Columns.Add("Role", typeof(string));
                //newTeam.Columns.Add("Group", typeof(string));
                newNonSystemUser.Columns.Add("Organization", typeof(string));
                newNonSystemUser.Columns.Add("Primary", typeof(string));


                newNonSystemUser.Columns.Add("TimeZone", typeof(string));
                newNonSystemUser.Columns.Add("Group", typeof(string));
                newNonSystemUser.Columns.Add("Login", typeof(string));

            }

        }

        /// <summary>
        /// Add a new row to the team modification datatable
        /// </summary>
        /// <param name="row"></param>
        private static void addRow(DataRow row, string type, DataTable records)
        {
            if (!string.IsNullOrWhiteSpace((string)row["PrimaryEMailAddress"]))
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

                else if (Agency.AgencyVal == Agency.AgencyList.IRIS)
                {
                    role = IRISMap.RoleMap.getRole((string)row["Role"], primary);
                    //group = BranyRoleMap.getGroup((string)row["Role"]);
                    site = IRISMap.SiteMap.getSite((string)row["SiteName"]);
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

                    if (type == "New non system user")
                    {
                        dr["TimeZone"] = "GMT-5";
                        dr["Group"] = "NO_PRIVILEGE";
                        dr["Login"] = ((string)row["PrimaryEMailAddress"]).Split('@')[0];
                    }

                    if (fpTeam.initColumnCount < row.Table.Columns.Count)
                    {
                        for (int i = fpTeam.initColumnCount; i < row.Table.Columns.Count; i++)
                        {
                            if (!dr.Table.Columns.Contains(row.Table.Columns[i].ColumnName))
                            {
                                dr.Table.Columns.Add(row.Table.Columns[i].ColumnName);
                            }
                            dr[row.Table.Columns[i].ColumnName] = row[i];
                        }
                    }

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
        }

        /// <summary>
        /// Add a new row to the team modification datatable
        /// </summary>
        /// <param name="row"></param>
        private static void addRowVelosUser(Model.VDA_V_STUDYTEAM_MEMBERS row, string type, DataTable records)
        {
            initiate();
            DataRow dr;

            dr = records.NewRow();

            dr["TYPE"] = type;

            dr["Study number"] = row.STUDY_NUMBER;

            dr["Email"] = row.USER_EMAIL;
            dr["First name"] = row.USER_NAME.Split(' ')[0];
            dr["Last name"] = row.USER_NAME.Split(' ')[1];
            dr["Full name"] = row.USER_NAME;
            dr["Role"] = row.ROLE;
            //dr["Group"] = group;
            dr["Organization"] = row.USER_SITE_NAME;

            records.Rows.Add(dr);

        }

        /// <summary>
        /// Analyse the team member file
        /// </summary>
        /// <param name="team"></param>
        public static void analyse(string filename)
        {
            initiate();

            foreach (DataRow user in fpTeam.data.Rows)
            {
                analyseRow(user);
            }

            analyseDelete();
        }

        /// <summary>
        /// Analyse study row from the import file
        /// </summary>
        /// <param name="dr"></param>
        private static void analyseRow(DataRow userRow)
        {
            string irbstudyId = userRow["StudyId"].ToString();

            string email = ((string)userRow["PrimaryEMailAddress"]).ToLower().Trim();

            if (!string.IsNullOrWhiteSpace(email))
            {

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

                if (currentuser == null)
                {
                    string login = ((string)userRow["PrimaryEMailAddress"]).Split('@')[0].ToLower().Trim();

                    currentuser = ((from us in accounts
                                        where us.USER_USRNAME.ToLower() == login
                                        select us).FirstOrDefault());
                    if (currentuser != null)
                    {
                        userRow["PrimaryEMailAddress"] = currentuser.USER_EMAIL;
                    }                
                }

                if (currentuser != null)
                {
                    if (!issuperuser)// && currentuser.USER_DEFAULTGRP == enabledGroup && isactiveuser)
                    {
                        if (Tools.getOldStudy((string)userRow["StudyId"]))
                        {
                            //using (Model.VelosDb db = new Model.VelosDb())
                            //{

                            if (!String.IsNullOrEmpty(email))
                            {
                                var user = from us in team
                                           where us.IRBIDENTIFIERS.Trim().ToLower().Contains(irbstudyId.Trim().ToLower())
                                          && us.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                          && us.USER_EMAIL.ToLower() == email
                                           select us;
                                if (!user.Any())
                                {
                                    addRow(userRow, "New member", newTeam);
                                    if (!isactiveuser)
                                    {
                                        addRow(userRow, "Inactive User added to study team", triggerTeam);
                                    }
                                    else if (currentuser.USER_DEFAULTGRP != enabledGroup && currentuser.USER_EMAIL == email && isactiveuser && !issuperuser)
                                    {
                                        addRow(userRow, "User needs training", triggerTeam);
                                    }
                                }
                                else
                                {
                                    var changed = false;

                                    bool primary = Tools.compareStr(userRow["Primary"], "true");

                                    if (user.First().USER_NAME != (string)userRow["FirstName"] + " " + (string)userRow["LastName"])
                                    {
                                        changed = true;
                                    }

                                    if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                                    {
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
                                    }

                                    if (Agency.AgencyVal == Agency.AgencyList.IRIS)
                                    {
                                        if (user.First().ROLE != IRISMap.RoleMap.getRole((string)userRow["Role"], primary)
                                            && IRISMap.RoleMap.getRole((string)userRow["Role"], primary) != "NA")
                                        {
                                            changed = true;
                                        }
                                        else { userRow["Role"] = ""; }
                                        if (user.First().USER_SITE_NAME != IRISMap.SiteMap.getSite((string)userRow["SiteName"]).Replace("(IBC)", ""))
                                        {
                                            changed = true;
                                        }
                                        else { userRow["SiteName"] = ""; }
                                    }


                                    //todo map sites and check
                                    if (changed)
                                    {
                                        addRow(userRow, "Modified member", updatedTeam);
                                    }
                                }
                            }
                            //}
                        }
                        else
                        {
                            addRow(userRow, "New study", newTeam);
                            if (!isactiveuser)
                            {
                                addRow(userRow, "Inactive User added to study team", triggerTeam);
                            }
                            else if (currentuser.USER_DEFAULTGRP != enabledGroup && currentuser.USER_EMAIL == email && isactiveuser && !issuperuser)
                            {
                                addRow(userRow, "User needs training", triggerTeam);
                            }
                        }
                    }
                }
                else
                {
                    addRow(userRow, "User needs access", triggerTeam);
                    addRow(userRow, "New non system user", newNonSystemUser);
                }
            }
        }

        /// <summary>
        /// Analyse complete tables to find removed value
        /// </summary>
        /// <param name="studys"></param>
        private static void analyseDelete()
        {
            foreach (var user in team)
            {
                if (user.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr)
                {
                    var agency = user.MORE_IRBAGENCY;

                    var studyId = Tools.getStudyIdentifiers(user.IRBIDENTIFIERS);

                    var countEmail = (from DataRow dr in fpTeam.data.Rows
                                      where (string)dr["StudyId"] == studyId
                                      && ((string)dr["PrimaryEMailAddress"]).ToLower().Trim() == user.USER_EMAIL.ToLower().Trim()
                                      select dr).Count();

                    var countStudy = (from DataRow dr in OutputStudy.fpstudys.data.Rows
                                      where (string)dr["StudyId"] == studyId
                                      select dr).Count();

                    if (countEmail == 0 && countStudy != 0 && user.ROLE.Trim().ToLower() != RC.Trim().ToLower())
                    {
                        addRowVelosUser(user, "Deleted member", updatedTeam);
                    }
                    else if (user.ROLE.Trim().ToLower() == RC.Trim().ToLower() && countEmail == 0 && countStudy != 0)
                    {
                        var delete1 = (from DataRow dr in newTeam.AsEnumerable()
                                       where (string)dr["Study number"] == user.STUDY_NUMBER
                                       && (string)dr["Role"] == RC
                                       select dr).Any();

                        var delete2 = (from us in team
                                       where us.MORE_IRBAGENCY.ToLower() == Agency.agencyStrLwr
                                       && us.IRBIDENTIFIERS.Trim().ToLower() == studyId
                                       && us.ROLE == RC
                                       select us).Any();

                        var delete3 = (from DataRow dr in updatedTeam.AsEnumerable()
                                       where (string)dr["Study number"] == user.STUDY_NUMBER
                                       && (string)dr["Role"] == RC
                                       && (string)dr["TYPE"] == "Modified member"
                                       select dr).Any();


                        if (delete1 || delete2 || delete3)
                        {
                            addRowVelosUser(user, "Deleted member", updatedTeam);
                        }
                        else
                        {
                            addRowVelosUser(user, "RC deleted", triggerTeam);
                        }
                    }
                }
            }
        }

    }

}
