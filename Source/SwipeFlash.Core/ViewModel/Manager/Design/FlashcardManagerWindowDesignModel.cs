using System.Collections.ObjectModel;

namespace SwipeFlash.Core
{
    public class FlashcardManagerWindowDesignModel : FlashcardManagerWindowViewModel
    {
        #region Singleton

        /// <summary>
        /// The static instance of the design model
        /// </summary>
        public static FlashcardManagerWindowDesignModel Instance => new FlashcardManagerWindowDesignModel();

        #endregion

        #region Constructor

        public FlashcardManagerWindowDesignModel()
        {
            FlashcardFamilies = new ObservableCollection<FlashcardFamilyListElementViewModel>()
            {
                new FlashcardFamilyListElementViewModel()
                {
                   DisplayName = "Spanish to English",
                   CardCount = 4000,
                   IsFamilyEnabled = true,
                },
                new FlashcardFamilyListElementViewModel()
                {
                   DisplayName = "German to English",
                   CardCount = 5555,
                   IsFamilyEnabled = false,
                },
            };
        }

        #endregion
    }
}
