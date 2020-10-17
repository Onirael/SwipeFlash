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

        /// <summary>
        /// Fires when the static data has been modified
        /// </summary>
        public EventHandler OnStaticDataUpdated;

        /// <summary>
        /// Fires when the families List has been updated
        /// </summary>
        public EventHandler OnFamiliesUpdated;

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

            // Finds flashcard families whenever the static data is updated
            OnStaticDataUpdated += FindFlashcardFamilies;
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
            var foundFamily = FindStaticFamily(familyName);

            // If a family was found
            if (foundFamily != null)
            {
                // Gets the family data from the JSON
                familyData.Name = familyName;
                familyData.Category1 = (string)foundFamily["category1"];
                familyData.Category2 = (string)foundFamily["category2"];
                familyData.CardCount = foundFamily["cards"].AsJEnumerable().Count();

                // Finds the categories in the JSON
                var foundCategory1 = FindCategory(familyData.Category1);
                var foundCategory2 = FindCategory(familyData.Category2);

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
            // Families Collection 

            // Gets the corresponding family
            var foundFamily = FlashcardFamilies.FirstOrDefault(family => family.Name == familyName);

            // Removes it from the flashcard families
            FlashcardFamilies.Remove(foundFamily);

            // Static Data

            // Find the family in the JSON
            var staticFamily = FindStaticFamily(familyName);

            // If it exists, remove it
            if (staticFamily != null)
                staticFamily.Remove();

            // User Data

            // Gets the flashcards enumerable
            var jsonFamiliesUser = UserData["flashcards"].AsJEnumerable();

            // Find the family in the JSON
            var userFamily = FindUserFamily(familyName);

            // If it exists, remove it
            if (userFamily != null)
                userFamily.Remove();

            // Delete all the unused categories
            DeleteUnusedCategories();

            // Fires the data updated event
            OnStaticDataUpdated?.Invoke(this, null);

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
            // Find user family
            var userFamily = FindUserFamily(familyName);

            // If the family wasn't found in the data, quit
            if (userFamily == null)
                return;

            // Set JSON value
            userFamily["isEnabled"] = isEnabled;

            // Finds the index of the family
            int familyIndex = FlashcardFamilies.FindIndex(family => family.Name == familyName);

            // Copies the family
            var familyData = FlashcardFamilies[familyIndex];

            // Sets IsEnabled attributes
            familyData.IsEnabled = isEnabled;

            // Sets the collection element back
            FlashcardFamilies[familyIndex] = familyData;

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
            var familyObject = FindStaticFamily(newFlashcardData.FamilyName);

            // If the family wasn't found, quit
            if (familyObject == null)
                return false;

            // Gets the flashcard with the matching ID
            var flashcard = GetFlashcard(newFlashcardData.FlashcardID, 
                                         familyObject);

            // If the flashcard wasn't found, quit
            if (flashcard == null)
                return false;

            // Changes the text values
            flashcard["side1Text"] = newFlashcardData.Side1Text;
            flashcard["side2Text"] = newFlashcardData.Side2Text;
            flashcard["hasIllustration"] = newFlashcardData.HasIllustration;
            
            // Fires the data updated event
            OnStaticDataUpdated?.Invoke(this, null);

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
            // If both are the same, return true
            if (newFamilyData.Equals(oldFamilyData))
                return true;

            // Gets the static data JToken with the matching family name
            var familyStaticObject = FindStaticFamily(oldFamilyData.Name);

            // Gets the user data JToken with the matching family name
            var familyUserObject = FindUserFamily(oldFamilyData.Name);

            // If any of the family objects are null, quit
            if (familyStaticObject == null || familyUserObject == null)
                return false;

            // Sets the family names
            familyStaticObject["family"] = newFamilyData.Name;
            familyUserObject["family"] = newFamilyData.Name;

            // If the category or the logo has been changed
            if (oldFamilyData.Category1 != newFamilyData.Category1 || 
                oldFamilyData.Logo1 != newFamilyData.Logo1)
            {
                // Edits the category or creates a new one with the new data
                EditCategory(newFamilyData.Category1, newFamilyData.Logo1);

                // Changes the category of the static family
                familyStaticObject["category1"] = newFamilyData.Category1;
            }

            // If the category or the logo has been changed
            if (oldFamilyData.Category2 != newFamilyData.Category2 ||
                oldFamilyData.Logo2 != newFamilyData.Logo2)
            {
                // Edits the category or create a new one with the new data
                EditCategory(newFamilyData.Category2, newFamilyData.Logo2);

                // Updates the category of the static family
                familyStaticObject["category2"] = newFamilyData.Category2;
            }

            // Deletes all the unused categories
            DeleteUnusedCategories();

            // Fires the data updated event
            OnStaticDataUpdated?.Invoke(this, null);

            // Updates the JSON files
            JSONWriter.UpdateJSONFiles();

            return true;
        }

        /// <summary>
        /// Edits a category, if the category doesn't exist, creates it
        /// </summary>
        /// <param name="categoryName">The name of the category</param>
        /// <param name="logo">The logo of the category</param>
        public void EditCategory(string categoryName, string logo)
        {
            // Gets the category object
            var category2Object = FindCategory(categoryName);

            // If the category exists
            if (category2Object != null)
            {
                // Set the icon
                category2Object["icon"] = logo;
            }
            else
            {
                // Creates a new category
                var newCategory = new JObject();
                newCategory["name"] = categoryName;
                newCategory["icon"] = logo;

                // Adds it to the categories array
                (StaticData["categories"] as JArray).Add(newCategory);
            }
        }

        /// <summary>
        /// Finds a family token by name in the static data
        /// </summary>
        /// <param name="familyName">The name of the family</param>
        /// <returns>The token of the family</returns>
        public JToken FindStaticFamily(string familyName)
        {
            // Gets the family in the static data
            return StaticData["flashcards"].AsJEnumerable()
                                           .FirstOrDefault(family => (string)family["family"] == familyName);
        }

        /// <summary>
        /// Finds a family token by name in the user data
        /// </summary>
        /// <param name="familyName">The name of the family</param>
        /// <returns>The token of the family</returns>
        public JToken FindUserFamily(string familyName)
        {
            // Gets the family in the static data
            return UserData["flashcards"].AsJEnumerable()
                                         .FirstOrDefault(family => (string)family["family"] == familyName);
        }

        /// <summary>
        /// Gets a specified flashcard token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="family"></param>
        /// <returns>The token of the flashcard</returns>
        public JToken GetFlashcard(int id, JToken family)
        {
            // Gets the flashcard in the family
            return family["cards"].AsJEnumerable()
                                  .FirstOrDefault(card => (int)card["id"] == id);
        }

        /// <summary>
        /// Finds a category token by name
        /// </summary>
        /// <param name="categoryName">The name of the category</param>
        /// <returns>The token of the category</returns>
        public JToken FindCategory(string categoryName)
        {
            return StaticData["categories"].AsJEnumerable()
                                           .FirstOrDefault(category => (string)category["name"] == categoryName);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Removes any unused category from the static JSON data
        /// </summary>
        /// <returns>The unused categories count</returns>
        private int DeleteUnusedCategories()
        {
            int unusedCategoriesCount = 0;

            // Gets all categories
            var categories = StaticData["categories"].AsJEnumerable();

            // Gets all families
            var families = StaticData["flashcards"].AsJEnumerable();

            var removeCategories = new List<JToken>();
            // For each category
            foreach (var category in categories)
            {
                string categoryName = (string)category["name"];

                // If the category is used as either category1 or category 2
                var isUsed = families.Any(family => (string)family["category1"] == categoryName ||
                                                    (string)family["category2"] == categoryName);
                // If it isn't used anywhere
                if (!isUsed)
                    // Add it to the remove list
                    removeCategories.Add(category);
            }

            // Set the count of unused categories
            unusedCategoriesCount = removeCategories.Count;

            // Removes all found categories
            removeCategories.ForEach((category) => category.Remove());

            return unusedCategoriesCount;
        }

        /// <summary>
        /// Finds all the flashcard families and stores them in the collection
        /// </summary>
        private void FindFlashcardFamilies(object sender, EventArgs e)
        {
            // Empties the Flashcard families List
            FlashcardFamilies.Clear();

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
                        IsEnabled = (bool)FindUserFamily((string)family["family"])["isEnabled"],
                        Category1 = (string)family["category1"],
                        Category2 = (string)family["category2"],
                    };

                    // Store it in FlashcardFamilies
                    FlashcardFamilies.Add(familyData);
                }
            }

            OnFamiliesUpdated?.Invoke(this, null);
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