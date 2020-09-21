using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Unsplasharp.Models;
using System.Windows.Media.Imaging;

namespace SwipeFlash.Core
{
    /// <summary>
    /// The view model of the flash card control
    /// </summary>
    public class FlashcardViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The text on the A side of the card
        /// </summary>
        public string Side1Text { get; set; }

        /// <summary>
        /// The text on the A side of the card
        /// </summary>
        public string Side1Icon { get; set; }

        /// <summary>
        /// The text on the B side of the card
        /// </summary>
        public string Side2Text { get; set; }

        /// <summary>
        /// The text on the B side of the card
        /// </summary>
        public string Side2Icon { get; set; }

        /// <summary>
        /// Whether the card was flipped by the user
        /// </summary>
        public bool IsFlipped { get; set; } = false;

        /// <summary>
        /// Whether the card takes in the input by the user
        /// </summary>
        public bool HasInput { get; set; } = false;

        /// <summary>
        /// Whether the card was swiped left by the user
        /// </summary>
        public bool IsSwipedLeft { get; private set; } = false;

        /// <summary>
        /// Whether the card was swiped right by the user
        /// </summary>
        public bool IsSwipedRight { get; private set; } = false;

        /// <summary>
        /// Whether the card starts on the B side
        /// </summary>
        public bool IsInverted { get; set; }

        /// <summary>
        /// Whether the card is currently showing the A side
        /// </summary>
        public bool IsOnSide1 { get; set; }

        /// <summary>
        /// The position in the card queue that the card is currently in
        /// If the value is set to 0, the card is the front card
        /// </summary>
        public int CardQueuePosition { get; set; } = 0;

        /// <summary>
        /// Whether the card is set to be destroyed
        /// </summary>
        public bool IsPendingDestroy { get; set; } = false;

        /// <summary>
        /// The duration of the flip animation in seconds
        /// </summary>
        public double FlipDuration { get; set; } = 0.4;

        /// <summary>
        /// The duration of the swipe animation
        /// </summary>
        public double SwipeDuration { get; set; } = 0.4;

        /// <summary>
        /// The duration of the swipe animation
        /// </summary>
        public double UndoDuration { get; set; } = .2;

        /// <summary>
        /// Whether the card should display an illustration
        /// </summary>
        public bool HasIllustration { get; set; } = false;

        /// <summary>
        /// The Unsplasharp data of the illustration
        /// </summary>
        public Photo IllustrationData
        {
            get => IllustrationData;
            set
            {
                IllustrationData = value;
                if (value != null) DownloadIllustration();
            }
        }

        /// <summary>
        /// The card's illustration bitmap
        /// </summary>
        public BitmapImage Illustration
        {
            get => Illustration;
            set
            { Illustration = value;
                if (value != null) IsIllustrationLoaded = true;
            }
        }

        /// <summary>
        /// Set to true if the illustration was successfully loaded
        /// </summary>
        public bool IsIllustrationLoaded { get; set; } = false;

        #endregion

        #region Event Handlers

        public EventHandler OnCardSwipeLeft;

        public EventHandler OnCardSwipeRight;

        public EventHandler OnUndoSwipe;

        #endregion

        #region Commands

        /// <summary>
        /// The command for flipping the card
        /// </summary>
        public ICommand FlipCommand { get; set; }

        /// <summary>
        /// The command for swiping the card to the left
        /// </summary>
        public ICommand SwipeLeftCommand { get; set; }

        /// <summary>
        /// The command for swiping the card to the right
        /// </summary>
        public ICommand SwipeRightCommand { get; set; }

        /// <summary>
        /// Undo the previous swipe
        /// </summary>
        public ICommand UndoSwipeCommand { get; set; }

        #endregion

        #region Constructor

