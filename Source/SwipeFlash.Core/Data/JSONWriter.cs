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
            var fm = IoC.Get<FlashcardManager>();
            var staticJSON = fm.StaticData;
            var userJSON = fm.UserData;

            // Gets the categories array from the static data
            if (!(staticJSON["categories"] is JArray categories))
                return;

            // Tries to find the categories in the JSON
            var foundCategory1 = fm.FindCategory(family.Category1);
            var foundCategory2 = fm.FindCategory(family.Category2);

            // If category 1 doesn't exist
            if (foundCategory1 == null)
            {
                // Creates the category
                var category1 = new JObject
                {
                    ["name"] = family.Category1,
                    ["icon"] = family.Logo1,
                    ["articles"] = JArray.FromObject(family.Articles1)
                };

                // Adds it to the array
                categories.Add(category1);
            }

            // If category 2 doesn't exist
            if (foundCategory2 == null)
            {
                // Creates the category
                var category2 = new JObject
                {
                    ["name"] = family.Category2,
                    ["icon"] = family.Logo2,
                    ["articles"] = JArray.FromObject(family.Articles2)
                };

                // Adds it to the array
                categories.Add(category2);
            }

            // Gets the flashcard families in the JSON
            var staticFlashcardFamilies = staticJSON["flashcards"] as JArray;
            var userFlashcardFamilies = userJSON["flashcards"] as JArray;
            if (staticFlashcardFamilies == null || userFlashcardFamilies == null) return;

            // Tries to find the families in the JSON files
            var activeStaticFamily = fm.FindStaticFamily(family.FamilyName);
            var activeUserFamily = fm.FindUserFamily(family.FamilyName);

            // This naively assumes that both the static and the user data are valid

            JArray staticCards;
            JArray userCards;

            // If the family doesn't exist
            if (activeStaticFamily == null)
            {
                // Creates the static family
                var newStaticFamily = new JObject
                {
                    ["family"] = family.FamilyName,
                    ["category1"] = family.Category1,
                    ["category2"] = family.Category2,
                    ["cards"] = new JArray(),
                    ["hasIllustrations"] = true
                };

                // Creates the user family
                var newUserFamily = new JObject
                {
                    ["family"] = family.FamilyName,
                    ["isEnabled"] = true,
                    ["cards"] = new JArray()
                };

                // Adds the newly created family
                staticFlashcardFamilies.Add(newStaticFamily);
                userFlashcardFamilies.Add(newUserFamily);

                activeStaticFamily = newStaticFamily;
                activeUserFamily = newUserFamily;
            }

            // Set the cards array to the existing family's card array
            staticCards = activeStaticFamily["cards"] as JArray;
            userCards = activeUserFamily["cards"] as JArray;

            // If there are any cards in the array, 
            // set the starting ID to the maximum ID
            int cardID = 0;
            if (staticCards.Any())
                cardID = staticCards.Max(card => (int)card["id"]) + 1;

            // For each flashcard
            foreach (var flashcard in family.Flashcards)
            {
                // Whether the card already exists
                bool isDuplicate = staticCards.Any(card => (string)card["side1Text"] == flashcard.Side1Text && 
                                                           (string)card["side2Text"] == flashcard.Side2Text);
                // If it is a duplicate, skip
                if (isDuplicate)
                    continue;

                // Creates the flashcard JObject
                var newStaticCard = new JObject
                {
                    ["side1Text"] = flashcard.Side1Text,
                    ["side2Text"] = flashcard.Side2Text,
                    ["id"] = cardID,
                    ["hasIllustration"] = true
                };

                var newUserCard = new JObject
                {
                    ["id"] = cardID,
                    ["section"] = 0,
                    ["reversedSection"] = 0,
                    ["lastSeen"] = DateTimeOffset.MinValue.ToString(),
                    ["lastSeenReversed"] = DateTimeOffset.MinValue.ToString(),
                };

                // Adds the card to the array
                staticCards.Add(newStaticCard);
                userCards.Add(newUserCard);

                // Increments the ID
                cardID++;
            }

            IoC.Get<FlashcardManager>().OnStaticDataUpdated?.Invoke(null, null);

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
                // Waits for the files to be available
                while (!IsFileReady(staticFile) ||
                       !IsFileReady(userFile)) { }

                File.WriteAllText(staticFile, newStaticData);
                File.WriteAllText(userFile, newUserData);
            });
        }

        /// <summary>
        /// Creates the user data from a static family JSON token
        /// </summary>
        /// <param name="staticFamily">The static data flashcard family</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public static void CreateUserData(JToken staticFamily)
        {
            // Gets the user data
            var fm = IoC.Get<FlashcardManager>();
            var userData = IoC.Get<FlashcardManager>().UserData;

            // Tries to find the family
            var userFamily = fm.FindUserFamily((string)staticFamily["family"]);

            // Gets the static cards enumerable
            var staticCards = staticFamily["cards"].AsJEnumerable();

            // If the family exists
            if (userFamily != null)
            {
                // Gets the cards array
                var userCardsArray = userFamily["cards"] as JArray;

                // For each static data card
                foreach (var staticCard in staticCards)
                {
                    // If this ID isn't already in the user array
                    if (!userCardsArray.Any(card => (int)card["id"] == (int)staticCard["id"]))
                    {
                        // Creates a new user card
                        var newUserCard = new JObject
                        {
                            ["id"] = (int)staticCard["id"],
                            ["section"] = 0,
                            ["reversedSection"] = 0,
                            ["lastSeen"] = DateTimeOffset.MinValue.ToString(),
                            ["lastSeenReversed"] = DateTimeOffset.MinValue.ToString(),
                        };

                        // Adds it to the array
                        userCardsArray.Add(newUserCard);
                    }
                }
            }
            // Otherwise
            else
            {
                // Creates the new user family
                userFamily = new JObject();
                var userCardsArray = new JArray();
                userFamily["family"] = (string)staticFamily["family"];
                userFamily["cards"] = userCardsArray;
                userFamily["isEnabled"] = true;

                // Adds the family to the user data
                (userData["flashcards"] as JArray).Add(userFamily);

                // For each static data card
                foreach (var staticCard in staticCards)
                {
                    // Creates a new user card
                    var newUserCard = new JObject
                    {
                        ["id"] = (int)staticCard["id"],
                        ["section"] = 0,
                        ["reversedSection"] = 0,
                        ["lastSeen"] = DateTimeOffset.MinValue.ToString(),
                        ["lastSeenReversed"] = DateTimeOffset.MinValue.ToString(),
                    };

                    // Adds it to the array
                    userCardsArray.Add(newUserCard);
                }
            }
        }


        /// <summary>
        /// Whether a file is available to write on in the main thread
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsFileReady(string fileName)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (Stream stream = new FileStream(fileName, FileMode.Open))
                {
                    if (stream != null)
                    {
                        stream.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}