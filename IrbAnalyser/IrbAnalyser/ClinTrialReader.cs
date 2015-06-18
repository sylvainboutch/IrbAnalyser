using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IrbAnalyser
{
    public static class ClinTrialReader
    {
        
        public static List<ClinTrialObject.clinical_study> clinStudyList = new List<ClinTrialObject.clinical_study>();
        
        
        public void Read(string filedir)
        {
            string[] files = Directory.GetFiles(filedir);
            foreach (string file in files)
            { 
                using (FileStream xmlStream = new FileStream(file, FileMode.Open))
                {
                    using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                    {
                        XmlSerializer xmlserializer = new XmlSerializer(typeof(ClinTrialObject.clinical_study));
                        ClinTrialObject.clinical_study study = xmlserializer.Deserialize(xmlReader) as ClinTrialObject.clinical_study;
                        clinStudyList.Add(study);
                    }
                }
            }
        }

    }
}
