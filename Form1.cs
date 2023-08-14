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
        //public List<int> KeyCodes = new List<int>() { 8, 17, 37, 39 };

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
                    this.Location = new Point(int.Parse(ImportDataFromTXT(myLastLocation).Split(',')[0]), int.Parse(ImportDataFromTXT(myLastLocation).Split(',')[1]));
                }
                else
                {
                    //create text files
                    ExportDataToTXT(this.richTextBox1.Text, myPath);
                    ExportDataToTXT(("" + this.Location.X + "," + this.Location.Y), myLastLocation);
                    //if (File.Exists(myLastLocation) == false)
                    //{
                      
                    //}
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


        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {   // save the notes to text file in the path
                ExportDataToTXT(this.richTextBox1.Text, myPath);
                //ask for exit...
                DialogResult tempDg = MessageBox.Show(this, "done!,do you want to exit?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tempDg == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {   
                // then close the app 
                Application.Exit();
            }

            //else
            //{
            //    if (KeyCodes.Contains(e.KeyValue) || (e.KeyCode == Keys.S && e.Control))
            //    {
            //        e.SuppressKeyPress = false;
            //        this.richTextBox1_DoubleClick(sender, e);
            //    }
            //    else
            //        e.SuppressKeyPress = true;
            //}
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
                // this.Location = new System.Drawing.Point(e.Location.X,e.Location.Y);
                this.MoveCursor();
            }
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                // save last location to text file
                this.ExportDataToTXT(("" + this.Location.X + "," + this.Location.Y), myLastLocation);
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
        {    // '1245203' this is a HachCode value of (ControlKey+S) at same time.
            if (e.KeyChar.GetHashCode() == (1245203))
            {
                this.richTextBox1_DoubleClick(sender, e);           
            }            
        }
    }
}
