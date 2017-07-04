using System;
using System.Windows.Forms;

namespace BuildTablesFromPdf.Renderer
{
    public partial class frmNotepad : Form
    {
        public void Start(string content)
        {
            txtNotepad.Text = content;
            Show();
        }


        public frmNotepad()
        {
            InitializeComponent();
        }
    }
}
