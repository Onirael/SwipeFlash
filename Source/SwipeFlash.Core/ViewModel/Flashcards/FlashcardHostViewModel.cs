using System;

namespace SwipeFlash.Core
{
    /// <summary>
    /// The view model for the flashcard host
    /// </summary>
    public class FlashcardHostViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// An array of view models for the flashcards
        /// </summary>
        public FlashcardViewModel[] Flashcards { get; set; }

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
                // Gets the next card
                Flashcards[i] = GetNextFlashCard();
                // Sets the queue position of the card
                Flashcards[i].CardQueuePosition = FlashcardCount - i - 1;
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
            Flashcards[FlashcardCount-1].HasInput = false;

            // Append new flashcard
            PushCardToArray(GetNextFlashCard());

            // Enable the input on the new active card
            Flashcards[FlashcardCount-1].HasInput = true;
        }

        /// <summary>
        /// Adds a card to the <see cref="Flashcards"/> array
        /// pushes all the cards in the array, removing the last element
        /// </summary>
        /// <param name="card"></param>
        public void PushCardToArray(FlashcardViewModel card)
        {
            // Offset all items in the array
            for (int i = FlashcardCount-1; i > 0; --i)
            {
                // Offsets the item
                Flashcards[i] = Flashcards[i-1];

                // Updates the card queue positions
                Flashcards[i].CardQueuePosition = FlashcardCount - i - 1;
            }

            // Sets the new card's queue position
            card.CardQueuePosition = FlashcardCount - 1;

            // Adds the card to the beginning of the array
            Flashcards[0] = card;
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
