using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Specialized;
using System.Collections;
using System.Text.RegularExpressions;

namespace IrbAnalyser
{
    /// <summary>
    /// Handles the team data
    /// </summary>
    class OutputTeam
    {
        //Hols the velos role name
        public static string PI = "PI-View Access";
        public static string SubIno = "Sub-Investigator-No Access";
        public static string RC = "Regulatory Coordinator Full";
        public static string RSC = "SC_REG Coordinator (OCT Managed)";
        public static string SC = "Study Coordinator";
        public static string defaultDisabledRole = "No Privilege";
        public static string enabledGroup = "Study Member";
        public static string disabledGroup = "NO_PRIVILEGE";


        public static string[] listRC = new string[]{
            "Regulatory Coordinator (OCT Managed)",
            "Regulatory Coordinator Full",
            "SC_REG Coordinator (OCT Managed)",
            "SC_REG Coordinator Full",
            "Full Study Manager"
        };

        public static string[] activeGroups = new string[]{
            "Admin",
            "CPDMU_Admin",
            "CPDMU_QA",
            "CPDMU_RC", 
            "CPDMU_SC", 
            "GYN_RSC", 
            "OCT Admin", 
            "OCT Study Management", 
            "Oncology Pharmacy", 
            "OSRP Management", 
            "PFS_Cal", 
            "Study Team", 
            "Study Team Limited"
        };

        //List of new team members
        public static DataTable newTeam = new DataTable();
        //List of updated team members
        public static DataTable updatedTeam = new DataTable();
        //List of trigger from the team workflow
        public static DataTable triggerTeam = new DataTable();
        //List of new member to create in Velos
        public static DataTable newNonSystemUser = new DataTable();
        //List of new member to create in Velos
        public static DataTable addedDeletedUser = new DataTable();

