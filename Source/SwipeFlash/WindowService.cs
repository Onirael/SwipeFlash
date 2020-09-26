using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// An interface used to manage windows
    /// </summary>
    interface IWindowService
    {
        void ShowWindow(object viewModel);
    }

    /// <summary>
    /// A class containing helper methods to manage windows
    /// </summary>
    public class WindowService : IWindowService
    {
        #region Helper Methods

        /// <summary>
        /// Creates a new instance of a window
        /// </summary>
        /// <param name="viewModel">The view model of the new window</param>
        public void ShowWindow(object viewModel)
        {
            // Show the window
        }

        #endregion
    }
}
