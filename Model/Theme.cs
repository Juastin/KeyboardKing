﻿using System;

namespace Model
{
    public class Theme
    {
        public string ThemeTitle { get; private set; }
        public Uri ThemeUri { get; private set; }

        public Theme(string themeTitle, string themePath)
        {
            ThemeTitle = themeTitle;
            ThemeUri = CreateUri(themePath);
        }

        private Uri CreateUri(string path)
        {
            return new Uri(path, UriKind.Relative);
        }
    }
}
