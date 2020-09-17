using System;
using System.Windows.Input;

namespace SwipeFlash.Core
{
    class SettingsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Whether the flashcard illustrations are enabled
        /// </summary>
        public bool IllustrationsEnabled { get; set; }

        #endregion

        #region Commands

        public ICommand CloseSettingsCommand { get; set; }

        #endregion

        #region Constructor

        public SettingsViewModel()
        {
            CloseSettingsCommand = new RelayCommand(OnCloseSettingsPressed);
        }

        #endregion

        #region Command Helpers

        private void OnCloseSettingsPressed()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
