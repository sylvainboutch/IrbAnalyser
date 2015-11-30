using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    public static class Study_Personnel_list
    {
        public static DataTable studyDT = new DataTable();
        public static DataTable studyShort = new DataTable();

        public static DataTable latestStatus = new DataTable();

        public static void generateData()
        {
            getLatestStatus();

            studyDT = OutputStudy.fpstudys.data.Clone();
            studyShort = OutputStudy.fpstudys.data.Clone();

            foreach (DataColumn dc in OutputTeam.fpTeam.data.Columns)
            {
                if (!studyDT.Columns.Contains(dc.ColumnName))
                {
                    studyDT.Columns.Add(dc.ColumnName);
                }
            }

            foreach (DataColumn dc in latestStatus.Columns)
            {
                if (!studyDT.Columns.Contains(dc.ColumnName))
                {
                    studyDT.Columns.Add(dc.ColumnName);
                    studyShort.Columns.Add(dc.ColumnName);
                }
            }

            studyShort.Columns.Add("StudyType");

            //studyShort.Columns.Add("PI");
            //studyDT.Columns.Add("PI");

            studyShort.Columns.Add("PIemail");
            studyDT.Columns.Add("PIemail");


            //studyShort.Columns.Add("SC");
            //studyDT.Columns.Add("SC");

            studyShort.Columns.Add("SCemail");
            studyDT.Columns.Add("SCemail");


            //studyShort.Columns.Add("RC");
            //studyDT.Columns.Add("RC");

            studyShort.Columns.Add("RCemail");
            studyDT.Columns.Add("RCemail");

            studyShort.Columns.Add("OtherPersonnel");

            foreach (DataColumn dc in OutputStudy.newStudy.Columns)
            {
                if (!studyShort.Columns.Contains(dc.ColumnName))
                {
                    studyShort.Columns.Add(dc.ColumnName);
                }
            }

            foreach (DataColumn dc in OutputStudy.updatedStudy.Columns)
            {
                if (!studyShort.Columns.Contains(dc.ColumnName))
                {
                    studyShort.Columns.Add(dc.ColumnName);
                }
            }


            foreach (DataRow drStudy in OutputStudy.fpstudys.data.Rows)
            {
                if (drStudy["StudyId"].ToString() != "")
                {
                    var drShort = studyShort.NewRow();

                    var newStudy = OutputStudy.newStudy.AsEnumerable().Where(x => (string)x["IRB Study ID"] == (string)drStudy["StudyId"]).FirstOrDefault();
                    var updatedStudy = OutputStudy.updatedStudy.AsEnumerable().Where(x => (string)x["IRB Study ID"] == (string)drStudy["StudyId"]).FirstOrDefault();
                    bool ignored = !OutputStudy.shouldStudyBeAdded((string)drStudy["StudyId"]);

                    if (newStudy != null && !String.IsNullOrWhiteSpace(newStudy.Field<string>("IRB Study ID")))
                    {
                        drShort["StudyType"] = "New";
                        foreach (DataColumn dc in newStudy.Table.Columns)
                        {
                            drShort[dc.ColumnName] = newStudy.Field<string>(dc.ColumnName);
                        }
                    }

                    else if (updatedStudy != null && !String.IsNullOrWhiteSpace(updatedStudy.Field<string>("IRB Study ID")))
                    {
                        drShort["StudyType"] = "Updated";
                        foreach (DataColumn dc in updatedStudy.Table.Columns)
                        {
                            drShort[dc.ColumnName] = updatedStudy.Field<string>(dc.ColumnName);
                        }
                    }

                    else if (ignored) drShort["StudyType"] = "Ignored";
                    else drShort["StudyType"] = "Old";



                    drShort["OtherPersonnel"] = "= \"\"";
                    drShort["PI"] = OutputStudy.getPI((string)drStudy["StudyId"]);
                    //drShort["PIemail"] = OutputStudy.getPIeMail((string)drStudy["StudyId"]);

                    drShort["RC"] = OutputStudy.getRC((string)drStudy["StudyId"]);
                    drShort["SC"] = OutputStudy.getSC((string)drStudy["StudyId"]);

                    foreach (DataColumn dc in OutputStudy.fpstudys.data.Columns)
                    {
                        drShort[dc.ColumnName] = string.IsNullOrWhiteSpace((string)drStudy[dc.ColumnName]) ? drShort[dc.ColumnName] : drStudy[dc.ColumnName];
                    }
                    foreach (DataRow drStatus in latestStatus.Rows)
                    {
                        if ((string)drStudy["StudyId"] == (string)drStatus["StudyId"])
                        {
                            foreach (DataColumn dc in latestStatus.Columns)
                            {

                                drShort[dc.ColumnName] = drStatus[dc.ColumnName];
                            }
                        }
                    }

                    string[] irbno = ((string)drStudy["IRBNumber"]).Replace("(IBC)", "").Split('-');

                    if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                    {
                        drShort["Cancer"] = "N";
                        if (irbno.Count() >= 2 && irbno[1] == "06")
                            drShort["Cancer"] = "Y";
                    }


                    foreach (DataRow drTeam in OutputTeam.fpTeam.data.Rows)
                    {
                        if ((string)drStudy["StudyId"] == (string)drTeam["StudyId"])
                        {
                            var dr = studyDT.NewRow();
                            dr["PI"] = drShort["PI"];
                            dr["SC"] = drShort["SC"];
                            dr["RC"] = drShort["RC"];
                            dr["PIemail"] = drShort["PIemail"];

                            string primary = ((string)drTeam["Primary"]).ToLower() == "y" ? " Primary " : "";

                            drShort["OtherPersonnel"] += " & \"" + primary + drTeam["Role"] + " : " + drTeam["FirstName"] + " " + drTeam["LastName"] + " - " + drTeam["PrimaryEMailAddress"] + "  \" & CHAR(10) ";
                            dr["Cancer"] = drShort["Cancer"];
                            foreach (DataColumn dc in OutputStudy.fpstudys.data.Columns)
                            {
                                dr[dc.ColumnName] = string.IsNullOrWhiteSpace((string)drStudy[dc.ColumnName]) ? dr[dc.ColumnName] : drStudy[dc.ColumnName];
                            }
                            foreach (DataColumn dc in OutputTeam.fpTeam.data.Columns)
                            {
                                dr[dc.ColumnName] = string.IsNullOrWhiteSpace((string)drTeam[dc.ColumnName]) ? dr[dc.ColumnName] : drTeam[dc.ColumnName];
                            }


                            foreach (DataRow drStatus in latestStatus.Rows)
                            {
                                if ((string)drStudy["StudyId"] == (string)drStatus["StudyId"])
                                {
                                    foreach (DataColumn dc in latestStatus.Columns)
                                    {
                                        dr[dc.ColumnName] = string.IsNullOrWhiteSpace((string)drStatus[dc.ColumnName]) ? dr[dc.ColumnName] : drStatus[dc.ColumnName];
                                    }
                                }
                            }
                            studyDT.Rows.Add(dr);
                        }
                    }
                    studyShort.Rows.Add(drShort);
                }
            }
        }


        public static void getLatestStatus()
        {
            DataView dv = OutputStatus.fpstatus.data.DefaultView;
            dv.Sort = "StudyId desc, ValidOn asc";
            OutputStatus.fpstatus.data = dv.ToTable();

            latestStatus = OutputStatus.fpstatus.data.Clone();

            for (int i = 0; i < OutputStatus.fpstatus.data.Rows.Count - 1; i++)
            {
                if ((string)OutputStatus.fpstatus.data.Rows[i]["StudyId"] != (string)OutputStatus.fpstatus.data.Rows[i + 1]["StudyId"])
                {
                    latestStatus.Rows.Add(OutputStatus.fpstatus.data.Rows[i].ItemArray);
                }
            }

            if (OutputStatus.fpstatus.data.Rows.Count >= 2)
            {
                if (OutputStatus.fpstatus.data.Rows[OutputStatus.fpstatus.data.Rows.Count - 1]["StudyId"] != OutputStatus.fpstatus.data.Rows[OutputStatus.fpstatus.data.Rows.Count - 2]["StudyId"])
                {
                    latestStatus.Rows.Add(OutputStatus.fpstatus.data.Rows[OutputStatus.fpstatus.data.Rows.Count - 1].ItemArray);
                }
            }

            if (OutputStatus.fpstatus.data.Rows.Count == 1)
            {
                latestStatus.Rows.Add(OutputStatus.fpstatus.data.Rows[1].ItemArray);
            }
        }

    }
}
