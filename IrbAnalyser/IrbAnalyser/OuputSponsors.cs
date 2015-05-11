using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    /*class Sponsors
    {
        public string name { get; set; }
        public string contactName { get; set; }
        public string contactPhone { get; set; }
        public string contactEmail { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
    }*/

    class OuputSponsors
    {
        public static DataTable sponsors = new DataTable();

        private static void initiate()
        {
            if (sponsors.Columns.Count == 0)
            {
                sponsors.Columns.Add("TYPE", typeof(string));
                sponsors.Columns.Add("IRB Agency name", typeof(string));
                sponsors.Columns.Add("IRB no", typeof(string));
                sponsors.Columns.Add("IRB Study ID", typeof(string));
                sponsors.Columns.Add("Study name", typeof(string));

                sponsors.Columns.Add("Organization", typeof(string));

                sponsors.Columns.Add("Name", typeof(string));
                sponsors.Columns.Add("Contact Name", typeof(string));
                sponsors.Columns.Add("Contact Phone", typeof(string));
                sponsors.Columns.Add("Contact Email", typeof(string));
                sponsors.Columns.Add("City", typeof(string));
                sponsors.Columns.Add("State", typeof(string));
                sponsors.Columns.Add("Postal Code", typeof(string));
                sponsors.Columns.Add("Country", typeof(string));

                //sponsors.Columns.Add("Sponsors", typeof(List<Sponsors>));
            }
        }

        private static void addRow(DataRow row, string type)
        {
            initiate();

            DataRow newrow = sponsors.NewRow();

            newrow["TYPE"] = type;

            newrow["IRB Agency name"] = (string)row["IRBAgency"];
            newrow["IRB no"] = (string)row["IRBNumber"];
            newrow["IRB Study ID"] = (string)row["StudyId"];
            newrow["Study name"] = Tools.getStudyNumber((string)row["StudyId"], (string)row["IRBAgency"], (string)row["IRBNumber"]);


            newrow["Name"] = (string)row["SponsorName"];
            newrow["Contact Name"] = (string)row["SponsorContactFirstName"] + " " + (string)row["SponsorContactLastName"];
            newrow["Contact Phone"] = (string)row["SponsorContactPhone"];
            newrow["Contact Email"] = (string)row["SponsorContactEmail"];
            newrow["City"] = (string)row["SponsorCity"];
            newrow["State"] = (string)row["SponsorState"];
            newrow["Postal Code"] = (string)row["SponsorPostalCode"];
            newrow["Country"] = (string)row["SponsorCountry"];

            sponsors.Rows.Add(newrow);
        }

        /// <summary>
        /// Analyse the sponsors file
        /// </summary>
        /// <param name="team"></param>
        public static void analyse(string filename)
        {
            initiate();

            FileParser fpSponsors = new FileParser(filename);

            foreach (DataRow sponsor in fpSponsors.data.Rows)
            {
                analyseRow(sponsor);
            }

            analyseDelete(fpSponsors.data);
        }

        public static void analyseRow(DataRow row)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {
                string irbstudyId = (string)row["StudyId"];
                string irbagency = ((string)row["IRBAgency"]).ToLower();
                string name = (string)row["SponsorName"];


                var sponsorsCount = (from form in db.ER_FORMSLINEAR
                                     join studform in db.ER_STUDYFORMS on form.FK_FILLEDFORM equals studform.PK_STUDYFORMS
                                     join stud in db.LCL_V_STUDYSUMM_PLUSMORE on studform.FK_STUDY equals stud.PK_STUDY
                                     where stud.MORE_IRBSTUDYID == irbstudyId
                                    && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                     select form).Count();

                var sponsors = from form in db.ER_FORMSLINEAR
                               join studform in db.ER_STUDYFORMS on form.FK_FILLEDFORM equals studform.PK_STUDYFORMS
                               join stud in db.LCL_V_STUDYSUMM_PLUSMORE on studform.FK_STUDY equals stud.PK_STUDY
                               where stud.MORE_IRBSTUDYID == irbstudyId
                              && stud.MORE_IRBAGENCY.ToLower() == irbagency
                              && (
                              form.COL6.ToLower() == name.ToLower() ||
                              form.COL30.ToLower() == name.ToLower() ||
                              form.COL52.ToLower() == name.ToLower() ||
                              form.COL76.ToLower() == name.ToLower() ||
                              form.COL98.ToLower() == name.ToLower()
                              )
                               select form;
                if (!sponsors.Any() && sponsorsCount < 5)
                {
                    addRow(row, "New sponsor");
                }
                else
                {
                    var changed = false;
                    if (Tools.compareStr(sponsors.First().COL7, (string)row["SponsorContactFirstName"] + " " + (string)row["SponsorContactLastName"]) ||
                        Tools.compareStr(sponsors.First().COL31, (string)row["SponsorContactFirstName"] + " " + (string)row["SponsorContactLastName"]) ||
                        Tools.compareStr(sponsors.First().COL53, (string)row["SponsorContactFirstName"] + " " + (string)row["SponsorContactLastName"]) ||
                        Tools.compareStr(sponsors.First().COL77, (string)row["SponsorContactFirstName"] + " " + (string)row["SponsorContactLastName"]) ||
                        Tools.compareStr(sponsors.First().COL99, (string)row["SponsorContactFirstName"] + " " + (string)row["SponsorContactLastName"])
                        )
                    {
                        row["SponsorContactFirstName"] = "";
                        row["SponsorContactLastName"] = "";
                    }
                    else { changed = true; }

                    if (Tools.compareStr(sponsors.First().COL8, (string)row["SponsorContactEmail"]) ||
                        Tools.compareStr(sponsors.First().COL32, (string)row["SponsorContactEmail"]) ||
                        Tools.compareStr(sponsors.First().COL54, (string)row["SponsorContactEmail"]) ||
                        Tools.compareStr(sponsors.First().COL78, (string)row["SponsorContactEmail"]) ||
                        Tools.compareStr(sponsors.First().COL100, (string)row["SponsorContactEmail"])
                        )
                    {
                        row["SponsorContactEmail"] = "";
                    }
                    else { changed = true; }

                    if (Tools.compareStr(sponsors.First().COL9, (string)row["SponsorContactPhone"]) ||
                        Tools.compareStr(sponsors.First().COL33, (string)row["SponsorContactPhone"]) ||
                        Tools.compareStr(sponsors.First().COL55, (string)row["SponsorContactPhone"]) ||
                        Tools.compareStr(sponsors.First().COL79, (string)row["SponsorContactPhone"]) ||
                        Tools.compareStr(sponsors.First().COL101, (string)row["SponsorContactPhone"])
                        )
                    {
                        row["SponsorContactPhone"] = "";
                    }
                    else { changed = true; }

                    if (Tools.compareStr(sponsors.First().COL17, (string)row["SponsorCity"]) ||
                        Tools.compareStr(sponsors.First().COL41, (string)row["SponsorCity"]) ||
                        Tools.compareStr(sponsors.First().COL63, (string)row["SponsorCity"]) ||
                        Tools.compareStr(sponsors.First().COL87, (string)row["SponsorCity"]) ||
                        Tools.compareStr(sponsors.First().COL109, (string)row["SponsorCity"])
                        )
                    {
                        row["SponsorCity"] = "";
                    }
                    else { changed = true; }

                    if (Tools.compareStr(sponsors.First().COL18, (string)row["SponsorState"]) ||
                        Tools.compareStr(sponsors.First().COL42, (string)row["SponsorState"]) ||
                        Tools.compareStr(sponsors.First().COL64, (string)row["SponsorState"]) ||
                        Tools.compareStr(sponsors.First().COL88, (string)row["SponsorState"]) ||
                        Tools.compareStr(sponsors.First().COL110, (string)row["SponsorState"])
                        )
                    {
                        row["SponsorState"] = "";
                    }
                    else { changed = true; }

                    if (Tools.compareStr(sponsors.First().COL19, (string)row["SponsorPostalCode"]) ||
                        Tools.compareStr(sponsors.First().COL43, (string)row["SponsorPostalCode"]) ||
                        Tools.compareStr(sponsors.First().COL65, (string)row["SponsorPostalCode"]) ||
                        Tools.compareStr(sponsors.First().COL89, (string)row["SponsorPostalCode"]) ||
                        Tools.compareStr(sponsors.First().COL111, (string)row["SponsorPostalCode"])
                        )
                    {
                        row["SponsorPostalCode"] = "";
                    }
                    else { changed = true; }

                    if (Tools.compareStr(sponsors.First().COL20, (string)row["SponsorCountry"]) ||
                        Tools.compareStr(sponsors.First().COL44, (string)row["SponsorCountry"]) ||
                        Tools.compareStr(sponsors.First().COL66, (string)row["SponsorCountry"]) ||
                        Tools.compareStr(sponsors.First().COL90, (string)row["SponsorCountry"]) ||
                        Tools.compareStr(sponsors.First().COL112, (string)row["SponsorCountry"])
                        )
                    {
                        row["SponsorCountry"] = "";
                    }
                    else { changed = true; }

                    if (changed) { addRow(row, "Modified sponsor"); }
                }
            }
        }

        /// <summary>
        /// RULES : 
        /// </summary>
        /// <param name="table"></param>
        public static void analyseDelete(DataTable table)
        {
            //get all study id in table then get all sponsors in DB where ID is there
            var std = from rw in table.AsEnumerable()
                      select rw.Field<string>("StudyId");

            foreach (DataRow row in table.Rows)
            {
                using (Model.VelosDb db = new Model.VelosDb())
                {
                    string irbstudyId = (string)row["StudyId"];
                    string irbagency = ((string)row["IRBAgency"]).ToLower();
                    string name = (string)row["SponsorName"];

                    var sponsors = from form in db.ER_FORMSLINEAR
                                   join studform in db.ER_STUDYFORMS on form.FK_FILLEDFORM equals studform.PK_STUDYFORMS
                                   join stud in db.LCL_V_STUDYSUMM_PLUSMORE on studform.FK_STUDY equals stud.PK_STUDY
                                   where stud.MORE_IRBSTUDYID == irbstudyId
                                  && stud.MORE_IRBAGENCY.ToLower() == irbagency
                                  && (
                                  form.COL6.ToLower() == name.ToLower() ||
                                  form.COL30.ToLower() == name.ToLower() ||
                                  form.COL52.ToLower() == name.ToLower() ||
                                  form.COL76.ToLower() == name.ToLower() ||
                                  form.COL98.ToLower() == name.ToLower()
                                  )
                                   select form;
                }
            }
        }

    }


}
