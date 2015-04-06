using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IrbAnalyser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new Models.VelosStg())
            {
                var query = from b in db.VDA_V_STUDY_SUMMARY
                            orderby b.STUDY_NUMBER
                            select b;
                var output = "List all study /r/n";
                foreach (var item in query)
                {
                    output += item.STUDY_NUMBER + "/r/n";
                }

                txtOutput.Text = output;
            }
        }
    }
}
