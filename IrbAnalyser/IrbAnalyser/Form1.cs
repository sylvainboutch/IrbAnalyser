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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            using (Model.VelosDb db = new Model.VelosDb())
            { 
                var study = from st in db.VDA_V_STUDY_SUMMARY
                        select st;

                var output = "OUPUT : \r\n";
                foreach (var stu in study)
                {
                    output += stu.STUDY_NUMBER + " \r\n";
                }
                txtOutput.Text = output;
            }
        }
    }
}
