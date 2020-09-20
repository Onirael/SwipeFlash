using System.Windows.Input;

namespace SwipeFlash.Core
{
    public class SettingsMenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current aplication settings
        /// </summary>
        public ApplicationSettings CurrentSettings { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to close the settings menu
        /// </summary>
        public ICommand CloseSettingsCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsMenuViewModel()
        {
            // Get the current settings from the application view model
            CurrentSettings = IoC.Get<ApplicationViewModel>().UserSettings;

            // Initialize the close button command
            CloseSettingsCommand = new RelayCommand(OnCloseSettingsPressed);
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Called when the close settings menu button is pressed
        /// </summary>
        private void OnCloseSettingsPressed()
        {
            IoC.Get<ApplicationViewModel>().UserSettings = CurrentSettings;
        }

        #endregion
    }
}
