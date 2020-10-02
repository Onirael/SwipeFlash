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

        /// <summary>
        /// The random generator for this object
        /// </summary>
        Random Rand;

        /// <summary>
        /// Whether the queue was initialized
        /// </summary>
        public bool IsQueueInitialized { get; set; } = false;

        /// <summary>
        /// A collection containing the existing card families
        /// </summary>
        public List<FlashcardFamilyData> FlashcardFamilies { get; set; }

        // DEVELOPMENT ONLY
        public int FlashcardID;

        #endregion

        #region Event Handlers

        /// <summary>
        /// Fires when the JSON data has been parsed
        /// </summary>
        private EventHandler OnJSONLoaded;

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

            FlashcardFamilies = new List<FlashcardFamilyData>();

            // Finds flashcard families as soon as the JSON has been parsed
            OnJSONLoaded += FindFlashcardFamilies;
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Asynchronously initializes the JSON objects
        /// </summary>
        /// <returns></returns>
        public void InitJSONData()
        {
            // Gets the application view model
            var appVM = IoC.Get<ApplicationViewModel>();

            // Gets the JSON data paths
            string staticDataFile = appVM.StaticDataPath;
            string userDataFile = appVM.UserDataPath;

            // Asynchronously parse JSON files
            Task.Run(() =>
            {
                try
                {
                    // Parse JSON files
                    StaticData = JObject.Parse(File.ReadAllText(staticDataFile));
                    UserData = JObject.Parse(File.ReadAllText(userDataFile));

                    // Fire JSON loaded event
                    OnJSONLoaded(this, null);
                }
                catch { }

            });

            // Fill the card queue
            FillCardQueue();
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
            FillCardQueue();

            return nextCard;
        }

        /// <summary>
        /// Deletes a card family
        /// </summary>
        /// <param name="familyName">The display name of the family</param>
        public void DeleteFamily(string familyName)
        {
            // Gets the corresponding family
            var foundFamily = FlashcardFamilies.Where(family => family.Name == familyName).First();

            // Removes it from the flashcard families
            FlashcardFamilies.Remove(foundFamily);

            // Static Data

            // Gets the flashcards enumerable
            var jsonFamiliesStatic = StaticData["flashcards"].AsJEnumerable();

            // Find the family in the JSON
            var jsonFoundFamilyStatic = jsonFamiliesStatic.Where(result => result["displayName"].ToString() == familyName).First();

            if (jsonFoundFamilyStatic != null)
                // Removes the family
                jsonFoundFamilyStatic.Remove();

            // User Data

            // Gets the flashcards enumerable
            var jsonFamiliesUser = StaticData["flashcards"].AsJEnumerable();

            // Find the family in the JSON
            var jsonFoundFamilyUser = jsonFamiliesStatic.Where(result => result["displayName"].ToString() == familyName).First();

            if (jsonFoundFamilyUser != null)
                // Removes the family
                jsonFoundFamilyUser.Remove();
        }

        /// <summary>
        /// Sets whether the family is enabled or not
        /// </summary>
        /// <param name="familyName">The display name of the family</param>
        /// <param name="isEnabled">Whether the family is now set to enabled</param>
        public void SetFamilyEnabled(string familyName, bool isEnabled)
        {
            // Gets the corresponding family
            var foundFamily = FlashcardFamilies.Where(family => family.Name == familyName).First();

            // Sets the new enabled status of the family
            foundFamily.IsEnabled = isEnabled;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Finds all the flashcard families and stores them in the collection
        /// </summary>
        private void FindFlashcardFamilies(object sender, EventArgs e)
        {
            // If the data is valid
            if (StaticData != null && UserData != null)
            {
                // Get the families enumerable
                var familiesEnumerable = StaticData["flashcards"].AsJEnumerable();

                // For each family
                foreach(var family in familiesEnumerable)
                {
                    // Create a family data struct
                    var familyData = new FlashcardFamilyData()
                    {
                        Name = (string)family["displayName"],
                        CardCount = family["cards"].AsJEnumerable().Count(),
                        IsEnabled = (bool)UserData["flashcards"].Where(result => (string)result["family"] == (string)family["family"]).First()["isEnabled"],
                    };

                    // Store it in FlashcardFamilies
                    FlashcardFamilies.Add(familyData);
                }
            }
        }

        /// <summary>
        /// Asynchronously updates the card queue
        /// </summary>
        private void FillCardQueue()
        {
            Task.Run(() =>
            {
                // While the card queue isn't full
                while (CardQueue.Count < CardQueueMaxLength)
                    // Add card to queue
                    AddFlashcardToQueue();

                // If the array has not yet been flagged as initialized
                if (!IsQueueInitialized)
                {
                    // Set the queue initialized flag
                    IsQueueInitialized = true;

                    // Fire the OnQueueInitialized event handler
                    OnQueueInitialized(this, null);
                }
            });
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
