using System.ComponentModel;

namespace SwipeFlash.Core
{
    public class WindowViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Whether the instructions are currently visible
        /// </summary>
        public bool InstructionsVisible => InstructionsEnabled && IsContentLoaded;

        /// <summary>
        /// Whether the instructions are enabled in the settings
        /// </summary>
        public bool InstructionsEnabled { get; private set; } = true;

        /// <summary>
        /// Whether the window content is loaded
        /// </summary>
        public bool IsContentLoaded { get; set; } = false;

        private bool _isCardSwipedRight;
        /// <summary>
        /// Whether a card was swiped right
        /// </summary>
        public bool IsCardSwipedRight
        {
            get => _isCardSwipedRight;
            set
            {
                _isCardSwipedRight = value;
                OnPropertyChanged(nameof(IsCardSwipedRight));
                _isCardSwipedRight = false;
            }
        }

        private bool _isCardSwipedLeft;
        /// <summary>
        /// Whether a card was swiped left
        /// </summary>
        public bool IsCardSwipedLeft
        {
            get => _isCardSwipedLeft;
            set
            {
                _isCardSwipedLeft = value;
                OnPropertyChanged(nameof(IsCardSwipedRight));
                _isCardSwipedLeft = false;
            }
        }

        #endregion

        #region Constructor

        public WindowViewModel()
        {
            // Hook OnSettingsChanged to Settings' PropertyChanged Event
            Properties.Settings.Default.PropertyChanged += OnSettingsChanged;

            // Gets the value of instrutions enabled
            InstructionsEnabled = Properties.Settings.Default.ShowInstructions;

            // Gets the application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // Updates the content loaded flag when the content loaded event is fired
            appVM.OnContentLoaded += (ss, ee) => { IsContentLoaded = appVM.IsContentLoaded; };

            var fm = IoC.Get<FlashcardManager>();
        }

        #endregion

        #region Private Helpers

        // Called when the settings have been modified
        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            // Switch on property name
            switch (e.PropertyName)
            {
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