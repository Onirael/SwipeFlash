using System;
using System.IO;
using System.Windows.Input;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A view model for the Add flashcards window
    /// </summary>
    public class AddFlashcardsWindowViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The path of the selected file
        /// </summary>
        public string SelectedFilePath { get; set; } = "";

        /// <summary>
        /// The name of the selected file
        /// </summary>
        public string SelectedFileName => Path.GetFileName(SelectedFilePath);

        /// <summary>
        /// The display name of the selected file, shows a message if no file was selected
        /// </summary>
        public string SelectedFileDisplayName => SelectedFileName == "" ? "No file was selected" : SelectedFileName;

        /// <summary>
        /// The name of the flashcard family to create/append to
        /// </summary>
        public string FamilyName { get; set; } = "";

        /// <summary>
        /// The user-defined line ignore pattern 
        /// </summary>
        public string IgnorePatternDescription { get; set; } = "";

        /// <summary>
        /// The user-defined separators to use in a line
        /// </summary>
        public string SeparatorsDescription { get; set; } = "";

        /// <summary>
        /// The user-defined pattern of the line
        /// </summary>
        public string LinePatternDescription { get; set; } = "";

        #endregion

        #region Commands

        /// <summary>
        /// A command to open the windows file browser
        /// </summary>
        public ICommand SelectFileCommand { get; set; }

        /// <summary>
        /// A command fired when the OK button is pressed
        /// </summary>
        public ICommand PressOKCommand { get; set; }

        /// <summary>
        /// A command fired when the Cancel button is pressed
        /// </summary>
        public ICommand PressCancelCommand { get; set; }

        #endregion

        #region Constructor

        public AddFlashcardsWindowViewModel()
        {
            // Initializes the OK button command
            PressOKCommand = new RelayCommand(OnOKPressed);

            // Initializes the Cancel button command
            PressCancelCommand = new RelayCommand(OnCancelPressed);

            // Initializes the choose file button command
            SelectFileCommand = new RelayCommand(OnSelectFilePressed);
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Called when the OK button is pressed
        /// </summary>
        private void OnOKPressed()
        {
            IoC.Get<WindowService>().DestroyWindow(WindowType.AddFlashcards);
        }

        /// <summary>
        /// Called when the Cancel button is pressed
        /// </summary>
        private void OnCancelPressed()
        {
            IoC.Get<WindowService>().DestroyWindow(WindowType.AddFlashcards);
        }

        /// <summary>
        /// Called when the Choose file button is pressed
        /// </summary>
        private void OnSelectFilePressed()
        {
            // Create a single self-unhookable event
            // hooked to the application's file selected event handler
            EventHandler<string> onFileSelected = null;
            onFileSelected = (sender, fileName) =>
            {
                // Unhook the event
                IoC.Get<ApplicationViewModel>().OnFileSelected -= onFileSelected;

                // Run the animation
                SelectedFilePath = fileName;
            };

            // Hooks the event to the application view model
            IoC.Get<ApplicationViewModel>().OnFileSelected += onFileSelected;

            // Creates the window
            IoC.Get<WindowService>().CreateWindow(WindowType.FileExplorer);
        }

        #endregion
    }
}
