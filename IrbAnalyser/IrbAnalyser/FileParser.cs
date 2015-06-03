using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace IrbAnalyser
{
    class FileParser
    {
        public DataTable data;

        public FileParser(string file, type typ)
        {
            getDataTable(file, typ);
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
                    data.Columns.Add("Sitename");
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
                    data.Columns.Add("PrimarySponsorContactFirstName");
                    data.Columns.Add("PrimarySponsorContactLastName");
                    data.Columns.Add("PrimarySponsorContactEmail");
                    data.Columns.Add("PrimarySponsorStudyId");
                    data.Columns.Add("DocumentLink");
                    data.Columns.Add("InitialApprovalDate");
                    data.Columns.Add("MostRecentApprovalDate");
                    data.Columns.Add("ExpirationDate");
                    break;
                case type.Status:
                    data.Columns.Add("StudyId");
                    data.Columns.Add("StudySiteId");
                    data.Columns.Add("IRBNumber");
                    data.Columns.Add("SiteName");
                    data.Columns.Add("Status");
                    data.Columns.Add("ValidOn");
                    break;
                case type.Team:
                    data.Columns.Add("StudyId");
                    data.Columns.Add("StudySiteId");
                    data.Columns.Add("IRBNumber");
                    data.Columns.Add("SiteName");
                    data.Columns.Add("TeamMemberID");
                    data.Columns.Add("Primary");
                    data.Columns.Add("PrimaryEmailAdress");
                    data.Columns.Add("OtherEmailAdresses");
                    data.Columns.Add("FirstName");
                    data.Columns.Add("LastNAme");
                    data.Columns.Add("Role");
                    break;
                case type.Event:
                    data.Columns.Add("StudyId");
                    data.Columns.Add("StudySiteId");
                    data.Columns.Add("IRBNumber");
                    data.Columns.Add("SiteName");
                    data.Columns.Add("EventId");
                    data.Columns.Add("Event");
                    data.Columns.Add("EventCreationDate");
                    data.Columns.Add("EventOutcome");
                    data.Columns.Add("TaskCompletionDate");
                    data.Columns.Add("EventCompletionDate");
                    break;
            }

            /*foreach (DataRow dr in data.Rows)
            {
                foreach (DataColumn dc in data.Columns)
                {
                    dr[dc.ColumnName] = "";
                }
            }*/

            var lines = File.ReadAllLines(file).ToList();
            if (lines.Count > 0)
            {
                var columns = Tools.removeQuote(lines[0].Split((char)9));

                lines.RemoveAt(0);

                foreach (string line in lines)
                {
                    string[] linesplt = line.Split((char)9);
                    DataRow dr = data.NewRow();
                    foreach (DataColumn dc in data.Columns)
                    {
                        dr[dc.ColumnName] = "";
                    }
                    for (int i=0;i<columns.Count();i++)
                    {
                        if (!String.IsNullOrEmpty(columns[i]))
                            dr[columns[i]] = Tools.removeQuote((linesplt[i]));
                    }
                    data.Rows.Add(dr);
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
