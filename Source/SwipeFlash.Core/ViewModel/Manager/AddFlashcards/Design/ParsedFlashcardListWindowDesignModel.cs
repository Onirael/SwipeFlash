namespace SwipeFlash.Core
{
    /// <summary>
    /// The design model for the flashcards
    /// </summary>
    public class ParsedFlashcardListWindowDesignModel : ParsedFlashcardListWindowViewModel
    {
        #region Singleton

        /// <summary>
        /// The static instance of the design model
        /// </summary>
        public static ParsedFlashcardListWindowDesignModel Instance => new ParsedFlashcardListWindowDesignModel();

        #endregion

        #region Constructor

        public ParsedFlashcardListWindowDesignModel()
        {
            ListItems = new AsyncObservableCollection<ParsedFlashcardListItemViewModel>()
            {
                new ParsedFlashcardListItemViewModel()
                {
                    Side1Text = "The door",
                    Side2Text = "La puerta",
                },
                new ParsedFlashcardListItemViewModel()
                {
                    Side1Text = "The lamp",
                    Side2Text = "La lámpara",
                    Side1DefaultText = "The lump",
                },
            };
        }

        #endregion
    }
}
