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

namespace MySimpleNotes
{
    public partial class Form1 : Form
    {
        static readonly string myPath = @"C:\MySimpleNotes\MySimpleNotes_AllText.txt";
        static readonly string myLastLocation = @"C:\MySimpleNotes\MySimpleNotes_LastLocation.txt";

        public Form1()
        {
            InitializeComponent();
            this.MyInitialize();
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
                    //Get start from last text point...
                    this.richTextBox1.SelectionStart=this.richTextBox1.TextLength;
                    //get last location
                    string lastLocationText = ImportDataFromTXT(myLastLocation).Split('|')[0];
                    this.Location = new Point(int.Parse(lastLocationText.Split(',')[0]), int.Parse(lastLocationText.Split(',')[1]));
                    //get last form size
                    string lastFormSizeText = ImportDataFromTXT(myLastLocation).Split('|')[1];
                    this.MainFormSize(lastFormSizeText.Split(',')[0], lastFormSizeText.Split(',')[1]);
                }
                else
                {
                    //create text files
                    ExportDataToTXT(this.richTextBox1.Text, myPath);
                    ExportDataToTXT(("" + this.Location.X + "," + this.Location.Y + "|" + this.Width + "," + this.Height), myLastLocation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            var csv = new System.Text.StringBuilder();
            csv.Append(op);
            // csv.Replace(',', ';');
            File.WriteAllText((path), csv.ToString(), Encoding.Default);
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

        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    this.MySaving();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
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
                this.richTextBox1.SelectionColor = Color.Gray;
                this.richTextBox1.SelectionFont = new Font(this.richTextBox1.SelectionFont, FontStyle.Strikeout);
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
                // this.richTextBox1_DoubleClick(sender, e);        
                this.MySaving();
            }
            //maximize font size (ShiftKey & '+') == (2818091)
            //minimize font size (ShiftKey & '-')== (6226015)         
            // MessageBox.Show("HashCode:" + e.KeyChar.GetHashCode());
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
                this.MySaving();
            }
        }
    }
}
