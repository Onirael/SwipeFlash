using SwipeFlash.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            EventManager.RegisterClassHandler(typeof(FlashcardControl), UserControl.PreviewKeyUpEvent, new KeyEventHandler(FlashcardControl_KeyDown));
        }

        private void FlashcardControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Left:
                    ((FlashcardViewModel)DataContext).SwipeLeftCommand.Execute(null);
                    break;

                case Key.Right:
                    ((FlashcardViewModel)DataContext).SwipeRightCommand.Execute(null);
                    break;

                case Key.Space:
                    ((FlashcardViewModel)DataContext).FlipCommand.Execute(null);
                    break;
            }
        }

        /// <summary>
        /// <see cref="IFlippableElement"/> interface implementation
        /// </summary>
        public float FlipAnimDuration => (float)((FlashcardViewModel)DataContext).FlipDuration;


    }
}
