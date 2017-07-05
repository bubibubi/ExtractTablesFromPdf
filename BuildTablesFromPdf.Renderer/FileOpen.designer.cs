namespace BuildTablesFromPdf.Renderer
{
  partial class FileOpen
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

    #region Component Designer generated code
    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileOpen));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFile
            // 
            this.btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFile.Image = ((System.Drawing.Image)(resources.GetObject("btnFile.Image")));
            this.btnFile.Location = new System.Drawing.Point(194, 1);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(18, 18);
            this.btnFile.TabIndex = 1;
            this.btnFile.TabStop = false;
            // 
            // txtFile
            // 
            this.txtFile.AllowDrop = true;
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(0, 0);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(192, 20);
            this.txtFile.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.Location = new System.Drawing.Point(213, 1);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(18, 18);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.TabStop = false;
            // 
            // FileOpen
            // 
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.txtFile);
            this.Name = "FileOpen";
            this.Size = new System.Drawing.Size(232, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

    }


    #endregion

    /// <summary>
    /// Oggetto che consente l'apertura del dialogo per la ricerca di un file
    /// </summary>
    private System.Windows.Forms.OpenFileDialog openFileDialog;

    /// <summary>
    /// Pulsante per l'apertura del dialogo ricerca file
    /// </summary>
    private System.Windows.Forms.Button btnFile;

    /// <summary>
    /// Controllo per contenere il percorso del file
    /// </summary>
    private System.Windows.Forms.TextBox txtFile;

    /// <summary>
    /// Per mostrare messaggi sugli oggetti presenti nel controllo
    /// </summary>
    private System.Windows.Forms.ToolTip toolTip;

    /// <summary>
    /// Per aprire la cartella in cui si trova il file
    /// </summary>
    private System.Windows.Forms.Button btnBrowse;


  }
}
