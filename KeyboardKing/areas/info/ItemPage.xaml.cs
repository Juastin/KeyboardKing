﻿using System.Windows;
using System.Windows.Controls;

namespace KeyboardKing.areas.info
{
    /// <summary>
    /// Interaction logic for ItemPage.xaml
    /// </summary>
    public partial class ItemPage : UserControl
    {
        public ItemPage()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }

        public void ShowOverlay()
        {
            Visibility = Visibility.Visible;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Process buy item here");
            Visibility = Visibility.Hidden;
        }
    }
}