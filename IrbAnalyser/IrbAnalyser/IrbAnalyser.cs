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

                //ofdCsv.Filter = "Csv Files (.csv)|*.csv|Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                ofdCsv.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
                DialogResult dr = ofdCsv.ShowDialog();
                string filename = dr == DialogResult.OK ? ofdCsv.FileName : "";
                btnOk.Enabled = false;
                btnOk.Text = "ANALYSING !";

                Analyse(filename);

                ExcelUtility exc = new ExcelUtility();
                sfdCsv.Filter = "Excel Files|*.xlsx";
                sfdCsv.ShowDialog();
                string savefilename = dr == DialogResult.OK ? sfdCsv.FileName : "";
                OutputStudy.initiate();
                exc.WriteDataTableToExcel(OutputStudy.study, "New studies", savefilename, "List of studies to create in Velos");
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
        private void Analyse(string filename)
        { 
            FileParser fp =  new FileParser(filename);
            fp.getDataTable();
            IsNewStudy(fp.data);
            foreach (DataRow row in fp.data.Rows)
            {
                OutputStudy.changedValue(row);
            }
        }

        /// <summary>
        /// Compare the file with the database, populates the databable for newStudy
        /// </summary>
        /// <param name="data"></param>
        private void IsNewStudy(DataTable data)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {

                var study = from st in db.LCL_V_STUDYSUMM_PLUSMORE
                            select st;

                var dat = data.AsEnumerable();

                foreach (var row in dat)
                {
                    bool isContained = false;
                    foreach (var stu in study)
                    {
                        isContained = stu.MORE_IRBNUM == row["IRBNumber"].ToString() ? true : isContained;
                    }
                    if (!isContained)
                    {
                        OutputStudy.addRowStudy(row, "New study");
                    }
                }
            }

        }

    }
}
