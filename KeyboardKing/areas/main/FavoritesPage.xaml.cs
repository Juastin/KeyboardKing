﻿using KeyboardKing.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
using Controller;

namespace KeyboardKing.areas.main
{
    /// <summary>
    /// Interaction logic for FavoritesPage.xaml
    /// </summary>
    public partial class FavoritesPage : JumpPage
    {
        public FavoritesPage(MainWindow w) : base(w)
        {
            InitializeComponent();
        }

        public override void OnLoad()
        {
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }
    }
}
