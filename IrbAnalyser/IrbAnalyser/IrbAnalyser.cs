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

                ofdCsv.Filter = "Csv Files (.csv)|*.csv|Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                DialogResult dr = ofdCsv.ShowDialog();
                string filename = dr == DialogResult.OK ? ofdCsv.FileName : "";
                btnOk.Enabled = false;
                btnOk.Text = "ANALYSING !";
                FileParser fileparser = new FileParser(filename);
                fileparser.getDataTable();
                dbc.isNewStudy(fileparser.data);
                //CsvReader csvReader = new CsvReader(filename);
                //csvReader.getDataTable();
                //dbc.isNewStudy(csvReader.data);

                ExcelUtility exc = new ExcelUtility();
                sfdCsv.Filter = "Excel Files|*.xlsx";
                sfdCsv.ShowDialog();
                string savefilename = dr == DialogResult.OK ? sfdCsv.FileName : "";
                exc.WriteDataTableToExcel(OutputNewStudy.study, "New studies", savefilename, "List of studies to create in Velos");
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


    }
}
