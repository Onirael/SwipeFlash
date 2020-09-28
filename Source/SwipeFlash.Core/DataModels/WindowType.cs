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
        FileExplorer = 4,
    }
}
