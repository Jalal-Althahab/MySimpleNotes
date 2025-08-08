// SettingsManager.cs
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace MySimpleNotes
{
    public static class SettingsManager
    {
        private static readonly string _filePath;

        // *** ADD THIS PUBLIC PROPERTY ***
        // This allows other parts of the app to know where the settings file is.
        public static string FilePath => _filePath;

        static SettingsManager()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDir = Path.Combine(appDataPath, "MySimpleNotes");
            Directory.CreateDirectory(appDir);
            _filePath = Path.Combine(appDir, "settings.json");
        }

        public static AppSettings Load()
        {
            // ... (rest of the method is unchanged)
            if (!File.Exists(_filePath))
            {
                return new AppSettings();
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}\n\nA new settings file will be created.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new AppSettings();
            }
        }

        public static void Save(AppSettings settings)
        {
            // ... (rest of the method is unchanged)
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}