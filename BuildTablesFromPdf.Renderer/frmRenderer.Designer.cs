namespace BuildTablesFromPdf.Renderer
{
    partial class frmRenderer
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCheckAllPages = new System.Windows.Forms.Button();
            this.btnViewRawContent = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.fileOpen = new BuildTablesFromPdf.Renderer.FileOpen();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPageContent = new System.Windows.Forms.TextBox();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.lblPages = new System.Windows.Forms.Label();
            this.lblPageSummary = new System.Windows.Forms.Label();
            this.chkTextRealSize = new System.Windows.Forms.CheckBox();
            this.chkText = new System.Windows.Forms.CheckBox();
            this.chkParagraphs = new System.Windows.Forms.CheckBox();
            this.chkTables = new System.Windows.Forms.CheckBox();
            this.chkLines = new System.Windows.Forms.CheckBox();
            this.btnHtmlExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer.Panel1.Controls.Add(this.btnHtmlExport);
            this.splitContainer.Panel1.Controls.Add(this.btnCheckAllPages);
            this.splitContainer.Panel1.Controls.Add(this.btnViewRawContent);
            this.splitContainer.Panel1.Controls.Add(this.btnRead);
            this.splitContainer.Panel1.Controls.Add(this.fileOpen);
            this.splitContainer.Panel1.Controls.Add(this.label1);
            this.splitContainer.Panel1.Controls.Add(this.txtPageContent);
            this.splitContainer.Panel1.Controls.Add(this.txtPage);
            this.splitContainer.Panel1.Controls.Add(this.btnGo);
            this.splitContainer.Panel1.Controls.Add(this.btnLast);
            this.splitContainer.Panel1.Controls.Add(this.btnNext);
            this.splitContainer.Panel1.Controls.Add(this.btnPrevious);
            this.splitContainer.Panel1.Controls.Add(this.btnFirst);
            this.splitContainer.Panel1.Controls.Add(this.lblPages);
            this.splitContainer.Panel1.Controls.Add(this.lblPageSummary);
            this.splitContainer.Panel1.Controls.Add(this.chkTextRealSize);
            this.splitContainer.Panel1.Controls.Add(this.chkText);
            this.splitContainer.Panel1.Controls.Add(this.chkParagraphs);
            this.splitContainer.Panel1.Controls.Add(this.chkTables);
            this.splitContainer.Panel1.Controls.Add(this.chkLines);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer_Panel2_Paint);
            this.splitContainer.Size = new System.Drawing.Size(695, 456);
            this.splitContainer.SplitterDistance = 311;
            this.splitContainer.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(12, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 7);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // btnCheckAllPages
            // 
            this.btnCheckAllPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckAllPages.Location = new System.Drawing.Point(192, 107);
            this.btnCheckAllPages.Name = "btnCheckAllPages";
            this.btnCheckAllPages.Size = new System.Drawing.Size(104, 23);
            this.btnCheckAllPages.TabIndex = 8;
            this.btnCheckAllPages.Text = "Check all pages";
            this.btnCheckAllPages.UseVisualStyleBackColor = true;
            this.btnCheckAllPages.Click += new System.EventHandler(this.btnCheckAllPages_Click);
            // 
            // btnViewRawContent
            // 
            this.btnViewRawContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewRawContent.Location = new System.Drawing.Point(192, 78);
            this.btnViewRawContent.Name = "btnViewRawContent";
            this.btnViewRawContent.Size = new System.Drawing.Size(104, 23);
            this.btnViewRawContent.TabIndex = 8;
            this.btnViewRawContent.Text = "View raw content";
            this.btnViewRawContent.UseVisualStyleBackColor = true;
            this.btnViewRawContent.Click += new System.EventHandler(this.btnViewRawContent_Click);
            // 
            // btnRead
            // 
            this.btnRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRead.Location = new System.Drawing.Point(246, 24);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(50, 22);
            this.btnRead.TabIndex = 7;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // fileOpen
            // 
            this.fileOpen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpen.BrowserCaption = "";
            this.fileOpen.DefaultExtension = "";
            this.fileOpen.FileFilters = "";
            this.fileOpen.Folder = "";
            this.fileOpen.Location = new System.Drawing.Point(12, 26);
            this.fileOpen.Name = "fileOpen";
            this.fileOpen.ReadOnly = false;
            this.fileOpen.Size = new System.Drawing.Size(228, 20);
            this.fileOpen.TabIndex = 6;
            this.fileOpen.Value = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "File";
            // 
            // txtPageContent
            // 
            this.txtPageContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPageContent.Location = new System.Drawing.Point(12, 256);
            this.txtPageContent.Multiline = true;
            this.txtPageContent.Name = "txtPageContent";
            this.txtPageContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPageContent.Size = new System.Drawing.Size(284, 188);
            this.txtPageContent.TabIndex = 4;
            this.txtPageContent.WordWrap = false;
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(82, 224);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(47, 20);
            this.txtPage.TabIndex = 3;
            this.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(257, 224);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(39, 22);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(221, 224);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(30, 22);
            this.btnLast.TabIndex = 2;
            this.btnLast.Text = ">>";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(192, 224);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 22);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(46, 224);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(30, 22);
            this.btnPrevious.TabIndex = 2;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(17, 224);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(30, 22);
            this.btnFirst.TabIndex = 2;
            this.btnFirst.Text = "<<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // lblPages
            // 
            this.lblPages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPages.Location = new System.Drawing.Point(135, 224);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(51, 20);
            this.lblPages.TabIndex = 1;
            this.lblPages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPageSummary
            // 
            this.lblPageSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPageSummary.Location = new System.Drawing.Point(12, 181);
            this.lblPageSummary.Name = "lblPageSummary";
            this.lblPageSummary.Size = new System.Drawing.Size(284, 17);
            this.lblPageSummary.TabIndex = 1;
            this.lblPageSummary.Text = "label1";
            // 
            // chkTextRealSize
            // 
            this.chkTextRealSize.AutoSize = true;
            this.chkTextRealSize.Location = new System.Drawing.Point(93, 147);
            this.chkTextRealSize.Name = "chkTextRealSize";
            this.chkTextRealSize.Size = new System.Drawing.Size(69, 17);
            this.chkTextRealSize.TabIndex = 0;
            this.chkTextRealSize.Text = "Real size";
            this.chkTextRealSize.UseVisualStyleBackColor = true;
            this.chkTextRealSize.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkText
            // 
            this.chkText.AutoSize = true;
            this.chkText.Location = new System.Drawing.Point(12, 147);
            this.chkText.Name = "chkText";
            this.chkText.Size = new System.Drawing.Size(47, 17);
            this.chkText.TabIndex = 0;
            this.chkText.Text = "Text";
            this.chkText.UseVisualStyleBackColor = true;
            this.chkText.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkParagraphs
            // 
            this.chkParagraphs.AutoSize = true;
            this.chkParagraphs.Checked = true;
            this.chkParagraphs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkParagraphs.Location = new System.Drawing.Point(12, 124);
            this.chkParagraphs.Name = "chkParagraphs";
            this.chkParagraphs.Size = new System.Drawing.Size(80, 17);
            this.chkParagraphs.TabIndex = 0;
            this.chkParagraphs.Text = "Paragraphs";
            this.chkParagraphs.UseVisualStyleBackColor = true;
            this.chkParagraphs.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkTables
            // 
            this.chkTables.AutoSize = true;
            this.chkTables.Checked = true;
            this.chkTables.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTables.Location = new System.Drawing.Point(12, 101);
            this.chkTables.Name = "chkTables";
            this.chkTables.Size = new System.Drawing.Size(58, 17);
            this.chkTables.TabIndex = 0;
            this.chkTables.Text = "Tables";
            this.chkTables.UseVisualStyleBackColor = true;
            this.chkTables.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkLines
            // 
            this.chkLines.AutoSize = true;
            this.chkLines.Checked = true;
            this.chkLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLines.Location = new System.Drawing.Point(12, 78);
            this.chkLines.Name = "chkLines";
            this.chkLines.Size = new System.Drawing.Size(51, 17);
            this.chkLines.TabIndex = 0;
            this.chkLines.Text = "Lines";
            this.chkLines.UseVisualStyleBackColor = true;
            this.chkLines.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // btnHtmlExport
            // 
            this.btnHtmlExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHtmlExport.Location = new System.Drawing.Point(192, 136);
            this.btnHtmlExport.Name = "btnHtmlExport";
            this.btnHtmlExport.Size = new System.Drawing.Size(104, 23);
            this.btnHtmlExport.TabIndex = 8;
            this.btnHtmlExport.Text = "Export to HTML";
            this.btnHtmlExport.UseVisualStyleBackColor = true;
            this.btnHtmlExport.Click += new System.EventHandler(this.btnHtmlExport_Click);
            // 
            // frmRenderer
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 456);
            this.Controls.Add(this.splitContainer);
            this.Name = "frmRenderer";
            this.Text = "Form1";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.CheckBox chkParagraphs;
        private System.Windows.Forms.CheckBox chkTables;
        private System.Windows.Forms.CheckBox chkLines;
        private System.Windows.Forms.Label lblPageSummary;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Label lblPages;
        private System.Windows.Forms.TextBox txtPageContent;
        private System.Windows.Forms.CheckBox chkText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRead;
        private FileOpen fileOpen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnViewRawContent;
        private System.Windows.Forms.CheckBox chkTextRealSize;
        private System.Windows.Forms.Button btnCheckAllPages;
        private System.Windows.Forms.Button btnHtmlExport;
    }
}

