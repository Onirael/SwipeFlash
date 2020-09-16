using System;
using System.Threading.Tasks;

namespace SwipeFlash.Core
{
    /// <summary>
    /// The view model for the flashcard host
    /// </summary>
    public class FlashcardHostViewModel
    {
        #region Public Properties

        /// <summary>
        /// An array of view models for the flashcards
        /// </summary>
        public FlashcardViewModel[] Flashcards { get; set; }

        /// <summary>
        /// The flashcard currently in front
        /// </summary>
        public int ActiveFlashcard { get; set; } = 0;

        /// <summary>
        /// The amount of flashcards stored in <see cref="Flashcards"/>
        /// </summary>
        public int FlashcardCount { get; set; } = 3;

        /// <summary>
        /// DEVELOPMENT
        /// </summary>
        private int FlashcardID { get; set; } = 0;

        #endregion

        #region Constructor

        public FlashcardHostViewModel()
        {
            // Initializes the flashcards array
            Flashcards = new FlashcardViewModel[FlashcardCount];
            for (int i=0; i<FlashcardCount; ++i)
            {
                Flashcards[i] = GetNextFlashCard();
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Called when a card is swiped to the left or right
        /// </summary>
        public void OnCardSwipe(object sender, EventArgs e)
        {
            // Disable the input on the old card
            Flashcards[ActiveFlashcard].HasInput = false;

            // Append new flashcard
            Flashcards[ActiveFlashcard] = GetNextFlashCard();

            // Update the active flash card
            ActiveFlashcard = (ActiveFlashcard + 1) % 3;

            // Enable the input on the new active card
            Flashcards[ActiveFlashcard].HasInput = true;
        }

        /// <summary>
        /// Retrieves the next flash card and returns a view model
        /// </summary>
        /// <returns></returns>
        private FlashcardViewModel GetNextFlashCard()
        {
            // DEVELOPMENT //
            FlashcardID++;

            var flashcard =  new FlashcardViewModel()
            {
                Side1Text = $"Test card {FlashcardID}",
                Side2Text = "Test card side 2",
            };
            //

            flashcard.OnCardSwipeLeft += OnCardSwipe;
            flashcard.OnCardSwipeRight += OnCardSwipe;

            return flashcard;
        }

        #endregion
    }
}
