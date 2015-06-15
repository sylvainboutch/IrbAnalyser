﻿using System;
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

                dr["IRB_Event"] = dr["IRB_Event"] + eventData + "&#13;&#10;";
            }
            else
            {
                DataRow dr = newIRBForm.NewRow();
                dr["IRB_Event"] = dr["IRB_Event"] + eventData + "&#13;&#10;";
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
                dr["Study_number"] = studyNumber;
                newIRBForm.Rows.Add(dr);
            }
        }
    }
}
