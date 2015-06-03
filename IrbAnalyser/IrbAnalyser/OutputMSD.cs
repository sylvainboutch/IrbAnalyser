﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
    public class OutputMSD
    {
        public static DataTable newMSD = new DataTable();
        public static DataTable updatedMSD = new DataTable();

        public static void initiate()
        {
            if (newMSD.Columns.Count == 0)
            {               
                newMSD.Columns.Add("Study number", typeof(string));
                newMSD.Columns.Add("Label", typeof(string));
                newMSD.Columns.Add("Value", typeof(string));
            }

            if (updatedMSD.Columns.Count == 0)
            {
                updatedMSD.Columns.Add("Study number", typeof(string));
                updatedMSD.Columns.Add("Label", typeof(string));
                updatedMSD.Columns.Add("Value", typeof(string));                
            }
        }

        public static void addRow(string label, string value, string studyid, string IRBno, string accronym, bool newrecord)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            { dr = newMSD.NewRow(); }
            else
            { dr = updatedMSD.NewRow(); }
            dr["Label"] = label;

            dr["Study number"] = Tools.getStudyNumber(studyid, IRBno, accronym);

            dr["Value"] = value;
            if (newrecord)
            { newMSD.Rows.Add(dr); }
            else
            { updatedMSD.Rows.Add(dr); }
        }
    }
}
