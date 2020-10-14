using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace SwipeFlash.Core
{
    public class FamilyInfoViewModel : BaseViewModel
    {
        #region Public Properties
        
        /// <summary>
        /// The name of the family
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// The category of the family's side 1
        /// </summary>
        public string FamilyCategory1 { get; set; }

        /// <summary>
        /// The logo of the family's side 1
        /// </summary>
        public string FamilyLogo1 { get; set; }

        /// <summary>
        /// The category of the family's side 2
        /// </summary>
        public string FamilyCategory2 { get; set; }

        /// <summary>
        /// The logo of the family's side 2
        /// </summary>
        public string FamilyLogo2 { get; set; }

        /// <summary>
        /// The amount of cards in the family
        /// </summary>
        public int FamilyCardCount { get; private set; }

        public string CardCountDisplayText => $"Contains {FamilyCardCount} cards";

        /// <summary>
        /// The existing categories
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// The data of this family before edit
        /// </summary>
        public FlashcardFamilyData DefaultData { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command triggered by the export family button
        /// </summary>
        public ICommand ExportFamilyCommand { get; set; }

        /// <summary>
        /// The command triggered by the OK button
        /// </summary>
        public ICommand OKButtonCommand { get; set; }

        /// <summary>
        /// The command triggered by the cancel button
        /// </summary>
        public ICommand CancelButtonCommand { get; set; }

        #endregion

        #region Constructor

        public FamilyInfoViewModel(string familyName)
        {
            // Gets the family data from the JSON
            var familyData = IoC.Get<FlashcardManager>().GetFamilyData(familyName);

            DefaultData = familyData;

            // Sets the variables
            FamilyName = familyData.Name;
            FamilyCategory1 = familyData.Category1;
            FamilyCategory2 = familyData.Category2;
            FamilyLogo1 = familyData.Logo1;
            FamilyLogo2 = familyData.Logo2;
            FamilyCardCount = familyData.CardCount;

            // Initializes the Categories list
            InitCategories();

            // Initializes the export family command
            ExportFamilyCommand = new RelayCommand(OnExportFamilyPressed);

            // Initializes the OK button command
            OKButtonCommand = new RelayCommand(OnOKPressed);

            // Initializes the Cancel button command
            CancelButtonCommand = new RelayCommand(OnCancelPressed);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the categories from the JSON data
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
        /// Called when the user pressed the Export family... button
        /// </summary>
        private void OnExportFamilyPressed()
        {
            // Creates the window args
            var saveFileWindowArgs = new WindowArgs()
            {
                TargetType = WindowType.SaveFileExplorer,
            };

            // Creates the window
            IoC.Get<WindowService>().CreateWindow(saveFileWindowArgs);
        }
        
        /// <summary>
        /// Called when the cancel button is pressed
        /// </summary>
        private void OnCancelPressed()
        {
            // Destroys the window
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.FamilyInfo });
        }

        /// <summary>
        /// Called when the OK button is pressed
        /// </summary>
        private void OnOKPressed()
        {
            var newFamilyData = new FlashcardFamilyData()
            {
                Name = FamilyName,
                Logo1 = FamilyLogo1,
                Logo2 = FamilyLogo2,
                Category1 = FamilyCategory1,
                Category2 = FamilyCategory2,
            };

            bool couldEditData = IoC.Get<FlashcardManager>().EditFamilyData(newFamilyData, DefaultData);

            if (!couldEditData)
            {
                IoC.Get<WindowService>().CreateWindow(new WindowArgs()
                {
                    TargetType = WindowType.Warning,
                    Message = "Invalid family data was entered",
                });

                return;
            }

            // Destroys the window
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.FamilyInfo });
        }

        #endregion
    }
}