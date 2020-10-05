using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SwipeFlash.Core
{
    public static class FileParser
    {
        /// <summary>
        /// Parses a given file
        /// </summary>
        /// <param name="familyName"></param>
        /// <param name="category1"></param>
        /// <param name="category2"></param>
        /// <param name="logo1"></param>
        /// <param name="logo2"></param>
        /// <param name="ignorePattern"></param>
        /// <param name="separators"></param>
        /// <param name="linePattern"></param>
        /// <returns>Returns true if the parsing was successful, false otherwise</returns>
        public static bool ParseFile(ref ParsedFlashcardFamilyData inFamilyData,
                                     string filePath,
                                     string ignorePattern, 
                                     string separatorsDescription, 
                                     string linePattern)
        {
            // Parse the flashcards

            var parsedFlashcards = new List<ParsedFlashcardData>();

            #region Create Separators Array

            char[] separators;
            // If the separators description is empty
            if (separatorsDescription.Length != 0)
            {
                // Splits the separators description string 
                // by removing spaces, commas and semicolons
                var separatorsSplit = separatorsDescription.Split(new char[] { ' ' });

                // Merges the resulting string array to a string and divides it to characters
                separators = string.Join("", separatorsSplit).ToCharArray();
            }
            else
            {
                // Creates an empty array
                separators = new char[] { };
            }

            #endregion
            
            var ignoreRule = CreateIgnorePattern(ignorePattern);

            #region Parse File

            // Create reader
            StreamReader reader = File.OpenText(filePath);

            // Reads line by line until the end of the document is reached
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // Check if line should be ignored
                if (line == "" || GetIsLineIgnored(line, ignoreRule))
                    continue;


                // Split the line by the given separators and tabs
                string[] items = line.Split('\t');

                // Convert line to list for easier handling
                List<string> itemsList = items.ToList();

                // Remove all whitespace elements from list
                itemsList.RemoveAll(item => string.IsNullOrWhiteSpace(item));

                // If the list is empty, skip this line
                if (itemsList.Count == 0)
                    continue;

                // Set line variables

                // Initialize variables

                // DEVELOPMENT ONLY
                int maxlineVars = 3;

                // Creates the line variables list
                var lineVars = new List<string>(maxlineVars);
                // Adds dummy data to the list
                for (int i = 0; i < maxlineVars; ++i) lineVars.Add(null);

                // Sets the variables according to the given pattern
                lineVars[0] = itemsList[0];
                if (itemsList.Count == 2)
                    lineVars[2] = itemsList[1];
                else
                {
                    lineVars[1] = itemsList[1];
                    lineVars[2] = itemsList[2];
                }

                parsedFlashcards.Add(new ParsedFlashcardData() { Side1Text = lineVars[0], Side2Text = lineVars[2] });
            }

            #endregion

            // Set the flashcards value to the parsed flashcards
            inFamilyData.Flashcards = parsedFlashcards;

            return true;
        }


        #region Private Helpers

        private static ParserIgnoreRule CreateIgnorePattern(string ignorePattern)
        {
            // Creates the rule struct
            var ignoreRule = new ParserIgnoreRule();

            // Check if the ignore pattern isn't empty
            if (ignorePattern == "")
                return ignoreRule;

            // Remove spaces from pattern
            ignorePattern = Regex.Replace(ignorePattern, " ", "");

            // Split the pattern whenever a * is used
            // this allows it to divide the string for pattern checking
            var splitIgnorePattern = ignorePattern.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

            // If the pattern is empty, quit
            if (splitIgnorePattern.Count() == 0)
                return ignoreRule;

            // Initializes mid chars array
            ignoreRule.LineMidChars = new List<string>();

            // If the pattern has at least one element
            if (splitIgnorePattern.Count() >= 1)
            {
                // If the first character of the first pattern is not preceded by a *
                if (splitIgnorePattern[0][0] == ignorePattern[0])
                    // Set the first characters of the ignore pattern
                    ignoreRule.LineStartChars = splitIgnorePattern[0];
                else if (splitIgnorePattern.Count() == 1)
                    // Set the end characters of the ignore pattern
                    ignoreRule.LineEndChars = splitIgnorePattern[0];
                else
                    // Set the first middle characters of the ignore pattern
                    ignoreRule.LineMidChars.Add(splitIgnorePattern[0]);
            }

            if (splitIgnorePattern.Count() > 2)
            {
                // Gets the amount of mid patterns
                var midPatternsCount = splitIgnorePattern.Count() - 2;

                // Add each middle pattern to the list
                for (int i = 1; i < midPatternsCount + 1; ++i)
                {
                    ignoreRule.LineMidChars.Add(splitIgnorePattern[i]);
                }
            }

            if (splitIgnorePattern.Count() >= 2)
            {
                // Gets the last ignore pattern
                var lastIgnorePattern = splitIgnorePattern.Last();

                // Gets the last character of the unsplit pattern
                var lastIgnorePatternChar = ignorePattern.Last().ToString();

                // If the last character of the last pattern is not followed by a *
                if (lastIgnorePattern.EndsWith(lastIgnorePatternChar))
                    // Set the last characters of the ignore pattern
                    ignoreRule.LineEndChars = lastIgnorePattern;
                else
                    // Set the mid characters of the ignore pattern
                    // This has to be done after all other mid characters have been added
                    // in order to maintain the order of the array
                    ignoreRule.LineMidChars.Add(lastIgnorePattern);
            }

            return ignoreRule;
        }

        private static bool GetIsLineIgnored(string line, ParserIgnoreRule ignoreRule)
        {
            // CHECK THE LINE FOR THE IGNORE RULE

            return false;
        }

        #endregion
    }
}
