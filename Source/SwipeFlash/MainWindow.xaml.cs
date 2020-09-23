using SwipeFlash.Core;
using System.ComponentModel;
using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new WindowViewModel();
        }
    }
}
