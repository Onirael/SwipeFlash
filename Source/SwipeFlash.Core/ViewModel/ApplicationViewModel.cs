using System.Windows.Input;

namespace SwipeFlash.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// True if the side menu is visible, false it is collapsed
        /// </summary>
        public bool IsSettingsMenuVisible
        {
            get;
            set;
        } = false;

        #endregion

        #region Command

        /// <summary>
        /// A command to toggle the visibility of the settings menu
        /// </summary>
        public ICommand ToggleSettingsMenuCommand { get; set; }

        #endregion

        #region Constructor

        public ApplicationViewModel()
        {
            // Initializes the settings menu visibility
            ToggleSettingsMenuCommand = new RelayCommand(ToggleSettingsMenu);
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Toggles the visibility of the settings menu
        /// </summary>
        public void ToggleSettingsMenu()
        {
            IsSettingsMenuVisible ^= true;
        }

        #endregion
    }
}
