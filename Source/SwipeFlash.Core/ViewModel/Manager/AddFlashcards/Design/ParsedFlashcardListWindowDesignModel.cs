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
            Category1Name = "English";
            Category2Name = "Spanish";

            ListItems = new AsyncObservableCollection<ParsedFlashcardListItemViewModel>()
            {
                new ParsedFlashcardListItemViewModel("The door", "La puerta", 0),
                new ParsedFlashcardListItemViewModel("The cat", "El gato", 1),
                new ParsedFlashcardListItemViewModel("The dog", "El perro", 2),
            };
        }

        #endregion
        
        public override void InitAsync(object items)
        {
            return;
        }

    }
}
