using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KeyboardKing.core
{
    public abstract class JumpPage : Page
    {
        /// <summary>
        /// Parent Window that contains the Frame.
        /// </summary>
        private MainWindow _window {get;set;}

        public JumpPage(MainWindow w)
        {
            _window = w;
        }

        /// <summary>
        /// Method that is called upon entering the view.
        /// </summary>
        public abstract void OnLoad();

        /// <summary>
        /// Method that is called upon leaving the view.
        /// </summary>
        public abstract void OnShadow();

        /// <summary>
        /// Method that is called every x seconds.
        /// </summary>
        public abstract void OnTick();
    }
}
