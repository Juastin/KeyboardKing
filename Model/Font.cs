using System;

namespace Model
{
    public class Font
    {
        public string FontTitle { get; private set; }
        public Uri FontUri { get; private set; }

        public Font(string themeTitle, string themePath)
        {
            FontTitle = themeTitle;
            FontUri = CreateUri(themePath);
        }

        private Uri CreateUri(string path)
        {
            return new Uri(path, UriKind.Relative);
        }
    }
}
