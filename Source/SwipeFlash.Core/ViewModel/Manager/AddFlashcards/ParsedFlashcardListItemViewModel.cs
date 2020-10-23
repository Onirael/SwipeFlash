namespace SwipeFlash.Core
{
    /// <summary>
    /// A view model controlling the items of the parsed flashcard list
    /// </summary>
    public class ParsedFlashcardListItemViewModel : BaseViewModel
    {
        /// <summary>
        /// Whether the side 1 text is in edit mode
        /// </summary>
        public bool IsSide1InEditMode { get; set; }

        /// <summary>
        /// Whether the side 2 text is in edit mode
        /// </summary>
        public bool IsSide2InEditMode { get; set; }

        /// <summary>
        /// The text of side 1
        /// </summary>
        public string Side1Text { get; set; }

        /// <summary>
        /// The text of side 2
        /// </summary>
        public string Side2Text { get; set; }

        /// <summary>
        /// The default side 1 text
        /// </summary>
        public string Side1DefaultText { get; set; }

        /// <summary>
        /// The default side 2 text
        /// </summary>
        public string Side2DefaultText { get; set; }

        /// <summary>
        /// Whether the side 1 undo button is visible
        /// </summary>
        public bool IsSide1UndoButtonVisible => Side1Text != Side1DefaultText;

        /// <summary>
        /// Whether the side 2 undo button is visible
        /// </summary>
        public bool IsSide2UndoButtonVisible => Side2Text != Side2DefaultText;
    }
}