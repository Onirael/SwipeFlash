﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Unsplasharp.Models;
using System.Windows.Media.Imaging;
using System.Diagnostics;

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

        private bool _appIllustrationsEnabled { get; set; } = Properties.Settings.Default.IllustrationsEnabled;
        /// <summary>
        /// Whether the image is currently visible on this side of the card
        /// </summary>
        public bool IsImageVisible => IsOnSide1 && HasIllustration && IsIllustrationLoaded && _appIllustrationsEnabled;

        private Photo _illustrationData;
        /// <summary>
        /// The Unsplasharp data of the illustration
        /// </summary>
        public Photo IllustrationData
        {
            get => _illustrationData;
            set
            {
                _illustrationData = value;
                // If the new value isn't null, set the illustration's Uri
                if (value != null) SetIllustrationUri(value.Urls.Small);
            }
        }

        private BitmapImage _illustration;
        /// <summary>
        /// The card's illustration bitmap
        /// </summary>
        public BitmapImage Illustration
        {
            get => _illustration;
            set
            {
                _illustration = value;
                // If the value isn't null, set the IsIllustrationLoaded flag
                if (value != null) IsIllustrationLoaded = true;
            }
        }

        /// <summary>
        /// Flag indicating whether the illustration was successfully loaded
        /// </summary>
        public bool IsIllustrationLoaded { get; private set; } = false;

        /// <summary>
        /// The image credit display text
        /// </summary>
        public string ImageCreditText => 
            IllustrationData != null ? 
            $"Image credit:\n{IllustrationData.User.FirstName} {IllustrationData.User.LastName} on Unsplash" : 
            "No illustration data";

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

            // Hook the OnSettingsUpdated method to settings' Property changed
            Properties.Settings.Default.PropertyChanged += OnSettingsUpdated;
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
        /// Enables the illustration on this card
        /// </summary>
        public void EnableIllustration()
        {
            HasIllustration = true;

            // If this card should have an illustration, call the API
            if (HasIllustration && Properties.Settings.Default.IllustrationsEnabled)
                FindIllustrationAsync();

        }

        /// <summary>
        /// Ansynchronously finds an appropriate illustration for this card via an API call
        /// </summary>
        private async void FindIllustrationAsync()
        {
            // Get the application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // If the server is unreachable
            if (!appVM.IsServerReachable)
                return;
            
            // Logs the remaining API calls count
            Debugger.Log(0, "UserLog", $"API calls remaining: {IoC.Get<ApplicationViewModel>().IllustrationsClient.RateLimitRemaining}\n");

            // Get the search results
            var foundPhotos = await appVM.IllustrationsClient.SearchPhotos(Side1Text.RemoveArticle());

            // Create a new RNG
            var rand = new Random();

            // If no photos were found, quit
            if (foundPhotos.Count == 0) return;
            
            // Get a random photo from the 10 first results
            IllustrationData = foundPhotos[rand.Next(Math.Min(foundPhotos.Count, 10))];
        }

        /// <summary>
        /// Downloads the illustration
        /// </summary>
        /// <returns></returns>
        public void SetIllustrationUri(string illustrationUri)
        {
            if (Illustration == null)
                Illustration = new BitmapImage();

            // Try initializing, an exception will be raised if it has begun Init but not ended it yet
            try { Illustration.BeginInit(); } catch { };
            Illustration.UriSource = new Uri(illustrationUri, UriKind.Absolute);
            try { Illustration.EndInit(); } catch { };
        }

        /// <summary>
        /// Called when the settings have been updated
        /// </summary>
        private void OnSettingsUpdated(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Update the illustrations enabled private property
            _appIllustrationsEnabled = Properties.Settings.Default.IllustrationsEnabled;

            // If the IllustrationsEnabled property changed to true
            bool isPropertyIllustrationsEnabled = e.PropertyName == nameof(Properties.Settings.Default.IllustrationsEnabled);
            if (isPropertyIllustrationsEnabled && _appIllustrationsEnabled && IllustrationData == null)
                // Fire the FindIllustration method
                FindIllustrationAsync();
        }

        #endregion
    }
}
