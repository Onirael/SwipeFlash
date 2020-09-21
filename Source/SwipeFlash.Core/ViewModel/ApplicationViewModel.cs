using System;
using System.Diagnostics;
using System.Windows.Input;
using Unsplasharp;

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
        public bool IsSettingsMenuVisible { get; set; } = false;

        /// <summary>
        /// The Unsplasharp client used to fetch images
        /// </summary>
        public UnsplasharpClient IllustrationsClient { get; set; }

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

            // Initialize Unsplasharp client
            if (Properties.Settings.Default.IllustrationsEnabled)
            {
                IllustrationsClient = InitializeUnsplasharp();

                bool isClientLoaded = IllustrationsClient != null;

                string logString = isClientLoaded ? "SUCCESS" : "FAILED";
                Debugger.Log(0, "UserLog", $"Loaded Unsplasharp client with result {logString}");

                // Add network check

                Properties.Settings.Default.IllustrationsEnabled = isClientLoaded;
            }

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

        private UnsplasharpClient InitializeUnsplasharp()
        {
            return new UnsplasharpClient("GKK8X3xKGBUXa6hamQgHPb79GEBlkUZ8vzt23DjKmF0");
        }

    }
}