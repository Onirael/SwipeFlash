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

        #region Private Properties

        /// <summary>
        /// The queue position of the new cards
        /// </summary>
        private int QueuePosition = 0;

        #endregion

        #region Constructor

        public FlashcardHostDesignModel() : base()
        {
            Flashcards = new AsyncObservableCollection<FlashcardViewModel>()
            {
                GetNextFlashcard(),
                GetNextFlashcard(),
                GetNextFlashcard(),
            };
        }

        #endregion

        protected override FlashcardViewModel GetNextFlashcard()
        {
            var newFlashcard = new FlashcardViewModel()
            {
                Side1Text = "The door",
                Side2Text = "La puerta",
                Side1Icon = "🇪🇸",
                Side2Icon = "🇬🇧",
                HasIllustration = true,
                CardQueuePosition = 2 - QueuePosition,
            };

            QueuePosition++;

            return newFlashcard;
        }
    }
}
