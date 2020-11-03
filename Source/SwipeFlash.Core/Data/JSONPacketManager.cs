using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A static class used to manage JSON data packages for export
    /// </summary>
    public static class JSONPacketManager
    {
        /// <summary>
        /// Creates a <see cref="JObject"/> containing only the data related to the given family
        /// </summary>
        /// <param name="familyName">The family name</param>
        /// <returns>The created JSON object ready for export</returns>
        public static JObject CreateFamilyPacket(string familyName)
        {
            // Get static data object
            var staticData = IoC.Get<FlashcardManager>().StaticData;

            // Copies the static data
            var familyPacket = (JObject)staticData.DeepClone();

            // Get all unrelated families
            var families = familyPacket["flashcards"] as JArray;
            var removeFamilies = families.Where(family => family["family"].ToString() != familyName).ToArray();
            // Remove them from the JSON
            foreach (var family in removeFamilies)
                families.Remove(family);
            
            // If there isn't a single family remaining, quit
            if (families.Count() != 1)
                return null;

            // Gets the categories from the family
            var category1 = families[0]["category1"].ToString();
            var category2 = families[0]["category2"].ToString();
            var validCategories = new string[] { category1, category2 };

            // Get all unrelated categories
            var categories = familyPacket["categories"] as JArray;
            var removeCategories = categories.Where(category => !validCategories.Contains(category["name"].ToString())).ToArray();
            // Remove them from the JSON
            foreach (var category in removeCategories)
                categories.Remove(category);

            // Returns the family JObject
            return familyPacket;
        }

        /// <summary>
        /// Gets the <see cref="FlashcardFamilyData"/> of the packet family
        /// </summary>
        /// <param name="familyPacket"></param>
        /// <returns></returns>
        public static FlashcardFamilyData GetFamilyPacketData(JObject familyPacket)
        {
            // Creates a new family data container
            var familyData = new FlashcardFamilyData();

            // Gets the families and categories enumerables contained in the packet
            var packetFamilies = familyPacket["flashcards"].AsJEnumerable();
            var packetCategories = familyPacket["categories"].AsJEnumerable();

            // If the packet contains no families or categories, quit
            if (packetFamilies == null || packetCategories == null ||
                packetFamilies.Count() == 0 || packetCategories.Count() == 0)
                return familyData;

            // Gets the first family
            var foundFamily = packetFamilies.FirstOrDefault();
            var foundCategory1 = packetCategories.FirstOrDefault(category => (string)category["name"] == (string)foundFamily["category1"]);
            var foundCategory2 = packetCategories.FirstOrDefault(category => (string)category["name"] == (string)foundFamily["category2"]);

            // If either of the categories isn't contained in the packet, quit
            if (foundCategory1 == null || foundCategory2 == null)
                return familyData;

            // Writes the family data
            familyData.Name = (string)foundFamily["family"];

            familyData.Category1 = new CategoryData((string)foundFamily["category1"],
                                                    (string)foundCategory1["icon"]);
            familyData.Category2 = new CategoryData((string)foundFamily["category1"],
                                                    (string)foundCategory1["icon"]);


            // Gets the article enumerables
            var foundArticles1 = foundCategory1["articles"].AsJEnumerable();
            var foundArticles2 = foundCategory2["articles"].AsJEnumerable();

            // Initializes the article lists
            var articles1 = new List<string>();
            var articles2 = new List<string>();

            // Adds the enumerable articles to the lists
            foreach(var article in foundArticles1) { articles1.Add((string)article); }
            foreach(var article in foundArticles2) { articles2.Add((string)article); }
            
            // Sets the category data articles
            familyData.Category1.Articles = articles1;
            familyData.Category2.Articles = articles2;

            familyData.HasIllustrations = (bool)foundFamily["hasIllustrations"];

            return familyData;
        }

        /// <summary>
        /// Adds a family to the JSON data
        /// </summary>
        /// <param name="familyPacket">The imported family packet</param>
        /// <returns>Whether the operation was successful</returns>
        public static bool AddFamily(JObject familyPacket)
        {
            // Get static data object
            var fm = IoC.Get<FlashcardManager>();
            var staticData = fm.StaticData;

            var staticDataCategories = staticData["categories"].AsJEnumerable();
            var categoriesArray = staticDataCategories as JArray;

            // Gets the packet categories
            var packetCategories = familyPacket["categories"].AsJEnumerable();

            // Checks if the packet categories are valid
            if (packetCategories == null)
            {
                IoC.Get<WindowService>().CreateWarning("SFF file data is invalid");
                return false;
            }

            // For each category in the packet
            foreach (var category in packetCategories)
            {
                // Gets whether the category already exists
                bool categoryExists = staticDataCategories.Any(existingCategory =>
                                                               existingCategory["name"].ToString() == category["name"].ToString());

                // If the category doesn't already exist in the JSON
                if (!categoryExists)
                {
                    // Add it to the JSON
                    categoriesArray.Add(category);
                }
            }

            var staticDataFamilies = staticData["flashcards"] as JArray;
            // Gets the packet family
            var packetFamily = familyPacket["flashcards"].AsJEnumerable().FirstOrDefault();

            // If the packet family is valid
            if (packetFamily != null)
            {
                // Tries to get a family with the same name in the static data
                var staticFamily = fm.FindStaticFamily((string)packetFamily["family"]);

                // If the family already exists
                if (staticFamily != null)
                {
                    // Gets the static family cards
                    var staticFamilyCards = staticFamily["cards"] as JArray;

                    int maxCardID = staticFamilyCards.Max(card => (int)card["id"]);
                    int cardID = staticFamilyCards.Count > 0 ? maxCardID + 1 : 0;

                    // The cards contained in the packet family
                    var packetCards = packetFamily["cards"].AsJEnumerable();

                    // For each card in the packet
                    foreach (var card in packetCards)
                    {
                        // Whether the card already exists
                        bool cardExists = staticFamilyCards.Any(staticCard => (string)staticCard["side1Text"] == (string)card["side1Text"] &&
                                                                              (string)staticCard["side2Text"] == (string)card["side2Text"]);
                        // If the card already exists, skip it
                        if (cardExists)
                            continue;

                        // Sets the card ID
                        card["id"] = cardID;

                        // Increments the ID
                        cardID++;

                        // Add the card to the static cards array
                        staticFamilyCards.Add(card);
                    }
                }
                else
                {
                    // Add it to the static data families
                    staticDataFamilies.Add(packetFamily);
                }
            }
            else
            {
                IoC.Get<WindowService>().CreateWarning("SFF file data is invalid");
                return false;
            }

            // Creates the user data family from the static family
            JSONWriter.CreateUserData(packetFamily);

            // Calls the family updated event
            IoC.Get<FlashcardManager>().OnStaticDataUpdated?.Invoke(null, null);

            // Update the local JSON files
            JSONWriter.UpdateJSONFiles();

            return true;
        }
    }
}
