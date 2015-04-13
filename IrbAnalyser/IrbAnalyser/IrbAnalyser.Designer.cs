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
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtEvent = new System.Windows.Forms.TextBox();
            this.txtMember = new System.Windows.Forms.TextBox();
            this.btnStudy = new System.Windows.Forms.Button();
            this.btnStatus = new System.Windows.Forms.Button();
            this.btnEvent = new System.Windows.Forms.Button();
            this.btnMember = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ofdStatus = new System.Windows.Forms.OpenFileDialog();
            this.ofdEvent = new System.Windows.Forms.OpenFileDialog();
            this.ofdMember = new System.Windows.Forms.OpenFileDialog();
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
            this.btnOk.Text = "Analyse";
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
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(12, 44);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(214, 20);
            this.txtStatus.TabIndex = 3;
            // 
            // txtEvent
            // 
            this.txtEvent.Location = new System.Drawing.Point(12, 73);
            this.txtEvent.Name = "txtEvent";
            this.txtEvent.ReadOnly = true;
            this.txtEvent.Size = new System.Drawing.Size(215, 20);
            this.txtEvent.TabIndex = 4;
            // 
            // txtMember
            // 
            this.txtMember.Location = new System.Drawing.Point(14, 102);
            this.txtMember.Name = "txtMember";
            this.txtMember.ReadOnly = true;
            this.txtMember.Size = new System.Drawing.Size(214, 20);
            this.txtMember.TabIndex = 5;
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
            // btnStatus
            // 
            this.btnStatus.Location = new System.Drawing.Point(234, 42);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(75, 23);
            this.btnStatus.TabIndex = 7;
            this.btnStatus.Text = "Load";
            this.btnStatus.UseVisualStyleBackColor = true;
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);
            // 
            // btnEvent
            // 
            this.btnEvent.Location = new System.Drawing.Point(234, 71);
            this.btnEvent.Name = "btnEvent";
            this.btnEvent.Size = new System.Drawing.Size(75, 23);
            this.btnEvent.TabIndex = 8;
            this.btnEvent.Text = "Load";
            this.btnEvent.UseVisualStyleBackColor = true;
            this.btnEvent.Click += new System.EventHandler(this.btnEvent_Click);
            // 
            // btnMember
            // 
            this.btnMember.Location = new System.Drawing.Point(234, 100);
            this.btnMember.Name = "btnMember";
            this.btnMember.Size = new System.Drawing.Size(75, 23);
            this.btnMember.TabIndex = 9;
            this.btnMember.Text = "Load";
            this.btnMember.UseVisualStyleBackColor = true;
            this.btnMember.Click += new System.EventHandler(this.btnMember_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(316, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Studies file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Statuses file";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(315, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Events file";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(315, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Study members file";
            // 
            // IrbAnalyser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 327);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMember);
            this.Controls.Add(this.btnEvent);
            this.Controls.Add(this.btnStatus);
            this.Controls.Add(this.btnStudy);
            this.Controls.Add(this.txtMember);
            this.Controls.Add(this.txtEvent);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtStudy);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtOutput);
            this.Name = "IrbAnalyser";
            this.Text = "Irb Analyser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.OpenFileDialog ofdStudy;
        private System.Windows.Forms.SaveFileDialog sfdCsv;
        private System.Windows.Forms.TextBox txtStudy;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtEvent;
        private System.Windows.Forms.TextBox txtMember;
        private System.Windows.Forms.Button btnStudy;
        private System.Windows.Forms.Button btnStatus;
        private System.Windows.Forms.Button btnEvent;
        private System.Windows.Forms.Button btnMember;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog ofdStatus;
        private System.Windows.Forms.OpenFileDialog ofdEvent;
        private System.Windows.Forms.OpenFileDialog ofdMember;
    }
}

