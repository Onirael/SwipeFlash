namespace SwipeFlash.Core
{
    /// <summary>
    /// The design model for the flashcards
    /// </summary>
    public class ParsedFlashcardListItemDesignModel : ParsedFlashcardListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// The static instance of the design model
        /// </summary>
        public static ParsedFlashcardListItemDesignModel Instance => new ParsedFlashcardListItemDesignModel();

        #endregion

        #region Constructor
        
        public ParsedFlashcardListItemDesignModel()
        {
            Side1Text = "The door";
            Side2Text = "La puerta";
        }

        #endregion
    }
}
