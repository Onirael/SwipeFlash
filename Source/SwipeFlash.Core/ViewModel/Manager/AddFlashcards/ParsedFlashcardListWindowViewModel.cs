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

        private ParsedFlashcardFamilyData _parsedFamily;
        /// <summary>
        /// The parsed flashcard family data
        /// </summary>
        public ParsedFlashcardFamilyData ParsedFamily
        {
            get => _parsedFamily;
            protected set => _parsedFamily = value;
        }

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

        /// <summary>
        /// Called when the cancel button has been pressed
        /// </summary>
        private void OnCancelPressed()
        {
            // Destroy the windows
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.ParsedFlashcards });
        }

        /// <summary>
        /// Called when the OK button is pressed
        /// </summary>
        private void OnOKPressed()
        {
            // Casts the list items to a collection
            var itemsList = (ListItems as Collection<ParsedFlashcardListItemViewModel>).ToList();

            var editedFlashcards = new List<ParsedFlashcardData>(itemsList.Count);

            // For each list item
            itemsList.ForEach((item) =>
            {
                // If the item is empty, quit
                if (string.IsNullOrEmpty(item.Side1Text) ||
                    string.IsNullOrEmpty(item.Side2Text))
                    return;

                // Adds a new flashcard to the edited list
                editedFlashcards.Add(new ParsedFlashcardData()
                {
                    Side1Text = item.Side1Text,
                    Side2Text = item.Side2Text,
                });
            });

            // Replaces the flashcard list
            _parsedFamily.Flashcards = editedFlashcards;

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
        public bool IsRowEdited => (Side1Text != DefaultSide1Text || Side2Text != DefaultSide2Text) && !(ListID < 0);

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

        /// <summary>
        /// Overriden constructor
        /// </summary>
        /// <param name="side1Text">The text on side 1 of the card</param>
        /// <param name="side2Text">The text on side 2 of the card</param>
        /// <param name="id">The ID of the card</param>
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

        /// <summary>
        /// Default constructor
        /// </summary>
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
