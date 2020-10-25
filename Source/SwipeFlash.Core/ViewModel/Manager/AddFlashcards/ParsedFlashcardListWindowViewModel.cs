using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A view model for the parsed flashcard list window
    /// </summary>
    public class ParsedFlashcardListWindowViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The items of the parsed flashcard editable list
        /// </summary>
        public AsyncObservableCollection<ParsedFlashcardListItemViewModel> ListItems { get; set; }

        /// <summary>
        /// The parsed flashcard family data
        /// </summary>
        public ParsedFlashcardFamilyData ParsedFamily { get; protected set; }

        /// <summary>
        /// The name of the side 1 category
        /// </summary>
        public string Category1Name { get; protected set; } = "";

        /// <summary>
        /// the name of the side 2 category
        /// </summary>
        public string Category2Name { get; protected set; } = "";

        #endregion

        #region Commands
        
        /// <summary>
        /// A command for the OK button
        /// </summary>
        public ICommand PressOKCommand { get; set; }

        /// <summary>
        /// A command for the cancel button
        /// </summary>
        public ICommand PressCancelCommand { get; set; }

        #endregion

        #region Constructor

        public ParsedFlashcardListWindowViewModel()
        {
            // Initializes the items collection
            ListItems = new AsyncObservableCollection<ParsedFlashcardListItemViewModel>();

            // Initializes the OK button command
            PressOKCommand = new RelayCommand(OnOKPressed);

            // Initializes the cancel button command
            PressCancelCommand = new RelayCommand(OnCancelPressed);
        }

        #endregion

        #region Command Methods

        private void OnCancelPressed()
        {
            // Destroy the windows
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.ParsedFlashcards });
        }

        private void OnOKPressed()
        {
            // Casts the list items to a collection
            var itemsCollection = (ListItems as Collection<ParsedFlashcardListItemViewModel>);

            // Initializes the list of removed flashcards
            var removedFlashcards = new List<ParsedFlashcardData>();

            // Creates a current ID variable 
            // to track whether an item has been removed
            int currentID = -1;

            // For each edited item
            foreach (var item in itemsCollection)
            {
                // Increments the ID
                currentID++;

                // Stores whether the item is new
                bool isNewItem = item.ListID < 0;

                // If the item wasn't modified, continue
                if (!item.IsRowEdited && !isNewItem)
                    continue;

                // If side 1 or side 2 is empty, continue
                if (string.IsNullOrEmpty(item.Side1Text) ||
                    string.IsNullOrEmpty(item.Side2Text))
                    continue;

                // If the item isn't new and its ID is not the current ID
                if (!isNewItem && currentID != item.ListID)
                {
                    // For each missing index
                    for (int i = 0; i < item.ListID - currentID; i++)
                    {
                        // Adds the card with the index to the remove list
                        removedFlashcards.Add(ParsedFamily.Flashcards[currentID]);
                        // Increment the ID
                        currentID++;
                    }
                }

                // If the item is from the list
                if (item.ListID > 0)
                {
                    // Edits the family data
                    ParsedFamily.Flashcards[item.ListID] = new ParsedFlashcardData()
                    {
                        Side1Text = item.Side1Text,
                        Side2Text = item.Side2Text,
                    };
                }
                // If it was added
                else
                {
                    // Adds it to the family data
                    ParsedFamily.Flashcards.Add(new ParsedFlashcardData()
                    {
                        Side1Text = item.Side1Text,
                        Side2Text = item.Side2Text,
                    });
                }
            }

            // Adds the family to the JSON
            JSONWriter.AddFamilyToJSON(ParsedFamily);

            // Destroy the windows
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.ParsedFlashcards });
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.AddFlashcards });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the parsed flashcard items list
        /// </summary>
        /// <param name="items">The parsed flashcard family data</param>
        public virtual async void InitAsync(object items)
        {
            // Type checks the items
            if (!(items is ParsedFlashcardFamilyData))
            {
                // Sends a warning to the user
                IoC.Get<WindowService>().CreateWarning("Error when loading parsed object");

                // Cancels the operation
                OnCancelPressed();
            }

            // Gets the items family data
            ParsedFamily = (ParsedFlashcardFamilyData)items;

            // Sets the category names
            Category1Name = ParsedFamily.Category1;
            Category2Name = ParsedFamily.Category2;
            
            // Asynchronously adds the flashcards to the list
            await Task.Run(() =>
            {
                // For each flashcard
                for (int i = 0; i < ParsedFamily.Flashcards.Count; i++)
                {
                    // Gets the flashcard
                    var flashcard = ParsedFamily.Flashcards[i];

                    // Adds the flashcard to the list
                    ListItems.Add(new ParsedFlashcardListItemViewModel(flashcard.Side1Text, flashcard.Side2Text, i));
                }
            });
        }

        #endregion
    }

    /// <summary>
    /// The data container for a single parsed flashcard list item
    /// </summary>
    public class ParsedFlashcardListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The text of side 1
        /// </summary>
        public string Side1Text { get; set; }

        /// <summary>
        /// The default text of side 1
        /// </summary>
        public string DefaultSide1Text { get; set; }

        /// <summary>
        /// The text of side 2
        /// </summary>
        public string Side2Text { get; set; }

        /// <summary>
        /// The default text of side 2
        /// </summary>
        public string DefaultSide2Text { get; set; }

        /// <summary>
        /// Whether the row was edited
        /// </summary>
        public bool IsRowEdited => Side1Text != DefaultSide1Text || Side2Text != DefaultSide2Text;

        /// <summary>
        /// The index of the flashcard
        /// </summary>
        public int ListID { get; set; } = -1;

        #endregion
        
        #region Commands

        /// <summary>
        /// A command for the undo button
        /// </summary>
        public ICommand UndoCommand { get; set; }

        #endregion

        #region Constructor

        public ParsedFlashcardListItemViewModel(string side1Text, string side2Text, int id)
        {
            // Initializes the texts
            DefaultSide1Text = Side1Text = side1Text;
            DefaultSide2Text = Side2Text = side2Text;

            // Sets the item ID
            ListID = id;

            // Initializes the undo command
            UndoCommand = new RelayCommand(() => { Side1Text = DefaultSide1Text;
                                                   Side2Text = DefaultSide2Text; });
        }

        public ParsedFlashcardListItemViewModel()
        {
            // Initializes the texts
            DefaultSide1Text = Side1Text = "";
            DefaultSide2Text = Side2Text = "";

            ListID = -1;
        }


        #endregion
    }
}
