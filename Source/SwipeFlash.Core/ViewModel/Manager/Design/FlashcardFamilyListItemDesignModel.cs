using System.Collections.Generic;

namespace SwipeFlash.Core
{
    public class FlashcardFamilyListItemDesignModel : FlashcardFamilyListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// The static instance of the design model
        /// </summary>
        public static FlashcardFamilyListItemDesignModel Instance => new FlashcardFamilyListItemDesignModel();

        #endregion

        #region Constructor

        public FlashcardFamilyListItemDesignModel()
        {
            FamilyName = "Language 1 to Language 2";
            CardCount = 999;
            IsFamilyEnabled = true;
        }

        #endregion
    }
}
