using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;

namespace IrbAnalyser
{
    public class XmlTools
    {

        public XmlTools()
        {

        }

        public bool AnalyseXML()
        {
            //string xmlString
            //XmlDocument doc = new XmlDocument();
            /*List<string> questions = new List<string>();*/

            DataTable dt = new DataTable();
            List<Tuple<string, string>> fields = new List<Tuple<string, string>>();

            fields.Add(Tuple.Create("XF", "FormName"));
            fields.Add(Tuple.Create("XF", "FormGUID"));
            fields.Add(Tuple.Create("XF", "FormVersionGUID"));

            fields.Add(Tuple.Create("XFS", "StageName"));
            fields.Add(Tuple.Create("XFS", "StageVIG"));

            fields.Add(Tuple.Create("XFPG", "PageGroup"));

            fields.Add(Tuple.Create("XFP", "PageTitle"));
            fields.Add(Tuple.Create("XFP", "FormPageGUID"));

            fields.Add(Tuple.Create("XFPQ", "QuestionLabel"));
            fields.Add(Tuple.Create("XFPQ", "QuestionVIG"));
            fields.Add(Tuple.Create("XFIR", "RepeatingGroupInstance"));
            fields.Add(Tuple.Create("XFIR", "Response"));

            fields.Add(Tuple.Create("XFI", "FormInstanceGUID"));
            fields.Add(Tuple.Create("XFI", "OwningRowGUID"));
            fields.Add(Tuple.Create("XFI", "Status"));
            fields.Add(Tuple.Create("XFI", "StartedAt"));
            fields.Add(Tuple.Create("XFI", "CompletedAt"));

            string saveField = "XFIR";

            foreach (var tu in fields)
            {
                dt.Columns.Add(tu.Item2);
            }

            /*
            dt.Columns.Add("FormName");
            dt.Columns.Add("FormGUID");
            dt.Columns.Add("FormVersionGUID");

            dt.Columns.Add("StageName");
            dt.Columns.Add("StageVIG");

            dt.Columns.Add("PageGroup");

            dt.Columns.Add("PageTitle");
            dt.Columns.Add("FormPageGUID");
            
            dt.Columns.Add("QuestionLabel");
            dt.Columns.Add("QuestionVIG");
            dt.Columns.Add("RepeatingGroupInstance");
            dt.Columns.Add("Response");

            dt.Columns.Add("FormInstanceGUID");
            dt.Columns.Add("OwningRowGUID");
            dt.Columns.Add("Status");
            dt.Columns.Add("StartedAt");
            dt.Columns.Add("CompletedAt");
            */

            /*using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                //doc.Load(reader);
                //foreach (var element in doc.
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        questions.Add(reader.LocalName);
                    }
                }

                File.WriteAllLines("C:\\Users\\sbouchar\\Dropbox\\Work\\IRB_Interface\\mapping\\questions.txt", questions.ToArray());
            }*/
            XmlTextReader reader = new XmlTextReader("C:\\Users\\sbouchar\\Dropbox\\Work\\IRB_Interface\\mapping\\XFORM_Submission.xml");

            string[] values = new string[fields.Count];
            /*
            string PageGroup = "";
            string PageTitle = "";
            string QuestionLabel = "";
            string QuestionVIG = "";
            string RepeatingGroupInstance = "";
            string Response = "";
            */
            DataRow dr = dt.NewRow();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    /*if (reader.LocalName == "XFPG")
                    {
                        PageGroup = reader.HasAttributes ? reader.GetAttribute(0) : "";
                    }
                    if (reader.LocalName == "XFP")
                    {
                        PageTitle = reader.HasAttributes ? reader.GetAttribute(0) : "";
                    }
                    if (reader.LocalName == "XFPQ")
                    {
                        QuestionLabel = reader.HasAttributes ? reader.GetAttribute(0) : "";
                        QuestionVIG = reader.HasAttributes ? reader.GetAttribute(1) : "";
                    }
                    if (reader.LocalName == "XFIR")
                    {
                        RepeatingGroupInstance = reader.HasAttributes ? reader.GetAttribute(0) : "";
                        Response = reader.HasAttributes ? reader.GetAttribute(1) : "";

                        DataRow dr = dt.NewRow();
                        dr["PageGroup"] = PageGroup;
                        dr["PageTitle"] = PageTitle;
                        dr["QuestionLabel"] = QuestionLabel;
                        dr["QuestionVIG"] = QuestionVIG;
                        dr["RepeatingGroupInstance"] = RepeatingGroupInstance;
                        dr["Response"] = Response;
                        dt.Rows.Add(dr);
                    }*/

                    List<Tuple<string, string>> currentFields =  fields.FindAll(x => x.Item1 == reader.LocalName);
                    for (int i = 0; i < currentFields.Count; i++ )
                    {
                        dr[currentFields[i].Item2] = reader.HasAttributes ? reader.GetAttribute(i) : "";
                    }

                    if (reader.LocalName == saveField)
                    {
                        dt.Rows.Add(dr);
                        var itemarray = dr.ItemArray;
                        dr = dt.NewRow();
                        dr.ItemArray = itemarray;
                    }
                    


                    //questions.Add(reader.LocalName);
                }
            }

