using System.Collections.Generic;

namespace SwipeFlash.Core
{
    public class FlashcardFamilyListElementDesignModel : FlashcardFamilyListElementViewModel
    {
        #region Singleton

        /// <summary>
        /// The static instance of the design model
        /// </summary>
        public static FlashcardFamilyListElementDesignModel Instance => new FlashcardFamilyListElementDesignModel();

        #endregion

        #region Constructor

        public FlashcardFamilyListElementDesignModel()
        {
            DisplayName = "Language 1 to Language 2";
            CardCount = 999;
            IsFamilyEnabled = true;
        }

        #endregion
    }
}
