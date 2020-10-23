using System.Windows.Input;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A view model for the parsed flashcard list window
    /// </summary>
    public class ParsedFlashcardListWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// The items of the parsed flashcard editable list
        /// </summary>
        public AsyncObservableCollection<ParsedFlashcardListItemViewModel> ListItems { get; set; }

        /// <summary>
        /// A command for the OK button
        /// </summary>
        public ICommand OKButtonCommand;

        /// <summary>
        /// A command for the cancel button
        /// </summary>
        public ICommand CancelButtonCommand;
    }
}
