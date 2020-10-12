namespace SwipeFlash.Core
{
    /// <summary>
    /// A struct containing the data of a flashcard
    /// </summary>
    public struct FlashcardData
    {
        /// <summary>
        /// The text on side 1 of the card
        /// </summary>
        public string Side1Text;

        /// <summary>
        /// The text on side 2 of the card
        /// </summary>
        public string Side2Text;

        /// <summary>
        /// The icon on side 1 of the card
        /// </summary>
        public string Side1Icon;

        /// <summary>
        /// The icon on side 2 of the card
        /// </summary>
        public string Side2Icon;

        /// <summary>
        /// Whether the card starts on side 2
        /// </summary>
        public bool IsInverted;

        /// <summary>
        /// Whether the card has an illustration
        /// </summary>
        public bool HasIllustration;

        /// <summary>
        /// Whether the card is the end of stack card
        /// </summary>
        public bool IsEndOfStackCard;

        /// <summary>
        /// The unique ID of the flashcard within its family
        /// </summary>
        public int FlashcardID;

        /// <summary>
        /// The name of the flashcard's family
        /// </summary>
        public string FamilyName;
    }
}
