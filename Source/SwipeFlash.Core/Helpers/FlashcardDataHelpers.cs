namespace SwipeFlash.Core
{
    public static class FlashcardDataHelpers
    {
        /// <summary>
        /// Modifies the flashcard data
        /// </summary>
        /// <param name="newFlashcardData">The edited data of the flashcard</param>
        /// <returns>True if the operation was successful</returns>
        public static bool EditFlashcardData(this FlashcardData newFlashcardData)
        {
            var fm = IoC.Get<FlashcardManager>();

            // Gets the JToken with the matching family name
            var familyObject = fm.FindStaticFamily(newFlashcardData.FamilyName);

            // If the family wasn't found, quit
            if (familyObject == null)
                return false;

            // Gets the flashcard with the matching ID
            var flashcard = fm.GetFlashcard(newFlashcardData.FlashcardID,
                                         familyObject);

            // If the flashcard wasn't found, quit
            if (flashcard == null)
                return false;

            // Changes the text values
            flashcard["side1Text"] = newFlashcardData.Side1Text;
            flashcard["side2Text"] = newFlashcardData.Side2Text;
            flashcard["hasIllustration"] = newFlashcardData.HasIllustration;

            // Fires the data updated event
            fm.OnStaticDataUpdated?.Invoke(null, null);

            // Updates the JSON files
            JSONWriter.UpdateJSONFiles();

            return true;
        }
    }
}
