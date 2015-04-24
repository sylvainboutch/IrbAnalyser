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

namespace IrbAnalyser
{
    public partial class IrbAnalyser : Form
    {
        public IrbAnalyser()
        {
            InitializeComponent();
        }
        private static bool btnclicked = false;
        private void btnOk_Click(object sender, EventArgs e)
        {
            //try
            //{
            if (btnclicked)
            {
                this.Close();
            }
            else
            {
                DbCompare dbc = new DbCompare();

                btnOk.Enabled = false;
                btnOk.Text = "ANALYSING !";

                Analyse();

                ExcelUtility exc = new ExcelUtility();
                sfdCsv.Filter = "Excel Files|*.xlsx";
                DialogResult dr = sfdCsv.ShowDialog();
                string savefilename = dr == DialogResult.OK ? sfdCsv.FileName : "";
                List<ExcelWorksheet> lstxls = new List<ExcelWorksheet>();
                
                lstxls.Add(new ExcelWorksheet("Status", "List of status to add or modify in Velos",OutputStatus.status));
                lstxls.Add(new ExcelWorksheet("Team", "List of team members to add or modify in Velos", OutputTeam.team));
                lstxls.Add(new ExcelWorksheet("Site", "List of organization to add or modify in Velos", OutputSite.sites));
                lstxls.Add(new ExcelWorksheet("Attachments", "List of version (attachment) to add or modify in Velos", OutputDocs.docs));
                lstxls.Add(new ExcelWorksheet("Studies", "List of studies to create or modify in Velos", OutputStudy.study));

                //exc.WriteDataTableToExcel(OutputStudy.study, "New studies", savefilename, "List of studies to create in Velos");
                exc.WriteDataTableToExcel(savefilename,lstxls);
                txtOutput.Text = "Analysis complete.\r\nPlease open the excel file and create/modify studies in Velos accordingly.";
                btnclicked = true;
                btnOk.Text = "Close";
                btnOk.Enabled = true;
            }
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
            string dir = Tools.UnZip(ofdStudy.FileName);

            OutputTeam.analyse(dir + "Team.txt");
            OutputStatus.analyse(dir);
            OutputStudy.analyse(dir);

            Tools.CleanUpFile(dir);

        }

        private void btnStudy_Click(object sender, EventArgs e)
        {
            ofdStudy.Filter = "Zip Files (.zip)|*.zip|All Files (*.*)|*.*";
            DialogResult dr = ofdStudy.ShowDialog();
            string filename = dr == DialogResult.OK ? ofdStudy.FileName : "";
            txtStudy.Text = Path.GetFileName(filename);
        }

    }
}
