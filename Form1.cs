using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace MySimpleNotes
{
    public partial class Form1 : Form
    {
        static readonly string myPath = @"C:\MySimpleNotes\MySimpleNotes_AllText.txt";
        static readonly string myLastLocation = @"C:\MySimpleNotes\MySimpleNotes_LastLocation.txt";
        static readonly string myStyle = @"C:\MySimpleNotes\MyStyle.txt";

        public Form1()
        {
            InitializeComponent();
            this.MyInitialize();
            Program.LinkSaved += OnLinkSaved; // Subscribe to the LinkSaved event
        }


        public void MyInitialize()
        {
            try
            {
               
                //for first timp 
                if (!System.IO.Directory.Exists(@"C:\MySimpleNotes\MySimpleNotes_AllText.txt"))
                    System.IO.Directory.CreateDirectory(@"C:\MySimpleNotes");

                 //test if the text file is alredy exists or not
                if (File.Exists(myPath))
                {
                    this.richTextBox1.Text = "" + ImportDataFromTXT(myPath);
                    //get last location
                    string lastLocationText = ImportDataFromTXT(myLastLocation).Split('|')[0];
                    this.Location = new Point(int.Parse(lastLocationText.Split(',')[0]), int.Parse(lastLocationText.Split(',')[1]));
                    //get last form size
                    string lastFormSizeText = ImportDataFromTXT(myLastLocation).Split('|')[1];
                    this.MainFormSize(lastFormSizeText.Split(',')[0], lastFormSizeText.Split(',')[1]);
                    //get text style                
                    this.StyleSave(true);

                    //get the clipboard text and catch any url
                    //string clipboardText = Clipboard.GetText();
                    //if (clipboardText.Length > 1)
                    //{
                    //    if (clipboardText.Contains("http"))
                    //    {
                    //        if (this.richTextBox1.Text.Contains(clipboardText) == false)
                    //            this.richTextBox1.Text += "\n" + clipboardText;
                    //    }
                    //}

                }
                else
                {
                    //create text files
                    ExportDataToTXT(this.richTextBox1.Text, myPath);
                    ExportDataToTXT(("" + this.Location.X + "," + this.Location.Y + "|" + this.Width + "," + this.Height), myLastLocation);
                    ExportDataToTXT("", myStyle);
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //This method appends the new link to richTextBox1
        public void OnLinkSaved(string link)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnLinkSaved(link)));
               // return;
            }

            if (CheckMultiUrls(link))
            {
                return;
            }
            else
            {
                SetTheSingleLine(link);          
            }

        }

        private void SetTheSingleLine(string line)
        {
            if (!CheckSingleLine(line))
            {
                // To make sure we but the link on new line
                this.CheckForNewLine();
                this.richTextBox1.AppendText(line + Environment.NewLine); // Append the new link if it's not already present
            }
        }
        private bool CheckSingleLine(string singleLine)
        {
            return this.richTextBox1.Text.Contains(singleLine);
        }
        private bool CheckMultiUrls(string textClip)
        {
            // Check if the last character is a newline
            if (textClip.Split('\n').Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckForNewLine()
        {
             // Check if the last character is a newline
            if (richTextBox1.Text.Length > 0 && richTextBox1.Text[richTextBox1.Text.Length - 1] != '\n')
            {
               // MessageBox.Show("yes");
                // Add a newline if necessary
                richTextBox1.AppendText(Environment.NewLine);
            }
        }


        public string ImportDataFromTXT(string path)
        {
            string employeeList = "";
            try
            {
                using (Stream stream = null)
                {
                    var rows = (File.ReadAllText(path, Encoding.Default));
                    employeeList = rows;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return employeeList;
        }


        public void ExportDataToTXT(string op, string path)
        {
            var txt = new System.Text.StringBuilder();
            txt.Append(op);
            File.WriteAllText((path), txt.ToString(), Encoding.Default);
        }

        private void MySaving()
        {
            //ask for Save...
            DialogResult tempDg = MessageBox.Show(this, "done!,do you want to Save?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tempDg == DialogResult.Yes)
            {
                // save the notes to text file in the path
                ExportDataToTXT(this.richTextBox1.Text, myPath);
                this.ExportDataToTXT(("" + this.Location.X + "," + this.Location.Y + "|" + this.Width + "," + this.Height), myLastLocation);
                Application.Exit();
            }
            else
            {
                Application.Exit();
            }
        }

        private void DoIt()
        {
            //and do it now :)
            this.richTextBox1.SelectionColor = Color.Gray;
            this.richTextBox1.SelectionFont = new Font(this.richTextBox1.SelectionFont, FontStyle.Strikeout);
        }
        private void DoItInvers()
        {
            //and do it now :)
            this.richTextBox1.SelectionColor = Color.White;
            this.richTextBox1.SelectionFont = new Font(this.richTextBox1.SelectionFont, FontStyle.Regular);
        }
        private void StyleSave(bool bit)
        {
            //get last selectedText style
            string tempText = this.ImportDataFromTXT(myStyle);

            if (bit ==true)
            {
                if(tempText.Length>1)
                foreach (var item in tempText.Split('#'))
                {
                 //   MessageBox.Show("item:" + item.Split('|')[0]);
                   this.richTextBox1.Select(int.Parse(item.Split('|')[0]), int.Parse(item.Split('|')[1]));
                    //and do it now :)
                    this.DoIt();
                }
            }        
            else
            {
                bool testStart = false;
                string testTempText = "";
                int tempIndex = 0;
                int trueIndex = 0;
                
                if(tempText.Length>1)
                foreach (var item in tempText.Split('#'))
                {
                    //size of array tempText
                    tempIndex++;
                    if((int.Parse(item.Split('|')[0])) == this.richTextBox1.SelectionStart)
                    {
                        testStart = true;
                        trueIndex = tempIndex;
                    }
                    else
                    {
                        testTempText += item + "#";
                    }
                }

                if (testStart == false)
                {
                    //save last selectedText style
                    if (tempText.Length > 0)
                        this.ExportDataToTXT((tempText + "#" + this.richTextBox1.SelectionStart + "|" + this.richTextBox1.SelectedText.Length), myStyle);
                    else
                        this.ExportDataToTXT(("" + this.richTextBox1.SelectionStart + "|" + this.richTextBox1.SelectedText.Length), myStyle);

                    //and do it now :)
                    this.DoIt();
                }
                else
                {
                    //if the style list > 1 then delete last '#'
                   if((tempText.Split('#').Length > 1 ))
                    {
                        testTempText = testTempText.Substring(0, (testTempText.Length - 1));
                    }
                
                    this.ExportDataToTXT((testTempText), myStyle);
                    //and do it now invers :)
                    this.DoItInvers();
                }

              }

            //Get start from last text point...
            this.richTextBox1.SelectionStart = this.richTextBox1.TextLength;  
            //rest the style
            this.DoItInvers();
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            //to Save and Exit
            if (e.KeyData == Keys.Escape)
            {
                this.MySaving();
            }
            //to change the style of the SelectionText ...
            else if (e.KeyData == Keys.ShiftKey)
            {
                if (this.richTextBox1.SelectedText.Trim().Length > 0)
                {
                    this.StyleSave(false);
                }       
            }
        }
        private void MoveCursor()
        {
            // Set the Current cursor, move the form to the cursor Position 
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y);
            this.Location = new System.Drawing.Point(Cursor.Position.X, Cursor.Position.Y);
        }

   
        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // Determine which mouse button is clicked.
            if (e.Button == MouseButtons.Middle)
            {
                this.MoveCursor();
            }
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                // save last location and last form size to text file
                this.ExportDataToTXT(("" + this.Location.X + "," + this.Location.Y+"|"+this.Width+","+this.Height), myLastLocation);
            }

            if (e.Button == MouseButtons.Right)
            {       //show the menu strrip on the cursor click    
                this.contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                this.MoveCursor();
            }
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {    // '1245203' this is a HashCode value of (ControlKey+S) at same time.
            if (e.KeyChar.GetHashCode() == (1245203))
            {      
                this.MySaving();
            }
        }

        //restore the form size 
        public void MainFormSize(string width,string height)
        {
            this.Size = new Size(int.Parse(width),int.Parse(height));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason.ToString() == "UserClosing")
            { 
                    Program.LinkSaved -= OnLinkSaved; // Unsubscribe from the LinkSaved event
                    this.MySaving();
            }
        }

        private void doneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.SelectedText.Trim().Length > 0)
            {
                this.StyleSave(false);
            }
        }
    }
}
