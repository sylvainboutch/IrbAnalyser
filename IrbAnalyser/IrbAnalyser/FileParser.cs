﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace IrbAnalyser
{
    public class FileParser
    {
        public DataTable data;
        public int initColumnCount = 0;

        public void reset()
        {
            data.Clear();
        }

        public FileParser(string file, type typ)
        {
            getDataTable(file, typ);
        }

        public FileParser()
        {
            data = new DataTable();
        }

        public enum type { Study, Team, Status, Event }

        private void getDataTable(string file, type typ)
        {
            data = new DataTable();

            switch (typ)
            {
                case type.Study:
                    data.Columns.Add("StudyId");
                    data.Columns.Add("StudySiteId");
                    data.Columns.Add("IRBNumber");
                    data.Columns.Add("IRBAgency");
                    data.Columns.Add("PI");
                    data.Columns.Add("PIMajorAuthor");
                    data.Columns.Add("RC");
                    data.Columns.Add("SC");
                    data.Columns.Add("CRO");
                    data.Columns.Add("SiteName");
                    data.Columns.Add("StudyTitle");
                    data.Columns.Add("StudySummary");
                    data.Columns.Add("StudyAcronym");
                    data.Columns.Add("Department");
                    data.Columns.Add("Division");
                    data.Columns.Add("Studysamplesize");
                    data.Columns.Add("SiteSampleSize");
                    data.Columns.Add("Multicenter");
                    data.Columns.Add("Phase");
                    data.Columns.Add("PrimarySponsorName");
                    data.Columns.Add("PrimarySponsorOther");
                    data.Columns.Add("PrimarySponsorContact");
                    data.Columns.Add("PrimarySponsorContactFirstName");
                    data.Columns.Add("PrimarySponsorContactLastName");
                    data.Columns.Add("PrimarySponsorContactEmail");
                    data.Columns.Add("PrimarySponsorStudyId");
                    data.Columns.Add("KeyWords");
                    data.Columns.Add("DocumentLink1");
                    data.Columns.Add("InitialApprovalDate");
                    data.Columns.Add("MostRecentApprovalDate");
                    data.Columns.Add("ExpirationDate");
                    data.Columns.Add("Cancer");
                    data.Columns.Add("ExternalIRB");
                    data.Columns.Add("ExternalIRBnumber");
                    data.Columns.Add("oldNumber");
                    data.Columns.Add("newNumber");

                    data.Columns.Add("Regulatory_coordinator");
                    data.Columns.Add("Principal Investigator");

                    data.Columns.Add("RecordCategory");

                    data.Columns.Add("HasConsentForm");

                    data.Columns.Add("AgentDevice");
                    //data.Columns.Add("Device");
                    //data.Columns.Add("Agent");

                    data.Columns.Add("ReviewType");

                    data.Columns.Add("Agent");
                    data.Columns.Add("Biological");
                    data.Columns.Add("BLood_Draw");
                    data.Columns.Add("Data_Collection");
                    data.Columns.Add("Device");
                    data.Columns.Add("EMERGENCY_INVESTIGATIONAL");
                    data.Columns.Add("HUMANITARIAN_USE");
                    data.Columns.Add("QI_STUDY");
                    data.Columns.Add("RETROSPECTIVE_CHART_REVIEW");
                    data.Columns.Add("Survey");
                    data.Columns.Add("TISSUE_BANKING");
                    data.Columns.Add("TRIALS_Involving_INTERVENTIONS");

                    data.Columns.Add("CT_FDA");
                    data.Columns.Add("CT_ICMJE");
                    data.Columns.Add("CT_NIH");  

                    data.Columns.Add("SpecimenDataAnalysis");


                    data.Columns.Add("NCT_NUMBER");

                    data.Columns.Add("IND_Holder");
                    data.Columns.Add("IND_NUMBERS");
                    data.Columns.Add("pk_study");
                    data.Columns.Add("FinancialBy");
                    data.Columns.Add("SignOffBy");

                    data.Columns.Add("PhaseDrugDevice");
                    
                    initColumnCount = data.Columns.Count;
                    break;
                case type.Status:
                    data.Columns.Add("StudyId");
                    data.Columns.Add("StudySiteId");
                    data.Columns.Add("IRBNumber");
                    data.Columns.Add("IRBAgency");
                    data.Columns.Add("SiteName");
                    data.Columns.Add("Status");
                    data.Columns.Add("ValidOn");
                    data.Columns.Add("Comments");
                    initColumnCount = data.Columns.Count;
                    break;
                case type.Team:
                    data.Columns.Add("StudyId");
                    data.Columns.Add("StudySiteId");
                    data.Columns.Add("IRBNumber");
                    data.Columns.Add("IRBAgency");
                    data.Columns.Add("SiteName");
                    data.Columns.Add("TeamMemberID");
                    data.Columns.Add("Primary");
                    data.Columns.Add("PrimaryEMailAddress");
                    data.Columns.Add("OtherEmailAdresses");
                    data.Columns.Add("FirstName");
                    data.Columns.Add("LastName");
                    data.Columns.Add("Role");
                    data.Columns.Add("UserName");
                    data.Columns.Add("Source");
                    data.Columns.Add("PK_STUDY");
                    data.Columns.Add("PK_USER");
                    data.Columns.Add("PK_STUDYTEAM");
                    initColumnCount = data.Columns.Count;
                    break;
                case type.Event:
                    data.Columns.Add("StudyId");
                    data.Columns.Add("StudySiteId");
                    data.Columns.Add("IRBNumber");
                    data.Columns.Add("IRBAgency");
                    data.Columns.Add("SiteName");
                    data.Columns.Add("EventId");
                    data.Columns.Add("Event");
                    data.Columns.Add("EventCreationDate");
                    data.Columns.Add("EventOutcome");
                    data.Columns.Add("TaskCompletionDate");
                    data.Columns.Add("EventCompletionDate");
                    data.Columns.Add("xForms");
                    initColumnCount = data.Columns.Count;
                    break;
            }

            /*foreach (DataRow dr in data.Rows)
            {
                foreach (DataColumn dc in data.Columns)
                {
                    dr[dc.ColumnName] = "";
                }
            }*/
            if (File.Exists(file))
            {
                var lines = File.ReadAllLines(file, Encoding.UTF8).ToList();
                if (lines.Count > 0)
                {
                    var columns = Tools.removeQuote(lines[0].Split((char)9));

                    lines.RemoveAt(0);

                    foreach (var column in columns)
                    {
                        if (!data.Columns.Contains(column))
                            data.Columns.Add(column);
                    }


                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            string[] linesplt = line.Split((char)9);
                            DataRow dr = data.NewRow();
                            foreach (DataColumn dc in data.Columns)
                            {
                                dr[dc.ColumnName] = "";
                            }
                            for (int i = 0; i < columns.Count(); i++)
                            {
                                if (!String.IsNullOrEmpty(columns[i]))
                                {
                                    if (columns[i] == "xForms")
                                    {
                                        dr[columns[i]] = linesplt[i];
                                        if (linesplt.Length > i)
                                        {
                                            dr[columns[i]] = String.Join(((char)9).ToString(), linesplt.Skip(i).Take(linesplt.Length - i).ToArray());
                                        }
                                    }
                                    else if (linesplt[i].Contains("�"))
                                    {
                                        dr[columns[i]] = Tools.parse(linesplt[i].Replace("�", "-"));
                                    } 
                                    else
                                    {
                                        dr[columns[i]] = Tools.parse((linesplt[i]));
                                    }
                                }
                            }

                            data.Rows.Add(dr);
                        }
                    }

                    if (typ == type.Event)
                    {
                        data.DefaultView.Sort = "StudyId desc, EventCreationDate desc";
                        data = data.DefaultView.ToTable();
                    }

                    if (typ == type.Status)
                    {
                        data.DefaultView.Sort = "StudyId desc, ValidOn desc";
                        data = data.DefaultView.ToTable();
                    }
                    if (typ == type.Team)
                    {
                        data.DefaultView.Sort = "StudyId desc, FirstName desc";
                        data = data.DefaultView.ToTable();
                    }
                    /*foreach (var title in column)
                    {
                        data.Columns.Add(title.ToString());
                    }*/
                    //lines.RemoveAt(0);
                    //lines.ForEach(line => data.Rows.Add(Tools.removeQuote(line.Split((char)9))));
                }
            }
        }
    }
}
