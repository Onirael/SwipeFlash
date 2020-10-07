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
    /// A struct containing the data of a parser line variable
    /// </summary>
    public struct ParserLineVariable
    {
        /// <summary>
        /// The name of the variable
        /// </summary>
        public string VariableName;

        /// <summary>
        /// True if the variable was flagged as optional
        /// </summary>
        public bool IsOptional;

        /// <summary>
        /// The instruction linked to this variable
        /// </summary>
        public ParserVariableInstruction VariableInstruction;
    }
}
