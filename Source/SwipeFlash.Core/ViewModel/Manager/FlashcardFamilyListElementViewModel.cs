namespace SwipeFlash.Core
{
    public class FlashcardFamilyListElementViewModel : BaseViewModel
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

        /// <summary>
        /// Whether the card family is enabled
        /// </summary>
        public bool IsFamilyEnabled { get; set; }

        #endregion

        #region Constructor

        public FlashcardFamilyListElementViewModel()
        {
        }

        public FlashcardFamilyListElementViewModel(FlashcardFamilyData familyData)
        {
            DisplayName = familyData.Name;
            CardCount = familyData.CardCount;
            IsFamilyEnabled = familyData.IsEnabled;
        }

        #endregion
    }
}
