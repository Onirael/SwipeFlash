using SwipeFlash.Core;
using System.Windows;

namespace SwipeFlash
{
    public class BaseWindow : Window
    {
        /// <summary>
        /// The type of the window
        /// </summary>
        public WindowType BaseWindowType { get; set; }
    }

    public class BaseWindow<VM> : BaseWindow
        where VM : BaseViewModel, new()
    {
        #region Private Properties

        private VM mViewModel;

        #endregion

        #region Public Properties

        public VM ViewModel
        {
            get => mViewModel;

            set
            {
                // Check if the value has changed
                if (mViewModel == value)
                    return;

                // Update the value
                mViewModel = value;

                DataContext = mViewModel;
            }
        }

        #endregion

        #region Constructor

        public BaseWindow() : base()
        {
            // Create a default view model
            ViewModel = new VM();
        }

        #endregion
    }
}
