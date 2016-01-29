using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Xml.Linq;

namespace IrbAnalyser
{
    public class XmlTools
    {

        private string submissionFormEvent = "New Application for IRB Review";
        private string[] modificationFormEvents = new string[] {  
            "Amendment",
            "Administrative Review",
            "Administrative - Translation",
            "1572 Modification",
            "Personnel Change",
            "xForm Request Form Only",
            "Advertisements",
            "Full Board Reconsideration"
        };

        private Tuple<string, string> xmlQuestion = Tuple.Create("XFPQ", "QuestionVIG");
        private Tuple<string, string> xmlAnswer = Tuple.Create("XFIR", "Response");
        private string xmlAnswerNumber = "RepeatingGroupInstance";


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



        public void fillDataRow(DataRow dr)
        {
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                string type = getStudyType((string)dr["StudyId"]);
                switch (type)
                {
                    case "Blood Draw":
                        dr["BLood_Draw"] = "Y";
                        break;
                    case "Drug/Biologic or New Use of Drug/Biologic":
                        dr["Agent"] = "Y";
                        break;
                    case "Retrospective Chart Review":
                        dr["RETROSPECTIVE_CHART_REVIEW"] = "Y";
                        break;
                    case "Biological Specimen Research":
                        dr["Biological"] = "Y";
                        break;
                    case "Combination Product":
                        //dr["Agent"] = "Y";
                        //dr["Device"] = "Y";
                        dr["AgentDevice"] = "Please Specify";
                        break;
                    case "Data Collection during routine clinical care":
                        dr["Data_Collection"] = "Y";
                        break;
                    case "Survey Study":
                        dr["Survey"] = "Y";
                        break;
                    case "Device":
                        dr["Device"] = "Y";
                        break;
                    case "Combination Product (Drug/Device, Drug/Biologic, Device/Biologic, Drug/Device/Biologic, co-packaged test articles, 2 products separately packaged but labeled for use together)":
                        //dr["Agent"] = "Y";
                        //dr["Device"] = "Y";
                        dr["AgentDevice"] = "Please Specify";
                        break;
                }
                dr["Studysamplesize"] = getValue((string)dr["StudyId"], "B1259520-1DCD-41FE-A62B-0C2516D3C4F1");
                dr["StudySummary"] = getValue((string)dr["StudyId"], "E1077C72-26E4-427C-A47B-97D2EFD29D0C");
                        

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public string getStudyType(string studyId)
        {
            return getValue(studyId, "C1D6AD85-CFEF-471C-ABE1-E87D66817DB8");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public DataTable getAllPersonnal(string studyId)
        {
            DataTable personnal = new DataTable();
            //KP
            string vigName = "0A22BA93-DCDD-4E24-BC8C-2E15DEF7DB8A";
            string vigRole = "433DA8A3-5F34-4B32-AFC9-A26E07F605AA";
            string vigEmail = "4982BAF6-25C0-4F36-8303-0EF23CC40B83";
            string[] submission = new string[] { submissionFormEvent };
            personnal = getValuePersonnelDataTable(studyId, vigName, vigEmail, vigRole, personnal, submission);

            //Regulatory Personnel Information
            vigName = "63ACBAE3-A690-4B7B-9E0B-E41EDE217F15";
            vigRole = "";
            vigEmail = "A8F10A12-F2AF-466F-B937-281C75FB36DB";

            personnal = getValuePersonnelDataTable(studyId, vigName, vigEmail, vigRole, personnal, submission);

            //Add key personnel
            vigName = "56918BF3-7EB6-4DD2-BE5C-C5A8AADEE86D";
            vigRole = "FCD3E85F-4EED-4D6A-925B-12EC7423EA33";
            vigEmail = "8FC0DA63-E6BC-4D44-8AF9-4850E48EBE5E";

            personnal = getValuePersonnelDataTable(studyId, vigName, vigEmail, vigRole, personnal, submission);

            //remove KP
            vigName = "173251E9-E0C7-4C3A-93AD-8EAC85E54083";

            personnal = removePersonnel(studyId, vigName, personnal, submission);

            return personnal;
        }


        private DataTable removePersonnel(string studyId, string vigName, DataTable personnal, string[] formEvents)
        {
            var eventRecords = OutputStatus.fpevent.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId && formEvents.Contains((string)x["Event"])).AsEnumerable();

            string name = "";


            string xmlString = "";

            if (formEvents.Count() == 1)
            {
                eventRecords = eventRecords.OrderByDescending(x => x.Field<string>("EventCreationDate")).Take(1);
            }
            else if (formEvents.Count() >= 1)
            {
                eventRecords = eventRecords.OrderBy(x => x.Field<string>("EventCreationDate"));
            }

            foreach (var eventRecord in eventRecords)
            {
                if (eventRecord != null)
                {
                    xmlString = eventRecord.Field<string>("xForms");
                    if (!String.IsNullOrWhiteSpace(xmlString))
                    {
                        XDocument xdocument = XDocument.Parse(xmlString);
                        IEnumerable<XElement> document = xdocument.Elements();

                        var persons = from d in document.Descendants(xmlQuestion.Item1)
                                      where d.Attribute(xmlQuestion.Item2).Value.Equals(vigName)
                                      from q in d.Elements(xmlAnswer.Item1)
                                      select q;

                        foreach (var p in persons)
                        {
                            name = p.Attribute(xmlAnswer.Item2).Value;

                            var rows = (from DataRow dr in personnal.Rows
                                        where ((string)dr["name"]).Trim().ToLower() == name.ToLower().Trim()
                                        select dr);

                            foreach (var row in rows)
                            {
                                personnal.Rows.Remove(row);
                            }
                        }
                    }
                }
            }




            return personnal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studyId"></param>
        /// <param name="vigName"></param>
        /// <param name="vigEmail"></param>
        /// <param name="vigRole"></param>
        /// <param name="personnal"></param>
        /// <returns></returns>
        private DataTable getValuePersonnelDataTable(string studyId, string vigName, string vigEmail, string vigRole, DataTable personnal, string[] formEvents)
        {
            if (!personnal.Columns.Contains("Name"))
            {
                personnal.Columns.Add("Name");
            }
            if (!personnal.Columns.Contains("Email"))
            {
                personnal.Columns.Add("Email");
            }
            if (!personnal.Columns.Contains("Role"))
            {
                personnal.Columns.Add("Role");
            }

            string name = "";
            string email = "";
            string role = "";
            string id = "";

                        
            string xmlString = "";
            

            var eventRecords = OutputStatus.fpevent.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId && formEvents.Contains((string)x["Event"])).AsEnumerable();

            if (formEvents.Count() == 1)
            {
                eventRecords = eventRecords.OrderByDescending(x => x.Field<string>("EventCreationDate")).Take(1);
            }
            else if (formEvents.Count() >= 1)
            {
                eventRecords = eventRecords.OrderBy(x => x.Field<string>("EventCreationDate"));
            }

            foreach (var eventRecord in eventRecords)
            {
                if (eventRecord != null)
                {
                    xmlString = eventRecord.Field<string>("xForms");
                    if (!String.IsNullOrWhiteSpace(xmlString))
                    {
                        XDocument xdocument = XDocument.Parse(xmlString);
                        IEnumerable<XElement> document = xdocument.Elements();

                        var persons = from d in document.Descendants(xmlQuestion.Item1)
                                      where d.Attribute(xmlQuestion.Item2).Value.Equals(vigName)
                                      from q in d.Elements(xmlAnswer.Item1)
                                      select q;

                        foreach (var p in persons)
                        {
                            name = p.Attribute(xmlAnswer.Item2).Value;
                            id = p.Attribute(xmlAnswerNumber).Value;

                            if (!String.IsNullOrEmpty(vigEmail))
                            {
                                email = (from d in document.Descendants(xmlQuestion.Item1)
                                         where d.Attribute(xmlQuestion.Item2).Value.Equals(vigEmail)
                                         from q in d.Elements(xmlAnswer.Item1)
                                         where q.Attribute(xmlAnswerNumber).Value.Equals(id)
                                         select q.Attribute(xmlAnswer.Item2).Value).FirstOrDefault();
                            }
                            if (!String.IsNullOrEmpty(vigRole))
                            {
                                role = (from d in document.Descendants(xmlQuestion.Item1)
                                        where d.Attribute(xmlQuestion.Item2).Value.Equals(vigRole)
                                        from q in d.Elements(xmlAnswer.Item1)
                                        where q.Attribute(xmlAnswerNumber).Value.Equals(id)
                                        select q.Attribute(xmlAnswer.Item2).Value).FirstOrDefault();
                            }
                            DataRow nr = personnal.NewRow();
                            nr["Name"] = String.IsNullOrEmpty(name) ? "" : name;
                            nr["Email"] = String.IsNullOrEmpty(email) ? "" : email;
                            nr["Role"] = String.IsNullOrEmpty(role) ? "" : role;

                            personnal.Rows.Add(nr);

                        }
                    }

                }
            }
            return personnal;


        }

        /// <summary>
        /// Possible values : 
        /// </summary>
        /// <param name="studyId"></param>
        /// <param name="valuename"></param>
        /// <returns></returns>
        private string getValue(string studyId, string questionVIG)
        {
            string retour = "";
            string xmlString = "";
            var eventRecord = OutputStatus.fpevent.data.AsEnumerable().Where(x => (string)x["StudyId"] == studyId && (string)x["Event"] == submissionFormEvent).OrderByDescending(x => x.Field<string>("EventCreationDate")).FirstOrDefault();
            if (eventRecord != null)
            {
                xmlString = eventRecord.Field<string>("xForms");
                if (!string.IsNullOrWhiteSpace(xmlString))
                {
                    XDocument xdocument = XDocument.Parse(xmlString);
                    IEnumerable<XElement> document = xdocument.Elements();

                    var childType = from d in document.Descendants(xmlQuestion.Item1)
                                    where d.Attribute(xmlQuestion.Item2).Value.Equals(questionVIG)
                                    from q in d.Elements(xmlAnswer.Item1)
                                    select new
                                    {
                                        value = q.Attribute(xmlAnswer.Item2).Value
                                    };

                    foreach (var t in childType)
                    {
                        retour += t.value;
                    }
                }
            }

            if (retour == "0")
                retour = "";
            return retour;
        }


    }
}
