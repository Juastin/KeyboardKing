﻿using KeyboardKing.core;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace KeyboardKing.areas.explanation
{
    /// <summary>
    /// Interaction logic for RegistrationSkillPage.xaml
    /// </summary>
    public partial class ExplanationKeyboard : JumpPage
    {
        //Backslash and LeftAlt not working, LeftAlt probably because of windows limitation
        private readonly Key[] LeftPinkyKeys = { Key.OemTilde, Key.D1, Key.Tab, Key.Q, Key.CapsLock, Key.A, Key.LeftShift, Key.LeftCtrl, Key.Z };
        private readonly Key[] LeftRingKeys = { Key.D2, Key.W, Key.S, Key.X };
        private readonly Key[] LeftMiddleKeys = { Key.D3, Key.E, Key.D, Key.C };
        private readonly Key[] LeftIndexKeys = { Key.D4, Key.D5, Key.R, Key.T, Key.F, Key.G, Key.V, Key.B };
        private readonly Key[] LeftThumbKeys = { Key.Space, Key.LeftAlt };
        private readonly Key[] RightThumbKeys = { Key.Space, Key.RightAlt };
        private readonly Key[] RightIndexKeys = { Key.D6, Key.D7, Key.Y, Key.U, Key.H, Key.J, Key.N, Key.M };
        private readonly Key[] RightMiddleKeys = { Key.D8, Key.I, Key.K, Key.OemComma };
        private readonly Key[] RightRingKeys = { Key.D9, Key.O, Key.L, Key.OemPeriod };
        private readonly Key[] RightPinkyKeys = { Key.D0, Key.P, Key.OemSemicolon, Key.OemQuestion, Key.OemMinus, Key.OemOpenBrackets, Key.OemQuotes,
            Key.RightShift, Key.OemPlus, Key.OemCloseBrackets, Key.Enter, Key.Back, Key.OemBackslash, Key.RightCtrl };

        public ExplanationKeyboard(MainWindow w) : base(w)
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.PreviewKeyDown += Explanation_KeyDown;
        }

        public override void OnLoad()
        {
            ShowIndicators(Key.None);
        }

        public override void OnShadow()
        {
        }

        public override void OnTick()
        {
        }

        private void Explanation_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            ShowIndicators(e.Key);
        }

        /// <summary>
        /// Makes specific indicator(s) vissible.
        /// </summary>
        /// <param name="key">This will determine which indicator will be shown</param>
        private void ShowIndicators(Key key)
        {
            //Set all indicators hidden
            LeftPinky.Visibility = LeftRing.Visibility = LeftMiddle.Visibility = LeftIndex.Visibility = LeftThumb.Visibility =
            RightThumb.Visibility = RightIndex.Visibility = RightMiddle.Visibility = RightRing.Visibility = RightPinky.Visibility
            = Visibility.Hidden;

            if (LeftPinkyKeys.Contains(key))
                LeftPinky.Visibility = Visibility.Visible;

            if (LeftRingKeys.Contains(key))
                LeftRing.Visibility = Visibility.Visible;

            if (LeftMiddleKeys.Contains(key))
                LeftMiddle.Visibility = Visibility.Visible;

            if (LeftIndexKeys.Contains(key))
                LeftIndex.Visibility = Visibility.Visible;

            if (LeftThumbKeys.Contains(key))
                LeftThumb.Visibility = Visibility.Visible;

            if (RightThumbKeys.Contains(key))
                RightThumb.Visibility = Visibility.Visible;

            if (RightIndexKeys.Contains(key))
                RightIndex.Visibility = Visibility.Visible;

            if (RightMiddleKeys.Contains(key))
                RightMiddle.Visibility = Visibility.Visible;

            if (RightRingKeys.Contains(key))
                RightRing.Visibility = Visibility.Visible;

            if (RightPinkyKeys.Contains(key))
                RightPinky.Visibility = Visibility.Visible;
        }
    }
}
