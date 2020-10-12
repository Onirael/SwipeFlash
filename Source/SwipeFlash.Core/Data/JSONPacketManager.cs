using Newtonsoft.Json.Linq;
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
            var families = familyPacket["flashcards"].AsJEnumerable();
            var removeFamilies = families.Where(family => family["family"].ToString() != familyName);
            // Remove them from the JSON
            foreach (var family in removeFamilies)
                family.Remove();
            
            // If there isn't a single family remaining, quit
            if (families.Count() != 1)
                return null;

            // Gets the categories from the family
            var category1 = families[0]["category1"].ToString();
            var category2 = families[0]["category2"].ToString();
            var validCategories = new string[] { category1, category2 };

            // Get all unrelated categories
            var categories = familyPacket["categories"].AsJEnumerable();
            var removeCategories = categories.Where(category => !validCategories.Contains(category["name"].ToString()));
            // Remove them from the JSON
            foreach (var category in removeCategories)
                category.Remove();

            // Returns the family JObject
            return familyPacket;
        }

        /// <summary>
        /// Adds a family to the JSON data
        /// </summary>
        /// <param name="familyPacket">The imported family packet</param>
        /// <returns>Whether the operation was successful</returns>
        public static bool AddFamily(JObject familyPacket)
        {
            // Get static data object
            var staticData = IoC.Get<FlashcardManager>().StaticData;

            var staticDataCategories = staticData["categories"].AsJEnumerable();
            var categoriesArray = staticDataCategories as JArray;
            // Gets the packet categories
            var packetCategories = familyPacket["categories"].AsJEnumerable();

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
                // Add it to the static data families
                staticDataFamilies.Add(packetFamily);

            // Update the local JSON files
            JSONWriter.UpdateJSONFiles();

            return true;
        }
    }
}
