using System;

namespace SwipeFlash.Core
{
    /// <summary>
    /// An interface used to manage windows
    /// </summary>
    public interface IWindowService
    {
        void CreateWindow(WindowType windowType);

        void DestroyWindow(WindowType windowType);
    }

    /// <summary>
    /// A class containing helper methods to manage windows
    /// </summary>
    public class WindowService : IWindowService
    {
        #region Event Handlers

        /// <summary>
        /// An event handler fired when a window is being created
        /// </summary>
        public EventHandler<WindowType> OnCreateWindow { get; set; }

        /// <summary>
        /// An event handler fired when a window is being destroyed
        /// </summary>
        public EventHandler<WindowType> OnDestroyWindow { get; set; }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a new instance of a window
        /// </summary>
        /// <param name="viewModel">The view model of the new window</param>
        public void CreateWindow(WindowType windowType)
        {
            OnCreateWindow?.Invoke(this, windowType);
        }

        /// <summary>
        /// Destroys an instance of a window
        /// </summary>
        public void DestroyWindow(WindowType windowType)
        {
            // If the event handler is not null, invoke it
            OnDestroyWindow?.Invoke(this, windowType);
        }

        #endregion
    }
}
