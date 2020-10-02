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
        public string DisplayName { get; set; }

        /// <summary>
        /// The number of cards in the element family
        /// </summary>
        public int CardCount { get; set; }

        private bool _isFamilyEnabled;
        /// <summary>
        /// Whether the card family is enabled
        /// </summary>
        public bool IsFamilyEnabled
        {
            get => _isFamilyEnabled;
            set { _isFamilyEnabled = value; OnFamilyEnabledChanged(); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// A command called by the delete family button
        /// </summary>
        public ICommand DeleteFamilyCommand { get; set; }

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
            // Initializes delete command
            DeleteFamilyCommand = new RelayCommand(OnDeleteFamily);

            DisplayName = familyData.Name;
            CardCount = familyData.CardCount;
            IsFamilyEnabled = familyData.IsEnabled;
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Called when the delete family button is pressed
        /// </summary>
        private void OnDeleteFamily()
        {
            // ADD A WARNING HERE !

            // Calls the Delete Family method from the Flashcard Manager
            IoC.Get<FlashcardManager>().DeleteFamily(DisplayName);
        }

        /// <summary>
        /// Called when the <see cref="IsFamilyEnabled"/> property is changed
        /// </summary>
        private void OnFamilyEnabledChanged()
        {
            // Calls the Set Family Enabled method from the Flashcard Manager
            IoC.Get<FlashcardManager>().SetFamilyEnabled(DisplayName, IsFamilyEnabled);
        }

        #endregion
    }
}
