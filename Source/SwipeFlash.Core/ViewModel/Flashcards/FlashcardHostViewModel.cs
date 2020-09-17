using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SwipeFlash.Core
{
    /// <summary>
    /// The view model for the flashcard host
    /// </summary>
    public class FlashcardHostViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// An observable collection of view models for the flashcards
        /// </summary>
        public ObservableCollection<FlashcardViewModel> Flashcards { get; set; }
        
        /// <summary>
        /// A list containing cards pending destruction to preserve them from garbage collection
        /// </summary>
        public AsyncObservableCollection<FlashcardViewModel> PendingDestroyCards { get; set; }

        public List<FlashcardViewModel> FlashcardHistory { get; set; }

        /// <summary>
        /// The amount of cards stored in the history
        /// </summary>
        public int FlashcardHistoryLength { get; set; } = 3;

        /// <summary>
        /// The amount of flashcards stored in <see cref="Flashcards"/>
        /// </summary>
        public int FlashcardCount { get; set; } = 5;

        /// <summary>
        /// The time in seconds between the swipe of the previous card 
        /// and the activation of the input on the new active card
        /// </summary>
        public double CardChangeInputDelay { get; set; } = 0.15;

        /// <summary>
        /// DEVELOPMENT
        /// </summary>
        private int FlashcardID { get; set; } = 0;

        #endregion

        #region Constructor

        public FlashcardHostViewModel()
        {
            // Initializes the flashcards array
            Flashcards = new ObservableCollection<FlashcardViewModel>();

            for (int i = 0; i < FlashcardCount; ++i)
            {
                // Get the new card
                var newCard = GetNextFlashCard();

                // Set the new card's queue position
                newCard.CardQueuePosition = i;

                // Enables input on the last card
                if (i == 0) newCard.HasInput = true;

                // Add the card to the list
                Flashcards.Insert(0, newCard);
            }

            // Initialize PendingDestroyCards collection
            PendingDestroyCards = new AsyncObservableCollection<FlashcardViewModel>();

            // Initialize history array
            FlashcardHistory = new List<FlashcardViewModel>();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Called when a card is swiped to the left or right
        /// </summary>
        public void OnCardSwipe(object sender, EventArgs e)
        {
            if (Flashcards.Count > 0)
            {
                // Disable the input on the old card
                Flashcards[Flashcards.Count - 1].HasInput = false;

                // Insert new flashcard at index 0
                PushCardToStack(GetNextFlashCard(), true);
            }
        }

        public void OnUndo(object sender, EventArgs e)
        {
            // If the history is empty quit
            if (FlashcardHistory.Count == 0)
                return;

            // Push card to end of array
            PushCardToStack(FlashcardHistory[FlashcardHistoryLength - 1], false);

            //
            // Reverse the cards' queue position animation
            // Reverse slide out
            //
        }

        /// <summary>
        /// Adds a card to the <see cref="Flashcards"/> array
        /// pushes all the cards in the array, removing the last element
        /// </summary>
        /// <param name="card">The pushed card's view model</param>
        /// <param name="addAsFirst">True if the card is to be added as first element, false if it is to be added as last element</param>
        public void PushCardToStack(FlashcardViewModel card, bool addAsFirst)
        {
            // If the card is to be pushed to the beginning of the list
            if (addAsFirst)
            {
                // Get top card
                FlashcardViewModel lastCard = Flashcards[Flashcards.Count - 1];

                // Add top card to PendingDestroyCards array
                PendingDestroyCards.Add(lastCard);

                // Add top card to card history
                FlashcardHistory.Add(lastCard);

                // If the history is full
                if (FlashcardHistory.Count > FlashcardHistoryLength)
                    // Remove first history element
                    FlashcardHistory.RemoveAt(0);

                // Remove top card from flashcards array
                Flashcards.RemoveAt(Flashcards.Count - 1);

                // Remove top card from array after the duration of the swipe
                Task.Delay((int)(lastCard.SwipeDuration * 1000)).ContinueWith((t) =>
                {
                    // Remove the card from the PendingDestroyCards
                    PendingDestroyCards.Remove(lastCard);

                    // Destroy the UI element
                    lastCard.DestroyCard();
                });

                // Add new card to beginning of array
                Flashcards.Insert(0, card);
            }
            // If the element is to be added to the end of the list
            else
            {
                // Reset the card
                card.ResetCard();

                // Add new card to end
                Flashcards.Add(card);
            }

            // Update queue positions
            UpdateQueuePositions();

        }

        /// <summary>
        /// Updates the <see cref="FlashcardViewModel.CardQueuePosition"/> of all cards in the <see cref="Flashcards"/> list
        /// </summary>
        private void UpdateQueuePositions()
        {
            // For each flashcard
            int flashcardCount = Flashcards.Count;
            for (int i = 0; i < flashcardCount; ++i)
            {
                // Updates the card queue positions
                int newPos = Flashcards.Count - i - 1;
                Flashcards[i].CardQueuePosition = newPos;

                // If the card is at the front
                if (newPos == 0)
                {
                    // Enable its input after a delay
                    FlashcardViewModel activeCard = Flashcards[i];
                    Task.Delay((int)(CardChangeInputDelay * 1000)).ContinueWith((t) =>
                    {
                        activeCard.HasInput = true;
                    });
                }
            }
        }

        /// <summary>
        /// Retrieves the next flash card and returns a view model
        /// </summary>
        /// <returns></returns>
        private FlashcardViewModel GetNextFlashCard()
        {
            // DEVELOPMENT //
            FlashcardID++;

            var flashcard = new FlashcardViewModel()
            {
                Side1Text = $"Test card {FlashcardID}",
                Side2Text = "Test card side 2",
                Side1Icon = "🇪🇸",
                Side2Icon = "🇬🇧",

                HasInput = false,

                CardQueuePosition = FlashcardCount,
            };
            //

            flashcard.OnCardSwipeLeft += OnCardSwipe;
            flashcard.OnCardSwipeRight += OnCardSwipe;

            return flashcard;
        }

        #endregion
    }
}
