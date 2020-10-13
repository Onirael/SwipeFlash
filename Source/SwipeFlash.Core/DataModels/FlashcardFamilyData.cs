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
        /// The name of the family
        /// </summary>
        public string Name;

        /// <summary>
        /// The number of cards in the family
        /// </summary>
        public int CardCount;

        /// <summary>
        /// The category of the family's side 1
        /// </summary>
        public string Category1;

        /// <summary>
        /// The category of the family's side 2
        /// </summary>
        public string Category2;
        
        /// <summary>
        /// The logo of the family's side 1
        /// </summary>
        public string Logo1;

        /// <summary>
        /// The logo of the family's side 2
        /// </summary>
        public string Logo2;
    }
}
