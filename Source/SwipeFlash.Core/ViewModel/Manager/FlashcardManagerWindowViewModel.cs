﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SwipeFlash.Core
{
    public class FlashcardManagerWindowViewModel : BaseViewModel
    {
        #region Public Properties
        
        /// <summary>
        /// The flashcard families to display in the scroll viewer
        /// </summary>
        public AsyncObservableCollection<FlashcardFamilyListItemViewModel> FlashcardFamilies { get; set;}

        #endregion

        #region Commands

        /// <summary>
        /// A command for the OK button
        /// </summary>
        public ICommand PressOKCommand { get; set; }

        /// <summary>
        /// A command for the Add flashcards button
        /// </summary>
        public ICommand AddFlashcardsCommand { get; set; }

        #endregion

        #region Constructor

        public FlashcardManagerWindowViewModel()
        {
            // Initializes the flashcard families
            FlashcardFamilies = new AsyncObservableCollection<FlashcardFamilyListItemViewModel>();

            // Fills the FlashcardFamilies array
            GetFlashcardFamilies(this, null);

            // Initializes the Add flashcards button command
            AddFlashcardsCommand = new RelayCommand(OnAddFlashcardsPressed);

            // Initializes the OK button command
            PressOKCommand = new RelayCommand(OnOKPressed);

            // Hook the fetching of families to the data updated event
            IoC.Get<FlashcardManager>().OnFamiliesUpdated += GetFlashcardFamilies;
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Called when the OK button is pressed
        /// </summary>
        private void OnOKPressed()
        {
            // Sends the message to the window service to destroy the window
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.FlashcardManager });
        }

        /// <summary>
        /// Called when the Add flashcards button is pressed
        /// </summary>
        private void OnAddFlashcardsPressed()
        {
            // Creates the add flashcards window from the window service
            IoC.Get<WindowService>().CreateWindow(new WindowArgs() { TargetType = WindowType.AddFlashcards });
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the flashcard families from the flashcard manager, 
        /// called on startup and when a data update has been issued by the Flashcard manager
        /// </summary>
        private void GetFlashcardFamilies(object sender, EventArgs e)
        {
            // Empties the families collection
            FlashcardFamilies.Clear();

            // Get the families from the flashcard manager
            var familiesData = IoC.Get<FlashcardManager>().FlashcardFamilies;

            // For each family data struct, create a family view model
            foreach (var familyData in familiesData) { FlashcardFamilies.Add(new FlashcardFamilyListItemViewModel(familyData)); }
        }
        
        #endregion
    }
}
