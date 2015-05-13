using System;
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
                

                newMSD.Columns.Add("IRB Agency name", typeof(string));
                newMSD.Columns.Add("IRB no", typeof(string));
                newMSD.Columns.Add("IRB Study ID", typeof(string));
                newMSD.Columns.Add("Study name", typeof(string));
                newMSD.Columns.Add("Label", typeof(string));
                newMSD.Columns.Add("Value", typeof(string));
            }

            if (updatedMSD.Columns.Count == 0)
            {
                updatedMSD.Columns.Add("IRB Agency name", typeof(string));
                updatedMSD.Columns.Add("IRB no", typeof(string));
                updatedMSD.Columns.Add("IRB Study ID", typeof(string));
                updatedMSD.Columns.Add("Study name", typeof(string));
                updatedMSD.Columns.Add("Label", typeof(string));
                updatedMSD.Columns.Add("Value", typeof(string));                
            }
        }

        public static void addRow(string label, string value, string studyid, string agency, string IRBno, bool newrecord)
        {
            initiate();
            DataRow dr;
            if (newrecord)
            { dr = newMSD.NewRow(); }
            else
            { dr = updatedMSD.NewRow(); }
            dr["Label"] = label;

            dr["IRB Agency name"] = agency;
            dr["IRB no"] = "";
            dr["IRB Study ID"] = studyid;
            dr["Study name"] = Tools.getStudyNumber(studyid, agency, IRBno);

            dr["Value"] = value;
            if (newrecord)
            { newMSD.Rows.Add(dr); }
            else
            { updatedMSD.Rows.Add(dr); }
        }
    }
}
