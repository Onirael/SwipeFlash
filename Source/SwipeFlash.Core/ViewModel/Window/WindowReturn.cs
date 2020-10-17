namespace SwipeFlash.Core
{
    /// <summary>
    /// A delegate used to listen for window events
    /// </summary>
    /// <param name="parameter">The passed parameter</param>
    public delegate void ListenerDelegate(object parameter);

    /// <summary>
    /// The window return event types
    /// </summary>
    public enum WindowReturnEvent
    {
        /// <summary>
        /// File selected event
        /// </summary>
        FileSelected = 0,
        /// <summary>
        /// File saved event
        /// </summary>
        FileSaved = 1,
        /// <summary>
        /// User confirmation received event
        /// </summary>
        ConfirmationReceived = 2,
    }
}
