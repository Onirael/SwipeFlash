using SwipeFlash.Core;
using System.Windows.Controls;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for FlashcardControl.xaml
    /// </summary>
    public partial class FlashcardControl : UserControl, IFlippableElement
    {
        public FlashcardControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// <see cref="IFlippableElement"/> interface implementation
        /// </summary>
        public float FlipAnimDuration => (float)((FlashcardViewModel)DataContext).FlipDuration;
    }
}
