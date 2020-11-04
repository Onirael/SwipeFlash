using System;
using System.Collections.Generic;
using System.Linq;
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

        private AsyncObservableCollection<FlashcardViewModel> _flashcards;
        /// <summary>
        /// An observable collection of view models for the flashcards
        /// </summary>
        public AsyncObservableCollection<FlashcardViewModel> Flashcards
        {
            get => _flashcards;
            set => _flashcards = value;
        }

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
        /// Whether the host is currently in flashcard edit mode
        /// </summary>
        public bool IsInEditMode { get; set; } = false;

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

        /// <summary>
        /// The command triggered by the cancel edit card button
        /// </summary>
        public ICommand CancelEditCommand { get; set; }

        /// <summary>
        /// The command triggered by the confirm edit card button
        /// </summary>
        public ICommand ConfirmEditCommand { get; set; }

        /// <summary>
        /// The command triggered by the delete card button
        /// </summary>
        public ICommand DeleteCardCommand { get; set; }

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

            // Initializes the edit card button command
            EditCardCommand = new RelayCommand(OnEditCardButtonPressed);

            // Initializes the cancel edit card button command
            CancelEditCommand = new RelayCommand(OnCancelEditButtonPressed);

            // Initializes the confirm edit card button command
            ConfirmEditCommand = new RelayCommand(OnConfirmEditButtonPressed);

            // Initializes the delete card button command
            DeleteCardCommand = new RelayCommand(OnDeleteCardButtonPressed);

            IoC.Get<ApplicationViewModel>().OnPreviewKeyDown += OnPreviewKeyDown;

            var fm = IoC.Get<FlashcardManager>();

            // Hooks the initlaize flashcard list function to the OnQueueInitialized event
            fm.OnQueueInitialized += InitializeFlashcardList;

            // Hooks a flashcard family data update method to the families updated event
            fm.OnFamiliesUpdated += UpdateFlashcardFamilyData;

            // Initialize the JSON data
            // This must be done after InitializeFlashcardList has been hooked
            IoC.Get<FlashcardManager>().InitJSONData();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Updates the all flashcards' family related data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateFlashcardFamilyData(object sender, EventArgs e)
        {
            // Gets the families data
            var familiesData = IoC.Get<FlashcardManager>().FlashcardFamilies;

            // For each flashcard
            foreach (var flashcard in Flashcards)
            {
                // The family of the flashcard
                var cardFamily = familiesData.FirstOrDefault(family => family.Name == flashcard.CardFamily);

                // If the card family was found
                if (cardFamily.IsFamilyDataValid(false))
                {
                    // Finds the corresponding flashcard family
                    var foundFamily = IoC.Get<FlashcardManager>().FindFlashcardFamily(flashcard.CardID,
                                                                                      flashcard.Side1Text,
                                                                                      flashcard.Side2Text);
                    // Gets the new family of the card
                    cardFamily = familiesData.FirstOrDefault(family => family.Name == (string)foundFamily["family"]);

                    // Sets the flashcard family name in the flashcard
                    flashcard.CardFamily = cardFamily.Name;
                }

                // Updates the Family has illustrations flag
                flashcard.FamilyHasIllustration = cardFamily.HasIllustrations;
            }
        }

        /// <summary>
        /// Called when the flashcard manager has finished loading the card queue
        /// initializes the flashcard list
        /// </summary>
        private void InitializeFlashcardList(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var initialCardList = new List<FlashcardViewModel>(FlashcardCount);

                for (int i = 0; i < FlashcardCount; ++i)
                {
                    // Get the new card
                    var newCard = GetNextFlashcard();

                    // Set the new card's queue position
                    newCard.CardQueuePosition = i;

                    // Add the card to the list
                    initialCardList.Insert(0, newCard);
                }
                
                // Waits for the splash screen animation to start before loading
                Task.Delay(500).ContinueWith((t) =>
                {
                    // Empties the flashcards collection
                    Flashcards.Clear();

                    // Adds all flashcards from the list to the collection
                    initialCardList.ForEach((card) =>
                    {
                        Flashcards.Add(card);
                    });

                    Task.Delay(100).ContinueWith((t2) =>
                    {
                        // Set the application view model's content loaded flag
                        IoC.Get<ApplicationViewModel>().IsContentLoaded = true;
                    });
                });
            });
        }

        /// <summary>
        /// Called when the delete card button is pressed
        /// </summary>
        private void OnDeleteCardButtonPressed()
        {
            // Gets the application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // Listens for the confirmation event
            appVM.ListenForEvent<bool>(WindowReturnEvent.ConfirmationReceived,
                                       new ListenerDelegate(OnDeleteCardConfirmed));

            // Creates the confirmation window
            IoC.Get<WindowService>().CreateWindow(new WindowArgs()
            {
                Message = "Confirm card deletion ? This cannot be undone",
                TargetType = WindowType.Confirmation,
            });
        }

        /// <summary>
        /// Called when the user confirms the deletion of a card
        /// </summary>
        /// <param name="parameter"></param>
        private void OnDeleteCardConfirmed(object parameter)
        {
            // If the user has cancelled the operation, quit
            if (!(bool)parameter)
                return;

            // Gets the stack top flashcard
            var lastFlashcard = Flashcards[Flashcards.Count - 1];

            // Deletes it
            lastFlashcard.DeleteCard();

            // Exits edit mode
            IsInEditMode = false;
        }

        /// <summary>
        /// Called when the undo button is pressed
        /// </summary>
        private void OnUndoButtonPressed()
        {
            // Relay the command to the last swiped card if it exists
            if (FlashcardHistory.Count > 0)
                OnUndoSwipe(this, null);
        }

        /// <summary>
        /// Called when the settings button is pressed
        /// </summary>
        private void OnSettingsButtonPressed()
        {
            IoC.Get<ApplicationViewModel>().IsSettingsMenuVisible = true;
        }

        /// <summary>
        /// Called when the edit card button is pressed
        /// </summary>
        private void OnEditCardButtonPressed()
        {
            var lastFlashcard = Flashcards[Flashcards.Count - 1];

            // If the active flashcard if the end of stack card, quit
            if (lastFlashcard.IsEndOfStackCard)
                return;

            // Enables the edit mode
            IsInEditMode = true;

            // Enables the flashcard's edit mode
            lastFlashcard.IsInEditMode = true;
        }

        /// <summary>
        /// Called when the confirm edit card button is pressed
        /// </summary>
        private void OnConfirmEditButtonPressed()
        {
            // Disables the edit mode
            IsInEditMode = false;

            // Disables the flashcard's edit mode
            Flashcards[Flashcards.Count - 1].ConfirmEdit();
        }

        /// <summary>
        /// Called when the cancel edit card button is pressed
        /// </summary>
        private void OnCancelEditButtonPressed()
        {
            // Disables the edit mode
            IsInEditMode = false;

            // Signals to the card that the edit has been cancelled
            Flashcards[Flashcards.Count - 1].CancelEdit();
        }

        /// <summary>
        /// Called when a card is swiped to the left or right
        /// </summary>
        public void OnCardSwipe(object sender, bool isSwipedRight)
        {
            if (Flashcards.Count > 0)
            {
                // If the maximum amount of cards hasn't been reached
                if (Flashcards.Count <= FlashcardCount)
                    // Insert new flashcard at index 0
                    PushCardToStack(GetNextFlashcard(), true);
                else
                    // Update stack
                    PushCardToStack(null, true);
            }

            // Gets the application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // If the card was swiped to the right
            if (isSwipedRight)
                appVM.MainWindowVM.IsCardSwipedRight = true;
            else
                appVM.MainWindowVM.IsCardSwipedLeft = true;
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
            flashcard.OnCardSwipe += OnCardSwipe;

            return flashcard;
        }

        /// <summary>
        /// Called when any preview key has been pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewKeyDown(object sender, Key previewKey)
        {
            // If the settings menu is visible, quit
            if (IoC.Get<ApplicationViewModel>().IsSettingsMenuVisible ||
                IsInEditMode)
                return;
            
            switch(previewKey)
            {
                case Key.R:
                    OnUndoSwipe(sender, null);
                    break;
                case Key.Space:
                    Flashcards.Last().FlipCommand.Execute(this);
                    break;
                case Key.Left:
                    Flashcards.Last().SwipeLeftCommand.Execute(this);
                    break;
                case Key.Right:
                    Flashcards.Last().SwipeRightCommand.Execute(this);
                    break;
            }
        }

        #endregion
    }
}