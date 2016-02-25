using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Common;
using System.Data.EntityClient;
using System.Data.EntityModel;
using Oracle.DataAccess.Client;
using System.Data.Objects;
using System.IO;

using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace IrbAnalyser
{
    public partial class IrbAnalyser : Form
    {
        //public static string zipFile = "";
        public static string dir = "";
        public void setTxt(string text)
        {
            this.txtOutput.Text = text;
        }

        public IrbAnalyser()
        {
            InitializeComponent();
            cboGenerate.Visible = true;
            cboSource.DataSource = Enum.GetValues(typeof(Agency.AgencyList));
            cboSource.SelectedIndex = -1;
        }

        //private static bool btnclicked = false;
        //private static List<string> files = new List<string>();
        private static int filecount = 0;

        private static string zipDir = Path.GetTempPath() + "zips";

        /// <summary>
        /// Analyse the file
        /// </summary>
        private void Analyse(string zipfile, Agency.AgencyList agency)
        {
            Agency.AgencyVal = agency;
            Agency.AgencySetupVal = agency;

            Tools.reset();

            filecount = filecount + 1;
            createDir(zipDir);

            string destination = zipDir + "\\" + Agency.agencyStrLwr + filecount.ToString() + ".zip";

            if (File.Exists(destination))
            {
                File.Delete(destination);
            }

            File.Copy(zipfile, destination);

            dir = Zip.UnZip(zipfile);
            Tools.filename = dir;

            if (Agency.AgencyVal == Agency.AgencyList.BRANY || Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                OutputStudy.analyse(dir);

                

                OutputStatus.analyse(dir);

                OutputStatus.removeDuplicateStatus();
                OutputIRBForm.finalizeEventIrbForm();

                OutputTeam.analyse(dir + "Team.txt");

                OutputTeam.removeDuplicateNewMembers();
                OutputTeam.removeDuplicateDeletedUser();
            }
            else if (Agency.AgencyVal == Agency.AgencyList.NONE)
            {
                OutputStudy.analyse(dir);


                OutputStatus.analyse(dir);

                OutputStatus.removeDuplicateStatus();
                OutputIRBForm.finalizeEventIrbForm();

                OutputTeam.analyse(dir + "Team.txt");

                OutputTeam.removeDuplicateNewMembers();
                OutputTeam.removeDuplicateDeletedUser();
            }

            if (cboGenerate.Checked && cboGenerate.Visible)
            {
                Study_Personnel_list.generateData();
            }

            /*if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                DataTable newDT = new DataTable();
                XmlTools xmltool = new XmlTools();
                foreach(DataRow dr in OutputStatus.fpevent.data.Rows)
                {
                    if (!String.IsNullOrWhiteSpace((string)dr["xForms"]))
                    {
                        newDT = xmltool.AnalyseXmlString((string)dr["xForms"], newDT);
                    }
                }

                ExcelUtility exc = new ExcelUtility();
                List<ExcelWorksheet> lstxls = new List<ExcelWorksheet>();

                lstxls.Add(new ExcelWorksheet("Submission", "Submission XFORM", newDT));
                exc.WriteDataTableToExcel(zipDir + "\\XFORM_Submission.xlsx", lstxls);

            }*/

        }

        private void btnStudy_Click(object sender, EventArgs e)
        {
            ofdStudy.Filter = "Zip Files (.zip)|*.zip|All Files (*.*)|*.*";
            DialogResult dr = ofdStudy.ShowDialog();
            string filename = dr == DialogResult.OK ? ofdStudy.FileName : "";
            txtStudy.Text = Path.GetFileName(filename);
        }

        private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSource.SelectedValue != null && cboSource.SelectedValue.ToString().ToLower() == Agency.AgencyList.BRANY.ToString().ToLower())
            {
                //cboGenerate.Visible = true;
                btnStudy.Visible = false;
                txtStudy.Visible = false;
                label1.Visible = false;
            }
            else
            {
                //cboGenerate.Visible = false;
                btnStudy.Visible = true;
                txtStudy.Visible = true;
                label1.Visible = true;
            }
        }

        private void saveFiles(string savefilenoext)
        {
            enableBtns(false, false);
            txtOutput.Text = "SAVING FILES !";

            ExcelUtility exc = new ExcelUtility();

            string separator = "}";

            NewValueOuput.saveFile(savefilenoext);

            Csv.saveCsv(OutputStudy.newStudy, separator, savefilenoext + "_newStudy");
            Csv.saveCsv(OutputMSD.newMSD, separator, savefilenoext + "_newMSD");
            Csv.saveCsv(OutputDocs.newDocs, separator, savefilenoext + "_newAttachments");
            Csv.saveCsv(OutputSite.newSites, separator, savefilenoext + "_newSites");
            Csv.saveCsv(OutputTeam.newTeam, separator, savefilenoext + "_newTeam");
            Csv.saveCsv(OutputStatus.newStatus, separator, savefilenoext + "_newStatus");
            Csv.saveCsv(OutputIRBForm.newIRBForm, separator, savefilenoext + "_newIRBForm");

            Csv.saveCsv(OutputTeam.newNonSystemUser, separator, savefilenoext + "_newNonSystemUser");

            Csv.saveCsv(OutputTeam.addedDeletedUser, ",", savefilenoext + "_addedDeletedUser", ".csv");

            /*
            Csv.saveCsv(OutputStudy.updatedStudy, separator, savefilenoext + "_updatedStudy");
            Csv.saveCsv(OutputMSD.updatedMSD, separator, savefilenoext + "_updatedMSD");
            Csv.saveCsv(OutputDocs.updatedDocs, separator, savefilenoext + "_updatedAttachments");
            Csv.saveCsv(OutputSite.updatedSites, separator, savefilenoext + "_updatedSites");
            Csv.saveCsv(OutputTeam.updatedTeam, separator, savefilenoext + "_updatedTeam");
            Csv.saveCsv(OutputStatus.updatedStatus, separator, savefilenoext + "_updatedStatus");                
            */

            OutputStatus.newStatus = Tools.removeEmptyColumns(OutputStatus.newStatus);
            OutputTeam.newTeam = Tools.removeEmptyColumns(OutputTeam.newTeam);
            OutputSite.newSites = Tools.removeEmptyColumns(OutputSite.newSites);
            OutputDocs.newDocs = Tools.removeEmptyColumns(OutputDocs.newDocs);
            OutputStudy.newStudy = Tools.removeEmptyColumns(OutputStudy.newStudy);
            OutputStatus.updatedStatus = Tools.removeEmptyColumns(OutputStatus.updatedStatus);
            OutputTeam.updatedTeam = Tools.removeEmptyColumns(OutputTeam.updatedTeam);
            OutputSite.updatedSites = Tools.removeEmptyColumns(OutputSite.updatedSites);
            OutputDocs.updatedDocs = Tools.removeEmptyColumns(OutputDocs.updatedDocs);
            OutputStudy.updatedStudy = Tools.removeEmptyColumns(OutputStudy.updatedStudy);
            OutputTeam.triggerTeam = Tools.removeEmptyColumns(OutputTeam.triggerTeam);

            // = Tools.removeEmptyColumns();


            List<ExcelWorksheet> lstxls = new List<ExcelWorksheet>();

            lstxls.Add(new ExcelWorksheet("Status", "List of status to add in Velos", OutputStatus.newStatus));
            lstxls.Add(new ExcelWorksheet("Team", "List of team members to add Velos", OutputTeam.newTeam));
            lstxls.Add(new ExcelWorksheet("Site", "List of organization to add in Velos", OutputSite.newSites));
            lstxls.Add(new ExcelWorksheet("Attachments", "List of version (attachment) to add in Velos", OutputDocs.newDocs));
            lstxls.Add(new ExcelWorksheet("Studies", "List of studies to create or modify in Velos", OutputStudy.newStudy));
            exc.WriteDataTableToExcel(savefilenoext + "_new.xlsx", lstxls);


            lstxls = new List<ExcelWorksheet>();

            lstxls.Add(new ExcelWorksheet("Status", "List of status to modify in Velos", OutputStatus.updatedStatus));
            lstxls.Add(new ExcelWorksheet("Team", "List of team members to modify in Velos", OutputTeam.updatedTeam));
            lstxls.Add(new ExcelWorksheet("Site", "List of organization to modify in Velos", OutputSite.updatedSites));
            lstxls.Add(new ExcelWorksheet("Attachments", "List of version (attachment) to modify in Velos", OutputDocs.updatedDocs));
            lstxls.Add(new ExcelWorksheet("Studies", "List of studies to modify in Velos", OutputStudy.updatedStudy));
            exc.WriteDataTableToExcel(savefilenoext + "_updated.xlsx", lstxls);

            lstxls = new List<ExcelWorksheet>();

            lstxls.Add(new ExcelWorksheet("Team", "List of team members to modify in Velos", OutputTeam.triggerTeam));
            exc.WriteDataTableToExcel(savefilenoext + "_triggers.xlsx", lstxls);

            /*
            lstxls = new List<ExcelWorksheet>();

            lstxls.Add(new ExcelWorksheet("Team", "List of non system user to add in Velos", OutputTeam.newNonSystemUser));
            exc.WriteDataTableToExcel(savefilenoext + "_newNonSystem.xlsx", lstxls);
            */

            if (cboGenerate.Checked && cboGenerate.Visible)
            {
                lstxls = new List<ExcelWorksheet>();

                lstxls.Add(new ExcelWorksheet("StudyPersonnels", "List of study and Personnels", Study_Personnel_list.studyDT));
                lstxls.Add(new ExcelWorksheet("StudyPersonnelsShort", "List of study and Personnels short version", Study_Personnel_list.studyShort));
                exc.WriteDataTableToExcel(savefilenoext + "_study_personnels.xlsx", lstxls);
            }

            /*if (zipFile != savefilenoext + "_Source.zip")
            {
                File.Copy(zipFile, savefilenoext + "_Source.zip", true);
            }*/

            string saveDir = System.IO.Path.GetDirectoryName(savefilenoext + "xlsx");
            copyDir(zipDir, saveDir + "\\zip");

            CleanUpDir(zipDir);
            Zip.CleanUpFile(dir);

            txtOutput.Text = "Analysis complete.\r\nPlease open the excel file and create/modify studies in Velos accordingly.";
            //btnclicked = true;

            enableBtns(true, true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                sfdCsv.Filter = "Excel Files|*.xlsx";
                DialogResult dr = sfdCsv.ShowDialog();
                string savefilename = dr == DialogResult.OK ? sfdCsv.FileName : "";


                string savefilenoext = savefilename.Remove(savefilename.Length - 5, 5);
                saveFiles(savefilenoext);
            }
            catch (Exception ex)
            {
                txtOutput.Text = ex.ToString();
            }
        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            //try
            //{
                Agency.AgencyList agency = Agency.AgencyList.BRANY;
                Enum.TryParse<Agency.AgencyList>(cboSource.SelectedValue.ToString(), out agency);
                Agency.AgencyVal = agency;
                Agency.AgencySetupVal = agency;
                string zipfile = "";
                //string zipFile;
                if (Agency.AgencyVal == Agency.AgencyList.BRANY)
                {
                    BRANY_API.getZip();
                    string directory = Path.GetTempPath();
                    directory = directory + "IRBreport\\";
                    zipfile = directory + "brany.zip";
                }
                else
                {
                    zipfile = ofdStudy.FileName;
                }

                Tools.reset();

                //DbCompare dbc = new DbCompare();

                enableBtns(false, false);
                txtOutput.Text = "ANALYSING !";

                Analyse(zipfile, agency);

                //Zip.CleanUpFile(dir);

                txtOutput.Text = "Analysis complete.\r\nPlease choose a new file / options to add a new analysis or click Save.";
                //btnclicked = true;
                enableBtns(true, true);
            /*}
            catch (Exception ex)
            {
                txtOutput.Text = ex.ToString();
            }*/
        }

        private void copyDir(string source, string destination)
        {
            createDir(destination);
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(source, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(source, destination));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(source, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(source, destination), true);
        }

        private void createDir(string dir)
        {
            bool exists = System.IO.Directory.Exists(dir);

            if (!exists)
                System.IO.Directory.CreateDirectory(dir);
        }

        public static void CleanUpDir(string dir)
        {
            bool exists = System.IO.Directory.Exists(dir);

            if (exists)
            {
                DirectoryInfo directory = new DirectoryInfo(dir);
                foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
                foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
                directory.Delete();
            }
        }

        private void IrbAnalyser_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanUpDir(zipDir);
            Zip.CleanUpFile(dir);
            e.Cancel = false;
            return;
        }

        private bool runScript()
        {
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();

            //Here's how you add a new script with arguments
            Command myCommand = new Command("C:\\Users\\sbouchar\\Dropbox\\IRIS_SQL\\final\\getAll.ps1");
            //CommandParameter testParam = new CommandParameter("key","value");
            //myCommand.Parameters.Add(testParam);

            // add an extra command to transform the script output objects into nicely formatted strings 
            // remove this line to get the actual objects that the script returns. For example, the script 
            // "Get-Process" returns a collection of System.Diagnostics.Process instances.
            pipeline.Commands.Add(myCommand);

            pipeline.Commands.Add("Out-String");

            // execute the script 
            Collection<PSObject> results = pipeline.Invoke();

            // close the runspace 
            runspace.Close();

            // convert the script result into a single string 
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            return true;

            // return the results of the script that has 
            // now been converted to text 
            //return stringBuilder.ToString(); 

            //pipeline.Commands.Add(myCommand);
            //pipeline.Invoke();
            //runspace.Close();
            // Execute PowerShell script
            //results = pipeline.Invoke()
        }

        private void doAll()
        {
            enableBtns(false, false);
            txtOutput.Text = "ANALYSING EINSTEIN !";



            string datepart = DateTime.Now.ToString("yyyyMMdd");

            string zipLoc = "C:\\Users\\sbouchar\\Desktop\\interface\\" + datepart + "\\IRIS_Source\\Study.Zip";
            string saveLoc = "C:\\Users\\sbouchar\\Desktop\\interface\\" + datepart;

            string savedir = "";
            bool test = true;
            int i = 0;
            while (test)
            {
                i++;
                savedir = saveLoc + "\\out" + i + "\\";
                test = System.IO.Directory.Exists(savedir);
            }

            Directory.CreateDirectory(savedir);


            runScript();

            Tools.reset();

            //DbCompare dbc = new DbCompare();

            Analyse(zipLoc, Agency.AgencyList.EINSTEIN);

            Tools.reset();

            //dbc = new DbCompare();

            txtOutput.Text = "ANALYSING BRANY !";

            BRANY_API.getZip();
            string directory = Path.GetTempPath();
            directory = directory + "IRBreport\\";
            zipLoc = directory + "brany.zip";

            Analyse(zipLoc, Agency.AgencyList.BRANY);

            //Zip.CleanUpFile(dir);

            txtOutput.Text = "SAVING !";

            saveFiles(savedir + "out" + i);

            txtOutput.Text = "ALL DONE !";

            //btnclicked = true;
            enableBtns(true, true);


        }

        private void enableBtns(bool enable = true, bool enableUpdate = false)
        {
            btnAnalyse.Enabled = enable;
            btnSave.Enabled = enable;
            btnAll.Enabled = enable;
            btnUpdate.Enabled = enable;
            btnSave.Enabled = enableUpdate;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            doAll();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            enableBtns(false, false);
            txtOutput.Text = "Performing updates";
            if (OutputStatus.updatedStatus.Rows.Count == 0 && OutputStudy.updatedStudy.Rows.Count == 0)
            {
                DialogResult dr = ofdStudy.ShowDialog();
                string filename = dr == DialogResult.OK ? ofdStudy.FileName : "";
                if (filename != "")
                {
                    OutputStatus.initiate();
                    OutputStudy.initiate();
                    DoUpdates.loadExcel(filename, OutputStatus.updatedStatus, "Status");
                    DoUpdates.loadExcel(filename, OutputStudy.updatedStudy, "Studies");
                }
            }
            txtOutput.Text = "Done updating";
            DoUpdates.complete();
            enableBtns(true, true);
        }

    }
}