        //Holds the team information from Velos DB
        private static IEnumerable<Model.VDA_V_STUDYTEAM_MEMBERS> _team;
        public static IEnumerable<Model.VDA_V_STUDYTEAM_MEMBERS> team
        {
            get
            {
                if (_team == null || _team.Count() == 0)
                {
                    using (Model.VelosDb db = new Model.VelosDb())
                    {


                        IQueryable<Model.VDA_V_STUDYTEAM_MEMBERS> query;

                        query = (from st in db.VDA_V_STUDYTEAM_MEMBERS
                                 where st.IRBIDENTIFIERS != null
                                     //&& st.MORE_IRBAGENCY != null
                                 && st.USER_EMAIL != null
                                 select st);

                        /*if (Agency.AgencySetupVal == Agency.AgencyList.NONE)
                        {
                            query = (from st in db.VDA_V_STUDYTEAM_MEMBERS
                                     where st.MORE_IRBAGENCY != null
                                     && st.IRBIDENTIFIERS != null
                                     && st.USER_EMAIL != null
                                     select st);
                        }
                        //BRANY look up agency in MSD
                        else if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                        {
                            query = (from st in db.VDA_V_STUDYTEAM_MEMBERS
                                     where st.MORE_IRBAGENCY == Agency.agencyStrLwr
                                     && st.IRBIDENTIFIERS != null
                                     && st.USER_EMAIL != null
                                     select st);
                        }
                        //IRIS all other agency in MSD, non IRB studies wont have 
                        else
                        {
                            query = (from st in db.VDA_V_STUDYTEAM_MEMBERS
                                     where st.MORE_IRBAGENCY != Agency.brany
                                     && st.IRBIDENTIFIERS != null
                                     && st.USER_EMAIL != null
                                     select st);
                        }*/

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
                                     //where st.USER_EMAIL != null
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

                newTeam.Columns.Add("Study_number", typeof(string));

                newTeam.Columns.Add("Email", typeof(string));
                newTeam.Columns.Add("AdditionnalEmails", typeof(string));
                newTeam.Columns.Add("First name", typeof(string));
                newTeam.Columns.Add("Last name", typeof(string));
                newTeam.Columns.Add("User name", typeof(string));
                newTeam.Columns.Add("Role", typeof(string));
                //newTeam.Columns.Add("Group", typeof(string));
                newTeam.Columns.Add("Organization", typeof(string));
                newTeam.Columns.Add("Primary", typeof(string));
            }
            if (updatedTeam.Columns.Count == 0)
            {
                updatedTeam.Columns.Add("TYPE", typeof(string));

                updatedTeam.Columns.Add("Study_number", typeof(string));

                updatedTeam.Columns.Add("Email", typeof(string));
                updatedTeam.Columns.Add("AdditionnalEmails", typeof(string));
                updatedTeam.Columns.Add("First name", typeof(string));
                updatedTeam.Columns.Add("Last name", typeof(string));
                updatedTeam.Columns.Add("User name", typeof(string));
                updatedTeam.Columns.Add("Role", typeof(string));
                //updatedTeam.Columns.Add("Group", typeof(string));
                updatedTeam.Columns.Add("Organization", typeof(string));
                updatedTeam.Columns.Add("Primary", typeof(string));
            }
            if (triggerTeam.Columns.Count == 0)
            {
                triggerTeam.Columns.Add("TYPE", typeof(string));

                triggerTeam.Columns.Add("Study_number", typeof(string));

                triggerTeam.Columns.Add("Email", typeof(string));
                triggerTeam.Columns.Add("AdditionnalEmails", typeof(string));
                triggerTeam.Columns.Add("First name", typeof(string));
                triggerTeam.Columns.Add("Last name", typeof(string));
                triggerTeam.Columns.Add("User name", typeof(string));
                triggerTeam.Columns.Add("Role", typeof(string));
                //newTeam.Columns.Add("Group", typeof(string));
                triggerTeam.Columns.Add("Organization", typeof(string));
                triggerTeam.Columns.Add("Primary", typeof(string));
                triggerTeam.Columns.Add("RC", typeof(string));
                triggerTeam.Columns.Add("Source", typeof(string));
                triggerTeam.Columns.Add("Date", typeof(string));
            }
            if (newNonSystemUser.Columns.Count == 0)
            {
                newNonSystemUser.Columns.Add("TYPE", typeof(string));

                newNonSystemUser.Columns.Add("Study_number", typeof(string));

                newNonSystemUser.Columns.Add("Email", typeof(string));
                newNonSystemUser.Columns.Add("AdditionnalEmails", typeof(string));
                newNonSystemUser.Columns.Add("First name", typeof(string));
                newNonSystemUser.Columns.Add("Last name", typeof(string));
                newNonSystemUser.Columns.Add("User name", typeof(string));
                newNonSystemUser.Columns.Add("Role", typeof(string));
                //newTeam.Columns.Add("Group", typeof(string));
                newNonSystemUser.Columns.Add("Organization", typeof(string));
                newNonSystemUser.Columns.Add("Primary", typeof(string));


                newNonSystemUser.Columns.Add("TimeZone", typeof(string));
                newNonSystemUser.Columns.Add("Group", typeof(string));
                newNonSystemUser.Columns.Add("Login", typeof(string));

            }
            if (addedDeletedUser.Columns.Count == 0)
            {
                addedDeletedUser.Columns.Add("TYPE", typeof(string));

                addedDeletedUser.Columns.Add("Study_number", typeof(string));

                addedDeletedUser.Columns.Add("Email", typeof(string));
                addedDeletedUser.Columns.Add("AdditionnalEmails", typeof(string));
                addedDeletedUser.Columns.Add("First name", typeof(string));
                addedDeletedUser.Columns.Add("Last name", typeof(string));
                addedDeletedUser.Columns.Add("User name", typeof(string));
                addedDeletedUser.Columns.Add("Role", typeof(string));
                //newTeam.Columns.Add("Group", typeof(string));
                addedDeletedUser.Columns.Add("Organization", typeof(string));
                addedDeletedUser.Columns.Add("Primary", typeof(string));


                addedDeletedUser.Columns.Add("TimeZone", typeof(string));
                addedDeletedUser.Columns.Add("Group", typeof(string));
                addedDeletedUser.Columns.Add("Login", typeof(string));

            }

        }

        /// <summary>
        /// Add a new row to the team modification datatable
        /// </summary>
        /// <param name="row"></param>
        private static void addRow(DataRow row, string type, DataTable records)
        {
            //if (!string.IsNullOrWhiteSpace((string)row["PrimaryEMailAddress"]) || type == "New non system user")
            //{
                string role = "";
                //string group = "";
                string site = "";
                bool primary = Tools.compareStr(row["Primary"], "Y");


                if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    role = (string)row["Role"] == RSC ? (string)row["Role"] : BranyRoleMap.getRole((string)row["Role"], primary);
                    //group = BranyRoleMap.getGroup((string)row["Role"]);
                    site = BranySiteMap.getSite(((string)row["SiteName"]).Replace("(IBC)", ""));
                }

                else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                {
                    role = (string)row["Role"] == RSC ? (string)row["Role"] : IRISMap.RoleMap.getRole((string)row["Role"], primary);
                    //group = BranyRoleMap.getGroup((string)row["Role"]);
                    site = IRISMap.SiteMap.getSite((string)row["SiteName"]);
                }
                else
                {
                    role = OutputTeam.defaultDisabledRole;
                    site = OutputSite.EMmainsite;
                }

                if (role != "NA")
                {
                    initiate();
                    DataRow dr;

                    bool doNotAdd = false;

                    string piName = OutputStudy.getPI((string)row["StudyId"]);
                    if ((string)row["UserName"] == piName)
                    {
                        role = PI;
                        doNotAdd = true;
                    }

                    string rcName = OutputStudy.getRC((string)row["StudyId"]);
                    if ((string)row["UserName"] == rcName)
                    {
                        role = RC;
                        doNotAdd = true;
                    }

                    string scName = OutputStudy.getSC((string)row["StudyId"]);
                    if ((string)row["UserName"] == scName)
                    {
                        role = SC;
                        doNotAdd = true;
                    }

                    /*if (type == "New non system user" && (role == SC || role == RC || role == PI))
                    {
                        doNotAdd = false;
                    }
                    else if (type == "New non system user")
                    {
                        doNotAdd = true;
                    }*/


                    dr = records.NewRow();

                    dr["TYPE"] = type;

                    dr["Study_number"] = Tools.getOldStudyNumber((string)row["StudyId"]);

                    dr["Email"] = row["PrimaryEMailAddress"].ToString();
                    dr["AdditionnalEmails"] = row["OtherEmailAdresses"].ToString();
                    dr["First name"] = row["FirstName"].ToString();
                    dr["Last name"] = row["LastName"].ToString();
                    dr["User Name"] = (string)row["UserName"];
                    //dr["Full name"] = row["FirstName"].ToString() + " " + row["LastName"].ToString();
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

                    string email = ((string)row["PrimaryEMailAddress"]).ToLower().Trim();

                    string[] split = ((string)row["FirstName"]).Split(' ');
                    string firstnameLonguest = split[0];
                    foreach (string part in split)
                    {
                        firstnameLonguest = firstnameLonguest.Length > part.Length ? firstnameLonguest : part;
                    }

                    split = ((string)row["LastName"]).Split(' ');
                    string lastnameLonguest = split[0];
                    foreach (string part in split)
                    {
                        lastnameLonguest = lastnameLonguest.Length > part.Length ? lastnameLonguest : part;
                    }

                    var dtUser = from user in records.AsEnumerable()
                                 where ((!string.IsNullOrWhiteSpace(user.Field<string>("Email")) && user.Field<string>("Email").Trim().ToLower() == email && !string.IsNullOrWhiteSpace(email))
                                        || (
                                        user.Field<string>("First name").Trim().ToLower().Contains(firstnameLonguest.Trim().ToLower())
                                        & user.Field<string>("Last name").Trim().ToLower().Contains(lastnameLonguest.Trim().ToLower())))
                                 
                                 && user.Field<string>("Study_number").Trim().ToLower() == ((string)dr["Study_number"]).Trim().ToLower()

                                 select user;

                    if (dtUser.Count() > 0)
                    {
                        foreach (DataRow rw in dtUser)
                        {
                            if (!rw.Field<string>("TYPE").Contains(type))
                            {
                                rw["TYPE"] += "  --  " + type;
                            }
                            if (rw.Field<string>("Role") != role && role == RC)
                            {
                                rw["Role"] = role;
                            }
                            else if (rw.Field<string>("Role") != role && role == PI && rw.Field<string>("Role") != RC)
                            {
                                rw["Role"] = role;
                            }
                            else if (rw.Field<string>("Role") != role && rw.Field<string>("Role") == defaultDisabledRole)
                            {
                                rw["Role"] = role;
                            }
                        }
                    }
                    else if (!(doNotAdd && (type == "New study" || type == "New non system user")))
                    {
                        records.Rows.Add(dr);
                    }
                }
            //}
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

            if (records.Columns.Contains("RC"))
            {
                dr["RC"] = Tools.getRC(row.FK_STUDY);
            }

            if (records.Columns.Contains("Date"))
            {
                dr["Date"] = row.LAST_MODIFIED_DATE.ToString();
            }

            if (records.Columns.Contains("Source"))
            {
                dr["Source"] = Agency.agencyStrLwr;
            }

            dr["Study_number"] = row.STUDY_NUMBER;

            dr["Email"] = row.USER_EMAIL;
            dr["First name"] = row.USER_NAME.Split(' ')[0];
            dr["Last name"] = row.USER_NAME.Split(' ')[1];
            dr["User name"] = row.USER_NAME;
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

            if (!String.IsNullOrEmpty((string)userRow["IRBAgency"]))
            {
                if (((string)userRow["IRBAgency"]).ToLower() == "brany")
                    Agency.AgencyVal = Agency.AgencyList.BRANY;
                else if (((string)userRow["IRBAgency"]).ToLower() == "einstein" || ((string)userRow["IRBAgency"]).ToLower() == "IRIS")
                    Agency.AgencyVal = Agency.AgencyList.EINSTEIN;
                else Agency.AgencyVal = Agency.AgencySetupVal;
            }

            if (OutputStudy.shouldStudyBeAdded(irbstudyId))
            {
                /*if ((string)userRow["LastName"] == "Anampa Mesias")
                {
                    userRow["Role"] = "wtf";
                }*/

                string email = ((string)userRow["PrimaryEMailAddress"]).ToLower().Trim();

                string[] split = ((string)userRow["FirstName"]).Split(' ');
                split = split.Count() == 1 ? ((string)userRow["FirstName"]).Split('-') : split;
                string firstnameLonguest = split[0];
                foreach (string part in split)
                {
                    firstnameLonguest = firstnameLonguest.Length > part.Length ? firstnameLonguest : part;
                }

                split = ((string)userRow["LastName"]).Split(' ');
                split = split.Count() == 1 ? ((string)userRow["LastName"]).Split('-') : split;
                string lastnameLonguest = split[0];
                foreach (string part in split)
                {
                    lastnameLonguest = lastnameLonguest.Length >= part.Length ? lastnameLonguest : part;
                }

                var currentusers = (from us in accounts
                                    where ((!string.IsNullOrWhiteSpace(us.USER_EMAIL) && us.USER_EMAIL.ToLower() == email && !string.IsNullOrWhiteSpace(email))
                                    || (us.USER_NAME.ToLower().Contains((firstnameLonguest).ToLower().Trim())
                                    & us.USER_NAME.ToLower().Contains((lastnameLonguest).ToLower().Trim())
                                    ))
                                        select us);

                /*var currentusers = (from us in accounts
                                    where ((!string.IsNullOrWhiteSpace(us.USER_EMAIL) && us.USER_EMAIL.ToLower() == email && !string.IsNullOrWhiteSpace(email))
                                    || (us.USER_NAME.ToLower().Contains(((string)userRow["FirstName"]).ToLower().Trim())
                                    & us.USER_NAME.ToLower().Contains(((string)userRow["LastName"]).ToLower().Trim())
                                    ))
                                    select us);*/

                var currentuser = currentusers.FirstOrDefault();

                if (currentuser == null && !String.IsNullOrWhiteSpace((string)userRow["PrimaryEMailAddress"]))
                {
                    string login = ((string)userRow["PrimaryEMailAddress"]).Split('@')[0].ToLower().Trim();

                    var nonull = (from us in accounts
                                  where us.USER_USRNAME != null
                                  select us);

                    currentuser = ((from us in nonull
                                    where us.USER_USRNAME != null & us.USER_USRNAME.ToLower() == login
                                    select us).FirstOrDefault());
                    if (currentuser != null)
                    {
                        userRow["PrimaryEMailAddress"] = currentuser.USER_EMAIL.Trim().ToLower();
                        email = currentuser.USER_EMAIL.Trim().ToLower();
                    }
                }
                else if (currentuser != null && !string.IsNullOrWhiteSpace(email))
                {
                    userRow["PrimaryEMailAddress"] = currentuser.USER_EMAIL.Trim().ToLower();
                    email = currentuser.USER_EMAIL.Trim().ToLower();
                }

                var issuperuser = (from us in accounts
                                   where
                                   ((!string.IsNullOrWhiteSpace(us.USER_EMAIL) && us.USER_EMAIL.ToLower() == email)
                                    || (us.USER_NAME.ToLower().Contains(firstnameLonguest.ToLower().Trim())
                                    & us.USER_NAME.ToLower().Contains(lastnameLonguest.ToLower().Trim())
                                    ))
                                   && us.GRP_SUPUSR_FLAG == 1
                                   && us.USER_STATUS == "Active"
                                   select us).Any();

                var isactiveuser = (from us in accounts
                                    where ((!string.IsNullOrWhiteSpace(us.USER_EMAIL) && us.USER_EMAIL.ToLower() == email)
                                    || (us.USER_NAME.ToLower().Contains(firstnameLonguest.ToLower().Trim())
                                    & us.USER_NAME.ToLower().Contains(lastnameLonguest.ToLower().Trim())
                                    ))
                                    && us.USER_STATUS == "Active"
                                    select us).Any();

                var isdeleted = (from us in accounts
                                 where ((!string.IsNullOrWhiteSpace(us.USER_EMAIL) && us.USER_EMAIL.ToLower() == email)
                                    || (us.USER_NAME.ToLower().Contains(firstnameLonguest.ToLower().Trim())
                                    & us.USER_NAME.ToLower().Contains(lastnameLonguest.ToLower().Trim())
                                    ))
                                 select us).All(x => x.USER_HIDDEN == 1);

                var istrained = (from us in accounts
                                 where ((!string.IsNullOrWhiteSpace(us.USER_EMAIL) && us.USER_EMAIL.ToLower() == email)
                                    || (us.USER_NAME.ToLower().Contains(firstnameLonguest.ToLower().Trim())
                                    & us.USER_NAME.ToLower().Contains(lastnameLonguest.ToLower().Trim())
                                    ))
                                 && !(us.USER_JOBTYPE == null || us.USER_JOBTYPE == "Untrained")
                                 select us).Any();

                //For testing only   Michler is a deleted user
                /*if (currentuser.USER_NAME.Contains("Michler"))
                {
                    wtf = "w";
                    isdeleted = isdeleted;

                }

                wtf = "2";*/

                if (currentuser != null)
                {
                    if (currentusers.Count() > 1)
                    {
                        var activeuser = (from us in currentusers
                                          where us.USER_STATUS == "Active"
                                          select us).FirstOrDefault();
                        userRow["UserName"] = activeuser == null ? currentuser.USER_NAME : activeuser.USER_NAME;
                    }
                    else
                    {
                        userRow["UserName"] = currentuser.USER_NAME;
                    }

                    if (!issuperuser)// && currentuser.USER_DEFAULTGRP == enabledGroup && isactiveuser)
                    {
                        if (Tools.getOldStudy((string)userRow["StudyId"]))
                        {
                            //using (Model.VelosDb db = new Model.VelosDb())
                            //{

                            if (!String.IsNullOrEmpty(email))
                            {
                                var user = from us in team
                                           where us.IRBIDENTIFIERS.Trim().ToLower().Split('>')[0] == (irbstudyId.Trim().ToLower())
                                          && ((!string.IsNullOrWhiteSpace(us.USER_EMAIL) && us.USER_EMAIL.ToLower() == email)
                                    || (us.USER_NAME.ToLower().Contains(firstnameLonguest.ToLower().Trim())
                                    & us.USER_NAME.ToLower().Contains(lastnameLonguest.ToLower().Trim())
                                    ))
                                           select us;
                                if (!user.Any() && !isdeleted)
                                {
                                    addRow(userRow, "New member", newTeam);
                                    if (!isactiveuser)
                                    {
                                        addRow(userRow, "Inactive User added to study team", triggerTeam);
                                    }
                                    //else if (currentuser.USER_DEFAULTGRP != enabledGroup && currentuser.USER_EMAIL == email && isactiveuser && !issuperuser)
                                    else if (!istrained)
                                    {
                                        addRow(userRow, "User needs training", triggerTeam);
                                    }
                                }
                                else if (isdeleted)
                                {
                                    addRow(userRow, "Deleted user added to a study", addedDeletedUser);
                                }
                                else
                                {
                                    var changed = false;

                                    bool primary = Tools.compareStr(userRow["Primary"], "true");

                                    //Out of specs
                                    /*if (user.First().USER_NAME != (string)userRow["FirstName"] + " " + (string)userRow["LastName"])
                                    {
                                        changed = true;
                                    }*/

                                    string newRole = "";
                                    if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                                    {
                                        newRole = BranyRoleMap.getRole((string)userRow["Role"], primary);
                                    }
                                    else if (Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
                                    {
                                        newRole = IRISMap.RoleMap.getRole((string)userRow["Role"], primary);
                                    }
                                    else
                                    {
                                        newRole = OutputTeam.defaultDisabledRole;
                                    }

                                    //For debugging only
                                    /*if (String.IsNullOrWhiteSpace(newRole))
                                    {
                                        int bla = 1;
                                        bla = bla + 1;
                                    }*/

                                    if (newRole != user.First().ROLE)
                                    {
                                        if (newRole == PI && user.First().ROLE != RC)
                                        {
                                            changed = true;
                                        }
                                        else if (newRole == RC && user.First().ROLE == defaultDisabledRole)
                                        {
                                            changed = true;
                                        }
                                        else if (newRole == RC && user.First().ROLE == SC)
                                        {
                                            changed = true;
                                            userRow["Role"] = RSC;
                                        }

                                        if (changed)
                                        {
                                            addRow(userRow, "Modified member", updatedTeam);
                                        }
                                    }
                                }
                            }
                            //}
                        }
                        else
                        {
                            if (isdeleted)
                            {
                                addRow(userRow, "Deleted/Hidden user added to a study", addedDeletedUser);
                            }
                            else
                            {
                                addRow(userRow, "New study", newTeam);
                                if (!isactiveuser)
                                {
                                    addRow(userRow, "Inactive User added to study team", triggerTeam);
                                }
                                else if (!listRC.Contains(string.IsNullOrWhiteSpace(currentuser.USER_DEFAULTGRP) ? "" : currentuser.USER_DEFAULTGRP.Trim().ToLower(), StringComparer.OrdinalIgnoreCase) && currentuser.USER_EMAIL == email && isactiveuser && !issuperuser)
                                {
                                    addRow(userRow, "User needs training", triggerTeam);
                                }
                            }
                        }
                    }
                }
                else// if (!Tools.getOldStudy((string)userRow["StudyId"]))
                {
                    //addRow(userRow, "User needs access", triggerTeam);
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
                var studyId = Tools.getStudyIdentifiers(user.IRBIDENTIFIERS);
                if (OutputStudy.shouldStudyBeAdded(studyId))
                {
                    if (OutputStudy.isStudyInDataSource(studyId))
                    {


                        var userAccount = (from account in accounts
                                           where (!string.IsNullOrWhiteSpace(account.USER_EMAIL) && account.USER_EMAIL.Trim().ToLower() == user.USER_EMAIL.Trim().ToLower())
                                           & account.USER_NAME.Trim().ToLower() == user.USER_NAME.Trim().ToLower()
                                           select account).First();

                        //if (userAccount.USER_TYPE != "Non-System User")
                        //{
                        //var agency = user.MORE_IRBAGENCY;

                        //CONTEXT SWITCH BLOCKING HERE
                        var countEmail = (from DataRow dr in fpTeam.data.Rows
                                          where (string)dr["StudyId"] == studyId
                                          && ((!string.IsNullOrWhiteSpace(user.USER_EMAIL) && ((string)dr["PrimaryEMailAddress"]).ToLower().Trim() == user.USER_EMAIL.ToLower().Trim())
                                          || (user.USER_NAME.ToLower().Trim().Contains(((string)dr["LastName"]).ToLower().Trim())))
                                          select dr).Count();

                        var countStudy = (from DataRow dr in OutputStudy.fpstudys.data.Rows
                                          where (string)dr["StudyId"] == studyId
                                          select dr).Count();

                        if (countEmail == 0 && countStudy != 0 && user.ROLE != null && user.ROLE.Trim().ToLower() != RC.Trim().ToLower())
                        {
                            //addRowVelosUser(user, "Non-RC Absent from the data source", updatedTeam);
                            addRowVelosUser(user, "Non-RC Absent from the data source", triggerTeam);
                        }
                        else if (user.ROLE != null && listRC.Contains(user.ROLE.Trim().ToLower(), StringComparer.OrdinalIgnoreCase) && countEmail == 0 && countStudy != 0)
                        {
                            var delete1 = (from DataRow dr in newTeam.AsEnumerable()
                                           where (string)dr["Study_number"] == user.STUDY_NUMBER
                                           && (string)dr["Role"] == RC
                                           select dr).Any();

                            var delete2 = (from us in team
                                           where us.IRBIDENTIFIERS.Trim().ToLower() == studyId
                                           && listRC.Contains(user.ROLE.Trim().ToLower(), StringComparer.OrdinalIgnoreCase)
                                           //&& us.ROLE == RC
                                           select us).Any();

                            var delete3 = (from DataRow dr in updatedTeam.AsEnumerable()
                                           where (string)dr["Study_number"] == user.STUDY_NUMBER
                                           && (string)dr["Role"] == RC
                                           //&& (string)dr["TYPE"] == "Modified member"
                                           select dr).Any();


                            if (delete1 || delete2 || delete3)
                            {
                                //addRowVelosUser(user, "Deleted member", updatedTeam);
                                addRowVelosUser(user, "RC Absent from the data source", triggerTeam);
                            }
                            else
                            {
                                addRowVelosUser(user, "RC Absent from the data source - No replacement", triggerTeam);
                            }
                        }
                        //}
                    }
                }
            }
        }

        public static void addRowMigration(DataRow dr, string studynumber)
        {
            for (int i = 1; i < 10; i++)
            {
                string role = SC;
                string name = (string)dr["SC" + i];
                var splitted = name.Split(':');
                if (splitted.Count() > 1)
                {
                    name = splitted[1].Trim();
                    role = splitted[0].Trim();
                }
                if (!String.IsNullOrWhiteSpace(name))
                {
                    var issuperuser = (from us in accounts
                                       where
                                       ((us.USER_NAME.Trim().ToLower() == name.Trim().ToLower())
                                        )
                                       && us.GRP_SUPUSR_FLAG == 1
                                       && us.USER_STATUS == "Active"
                                       select us).Any();
                    if (!issuperuser)
                        addRowMigration(studynumber, name, role);
                }
            }
        }

        public static void addRowMigration(string studynumber, string fullname, string role)
        {
            initiate();
            DataRow dr = newTeam.NewRow();
            dr["Study_number"] = studynumber;
            var namesplit = fullname.Split(' ');
            var lastname = "";
            for (int i = 1; i < namesplit.Count(); i++)
            {
                lastname += namesplit[i].Trim();
                if (i + 1 < namesplit.Count())
                    lastname += " ";
            }
            dr["First name"] = namesplit[0].Trim();
            dr["Last name"] = lastname;
            dr["User Name"] = fullname;
            dr["Role"] = migrationRoleMap(role);
            dr["Organization"] = OutputSite.EMmainsite;
            newTeam.Rows.Add(dr);
        }

        public static void addRowXForm(string studyId, string studyNumber, string irbNumber)
        {

            initiate();

            XmlTools xmltools =  new XmlTools();
            DataTable team = xmltools.getAllPersonnal(studyId);

            foreach (DataRow person in team.Rows)
            {

                //DataRow dr = newTeam.NewRow();
                //dr["Study_number"] = studyNumber;

                string name = (string)person["Name"];

                name = name.Replace(", MD", "");
                name = name.Replace("MD", "");
                name = name.Replace("Md", "");
                name = name.Replace("M.D", "");
                name = name.Replace("PA-C", "");
                name = name.Replace("FNP-BC", "");
                name = name.Replace("NP-C", "");
                name = name.Replace("RN", "");
                name = name.Replace("ANP", "");
                name = name.Replace("FNP", "");
                name = name.Replace("CCRP", "");
                name = name.Replace("PhD", "");
                name = name.Replace("MSN", "");
                name = name.Replace("BSN", "");
                name = name.Replace("LNC", "");
                name = name.Replace("FNP", "");
                name = name.Replace("NP", "");
                name = name.Replace("MS", "");
                name = name.Replace("PAS", "");
                name = name.Replace("PA", "");


                name = name.Replace("Dr.", "");
                name = name.Replace("Dr ", "");

                name = name.Replace(",", "");

                while (name.Contains("  ")) name = name.Replace("  ", " ");

                name = name.Trim();

                var namesplit = name.Split(' ');
                var lastname = "";
                for (int i = 1; i < namesplit.Count(); i++)
                {
                    lastname += namesplit[i].Trim();
                    if (i + 1 < namesplit.Count())
                        lastname += " ";
                }



                /*dr["First name"] = namesplit[0].Trim();
                dr["Last name"] = lastname;
                dr["User Name"] = (string)person["Name"];
                dr["Role"] = (string)person["Role"];//migrationRoleMap(role);
                dr["Email"] = (string)person["Email"];
                dr["Organization"] = OutputSite.EMmainsite;

                newTeam.Rows.Add(dr);*/

                DataRow dr = fpTeam.data.NewRow();

                foreach (DataColumn dc in fpTeam.data.Columns)
                {
                    dr[dc.ColumnName] = "";
                }

                dr["StudyId"] = studyId;
                dr["IRBNumber"] = irbNumber;
                dr["IRBAgency"] = "brany";
                dr["SiteName"] = OutputSite.EMmainsite; ;
                dr["PrimaryEMailAddress"] = (string)person["Email"];
                dr["FirstName"] = namesplit[0].Trim();
                dr["LastName"] = lastname;
                dr["Role"] = (string)person["Role"];
                //dr[""] = "";

                fpTeam.data.Rows.Add(dr);
            }

        }

        public static string migrationRoleMap(string role)
        {
            role = role.Trim();
            if (role == "STU RES") return "No Privilege";
            if (role == "CO-PI") return "Limited Investigator";
            if (role == "SUB-PI") return "Limited Investigator";
            if (role == "NP") return "No Privilege";
            if (role == "NURSE") return "Nurse";
            if (role == "Research Assistant") return "Study Assistant";
            if (role == "ResAsso") return "No Privilege";
            if (role == "Collabo") return "No Privilege";
            if (role == "BIOSTAT") return "No Privilege";
            if (role == "MD") return "Limited Investigator";
            if (role == "ADMIN") return "No Privilege";
            if (role == "PA") return "No Privilege";
            else return role;
        }


        public static void removeDuplicateNewMembers()
        {
            newNonSystemUser = Tools.removeDuplicate(newNonSystemUser, new string[] { "Email", "First name", "Last name", "User Name" });
        }


        public static void removeDuplicateDeletedUser()
        {
            addedDeletedUser = Tools.removeDuplicate(addedDeletedUser, new string[] { "Email", "First name", "Last name", "User Name" });
        }
    }

}
