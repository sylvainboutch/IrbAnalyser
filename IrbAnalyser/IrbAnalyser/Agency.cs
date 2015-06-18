using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrbAnalyser
{
    class Agency
    {
        public enum AgencyList { BRANY, IRIS, CLINICALTRIALdotGOV };

        private static AgencyList agency;

        public static string agencyStrLwr;

        public static AgencyList AgencyVal
        {
            get
            {
                return agency;
            }
            set
            {
                agencyStrLwr = value.ToString().ToLower();
                agency = value;
            }
        }



    }
}
