using SwipeFlash.Core;
using System.Windows.Input;

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

            // Sets the window view model reference in the application view model
            IoC.Get<ApplicationViewModel>().MainWindowVM = (WindowViewModel)DataContext;

            Keyboard.AddPreviewKeyDownHandler(this, OnPreviewKeyDown);
            //Keyboard.AddKeyDownHandler(this, OnKeyDown);

            BaseWindowType = WindowType.Main;
        }

        /// <summary>
        /// Called when a preview key has been pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Signals the application view model that a key has been pressed
            IoC.Get<ApplicationViewModel>().OnPreviewKeyDown?.Invoke(this, e.Key);
        }
    }
}
