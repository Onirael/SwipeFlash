using SwipeFlash.Core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for FlashcardManagerWindow.xaml
    /// </summary>
    public partial class AddFlashcardsWindow : BaseWindow<AddFlashcardsWindowViewModel>
    {
        public AddFlashcardsWindow()
        {
            InitializeComponent();

            BaseWindowType = WindowType.AddFlashcards;


        }

        //private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    // Type check the sender
        //    if (!(sender is ComboBox comboBox))
        //        return;

        //    // Type check its item source
        //    if (!(comboBox.ItemsSource is List<string> categories))
        //        return;

        //    foreach(var str in categories)
        //    {
        //        var cbItem = new ComboBoxItem();

        //        cbItem.Content = str;

        //        comboBox.Items.Add(cbItem);
        //    }
        //}
    }
}
