using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IrbAnalyser
{
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

                sponsors.Columns.Add("Sponsors", typeof(List<Sponsors>));
            }
        }

        private static void addRow()
        {

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
        
        }


        public static void analyseDelete(DataTable table)
        { 
        
        }

    }

    class Sponsors
    {
        public string name { get; set; }
        public string contactName { get; set; }
        public string contactPhone { get; set; }
        public string contactEmail { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
    }
}
