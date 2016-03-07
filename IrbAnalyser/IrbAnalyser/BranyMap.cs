using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace IrbAnalyser
{

    /// <summary>
    /// Class for Site mapping, contains string dictionnary for site
    /// </summary>
    public static class BranySiteMap
    {
        public static readonly Dictionary<string, string> siteMapBrany = new Dictionary<string, string>()
            {
                {"",""},
                { "Montefiore Medical Center", OutputSite.EMmainsite },
                { "Albert Einstein Col. of Med. of Yeshiva University", OutputSite.EMmainsite },
                { "Westchester Cardiology", OutputSite.EMmainsite },
                { "Montefiore (Weiler Division)", OutputSite.EMmainsite },
                { "Montefiore (Moses Division)", OutputSite.EMmainsite },
                { "Montefiore (Children's Hosp)", OutputSite.EMmainsite },
                { "Montefiore (Einstein Liver Center)", OutputSite.EMmainsite },
                { "Einstein Montefiore (non-treating site)", OutputSite.EMmainsite }
            };

        public static string getSite(string key)
        {
            try
            {
                return siteMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New site", key);
                return key + "  (NEW !!!)";
            }
        }

        public static string getSiteType(string key)
        {
            try
            {
                return siteMapBrany[key.Trim()] == OutputSite.EMmainsite ? OutputSite.siteTypePrimary : OutputSite.siteTypeTreating;
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
    public static class BranyRoleMap
    {
        public static string PI = "Investigator";
        public static string RC = "Coordinator";
        public static string CRO = "CRO";

        public static readonly Dictionary<string, string> roleMapBrany = new Dictionary<string, string>()
            {
                { RC, OutputTeam.defaultDisabledRole },
                { PI, OutputTeam.defaultDisabledRole },
                { "Faculty Advisor", OutputTeam.defaultDisabledRole },
                { "Research Assistant", OutputTeam.defaultDisabledRole },
                { "Auditor", "NA" },
                { "CC Recipient", OutputTeam.defaultDisabledRole },
                { "Co-Investigator", OutputTeam.defaultDisabledRole },
                { "Sponsor", "NA" },
                { "Consultant", "NA" },
                {"",""},
                {"Sub-Investigator","Sub-Investigator-No Access"},
                {"Other/Not Listed (explain)",OutputTeam.defaultDisabledRole},
                { CRO, "NA" }
            };

        public static readonly Dictionary<string, string> roleMapBranyPrimary = new Dictionary<string, string>()
            {
                { PI, OutputTeam.PI },
                { RC, OutputTeam.RC },
                { "Faculty Advisor", OutputTeam.defaultDisabledRole },
                { "Research Assistant", OutputTeam.defaultDisabledRole },
                { "Auditor", OutputTeam.defaultDisabledRole },
                { "CC Recipient", OutputTeam.defaultDisabledRole },
                { "Co-Investigator", OutputTeam.defaultDisabledRole },
                { "Sponsor", OutputTeam.defaultDisabledRole },
                { "Consultant", OutputTeam.defaultDisabledRole },
                {"Sub-Investigator","Sub-Investigator-No Access"},
                {"Other/Not Listed (explain)",OutputTeam.defaultDisabledRole},
                {"",""},
                { CRO, "NA" }
            };

        public static string getRole(string key, bool primary)
        {
            try
            {
                if (primary)
                    return roleMapBranyPrimary[key.Trim()];
                else
                    return roleMapBrany[key.Trim()];
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
    public static class BranyStatusMap
    {
        public static readonly Dictionary<string, string> statusMapBrany = new Dictionary<string, string>()
            {
                {"",""},
                {"Pending Review - IRB only","IRB Initial Submitted"},
                {"Submission pending - IRB only","IRB Draft"},
                {"Closed - IRB only","Complete"},
                {"Closed to enrollment -IRB only","Closed to Accrual"},
                {"Approved -IBC Only",""},
                {"Suspended by entity other than IRB - IRB only",""},
                {"Approved",""},
                {"Approved IRB only",""},
                {"Approved -IRB only",""},
                {"Closed","Complete "},
                {"Closed IRB only","Complete"},
                {"Closed to Enrollment","Closed to Accrual"},
                {"Closed to enrollment IRB only","Closed to Accrual"},
                {"Deferred","IRB Initial Deferred"},
                {"Deferred IRB only","IRB Initial Deferred"},
                {"Disapproved","IRB Initial Disapproved "},
                {"Emergency Use ONLY",""},
                {"Exempt","IRB Exempt"},
                {"Exempt Closed/Completed","Complete "},
                {"Expired",""},
                {"Not Engaged in Human Sub Rsrch","Archived"},
                {"Other than Human Subject Rsrch","Archived"},
                {"Pending Approval",""},
                {"Pending Approval IRB only",""},
                {"Pending Closure",""},
                {"Pending closure IRB only",""},
                {"Pending Review","IRB Initial Submitted"},
                {"Pending Review IRB only","IRB Initial Submitted"},
                {"PENDING VETTING","IRB Draft"},
                {"Submission pending","IRB Draft"},
                {"Submission pending IRB only","IRB Draft"},
                {"Suspended by entity other than IRB","Temporarily Closed to Accrual and Intervention"},
                {"Suspended by entity other than IRB IRB only","Temporarily Closed to Accrual and Intervention"},
                {"Suspended by IRB","Temporarily Closed to Accrual and Intervention"},
                {"Suspended by IRB IRB only","Temporarily Closed to Accrual and Intervention"},
                {"Terminated by IRB","IRB Initial Disapproved "},
                {"Terminated by IRB IRB only","IRB Initial Disapproved "},
                {"Transferred to Another Record",""},
                {"Withdrawn by PI/Institution","Withdrawn"},
                {"Withdrawn by Sponsor","Withdrawn"},
                {"Pulled",""},
                {"Approved (IBC)",""},
                {"Approved IBC Only",""},
                {"Pending Review (IBC)",""},
                {"Closed to Enrollment (IBC)",""},
                {"Dropped by Investigator",""},
                {"Administrative - Translation",""},
                {"Closed (IBC)",""},
                {"Pending Approval - IRB only",""},


            };

        public static string getStatus2(string key)
        {
            try
            {
                return statusMapBrany[Tools.cleanMap(key)];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status", key);
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, bool> statusMultipleBrany = new Dictionary<string, bool>()
            {
                {"",false},
                {"Pending Review - IRB only",false},
                {"Submission pending - IRB only",false},
                {"Closed - IRB only",false},
                {"Closed to enrollment -IRB only",true},
                {"Approved -IBC Only",false},
                {"Suspended by entity other than IRB - IRB only",false},
                {"Approved",false},
                {"Approved IRB only",false},
                {"Approved -IRB only",false},
                {"Closed",false},
                {"Closed IRB only",false},
                {"Closed to Enrollment",true},
                {"Closed to enrollment IRB only",true},
                {"Deferred",false},
                {"Deferred IRB only",false},
                {"Disapproved",false},
                {"Emergency Use ONLY",false},
                {"Exempt",false},
                {"Exempt Closed/Completed",false},
                {"Expired",false},
                {"Not Engaged in Human Sub Rsrch",false},
                {"Other than Human Subject Rsrch",false},
                {"Pending Approval",false},
                {"Pending Approval IRB only",false},
                {"Pending Closure",false},
                {"Pending closure IRB only",false},
                {"Pending Review",false},
                {"Pending Review IRB only",false},
                {"PENDING VETTING",false},
                {"Submission pending",false},
                {"Submission pending IRB only",false},
                {"Suspended by entity other than IRB",true},
                {"Suspended by entity other than IRB IRB only",true},
                {"Suspended by IRB",true},
                {"Suspended by IRB IRB only",true},
                {"Terminated by IRB",false},
                {"Terminated by IRB IRB only",false},
                {"Transferred to Another Record",false},
                {"Withdrawn by PI/Institution",false},
                {"Withdrawn by Sponsor",false},
                {"Pulled",false},
                {"Approved (IBC)",false},
                {"Approved IBC Only",false},
                {"Pending Review (IBC)",false},
                {"Closed to Enrollment (IBC)",false},
                {"Dropped by Investigator",false},
                {"Administrative - Translation",false},
                {"Closed (IBC)",false},
                {"Pending Approval - IRB only",false}
            };

        public static bool getStatusMultipleBrany2(string key)
        {
            try
            {
                return statusMultipleBrany[Tools.cleanMap(key)];
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static readonly Dictionary<string, string> typeMapBrany = new Dictionary<string, string>()
            {
                { "",""},
                {"Pending Review - IRB only","Pre Activation"},
                {"Submission pending - IRB only","Pre Activation"},
                {"Closed - IRB only","Study Status"},
                {"Closed to enrollment -IRB only","Study Status"},
                {"Approved -IBC Only",""},
                {"Suspended by entity other than IRB - IRB only",""},
                {"Approved",""},
                {"Approved IRB only",""},
                {"Approved -IRB only",""},
                {"Closed",""},
                {"Closed IRB only","Study Status"},
                {"Closed to Enrollment","Study Status"},
                {"Closed to enrollment IRB only","Study Status"},
                {"Deferred","Pre Activation"},
                {"Deferred IRB only","Pre Activation"},
                {"Disapproved",""},
                {"Emergency Use ONLY",""},
                {"Exempt","Pre Activation"},
                {"Exempt Closed/Completed",""},
                {"Expired",""},
                {"Not Engaged in Human Sub Rsrch","Study Status"},
                {"Other than Human Subject Rsrch","Study Status"},
                {"Pending Approval",""},
                {"Pending Approval IRB only",""},
                {"Pending Closure",""},
                {"Pending closure IRB only",""},
                {"Pending Review","Pre Activation"},
                {"Pending Review IRB only","Pre Activation"},
                {"PENDING VETTING","Pre Activation"},
                {"Submission pending","Pre Activation"},
                {"Submission pending IRB only","Pre Activation"},
                {"Suspended by entity other than IRB","Study Status"},
                {"Suspended by entity other than IRB IRB only","Study Status"},
                {"Suspended by IRB","Study Status"},
                {"Suspended by IRB IRB only","Study Status"},
                {"Terminated by IRB",""},
                {"Terminated by IRB IRB only",""},
                {"Transferred to Another Record",""},
                {"Withdrawn by PI/Institution","Study Status"},
                {"Withdrawn by Sponsor","Study Status"},
                {"Pulled",""},
                {"Approved (IBC)",""},
                {"Approved IBC Only",""},
                {"Pending Review (IBC)",""},
                {"Closed to Enrollment (IBC)",""},
                {"Dropped by Investigator",""},
                {"Administrative - Translation",""},
                {"Closed (IBC)",""},
                {"Pending Approval - IRB only",""},



            };

        public static string getType2(string key)
        {
            try
            {
                return typeMapBrany[Tools.cleanMap(key)];
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
    public static class BranyEventsMap
    {
        public static List<String> teamChangedEvents = new List<string>() 
        { 
            "Personnel Change",
            "Replacement PI",
            "Additional PI"
        };

        public static readonly Dictionary<string, string> eventsMapBrany = new Dictionary<string, string>()
            {
                {"1572 Modification",""},
                {"Additional PI",""},
                {"Administrative (Translation)",""},
                {"Administrative - Translation",""},
                {"Administrative Review",""},
                {"Administrative - Revised Letter",""},
                {"Advertisements",""},
                {"Amendment",OutputStatus.amendmentSubmitStatus},
                {"Continuing Review",""},
                {"Expedited Continuing Review",""},
                {"Expedited Initial Review",""},
                {"Full Board Reconsideration",""},
                {"New Application for IRB Review",""},
                {"Noncompliance",""},
                {"Protocol Exception",""},
                {"QA Review",""},
                {"Replacement PI",""},
                {"Reportable Event",""},
                {"Revised Letter",""},
                {"SAE",""},
                {"Study Closure/Expiration",""},
                {"Study Enrollment Closure Report",""},
                {"Unanticipated Problem",""},
                {"Re-Review of IRB-Requested Changes",""},
                {"IND Safety Report",""},
                {"Incomplete Submission",""},
                {"Protocol Deviation",""},
                {"Safety Report(s) - No Review",""},
                {"Personnel Change",""},
                {"COI Review",""},
                {"PI Signature Needed",""},
                {"Emergency Use of a Test Article",""},
                {"xForm Request Form Only",""},
                {"IND Safety Report with ICF Changes",""},
                {"IBC Screening",""},
                {"IBC Review",""},
                {"IBC Continuing/Periodic Review",""},
                {"Advertisements Re-Review",""},
                {"New Protocol Event (Not ready for IRB Review)",""},
                {"Initial Submission Documents",""},
                {"FYI to Full Board",""},
                {"Subject Complaint",""},
                {"Exempt Status Determination",""},
                {"Compassionate Use Request",""}

            };

        public static string getStatus(string key)
        {
            try
            {
                return eventsMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status (from events)", key);
                return key + "  (NEW !!!)";
            }
        }

        public static readonly Dictionary<string, string> eventsTypeMapBrany = new Dictionary<string, string>()
            {
                {"1572 Modification",""},
                {"Additional PI",""},
                {"Administrative (Translation)",""},
                {"Administrative - Translation",""},
                {"Administrative - Revised Letter",""},
                {"Administrative Review",""},
                {"Advertisements",""},
                {"Amendment",OutputStatus.amendmentSubmitType},
                {"Continuing Review",""},
                {"Expedited Continuing Review",""},
                {"Expedited Initial Review",""},
                {"Full Board Reconsideration",""},
                {"New Application for IRB Review",""},
                {"Noncompliance",""},
                {"Protocol Exception",""},
                {"QA Review",""},
                {"Replacement PI",""},
                {"Reportable Event",""},
                {"Revised Letter",""},
                {"SAE",""},
                {"Study Closure/Expiration",""},
                {"Study Enrollment Closure Report",""},
                {"Unanticipated Problem",""},
                {"Re-Review of IRB-Requested Changes",""},
                {"IND Safety Report",""},
                {"Incomplete Submission",""},
                {"Protocol Deviation",""},
                {"Safety Report(s) - No Review",""},
                {"Personnel Change",""},
                {"COI Review",""},
                {"PI Signature Needed",""},
                {"Emergency Use of a Test Article",""},
                {"xForm Request Form Only",""},
                {"IND Safety Report with ICF Changes",""},
                {"IBC Screening",""},
                {"IBC Review",""},
                {"IBC Continuing/Periodic Review",""},
                {"Advertisements Re-Review",""},
                {"New Protocol Event (Not ready for IRB Review)",""},
                {"Initial Submission Documents",""},
                {"FYI to Full Board",""},
                {"Subject Complaint",""},
                {"Exempt Status Determination",""},
                {"Compassionate Use Request",""}
            };

        public static string getType(string key)
        {
            try
            {
                return eventsTypeMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                NewValueOuput.appendString("New status (from events status type map)", key);
                return key + "  (NEW !!!)";
            }
        }

    }



    /// <summary>
    /// Class for Role mapping, contains string dictionnary for role and group
    /// </summary>
    public static class BranySponsorMap
    {

        public static readonly Dictionary<string, string> sponsorMapBrany = new Dictionary<string, string>()
            {
                    {"NCI (National Cancer Institute)", "National Cancer Institute"},
                    {"NMTRC","Neuroblastoma and Medulloblastoma Translational Research Consortium (NMTRC)"},
                    {"FDA","Food and Drug Administration"},
                    {"ACS","American Cancer Society"},
                    {"Actelion","Actelion Pharmaceuticals Switzerland"},
                    {"Advaxis, Inc.","Advaxis"},
                    {"Agency for Healthcare Research and Quality","Agency for Healthcare Research and Quality"},
                    {"AIDS Malignancy Clinical Trials Consortium","AIDS - Associated Malignancies Clinical Trials Consortium"},
                    {"Aids Malignancy Consortium","AIDS - Associated Malignancies Clinical Trials Consortium"},
                    {"AIDS Malignancy Consortium and National Institutes of Health/NICHD","AIDS - Associated Malignancies Clinical Trials Consortium"},
                    {"Aileron Therapeutics","Aileron Therapeutics Inc"},
                    {"Aileron Therapeutics, Inc.","Aileron Therapeutics Inc"},
                    {"Albert Einstein","Albert Einstein College of Medicine"},
                    {"Albert Einstein College of Medicine of Yeshiva University","Albert Einstein College of Medicine"},
                    {"Alliance","Alliance Cancer Center"},
                    {"Alliance for Clinical Trials in Oncology","Alliance for Clinical Trials in Oncology"},
                    {"Alliance/NCI","Alliance Cancer Center"},
                    {"AMC","AIDS - Associated Malignancies Clinical Trials Consortium"},
                    {"AMC/NCI","AIDS - Associated Malignancies Clinical Trials Consortium"},
                    {"American Cancer Society","American Cancer Society"},
                    {"Amit Verma, MD / Albert Einstein College of Medicine ","Albert Einstein College of Medicine"},
                    {"Ansun Biopharma, Inc","Ansun BioPharma Inc"},
                    {"Ansun Biopharma, Inc.","Ansun BioPharma Inc"},
                    {"Aragon Pharmaceuticals","Aragon Pharmaceuticals"},
                    {"Aragon Pharmaceuticals, Inc.","Aragon Pharmaceuticals"},
                    {"Arqule","ArQule"},
                    {"Array BioPharma","Array Biopharma Inc"},
                    {"Array BioPharma Inc.","Array Biopharma Inc"},
                    {"AS Kevelt","AS Kevelt"},
                    {"Astellas","Astellas Pharma US Inc"},
                    {"Astellas Pharma Global Development","Astellas Pharma US Inc"},
                    {"Astellas Scientific & Medical Affairs, Inc.","Astellas Pharma BV"},
                    {"AstraZeneca","AstraZeneca Pharmaceuticals LP"},
                    {"Baxalta U.S. Inc.","Baxalta U.S. Inc."},
                    {"Baxalta US Inc.","Baxalta U.S. Inc."},
                    {"Bayer","Bayer Healthcare Pharmaceuticals"},
                    {"Bayer HealthCare AG","Bayer Healthcare Pharmaceuticals"},
                    {"BD Diagnostics - Women's Health and Cancer","BD Diagnostics - TriPath"},
                    {"BD Diagnostics - Women''s Health and Cancer","BD Diagnostics - TriPath"},
                    {"Becton, Dickinson and Company","Becton Dickinson and Company (BD)"},
                    {"BioMed Valley Discoveries, Inc","BioMed Valley Discoveries, Inc"},
                    {"Boehringer Ingelheim","Boehringer Ingelheim Pharmaceuticals Inc"},
                    {"Bristol Myers Squibb","Bristol-Myers Squibb - New York"},
                    {"Bristol-Myers Squibb","Bristol-Myers Squibb - New York"},
                    {"Brown University","Brown University"},
                    {"Brown University Oncology Research Group","Brown University"},
                    {"Cancer and Leukemia Group B","Cancer and Leukemia Group B"},
                    {"Cardiovascular and Interventional Radiological Society of Europe","Cardiovascular and Interventional Radiological Society of Europe"},
                    {"Celgene","Celgene Corporation"},
                    {"Celgene Corporation","Celgene Corporation"},
                    {"Center for International Blood and Marrow Transplant Research","Center for International Blood and Marrow Transplant Research"},
                    {"Center for International Blood and Marrow Transplant Research ","Center for International Blood and Marrow Transplant Research"},
                    {"CHAM","Childrens Hospital at Montefiore"},
                    {"Children's Hospital Los Angeles","Childrens Hospital Los Angeles"},
                    {"Children's Oncology Grouo","Childrens Oncology Group"},
                    {"Children's Oncology Group","Childrens Oncology Group"},
                    {"Children''s Oncology Group","Childrens Oncology Group"},
                    {"Chimerix","Chimerix, Inc."},
                    {"Chimerix Inc","Chimerix, Inc."},
                    {"Clovis Oncology, Inc.","Clovis Oncology, Inc."},
                    {"COG","Childrens Oncology Group"},
                    {"Columbia University Medical Center","Columbia University Medical Center"},
                    {"CTEP","Cancer Therapy Evaluation Program"},
                    {"CytRx","CytRx Corporation"},
                    {"CytRx Corporation","CytRx Corporation"},
                    {"Daiichi Sankyo Inc.","Daiichi Sankyo Inc"},
                    {"Daiichi Sankyo Pharma Development and ArQule, Inc.","Daiichi Sankyo Inc"},
                    {"Dana Farber Cancer Institute","Dana-Farber Cancer Institute"},
                    {"Dana-Farber Cancer Institute","Dana-Farber Cancer Institute"},
                    {"Delcath Systems Inc.","Delcath Systems, Inc."},
                    {"DELCATH SYSTEMS, INC","Delcath Systems, Inc."},
                    {"DENDREON","Dendreon Corporation"},
                    {"Departmental","Albert Einstein College of Medicine"},
                    {"DEPT","Albert Einstein College of Medicine"},
                    {"Dermatology Foundation","Dermatology Foundation"},
                    {"Dr. Jennifer Stein at NYU","New York University Langone Medical Center"},
                    {"Eastern Cooperative Oncology Group","Eastern Cooperative Oncology Group"},
                    {"ECOG","Eastern Cooperative Oncology Group"},
                    {"ECOG/NCI","Eastern Cooperative Oncology Group"},
                    {"ECOG/SWOG","Eastern Cooperative Oncology Group"},
                    {"ECOG-ACRIN","Eastern Cooperative Oncology Group"},
                    {"Einstein","Albert Einstein College of Medicine"},
                    {"Eleison Pharmaceuticals LLC.","Eleison Pharmaceuticals, LLC"},
                    {"Eleison Pharmaceuticals, LLC","Eleison Pharmaceuticals, LLC"},
                    {"Eli Lilly & Co.","Eli Lilly and Company"},
                    {"Eli Lilly and Company","Eli Lilly and Company"},
                    {"EMD Serono","EMD Serono"},
                    {"EMD Serono, Inc.","EMD Serono"},
                    {"F. Hoffman- La Roche, Ltd.","F. Hoffmann-La Roche, Ltd.- Palo Alto"},
                    {"Fujirebio Diagnostics","Fujirebio Diagnostic"},
                    {"Fujirebio Diagnostics, Inc","Fujirebio Diagnostic"},
                    {"Gentium S.p.A","Gentium SpA"},
                    {"Georgia Regents University","Georgia Regents University"},
                    {"Gilead Sciences","Gilead"},
                    {"Gilead Sciences, Inc.","Gilead"},
                    {"Giselle Sholler","Giselle Sholler"},
                    {"GlaxoSmithKline","GlaxoSmithKline"},
                    {"GOG","Gynecologic Oncology Group (GOG) Research Base"},
                    {"Gynecologic Oncology Group","Gynecologic Oncology Group (GOG) Research Base"},
                    {"Gynelogic Oncology Group","Gynecologic Oncology Group (GOG) Research Base"},
                    {"H. Lee Moffitt Cancer Center","H Lee Moffitt Cancer Center and Research Institute Phase 2 Consortium"},
                    {"H. Lee Moffitt Cancer Center and Research Institute","H Lee Moffitt Cancer Center and Research Institute Phase 2 Consortium"},
                    {"H. Lee Moffitt Cancer Center and Research Institute Inc.","H Lee Moffitt Cancer Center and Research Institute Phase 2 Consortium"},
                    {"Heat Biologics","Heat BioLogics"},
                    {"Heat Biologics, Inc.","Heat BioLogics"},
                    {"Hoffmann LaRoche","F. Hoffmann-La Roche, Ltd.- Palo Alto"},
                    {"Hoffmann-La Roche","F. Hoffmann-La Roche, Ltd.- Palo Alto"},
                    {"Hoffmann-La Roche Inc.","F. Hoffmann-La Roche, Ltd.- Palo Alto"},
                    {"Icahn School of Medicine at Mount Sinai","Mount Sinai School of Medicine"},
                    {"Incyte Corp.","Incyte Corporation"},
                    {"Incyte Corporation","Incyte Corporation"},
                    {"Ira Braunschweig, MD / Montefiore Medical Center","Montefiore Medical Center-Einstein Campus"},
                    {"Janssen Research & Development, LLC","Janssen Research  and Development, LLC"},
                    {"Jazz Pharmaceuticals","Jazz Pharmaceuticals plc"},
                    {"Kevelt AS","AS Kevelt"},
                    {"Kite Pharma, Inc.","Kite Pharma Inc"},
                    {"Lilly USA, LLC","Lilly USA, LLC"},
                    {"M.D. Anderson Cancer Center","M D Anderson Cancer Center"},
                    {"MedImmune","MedImmune Inc"},
                    {"MedImmune LLC","MedImmune Inc"},
                    {"Merck Sharp & Dohme Corp.","Merck and Company Inc"},
                    {"Merck Sharp & Dohme Corp., a subsidiary of Merck & Co., Inc.","Merck and Company Inc"},
                    {"Merrimack Pharma","Merrimack Pharmaceuticals"},
                    {"Merrimack Pharmaceuticals","Merrimack Pharmaceuticals"},
                    {"Millennium Pharmaceuticals","Millennium Pharmaceuticals, Inc."},
                    {"Millennium Pharmaceuticals, Inc.","Millennium Pharmaceuticals, Inc."},
                    {"Mirati Therapeutics","Mirati Therapeutics"},
                    {"Mirati Therapeutics Inc.","Mirati Therapeutics"},
                    {"Momenta Pharmaceuticals, Inc.","Momenta Pharmaceuticals"},
                    {"Montefiore Medical Center","Montefiore Medical Center"},
                    {"Montefiore-Einstein ","Montefiore Medical Center-Einstein Campus"},
                    {"Nanospectra Biosciences, Inc.","Nanospectra Biosciences Inc"},
                    {"National Cancer Institute","National Cancer Institute"},
                    {"National Cancer Institute (NCI)","National Cancer Institute"},
                    {"National Institutes of Health ","National Institutes of Health"},
                    {"National Marrow Donor Program","National Marrow Donor Program"},
                    {"NCI","National Cancer Institute"},
                    {"NCI /AMC","National Cancer Institute"},
                    {"NCI AIDS Malignancy Consortium","National Cancer Institute HIV-AIDS Malignancy Branch"},
                    {"NCI CIRB","National Cancer Institute"},
                    {"NCI/CTSU","National Cancer Institute"},
                    {"NCI/ECOG","National Cancer Institute"},
                    {"Neuroblastoma and Medulloblastoma Translational Research Consortium (NMTRC) ","Neuroblastoma and Medulloblastoma Translational Research Consortium (NMTRC) "},
                    {"New York Medical College","New York Medical College"},
                    {"NIH","National Institute of Health"},
                    {"NIH (Fogarty and NCI)","National Institute of Health"},
                    {"NIH/NCI","National Institute of Health"},
                    {"Novartis","Novartis Pharmaceuticals Corporation"},
                    {"Novartis Pharmaceuticals","Novartis Pharmaceuticals Corporation"},
                    {"Novartis Pharmaceuticals Corp","Novartis Pharmaceuticals Corporation"},
                    {"NRG","NRG Oncology"},
                    {"NRG Oncology","NRG Oncology"},
                    {"NRG Oncology, Inc.","NRG Oncology"},
                    {"NSABP Foundation Inc","NSABP Foundation Inc"},
                    {"NYU Langone Medical Center","New York University Langone Medical Center"},
                    {"Ohio State University","Ohio State University"},
                    {"Oncolytics Biotech","Oncolytics Biotech Inc."},
                    {"Oncolytics Biotech Inc.","Oncolytics Biotech Inc."},
                    {"OREF/MSTS, PSI and CCSRI","Orthopaedic Research and Education Foundation"},
                    {"OxiGENE","Oxigene"},
                    {"PCORI","Patient-Centered Outcomes Research Institute"},
                    {"PDS Biotechnology Corp.","PDS Biotechnology Corporation"},
                    {"Pfizer","Pfizer Inc"},
                    {"Pharma Mar, SAU","Pharma Mar, SAU"},
                    {"Pharmacyclics","Pharmacyclics Inc"},
                    {"Pharmacyclics Inc.","Pharmacyclics Inc"},
                    {"PharmaMar","Pharma Mar, SAU"},
                    {"Philips Oy ¿ Philips Medical Systems MR Finland","Philips Healthcare"},
                    {".","Photocure ASA"},
                    {"Precision Biologics, Inc","Precision Biologics Inc"},
                    {"Precision Biologics, Inc.","Precision Biologics Inc"},
                    {"PrECOG","PrECOG, LLC"},
                    {"PrECOG, LLC","PrECOG, LLC"},
                    {"PrECOG, LLC.","PrECOG, LLC"},
                    {"Radiation Therapy Oncology Group","Radiation Therapy Oncology Group"},
                    {"Rexahn Pharmaceuticals","Rexahn Pharmaceuticals Inc"},
                    {"Rexahn Pharmaceuticals, Inc.","Rexahn Pharmaceuticals Inc"},
                    {"Roche Pharmaceutical","Roche Laboratories"},
                    {"RTOG","Radiation Therapy Oncology Group"},
                    {"Sanofi Services US Services Inc.","Sanofi - Synthelabo Inc"},
                    {"Seattle Genetics","Seattle Genetics"},
                    {"Seattle Genetics, Inc.","Seattle Genetics"},
                    {"SFJ Pharma Ltd. II","SFJ Pharma Ltd. II"},
                    {"SFJ Pharmaceuticals, Inc.","SFJ Pharma Ltd. II"},
                    {"Sidney Kimmel Comprehensive Cancer Center","Sidney Kimmel Cancer Center"},
                    {"Sirtex Medical","Sirtex Medical"},
                    {"Sirtex Technology Pty Ltd","Sirtex Medical"},
                    {"Soligenix","Soligenix Inc"},
                    {"SOTIO","SOTIO, LLC"},
                    {"Sotio a.s.","SOTIO, LLC"},
                    {"Southwest Oncology Group","SWOG"},
                    {"Sponsored by the National Institutes of Health","National Institutes of Health"},
                    {"Sunshine Project by the Pediatric Cancer Foundation ","Sunshine Project by the Pediatric Cancer Foundation "},
                    {"SWOG","SWOG"},
                    {"SWOG/NCI","Southwest Oncology and Hematology"},
                    {"TAKEDA","Takeda USA Inc"},
                    {"TetraLogic Pharmaceuticals","TetraLogic Pharmaceuticals"},
                    {"The Feinstein Institute for Medical Research","The Feinstein Institute for Medical Research"},
                    {"The Neuroblastoma and Medulloblastoma Translational Research Consortium (NMTRC)","The Neuroblastoma and Medulloblastoma Translational Research Consortium (NMTRC)"},
                    {"The Radiosurgery Society","The Radiosurgery Society"},
                    {"The Sidney Kimmel Comprehensive Cancer Center","Sidney Kimmel Comprehensive Cancer Center at Johns Hopkins Hospital"},
                    {"Threshold Pharmaceuticals","Threshold Pharmaceuticals"},
                    {"Threshold Pharmaceuticals, Inc","Threshold Pharmaceuticals"},
                    {"University of Hawaii","University of Hawaii"},
                    {"University of South Florida","University of South Florida"},
                    {"University of Virginia","University of Virginia"},
                    {"Washington Universtiy Medical Center/Siteman Cancer Center","Siteman Cancer Center at Washington University"},
                    {"WITS Health Consortium (PTY), LTD.","University of Witwatersrand"},
                    {"XBiotech","XBiotech USA"},
                    {"XBiotech, Inc.","XBiotech USA"},
                    {"Xcovery","Xcovery"},
                    {"Xcovery Holding Company, LLC","Xcovery"},
                    {"Yale University","Yale University"},
                    {"Eisai, Inc","Eisai Inc"},
                    {"Calistoga Pharmaceuticals, Inc","Calistoga Pharmaceutical Inc"},
                    {"EISAI","Eisai Inc"},
                    {"Oncogenex","OncoGenex Technologies Inc"},
                    {"Celator Pharmaceutical","Celator Pharmaceuticals"},
                    {"Onyx Pharmaceuticals Inc","Onyx Pharmaceuticals Inc"},
                    {"Kyowa Hakko Kirin Pharma, Inc.","Kyowa Hakko Kirin Pharma, Inc."},
                    {"Astellas Pharma US, Inc.","Astellas Pharma US Inc"},
                    {"Dendreon Pharmaceuticals, Inc.","Dendreon Corporation"},
                    {"Pharmacyclics LLC","Pharmacyclics Inc"}

            };

        public static string getSponsor(string key)
        {
            try
            {
                    return sponsorMapBrany[key.Trim()];
            }
            catch (Exception ex)
            {
                //NewValueOuput.appendString("New role", key);
                return "";
            }
        }
    }


}
