﻿using System;
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

namespace IrbAnalyser
{
    public partial class IrbAnalyser : Form
    {

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

        private static bool btnclicked = false;


        private void btnOk_Click(object sender, EventArgs e)
        {
            //try
            //{
            /*if (btnclicked)
            {
                this.Close();
            }
            else
            {*/
                DbCompare dbc = new DbCompare();

                btnOk.Enabled = false;
                btnOk.Text = "ANALYSING !";

                Analyse();

                

                ExcelUtility exc = new ExcelUtility();
                sfdCsv.Filter = "Excel Files|*.xlsx";
                DialogResult dr = sfdCsv.ShowDialog();
                string savefilename = dr == DialogResult.OK ? sfdCsv.FileName : "";

                string savefilenoext = savefilename.Remove(savefilename.Length - 5, 5);
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
                
                txtOutput.Text = "Analysis complete.\r\nPlease open the excel file and create/modify studies in Velos accordingly.";
                btnclicked = true;
               // btnOk.Text = "Analyse";
                //btnOk.Enabled = true;


            //}
            //}
            //catch (Exception ex)
            //{
            //    txtOutput.Text = ex.ToString();
            //}
        }

        /// <summary>
        /// Analyse the file
        /// </summary>
        private void Analyse()
        {
            Agency.AgencyList agency = Agency.AgencyList.BRANY;
            Enum.TryParse<Agency.AgencyList>(cboSource.SelectedValue.ToString(), out agency);
            Agency.AgencyVal = agency;
            Agency.AgencySetupVal = agency;

            string zipFile;
            if (Agency.AgencyVal == Agency.AgencyList.BRANY)
            {
                BRANY_API.getZip();
                string directory = Path.GetTempPath();
                directory = directory + "IRBreport\\";
                zipFile = directory + "brany.zip";
            }
            else
            {
                zipFile = ofdStudy.FileName;
            }

            string dir = Zip.UnZip(zipFile);
            Tools.filename = dir;

            if (Agency.AgencyVal == Agency.AgencyList.BRANY || Agency.AgencyVal == Agency.AgencyList.EINSTEIN)
            {
                OutputStudy.analyse(dir);

                OutputTeam.analyse(dir + "Team.txt");

                OutputStatus.analyse(dir);

                OutputStatus.removeDuplicateStatus();
                OutputIRBForm.finalizeEventIrbForm();
                OutputTeam.removeDuplicateNewMembers();
                OutputTeam.removeDuplicateDeletedUser();
            }
            else if (Agency.AgencyVal == Agency.AgencyList.NONE)
            {
                OutputStudy.analyse(dir);

                OutputTeam.analyse(dir + "Team.txt");

                OutputStatus.analyse(dir);

                OutputStatus.removeDuplicateStatus();
                OutputIRBForm.finalizeEventIrbForm();
                OutputTeam.removeDuplicateNewMembers();
                OutputTeam.removeDuplicateDeletedUser();
            }

            if (cboGenerate.Checked && cboGenerate.Visible)
            {
                Study_Personnel_list.generateData();
            }
                
            Zip.CleanUpFile(dir);

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

    }
}
