using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        /// The articles for category 1
        /// </summary>
        public string Category1Articles { get; set; }

        /// <summary>
        /// The articles for category 2
        /// </summary>
        public string Category2Articles { get; set; }

        /// <summary>
        /// Whether the illustrations are enabled for this family
        /// </summary>
        public bool AreIllustrationsEnabled { get; set; }

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
            FamilyCategory1 = familyData.Category1.Name;
            FamilyCategory2 = familyData.Category2.Name;
            FamilyLogo1 = familyData.Category1.Logo;
            FamilyLogo2 = familyData.Category2.Logo;
            Category1Articles = string.Join(", ", familyData.Category1.Articles);
            Category2Articles = string.Join(", ", familyData.Category2.Articles);
            FamilyCardCount = familyData.CardCount;
            AreIllustrationsEnabled = familyData.HasIllustrations;

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

        #region Command Methods

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
            // Format the input side 1 articles
            var articles1Str = Category1Articles.Replace(" ", "");
            var articles1List = articles1Str.Split(new char[] { ',', ';', '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Format the input side 2 articles
            var articles2Str = Category2Articles.Replace(" ", "");
            var articles2List = articles2Str.Split(new char[] { ',', ';', '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Creates the edited family data
            var newFamilyData = new FlashcardFamilyData()
            {
                Name = FamilyName,
                Category1 = new CategoryData(FamilyCategory1, FamilyLogo1, articles1List),
                Category2 = new CategoryData(FamilyCategory2, FamilyLogo2, articles2List),
                IsEnabled = DefaultData.IsEnabled,
                CardCount = DefaultData.CardCount,
                HasIllustrations = AreIllustrationsEnabled,
            };

            // Gets the validity of the family data
            var isFamilyDataValid = newFamilyData.IsFamilyDataValid();

            // If the family data isn't valid or is equal, quit
            if (!isFamilyDataValid)
                return;

            // Attempts to edit the family data
            bool couldEditData = newFamilyData.EditFamilyData(DefaultData);

            // If the edit was unsuccessful
            if (!couldEditData)
            {
                // Create a warning
                IoC.Get<WindowService>().CreateWarning("Invalid family data was entered");
                return;
            }

            // Destroys the window
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.FamilyInfo });
        }

        /// <summary>
        /// Called when the user pressed the Export family... button
        /// </summary>
        private void OnExportFamilyPressed()
        {
            var appVM = IoC.Get<ApplicationViewModel>();

            // Creates a listener delegate
            ListenerDelegate listener = OnFileSaved;

            // Listens for the feedback event
            appVM.ListenForEvent<string>(WindowReturnEvent.FileSaved, listener);

            // Creates the window args
            var saveFileWindowArgs = new WindowArgs()
            {
                TargetType = WindowType.SaveFileExplorer,
                Message = "SwipeFlash Family File (*.sff)|*.sff",
            };

            // Creates the window
            IoC.Get<WindowService>().CreateWindow(saveFileWindowArgs);
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
        /// Called when the user has specified a save file name for the exported file
        /// </summary>
        /// <param name="parameter"></param>
        private void OnFileSaved(object parameter)
        {
            // Gets the file path
            string filePath = parameter as string;
            if (string.IsNullOrEmpty(filePath))
                return;

            // Creates data packet
            var filePacket = JSONPacketManager.CreateFamilyPacket(FamilyName);

            // Converts to string
            var fileText = JsonConvert.SerializeObject(filePacket, Formatting.Indented);

            // Writes to file
            File.Create(filePath);
            Task.Run(() =>
            {
                while (!JSONWriter.IsFileReady(filePath)) { }
                File.WriteAllText(filePath, fileText);
            });
        }


        #endregion
    }
}