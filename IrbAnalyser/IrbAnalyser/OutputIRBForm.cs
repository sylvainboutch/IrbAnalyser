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
                if (!string.IsNullOrWhiteSpace(eventData))
                {
                    dr["IRB_Event"] = dr["IRB_Event"] + eventData + "&#13;&#10;";
                }
            }
            else
            {
                DataRow dr = newIRBForm.NewRow();
                if (!string.IsNullOrWhiteSpace(eventData))
                {
                    dr["IRB_Event"] = dr["IRB_Event"] + eventData + "&#13;&#10;";
                }
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
                if (!string.IsNullOrWhiteSpace(statusdata))
                {
                    dr["IRB_Status"] = dr["IRB_Status"] + statusdata + "&#13;&#10;";
                }
            }
            else
            {
                DataRow dr = newIRBForm.NewRow();
                if (!string.IsNullOrWhiteSpace(statusdata))
                {
                    dr["IRB_Status"] = dr["IRB_Status"] + statusdata + "&#13;&#10;";
                }
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


            IEnumerable<Model.LCL_V_IRBFORM> irbformsNoNull;
            using (Model.VelosDb db = new Model.VelosDb())
            {

                var queryrslt = from forms in db.LCL_V_IRBFORM
                                where forms.IRBIDENTIFIERS != null
                                && forms.IRBEVENTS != null
                                select forms;
                irbformsNoNull = queryrslt.ToList<Model.LCL_V_IRBFORM>();
            }



            foreach (DataRow dr in OutputIRBForm.newIRBForm.Rows)
            {
                bool delete = false;
                if (string.IsNullOrWhiteSpace((string)dr["IRB_Identifier"]))
                {
                    delete = (from irbform in irbforms
                              where irbform.IRBEVENTS == null
                            && irbform.IRBIDENTIFIERS == (string)dr["IRB_Identifier"]
                              select irbform).Any();

                    if (!delete)
                    {
                        delete = (from irbform in irbformsNoNull
                                  where irbform.IRBEVENTS == (string)dr["IRB_Event"]
                                  && irbform.IRBSTATUS == (string)dr["IRB_Status"]
                                 && irbform.IRBIDENTIFIERS == (string)dr["IRB_Identifier"]
                                  select irbform).Any();
                    }
                }
                else
                {
                    delete = (from irbform in irbformsNoNull
                              where irbform.IRBEVENTS == (string)dr["IRB_Event"]
                              && irbform.IRBSTATUS == (string)dr["IRB_Status"]
                             && irbform.IRBIDENTIFIERS == (string)dr["IRB_Identifier"]
                              select irbform).Any();
                }
                if (delete)
                {
                    OutputIRBForm.newIRBForm.Rows.Remove(dr);
                }
            }
        
        }


    }
}
