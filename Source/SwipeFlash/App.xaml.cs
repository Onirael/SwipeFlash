using System.Windows;
using SwipeFlash.Core;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private WindowManager mWindowManager;

        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup Ioc
            IoC.Setup();

            mWindowManager = new WindowManager();

            // Show the main window
            //Current.MainWindow = new MainWindow();
            Current.MainWindow = new FlashcardManagerWindow();
            Current.MainWindow.Show();
        }
    }
}
