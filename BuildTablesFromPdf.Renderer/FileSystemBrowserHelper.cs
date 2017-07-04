using System;
using System.IO;

namespace BuildTablesFromPdf.Renderer
{

    /// <summary>
    /// Funzioni per gli user control
    /// </summary>
    internal class FileSystemBrowserHelper
    {
        public static void DirectoryLookUp(System.Windows.Forms.TextBox txt)
        {
            int lunghezza;
            string[] nomeCartelle;
            string nomeCartella;
            string basePath;
            string basePattern;

            // Path su cui eseguiro' la ricerca
            basePath = txt.Text;

            // Controllo se il path e' gia' corretto e aggiungo il pattern per eseguire la ricerca
            if (basePath.EndsWith("\\"))
            {
                basePattern = "*";
            }
            else if (basePath.EndsWith(":"))
            {
                basePath = basePath + "\\";
                basePattern = "*";
            }
            else
            {
                // Utilizzato General.StripPath() perchè System.IO.Path.GetDirectoryName() con '\\aaa' ritorna null al posto di '\\' 
                basePath = StripPath(basePath);
                basePattern = txt.Text.Substring(basePath.Length) + "*";
            }

            lunghezza = txt.Text.Length;

            // Carico la struttura della cartella nell'array
            try
            {
                nomeCartelle = Directory.GetDirectories(basePath, basePattern);
            }
            catch
            {
                return;
            }

            // Controllo se ho trovato almeno una cartella che matcha il criterio di ricerca
            // Se l'ho trovata utilizzo la prima
            if (nomeCartelle.Length != 0)
                nomeCartella = nomeCartelle[0];
            else
                nomeCartella = "";

            // Controllo se ho trovato la cartella
            if (nomeCartella != "")
            {
                // Imposto il nuovo testo con il nome della cartella
                txt.Text = nomeCartella;
                // Riposiziona il cursore alla vecchia posizione
                txt.SelectionStart = lunghezza;
                // Seleziona la nuova parte aggiunta della cartella
                txt.SelectionLength = txt.Text.Length - lunghezza;
            }
        }

        public static void FileLookUp(System.Windows.Forms.TextBox txt, string extension)
        {
            int lunghezza;
            string nomeFile;
            string[] nomeFiles;
            string basePath;
            string basePattern;

            basePath = txt.Text;
            if (basePath.EndsWith("\\"))
            {
                basePattern = "*";
            }
            else if (basePath.EndsWith(":"))
            {
                basePath = basePath + "\\";
                basePattern = "*";
            }
            else
            {
                // Utilizzato General.StripPath() perchè System.IO.Path.GetDirectoryName() con '\\aaa' ritorna null al posto di '\\' 
                basePath = StripPath(basePath);
                basePattern = txt.Text.Substring(basePath.Length) + "*";
            }

            if (extension != "" && extension != null)
                basePattern = basePattern + (extension.StartsWith(".") ? "" : ".") + extension;

            lunghezza = txt.Text.Length;

            // Carico la struttura della cartella nell'array
            try
            {
                nomeFiles = System.IO.Directory.GetFileSystemEntries(basePath, basePattern);
            }
            catch
            {
                nomeFiles = new string[0];
            }

            if (nomeFiles.Length != 0)
            {
                int lnI;

                for (lnI = 0; lnI < nomeFiles.Length; lnI++)
                    if (nomeFiles[lnI].ToLower().StartsWith(txt.Text.ToLower()))
                        break;

                nomeFile = nomeFiles[lnI];
            }
            else
                nomeFile = "";

            if (nomeFile != "")
            {
                txt.Text = nomeFile;
                txt.SelectionStart = lunghezza;
                txt.SelectionLength = txt.Text.Length - lunghezza;
            }
            else
                DirectoryLookUp(txt);
        }
        
        /// <summary>
        /// Da un nome di file completo contenente anche il path restituisce solo il path
        /// </summary>
        /// <param name="fullPath">Nome del file con il path</param>
        /// <returns>Path senza nome del file</returns>
        private static string StripPath(string fullPath)
        {
            int i;

            i = fullPath.Length;

            if (i == 0)
                return string.Empty;

            while (i > 1 & fullPath.Substring(i - 1, 1) != "\\")
                i--;

            if (fullPath.Substring(i - 1, 1) == "\\")
                return fullPath.Substring(0, i);
            else
                return string.Empty;
        }
    }
}
