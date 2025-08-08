using System;
using System.Windows.Forms;

namespace MySimpleNotes
{
    // An enum to clearly represent the user's choice.
    public enum ImportExportChoice
    {
        Export,
        Import,
        Cancel
    }

    public partial class ImportExportDialog : Form
    {
        // A public property to hold the result.
        public ImportExportChoice UserChoice { get; private set; }

        public ImportExportDialog()
        {
            InitializeComponent();
            // Default choice is Cancel if the user closes the form via the 'X' button.
            UserChoice = ImportExportChoice.Cancel;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            UserChoice = ImportExportChoice.Export;
            this.DialogResult = DialogResult.OK; // Set DialogResult
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            UserChoice = ImportExportChoice.Import;
            this.DialogResult = DialogResult.OK; // Set DialogResult
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UserChoice = ImportExportChoice.Cancel;
            this.DialogResult = DialogResult.Cancel; // Set DialogResult
            this.Close();
        }
    }
}