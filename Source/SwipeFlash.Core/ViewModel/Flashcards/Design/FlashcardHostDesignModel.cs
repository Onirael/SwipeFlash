namespace SwipeFlash.Core
{
    /// <summary>
    /// The design model for the flashcard host
    /// </summary>
    public class FlashcardHostDesignModel : FlashcardHostViewModel
    {
        #region Singleton

        /// <summary>
        /// The static instance of the design model
        /// </summary>
        public static FlashcardHostDesignModel Instance => new FlashcardHostDesignModel();

        #endregion

        #region Constructor

        public FlashcardHostDesignModel() : base() { }

        #endregion
    }
}
