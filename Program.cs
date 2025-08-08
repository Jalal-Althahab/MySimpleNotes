using System;
using System.Threading;
using System.Windows.Forms;

namespace MySimpleNotes
{
    static class Program
    {
        private static string lastClipboardText = string.Empty;
        private static bool isRunning = true;

        // The event that the Form will subscribe to.
        public static event Action<string> LinkSaved;

        [STAThread]
        static void Main()
        {
            // Clean the clipboard once at the start.
            try { Clipboard.Clear(); } catch { }

            // Start the background thread to monitor the clipboard.
            Thread clipboardThread = new Thread(ClipboardMonitor);
            clipboardThread.SetApartmentState(ApartmentState.STA);
            clipboardThread.IsBackground = true;
            clipboardThread.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create and run the main form.
            var mainForm = new MainForm();

            // Set up a handler to stop the background thread when the form truly closes.
            mainForm.FormClosed += (sender, e) => { isRunning = false; };

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

                        // Check if it's a new, valid URL.
                        if (clipboardText != lastClipboardText && IsValidUrl(clipboardText))
                        {
                            lastClipboardText = clipboardText;
                            // Fire the event to notify the UI thread.
                            LinkSaved?.Invoke(clipboardText);
                        }
                    }
                }
                catch (Exception)
                {
                    // Ignore clipboard errors which can happen if another app has it locked.
                }

                Thread.Sleep(1000); // Check every second.
            }
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}