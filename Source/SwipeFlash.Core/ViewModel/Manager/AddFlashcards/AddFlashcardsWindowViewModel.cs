using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json.Linq;

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
        /// The emoji logo for side 1
        /// </summary>
        public string Side1Logo { get; set; } = "";

        /// <summary>
        /// The emoji logo for side 2
        /// </summary>
        public string Side2Logo { get; set; } = "";

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

        /// <summary>
        /// The existing categories
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// The category for side 1
        /// </summary>
        public string Category1 { get; set; }
        
        /// <summary>
        /// The category for side 2
        /// </summary>
        public string Category2 { get; set; }

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

            // Initializes the categories array
            InitCategories();


            // DEVELOPMENT ONLY

            SelectedFilePath = "D:/Unreal/SwipeFlash/Source/Resources/Test_SpanishToEnglish.txt";
            FamilyName = "Test family";
            Category1 = "test1";
            Category2 = "test2";
            Side1Logo = "❤";
            Side2Logo = "🐸";
            IgnorePatternDescription = "#*";
            SeparatorsDescription = "";
            LinePatternDescription = "[1],gender?,[2]; gender=={\"m\":[1]=\"el \"+[1], \"f\":[1]=\"la \"+[1], \"m/f\":[1]=\"el \"+[1]}";

            //
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Called when the OK button is pressed
        /// </summary>
        private void OnOKPressed()
        {
            // DEVELOPMENT
            // CHECK INPUT INFORMATION
            
            // Create the family data struct with the trivial data
            var familyData = new ParsedFlashcardFamilyData()
            {
                FamilyName = FamilyName,
                Category1 = Category1,
                Category2 = Category2,
                Logo1 = Side1Logo,
                Logo2 = Side2Logo,
            };

            // Parses the file to a data struct
            var parsingSuccessful = FileParser.ParseFile(ref familyData,
                                                         SelectedFilePath,
                                                         IgnorePatternDescription,
                                                         SeparatorsDescription,
                                                         LinePatternDescription);

            // If the flashcards could be successfully parsed
            if (parsingSuccessful)
                // Add the flashcard family to the JSON
                JSONWriter.AddFamilyToJSON(familyData);

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

        #region Private Helpers

        private void InitCategories()
        {
            // Initializes the array
            Categories = new List<string>();

            // Gets the static data from the Flashcard Manager
            var staticData = IoC.Get<FlashcardManager>().StaticData;

            if (staticData == null)
                return;

            // Gets the categories from the JSON file
            var jsonCategories = staticData["categories"].AsJEnumerable();

            // Add each category to the array
            foreach(var category in jsonCategories) { Categories.Add((string)category["name"]); }
        }

        #endregion
    }
}