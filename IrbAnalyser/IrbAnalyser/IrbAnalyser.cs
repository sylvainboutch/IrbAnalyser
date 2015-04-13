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
        //"Csv Files (.csv)|*.csv|Text Files (.txt)|*.txt|All Files (*.*)|*.*";
        private string fileformat = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";

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
                lstxls.Add(new ExcelWorksheet("Studies", "List of studies to create or modify in Velos",OutputStudy.study));
                lstxls.Add(new ExcelWorksheet("Status", "List of status to add or modify in Velos",OutputStatus.status));

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
        /// Compare the file with the database, populates the databable for newStudy
        /// </summary>
        /// <param name="data"></param>
        public static void IsNewStudy(DataTable studyDt, DataTable statusDt, DataTable memberDt, DataTable eventDt)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            {

                var study = from st in db.LCL_V_STUDYSUMM_PLUSMORE
                            select st;

                var dat = studyDt.AsEnumerable();

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
                        OutputStatus.addStatus(row, statusDt, eventDt);
                    }
                }
            }
        }

        /// <summary>
        /// Analyse the file
        /// </summary>
        private void Analyse()
        {

            FileParser fpStudy = new FileParser(ofdStudy.FileName);
            fpStudy.getDataTable();

            FileParser fpStatus = new FileParser(ofdStatus.FileName);
            fpStatus.getDataTable();
            /*
            FileParser fpEvent = new FileParser(ofdEvent.FileName);
            fpEvent.getDataTable();

            FileParser fpMember = new FileParser(ofdMember.FileName);
            fpMember.getDataTable();
            */
            IsNewStudy(fpStudy.data, fpStatus.data, new DataTable(), new DataTable());
            foreach (DataRow row in fpStudy.data.Rows)
            {
                OutputStudy.changedValue(row);
            }

        }

        private void btnStudy_Click(object sender, EventArgs e)
        {
            ofdStudy.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            DialogResult dr = ofdStudy.ShowDialog();
            string filename = dr == DialogResult.OK ? ofdStudy.FileName : "";
            txtStudy.Text = Path.GetFileName(filename);
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            ofdStatus.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            DialogResult dr = ofdStatus.ShowDialog();
            string filename = dr == DialogResult.OK ? ofdStatus.FileName : "";
            txtStatus.Text = Path.GetFileName(filename);
        }

        private void btnEvent_Click(object sender, EventArgs e)
        {
            ofdEvent.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            DialogResult dr = ofdEvent.ShowDialog();
            string filename = dr == DialogResult.OK ? ofdEvent.FileName : "";
            txtEvent.Text = Path.GetFileName(filename);
        }

        private void btnMember_Click(object sender, EventArgs e)
        {
            ofdMember.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            DialogResult dr = ofdMember.ShowDialog();
            string filename = dr == DialogResult.OK ? ofdMember.FileName : "";
            txtMember.Text = Path.GetFileName(filename);
        }


    }
}
