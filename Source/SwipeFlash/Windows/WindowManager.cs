using SwipeFlash.Core;
using System;
using System.Windows;
using System.Windows.Forms;

namespace SwipeFlash
{
    /// <summary>
    /// A window manager for the view
    /// </summary>
    public class WindowManager
    {
        #region Constructor

        public WindowManager()
        {
            // Gets the window service
            var windowService = IoC.Get<WindowService>();

            // Hooks the CreateWindow method to the window service event
            windowService.OnCreateWindow += CreateWindow;

            windowService.OnDestroyWindow += DestroyWindow;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Creates a new window of the given <see cref="WindowType"/>
        /// </summary>
        /// <param name="sender">The window service</param>
        /// <param name="e">The type of the new window</param>
        private void CreateWindow(object sender, WindowType e)
        {
            object newWindow = null;

            // Gets the currently open windows
            var openWindows = System.Windows.Application.Current.Windows;

            switch(e)
            {
                // Flashcard manager window
                case WindowType.FlashcardManager:
                    // Checks if a FlashcardManagerWindow is already open
                    foreach (var openWindow in openWindows) { if (openWindow is FlashcardManagerWindow) return; }
                    newWindow = new FlashcardManagerWindow();
                    break;

                // Add flashcards window
                case WindowType.AddFlashcards:
                    // Checks if a AddFlashcardsWindow is already open
                    foreach (var openWindow in openWindows) { if (openWindow is AddFlashcardsWindow) return; }
                    newWindow = new AddFlashcardsWindow();
                    break;

                // File explorer window
                case WindowType.FileExplorer:
                    // Checks if an OpenFileDialog is open
                    foreach (var openWindow in openWindows) { if (openWindow is OpenFileDialog) return; }
                    var dialog = new OpenFileDialog();
                    // Gets the file name from the file dialog
                    var appVM = IoC.Get<ApplicationViewModel>();
                    if (dialog.ShowDialog() == DialogResult.OK) { appVM.OnFileSelected?.Invoke(this, dialog.FileName); }
                    break;

                // Should not be hit
                default:
                    break;
            }

            // If the window isn't null, show the window
            if (newWindow is Window window)
            {
                window.ShowInTaskbar = false;
                window.Owner = System.Windows.Application.Current.MainWindow;
                window.Show();
            }

        }

        /// <summary>
        /// Destroys an existing instance of a window
        /// </summary>
        /// <param name="sender">The window service</param>
        /// <param name="e">The type of the window to destroy</param>
        private void DestroyWindow(object sender, WindowType e)
        {
            // Gets the currently open windows
            var openWindows = System.Windows.Application.Current.Windows;

            // For each currently open window
            foreach (var openWindow in openWindows)
            {
                // If the window is of the given type
                if (openWindow is BaseWindow baseWindow && baseWindow.BaseWindowType == e)
                    // Destroy it
                    baseWindow.Close();
            }

        }

        #endregion 
    }
}
