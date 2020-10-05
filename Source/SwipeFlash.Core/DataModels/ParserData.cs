using System.Collections.Generic;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A struct containing the data of a parsed flashcard
    /// </summary>
    public struct ParsedFlashcardData
    {
        /// <summary>
        /// The text on side 1 of the card
        /// </summary>
        public string Side1Text;

        /// <summary>
        /// The text on side 2 of the card
        /// </summary>
        public string Side2Text;
    }

    /// <summary>
    /// A struct containing the data of a parsed flashcard family
    /// </summary>
    public struct ParsedFlashcardFamilyData
    {
        /// <summary>
        /// The name of the family
        /// </summary>
        public string FamilyName;

        /// <summary>
        /// The name of the category of the family's side 1
        /// </summary>
        public string Category1;

        /// <summary>
        /// The name of the category of the family's side 2
        /// </summary>
        public string Category2;

        /// <summary>
        /// The logo of the category of side 1
        /// </summary>
        public string Logo1;

        /// <summary>
        /// The logo of the category of side 2
        /// </summary>
        public string Logo2;

        /// <summary>
        /// The list of flashcards in the family
        /// </summary>
        public List<ParsedFlashcardData> Flashcards;

    }

    /// <summary>
    /// A struct containing the data of a flashcard
    /// </summary>
    public struct ParserIgnoreRule
    {
        #region Public Properties

        /// <summary>
        /// The character pattern of the beginning of the line
        /// </summary>
        public string LineStartChars;

        /// <summary>
        /// The character pattern of the middle of the line
        /// </summary>
        public List<string> LineMidChars;

        /// <summary>
        /// The character pattern of the end of the line
        /// </summary>
        public string LineEndChars;

        #endregion

        #region Public Methods

        /// <summary>
        /// Computes the total character length, excluding '*' symbols, of the rule
        /// </summary>
        /// <returns>The minimum characters required to apply the rule</returns>
        public int GetRuleLength()
        {
            // Gets the start and end length
            int length = LineStartChars.Length + LineEndChars.Length;

            // Adds the length of the middle characters
            LineMidChars.ForEach((t) => length += t.Length);

            return length;
        }

        #endregion
    }
}
