using SwipeFlash.Core;
using System.Windows.Controls;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for SettingsMenuControl.xaml
    /// </summary>
    public partial class SettingsMenuControl : UserControl
    {
        public SettingsMenuControl()
        {
            InitializeComponent();

            DataContext = new SettingsMenuViewModel();
        }
    }
}
