﻿using SwipeFlash.Core;
using System.Windows.Controls;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for FlashcardHost.xaml
    /// </summary>
    public partial class FlashcardHost : UserControl
    {
        public FlashcardHost()
        {
            DataContext = new FlashcardHostViewModel();

            InitializeComponent();
        }

    }
}
