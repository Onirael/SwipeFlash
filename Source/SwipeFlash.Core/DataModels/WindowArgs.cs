namespace SwipeFlash.Core
{
    /// <summary>
    /// An argument struct used when creating a window
    /// </summary>
    public struct WindowArgs
    {
        /// <summary>
        /// The type of the window
        /// </summary>
        public WindowType TargetType;

        /// <summary>
        /// The message of the window, 
        /// used for warning and confirmation windows
        /// </summary>
        public string Message;

        /// <summary>
        /// The attachment sent to the window
        /// </summary>
        public object Attachment;
    }
}
