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

        #endregion

        #region Private Properties

        /// <summary>
        /// The amount by which sections are weighted when selected
        /// a scale factor of s means that the nth section has s times the weight of the n+1th
        /// </summary>
        private double SectionWeightFactor = 2;

        /// <summary>
        /// The maximal size of section 1
        /// </summary>
        private int MaxSection1Size = 15;

        /// <summary>
        /// How many swiped flashcards it takes to trigger an autosave
        /// </summary>
        private int AutoSaveFrequency = 10;

        /// <summary>
        /// Tracks how many cards the user has swiped this session, 
        /// used to trigger autosaves
        /// </summary>
        private int CardSwipeCount = 0;

        /// <summary>
        /// The amount of flashcards in enabled families
        /// </summary>
        private int ActiveFlashcardCount = 0;

        /// <summary>
        /// The maximum length of the card queue
        /// </summary>
        private int CardQueueMaxLength => Math.Min(10, ActiveFlashcardCount);
        
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

        /// <summary>
        /// Fires when a queue reset has been issued
        /// </summary>
        public EventHandler OnQueueReset;

        #endregion

        #region Constructor

        public FlashcardManager()
        {
            // Initialize card queue list
            CardQueue = new List<FlashcardData>();

            // Initializes the random generator
            Rand = new Random();

            // Initializes the flashcard families list
            FlashcardFamilies = new List<FlashcardFamilyData>();

            // Finds flashcard families as soon as the JSON has been parsed
            OnJSONLoaded += FindFlashcardFamilies;

            // Finds flashcard families whenever the static data is updated
            OnStaticDataUpdated += FindFlashcardFamilies;

            // Fills the card queue when the families
            OnFamiliesUpdated += FillCardQueue;

            // Resets the queues when the families have been updated
            OnFamiliesUpdated += ResetQueues;
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
            FillCardQueue(null, null);

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

            // Updates the active flashcards count
            ActiveFlashcardCount = FlashcardFamilies.Sum(family => family.IsEnabled ? family.CardCount : 0);

            // Updates the files
            JSONWriter.UpdateJSONFiles();

            ResetQueues(this, null);
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
                var newCategory = new JObject
                {
                    ["name"] = newCategoryData.Name,
                    ["icon"] = newCategoryData.Logo,
                    ["articles"] = JArray.FromObject(newCategoryData.Articles)
                };

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

        /// <summary>
        /// Updates the card section based on whether the card was succeeded or not
        /// </summary>
        /// <param name="flashcard">The user flashcard JSON token</param>
        /// <param name="swipedRight">Whether the user marked his/her answer as correct</param>
        /// <returns>Whether the card could be moved</returns>
        public bool UpdateCardSection(JToken flashcard, bool swipedRight, bool isReversed=false)
        {
            // The current section of the flashcard
            int currentSection = isReversed ? (int)flashcard["reversedSection"] : (int)flashcard["section"];
            
            // The section to send the card to
            int newSection = -1;
            switch (currentSection)
            {
                // Section 0
                // Success: the card jumps to section 3
                // Failure: the card goes to section 1
                case 0:
                    if (swipedRight)
                        newSection = 3;
                    else
                        newSection = 1;
                    break;
                // Section 1
                // Success: the card moves up one section
                // Failure: the card stays in section 1
                case 1:
                    if (swipedRight)
                        newSection = currentSection + 1;
                    else
                        newSection = 1;
                    break;
                // Section 2
                // Success: the card moves up one section
                // Failure: the card moves down one section
                case 2:
                    if (swipedRight)
                        newSection = currentSection + 1;
                    else
                        newSection = 1;
                    break;
                // Section 3
                // Success: the card moves up one section
                // Failure: the card moves down one section
                case 3:
                    if (swipedRight)
                        newSection = currentSection + 1;
                    else
                        newSection = 1;
                    break;
                // Section 4
                // Success: the card moves up one section
                // Failure: the card moves down one section
                case 4:
                    if (swipedRight)
                        newSection = currentSection + 1;
                    else
                        newSection = 1;
                    break;
                // Section 5
                // Success: the card stays in section 5
                // Failure: the card drops to section 2
                case 5:
                    if (swipedRight)
                        newSection = 5;
                    else
                        newSection = 1;
                    break;
                // This should never be hit
                default:
                    break;

            }

            // If the new section isn't valid
            if (newSection < 0)
                return false;

            // Sets the key of the last seen JSON value to set
            string lastSeenKey = isReversed ? "lastSeenReversed" : "lastSeen";

            // Sets the time and date when the card was seen for the last time
            flashcard[lastSeenKey] = DateTimeOffset.UtcNow;

            // Sets the new section of the flashcard
            if (currentSection == 0)
            {
                flashcard["section"] = newSection;
                flashcard["reversedSection"] = newSection;
            }
            else if (isReversed)
                flashcard["reversedSection"] = newSection;
            else
                flashcard["section"] = newSection;


            // Increments the session card swipe count
            CardSwipeCount++;

            // If an autosave point has been reached
            if (CardSwipeCount % AutoSaveFrequency == 0)
                // Update the files
                JSONWriter.UpdateJSONFiles();

            return true;
        }

        /// <summary>
        /// Clears all queues, showing a loading screen
        /// </summary>
        public void ResetQueues(object sender, EventArgs e)
        {
            // If the queue has not yet been initialized
            if (!IsQueueInitialized)
                return;

            // Sets the loading flag
            IoC.Get<ApplicationViewModel>().IsContentLoaded = false;
            // Clears the local card queue
            CardQueue.Clear();
            // Sets the queue as not initialized
            IsQueueInitialized = false;
            // Refills the local queue
            FillCardQueue(this, null);
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

            // Updates the active flashcards count
            ActiveFlashcardCount = FlashcardFamilies.Sum(family => family.IsEnabled ? family.CardCount : 0);

            // Fires the families updated event
            OnFamiliesUpdated?.Invoke(this, null);
        }

        /// <summary>
        /// Asynchronously updates the card queue
        /// </summary>
        private void FillCardQueue(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                // Update the active flashcard count
                UpdateActiveFlashcardCount();

                // While the card queue isn't full
                while (CardQueue.Count < CardQueueMaxLength)
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
        /// Updates the active flashcard count
        /// </summary>
        private void UpdateActiveFlashcardCount()
        {
            // Initializes the total count
            int totalCount = 0;

            // Iterate all families
            foreach (var family in FlashcardFamilies)
            {
                // If the family is disabled, quit
                if (!family.IsEnabled)
                    continue;

                // Gets the family token
                var familyToken = FindUserFamily(family.Name);

                // For each section
                for (int i = 0; i < 5; i++)
                {
                    // If the active flashcards found so far match or exceed the max queue size
                    if (totalCount >= CardQueueMaxLength)
                    {
                        // Set the active flashcards count to the max queue length and quit
                        ActiveFlashcardCount = CardQueueMaxLength;
                        return;
                    }

                    // If it is the first section
                    if (i <= 1)
                    {
                        // Adds the section flashcard count to the total count
                        totalCount += GetSectionFlashcards(i, familyToken).Count();
                        totalCount += GetSectionFlashcards(i, familyToken, true).Count();
                    }
                    else
                    {
                        // Adds the expired flashcard count to the total count
                        totalCount += GetExpiredFlashcards(i, family.Name).Count();
                        totalCount += GetExpiredFlashcards(i, family.Name, true).Count();
                    }
                }
            }

            ActiveFlashcardCount = totalCount;
        }

        /// <summary>
        /// Adds an empty card to the queue
        /// </summary>
        private void AddEmptyFlashcardToQueue()
        {
            CardQueue.Add(new FlashcardData()
            {
                IsEndOfStackCard = true,
            });
        }

        /// <summary>
        /// Adds a flashcard to the end of the queue
        /// </summary>
        private void AddFlashcardToQueue()
        {
            // Creates a new flashcard data object
            FlashcardData newFlashcardData = new FlashcardData();

            // Sets whether the card is reversed
            var isCardReversed = Rand.Next(2) == 1;

            // Select a flashcard family
            // Gets the families flagged as enabled
            var enabledFamilies = FlashcardFamilies.Where(family => family.IsEnabled).ToList();
            // Gets the families count
            var familiesCount = enabledFamilies.Count;
            // If there are no families selected
            if (familiesCount == 0)
            {
                // Adds end of stack card to queue and quit
                newFlashcardData.IsEndOfStackCard = true;
                CardQueue.Add(newFlashcardData);
                return;
            }

            // Selects a random family index
            var selectedFamily = Rand.Next(familiesCount);

            // Select a section
            // The amount of sections
            int sectionCount = 5;
            // Gets the maximal random value (sum of weights)
            var weightedRandMax = Math.Pow(SectionWeightFactor, sectionCount) - 1;
            // Generates a random number
            var randSelection = Rand.NextDouble();
            // Gets the selected section
            var selectedSection = (int)Math.Floor(sectionCount - Math.Log(weightedRandMax * (1 - randSelection) + 1, SectionWeightFactor)) + 1;

            // Selects a flashcard in the selected section
            var flashcardID = SelectFlashcardInSection(selectedSection, enabledFamilies[selectedFamily].Name, isReversed: isCardReversed);

            // If the flashcard is invalid, quit
            if (flashcardID < 0)
                return;

            // Whether the card is already in the queue
            bool isCardInQueue = CardQueue.ToList().Any(card => card.FamilyName == enabledFamilies[selectedFamily].Name && 
                                                       card.FlashcardID == flashcardID);

            // If the card is already in the queue, quit
            if (isCardInQueue)
                return;

            // Get card from JSON
            if (StaticData != null && UserData != null)
            {
                // Get the card family and card family flashcard count
                var cardFamily = FindStaticFamily(enabledFamilies[selectedFamily].Name);
                var cardFamilySize = cardFamily["cards"].Count();
                                
                // Sets the ID and family of the card
                newFlashcardData.FlashcardID = flashcardID;
                newFlashcardData.FamilyName = cardFamily["family"].ToString();

                // Gets the card family's categories
                var side1Category = (string)cardFamily["category1"];
                var side2Category = (string)cardFamily["category2"];

                // Gets the flashcard from the ID
                var flashcard = cardFamily["cards"].AsJEnumerable().FirstOrDefault(card => (int)card["id"] == flashcardID);

                // Gets the texts
                newFlashcardData.Side1Text = (string)flashcard["side1Text"];
                newFlashcardData.Side2Text = (string)flashcard["side2Text"];

                // Gets the icons from the categories
                newFlashcardData.Side1Icon = (string)StaticData["categories"].FirstOrDefault(result => (string)result["name"] == side1Category)["icon"];
                newFlashcardData.Side2Icon = (string)StaticData["categories"].FirstOrDefault(result => (string)result["name"] == side2Category)["icon"];

                // Gets the HasIllustration flag, the flag is overriden by the family settings
                newFlashcardData.HasIllustration = (bool)flashcard["hasIllustration"];
                newFlashcardData.FamilyHasIllustrations = (bool)cardFamily["hasIllustrations"];

                // Sets the reversed flag
                newFlashcardData.IsInverted = isCardReversed;
                
                // Adds card to array
                CardQueue.Add(newFlashcardData);
            }
        }

        /// <summary>
        /// Selects a flashcard in the given section, 
        /// if none is valid, gets it from another section
        /// </summary>
        /// <param name="selectedSection">The section to select a flashcard in</param>
        /// <param name="familyName">The name of the family</param>
        /// <param name="ignoreExpiry">Whether the flashcards should be checked for expiry</param>
        /// <param name="isReversed">Whether the card is reversed</param>
        /// <returns>The selected flashcard, -1 if no flashcard could be selected</returns>
        private int SelectFlashcardInSection(int selectedSection, string familyName, bool ignoreExpiry=false, bool isReversed=false)
        {
            switch(selectedSection)
            {
                case 0:
                    // Returns a random flashcard in this section
                    return GetRandomFlashcardInSection(selectedSection, familyName, isReversed:isReversed);
                case 1:
                    // Gets the size of the section
                    int sectionSize = GetSectionCount(familyName, selectedSection, isReversed: isReversed);
                    
                    // If the section isn't full
                    if (sectionSize < MaxSection1Size && !isReversed)
                    {
                        // Gets a flashcard in section 0
                        var flashcard = SelectFlashcardInSection(0, familyName, isReversed: isReversed);

                        // If the flashcard is valid
                        if (flashcard >= 0)
                            // Returns the flashcard
                            return flashcard;
                    }

                    // Gets a random flashcard in the section, 
                    // falling back to section 2 if none was found
                    return GetRandomFlashcardInSection(selectedSection, familyName, 2, isReversed: isReversed);
                case 2:
                    // Gets a random expired card in the section
                    return GetRandomExpiredFlashcardInSection(selectedSection, familyName, isReversed: isReversed);
                case 3:
                    // Gets a random expired card in the section
                    return GetRandomExpiredFlashcardInSection(selectedSection, familyName, isReversed: isReversed);
                case 4:
                    // Gets a random expired card in the section
                    return GetRandomExpiredFlashcardInSection(selectedSection, familyName, isReversed: isReversed);
                case 5:
                    // Gets a random expired card in the section
                    return GetRandomExpiredFlashcardInSection(selectedSection, familyName, false, isReversed: isReversed);
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Gets a random flashcard in the given selection of the given family
        /// </summary>
        /// <param name="section">The section to get the flashcard in</param>
        /// <param name="familyName">The name of the flashard's family</param>
        /// <param name="fallbackSection">The section to fallback to if no card was found, 
        /// a value of -1 will prevent the method from looking further for a match</param>
        /// <param name="isReversed">Whether the card is reversed</param>
        /// <returns>The found flashcard, -1 if none was found</returns>
        private int GetRandomFlashcardInSection(int section, string familyName, int fallbackSection=-1, bool isReversed=false)
        {
            // Gets all flashcards in the section
            var sectionFlashcards = GetSectionFlashcards(section,
                                                         FindUserFamily(familyName),
                                                         isReversed: isReversed);
            // Counts the flashcards
            var flashcardsCount = sectionFlashcards.Count();

            // If there are no flashcards in this section, quit
            if (flashcardsCount <= 0)
            {
                // If a fallback section was specified
                if (fallbackSection > 0)
                    // Select a card in that section
                    return SelectFlashcardInSection(fallbackSection, familyName, isReversed: isReversed);
                // Otherwise, quit
                else
                    return - 1;
            }

            // Gets a random flashcard in the section
            var flashcard = sectionFlashcards.ElementAtOrDefault(Rand.Next(flashcardsCount));

            // Returns the flashcard if it is valid
            return flashcard == null ? -1 : (int)flashcard["id"];
        }

        /// <summary>
        /// Gets a random expired flashcard in the given section
        /// </summary>
        /// <param name="section">The section of the card</param>
        /// <param name="familyName">The family of the card</param>
        /// <param name="isReversed">Whether the card is reversed</param>
        /// <returns>The expired flashcard's ID</returns>
        private int GetRandomExpiredFlashcardInSection(int section, string familyName, bool searchUpSections=true, bool isReversed=false)
        {
            // Gets the expired section flashcards
            var expiredFlashcards = GetExpiredFlashcards(section, familyName, isReversed: isReversed);

            // The amount of expired flashcards
            var expiredCardCount = expiredFlashcards.Count();

            // If there are expired cards
            if (expiredCardCount > 0)
            {
                // Gets a random expired flashcard
                var flashcard = expiredFlashcards.ElementAtOrDefault(Rand.Next(expiredCardCount));

                // Returns the flashcard if it is valid
                return flashcard == null ? -1 : (int)flashcard["id"];
            }
            else
            {
                if (searchUpSections)
                    // Returns a flashcard in section 1
                    return SelectFlashcardInSection(section + 1, familyName);
                else
                    return -1;
            }
        }

        /// <summary>
        /// Gets the expired flashcards of a section in a given family
        /// </summary>
        /// <param name="section">The section to get the flashcards from</param>
        /// <param name="familyName">The family of the flashcards</param>
        /// <param name="isReversed">Whether the card is reversed</param>
        /// <returns></returns>
        private JEnumerable<JToken> GetExpiredFlashcards(int section, string familyName, bool isReversed=false)
        {
            // Gets the family JToken
            var family = FindUserFamily(familyName);

            // If the found family is invalid
            if (family == null)
                // Returns an empty enumerable
                return new JEnumerable<JToken>();

            // Gets the section flashcards
            var sectionFlashcards = GetSectionFlashcards(section, family, isReversed: isReversed);

            // Gets expiry time of the flashcards in this section
            var expiryTime = FlashcardSelectionData.SectionExpiryTime[section];

            // The key to use to look for when the card was seen for the last time
            string lastSeenKey = isReversed ? "lastSeenReversed" : "lastSeen";

            // Gets the expired flashcards
            var expiredFlashcards = sectionFlashcards.Where(flashcard => DateTimeOffset.UtcNow
                                                     .Subtract((DateTimeOffset)flashcard[lastSeenKey])
                                                     .Duration() > expiryTime);
            // returns the expired flashcards
            return new JEnumerable<JToken>(expiredFlashcards);
        }

        /// <summary>
        /// Gets the size of a section within a given family
        /// </summary>
        /// <param name="familyName">The family to count in</param>
        /// <param name="selectedSection">The section to count</param>
        /// <param name="isReversed">Whether the card is reversed</param>
        /// <returns>The element count of the section</returns>
        private int GetSectionCount(string familyName, int section, bool isReversed=false)
        {
            // Gets the family JSON Token
            var foundFamily = FindUserFamily(familyName);

            // If no family was found, quit
            if (foundFamily == null)
                return -1;

            // Gets all flashcards of the section
            var sectionFlashcards = GetSectionFlashcards(section, foundFamily, isReversed: isReversed);

            // Returns the cards count
            return sectionFlashcards.Count();
        }

        /// <summary>
        /// Gets all flashcards of a given section within a given family
        /// </summary>
        /// <param name="section">The section to search for</param>
        /// <param name="family">The family to search in</param>
        /// <param name="isReversed">Whether the card is reversed</param>
        /// <returns>The flashcards of the section</returns>
        private JEnumerable<JToken> GetSectionFlashcards(int section, JToken family, bool isReversed = false)
        {
            // The key to use to look for the section of a card
            string sectionKey = isReversed ? "reversedSection" : "section";

            // Finds corresponding flahcards
            var foundFlashcards = family["cards"].AsJEnumerable()
                                                       .Where(flashcard => (int)flashcard[sectionKey] == section);

            // Returns the found flashcards
            return new JEnumerable<JToken>(foundFlashcards);
        }

        #endregion
    }
}