using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        /// Whether a vaild file was selected
        /// </summary>
        public bool IsFileSelected => !string.IsNullOrEmpty(SelectedFilePath);

        /// <summary>
        /// The type of the selected file
        /// </summary>
        public string SelectedFileType => Path.GetExtension(SelectedFilePath);

        /// <summary>
        /// Whether the selected file is a TXT file
        /// </summary>
        public bool IsFileTXT => SelectedFileType == ".txt";

        /// <summary>
        /// Whether the selected file is a SFF file
        /// </summary>
        public bool IsFileSFF => SelectedFileType == ".sff";

        private JObject _sFFData = null;
        /// <summary>
        /// The data contained in an imported SFF file
        /// </summary>
        public JObject SFFData
        {
            get => _sFFData;
            set
            {
                _sFFData = value;
                OnSFFDataUpdated();
            }
        }

        /// <summary>
        /// Whether the family data input can be modified
        /// </summary>
        public bool IsFamilyDataInputEnabled => !IsFileSFF;

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

        private bool _isLogo1InputEnabled = true;
        /// <summary>
        /// Whether the logo 1 text input is enabled
        /// </summary>
        public bool IsLogo1InputEnabled
        {
            get => _isLogo1InputEnabled && IsFamilyDataInputEnabled;
            set => _isLogo1InputEnabled = value;
        }

        /// <summary>
        /// The emoji logo for side 2
        /// </summary>
        public string Side2Logo { get; set; } = "";

        private bool _isLogo2InputEnabled = true;
        /// <summary>
        /// Whether the logo 2 text input is enabled
        /// </summary>
        public bool IsLogo2InputEnabled
        {
            get => _isLogo2InputEnabled && IsFamilyDataInputEnabled;
            set => _isLogo2InputEnabled = value;
        }

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
        public string LinePatternDescription { get; set; } = "[1],[2];";

        /// <summary>
        /// The existing categories
        /// </summary>
        public List<string> Categories { get; set; }

        private string _category1;
        /// <summary>
        /// The category for side 1
        /// </summary>
        public string Category1
        {
            get => _category1;
            set
            {
                _category1 = value;
                OnCategory1Changed();
            }
        }

        private string _category2;

        /// <summary>
        /// The category for side 2
        /// </summary>
        public string Category2
        {
            get => _category2;
            set
            {
                _category2 = value;
                OnCategory2Changed();
            }
        }

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
            // If no file was selected
            if (!IsFileSelected)
            {
                IoC.Get<WindowService>().CreateWindow(new WindowArgs()
                {
                    Message = "No file was selected",
                    TargetType = WindowType.Warning,
                });

                return;
            }

            switch (SelectedFileType)
            {
                case ".txt":
                    {
                        // Creates a family data struct
                        var baseFamilyData = new FlashcardFamilyData()
                        {
                            Name = FamilyName,
                            Category1 = Category1,
                            Category2 = Category2,
                            Logo1 = Side1Logo,
                            Logo2 = Side2Logo,
                        };

                        // Gets the validity of the family data
                        var isFamilyDataValid = DataChecker.IsFamilyDataValid(baseFamilyData);

                        // If it isn't valid, quit
                        if (!isFamilyDataValid)
                        {
                            IoC.Get<WindowService>().CreateWarning("Invalid family data entered");
                            return;
                        }

                        // Create the family data struct with the trivial data
                        var familyData = new ParsedFlashcardFamilyData(baseFamilyData);

                        // Parses the file to a data struct
                        var parsingSuccessful = FileParser.ParseFile(ref familyData,
                                                                     SelectedFilePath,
                                                                     IgnorePatternDescription,
                                                                     SeparatorsDescription,
                                                                     LinePatternDescription);

                        // If the flashcards could be successfully parsed
                        if (parsingSuccessful)
                        {
                            // Add the flashcard family to the JSON
                            JSONWriter.AddFamilyToJSON(familyData);
                            // Continue to close the window
                            break;
                        }
                        // Otherwise, quit
                        else
                            return;
                    }

                case ".sff":
                    {
                        // Attempt to add the family to the data
                        bool couldAddFamily = JSONPacketManager.AddFamily(SFFData);

                        // If the family could be added to the data
                        if (couldAddFamily)
                            // Continue to close the window
                            break;
                        // Otherwise, quit
                        else
                            return;
                    }

                default:
                    {
                        IoC.Get<WindowService>().CreateWindow(new WindowArgs()
                        {
                            Message = "This type of file is not currently handled",
                            TargetType = WindowType.Warning,
                        });

                        return;
                    }
            }

            // Close the window
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.AddFlashcards });
        }

        /// <summary>
        /// Called when the Cancel button is pressed
        /// </summary>
        private void OnCancelPressed()
        {
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.AddFlashcards });
        }

        /// <summary>
        /// Called when the Choose file button is pressed
        /// </summary>
        private void OnSelectFilePressed()
        {
            // Gets the application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            ListenerDelegate listener = OnFileSelected;
            appVM.ListenForEvent<string>(WindowReturnEvent.FileSelected, listener);

            // Creates the window
            IoC.Get<WindowService>().CreateWindow(new WindowArgs() { TargetType = WindowType.OpenFileExplorer });
        }

        /// <summary>
        /// Called when the user has chosen a file
        /// </summary>
        /// <param name="fileName"></param>
        private void OnFileSelected(object fileName)
        {
            // Sets the path of the selected file
            SelectedFilePath = (string)fileName;

            // If the file is a SFF file
            if (IsFileSFF)
            {
                Task.Run(() =>
                {
                    try
                    {
                        // Creates the SFF data from the file
                        SFFData = JObject.Parse(File.ReadAllText(SelectedFilePath));
                    }
                    catch (Exception) { SFFData = null; }
                });
            }
            else
            {
                // Resets the SFF data object
                SFFData = null;
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the category names from the JSON data
        /// </summary>
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
            foreach (var category in jsonCategories) { Categories.Add((string)category["name"]); }
        }

        /// <summary>
        /// Called when the category 1 input has changed
        /// </summary>
        private void OnCategory1Changed()
        {
            if (Categories.Contains(Category1))
            {
                var logo = GetCategoryLogo(Category1);

                if (logo != null)
                {
                    // Sets the logo
                    Side1Logo = logo;

                    // Disables logo input
                    IsLogo1InputEnabled = false;
                }
            }
            else
            {
                // Enables logo input
                IsLogo1InputEnabled = true;
            }
        }

        /// <summary>
        /// Called when the category 2 input has changed
        /// </summary>
        private void OnCategory2Changed()
        {
            if (Categories.Contains(Category2))
            {
                var logo = GetCategoryLogo(Category2);

                if (logo != null)
                {
                    // Sets the logo
                    Side2Logo = logo;

                    // Disables logo input
                    IsLogo2InputEnabled = false;
                }
            }
            else
            {
                // Enables logo input
                IsLogo2InputEnabled = true;
            }
        }

        /// <summary>
        /// Gets the emoji logo of a category
        /// </summary>
        /// <param name="categoryName">The name of the category</param>
        /// <returns>The emoji logo string</returns>
        private string GetCategoryLogo(string categoryName)
        {
            // Gets the static data from the Flashcard Manager
            var fm = IoC.Get<FlashcardManager>();
            var staticData = fm.StaticData;
            
            // Gets the category JToken
            var foundCategory = fm.FindCategory(categoryName);

            // If a category was found
            if (foundCategory == null)
                return null;

            // Returns the category logo
            return (string)foundCategory["icon"];
        }

        /// <summary>
        /// Called when SFF data has been received
        /// </summary>
        private void OnSFFDataUpdated()
        {
            if (SFFData != null)
            {
                // Gets the family data
                var familyData = JSONPacketManager.GetFamilyPacketData(SFFData);

                // Saves the variables
                FamilyName = familyData.Name;
                Category1 = familyData.Category1;
                Category2 = familyData.Category2;
                Side1Logo = familyData.Logo1;
                Side2Logo = familyData.Logo2;
            }
        }

        #endregion
    }
}