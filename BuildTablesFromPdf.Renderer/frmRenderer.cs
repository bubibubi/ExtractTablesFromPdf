using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BuildTablesFromPdf.Engine;
using BuildTablesFromPdf.Engine.Statements;
using BuildTablesFromPdf.Engine.Tables;

namespace BuildTablesFromPdf.Renderer
{
    public partial class frmRenderer : Form
    {
        private PageCollection _pages;

        public Page CurrentPage { get; set; }



        public frmRenderer()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            fileOpen.Value = Properties.Settings.Default.FileName;
        }

        private void ShowDocument(string fileName)
        {
            _pages = ExtractText.Read(fileName);
            lblPages.Text = _pages.Count.ToString();
            DrawPage(0);
        }


        private void DrawPage(int pageIndex)
        {
            if (!_pages[pageIndex].IsRefreshed)
            {
                _pages[pageIndex].DetermineTableStructures();
                _pages[pageIndex].DetermineParagraphs();

                _pages[pageIndex].FillContent();
            }

            txtPage.Text = (pageIndex + 1).ToString();
            CurrentPage = _pages[pageIndex];
            txtPageContent.Text = _pages[pageIndex].ToString();
            RedrawLines();

        }

        private void RedrawLines()
        {
            int pageHeight = splitContainer.Panel2.Height;

            if (CurrentPage == null)
                return;

            using (var g = splitContainer.Panel2.CreateGraphics())
            {

                g.Clear(this.BackColor);

                if (chkLines.Checked)
                {
                    foreach (Line line in CurrentPage.AllLines)
                        g.DrawLine(Pens.DarkGray, line.StartPoint.X, pageHeight - line.StartPoint.Y, line.EndPoint.X, pageHeight - line.EndPoint.Y);

                    foreach (Line line in CurrentPage.JoinedHorizontalLines)
                        g.DrawLine(Pens.Blue, line.StartPoint.X + 2, pageHeight - line.StartPoint.Y + 2, line.EndPoint.X + 2, pageHeight - line.EndPoint.Y + 2);

                    foreach (Line line in CurrentPage.JoinedVerticalLines)
                        g.DrawLine(Pens.Blue, line.StartPoint.X + 2, pageHeight - line.StartPoint.Y + 2, line.EndPoint.X + 2, pageHeight - line.EndPoint.Y + 2);
                }

                if (chkTables.Checked)
                {
                    foreach (Table tableStructure in CurrentPage.Tables)
                    {
                        g.DrawRectangle(Pens.OrangeRed, tableStructure.TopLeftPoint.X + 4, pageHeight - tableStructure.BottomRightPoint.Y + 4, tableStructure.Width, tableStructure.Heigth);

                        if (chkLines.Checked)
                        {
                            // To avoid too many lines
                            foreach (Row row in tableStructure.Rows)
                                g.FillRectangle(Brushes.OrangeRed, tableStructure.TopLeftPoint.X + 5, pageHeight - row.EndY + 5, 4, 4);

                            foreach (Column column in tableStructure.Columns)
                                g.FillRectangle(Brushes.OrangeRed, column.BeginX + 5, pageHeight - tableStructure.BottomRightPoint.Y + 5, 4, 4);
                            
                        }
                        else
                        {
                            for (int i = 0; i < tableStructure.Rows.Count - 1; i++)
                            {
                                Row row = tableStructure.Rows[i];
                                g.DrawLine(Pens.OrangeRed, tableStructure.TopLeftPoint.X + 5, pageHeight - row.EndY + 5, tableStructure.BottomRightPoint.X + 5, pageHeight - row.EndY + 5);
                            }

                            for (int i = 1; i < tableStructure.Columns.Count; i++)
                            {
                                Column column = tableStructure.Columns[i];
                                g.DrawLine(Pens.OrangeRed, column.BeginX + 5, pageHeight - tableStructure.BottomRightPoint.Y + 5, column.BeginX + 5, pageHeight - tableStructure.TopLeftPoint.Y + 5);
                            }
                        }
                    }
                }

                if (chkParagraphs.Checked)
                {
                    foreach (Paragraph paragraph in CurrentPage.Paragraphs)
                        g.FillRectangle(Brushes.OrangeRed, 0,pageHeight - paragraph.Y + 5, 10, 4);
                }

                if (chkText.Checked)
                {
                    if (chkTextRealSize.Checked)
                    {
                        foreach (var line in CurrentPage.Statements.Where(_ => _ is TextObjectStatement).Cast<TextObjectStatement>().SelectMany(_ => _.Lines).Where(_ => _.FontHeight > 0))
                        {
                            Font font = new Font("Arial", line.FontHeight * 0.7f);
                            g.DrawString(line.Content, font, Brushes.Black, line.Position.X + 4, pageHeight - line.Position.Y + 4);
                        }
                    }
                    else
                    {
                        foreach (var line in CurrentPage.Statements.Where(_ => _ is TextObjectStatement).Cast<TextObjectStatement>().SelectMany(_ => _.Lines))
                            g.DrawString(line.Content, this.Font, Brushes.Black, line.Position.X + 4, pageHeight - line.Position.Y + 4);
                    }
                }
            }
        }

        private void splitContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {
            RedrawLines();
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            RedrawLines();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            DrawPage(0);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage.Index > 0)
                DrawPage(CurrentPage.Index - 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage.Index < _pages.Count - 1)
                DrawPage(CurrentPage.Index + 1);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            DrawPage(_pages.Count - 1);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            int page;
            if (int.TryParse(txtPage.Text, out page) && page > 0 && page <= _pages.Count)
                DrawPage(page - 1);

        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fileOpen.Text))
                return;

            ShowDocument(fileOpen.Text);

            Properties.Settings.Default.FileName = fileOpen.Text;
            Properties.Settings.Default.Save();
        }

        private void btnViewRawContent_Click(object sender, EventArgs e)
        {
            if (_pages == null)
                return;

            int page;
            if (!int.TryParse(txtPage.Text, out page) && page > 0 && page <= _pages.Count)
                return;

            string textFromPage = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, _pages.PdfReader.GetPageContent(page)));
            textFromPage = textFromPage.Replace("\n", "\r\n");
            new frmNotepad().Start(textFromPage);

        }
    }
}
