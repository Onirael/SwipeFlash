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

        public override bool Equals(object obj)
        {
            // Type checks the object
            if (!(obj is FlashcardFamilyData familyData))
                return false;

            // Gets whether the static data is equal
            bool isEqual = familyData.Category1 == Category1 &&
                           familyData.Category2 == Category2 &&
                           familyData.Name == Name &&
                           familyData.Logo1 == Logo1 &&
                           familyData.Logo2 == Logo2;
            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
