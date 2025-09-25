using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySimpleNotes
{
    public partial class MainForm : Form
    {
        private AppSettings _settings;
        private Panel _lastCopiedUrlPanel = null;
        private bool _areUrlsRendered = false;

        public MainForm()
        {
            InitializeComponent();
            Program.LinkSaved += OnLinkSaved;
        }

        #region Key-Press Handling and Shortcuts

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                HandleImportExport();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // *** RESTORED THIS METHOD TO FIX THE DESIGNER ERROR ***
        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                PromptAndExit();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                ToggleStrikethrough();
            }
        }

        #endregion

        #region Form Load and Save Logic

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadAllSettings();
            EnableMouseWheelFun();
            SetupNotifyIcon();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                RefreshTheFormStyle();
            }
        }

        // *** MODIFIED TO BE ASYNC ***
        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // --- ASYNC LAZY LOADING LOGIC ---
            if (tabControl1.SelectedTab == tabPage2 && !_areUrlsRendered)
            {
                await RenderUrlBoxesAsync(); // Await the non-blocking version
                _areUrlsRendered = true;
            }

            if (tabControl1.SelectedTab == tabPage2)
            {
                RefreshTheFormStyle();
            }
            UpdateTabButtonStyles();
        }

        private void RefreshTheFormStyle()
        {
            this.Size = new Size(this.Width + 1, this.Height);
            this.Size = new Size(this.Width - 1, this.Height);
        }

        private void LoadAllSettings()
        {
            _settings = SettingsManager.Load();

            if (_settings.WindowLocation.HasValue) this.Location = _settings.WindowLocation.Value;
            if (_settings.WindowSize.HasValue && _settings.WindowSize.Value.Width > 0)
                this.Size = _settings.WindowSize.Value;
            else
                this.Size = new Size(350, 400);

            if (!string.IsNullOrEmpty(_settings.NotesRtf))
            {
                richTextBox1.Rtf = _settings.NotesRtf;
            }

            if (_settings.LastTabIndex >= 0 && _settings.LastTabIndex < tabControl1.TabCount)
            {
                tabControl1.SelectedIndex = _settings.LastTabIndex;
            }

            UpdateTabButtonStyles();
        }

        private void SaveAllSettings()
        {
            _settings.NotesRtf = richTextBox1.Rtf;
            _settings.WindowLocation = this.Location;
            _settings.WindowSize = this.Size;
            _settings.LastTabIndex = tabControl1.SelectedIndex;
            SettingsManager.Save(_settings);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) { SaveAllSettings(); return; }
            e.Cancel = true;
            DialogResult result = MessageBox.Show(this, "Do you want to save changes before exiting?", "Exit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Yes: Application.Exit(); break;
                case DialogResult.No: Program.LinkSaved -= OnLinkSaved; Environment.Exit(0); break;
                case DialogResult.Cancel: break;
            }
        }

        private void PromptAndExit() { this.Close(); }

        private void HandleImportExport()
        {
            SaveAllSettings();
            using (var dialog = new ImportExportDialog())
            {
                dialog.ShowDialog(this);
                switch (dialog.UserChoice)
                {
                    case ImportExportChoice.Export: ExportData(); break;
                    case ImportExportChoice.Import: ImportData(); break;
                    case ImportExportChoice.Cancel: break;
                }
            }
        }

        private void ExportData()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                sfd.Title = "Export Notes Data As...";
                sfd.FileName = "MySimpleNotes_Backup.json";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try { File.Copy(SettingsManager.FilePath, sfd.FileName, true); MessageBox.Show("Data exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    catch (Exception ex) { MessageBox.Show($"An error occurred during export: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
        }

        private void ImportData()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                ofd.Title = "Import Notes Data From...";
                ofd.FileName = "MySimpleNotes_Backup.json";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string jsonToImport = File.ReadAllText(ofd.FileName);
                        var importedSettings = JsonSerializer.Deserialize<AppSettings>(jsonToImport);
                        if (importedSettings == null) { throw new JsonException("The selected file is not a valid settings file or is empty."); }
                        File.WriteAllText(SettingsManager.FilePath, jsonToImport);
                        _areUrlsRendered = false;
                        LoadAllSettings();
                        MessageBox.Show("Data imported successfully!\nYour notes and URLs have been updated.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex) { MessageBox.Show($"An error occurred during import: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
        }

        #endregion

        #region System Tray (NotifyIcon) Logic
        private void ShowAndFocusForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        private void SetupNotifyIcon()
        {
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += (s, e) => ShowAndFocusForm();
            var menu = new ContextMenuStrip();
            menu.Items.Add("Show Notes", null, (s, e) => ShowAndFocusForm());
            menu.Items.Add("Exit", null, (s, e) => PromptAndExit());
            notifyIcon1.ContextMenuStrip = menu;
        }
        #endregion

        #region URL Handling

        // *** NEW ASYNC VERSION ***
        // This builds the UI without freezing the application.
        private async Task RenderUrlBoxesAsync()
        {
            lblLoading.Visible = true;
            flowLayoutPanel1.Visible = false; // Hide panel to prevent flicker
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();
            _lastCopiedUrlPanel = null;

            int batchSize = 25; // Render 25 controls before pausing
            int count = 0;

            foreach (UrlEntry entry in _settings.SavedUrls.OrderByDescending(e => e.SavedAt))
            {
                AddUrlBox(entry);
                count++;
                if (count % batchSize == 0)
                {
                    // This is the key: it pauses the loop for a tiny moment,
                    // allowing the UI to process events (like repainting)
                    // and stay responsive.
                    await Task.Delay(1);
                }
                // For showing the count value
                btnUrlsTab.Text = string.Format("URLs ({0})", count);
            }

            flowLayoutPanel1.ResumeLayout(true);
            lblLoading.Visible = false;
            flowLayoutPanel1.Visible = true;
        }

        // This is now only used for testing or a full synchronous reload if ever needed.
        // It is no longer the primary method for lazy loading.
        private void RenderUrlBoxes()
        {
            _lastCopiedUrlPanel = null;
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();
            foreach (UrlEntry entry in _settings.SavedUrls.OrderByDescending(e => e.SavedAt)) { AddUrlBox(entry); }
            flowLayoutPanel1.ResumeLayout(true);
        }

        private string FormatTimestamp(DateTime timestamp)
        {
            var now = DateTime.Now;
            var today = now.Date;
            var yesterday = today.AddDays(-1);
            if (timestamp.Date == today) return $"Today, {timestamp:h:mm tt}";
            if (timestamp.Date == yesterday) return $"Yesterday, {timestamp:h:mm tt}";
            if (timestamp.Year == now.Year) return timestamp.ToString("MMM d, h:mm tt");
            return timestamp.ToString("g");
        }

        private Panel AddUrlBox(UrlEntry entry)
        {
            int panelWidth = flowLayoutPanel1.ClientSize.Width > 50 ? flowLayoutPanel1.ClientSize.Width - 25 : 50;
            var originalColor = Color.FromArgb(50, 50, 50);
            var highlightColor = Color.FromArgb(45, 85, 45);
            var urlPanel = new Panel { Width = panelWidth, Height = 40, Margin = new Padding(5), BackColor = originalColor, Tag = entry };
            var timeLabel = new Label { Text = FormatTimestamp(entry.SavedAt), ForeColor = Color.Gray, Font = new Font("Tahoma", 7), Dock = DockStyle.Right, AutoSize = false, Width = 140, TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(0, 0, 5, 0) };
            var urlLabel = new Label { Text = GetDisplayUrl(entry.Url, urlPanel.Width - timeLabel.Width), ForeColor = Color.LightSkyBlue, Font = new Font("Tahoma", 8, FontStyle.Regular), Dock = DockStyle.Fill, Cursor = Cursors.Hand, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(5, 0, 0, 0), AutoEllipsis = true };
            Action openAction = () => { try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(entry.Url) { UseShellExecute = true }); } catch (Exception ex) { MessageBox.Show("Could not open URL: " + ex.Message); } };
            Action copyAction = () => { if (_lastCopiedUrlPanel != null) _lastCopiedUrlPanel.BackColor = originalColor; Clipboard.SetText(entry.Url); ToastForm.ShowToast(this, "URL Copied!"); urlPanel.BackColor = highlightColor; _lastCopiedUrlPanel = urlPanel; };
            urlLabel.DoubleClick += (s, e) => openAction();
            urlLabel.Click += (s, e) => copyAction();
            var menu = new ContextMenuStrip();
            menu.Items.Add("Open in browser", null, (s, e) => openAction());
            menu.Items.Add("Copy URL", null, (s, e) => copyAction());
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Delete", null, (s, e) => { if (_lastCopiedUrlPanel == urlPanel) _lastCopiedUrlPanel = null; _settings.SavedUrls.Remove(entry); flowLayoutPanel1.Controls.Remove(urlPanel); SaveAllSettings(); });
            urlLabel.ContextMenuStrip = menu;
            timeLabel.ContextMenuStrip = menu;
            urlPanel.ContextMenuStrip = menu;
            new ToolTip().SetToolTip(urlLabel, $"Left-Click: Copy\nDouble-Click: Open\nRight-Click: Options\n\n{entry.Url}");
            new ToolTip().SetToolTip(timeLabel, $"Saved on: {entry.SavedAt:F}");
            new ToolTip().SetToolTip(urlPanel, $"Left-Click: Copy\nDouble-Click: Open\nRight-Click: Options\n\n{entry.Url}");
            urlPanel.Controls.Add(urlLabel);
            urlPanel.Controls.Add(timeLabel);
            flowLayoutPanel1.Controls.Add(urlPanel);
            return urlPanel;
        }

        public void OnLinkSaved(string link)
        {
            if (InvokeRequired) { Invoke(new Action(() => OnLinkSaved(link))); return; }
            if (!_settings.SavedUrls.Any(entry => entry.Url == link))
            {
                var newEntry = new UrlEntry { Url = link, SavedAt = DateTime.Now };
                _settings.SavedUrls.Add(newEntry);

                if (_areUrlsRendered)
                {
                    Panel newUrlPanel = AddUrlBox(newEntry);
                    flowLayoutPanel1.Controls.SetChildIndex(newUrlPanel, 0);
                }

                SaveAllSettings();
                notifyIcon1.ShowBalloonTip(2000, "URL Captured", link, ToolTipIcon.Info);
                if (this.Visible) tabControl1.SelectedIndex = 1;
            }
        }

        private string GetDisplayUrl(string url, int controlWidth)
        {
            const int avgCharWidth = 7;
            const int ellipsisWidth = 20;
            int maxChars = (controlWidth - ellipsisWidth) / avgCharWidth;
            if (url.Length <= maxChars || maxChars < 10) return url;
            int frontChars = (int)(maxChars * 0.6);
            int endChars = maxChars - frontChars;
            return url.Substring(0, frontChars) + "..." + url.Substring(url.Length - endChars);
        }

        private void UpdateUrlBoxSizes()
        {
            if (!_areUrlsRendered) return; // Don't do this if the UI isn't built yet
            flowLayoutPanel1.SuspendLayout();
            foreach (Panel panel in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                int newWidth = flowLayoutPanel1.ClientSize.Width - 25;
                if (newWidth < 150) newWidth = 150;
                panel.Width = newWidth;
                var urlLabel = panel.Controls.OfType<Label>().FirstOrDefault(lbl => lbl.Dock == DockStyle.Fill);
                var timeLabel = panel.Controls.OfType<Label>().FirstOrDefault(lbl => lbl.Dock == DockStyle.Right);
                if (urlLabel != null && timeLabel != null && panel.Tag is UrlEntry entry) { urlLabel.Text = GetDisplayUrl(entry.Url, panel.Width - timeLabel.Width); }
            }
            flowLayoutPanel1.ResumeLayout(true);
        }
        #endregion

        #region RichTextBox and Note Formatting

        private void ToggleStrikethrough()
        {
            if (richTextBox1.SelectionLength > 0)
            {
                Font currentFont = richTextBox1.SelectionFont ?? richTextBox1.Font;
                FontStyle newStyle;
                if (currentFont.Strikeout) { newStyle = currentFont.Style & ~FontStyle.Strikeout; richTextBox1.SelectionColor = Color.White; }
                else { newStyle = currentFont.Style | FontStyle.Strikeout; richTextBox1.SelectionColor = Color.Gray; }
                richTextBox1.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void doneToolStripMenuItem_Click(object sender, EventArgs e) { ToggleStrikethrough(); }
        #endregion

        #region Form Movement, Sizing, and Events

        private void MoveCursor() { this.Location = new Point(Cursor.Position.X - (this.Width / 2), Cursor.Position.Y - 15); }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Middle) MoveCursor(); }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Middle) MoveCursor(); }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Right) { contextMenuStrip1.Show(Cursor.Position); } }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            UpdateUrlBoxSizes();
            int halfWidth = pnlTabButtons.ClientSize.Width / 2;
            btnNotesTab.Width = halfWidth;
        }

        private void EnableMouseWheelFun()
        {
            this.MouseWheel += MainForm_MouseWheel;
            richTextBox1.MouseWheel += MainForm_MouseWheel;
            flowLayoutPanel1.MouseWheel += MainForm_MouseWheel;
            tabControl1.MouseWheel += MainForm_MouseWheel;
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control) return;
            int change = e.Delta > 0 ? 20 : -20;
            int newWidth = Math.Max(200, this.Width + change);
            int newHeight = Math.Max(150, this.Height + change);
            this.Size = new Size(newWidth, newHeight);
        }
        #endregion

        #region Tab Button Logic

        private void btnNotesTab_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void btnUrlsTab_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void UpdateTabButtonStyles()
        {
            Color activeBackColor = Color.DarkSlateGray;
            Color activeForeColor = Color.White;
            Color inactiveBackColor = Color.FromArgb(45, 45, 48);
            Color inactiveForeColor = Color.LightGray;

            if (tabControl1.SelectedIndex == 0)
            {
                btnNotesTab.BackColor = activeBackColor;
                btnNotesTab.ForeColor = activeForeColor;
                btnUrlsTab.BackColor = inactiveBackColor;
                btnUrlsTab.ForeColor = inactiveForeColor;
            }
            else
            {
                btnNotesTab.BackColor = inactiveBackColor;
                btnNotesTab.ForeColor = inactiveForeColor;
                btnUrlsTab.BackColor = activeBackColor;
                btnUrlsTab.ForeColor = activeForeColor;
            }
        }

        #endregion
    }
}