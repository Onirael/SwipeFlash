using System;
using System.Threading.Tasks;
using System.Windows.Input;

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
        public bool IsFlipped { get; set; }

        /// <summary>
        /// Whether the card starts on the B side
        /// </summary>
        public bool IsInverted { get; set; }

        /// <summary>
        /// Whether the card is currently showing the A side
        /// </summary>
        public bool IsOnSide1 { get; set; }

        /// <summary>
        /// The duration of the flip animation in seconds
        /// </summary>
        public double FlipDuration { get; set; } = 0.4;

        /// <summary>
        /// The duration of the swipe animation
        /// </summary>
        public double SwipeDuration { get; set; } = 0.4;

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

            // Sets the initial side
            IsOnSide1 = IsFlipped == IsInverted;
        }

        #endregion

        #region Command Methods

        private void FlipCard()
        {
            // Toggle card flip
            IsFlipped ^= true;

            UpdateSideWithDelay();
        }

        private void SwipeRight()
        {
            throw new NotImplementedException();
        }

        private void SwipeLeft()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Update the <see cref="IsOnSide1"/> bool after a delay,
        /// used when running the flip animation
        /// </summary>
        /// <returns></returns>
        public void UpdateSideWithDelay()
        {
            // Wait for half the duration of the flip animation
            Task.Delay((int)(FlipDuration * 0.25 * 1000)).ContinueWith((t) =>
            {
                // Update the value
                IsOnSide1 = IsFlipped == IsInverted;
            });
        }

        #endregion
    }
}
