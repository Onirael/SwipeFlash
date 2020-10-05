using SwipeFlash.Core;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for FlashcardManagerWindow.xaml
    /// </summary>
    public partial class AddFlashcardsWindow : BaseWindow<AddFlashcardsWindowViewModel>
    {
        public AddFlashcardsWindow()
        {
            InitializeComponent();

            BaseWindowType = WindowType.AddFlashcards;
        }
    }
}
