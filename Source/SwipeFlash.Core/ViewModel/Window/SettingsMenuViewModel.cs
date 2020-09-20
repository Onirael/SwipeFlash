using System.Windows.Input;

namespace SwipeFlash.Core
{
    public class SettingsMenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Whether the flashcard illustrations are enabled
        /// </summary>
        public bool IllustrationsEnabled { get; set; } = true;

        #endregion

        #region Commands

        public ICommand CloseSettingsCommand { get; set; }

        #endregion

        #region Constructor

        public SettingsMenuViewModel()
        {
            CloseSettingsCommand = new RelayCommand(OnCloseSettingsPressed);
        }

        #endregion

        #region Command Helpers

        private void OnCloseSettingsPressed()
        {
            IoC.Get<ApplicationViewModel>().IsSettingsMenuVisible = false;
        }

        #endregion
    }
}
