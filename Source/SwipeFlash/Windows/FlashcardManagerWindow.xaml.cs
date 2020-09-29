using SwipeFlash.Core;
using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for FlashcardManagerWindow.xaml
    /// </summary>
    public partial class FlashcardManagerWindow : BaseWindow<FlashcardManagerWindowDesignModel>
    {
        public FlashcardManagerWindow()
        {
            InitializeComponent();

            BaseWindowType = WindowType.FlashcardManager;
        }
    }
}
