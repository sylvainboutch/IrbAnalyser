using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrbAnalyser.IRISMap
{
    class SiteMap
    {
        public static readonly Dictionary<string, string> siteMap = new Dictionary<string, string>()
            {
                {"",""},
                { "Einstein Montefiore", OutputSite.EMmainsite },
                {"Montefiore_Moses Division", "Montefiore Medical Center-Moses Campus"},
                {"Einstein_Laboratory/Office", "Montefiore Medical Center-Einstein Campus"},
                {"Montefiore_Weiler Division", "Montefiore Medical Center-Einstein Campus"},
                {"Einstein_CRC West",OutputSite.EMmainsite},
                {"Montefiore_North Division","Montefiore Medical Center-Wakefield Campus"},
                {"Montefiore_Radiation Oncology @ St. Barnabas",OutputSite.EMmainsite},
                {"Montefiore_Off-site clinics","Montefiore Medical Center-Einstein Campus"},
                {"Einstein_DOSA",OutputSite.EMmainsite},
                {"Einstein_CRC East",OutputSite.EMmainsite},
                {"NBHN_Jacobi Medical Center", "Jacobi Hospital"},
                {"NBHN_North Central Bronx Hospital",OutputSite.EMmainsite},
                {"Einstein_MRRC",OutputSite.EMmainsite},
                //{"Yeshiva University_Azrieli School of Education",OutputSite.EMmainsite}
                {"Yeshiva University_Ferkauf Graduate School of Psychology",OutputSite.EMmainsite},
                {"Yeshiva University_Yeshiva College",OutputSite.EMmainsite}               
            };

        public static string getSite(string key)
        {
            try
            {
                return siteMap[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New site" , key);
                return key + "  (NEW !!!)";
            }
        }

        public static string getSiteType(string key)
        {
            try
            {
                return siteMap[key.Trim()] == OutputSite.EMmainsite ? OutputSite.siteTypePrimary : OutputSite.siteTypeTreating;
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New site", key);
                return key + "  (NEW !!!)";
            }
        }
    }


    /// <summary>
    /// Class for Role mapping, contains string dictionnary for role and group
    /// </summary>
    public static class RoleMap
    {
        public static string PI = "Principal Investigator";
        public static string RC1 = "Faculty Advisor";
        public static string RC2 = "Study Contact";
        public static string SC = "Study Coordinator";
        
        

        public static readonly Dictionary<string, string> roleMap = new Dictionary<string, string>()
            {
                { PI, OutputTeam.PI },
                { RC1, OutputTeam.RC },
                { RC2,"No privilege"},
                {"Additional Research service Coordinator","No privilege"},
                {"Administrative Assistant","No privilege"},
                {"Biostatistician","No privilege"},
                {"Clinical Trials Coordinator","No privilege"},
                {"Co-Investigator","No privilege"},
                {"Co-Principal Investigator","No privilege"},
                {"Department Administrator","No privilege"},
                {"Nurse","No privilege"},
                {"Other Faculty Collaborator","No privilege"},
                {"Participating Clinician","No privilege"},
                {"Postaward Coordinator","No privilege"},
                {"Primary Research service Coordinator","No privilege"},
                {"Research Associate","No privilege"},
                {"Research Pharmacist","No privilege"},
                {"Student Researcher","No privilege"},
                {SC,"No privilege"},
                {"Study Author","No privilege"},
                {"UNKNOWN ROLE - Migrated KP 1","No privilege"},
                {"UNKNOWN ROLE - Migrated KP 2","No privilege"},
                {"Other/Not Listed (explain)","No privilege"},
                {"","No privilege"}

            };

        public static string getRole(string key, bool primary)
        {
            try
            {
                return roleMap[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New role", key);
                return key + "  (NEW !!!)";
            }
        }


    }

    /// <summary>
    /// Class to hold mapping, 2 dictionnary for status and status type
    /// </summary>
    public static class StatusMap
    {
        public static readonly Dictionary<string, string> statusMap = new Dictionary<string, string>()
            {
                {"Undergoing Administrative Review",""},
                {"Administrative File",""},
                {"Approved for Protocol Development",""},
                {"Approved Pending","IRB Initial Submitted"},
                {"Awaiting CRC Approval",""},
                {"Awaiting Execution of Contract",""},
                {"Awaiting PRMC Approval",""},
                {"Closed by PI prior to approval","Withdrawn"},
                {"Closed to Enrollment",""},
                {"COI review. Questions? coi@einstein.yu.edu","IRB Initial Submitted"},
                {"CORRUPTED",""},
                {"Deferred","IRB Initial Deferred"},
                {"Disapproved","IRB Initial Disapproved"},
                {"Draft","IRB Draft"},
                {"Education Validation Failed","IRB Initial Disapproved"},
                {"Emergency Use",""},
                {"Exempt","IRB Exempt"},
                {"Expired Approval",""},
                {"Externally Reviewed",""},
                {"Inactive - Administratively Closed","Complete"},
                {"Migrated","Undefined IRB Event**"},
                {"Migrated with amendment (no MCF)","Undefined IRB Event**"},
                {"Migration Incomplete","Undefined IRB Event**"},
                {"Open",""},
                {"Open (45CFR46.118)",""},
                {"Open, No Enrollment",""},
                {"Open/No Active Subjects",""},
                {"Pending","IRB Initial Submitted"},
                {"Pending - Submitted for Initial Review","IRB Initial Submitted"},
                {"Pending Acceptance to Participate",""},
                {"Preparing for Full Board Review","IRB Initial Submitted"},
                {"QI ONLY",""},
                {"Recruitment Suspended","Temporarily Closed to Accrual"},
                {"Returned to PI for Pre-Review Stipulations",""},
                {"Returned to PI for Stipulations",""},
                {"STUDY CLOSED (Open for Post-Closure Submissions)",""},
                {"Suspended","Temporarily Closed to Accrual and Intervention"},
                {"Terminated by IRB","Complete"},
                {"Terminated by PI","Complete"},
                {"Undergoing COI Review","IRB Initial Submitted"},
                {"Undergoing Exempt Determination",""},
                {"Undergoing Expedited Review","IRB Initial Submitted"},
                {"Undergoing Full Board Review","IRB Initial Submitted"},
                {"Undergoing Review by IRB Chair","IRB Initial Submitted"},
                {"Undergoing Review by IRB Chair/Designee","IRB Initial Submitted"},
                {"Undergoing Review by Legal","IRB Initial Submitted"},
                {"Withdrawn by PI","Withdrawn"},
                {"",""}
            };

        public static string getStatus2(string key)
        {
            try
            {
                return statusMap[Tools.cleanMap(key)];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status", key);
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, bool> multipleMap = new Dictionary<string, bool>()
            {
                {"Undergoing Administrative Review",false},
                {"Administrative File",false},
                {"Approved for Protocol Development",false},
                {"Approved Pending",false},
                {"Awaiting CRC Approval",false},
                {"Awaiting Execution of Contract",false},
                {"Awaiting PRMC Approval",false},
                {"Closed by PI prior to approval",false},
                {"Closed to Enrollment",false},
                {"COI review. Questions? coi@einstein.yu.edu",false},
                {"CORRUPTED",false},
                {"Deferred",false},
                {"Disapproved",false},
                {"Draft",false},
                {"Education Validation Failed",false},
                {"Emergency Use",false},
                {"Exempt",false},
                {"Expired Approval",false},
                {"Externally Reviewed",false},
                {"Inactive - Administratively Closed",false},
                {"Migrated",false},
                {"Migrated with amendment (no MCF)",false},
                {"Migration Incomplete",false},
                {"Open",false},
                {"Open (45CFR46.118)",false},
                {"Open, No Enrollment",false},
                {"Open/No Active Subjects",false},
                {"Pending",false},
                {"Pending - Submitted for Initial Review",false},
                {"Pending Acceptance to Participate",false},
                {"Preparing for Full Board Review",false},
                {"QI ONLY",false},
                {"Recruitment Suspended",false},
                {"Returned to PI for Pre-Review Stipulations",false},
                {"Returned to PI for Stipulations",false},
                {"STUDY CLOSED (Open for Post-Closure Submissions)",false},
                {"Suspended",false},
                {"Terminated by IRB",false},
                {"Terminated by PI",false},
                {"Undergoing COI Review",false},
                {"Undergoing Exempt Determination",false},
                {"Undergoing Expedited Review",false},
                {"Undergoing Full Board Review",false},
                {"Undergoing Review by IRB Chair",false},
                {"Undergoing Review by IRB Chair/Designee",false},
                {"Undergoing Review by Legal",false},
                {"Withdrawn by PI",false},
                {"",false}
            };

        public static bool getMultiple2(string key)
        {
            try
            {
                return multipleMap[Tools.cleanMap(key)];
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static readonly Dictionary<string, string> typeMap = new Dictionary<string, string>()
            {
                {"Undergoing Administrative Review",""},
                {"Administrative File",""},
                {"Approved for Protocol Development",""},
                {"Approved Pending","Pre Activation"},
                {"Awaiting CRC Approval",""},
                {"Awaiting Execution of Contract",""},
                {"Awaiting PRMC Approval",""},
                {"Closed by PI prior to approval","Study Status"},
                {"Closed to Enrollment",""},
                {"COI review. Questions? coi@einstein.yu.edu","Pre Activation"},
                {"CORRUPTED",""},
                {"Deferred","Pre Activation"},
                {"Disapproved","Pre Activation"},
                {"Draft","Pre Activation"},
                {"Education Validation Failed","Pre Activation"},
                {"Emergency Use",""},
                {"Exempt","Pre Activation"},
                {"Expired Approval",""},
                {"Externally Reviewed",""},
                {"Inactive - Administratively Closed","Study Status"},
                {"Migrated","Study Status"},
                {"Migrated with amendment (no MCF)","Study Status"},
                {"Migration Incomplete","Study Status"},
                {"Open",""},
                {"Open (45CFR46.118)",""},
                {"Open, No Enrollment",""},
                {"Open/No Active Subjects",""},
                {"Pending","Pre Activation"},
                {"Pending - Submitted for Initial Review","Pre Activation"},
                {"Pending Acceptance to Participate",""},
                {"Preparing for Full Board Review","Pre Activation"},
                {"QI ONLY",""},
                {"Recruitment Suspended","Study Status"},
                {"Returned to PI for Pre-Review Stipulations",""},
                {"Returned to PI for Stipulations",""},
                {"STUDY CLOSED (Open for Post-Closure Submissions)",""},
                {"Suspended","Study Status"},
                {"Terminated by IRB","Study Status"},
                {"Terminated by PI","Study Status"},
                {"Undergoing COI Review","Pre Activation"},
                {"Undergoing Exempt Determination",""},
                {"Undergoing Expedited Review","Pre Activation"},
                {"Undergoing Full Board Review","Pre Activation"},
                {"Undergoing Review by IRB Chair","Pre Activation"},
                {"Undergoing Review by IRB Chair/Designee","Pre Activation"},
                {"Undergoing Review by Legal","Pre Activation"},
                {"Withdrawn by PI","Study Status"},
                {"",""}
            };

        public static string getType2(string key)
        {
            try
            {                
                return typeMap[Tools.cleanMap(key)];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status (from status type mapping)", key);
                return key + "  (NEW !!!)";
            }
        }

    }

    /// <summary>
    /// Class to hold mapping, 2 dictionnary for status and status type
    /// </summary>
    public static class EventsMap
    {
        public static readonly Dictionary<string, string> eventsMap = new Dictionary<string, string>()
            {
                {"Amendment",OutputStatus.amendmentSubmitStatus}
            };

        public static string getStatus(string key)
        {
            try
            {
                return eventsMap[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status (from events mapping)", key);
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, string> eventsTypeMap = new Dictionary<string, string>()
            {
                {"Amendment",OutputStatus.amendmentSubmitType}
            };

        public static string getType(string key)
        {
            try
            {
                return eventsTypeMap[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status (from event status type mapping)", key);
                return key + "  (NEW !!!)";
            }
        }

    }

    public static class Department
    {

        public static readonly Dictionary<string, string> departmentMap = new Dictionary<string, string>()
        {
            {"Administration","N/A"},
            {"Allergy & Immunology","Medicine"},
            {"Anatomy and Structural Biology","Anatomy and Structural Biology"},
            {"Anesthesiology","Anesthesiology"},
            {"Biochemistry","Biochemistry"},
            {"Cardiology","Medicine"},
            {"Cardiovascular and Thoracic Surgery","Cardiovascular and Thoracic Surgery"},
            {"Cell Biology","Cell Biology"},
            {"Center for Bioethics","N/A"},
            {"Center for Public Health Sciences","N/A"},
            {"Clinical Research Center (CRC)","N/A"},
            {"CMO, The Care Management Company","N/A"},
            {"Critical Care Medicine","Medicine"},
            {"Default Department (INVALID)","N/A"},
            {"Dentistry","Dentistry"},
            {"Dermatology","Medicine"},
            {"Developmental and Molecular Biology","Developmental and Molecular Biology"},
            {"Division of Substance Abuse (DOSA)","Psychiatry and Behavioral Sciences"},
            {"Emergency Medicine","Emergency Medicine"},
            {"Endocrinology","Medicine"},
            {"Epidemiology & Population Health","Epidemiology and Population Health"},
            {"Family and Social Medicine","Family and Social Medicine"},
            {"Ferkauf Graduate School of Psychology","Psychiatry and Behavioral Sciences"},
            {"Gastroenterology & Liver Diseases","Medicine"},
            {"General Internal Medicine","Medicine"},
            {"Genetics","Genetics"},
            {"Geriatrics","Medicine"},
            {"Hematology","Medicine"},
            {"Hepatology","Medicine"},
            {"Hospital Medicine","Medicine"},
            {"Infectious Diseases","Medicine"},
            {"Institutional Review Board","N/A"},
            {"Learning Network","N/A"},
            {"Library, The D. Samuel Gottesman","N/A"},
            {"Medical Student","N/A"},
            {"Medicine","N/A"},
            {"Microbiology & Immunology","Microbiology & Immunology"},
            {"Molecular Pharmacology","Molecular Pharmacology"},
            {"Nephrology","Medicine"},
            {"Network Performance Group","N/A"},
            {"Neurological Surgery, Leo M. Davidoff Department of","Neurological Surgery"},
            {"Neurology, The Saul R. Korey Department of","Neurology"},
            {"Neurology","Neurology"},
            {"Neuroscience, Dominick P. Purpura Department of","Neuroscience"},
            {"Nuclear Medicine","N/A"},
            {"Nursing","N/A"},
            {"Obstetrics & Gynecology and Women's Health","Obstetrics & Gynecology and Women`s Health"},
            {"Office of Clinical Trials","N/A"},
            {"Oncology","Oncology (Medical/Hematologic)"},
            {"Ophthalmology and Visual Sciences","Ophthalmology and Visual Sciences"},
            {"Opthamology","Surgery"},
            {"Orthopaedic Surgery","Orthopaedic Surgery"},
            {"Orthopedics","Surgery"},
            {"Otorhinolaryngology: Head & Neck Surgery","Otorhinolaryngology - Head & Neck Surgery"},
            {"Pathology","Pathology"},
            {"Pediatrics","Pediatrics"},
            {"Pharmacy","N/A"},
            {"Physiology & Biophysics","Physiology and Biophysics"},
            {"Psychiatry and Behavioral Sciences","Psychiatry and Behavioral Sciences"},
            {"Pulmonary Medicine","Medicine"},
            {"Quality Management","N/A"},
            {"Radiation Oncology","Radiation Oncology"},
            {"Radiology","Radiology"},
            {"Rehabilitation Medicine, The Arthur S. Abramson Department of","Rehabiliation Medicine"},
            {"Rheumatology","Medicine"},
            {"Surgery","Surgery"},
            {"Systems & Computational Biology","Systems & Computational Biology"},
            {"Urology","Urology"},
            {"Plastic & Reconstructive Surgery","Surgery"},
            {"Stern College","N/A"},
            {"Oncology (Medical/Hematologic)","Oncology (Medical/Hematologic)"}
        };

        public static string getDepartment(string key)
        {
            try
            {
                return departmentMap[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New department", key);
                return key + "  (NEW !!!)";
            }
        }


        public static readonly Dictionary<string, string> divisionMap = new Dictionary<string, string>()
        {
            {"Administration","Administration"},
            {"Allergy & Immunology","Allergy & Immunology"},
            {"Anatomy and Structural Biology","N/A"},
            {"Anesthesiology","N/A"},
            {"Biochemistry","N/A"},
            {"Cardiology","Cardiology"},
            {"Cardiovascular and Thoracic Surgery","N/A"},
            {"Cell Biology","N/A"},
            {"Center for Bioethics","Center for Bioethics"},
            {"Center for Public Health Sciences","Center for Public Health Sciences"},
            {"Clinical Research Center (CRC)","Clinical Research Center (CRC)"},
            {"CMO, The Care Management Company","CMO, The Care Management Company"},
            {"Critical Care Medicine","Critical Care Medicine"},
            {"Default Department (INVALID)","N/A"},
            {"Dentistry","N/A"},
            {"Dermatology","Dermatology"},
            {"Developmental and Molecular Biology","N/A"},
            {"Division of Substance Abuse (DOSA)","Division of Substance Abuse (DOSA)"},
            {"Emergency Medicine","N/A"},
            {"Endocrinology","Endocrinology"},
            {"Epidemiology & Population Health","N/A"},
            {"Family and Social Medicine","N/A"},
            {"Ferkauf Graduate School of Psychology","N/A"},
            {"Gastroenterology & Liver Diseases","Gastroenterology & Liver Diseases"},
            {"General Internal Medicine","General Internal Medicine"},
            {"Genetics","N/A"},
            {"Geriatrics","Geriatrics"},
            {"Hematology","Hematology"},
            {"Hepatology","Hepatology"},
            {"Hospital Medicine","Hospital Medicine"},
            {"Infectious Diseases","Infectious Diseases"},
            {"Institutional Review Board","Institutional Review Board"},
            {"Learning Network","Learning Network"},
            {"Library, The D. Samuel Gottesman","Library, The D. Samuel Gottesman"},
            {"Medical Student","Medical Student"},
            {"Medicine","Medecine"},
            {"Microbiology & Immunology","N/A"},
            {"Molecular Pharmacology","N/A"},
            {"Nephrology","Nephrology"},
            {"Network Performance Group","Network Performance Group"},
            {"Neurological Surgery, Leo M. Davidoff Department of","N/A"},
            {"Neurology, The Saul R. Korey Department of","N/A"},
            {"Neurology","N/A"},
            {"Neuroscience, Dominick P. Purpura Department of","N/A"},
            {"Nuclear Medicine","Nuclear Medicine"},
            {"Nursing","Nursing"},
            {"Obstetrics & Gynecology and Women's Health","N/A"},
            {"Office of Clinical Trials","Office of Clinical Trials"},
            {"Oncology","Medical and Hematologic Oncology"},
            {"Ophthalmology and Visual Sciences","N/A"},
            {"Opthamology","Opthamology"},
            {"Orthopaedic Surgery","N/A"},
            {"Orthopedics","Orthopedics"},
            {"Otorhinolaryngology: Head & Neck Surgery","N/A"},
            {"Pathology","N/A"},
            {"Pediatrics","Pediatric Rheumatology"},
            {"Pharmacy","Pharmacy"},
            {"Physiology & Biophysics","N/A"},
            {"Psychiatry and Behavioral Sciences","N/A"},
            {"Pulmonary Medicine","Pulmonary"},
            {"Quality Management","Quality Management"},
            {"Radiation Oncology","N/A"},
            {"Radiology","N/A"},
            {"Rehabilitation Medicine, The Arthur S. Abramson Department of","Pediatric Physical Medicine and Rehabilitation"},
            {"Rheumatology","Rheumatology"},
            {"Surgery","N/A"},
            {"Systems & Computational Biology","N/A"},
            {"Urology","N/A"},
            {"Plastic & Reconstructive Surgery","Plastic and Reconstructive Surgery"},
            {"Stern College","N/A"}, 
            {"Oncology (Medical/Hematologic)","Medical and Hematologic Oncology"}
        };

        public static string getDivision(string key)
        {
            try
            {
                return divisionMap[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New division", key);
                return key + "  (NEW !!!)";
            }
        }
    }

    public static class Phase
    {

        public static string getPhase(string phaseIris)
        {
            if (phaseIris.ToLower().Contains("phase iv") && phaseIris.ToLower().Contains("phase v"))
                return "Phase IV/V";
            else if (phaseIris.ToLower().Contains("phase v"))
                return "Phase V";
            else if (phaseIris.ToLower().Contains("phase iv") && phaseIris.ToLower().Contains("phase iii"))
                return "Phase III/IV";
            else if (phaseIris.ToLower().Contains("phase iv"))
                return "Phase IV";
            else if (phaseIris.ToLower().Contains("phase ii") && phaseIris.ToLower().Contains("phase iii"))
                return "Phase II/III";
            else if (phaseIris.ToLower().Contains("phase iii"))
                return "Phase III";
            else if (phaseIris.ToLower().Contains("phase i") && phaseIris.ToLower().Contains("phase ii"))
                return "Phase I/II";
            else if (phaseIris.ToLower().Contains("phase ii"))
                return "Phase II";
            else if (phaseIris.ToLower().Contains("phase i"))
                return "Phase I";

            else return "Please specify";

        }
    }


}
