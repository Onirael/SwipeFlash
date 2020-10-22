using System.Collections.Generic;

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
        public CategoryData Category1;

        /// <summary>
        /// The category of the family's side 1
        /// </summary>
        public CategoryData Category2;

        /// <summary>
        /// Whether the family has illustrations
        /// </summary>
        public bool HasIllustrations;

        public override bool Equals(object obj)
        {
            // Type checks the object
            if (!(obj is FlashcardFamilyData familyData))
                return false;

            // Gets whether the static data is equal
            bool isEqual = familyData.Category1.Equals(Category1) &&
                           familyData.Category2.Equals(Category2) &&
                           familyData.Name == Name &&
                           familyData.HasIllustrations == HasIllustrations;
            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
