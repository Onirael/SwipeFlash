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
        private void CreateWindow(object sender, WindowArgs e)
        {
            object newWindow = null;

            // Gets the currently open windows
            var openWindows = System.Windows.Application.Current.Windows;

            switch(e.TargetType)
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
                case WindowType.OpenFileExplorer:
                    {
                        // Checks if an OpenFileDialog is open
                        foreach (var openWindow in openWindows) { if (openWindow is OpenFileDialog) return; }
                        var dialog = new OpenFileDialog();
                        // Gets the file name from the file dialog
                        var appVM = IoC.Get<ApplicationViewModel>();
                        if (dialog.ShowDialog() == DialogResult.OK) { appVM.OnFileSelected?.Invoke(this, dialog.FileName); }
                        break;
                    }

                // Save file explorer window
                case WindowType.SaveFileExplorer:
                    {
                        // Checks if an OpenFileDialog is open
                        foreach (var openWindow in openWindows) { if (openWindow is SaveFileDialog) return; }
                        var dialog = new SaveFileDialog();
                        dialog.Filter = e.Message;
                        // Gets the file name from the file dialog
                        var appVM = IoC.Get<ApplicationViewModel>();
                        if (dialog.ShowDialog() == DialogResult.OK) { appVM.OnFileSaved?.Invoke(this, dialog.FileName); }
                        break;
                    }
                // Warning popup
                case WindowType.Warning:
                    {
                        // Checks if a warning wndow is already open
                        foreach (var openWindow in openWindows) { if (openWindow is System.Windows.Forms.MessageBox) return; }
                        System.Windows.Forms.MessageBox.Show(e.Message, 
                                                             "Warning", 
                                                             MessageBoxButtons.OK);
                        break;
                    }

                // Confirmation popup
                case WindowType.Confirmation:
                    {
                        // Checks if a warning wndow is already open
                        foreach (var openWindow in openWindows) { if (openWindow is System.Windows.Forms.MessageBox) return; }
                        var dialogResult = System.Windows.Forms.MessageBox.Show(e.Message,
                                                                                "Confirmation",
                                                                                MessageBoxButtons.OKCancel);
                        // Gets the user input from the dialog
                        var appVM = IoC.Get<ApplicationViewModel>();
                        appVM.OnConfirmation?.Invoke(this, dialogResult == DialogResult.OK);
                        break;
                    }

                // Family info window
                case WindowType.FamilyInfo:
                    // Checks if a FamilyInfoWindow is already open
                    foreach (var openWindow in openWindows) { if (openWindow is FamilyInfoWindow) return; }
                    newWindow = new FamilyInfoWindow(e.Message);
                    break;

                case WindowType.ParsedFlashcards:
                    // Checks if a ParsedFlashcardListWindow is already open
                    foreach (var openWindow in openWindows) { if (openWindow is ParsedFlashcardListWindow) return; }
                    newWindow = new ParsedFlashcardListWindow(e.Attachment);
                    break;

                case WindowType.FamilyStats:
                    // Checks if a FamilyStatsWindow is already open
                    foreach (var openWindow in openWindows) { if (openWindow is FamilyStatsWindow) return; }
                    newWindow = new FamilyStatsWindow(e.Message);
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
        private void DestroyWindow(object sender, WindowArgs e)
        {
            // Gets the currently open windows
            var openWindows = System.Windows.Application.Current.Windows;

            // For each currently open window
            foreach (var openWindow in openWindows)
            {
                // If the window is of the given type
                if (openWindow is BaseWindow baseWindow && baseWindow.BaseWindowType == e.TargetType)
                {
                    // Closes the window and runs the garbage collector
                    baseWindow.DataContext = null;
                    baseWindow.Close();
                    baseWindow.Owner.Focus();
                    GC.Collect();
                }
            }
        }

        #endregion 
    }
}
