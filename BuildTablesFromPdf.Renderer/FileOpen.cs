using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BuildTablesFromPdf.Renderer
{
    /// <summary>
    /// Consente di specificare il nome di un file digitandolo (con suggeritore) oppure scegliendolo
    /// mediante l'apposito dialogo di richiesta. E' Anche possibile aprire la cartella in cui il file si trova.
    /// E' necessario che il file esista.
    /// </summary>
    public partial class FileOpen : UserControl
    {
        /// <summary>
        /// Utilizzata dalla omonima proprietà
        /// </summary>
        string _BrowserCaption = "";
        /// <summary>
        /// Utilizzata dalla omonima proprietà
        /// </summary>
        string _Folder = "";
        /// <summary>
        /// Utilizzata dalla omonima proprietà
        /// </summary>
        string _FileFilters = "";
        /// <summary>
        /// Utilizzata dalla omonima proprietà
        /// </summary>
        string _DefaultExtension = "";
        /// <summary>
        /// Variabile che necessita allo usercontrol per la gestione della lunghezza presente dentro il controllo testo
        /// </summary>
        int _LunghezzaText = 0;

        #region ctor

        /// <summary>
        /// Costruttore base
        /// </summary>
        public FileOpen()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // Add any initialization after the InitForm call
            Initialize();
        }


        #endregion

        #region Initialize

        /// <summary>
        /// Custom initialization of the control
        /// </summary>
        private void Initialize()
        {
            // Aggancio l'evento load del controllo      
            this.Load += this.FileOpen_OnLoad;

            // Aggancio gli eventi che devono funzionare anche in design e devono essere eseguiti subito
            this.Resize += this.OnResize;
        }


        /// <summary>
        /// Evento da utilizzare per iniziare eventi e proprietà con la possibilità di discriminare
        /// tra design mode e runtime mode
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>
        private void FileOpen_OnLoad(object sender, System.EventArgs e)
        {
            // --------------------------------------------------------------------
            //Inserire il codice da eseguire anche in design al load del controllo
            // --------------------------------------------------------------------
            //
            //---> ...


            // --------------------------------------------------------------------
            // Da qui in poi inserire il codice da eseguire solo in runtime
            // --------------------------------------------------------------------

            if (this.DesignMode)
                return;

            // Imposto i tooltip
            toolTip.SetToolTip(txtFile, "");
            toolTip.SetToolTip(btnFile, "F7 - Sfoglia");
            toolTip.SetToolTip(btnBrowse, "F8 - Apri file");

            // Aggancio gli eventi che vanno solo in runtime
            this.btnFile.Click += this.OnBtnFileClick;
            this.btnBrowse.Click += this.OnBtnBrowseClick;
            this.txtFile.KeyDown += this.OnTextKeyDown;
            this.txtFile.KeyPress += this.OnTextKeyPress;
            this.txtFile.TextChanged += this.OnTextTextChanged;
            this.txtFile.KeyUp += this.OnTextKeyUp;
            this.txtFile.Enter += this.OnTextEnter;
            this.txtFile.DragDrop += this.OnTextDragDrop;
        }

        #endregion

        #region Event

        /// <summary>
        /// Ridefinisco l'evento TextChanged
        /// </summary>
        new public event EventHandler TextChanged;

        /// <summary>
        /// Ridefinisco l'evento KeyDown
        /// </summary>
        new public event KeyEventHandler KeyDown;

        /// <summary>
        /// Ridefinisco l'evento KeyPress
        /// </summary>
        new public event KeyPressEventHandler KeyPress;

        /// <summary>
        /// Ridefinisco l'evento KeyUp
        /// </summary>
        new public event KeyEventHandler KeyUp;

        /// <summary>
        /// Consente di richiamare il dialogo per la richiesta di un file
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>
        private void OnBtnFileClick(object sender, System.EventArgs e)
        {
            string FileName = "";

            if (txtFile.Text.Trim() != "")
            {
                // Non imposto il folder iniziale ma cerco di capire il folder corretto
                if (txtFile.Text.EndsWith("\\") || txtFile.Text.EndsWith(":"))
                {
                    // Il testo e' tutto un folder perche' finisce con \
                    FileName = OpenFile(_BrowserCaption, txtFile.Text, "", _FileFilters, _DefaultExtension);
                }
                else
                {
                    // Controllo se il file (che esiste) e' una directory
                    System.IO.FileAttributes FileAttr;

                    try
                    {
                        FileAttr = System.IO.File.GetAttributes(txtFile.Text.Trim());

                        if (FileAttr == System.IO.FileAttributes.Directory)
                        {
                            FileName = OpenFile(_BrowserCaption, txtFile.Text, "", _FileFilters, _DefaultExtension);
                        }
                        else
                        {
                            FileName = OpenFile(_BrowserCaption, "", txtFile.Text, _FileFilters, _DefaultExtension);
                        }
                    }
                    catch
                    {
                        // Non esiste niente con quel nome per cui suppongo finisca con un nome di file
                        FileName = OpenFile(_BrowserCaption, "", txtFile.Text, _FileFilters, _DefaultExtension);
                    }
                }
            }
            else
            {
                FileName = OpenFile(_BrowserCaption, _Folder, txtFile.Text, _FileFilters, _DefaultExtension);
            }

            if (FileName.Trim() != "")
            {
                _Folder = Path.GetDirectoryName(FileName);
                txtFile.Text = FileName;
                toolTip.SetToolTip(txtFile, FileName);
            }
        }


        private void OnTextDragDrop(object sender, DragEventArgs e)
        {

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length != 1)
            {
                MessageBox.Show("Select only one file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            txtFile.Text = files[0];

        }

        /// <summary>
        /// Consente di aprire la cartella in cui si trova il file specificato
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>
        private void OnBtnBrowseClick(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process Command = new System.Diagnostics.Process();

            Command.StartInfo.FileName = txtFile.Text;

            try
            {
                Command.Start();
            }
            catch
            {
                MessageBox.Show("Impossibile aprire il file " + Command.StartInfo.FileName, "Apri file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        /// <summary>
        /// Evento scatenato ogni volta che il contenuto del controllo cambia
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>
        private void OnTextTextChanged(object sender, System.EventArgs e)
        {
            if (txtFile.SelectionStart > _LunghezzaText & _LookUp)
            {
                // Esegue l'effetto XL
                FileSystemBrowserHelper.FileLookUp(txtFile, _DefaultExtension);
            }

            _LunghezzaText = txtFile.SelectionStart;

            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }
        }


        /// <summary>
        /// Evento scatenato ogni volta che si preme un tasto (nel momento in cui il tasto è premuto)
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>
        private void OnTextKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                OnBtnFileClick(sender, EventArgs.Empty);

            else if (e.KeyCode == Keys.F8)
            {
                OnBtnBrowseClick(sender, EventArgs.Empty);
            }

            if (KeyDown != null)
                KeyDown(sender, e);

        }


        /// <summary>
        /// Evento scatenato ogni volta che si preme un tasto (nel momento in cui si rilascia il tasto)
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>        
        private void OnTextKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (KeyUp != null)
                KeyUp(sender, e);
        }


        /// <summary>
        /// Evento scatenato ogni volta che si preme un tasto all'interno del controllo
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>    
        private void OnTextKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
                if (e.KeyChar != '\b' && (int)e.KeyChar != 3 && (int)e.KeyChar != 22 && (int)e.KeyChar != 24) // 22 = CTRL+V 3 = CTRL+C 24 = CTRL+X
                    e.Handled = true;

            if (KeyPress != null)
                KeyPress(sender, e);
        }


        /// <summary>
        /// Quando entro nel textbox seleziono tutto il contenuto
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>            
        private void OnTextEnter(object sender, System.EventArgs e)
        {
            txtFile.SelectAll();
        }


        /// <summary>
        /// Si intercetta l'evento resize per gestire il ridimensionamento del controllo
        /// </summary>
        /// <param name="sender">Oggetto inviante l'evento</param>
        /// <param name="e">Argomenti standard dell'evento</param>    
        private void OnResize(object sender, System.EventArgs e)
        {
            this.Height = txtFile.Height;
        }

        #endregion

        #region Private function

        /// <summary>
        /// Apre il dialogo di ricerca file
        /// </summary>
        /// <param name="titolo">Titolo da visualizzare sul dialogo</param>
        /// <param name="initialFolder">Cartella proposta</param>
        /// <param name="initialFile">Nome del file proposto sul dialogo</param>
        /// <param name="filter">Filtro applicato all'elenco dei files</param>
        /// <param name="defaultExtension">Estensione di default</param>
        /// <returns>Ritorna il nome del file scelto il file deve essere necessariamente presente</returns>
        private string OpenFile(string titolo, string initialFolder, string initialFile, string filter, string defaultExtension)
        {

            try
            {
                if (initialFolder != "")
                    System.IO.Path.GetFullPath(initialFolder);

                if (initialFile != "")
                    System.IO.Path.GetFullPath(initialFile);
            }
            catch
            {
                MessageBox.Show("Errore nel formato del percorso inserito.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return string.Empty;
            }

            openFileDialog.Title = titolo;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = defaultExtension;
            openFileDialog.Filter = filter;
            openFileDialog.InitialDirectory = initialFolder;
            openFileDialog.FileName = initialFile;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFile.Focus();
                txtFile.SelectAll();
                return openFileDialog.FileName;
            }
            else
                return "";
        }


        #endregion

        #region Properties

        /// <summary>
        /// Applica la proprietà ReadOnly al controllo disabilitando/abilitando il pulsante
        /// </summary>
        [Category("Appearance")]
        [Description("Applica la proprietà ReadOnly al controllo disabilitando/abilitando il pulsante")]
        public bool ReadOnly
        {
            get
            { return txtFile.ReadOnly; }
            set
            {
                btnFile.Enabled = !value;
                btnBrowse.Enabled = !value;
                txtFile.ReadOnly = value;
            }
        }

        /// <summary>
        /// Ritorna il contenuto del controllo testo
        /// </summary>
        public override string Text
        {
            get
            { return Value; }
        }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get
            {
                if (txtFile.Text.Trim() == string.Empty || (_CheckFileExists && !File.Exists(txtFile.Text)))
                    return null;
                else
                    return txtFile.Text;
            }
            set
            {
                if (value == null)
                    txtFile.Text = string.Empty;
                else
                    txtFile.Text = value;
            }
        }



        private const bool LookUpDefaultValue = true;

        private bool _LookUp = LookUpDefaultValue;

        [Category("Appearance")]
        [Description("Abilita/Disabilita il suggeritore durante la digitazione")]
        [DefaultValue(LookUpDefaultValue)]
        public bool LookUp
        {
            get
            {
                return _LookUp;
            }
            set
            {
                _LookUp = value;
            }
        }

        private void ResetLookUp()
        {
            LookUp = LookUpDefaultValue;
        }

        private const bool CheckFileExistsDefaultValue = false;

        private bool _CheckFileExists = CheckFileExistsDefaultValue;

        [Category("Appearance")]
        [Description("Controlla se il file esiste (e ritorna null se non esiste)")]
        [DefaultValue(CheckFileExistsDefaultValue)]
        public bool CheckFileExists
        {
            get
            {
                return _CheckFileExists;
            }
            set
            {
                _CheckFileExists = value;
            }
        }

        private void ResetCheckFileExists()
        {
            CheckFileExists = CheckFileExistsDefaultValue;
        }



        /// <summary>
        /// Titolo del dialogo di ricerca file
        /// </summary>
        [Category("Appearance")]
        [Description("Titolo del dialogo di ricerca file")]
        public string BrowserCaption
        {
            get
            { return _BrowserCaption; }
            set
            { _BrowserCaption = value; }
        }


        /// <summary>
        /// Rappresenta la directory di partenza da cui iniziare a visualizzare nel dialogo di ricerca dei files
        /// </summary>
        [Category("Appearance")]
        [Description("Rappresenta la directory di partenza da cui iniziare a visualizzare nel dialogo di ricerca dei files")]
        public string Folder
        {
            get
            { return _Folder; }
            set
            { _Folder = value; }
        }

        /// <summary>
        /// Filtro di ricerca da applicare nel dialogo di ricerca dei files
        /// </summary>
        [Category("Appearance")]
        [Description("Filtro di ricerca da applicare nel dialogo di ricerca dei files")]
        public string FileFilters
        {
            get
            { return _FileFilters; }
            set
            { _FileFilters = value; }
        }

        /// <summary>
        /// Estensione di default da utilizzare nel dialogo di ricerca dei files
        /// </summary>
        [Category("Appearance")]
        [Description("Estensione di default da utilizzare nel dialogo di ricerca dei files")]
        public string DefaultExtension
        {
            get
            { return _DefaultExtension; }
            set
            { _DefaultExtension = value; }
        }


        #endregion

        #region IReaderControl Members


        /// <summary>
        /// Ritorna, correttamente formattato come stringa, ciò che è contenuto nel parametro ID
        /// </summary>
        /// <param name="value">Valore</param>
        /// <returns>L'oggetto da formattare</returns>
        public string Format(object value)
        {
            return value.ToString(); ;
        }

        /// <summary>
        /// Indica se catturare o meno gli eventi di Enter e Leave
        /// </summary>
        /// <param name="capture">true per catturare gli eventi false altrimenti</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void CaptureFocus(bool capture)
        {
        }

        #endregion
    }
}
