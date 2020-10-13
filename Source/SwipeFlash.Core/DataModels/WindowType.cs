namespace SwipeFlash.Core
{
    /// <summary>
    /// An enum describiing which child window is used
    /// </summary>
    public enum WindowType
    {
        /// <summary>
        /// No window
        /// </summary>
        None = 0,
        /// <summary>
        /// Main window
        /// </summary>
        Main = 1,
        /// <summary>
        /// The flashcard manager window
        /// </summary>
        FlashcardManager = 2,
        /// <summary>
        /// The flashcard parser window
        /// </summary>
        AddFlashcards = 3,
        /// <summary>
        /// The Windows file explorer
        /// </summary>
        OpenFileExplorer = 4,
        /// <summary>
        /// The Windows save file explorer
        /// </summary>
        SaveFileExplorer = 5,
        /// <summary>
        /// Warning popup
        /// </summary>
        Warning = 6,
        /// <summary>
        /// Confirmation popup
        /// </summary>
        Confirmation = 7,
        /// <summary>
        /// The information window of a given family
        /// </summary>
        FamilyInfo = 8,
    }
}
