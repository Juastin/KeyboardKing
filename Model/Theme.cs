﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Theme
    {
        public string ThemeTitle { get; private set; }
        public Uri ThemeUri { get; private set; }
        public Uri BackgroundUri { get; private set; }

        public Theme(string themeTitle, string themePath, string backgroundPath)
        {
            ThemeTitle = themeTitle;
            ThemeUri = CreateUri(themePath);
            BackgroundUri = CreateUri(backgroundPath);
        }

        private Uri CreateUri(string path)
        {
            return new Uri(path, UriKind.Relative);
        }
    }
}