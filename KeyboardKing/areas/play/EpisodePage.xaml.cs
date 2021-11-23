using KeyboardKing.core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace KeyboardKing.areas.play
{
    /// <summary>
    /// Interaction logic for EpisodePage.xaml
    /// </summary>
    public partial class EpisodePage : JumpPage
    {
        /// <summary>
        /// Constructor of <c>EpisodePage</c>
        /// </summary>
        /// <param name="w"></param>
        public EpisodePage(MainWindow w) : base(w)
        {
            InitializeComponent();
            this.UserInput.Focus();

            //Dummy data
            Episode e = new Episode();
            EpisodeStep step1 = new EpisodeStep() { Word = "aaa" };
            EpisodeStep step2 = new EpisodeStep() { Word = "bbb" };
            EpisodeStep step3 = new EpisodeStep() { Word = "ccc" };
            EpisodeStep step4 = new EpisodeStep() { Word = "ddd" };
            EpisodeStep step5 = new EpisodeStep() { Word = "abc" };
            EpisodeStep step6 = new EpisodeStep() { Word = "cba" };
            e.EpisodeSteps.Enqueue(step1);
            e.EpisodeSteps.Enqueue(step2);
            e.EpisodeSteps.Enqueue(step3);
            e.EpisodeSteps.Enqueue(step4);
            e.EpisodeSteps.Enqueue(step5);
            e.EpisodeSteps.Enqueue(step6);

            EpisodeController.Initialise(e);
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
        /// <summary>
        /// Event that fires each time when focus of window has been lost.
        /// This way the UserInput field is always focused and can always be filled in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInput_LostFocus(object sender, RoutedEventArgs e)
        {
            this.UserInput.Focus();
        }
        /// <summary>
        /// <para>Event that fires when the UserInput changes.</para>
        /// Method gets the last character in the UserInput.
        /// <para>This input is then passed to the method <see cref="EpisodeController.CheckInput"/></para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = this.UserInput.Text;
            char character = txt[^1];

            EpisodeController.CheckInput(character);
        }
    }
}
