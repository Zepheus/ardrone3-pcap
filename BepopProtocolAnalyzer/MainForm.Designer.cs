namespace BepopProtocolAnalyzer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chkDumpVideo = new System.Windows.Forms.CheckBox();
            this.btnStartSimulator = new System.Windows.Forms.Button();
            this.chkShowLL = new System.Windows.Forms.CheckBox();
            this.btnOpenPcap = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstPackets = new BepopProtocolAnalyzer.ListViewNF();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hxBox = new Be.Windows.Forms.HexBox();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 589);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(990, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(118, 17);
            this.lblStatus.Text = "toolStripStatusLabel1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(990, 589);
            this.splitContainer1.SplitterDistance = 402;
            this.splitContainer1.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chkDumpVideo);
            this.splitContainer2.Panel1.Controls.Add(this.btnStartSimulator);
            this.splitContainer2.Panel1.Controls.Add(this.chkShowLL);
            this.splitContainer2.Panel1.Controls.Add(this.btnOpenPcap);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(990, 402);
            this.splitContainer2.TabIndex = 0;
            // 
            // chkDumpVideo
            // 
            this.chkDumpVideo.AutoSize = true;
            this.chkDumpVideo.Location = new System.Drawing.Point(249, 17);
            this.chkDumpVideo.Name = "chkDumpVideo";
            this.chkDumpVideo.Size = new System.Drawing.Size(84, 17);
            this.chkDumpVideo.TabIndex = 10;
            this.chkDumpVideo.Text = "Dump Video";
            this.chkDumpVideo.UseVisualStyleBackColor = true;
            // 
            // btnStartSimulator
            // 
            this.btnStartSimulator.Location = new System.Drawing.Point(381, 16);
            this.btnStartSimulator.Name = "btnStartSimulator";
            this.btnStartSimulator.Size = new System.Drawing.Size(175, 23);
            this.btnStartSimulator.TabIndex = 9;
            this.btnStartSimulator.Text = "Start Drone Network Simulator";
            this.btnStartSimulator.UseVisualStyleBackColor = true;
            this.btnStartSimulator.Click += new System.EventHandler(this.btnStartSimulator_Click);
            // 
            // chkShowLL
            // 
            this.chkShowLL.AutoSize = true;
            this.chkShowLL.Location = new System.Drawing.Point(100, 16);
            this.chkShowLL.Name = "chkShowLL";
            this.chkShowLL.Size = new System.Drawing.Size(143, 17);
            this.chkShowLL.TabIndex = 8;
            this.chkShowLL.Text = "Show Low Latency Data";
            this.chkShowLL.UseVisualStyleBackColor = true;
            // 
            // btnOpenPcap
            // 
            this.btnOpenPcap.Location = new System.Drawing.Point(12, 13);
            this.btnOpenPcap.Name = "btnOpenPcap";
            this.btnOpenPcap.Size = new System.Drawing.Size(75, 23);
            this.btnOpenPcap.TabIndex = 7;
            this.btnOpenPcap.Text = "Open PCAP";
            this.btnOpenPcap.UseVisualStyleBackColor = true;
            this.btnOpenPcap.Click += new System.EventHandler(this.btnOpenPcap_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstPackets);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(990, 348);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Packet List:";
            // 
            // lstPackets
            // 
            this.lstPackets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lstPackets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPackets.FullRowSelect = true;
            this.lstPackets.Location = new System.Drawing.Point(3, 16);
            this.lstPackets.Name = "lstPackets";
            this.lstPackets.Size = new System.Drawing.Size(984, 329);
            this.lstPackets.TabIndex = 0;
            this.lstPackets.UseCompatibleStateImageBehavior = false;
            this.lstPackets.View = System.Windows.Forms.View.Details;
            this.lstPackets.SelectedIndexChanged += new System.EventHandler(this.lstPackets_SelectedIndexChanged);
            this.lstPackets.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstPackets_MouseDoubleClick);
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Time";
            this.columnHeader9.Width = 91;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Direction";
            this.columnHeader1.Width = 96;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 96;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Id";
            this.columnHeader3.Width = 54;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Seq";
            this.columnHeader4.Width = 56;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Len";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Proj";
            this.columnHeader6.Width = 139;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Class";
            this.columnHeader7.Width = 165;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Cmd";
            this.columnHeader8.Width = 204;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.hxBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(990, 183);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data:";
            // 
            // hxBox
            // 
            this.hxBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hxBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.hxBox.Location = new System.Drawing.Point(3, 16);
            this.hxBox.Name = "hxBox";
            this.hxBox.ReadOnly = true;
            this.hxBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hxBox.Size = new System.Drawing.Size(984, 164);
            this.hxBox.StringViewVisible = true;
            this.hxBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 611);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "BePop PCAP Analyzer v.3";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox chkDumpVideo;
        private System.Windows.Forms.Button btnStartSimulator;
        private System.Windows.Forms.CheckBox chkShowLL;
        private System.Windows.Forms.Button btnOpenPcap;
        private System.Windows.Forms.GroupBox groupBox1;
        private ListViewNF lstPackets;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.GroupBox groupBox2;
        private Be.Windows.Forms.HexBox hxBox;
        private System.Windows.Forms.ColumnHeader columnHeader9;
    }
}