        public FlashcardViewModel()
        {
            // Initializes the flip command
            FlipCommand = new RelayCommand(FlipCard);

            // Initializes the flip command
            SwipeLeftCommand = new RelayCommand(SwipeLeft);

            // Initializes the flip command
            SwipeRightCommand = new RelayCommand(SwipeRight);

            // Initializes the undo swipe command
            UndoSwipeCommand = new RelayCommand(UndoSwipe);

            // Sets the initial side
            IsOnSide1 = IsFlipped == IsInverted;

            // Initializes event handlers
            OnCardSwipeLeft = new EventHandler((ss, ee) => { });
            OnCardSwipeRight = new EventHandler((ss, ee) => { });
            OnUndoSwipe = new EventHandler((ss, ee) => { });

            // Get the application view model
            var appVieModel = IoC.Get<ApplicationViewModel>();


            if (HasIllustration && Properties.Settings.Default.IllustrationsEnabled)
            {
                FindIllustrationAsync();
            }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Flips the card
        /// </summary>
        private void FlipCard()
        {
            // Get application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // Quit if settings menu is visible
            if (appVM.IsSettingsMenuVisible)
                return;

            // Toggle card flip
            IsFlipped ^= true;

            UpdateSideWithDelay();
        }

        /// <summary>
        /// Swipes the card to the left
        /// </summary>
        private void SwipeLeft()
        {
            // Get application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // Quit if settings menu is visible
            if (appVM.IsSettingsMenuVisible)
                return;

            // Swipe to the left if the card hasn't yet been swiped
            if (!IsSwipedRight) IsSwipedLeft = true;

            // Fire swipe left event
            OnCardSwipeLeft(this, null);
        }

        /// <summary>
        /// Swipe the card to the right
        /// </summary>
        private void SwipeRight()
        {
            // Get application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // Quit if settings menu is visible
            if (appVM.IsSettingsMenuVisible)
                return;

            // Swipe to the right if the card hasn't yet been swiped
            if (!IsSwipedLeft) IsSwipedRight = true;

            // Fire swipe right event
            OnCardSwipeRight(this, null);
        }

        /// <summary>
        /// Undo the previous swipe
        /// </summary>
        private void UndoSwipe()
        {
            // Get application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // Quit if settings menu is visible
            if (appVM.IsSettingsMenuVisible)
                return;

            // Fire undo swipe event
            OnUndoSwipe(this, null);
        }


        #endregion

        #region Helper Methods

        /// <summary>
        /// Called when the card has been swiped out
        /// </summary>
        public void DestroyCard()
        {
            IsPendingDestroy = true;
        }

        /// <summary>
        /// Resets the card to its unswiped and unflipped state
        /// </summary>
        public void ResetCard()
        {
            // Reverts the swipe
            IsSwipedLeft = IsSwipedRight = false;

            // Reverts the flip
            IsFlipped = false; // Necessary ?
        }

        /// <summary>
        /// Update the <see cref="IsOnSide1"/> bool after a delay,
        /// used when running the flip animation
        /// </summary>
        /// <returns></returns>
        private void UpdateSideWithDelay()
        {
            // Wait for half the duration of the flip animation
            Task.Delay((int)(FlipDuration * 0.25 * 1000)).ContinueWith((t) =>
            {
                // Update the value
                IsOnSide1 = IsFlipped == IsInverted;
            });
        }

        /// <summary>
        /// Ansynchronously finds an appropriate illustration for this card
        /// </summary>
        private async Task FindIllustrationAsync()
        {
            // Get the search results
            var foundPhotos = await IoC.Get<ApplicationViewModel>().IllustrationsClient.SearchPhotos(Side1Text);

            // Create a new RNG
            var rand = new Random();

            // Get a random photo from the 10 first results
            IllustrationData = foundPhotos[rand.Next(10)];
        }

        /// <summary>
        /// Downloads the illustration
        /// </summary>
        /// <returns></returns>
        private void DownloadIllustration()
        {
            // Initializes the bitmap from the URL
            var filePath = IllustrationData.Urls.Thumbnail;

            Illustration.BeginInit();
            Illustration.UriSource = new Uri(filePath, UriKind.Absolute);
            Illustration.EndInit();
        }

        #endregion

    }
}
