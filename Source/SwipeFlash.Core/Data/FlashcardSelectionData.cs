using System;
using System.Collections.Generic;

namespace SwipeFlash.Core
{
    public static class FlashcardSelectionData
    {
        /// <summary>
        /// The expiry time of the flashcards in the given section
        /// </summary>
        public static Dictionary<int, TimeSpan> SectionExpiryTime = new Dictionary<int, TimeSpan>()
        {
            {1, new TimeSpan(0, 1, 0)},
            {2, new TimeSpan(1, 0, 0)},
            {3, new TimeSpan(1, 0, 0, 0)},
            {4, new TimeSpan(7, 0, 0, 0)},
            {5, new TimeSpan(30, 0, 0, 0)},
        };
    }
}
