// AppSettings.cs
using System; // <-- Add this for DateTime
using System.Collections.Generic;
using System.Drawing;

namespace MySimpleNotes
{
    /// <summary>
    /// Represents a single saved URL with its metadata.
    /// </summary>
    public class UrlEntry
    {
        public string Url { get; set; }
        public DateTime SavedAt { get; set; }
    }

    public class AppSettings
    {
        public string NotesRtf { get; set; } = "";

        // We now store a list of UrlEntry objects instead of simple strings.
        public List<UrlEntry> SavedUrls { get; set; } = new List<UrlEntry>();

        public Point? WindowLocation { get; set; }
        public Size? WindowSize { get; set; }
        public int LastTabIndex { get; set; } = 0;
    }
}