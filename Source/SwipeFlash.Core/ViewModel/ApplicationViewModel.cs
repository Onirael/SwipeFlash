using System;
using System.IO;
using System.Threading.Tasks;
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

        /// <summary>
        /// Whether the Unsplash server is currently reachable
        /// </summary>
        public bool IsServerReachable { get; private set; } = true;

        /// <summary>
        /// Whether the illustrations are enabled
        /// </summary>
        public bool IllustrationsEnabled { get; private set; }

        /// <summary>
        /// Whether the instructions are visible
        /// </summary>
        public bool InstructionsEnabled { get; private set; }

        /// <summary>
        /// Whether the "server unreachable" message should be displayed
        /// </summary>
        public bool IsNetworkErrorMessageVisible => !IsServerReachable && IllustrationsEnabled;

        /// <summary>
        /// The delay between the loading of the content and the apparition of that content
        /// </summary>
        public double ContentAppearDelay { get; private set; } = 0.2;

        private bool _isContentLoaded = false;
        /// <summary>
        /// True if the window content was loaded and the card queue is set
        /// </summary>
        public bool IsContentLoaded
        {
            get => _isContentLoaded;
            set
            {
                // Updates the current value
                _isContentLoaded = value;
                if (value)
                {
                    // Sets the content visibility after a delay
                    Task.Delay((int)(ContentAppearDelay * 1000)).ContinueWith((t) =>
                    {
                        IsContentVisible = true;
                        OnContentLoaded(null, null);
                    });
                }
                else
                {
                    IsContentVisible = false;
                }
            }
        }

        /// <summary>
        /// Whether the window content is currently visible
        /// </summary>
        public bool IsContentVisible { get; set; } = false;

        /// <summary>
        /// The path to the static data JSON file
        /// </summary>
        public string StaticDataPath { get; set; }

        /// <summary>
        /// The path to the user data JSON file
        /// </summary>
        public string UserDataPath { get; set; }

        /// <summary>
        /// The checker used to check for internet availability
        /// </summary>
        public HttpEndpointChecker InternetChecker { get; set; }

        /// <summary>
        /// The view model of the main window
        /// </summary>
        public WindowViewModel MainWindowVM { get; set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Fired when a file is selected from any OpenFileDialog, 
        /// when a dialog is open, the sender should hook to this event to listen for the result, 
        /// passes the resulting file as a parameter
        /// </summary>
        public EventHandler<string> OnFileSelected;

        /// <summary>
        /// Fired when a file is saved from any SaveFileDialog, 
        /// when a dialog is open, the sender should hook to this event to listen for the result, 
        /// passes the resulting file as a parameter
        /// </summary>
        public EventHandler<string> OnFileSaved;

        /// <summary>
        /// Fired when the user has selected OK or cancel in a 
        /// confirmation window
        /// </summary>
        public EventHandler<bool> OnConfirmation;

        /// <summary>
        /// Fired when the window data content has finished loading
        /// </summary>
        public EventHandler OnContentLoaded;

        /// <summary>
        /// Fired when a key has been pressed while on the main window
        /// </summary>
        public EventHandler<Key> OnPreviewKeyDown;

        #endregion

        #region Public Methods

        /// <summary>
        /// Listens once for an event and runs a given function when the event fires
        /// </summary>
        /// <typeparam name="T">The type of the event's output</typeparam>
        /// <param name="awaitedEvent">The event to listen for</param>
        /// <param name="listenerFunction">The function to run when the event fires</param>
        public void ListenForEvent<T>(WindowReturnEvent awaitedEvent, ListenerDelegate listener)
        {
            // Create a single self-unhookable event
            // hooked to the application's file selected event handler
            EventHandler<T> listenerEvent = null;
            listenerEvent = (sender, output) =>
            {
                // Unhook the event
                switch (awaitedEvent)
                {
                    case WindowReturnEvent.FileSelected:
                        OnFileSelected -= (ss, e) => listenerEvent(sender, (T)Convert.ChangeType(e, typeof(T)));
                        break;

                    case WindowReturnEvent.FileSaved:
                        OnFileSaved -= (ss, e) => listenerEvent(sender, (T)Convert.ChangeType(e, typeof(T)));
                        break;

                    case WindowReturnEvent.ConfirmationReceived:
                        OnConfirmation -= (ss, e) => listenerEvent(sender, (T)Convert.ChangeType(e, typeof(T)));
                        break;

                    default:
                        break;
                }

                // Run the input function
                listener(output);
            };

            // Hooks the event to the application view model
            switch (awaitedEvent)
            {
                case WindowReturnEvent.FileSelected:
                    OnFileSelected += (ss, e) => listenerEvent(ss, (T)Convert.ChangeType(e, typeof(T)));
                    break;

                case WindowReturnEvent.FileSaved:
                    OnFileSaved += (ss, e) => listenerEvent(ss, (T)Convert.ChangeType(e, typeof(T)));
                    break;

                case WindowReturnEvent.ConfirmationReceived:
                    OnConfirmation += (ss, e) => listenerEvent(ss, (T)Convert.ChangeType(e, typeof(T)));
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Constructor

        public ApplicationViewModel()
        {
            // Initialize Unsplasharp client
            IllustrationsClient = InitializeUnsplasharp();

            // Start monitoring the availability of the Unsplash server
            MonitorServerstatus();

            // Hook OnSettingsChanged to Settings' PropertyChanged Event
            Properties.Settings.Default.PropertyChanged += OnSettingsChanged;

            // Store the location of the data files
            var parentDirectory = Directory.GetParent(
                                  Directory.GetParent(
                                  Directory.GetParent(
                                  Directory.GetCurrentDirectory()).ToString()).ToString());

            StaticDataPath = parentDirectory + "/SwipeFlash.Core/Data/StaticData.JSON";
            UserDataPath = parentDirectory + "/SwipeFlash.Core/Data/UserData.JSON";
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Initializes the Unsplasharp client
        /// </summary>
        /// <returns></returns>
        private UnsplasharpClient InitializeUnsplasharp()
        {
            return new UnsplasharpClient("GKK8X3xKGBUXa6hamQgHPb79GEBlkUZ8vzt23DjKmF0");
        }

        /// <summary>
        /// Pings the Unsplash server to see if it is responsive
        /// </summary>
        private void MonitorServerstatus()
        {
            // Creates a new endpoint checker
            InternetChecker = new HttpEndpointChecker("www.unsplash.com",
                                                      2000,
                                                      (result) => {
                                                            IsServerReachable = result;
                                                            if (result) InternetChecker.RunChecker = false;
                                                      });
        }
        
        /// <summary>
        /// Called when the user settings have been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Switch on property name
            switch (e.PropertyName)
            {
                // If IllustrationsEnabled has been updated
                case nameof(Properties.Settings.Default.IllustrationsEnabled):
                    // Update the local property
                    IllustrationsEnabled = Properties.Settings.Default.IllustrationsEnabled;
                    // Run the network checker
                    if (IllustrationsEnabled) InternetChecker.RunChecker = true;
                    break;

                case nameof(Properties.Settings.Default.ShowInstructions):
                    // Update the local property
                    InstructionsEnabled = Properties.Settings.Default.ShowInstructions;
                    break;

                default:
                    return;
            }
        }

        #endregion
    }
}