using System.Drawing;

namespace SwipeFlash.Core
{
    /// <summary>
    /// The design model for the flashcards
    /// </summary>
    public class FlashcardDesignModel : FlashcardViewModel
    {
        #region Singleton

        /// <summary>
        /// The static instance of the design model
        /// </summary>
        public static FlashcardViewModel Instance => new FlashcardDesignModel();

        #endregion

        #region Constructor

        public FlashcardDesignModel()
        {
            Side1Text = "The door";
            Side2Text = "La puerta del dormitorio dónde duerme el señor Javier";
            Side1Icon = "🇪🇸";
            Side2Icon = "🇬🇧";
            IsFlipped = false;
            IsInverted = false;
            IsOnSide1 = true;
        }

        #endregion
    }
}
