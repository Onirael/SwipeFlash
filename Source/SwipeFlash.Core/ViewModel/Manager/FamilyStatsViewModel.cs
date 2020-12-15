using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SwipeFlash.Core
{
    public class FamilyStatsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The display name of the family
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// The amount of cards in category 1
        /// </summary>
        public int Category1Count { get; set; } = 0;
        /// <summary>
        /// The amount of cards in category 2
        /// </summary>
        public int Category2Count { get; set; } = 0;
        /// <summary>
        /// The amount of cards in category 3
        /// </summary>
        public int Category3Count { get; set; } = 0;
        /// <summary>
        /// The amount of cards in category 4
        /// </summary>
        public int Category4Count { get; set; } = 0;
        /// <summary>
        /// The amount of cards in category 5
        /// </summary>
        public int Category5Count { get; set; } = 0;

        /// <summary>
        /// The total cards in the family
        /// </summary>
        public int MaxCategoryCount => (new List<int>(){
                                                Category1Count,
                                                Category2Count,
                                                Category3Count,
                                                Category4Count,
                                                Category5Count }).Max();

        /// <summary>
        /// The size of the category 1 chart bar, between 0 and 1
        /// </summary>
        public double Category1BarSize => (double)Category1Count / (double)MaxCategoryCount;
        /// <summary>
        /// The size of the category 2 chart bar, between 0 and 1
        /// </summary>
        public double Category2BarSize => (double)Category2Count / (double)MaxCategoryCount;
        /// <summary>
        /// The size of the category 3 chart bar, between 0 and 1
        /// </summary>
        public double Category3BarSize => (double)Category3Count / (double)MaxCategoryCount;
        /// <summary>
        /// The size of the category 4 chart bar, between 0 and 1
        /// </summary>
        public double Category4BarSize => (double)Category4Count / (double)MaxCategoryCount;
        /// <summary>
        /// The size of the category 5 chart bar, between 0 and 1
        /// </summary>
        public double Category5BarSize => (double)Category5Count / (double)MaxCategoryCount;

        private DateTimeOffset _lastStudied;
        /// <summary>
        /// The date time when the family was last studied
        /// </summary>
        public DateTimeOffset LastStudied
        {
            get => _lastStudied;
            set
            {
                // Update the display string
                LastStudiedDisplayString = GetLastStudiedString(value);
                _lastStudied = value;
            }
        }

        public string LastStudiedDisplayString { get; private set; }
        
        #endregion

        #region Commands

        /// <summary>
        /// The command triggered by the OK button
        /// </summary>
        public ICommand CloseButtonCommand { get; set; }

        #endregion

        #region Constructor

        public FamilyStatsViewModel(string familyName)
        {
            // Sets the family name
            FamilyName = familyName;

            // Initializes the Cancel button command
            CloseButtonCommand = new RelayCommand(OnCloseButtonPressed);

            // Async updates the family data
            Task.Run(() => { UpdateFamilyData(); });
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Called when the cancel button is pressed
        /// </summary>
        private void OnCloseButtonPressed()
        {
            // Destroys the window
            IoC.Get<WindowService>().DestroyWindow(new WindowArgs() { TargetType = WindowType.FamilyStats });
        }

        #endregion

        #region Private Helpers
        
        /// <summary>
        /// Gets and updates the family data to show on the window
        /// </summary>
        private void UpdateFamilyData()
        {
            // Gets the flashcard manager
            var fm = IoC.Get<FlashcardManager>();

            // Finds the user family
            var familyToken = fm.FindUserFamily(FamilyName);
            // Gets the flashcards
            var flashcards = familyToken["cards"] as JArray;

            // Sets last studied to the minimum value
            var lastStudied = DateTimeOffset.MinValue;
            // Resets all category counts
            Category1Count = Category2Count = Category3Count = Category4Count = Category5Count = 0;
            // For each card
            foreach (var card in flashcards)
            {
                // Gets last seen value
                var lastSeen = (DateTimeOffset)card["lastSeen"];

                // Sets last studied to the min value
                if (lastStudied.CompareTo(lastSeen) < 0)
                    lastStudied = lastSeen;

                // Increments the corresponding category count
                switch((int)card["section"])
                {
                    case 0:
                        Category1Count++;
                        break;
                    case 1:
                        Category1Count++;
                        break;
                    case 2:
                        Category2Count++;
                        break;
                    case 3:
                        Category3Count++;
                        break;
                    case 4:
                        Category4Count++;
                        break;
                    case 5:
                        Category5Count++;
                        break;
                }
            }

            // Update the last studied value
            LastStudied = lastStudied;
        }

        /// <summary>
        /// Gets the value of the last studied display string based on a given date time
        /// </summary>
        /// <param name="value">The date time when the list was last studied</param>
        /// <returns></returns>
        private string GetLastStudiedString(DateTimeOffset lastStudied)
        {
            // Gets the time delta
            var timeSince = DateTimeOffset.UtcNow.Subtract(lastStudied);

            // Displays a different message based on the time delta
            if (timeSince.Days > 365)
            {
                return "more than a year ago";
            }
            else if (timeSince.Days > 60)
            {
                return (Math.Floor((double)timeSince.Days/30)).ToString() + " months ago";
            }
            else if (timeSince.Days > 30)
            {
                return "a month ago";
            }
            else if (timeSince.Days > 1)
            {
                return timeSince.Days.ToString() + " days ago";
            }
            else if (timeSince.Days == 1)
            {
                return "a day ago";
            }
            else if (timeSince.Hours > 1)
            {
                return timeSince.Hours.ToString() + " hours ago";
            }
            else if (timeSince.Hours == 1)
            {
                return "an hour ago";
            }
            else if (timeSince.Minutes > 1)
            {
                return timeSince.Minutes.ToString() + " minutes ago";
            }
            else if (timeSince.Minutes == 1)
            {
                return "a minute ago";
            }
            else
            {
                return "just now";
            }
        }

        #endregion
    }
}