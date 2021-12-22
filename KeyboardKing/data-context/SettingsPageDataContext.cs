﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace KeyboardKing.data_context
{
    public class SettingsPageDataContext
    {
        public User Student { get => SettingsController.Student; }
        public string Dyslectic { get => $"{SettingsController.Student?.Dyslectic ?? false}" ; }
    }
}