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
        public JObject StaticData;

        /// <summary>
        /// A JSON object containing the cards' user data
        /// </summary>
        public JObject UserData;

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

        #region Private Properties

        /// <summary>
        /// The random generator for this object
        /// </summary>
        Random Rand;

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
        /// Gets the data of a given family
        /// </summary>
        /// <param name="familyName">The name of the family</param>
        /// <returns>The family data</returns>
        public FlashcardFamilyData GetFamilyData(string familyName)
        {
            // Creates a new family data container
            var familyData = new FlashcardFamilyData();

            // Gets the family with the corresponding name
            var foundFamily = StaticData["flashcards"].AsJEnumerable()
                                                      .FirstOrDefault(family => (string)family["family"] == familyName);
            // If a family was found
            if (foundFamily != null)
            {
                // Gets the family data from the JSON
                familyData.Name = familyName;
                familyData.Category1 = (string)foundFamily["category1"];
                familyData.Category2 = (string)foundFamily["category2"];
                familyData.CardCount = foundFamily["cards"].AsJEnumerable().Count();

                // Finds the categories in the JSON
                var foundCategory1 = StaticData["categories"].AsJEnumerable()
                                                             .FirstOrDefault(category => (string)category["name"] == familyData.Category1);
                var foundCategory2 = StaticData["categories"].AsJEnumerable()
                                                             .FirstOrDefault(category => (string)category["name"] == familyData.Category2);
                // Gets the logos
                familyData.Logo1 = (string)foundCategory1?["icon"];
                familyData.Logo2 = (string)foundCategory2?["icon"];
            }

            return familyData;
        }

        /// <summary>
        /// Deletes a card family
        /// </summary>
        /// <param name="familyName">The display name of the family</param>
        public void DeleteFamily(string familyName)
        {
            // Gets the corresponding family
            var foundFamily = FlashcardFamilies.FirstOrDefault(family => family.Name == familyName);

            // Removes it from the flashcard families
            FlashcardFamilies.Remove(foundFamily);

            // Static Data

            // Gets the flashcards enumerable
            var jsonFamiliesStatic = StaticData["flashcards"].AsJEnumerable();

            // Find the family in the JSON
            var jsonFoundFamilyStatic = jsonFamiliesStatic.FirstOrDefault(result => result["family"].ToString() == familyName);

            if (jsonFoundFamilyStatic != null)
                // Removes the family
                jsonFoundFamilyStatic.Remove();

            // User Data

            // Gets the flashcards enumerable
            var jsonFamiliesUser = UserData["flashcards"].AsJEnumerable();

            // Find the family in the JSON
            var jsonFoundFamilyUser = jsonFamiliesUser.FirstOrDefault(result => result["family"].ToString() == familyName);

            if (jsonFoundFamilyUser != null)
                // Removes the family
                jsonFoundFamilyUser.Remove();

            // Updates the files
            JSONWriter.UpdateJSONFiles();
        }

        /// <summary>
        /// Sets whether the family is enabled or not
        /// </summary>
        /// <param name="familyName">The display name of the family</param>
        /// <param name="isEnabled">Whether the family is now set to enabled</param>
        public void SetFamilyEnabled(string familyName, bool isEnabled)
        {            
            // Finds the index of the family
            int familyIndex = FlashcardFamilies.FindIndex(family => family.Name == familyName);

            // Copies the family
            var familyData = FlashcardFamilies[familyIndex];

            // Sets IsEnabled attributes
            familyData.IsEnabled = isEnabled;

            // Sets the collection element
            FlashcardFamilies[familyIndex] = familyData;

            // User Data

            // Gets flashcard families enumerable
            var jsonFamilies = UserData["flashcards"].AsJEnumerable();

            // Find matching family
            var jsonFoundFamily = jsonFamilies.FirstOrDefault(family => (string)family["family"] == familyName);

            // Set JSON value
            jsonFoundFamily["isEnabled"] = isEnabled;

            // Updates the files
            JSONWriter.UpdateJSONFiles();
        }

        /// <summary>
        /// Modifies the flashcard data
        /// </summary>
        /// <param name="newFlashcardData">The edited data of the flashcard</param>
        /// <returns>True if the operation was successful</returns>
        public bool EditFlashcardData(FlashcardData newFlashcardData)
        {
            // Gets the JToken with the matching family name
            var familyObject = StaticData["flashcards"].AsJEnumerable()
                                                       .FirstOrDefault(family => family["family"]
                                                       .ToString() == newFlashcardData.FamilyName);

            // If the family wasn't found, quit
            if (familyObject == null)
                return false;

            // Gets the flashcard with the matching ID
            var flashcard = familyObject["cards"].AsJEnumerable()
                                                 .FirstOrDefault(card => card["id"]
                                                 .Value<int>() == newFlashcardData.FlashcardID);

            // If the flashcard wasn't found, quit
            if (flashcard == null)
                return false;

            // Changes the text values
            flashcard["side1Text"] = newFlashcardData.Side1Text;
            flashcard["side2Text"] = newFlashcardData.Side2Text;
            flashcard["hasIllustration"] = newFlashcardData.HasIllustration;

            // Updates the JSON files
            JSONWriter.UpdateJSONFiles();
            
            return true;
        }

        /// <summary>
        /// Modifies the family data
        /// </summary>
        /// <param name="newFamilyData">The modified data of the flashcard family</param>
        /// <param name="oldFamilyData">The base data of the flashcard family</param>
        /// <returns></returns>
        public bool EditFamilyData(FlashcardFamilyData newFamilyData, FlashcardFamilyData oldFamilyData)
        {
            // Gets the static data JToken with the matching family name
            var familyStaticObject = StaticData["flashcards"].AsJEnumerable()
                                                             .FirstOrDefault(family => family["family"]
                                                             .ToString() == oldFamilyData.Name);
            // Gets the user data JToken with the matching family name
            var familyUserObject = UserData["flashcards"].AsJEnumerable()
                                                         .FirstOrDefault(family => family["family"]
                                                         .ToString() == oldFamilyData.Name);

            familyStaticObject["family"] = newFamilyData.Name;
            familyUserObject["family"] = newFamilyData.Name;

            // If the category or the logo has been changed
            if (oldFamilyData.Category1 != newFamilyData.Category1 || 
                oldFamilyData.Logo1 != newFamilyData.Logo1)
            {
                // Gets the category object
                var category1Object = StaticData["categories"].AsJEnumerable()
                                                              .FirstOrDefault(category => category["name"]
                                                              .ToString() == newFamilyData.Category1);
                // If the category exists
                if (category1Object != null)
                {
                    // Set the icon
                    category1Object["icon"] = newFamilyData.Logo1;
                }
                else
                {
                    // Creates a new category
                    var newCategory = new JObject();
                    newCategory["name"] = newFamilyData.Category2;
                    newCategory["icon"] = newFamilyData.Logo1;

                    // Adds it to the categories array
                    (StaticData["categories"] as JArray).Add(newCategory);
                }
            }

            // If the category or the logo has been changed
            if (oldFamilyData.Category2 != newFamilyData.Category2 ||
                oldFamilyData.Logo2 != newFamilyData.Logo2)
            {
                // Gets the category object
                var category2Object = StaticData["categories"].AsJEnumerable()
                                                              .FirstOrDefault(category => category["name"]
                                                              .ToString() == newFamilyData.Category2);
                // If the category exists
                if (category2Object != null)
                {
                    // Set the icon
                    category2Object["icon"] = newFamilyData.Logo2;
                }
                else
                {
                    // Creates a new category
                    var newCategory = new JObject();
                    newCategory["name"] = newFamilyData.Category2;
                    newCategory["icon"] = newFamilyData.Logo2;

                    // Adds it to the categories array
                    (StaticData["categories"] as JArray).Add(newCategory);
                }
            }
            
            // DEVELOPMENT
            // CLEAN EMPTY CATEGORIES

            // Updates the JSON files
            JSONWriter.UpdateJSONFiles();

            return true;
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
                        Name = (string)family["family"],
                        CardCount = family["cards"].AsJEnumerable().Count(),
                        IsEnabled = (bool)UserData["flashcards"].FirstOrDefault(result => (string)result["family"] == (string)family["family"])["isEnabled"],
                        Category1 = (string)family["category1"],
                        Category2 = (string)family["category2"],
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
                    OnQueueInitialized?.Invoke(this, null);
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
                    // Sets the ID and family of the card
                    newFlashcardData.FlashcardID = FlashcardID;
                    newFlashcardData.FamilyName = cardFamily["family"].ToString();

                    // Gets the card family's categories
                    var side1Category = (string)cardFamily["category1"];
                    var side2Category = (string)cardFamily["category2"];

                    // Gets the flashcard from the ID
                    var flashcard = cardFamily["cards"][FlashcardID];

                    // Gets the texts
                    newFlashcardData.Side1Text = (string)flashcard["side1Text"];
                    newFlashcardData.Side2Text = (string)flashcard["side2Text"];

                    // Gets the icons from the categories
                    newFlashcardData.Side1Icon = (string)StaticData["categories"].FirstOrDefault(result => (string)result["name"] == side1Category)["icon"];
                    newFlashcardData.Side2Icon = (string)StaticData["categories"].FirstOrDefault(result => (string)result["name"] == side2Category)["icon"];

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
