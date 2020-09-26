using System.Windows.Input;

namespace SwipeFlash.Core
{
    class ParserWindowViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Whether the Windows file browser is currently open
        /// </summary>
        public bool IsFileBrowserOpen { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// A command to open the windows file browser
        /// </summary>
        ICommand OpenFileBrowserCommand { get; set; }

        #endregion

        #region Constructor

        public ParserWindowViewModel()
        {
            OpenFileBrowserCommand = new RelayCommand(OpenFileBrowser);
        }

        #endregion

        #region Command Helpers

        private void OpenFileBrowser()
        {
            IsFileBrowserOpen = true;
        }

        #endregion

    }
}
