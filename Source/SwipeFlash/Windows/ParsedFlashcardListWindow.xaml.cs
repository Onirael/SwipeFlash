using SwipeFlash.Core;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for ParsedFlashcardListWindow.xaml
    /// </summary>
    public partial class ParsedFlashcardListWindow : BaseWindow<ParsedFlashcardListWindowViewModel>
    {
        public ParsedFlashcardListWindow(object parsedFlashcards)
        {
            // Sets the base window type
            BaseWindowType = WindowType.ParsedFlashcards;

            // Initializes the view model with the flashcards
            (DataContext as ParsedFlashcardListWindowViewModel)?.InitAsync(parsedFlashcards);

            InitializeComponent();
        }
    }
}
