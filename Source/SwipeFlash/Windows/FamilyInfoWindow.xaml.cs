using SwipeFlash.Core;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for FamilyInfoWindow.xaml
    /// </summary>
    public partial class FamilyInfoWindow : BaseWindow
    {
        public FamilyInfoWindow(string familyName)
        {
            InitializeComponent();

            BaseWindowType = WindowType.FamilyInfo;

            DataContext = new FamilyInfoViewModel(familyName);
        }
    }
}
