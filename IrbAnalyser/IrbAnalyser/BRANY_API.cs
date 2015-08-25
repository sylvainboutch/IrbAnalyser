using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace IrbAnalyser
{
    public static class BRANY_API
    {
        public static void getZip()
        {
            //CURL COMMAND :
            //curl https://brany.my.irbmanager.com/api/xmlapi -d @BranyMonteReport.xml -u brany\BranyMonteAPI:6TzpCJYP+8yOqn0S --insecure > response.xml


            string url = "https://brany.my.irbmanager.com/api/xmlapi";
            string user = "BranyMonteAPI";
            string domain = "brany";
            string pass = "6TzpCJYP+8yOqn0S";
            string doc = "C:\\Users\\sbouchar\\Documents\\GitHub\\IrbAnalyser\\IrbAnalyser\\IrbAnalyser\\BranyMonteReport.xml";

            string directory = Path.GetTempPath();
            directory = directory + "IRBreport\\";
            string zipFile = directory + "brany.zip";

            //Declare XMLResponse document
            XmlDocument receivedoc = new XmlDocument();

            XmlTextReader myXMLReader = null; 

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            string authInfo = domain + "\\" + user + ":" + pass;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;

            request.ContentType = "text/xml";
            request.Method = "POST";

            var writer = new StreamWriter(request.GetRequestStream());
            writer.Write(GetTextFromXMLFile(doc));
            writer.Close();

            var response = (HttpWebResponse)request.GetResponse();


            myXMLReader = new XmlTextReader(response.GetResponseStream());

            receivedoc.Load(myXMLReader);

            myXMLReader.Close();

            response.Close();

            string bitestr = receivedoc.GetElementsByTagName("bytes")[0].InnerText;

            byte[] array = Convert.FromBase64String(bitestr);

            File.WriteAllBytes(zipFile, array);
            /*using (Stream file = File.OpenWrite(@"C:\\Users\\sbouchar\\brany2.zip"))
            {
                file.Write(array, 0, array.Length);
            }*/

        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Read XML data from file
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="file"></param></span>
        /// <span class="code-SummaryComment"><returns>returns file content in XML string format</returns></span>
        private static string GetTextFromXMLFile(string file)
        {
            StreamReader reader = new StreamReader(file);
            string ret = reader.ReadToEnd();
            reader.Close();
            return ret;
        }
    }
}
