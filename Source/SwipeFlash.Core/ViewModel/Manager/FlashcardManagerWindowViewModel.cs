using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SwipeFlash.Core
{
    public class FlashcardManagerWindowViewModel : BaseViewModel
    {
        #region Public Properties
        
        /// <summary>
        /// The flashcard families to display in the scroll viewer
        /// </summary>
        public List<FlashcardFamilyListElementViewModel> FlashcardFamilies { get; set;}

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
            // Initializes the Add flashcards button command
            AddFlashcardsCommand = new RelayCommand(OnAddFlashcardsPressed);

            // Initializes the OK button command
            PressOKCommand = new RelayCommand(OnOKPressed);
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Called when the OK button is pressed
        /// </summary>
        private void OnOKPressed()
        {
            // Sends the message to the window service to destroy the window
            IoC.Get<WindowService>().DestroyWindow(WindowType.FlashcardManager);
        }

        /// <summary>
        /// Called when the Add flashcards button is pressed
        /// </summary>
        private void OnAddFlashcardsPressed()
        {
            // Creates the add flashcards window from the window service
            IoC.Get<WindowService>().CreateWindow(WindowType.AddFlashcards);
        }

        #endregion

    }
}
