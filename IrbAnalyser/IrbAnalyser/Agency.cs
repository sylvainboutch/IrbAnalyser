using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrbAnalyser
{
    class Agency
    {
        //public enum AgencyList { BRANY, IRIS, CLINICALTRIALdotGOV };
        public enum AgencyList { BRANY, EINSTEIN, BOTH, NONE };
        public static string brany = "brany";
        private static AgencyList agency;
        private static AgencyList agencySetup;

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

        public static string agencySetupStrLwr;

        public static AgencyList AgencySetupVal
        {
            get
            {
                return agencySetup;
            }
            set
            {
                agencySetupStrLwr = value.ToString().ToLower();
                agencySetup = value;
            }
        }



    }
}