            ExcelUtility exc = new ExcelUtility();
            List<ExcelWorksheet> lstxls = new List<ExcelWorksheet>();

            lstxls.Add(new ExcelWorksheet("Submission", "Submission XFORM", dt));
            exc.WriteDataTableToExcel("C:\\Users\\sbouchar\\Dropbox\\Work\\IRB_Interface\\mapping\\XFORM_Submission.xlsx", lstxls);
            //File.WriteAllLines("C:\\Users\\sbouchar\\Dropbox\\Work\\IRB_Interface\\mapping\\questions.txt", questions.ToArray());

            return true;
        }

        public DataTable AnalyseXmlString(string xmlString, DataTable dt)
        {
            if (String.IsNullOrWhiteSpace(xmlString))
            {
                return dt;
            }
            //DataTable dt = new DataTable();
            List<Tuple<string, string>> fields = new List<Tuple<string, string>>();

            fields.Add(Tuple.Create("XF", "FormName"));
            fields.Add(Tuple.Create("XF", "FormGUID"));
            fields.Add(Tuple.Create("XF", "FormVersionGUID"));

            fields.Add(Tuple.Create("XFS", "StageName"));
            fields.Add(Tuple.Create("XFS", "StageVIG"));

            fields.Add(Tuple.Create("XFPG", "PageGroup"));

            fields.Add(Tuple.Create("XFP", "PageTitle"));
            fields.Add(Tuple.Create("XFP", "FormPageGUID"));

            fields.Add(Tuple.Create("XFPQ", "QuestionLabel"));
            fields.Add(Tuple.Create("XFPQ", "QuestionVIG"));
            fields.Add(Tuple.Create("XFIR", "RepeatingGroupInstance"));
            fields.Add(Tuple.Create("XFIR", "Response"));

            fields.Add(Tuple.Create("XFI", "FormInstanceGUID"));
            fields.Add(Tuple.Create("XFI", "OwningRowGUID"));
            fields.Add(Tuple.Create("XFI", "Status"));
            fields.Add(Tuple.Create("XFI", "StartedAt"));
            fields.Add(Tuple.Create("XFI", "CompletedAt"));

            string saveField = "XFIR";

            foreach (var tu in fields)
            {
                if (!dt.Columns.Contains(tu.Item2))
                {
                    dt.Columns.Add(tu.Item2);
                }
            }

            XmlReader reader = XmlReader.Create(new StringReader(xmlString));
            

            string[] values = new string[fields.Count];

            DataRow dr = dt.NewRow();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    List<Tuple<string, string>> currentFields = fields.FindAll(x => x.Item1 == reader.LocalName);
                    for (int i = 0; i < currentFields.Count; i++)
                    {
                        dr[currentFields[i].Item2] = reader.HasAttributes ? reader.GetAttribute(i) : "";
                    }

                    if (reader.LocalName == saveField)
                    {
                        dt.Rows.Add(dr);
                        var itemarray = dr.ItemArray;
                        dr = dt.NewRow();
                        dr.ItemArray = itemarray;
                    }
                }
            }

            return dt;
        }


    }
}
