using System;
using System.Drawing;
using System.Windows.Forms;

namespace MySimpleNotes
{
	public class ToastForm : Form
	{
		private System.Windows.Forms.Timer hideTimer;
		private Label messageLabel;

		// Private constructor to control how it's created
		private ToastForm(string message)
		{
			// --- Form Properties ---
			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.Manual;
			this.BackColor = Color.FromArgb(20, 20, 20); // Dark background
			this.TopMost = true; // Ensures it shows on top
			this.Opacity = 0.9;  // Slightly transparent
			this.ClientSize = new Size(200, 50); // Set a fixed size

			// --- Label Properties ---
			messageLabel = new Label();
			messageLabel.Text = message;
			messageLabel.ForeColor = Color.White;
			messageLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
			messageLabel.Dock = DockStyle.Fill;
			messageLabel.TextAlign = ContentAlignment.MiddleCenter;
			this.Controls.Add(messageLabel);

			// --- Timer to auto-hide the form ---
			hideTimer = new System.Windows.Forms.Timer();
			//hideTimer.Interval = 1500; // 1.5 seconds
			hideTimer.Interval = 2500; // 2.5 seconds
			hideTimer.Tick += (s, e) => {
				hideTimer.Stop(); // Stop the timer
				this.Close();     // Close the form
			};
		}

		/// <summary>
		/// Creates, positions, and shows a toast notification.
		/// </summary>
		/// <param name="parentForm">The form to position the toast relative to.</param>
		/// <param name="message">The message to display.</param>
		public static void ShowToast(Form parentForm, string message)
		{
			var toast = new ToastForm(message);

			// Calculate position to be centered horizontally on the parent form
			int x = parentForm.Location.X + (parentForm.Width - toast.Width) / 2;

			// Position the toast near the top of the parent form with a small margin.
			int y = parentForm.Location.Y + 20; // 20px padding from the top

			toast.Location = new Point(x, y);

			toast.Show(); // Use Show() so it doesn't block the UI
			toast.hideTimer.Start();
		}
	}
}