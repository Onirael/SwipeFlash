using System;
using System.Collections.Generic;
using System.Text;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A helper for strings
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Removes the articles from a given string
        /// </summary>
        /// <param name="text">The text to format</param>
        /// <param name="articles">The list of articles to remove</param>
        /// <returns>The formatted string</returns>
        public static string RemoveArticle(this string text, List<string> articles)
        {
            string newText = "";

            // Splits the card text to words
            char[] separators = { ' ', '/' };
            var splitText = text.Split(separators);

            // For each word
            foreach (var word in splitText)
            {
                // If the word isn't in the articles list
                if (!articles.Contains(word.ToLower()))
                    // Add it to the new text
                    newText += word + " ";
            }

            // Returns the resulting string
            return newText;
        }

        /// <summary>
        /// Checks whether a given string is Unicode
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUnicode(this string input)
        {
            var asciiBytesCount = Encoding.ASCII.GetByteCount(input);
            var unicodBytesCount = Encoding.UTF8.GetByteCount(input);
            return asciiBytesCount != unicodBytesCount;
        }
    }
}
