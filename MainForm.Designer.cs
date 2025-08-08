namespace MySimpleNotes
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.doneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlTabButtons = new System.Windows.Forms.Panel();
            this.btnUrlsTab = new System.Windows.Forms.Button();
            this.btnNotesTab = new System.Windows.Forms.Button();
            this.pnlTabContent = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnlTabButtons.SuspendLayout();
            this.pnlTabContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(325, 319);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyUp);
            this.richTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
            this.richTextBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseMove);
            this.richTextBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doneToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(242, 28);
            // 
            // doneToolStripMenuItem
            // 
            this.doneToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.doneToolStripMenuItem.Name = "doneToolStripMenuItem";
            this.doneToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.doneToolStripMenuItem.Size = new System.Drawing.Size(241, 24);
            this.doneToolStripMenuItem.Text = "Toggle Strikethrough";
            this.doneToolStripMenuItem.Click += new System.EventHandler(this.doneToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, -25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(339, 354);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(331, 325);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Notes";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.DarkSlateGray;
            this.tabPage2.Controls.Add(this.flowLayoutPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(331, 325);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "URLs";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(325, 319);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "LT-Notes";
            this.notifyIcon1.Visible = true;
            // 
            // pnlTabButtons
            // 
            this.pnlTabButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlTabButtons.Controls.Add(this.btnUrlsTab);
            this.pnlTabButtons.Controls.Add(this.btnNotesTab);
            this.pnlTabButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlTabButtons.Name = "pnlTabButtons";
            this.pnlTabButtons.Size = new System.Drawing.Size(339, 40);
            this.pnlTabButtons.TabIndex = 2;
            // 
            // btnUrlsTab
            // 
            this.btnUrlsTab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUrlsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUrlsTab.FlatAppearance.BorderSize = 0;
            this.btnUrlsTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrlsTab.Location = new System.Drawing.Point(170, 0);
            this.btnUrlsTab.Name = "btnUrlsTab";
            this.btnUrlsTab.Size = new System.Drawing.Size(169, 40);
            this.btnUrlsTab.TabIndex = 1;
            this.btnUrlsTab.Text = "URLs";
            this.btnUrlsTab.UseVisualStyleBackColor = true;
            this.btnUrlsTab.Click += new System.EventHandler(this.btnUrlsTab_Click);
            // 
            // btnNotesTab
            // 
            this.btnNotesTab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNotesTab.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNotesTab.FlatAppearance.BorderSize = 0;
            this.btnNotesTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotesTab.Location = new System.Drawing.Point(0, 0);
            this.btnNotesTab.Name = "btnNotesTab";
            this.btnNotesTab.Size = new System.Drawing.Size(170, 40);
            this.btnNotesTab.TabIndex = 0;
            this.btnNotesTab.Text = "Notes";
            this.btnNotesTab.UseVisualStyleBackColor = true;
            this.btnNotesTab.Click += new System.EventHandler(this.btnNotesTab_Click);
            // 
            // pnlTabContent
            // 
            this.pnlTabContent.Controls.Add(this.tabControl1);
            this.pnlTabContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabContent.Location = new System.Drawing.Point(0, 40);
            this.pnlTabContent.Name = "pnlTabContent";
            this.pnlTabContent.Size = new System.Drawing.Size(339, 329);
            this.pnlTabContent.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(339, 369);
            this.Controls.Add(this.pnlTabContent);
            this.Controls.Add(this.pnlTabButtons);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LT-Notes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.pnlTabButtons.ResumeLayout(false);
            this.pnlTabContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem doneToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Panel pnlTabButtons;
        private System.Windows.Forms.Button btnUrlsTab;
        private System.Windows.Forms.Button btnNotesTab;
        private System.Windows.Forms.Panel pnlTabContent;
    }
}