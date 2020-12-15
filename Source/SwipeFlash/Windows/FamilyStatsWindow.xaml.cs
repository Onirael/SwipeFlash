using SwipeFlash.Core;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for FamilyStatsWindow.xaml
    /// </summary>
    public partial class FamilyStatsWindow : BaseWindow
    {
        public FamilyStatsWindow(string familyName)
        {
            InitializeComponent();

            BaseWindowType = WindowType.FamilyStats;

            DataContext = new FamilyStatsViewModel(familyName);
        }
    }
}
