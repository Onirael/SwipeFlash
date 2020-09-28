namespace SwipeFlash.Core
{
    /// <summary>
    /// A struct storing the data for a flashcard family
    /// </summary>
    public struct FlashcardFamilyData
    {
        /// <summary>
        /// Whether the card family is currently enabled
        /// </summary>
        public bool IsEnabled;

        /// <summary>
        /// The display name of the family
        /// </summary>
        public string Name;

        /// <summary>
        /// The number of cards in the family
        /// </summary>
        public int CardCount;
    }
}
