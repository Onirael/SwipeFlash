using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A static class used to write data to JSON files
    /// </summary>
    public static class JSONWriter
    {
        /// <summary>
        /// Adds a parsed family to the JSON files
        /// </summary>
        /// <param name="family">The parsed family data</param>
        public static void AddFamilyToJSON(ParsedFlashcardFamilyData family)
        {
            // Gets the JSON objects
            var staticJSON = IoC.Get<FlashcardManager>().StaticData;
            var userJSON = IoC.Get<FlashcardManager>().UserData;

            // Gets the categories array from the JObject
            var categories = staticJSON["categories"] as JArray;
            if (categories == null) return;

            // Tries to find the categories in the JSON
            var foundCategories1 = categories.Where(category => category["name"].ToString() == family.Category1);
            var foundCategories2 = categories.Where(category => category["name"].ToString() == family.Category2);
            var foundCategory1 = foundCategories1.Count() > 0 ? foundCategories1.First() : null;
            var foundCategory2 = foundCategories2.Count() > 0 ? foundCategories2.First() : null;

            // If category 1 doesn't exist
            if (foundCategory1 == null)
            {
                // Creates the category
                var category1 = new JObject();
                category1["name"] = family.Category1;
                category1["icon"] = family.Logo1;

                // Adds it to the array
                categories.Add(category1);
            }

            // If category 2 doesn't exist
            if (foundCategory2 == null)
            {
                // Creates the category
                var category2 = new JObject();
                category2["name"] = family.Category2;
                category2["icon"] = family.Logo2;

                // Adds it to the array
                categories.Add(category2);
            }

            // Gets the flashcard family in the JSON
            var staticFlashcardFamilies = staticJSON["flashcards"] as JArray;
            var userFlashcardFamilies = userJSON["flashcards"] as JArray;
            if (staticFlashcardFamilies == null || userFlashcardFamilies == null) return;

            // Tries to find the families in the JSON files
            var foundActiveStaticFamilies = staticFlashcardFamilies.Where(existingFamily => existingFamily["family"].ToString() == family.FamilyName);
            var foundActiveUserFamilies = userFlashcardFamilies.Where(existingFamily => existingFamily["family"].ToString() == family.FamilyName);
            var activeStaticFamily = foundActiveStaticFamilies.Count() > 0 ? foundActiveStaticFamilies.First() : null;
            var activeUserFamily = foundActiveUserFamilies.Count() > 0 ? foundActiveUserFamilies.First() : null;

            // This naively assumes that both the static and the user data are valid

            JArray staticCards;
            JArray userCards;

            // If the family doesn't exist
            if (activeStaticFamily == null)
            {
                // Creates the static family
                var newStaticFamily = new JObject();
                newStaticFamily["family"] = family.FamilyName;
                newStaticFamily["category1"] = family.Category1;
                newStaticFamily["category2"] = family.Category2;
                newStaticFamily["cards"] = new JArray();

                // Creates the user family
                var newUserFamily = new JObject();
                newUserFamily["family"] = family.FamilyName;
                newUserFamily["isEnabled"] = true;
                newUserFamily["cards"] = new JArray();

                // Adds the newly created family
                staticFlashcardFamilies.Add(newStaticFamily);
                userFlashcardFamilies.Add(newUserFamily);

                activeStaticFamily = newStaticFamily;
                activeUserFamily = newUserFamily;
            }

            // Set the cards array to the existing family's card array
            staticCards = activeStaticFamily["cards"] as JArray;
            userCards = activeUserFamily["cards"] as JArray;

            // Gets the first card ID
            int cardID = staticCards.Count();

            // For each flashcard
            foreach (var flashcard in family.Flashcards)
            {
                // Creates the flashcard JObject
                var newStaticCard = new JObject();
                newStaticCard["side1Text"] = flashcard.Side1Text;
                newStaticCard["side2Text"] = flashcard.Side2Text;
                newStaticCard["id"] = cardID;
                newStaticCard["hasIllustration"] = true;

                var newUserCard = new JObject();
                newUserCard["id"] = cardID;
                newUserCard["category"] = 0;
                newUserCard["lastSeen"] = DateTimeOffset.MinValue.ToString();

                // Adds the card to the array
                staticCards.Add(newStaticCard);
                userCards.Add(newUserCard);

                // Increments the ID
                cardID++;
            }

            // Update the files
            UpdateJSONFiles();
        }

        /// <summary>
        /// Updates the data files by replacing the data with the JObject data
        /// </summary>
        public static void UpdateJSONFiles()
        {
            // Gets the JSON objects
            var staticJSON = IoC.Get<FlashcardManager>().StaticData;
            var userJSON = IoC.Get<FlashcardManager>().UserData;

            // Gets the JSON files
            var staticFile = IoC.Get<ApplicationViewModel>().StaticDataPath;
            var userFile = IoC.Get<ApplicationViewModel>().UserDataPath;

            // Serializes the JSON data
            var newStaticData = JsonConvert.SerializeObject(staticJSON, Formatting.Indented);
            var newUserData = JsonConvert.SerializeObject(userJSON, Formatting.Indented);

            // Writes the data to the files
            Task.Run(() => 
            {
                File.WriteAllText(staticFile, newStaticData);
                File.WriteAllText(userFile, newUserData);
            });
        }
    }
}
