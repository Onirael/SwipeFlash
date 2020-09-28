using System;
using System.Threading.Tasks;
using System.Windows.Input;

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
        public AsyncObservableCollection<FlashcardViewModel> Flashcards { get; set; }

        /// <summary>
        /// A list containing swiped cards, limited to <see cref="FlashcardHistoryLength"/>
        /// </summary>
        public AsyncObservableCollection<FlashcardViewModel> FlashcardHistory { get; set; }

        /// <summary>
        /// The amount of cards stored in the history
        /// </summary>
        public int FlashcardHistoryLength { get; set; } = 3;

        /// <summary>
        /// The amount of flashcards stored in <see cref="Flashcards"/>
        /// </summary>
        public int FlashcardCount { get; set; } = 4;

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

        #region Commands

        /// <summary>
        /// The command triggered by the undo button
        /// </summary>
        public ICommand UndoButtonCommand { get; set; }

        /// <summary>
        /// The command triggered by the settings button
        /// </summary>
        public ICommand SettingsButtonCommand { get; set; }

        /// <summary>
        /// The command triggered by the edit card button
        /// </summary>
        public ICommand EditCardCommand { get; set; }

        #endregion

        #region Constructor

        public FlashcardHostViewModel()
        {
            // Initializes the flashcards array
            Flashcards = new AsyncObservableCollection<FlashcardViewModel>();

            // Initialize history array
            FlashcardHistory = new AsyncObservableCollection<FlashcardViewModel>();

            // Initializes the undo command button command
            UndoButtonCommand = new RelayCommand(OnUndoButtonPressed);

            // Initializes the undo command button command
            SettingsButtonCommand = new RelayCommand(OnSettingsButtonPressed);

            // Hooks the initlaize flashcard list function to the OnQueueInitialized event
            IoC.Get<FlashcardManager>().OnQueueInitialized += InitializeFlashcardList;

            // Initialize the JSON data
            // This must be done after InitializeFlashcardList has been hooked
            IoC.Get<FlashcardManager>().InitJSONData();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Called when the flashcard manager has finished loading the card queue
        /// initializes the flashcard list
        /// </summary>
        private void InitializeFlashcardList(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < FlashcardCount; ++i)
                {
                    // Get the new card
                    var newCard = GetNextFlashcard();

                    // Set the new card's queue position
                    newCard.CardQueuePosition = i;

                    // Enables input on the last card
                    if (i == 0) newCard.HasInput = true;

                    // Add the card to the list
                    Flashcards.Insert(0, newCard);
                }

                Task.Delay(1000).ContinueWith((t) =>
                {

                    // Set the application view model's content loaded flag
                    IoC.Get<ApplicationViewModel>().IsContentLoaded = true;

                });
            });
        }

        /// <summary>
        /// Called when the undo button is pressed
        /// </summary>
        private void OnUndoButtonPressed()
        {
            // Relay the command to the last swiped card if it exists
            if (FlashcardHistory.Count > 0)
                FlashcardHistory[FlashcardHistory.Count - 1].UndoSwipeCommand.Execute(null);
        }

        /// <summary>
        /// Called when the settings button is pressed
        /// </summary>
        private void OnSettingsButtonPressed()
        {
            IoC.Get<ApplicationViewModel>().IsSettingsMenuVisible = true;
        }

        /// <summary>
        /// Called when a card is swiped to the left or right
        /// </summary>
        public void OnCardSwipe(object sender, EventArgs e)
        {
            if (Flashcards.Count > 0)
            {
                // Disable the input on the old card
                Flashcards[Flashcards.Count - 1].HasInput = false;

                // If the maximum amount of cards hasn't been reached
                if (Flashcards.Count <= FlashcardCount)
                    // Insert new flashcard at index 0
                    PushCardToStack(GetNextFlashcard(), true);
                else
                    // Update stack
                    PushCardToStack(null, true);
            }
        }

        /// <summary>
        /// Called when a card's swipe is undone
        /// </summary>
        public void OnUndoSwipe(object sender, EventArgs e)
        {
            // If the history is empty, quit
            if (FlashcardHistory.Count == 0)
                return;

            // Get the card to be returned to the stack
            var undoCard = FlashcardHistory[FlashcardHistory.Count - 1];

            // Reset the card on card Loaded
            undoCard.ResetCard();

            Task.Delay((int)(undoCard.UndoDuration * 1000)).ContinueWith((t) =>
            {
                // Push card to end of list
                PushCardToStack(undoCard, false);

                // Remove card from history
                FlashcardHistory.RemoveAt(FlashcardHistory.Count - 1);
            });

            // Update queue positions with an offset to pre-update before the card is added to the stack post-animation
            UpdateQueuePositions(1);
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

                // Add top card to card history
                FlashcardHistory.Add(lastCard);

                // If the history is full
                if (FlashcardHistory.Count > FlashcardHistoryLength)
                {
                    // Destroy the UI element
                    FlashcardHistory[0].DestroyCard();

                    // Remove the card from the FlashcardHistory
                    FlashcardHistory.RemoveAt(0);
                }

                // Remove top card from flashcards array
                Flashcards.RemoveAt(Flashcards.Count - 1);

                if (card != null)
                    // Add new card to beginning of array
                    Flashcards.Insert(0, card);
            }
            // If the element is to be added to the end of the list
            else
            {
                if (card != null)
                    // Add new card to end
                    Flashcards.Add(card);
            }

            // Update queue positions
            UpdateQueuePositions();

        }

        /// <summary>
        /// Updates the <see cref="FlashcardViewModel.CardQueuePosition"/> of all cards in the <see cref="Flashcards"/> list
        /// </summary>
        private void UpdateQueuePositions(int offsetPosition = 0)
        {
            // For each flashcard
            int flashcardCount = Flashcards.Count;
            for (int i = 0; i < flashcardCount; ++i)
            {
                // Updates the card queue positions
                int newPos = Flashcards.Count - i - 1 + offsetPosition;
                Flashcards[i].CardQueuePosition = newPos;

                // If the card is at the front
                if (newPos == 0)
                {
                    // Delay the input activation on the new active card
                    FlashcardViewModel activeCard = Flashcards[i]; // Store the variable to avoid error when reading i value on the task thread
                    Task.Delay((int)(CardChangeInputDelay * 1000)).ContinueWith((t) => activeCard.HasInput = true); // Error when switching input !!!!!!!
                }
                else
                    // Disable the card's input
                    Flashcards[i].HasInput = false;
            }
        }

        /// <summary>
        /// Retrieves the next flash card and returns a view model
        /// </summary>
        /// <returns></returns>
        virtual protected FlashcardViewModel GetNextFlashcard()
        {
            // Create a new flashcard view model
            var flashcard = new FlashcardViewModel();

            // Get the data for the new flashcard
            var newFlashcardData = IoC.Get<FlashcardManager>().GetNext();

            // Initialize the flashcard view model with the  data
            flashcard.InitCard(newFlashcardData);

            // Hook methods to EventHandlers
            flashcard.OnCardSwipeLeft += OnCardSwipe;
            flashcard.OnCardSwipeRight += OnCardSwipe;
            flashcard.OnUndoSwipe += OnUndoSwipe;

            return flashcard;
        }

        #endregion
    }
}