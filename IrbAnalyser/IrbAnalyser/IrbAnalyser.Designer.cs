namespace IrbAnalyser
{
    partial class IrbAnalyser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.ofdStudy = new System.Windows.Forms.OpenFileDialog();
            this.sfdCsv = new System.Windows.Forms.SaveFileDialog();
            this.txtStudy = new System.Windows.Forms.TextBox();
            this.btnStudy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ofdStatus = new System.Windows.Forms.OpenFileDialog();
            this.ofdEvent = new System.Windows.Forms.OpenFileDialog();
            this.ofdMember = new System.Windows.Forms.OpenFileDialog();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(0, 139);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(419, 159);
            this.txtOutput.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.Location = new System.Drawing.Point(0, 304);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(419, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Analyze";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtStudy
            // 
            this.txtStudy.Location = new System.Drawing.Point(12, 15);
            this.txtStudy.Name = "txtStudy";
            this.txtStudy.ReadOnly = true;
            this.txtStudy.Size = new System.Drawing.Size(214, 20);
            this.txtStudy.TabIndex = 2;
            // 
            // btnStudy
            // 
            this.btnStudy.Location = new System.Drawing.Point(234, 13);
            this.btnStudy.Name = "btnStudy";
            this.btnStudy.Size = new System.Drawing.Size(75, 23);
            this.btnStudy.TabIndex = 6;
            this.btnStudy.Text = "Load";
            this.btnStudy.UseVisualStyleBackColor = true;
            this.btnStudy.Click += new System.EventHandler(this.btnStudy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(316, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Zip file";
            // 
            // cboSource
            // 
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(188, 82);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(121, 21);
            this.cboSource.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Data source";
            // 
            // IrbAnalyser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 327);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStudy);
            this.Controls.Add(this.txtStudy);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtOutput);
            this.Name = "IrbAnalyser";
            this.Text = "IRB Analyzer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.OpenFileDialog ofdStudy;
        private System.Windows.Forms.SaveFileDialog sfdCsv;
        private System.Windows.Forms.TextBox txtStudy;
        private System.Windows.Forms.Button btnStudy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog ofdStatus;
        private System.Windows.Forms.OpenFileDialog ofdEvent;
        private System.Windows.Forms.OpenFileDialog ofdMember;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.Label label2;
    }
}

