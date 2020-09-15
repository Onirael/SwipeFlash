namespace SwipeFlash.Core
{
    /// <summary>
    /// The design model for the flashcards
    /// </summary>
    public class FlashcardDesignModel : FlashcardViewModel
    {
        #region Singleton

        /// <summary>
        /// The static instance of the design model
        /// </summary>
        public static FlashcardViewModel Instance => new FlashcardDesignModel();

        #endregion

        #region Constructor

        public FlashcardDesignModel()
        {
            Side1Text = "La puerta del dormitorio dónde duerme el señor Javier";
            Side2Text = "The door";
            Side1Icon = "\u1F1EA\u1F1F8";
            Side2Icon = "\u1F1EC\u1F1E7";
            IsFlipped = false;
            IsInverted = false;
        }

        #endregion
    }
}
