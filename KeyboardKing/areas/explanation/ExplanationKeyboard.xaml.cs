using KeyboardKing.core;
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
        /**
         * Backslash and LeftAlt not working, probably because of windows limitation
         * These arrays specify which keys are pressed with witch finger. 
         * The only duplicate keys are `Key.Space` in Left and Right Thumb.
         */

        private Window _window;
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
            _window = Window.GetWindow(this);
            _window.PreviewKeyDown += Explanation_KeyDown;
        }

        public override void OnLoad()
        {
            SetIndicatorsHidden();
        }

        public override void OnShadow()
        {
            _window.PreviewKeyDown -= Explanation_KeyDown;
        }

        public override void OnTick()
        {
        }

        private void Explanation_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            SetIndicatorsHidden();
            ShowIndicators(e.Key);
        }

        private void SetIndicatorsHidden()
        {
            //Set all indicators hidden
            LeftPinky.Visibility = LeftRing.Visibility = LeftMiddle.Visibility = LeftIndex.Visibility = LeftThumb.Visibility =
            RightThumb.Visibility = RightIndex.Visibility = RightMiddle.Visibility = RightRing.Visibility = RightPinky.Visibility
            = Visibility.Hidden;
        }

        /// <summary>
        /// Makes specific indicator(s) vissible.
        /// </summary>
        /// <param name="key">This will determine which indicator will be shown</param>
        private void ShowIndicators(Key key)
        {
            //Thumbs are not in the else if since you can press the spacebar with both thumbs.
            if (LeftThumbKeys.Contains(key))
                LeftThumb.Visibility = Visibility.Visible;

            if (RightThumbKeys.Contains(key))
                RightThumb.Visibility = Visibility.Visible;

            else if (LeftPinkyKeys.Contains(key))
                LeftPinky.Visibility = Visibility.Visible;

            else if(LeftRingKeys.Contains(key))
                LeftRing.Visibility = Visibility.Visible;

            else if (LeftMiddleKeys.Contains(key))
                LeftMiddle.Visibility = Visibility.Visible;

            else if (LeftIndexKeys.Contains(key))
                LeftIndex.Visibility = Visibility.Visible;

            else if (RightIndexKeys.Contains(key))
                RightIndex.Visibility = Visibility.Visible;

            else if (RightMiddleKeys.Contains(key))
                RightMiddle.Visibility = Visibility.Visible;

            else if (RightRingKeys.Contains(key))
                RightRing.Visibility = Visibility.Visible;

            else if (RightPinkyKeys.Contains(key))
                RightPinky.Visibility = Visibility.Visible;
        }
    }
}
