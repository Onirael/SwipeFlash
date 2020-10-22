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
                familyData.CardCount = foundFamily["cards"].AsJEnumerable().Count();

                // Gets the category names
                var familyCategory1 = (string)foundFamily["category1"];
                var familyCategory2 = (string)foundFamily["category2"];

                // Finds the categories in the JSON
                var foundCategory1 = FindCategory(familyCategory1);
                var foundCategory2 = FindCategory(familyCategory2);

                // Creates the category data containers
                familyData.Category1 = new CategoryData(familyCategory1, (string)foundCategory1?["icon"]);
                familyData.Category2 = new CategoryData(familyCategory2, (string)foundCategory2?["icon"]);

                // Initializes the article lists
                var articles1 = new List<string>();
                var articles2 = new List<string>();

                // Gets the articles enumerables
                var foundArticles1 = foundCategory1?["articles"].AsJEnumerable();
                var foundArticles2 = foundCategory2?["articles"].AsJEnumerable();

                // Adds the articles from the enumerables to the lists
                foreach (var article in foundArticles1) { articles1.Add((string)article); }
                foreach (var article in foundArticles2) { articles2.Add((string)article); }

                // Sets the category articles
                familyData.Category1.Articles = articles1;
                familyData.Category2.Articles = articles2;

                // Sets the HasIllustrations flag
                familyData.HasIllustrations = (bool)foundFamily["hasIllustrations"];
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
        /// Edits a category, if the category doesn't exist, creates it
        /// </summary>
        /// <param name="categoryName">The name of the category</param>
        /// <param name="logo">The logo of the category</param>
        public void EditCategory(CategoryData newCategoryData)
        {
            // Gets the category object
            var foundCategory = FindCategory(newCategoryData.Name);

            // If the category exists
            if (foundCategory != null)
            {
                // Set the values
                foundCategory["icon"] = newCategoryData.Logo;
                foundCategory["articles"] = JArray.FromObject(newCategoryData.Articles);
            }
            else
            {
                // Creates a new category
                var newCategory = new JObject();
                newCategory["name"] = newCategoryData.Name;
                newCategory["icon"] = newCategoryData.Logo;
                newCategory["articles"] = JArray.FromObject(newCategoryData.Articles);

                // Adds it to the categories array
                (StaticData["categories"] as JArray).Add(newCategory);
            }
        }

        /// <summary>
        /// Deletes a card's data from the static and user data
        /// </summary>
        /// <param name="cardFamily">The family of the card</param>
        /// <param name="cardID">The card's ID</param>
        public void DeleteCard(string cardFamily, int cardID)
        {
            // Gets the families
            var staticFamily = FindStaticFamily(cardFamily);
            var userFamily = FindUserFamily(cardFamily);

            // Gets the card
            var staticCard = GetFlashcard(cardID, staticFamily);
            var userCard = GetFlashcard(cardID, userFamily);

            // Removes the card from its parents
            staticCard.Remove();
            userCard.Remove();

            OnStaticDataUpdated?.Invoke(this, null);

            JSONWriter.UpdateJSONFiles();
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
        /// Finds a flashcard's family given its ID and text
        /// </summary>
        /// <param name="id">The unique ID of the card withing its family</param>
        /// <param name="side1Text">The text on side 1 of the card</param>
        /// <param name="side2Text">The text on side 2 of the card</param>
        /// <returns></returns>
        public JToken FindFlashcardFamily(int id, string side1Text, string side2Text)
        {
            // Gets the families enumerable
            var families = StaticData["flashcards"].AsJEnumerable();

            // Finds a family with the corresponding card
            var foundFamily = families.FirstOrDefault(family => family["cards"].AsJEnumerable()
                                                                               .FirstOrDefault(card => (int)card["id"] == id &&
                                                                                                       (string)card["side1Text"] == side1Text &&
                                                                                                       (string)card["side2Text"] == side2Text) != null);

            return foundFamily;
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
        
        /// <summary>
        /// Removes any unused category from the static JSON data
        /// </summary>
        /// <returns>The unused categories count</returns>
        public int DeleteUnusedCategories()
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

        #endregion

        #region Private Helpers

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
                        Category1 = new CategoryData((string)family["category1"], ""),
                        Category2 = new CategoryData((string)family["category2"], ""),
                        HasIllustrations = (bool)family["hasIllustrations"],
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

                if (FlashcardID < cardFamilySize) // DEVELOPMENT // // Card families might have IDs not starting at 0
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

                    // Gets the HasIllustration flag, the flag is overriden by the family settings
                    newFlashcardData.HasIllustration = (bool)flashcard["hasIllustration"];
                    newFlashcardData.FamilyHasIllustrations = (bool)cardFamily["hasIllustrations"];

                    // Sets whether the card is reversed
                    var isCardReversed = Rand.Next(2) == 1;
                    newFlashcardData.IsInverted = isCardReversed;
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