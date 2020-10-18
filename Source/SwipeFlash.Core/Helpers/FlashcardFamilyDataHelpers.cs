namespace SwipeFlash.Core
{
    public static class FlashcardFamilyDataHelpers
    {
        /// <summary>
        /// Checks the validity of family input data
        /// </summary>
        /// <param name="familyData">The family data container</param>
        /// <param name="showErrors">Whether to show error message boxes</param>
        /// <returns></returns>
        public static bool IsFamilyDataValid(this FlashcardFamilyData familyData, bool showErrors = true)
        {
            // If the name string is null or empty, quit
            if (string.IsNullOrEmpty(familyData.Name))
            {
                IoC.Get<WindowService>().CreateWarning("All fields must be filled in");
                return false;
            }

            // If both categories aren't valid, quit
            if (!familyData.Category1.IsCategoryDataValid() ||
                !familyData.Category2.IsCategoryDataValid())
                return false;

            return true;
        }

        /// <summary>
        /// Modifies the family data
        /// </summary>
        /// <param name="newFamilyData">The modified data of the flashcard family</param>
        /// <param name="oldFamilyData">The base data of the flashcard family</param>
        /// <returns></returns>
        public static bool EditFamilyData(this FlashcardFamilyData newFamilyData, FlashcardFamilyData oldFamilyData)
        {
            // Gets flashcard manager
            var fm = IoC.Get<FlashcardManager>();

            // If both are the same, return true
            if (newFamilyData.Equals(oldFamilyData))
                return true;

            // Gets the static data JToken with the matching family name
            var familyStaticObject = fm.FindStaticFamily(oldFamilyData.Name);

            // Gets the user data JToken with the matching family name
            var familyUserObject = fm.FindUserFamily(oldFamilyData.Name);

            // If any of the family objects are null, quit
            if (familyStaticObject == null || familyUserObject == null)
                return false;

            // Sets the family names
            familyStaticObject["family"] = newFamilyData.Name;
            familyUserObject["family"] = newFamilyData.Name;

            // If the category or the logo has been changed
            if (!oldFamilyData.Category1.Equals(newFamilyData.Category1))
            {
                // Edits the category or creates a new one with the new data
                fm.EditCategory(newFamilyData.Category1);

                // Changes the category of the static family
                familyStaticObject["category1"] = newFamilyData.Category1.Name;
            }

            // If the category or the logo has been changed
            if (!oldFamilyData.Category2.Equals(newFamilyData.Category2))
            {
                // Edits the category or create a new one with the new data
                fm.EditCategory(newFamilyData.Category2);

                // Updates the category of the static family
                familyStaticObject["category2"] = newFamilyData.Category2.Name;
            }

            // Deletes all the unused categories
            fm.DeleteUnusedCategories();

            // Fires the data updated event
            fm.OnStaticDataUpdated?.Invoke(null, null);

            // Updates the JSON files
            JSONWriter.UpdateJSONFiles();

            return true;
        }
    }
}
