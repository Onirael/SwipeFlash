using SwipeFlash.Core;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow<WindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            BaseWindowType = WindowType.Main;
        }
    }
}
