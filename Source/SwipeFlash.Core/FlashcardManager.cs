using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SwipeFlash.Core
{
    public class FlashcardManager
    {
        #region Public Properties

        /// <summary>
        /// The maximum length of the card queue
        /// </summary>
        public int CardQueueMaxLength = 10;

        /// <summary>
        /// The queue of card data elements
        /// </summary>
        public List<FlashcardData> CardQueue;

        /// <summary>
        /// A JSON object containing the cards' static data
        /// </summary>
        JObject StaticData;

        /// <summary>
        /// A JSON object containing the cards' user data
        /// </summary>
        JObject UserData;

        // DEVELOPMENT ONLY
        public int FlashcardID;

        /// <summary>
        /// The random generator for this object
        /// </summary>
        Random Rand;

        /// <summary>
        /// Whether the queue was initialized
        /// </summary>
        public bool IsQueueInitialized = false;

        #endregion

        #region Event Handlers

        /// <summary>
        /// Fires when the card queue has been initialized
        /// </summary>
        public EventHandler OnQueueInitialized;

        #endregion

        #region Constructor

        public FlashcardManager()
        {
            // Initialize card queue list
            CardQueue = new List<FlashcardData>();

            // Initializes the random generator
            Rand = new Random();

            // Initializes the JSON data
            InitJSONDataAsync();
        }

        #endregion

        #region Private Helpers
        
        /// <summary>
        /// Asynchronously initializes the JSON objects
        /// </summary>
        /// <returns></returns>
        private async void InitJSONDataAsync()
        {
            // Store the location of the files
            var parentDirectory = Directory.GetParent(
                                  Directory.GetParent(
                                  Directory.GetParent(
                                  Directory.GetCurrentDirectory()).ToString()).ToString());

            string staticDataFile = parentDirectory + "/SwipeFlash.Core/Data/StaticData.JSON";
            string userDataFile = parentDirectory + "/SwipeFlash.Core/Data/UserData.JSON";

            // Parse JSON files
            await Task.Run(() => StaticData = JObject.Parse(File.ReadAllText(staticDataFile)));
            await Task.Run(() => UserData = JObject.Parse(File.ReadAllText(userDataFile)));

            // Fill the card queue
            FillCardQueueAsync();
        }

        /// <summary>
        /// Asynchronously updates the card queue
        /// </summary>
        private async void FillCardQueueAsync()
        {
            await Task.Run(() =>
            {
                // While the card queue isn't full
                while (CardQueue.Count < CardQueueMaxLength)
                {
                    // Add card to queue
                    AddFlashcardToQueue();
                }

                // If this is the initialization of the array
                if (!IsQueueInitialized)
                {
                    OnQueueInitialized(this, null);
                    IsQueueInitialized = true;
                }
            });
        }

        /// <summary>
        /// Gets the next card in the queue
        /// </summary>
        public FlashcardData GetNext()
        {
            // If the queue is empty
            if (CardQueue.Count == 0)
                // Return an empty "End of stack" card
                return new FlashcardData() { IsEndOfStackCard = true };

            // Get first element
            FlashcardData nextCard = CardQueue[0];

            // Remove element from list
            CardQueue.RemoveAt(0);

            // Fills the card queue
            FillCardQueueAsync();

            return nextCard;
        }

        /// <summary>
        /// Adds a flashcard to the end of the queue
        /// </summary>
        private void AddFlashcardToQueue()
        {
            // Creates a new flashcard data object
            FlashcardData newFlashcardData = new FlashcardData();

            // Get card from JSON
            if (StaticData != null && UserData != null)
            {
                // Get the card family and card family flashcard count
                var cardFamily = StaticData["flashcards"][0];
                var cardFamilySize = cardFamily["cards"].Count();

                if (FlashcardID < cardFamilySize)
                {
                    // Gets the card family's categories
                    var side1Category = (string)cardFamily["category1"];
                    var side2Category = (string)cardFamily["category2"];

                    // Gets the flashcard from the ID
                    var flashcard = cardFamily["cards"][FlashcardID];

                    // Gets the texts
                    newFlashcardData.Side1Text = (string)flashcard["side1Text"];
                    newFlashcardData.Side2Text = (string)flashcard["side2Text"];

                    // Gets the icons from the categories
                    newFlashcardData.Side1Icon = (string)StaticData["categories"].Where(result => (string)result["name"] == side1Category).First()["icon"];
                    newFlashcardData.Side2Icon = (string)StaticData["categories"].Where(result => (string)result["name"] == side2Category).First()["icon"];

                    // Gets the HasIllustration flag
                    newFlashcardData.HasIllustration = (bool)flashcard["hasIllustration"];

                    // Sets whether the card is reversed
                    var isCardReversed = Rand.Next(2) == 1;
                    newFlashcardData.IsInverted = false;
                }
                else
                {
                    // If the card exceeds the array, return an end of stack card
                    newFlashcardData.IsEndOfStackCard = true;
                    FlashcardID = -1;
                }

                // Adds card to array
                CardQueue.Add(newFlashcardData);

                // DEVELOPMENT ONLY
                FlashcardID++;
            }
        }

        #endregion
    }
}
