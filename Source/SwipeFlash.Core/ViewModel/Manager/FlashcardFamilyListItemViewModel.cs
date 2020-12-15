using System;
using System.Windows.Input;

namespace SwipeFlash.Core
{
    public class FlashcardFamilyListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The display name of the element
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// The number of cards in the element family
        /// </summary>
        public int CardCount { get; set; }

        private bool _isFirstLoad = true;
        private bool _isFamilyEnabled;
        /// <summary>
        /// Whether the card family is enabled
        /// </summary>
        public bool IsFamilyEnabled
        {
            get => _isFamilyEnabled;
            set { _isFamilyEnabled = value; if (!_isFirstLoad) OnFamilyEnabledChanged(); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// A command called by the delete family button
        /// </summary>
        public ICommand DeleteFamilyCommand { get; set; }

        /// <summary>
        /// A command called by the see family button
        /// </summary>
        public ICommand OpenFamilyInfoCommand { get; set; }

        /// <summary>
        /// A command called by the see family stats button
        /// </summary>
        public ICommand OpenFamilyStatsCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlashcardFamilyListItemViewModel()
        {
            // Initializes delete command
            DeleteFamilyCommand = new RelayCommand(OnDeleteFamily);
        }

        /// <summary>
        /// Constructor taking in a data struct parameter
        /// </summary>
        /// <param name="familyData">The family data struct</param>
        public FlashcardFamilyListItemViewModel(FlashcardFamilyData familyData)
        {
            // Initializes the delete command
            DeleteFamilyCommand = new RelayCommand(OnDeleteFamily);

            // Initializes the open family info command
            OpenFamilyInfoCommand = new RelayCommand(OnOpenFamilyInfo);

            // Initializes the open family stats command
            OpenFamilyStatsCommand = new RelayCommand(OnOpenFamilyStats);

            // Sets properties from data
            FamilyName = familyData.Name;
            CardCount = familyData.CardCount;
            IsFamilyEnabled = familyData.IsEnabled;

            // Sets the first load flag
            _isFirstLoad = false;
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Called when the delete family button is pressed
        /// </summary>
        private void OnDeleteFamily()
        {
            // Gets the application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // Adds a listener to the confirmation event
            ListenerDelegate listener = OnDeleteFamilyConfirmed;
            appVM.ListenForEvent<bool>(WindowReturnEvent.ConfirmationReceived, listener);

            // Creates the window args
            var confirmWindowArgs = new WindowArgs()
            {
                Message = $"Delete the all flashcards in {FamilyName} ?",
                TargetType = WindowType.Confirmation,
            };

            // Creates the window
            IoC.Get<WindowService>().CreateWindow(confirmWindowArgs);
        }

        /// <summary>
        /// Called when the user has confirmed or cancelled the deletion of the family
        /// </summary>
        /// <param name="isConfirmed"></param>
        private void OnDeleteFamilyConfirmed(object isConfirmed)
        {
            // If the user confirmed the deletion
            if ((bool)isConfirmed)
                IoC.Get<FlashcardManager>().DeleteFamily(FamilyName);
        }

        /// <summary>
        /// Called when the show family info button is pressed 
        /// </summary>
        private void OnOpenFamilyInfo()
        {
            // Creates the window args
            var familyInfoWindowArgs = new WindowArgs()
            {
                Message = FamilyName,
                TargetType = WindowType.FamilyInfo,
            };

            // Creates the window
            IoC.Get<WindowService>().CreateWindow(familyInfoWindowArgs);
        }

        /// <summary>
        /// Called when the show family stats button is pressed
        /// </summary>
        private void OnOpenFamilyStats()
        {
            // Creates the window args
            var familyInfoWindowArgs = new WindowArgs()
            {
                Message = FamilyName,
                TargetType = WindowType.FamilyStats,
            };

            // Creates the window
            IoC.Get<WindowService>().CreateWindow(familyInfoWindowArgs);
        }

        /// <summary>
        /// Called when the <see cref="IsFamilyEnabled"/> property is changed
        /// </summary>
        private void OnFamilyEnabledChanged()
        {
            // Calls the Set Family Enabled method from the Flashcard Manager
            IoC.Get<FlashcardManager>().SetFamilyEnabled(FamilyName, IsFamilyEnabled);
        }

        #endregion
    }
}
