using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySimpleNotes
{
    static class Program
    {
        private static string lastClipboardText = string.Empty;
        private static readonly string filePath = @"C:\MySimpleNotes\MySimpleNotes_AllText.txt";
        private static bool isRunning = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread clipboardThread = new Thread(ClipboardMonitor);
            clipboardThread.SetApartmentState(ApartmentState.STA); // Set the thread to STA
            clipboardThread.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 mainForm = new Form1();
            mainForm.FormClosing += (sender, e) => { isRunning = false; }; // Stop the thread on form closing
            Application.Run(mainForm);
        }

        private static void ClipboardMonitor()
        {
            while (isRunning)
            {
                try
                {
                    if (Clipboard.ContainsText())
                    {
                        string clipboardText = Clipboard.GetText();

                        if (IsValidUrl(clipboardText) && clipboardText != lastClipboardText)
                        {
                            lastClipboardText = clipboardText;

                            if (!IsLinkInFile(clipboardText))
                            {
                                DialogResult result = MessageBox.Show($"Do you want to save this link?\n{clipboardText}", "New Link Detected", MessageBoxButtons.YesNo);

                                if (result == DialogResult.Yes)
                                {
                                    SaveLink(clipboardText);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error accessing clipboard: " + ex.Message);
                }

                Thread.Sleep(1000); // Check clipboard every second
            }
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private static bool IsLinkInFile(string link)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    var lines = System.IO.File.ReadAllLines(filePath);
                    return lines.Contains(link);
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                return false;
            }
        }

        private static void SaveLink(string link)
        {
            try
            {
                System.IO.File.AppendAllText(filePath, link + Environment.NewLine);
                Console.WriteLine($"Link saved: {link}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving link: " + ex.Message);
            }
        }
    }
}
