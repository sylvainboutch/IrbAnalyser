using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    public static class OutputIRBForm
    {
        public static DataTable newIRBForm = new DataTable();

        //All IRB Event will be reinserted, new line use  &#13;&#10;


        public static void initiate()
        {
            if (newIRBForm.Columns.Count == 0)
            {
                newIRBForm.Columns.Add("Study_number", typeof(string));
                newIRBForm.Columns.Add("Date", typeof(string));
                newIRBForm.Columns.Add("IRB_Identifier", typeof(string));
                newIRBForm.Columns.Add("IRB_Event", typeof(string));
                newIRBForm.Columns.Add("IRB_Status", typeof(string));
            }
        }

        public static void addEvents(string studyNumber, string eventData)
        {
            initiate();

            if ((from form in OutputIRBForm.newIRBForm.AsEnumerable()
                 where form.Field<string>("Study_number") == studyNumber
                 select form).Any())
            {
                DataRow dr = (from form in OutputIRBForm.newIRBForm.AsEnumerable()
                              where form.Field<string>("Study_number") == studyNumber
                              select form).First();
                if (!string.IsNullOrWhiteSpace(eventData) && ((string)dr["IRB_Event"]).Length < 3999)
                {
                    dr["IRB_Event"] = dr["IRB_Event"] + eventData + "&#13;&#10;";
                }
                if (((string)dr["IRB_Event"]).Length > 3999)
                {
                    dr["IRB_Event"] = ((string)dr["IRB_Event"]).Substring(0, 3999);
                }
            }
            else
            {
                DataRow dr = newIRBForm.NewRow();
                if (!string.IsNullOrWhiteSpace(eventData))
                {
                    dr["IRB_Event"] = dr["IRB_Event"] + eventData + "&#13;&#10;";
                }
                dr["IRB_Status"] = "";
                dr["IRB_Identifier"] = "";
                dr["Date"] = DateTime.Now.Date.ToString("MM/dd/yyyy");
                dr["Study_number"] = studyNumber;
                newIRBForm.Rows.Add(dr);
            }
        }


        public static void addStatus(string studyNumber, string statusdata)
        {
            initiate();

            if ((from form in OutputIRBForm.newIRBForm.AsEnumerable()
                 where form.Field<string>("Study_number") == studyNumber
                 select form).Any())
            {
                DataRow dr = (from form in OutputIRBForm.newIRBForm.AsEnumerable()
                              where form.Field<string>("Study_number") == studyNumber
                              select form).First();
                if (!string.IsNullOrWhiteSpace(statusdata) && ((string)dr["IRB_Status"]).Length < 3999)
                {
                    dr["IRB_Status"] = dr["IRB_Status"] + statusdata + "&#13;&#10;";
                }

                if (((string)dr["IRB_Status"]).Length > 3999)
                {
                    dr["IRB_Status"] = ((string)dr["IRB_Status"]).Substring(0, 3999);
                }
            }
            else
            {
                DataRow dr = newIRBForm.NewRow();
                if (!string.IsNullOrWhiteSpace(statusdata))
                {
                    dr["IRB_Status"] = dr["IRB_Status"] + statusdata + "&#13;&#10;";
                }
                dr["IRB_Event"] = "";
                dr["IRB_Identifier"] = "";
                dr["Date"] = DateTime.Now.Date.ToString("MM/dd/yyyy");
                dr["Study_number"] = studyNumber;
                newIRBForm.Rows.Add(dr);
            }
        }


        public static void addIds(string studyNumber, string IRBIds)
        {
            initiate();

            if ((from form in OutputIRBForm.newIRBForm.AsEnumerable()
                 where form.Field<string>("Study_number") == studyNumber
                 select form).Any())
            {
                DataRow dr = (from form in OutputIRBForm.newIRBForm.AsEnumerable()
                              where form.Field<string>("Study_number") == studyNumber
                              select form).First();

                dr["IRB_Identifier"] = IRBIds;
            }
            else
            {
                DataRow dr = newIRBForm.NewRow();
                dr["IRB_Identifier"] = IRBIds;
                dr["Date"] = DateTime.Now.Date.ToString("MM/dd/yyyy");
                dr["Study_number"] = studyNumber;
                dr["IRB_Status"] = "";
                dr["IRB_Event"] = "";
                newIRBForm.Rows.Add(dr);
            }
        }

        public static void finalizeEventIrbForm()
        {

            IEnumerable<Model.LCL_V_IRBFORM> irbforms;
            using (Model.VelosDb db = new Model.VelosDb())
            {

                var queryrslt = from forms in db.LCL_V_IRBFORM
                                where forms.IRBIDENTIFIERS != null
                                select forms;
                irbforms = queryrslt.ToList<Model.LCL_V_IRBFORM>();
            }


            List<DataRow> rowsToDelete = new List<DataRow>();
            foreach (DataRow dr in OutputIRBForm.newIRBForm.Rows)
            {
                bool deleteEvent = false;
                bool deleteStatus = false;

                Model.LCL_V_IRBFORM data = (from irbform in irbforms
                          where irbform.IRBIDENTIFIERS == (string)dr["IRB_Identifier"]
                          select irbform).FirstOrDefault();

                if (data != null)
                {
                    if (data.IRBEVENTS != null && data.IRBEVENTS == (string)dr["IRB_Event"])
                    {
                        deleteEvent = true;
                    }
                    else if (string.IsNullOrWhiteSpace((string)dr["IRB_Event"]) && (data.IRBEVENTS == null || data.IRBEVENTS == ""))
                    {
                        deleteEvent = true;
                    }

                    if (data.IRBSTATUS != null && data.IRBSTATUS == (string)dr["IRB_Status"])
                    {
                        deleteStatus = true;
                    }
                    else if (string.IsNullOrWhiteSpace((string)dr["IRB_Status"]) && (data.IRBSTATUS == null || data.IRBSTATUS == ""))
                    {
                        deleteStatus = true;
                    }

                    if (deleteEvent && deleteStatus)
                    {
                        rowsToDelete.Add(dr);
                        //dr.Delete();
                        //toremove.Add((string)dr["IRB_Identifier"]);
                        //OutputIRBForm.newIRBForm.Rows.Remove(dr);
                    }
                }
            }

            foreach (DataRow row in rowsToDelete)
            {
                OutputIRBForm.newIRBForm.Rows.Remove(row);
            }

            //OutputIRBForm.newIRBForm.AcceptChanges();

        }


    }
}
