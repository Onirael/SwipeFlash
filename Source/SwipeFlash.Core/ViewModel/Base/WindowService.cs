using System;

namespace SwipeFlash.Core
{
    /// <summary>
    /// An interface used to manage windows
    /// </summary>
    public interface IWindowService
    {
        void CreateWindow(WindowArgs args);

        void DestroyWindow(WindowArgs args);
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
        public EventHandler<WindowArgs> OnCreateWindow { get; set; }

        /// <summary>
        /// An event handler fired when a window is being destroyed
        /// </summary>
        public EventHandler<WindowArgs> OnDestroyWindow { get; set; }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a new instance of a window
        /// </summary>
        /// <param name="viewModel">The view model of the new window</param>
        public void CreateWindow(WindowArgs args)
        {
            OnCreateWindow?.Invoke(this, args);
        }

        /// <summary>
        /// Destroys an instance of a window
        /// </summary>
        public void DestroyWindow(WindowArgs args)
        {
            // If the event handler is not null, invoke it
            OnDestroyWindow?.Invoke(this, args);
        }

        #endregion
    }
}
