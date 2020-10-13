using System.Windows.Input;

namespace SwipeFlash.Core
{
    public class SettingsMenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current aplication settings
        /// </summary>
        public bool IllustrationsEnabled
        {
            get => Properties.Settings.Default.IllustrationsEnabled;
            set => Properties.Settings.Default.IllustrationsEnabled = value;
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to close the settings menu
        /// </summary>
        public ICommand CloseSettingsCommand { get; set; }

        /// <summary>
        /// The command to open the file parser
        /// </summary>
        public ICommand OpenManageCardsCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsMenuViewModel()
        {
            // Initialize the close button command
            CloseSettingsCommand = new RelayCommand(OnCloseSettingsPressed);

            // Initialize the open file parser button command
            OpenManageCardsCommand = new RelayCommand(OnManageCardsPressed);
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Called when the close settings menu button is pressed
        /// </summary>
        private void OnCloseSettingsPressed()
        {
            IoC.Get<ApplicationViewModel>().IsSettingsMenuVisible = false;
        }

        /// <summary>
        /// Called when the Manage cards button is pressed
        /// </summary>
        private void OnManageCardsPressed()
        {
            IoC.Get<WindowService>().CreateWindow(new WindowArgs() { TargetType = WindowType.FlashcardManager });
        }

        #endregion
    }
}