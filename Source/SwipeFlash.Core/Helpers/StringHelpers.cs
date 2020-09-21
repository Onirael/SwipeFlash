using System;
using System.Collections.Generic;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A helper for strings
    /// </summary>
    public static class StringHelpers
    {
        public static string RemoveArticle(this string text)
        {
            // DEVELOPMENT ONLY //
            // Later versions will get the articles from a JSON based on the language //

            var articles = new List<string>()
            {
                "the",
                "a",
                "an",
            };

            //

            string newText = "";

            char[] separators = { ' ', '/' };

            var splitText = text.Split(separators);

            foreach (var word in splitText)
            {
                if (!articles.Contains(word.ToLower()))
                    newText += word + " ";
            }

            return newText;
        }
    }
}
