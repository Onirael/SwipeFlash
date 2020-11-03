using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A class containing a parser ignore rule and its associated methods
    /// </summary>
    public class ParserIgnoreRule
    {
        #region Public Properties

        /// <summary>
        /// The character pattern of the beginning of the line
        /// </summary>
        public string LineStartChars { get; set; }

        /// <summary>
        /// The character pattern of the middle of the line
        /// </summary>
        public List<string> LineMidChars { get; set; }

        /// <summary>
        /// The character pattern of the end of the line
        /// </summary>
        public string LineEndChars { get; set; }

        /// <summary>
        /// True if the rule has at least one valid element, 
        /// false otherwise
        /// </summary>
        public bool IsRuleValid => !string.IsNullOrEmpty(LineStartChars) ||
                                   !string.IsNullOrEmpty(LineEndChars) ||
                                   LineMidChars.Count != 0;

        /// <summary>
        /// True if the rule doesn't contain any star, 
        /// meaning it is limited to a single string and cannot allow more characters
        /// </summary>
        public bool IsRuleLengthBound { get; set; } = false;

        #endregion

        #region Constructor

        /// <summary>
        /// A constructor taking in the user input string for this pattern
        /// </summary>
        /// <param name="rawIgnorePattern">The user input string</param>
        public ParserIgnoreRule(string rawIgnorePattern)
        {
            // Initializes mid chars array
            LineMidChars = new List<string>();

            // Sets the ignore pattern values
            CreateIgnorePattern(rawIgnorePattern);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks whether the line is ignored using this rule
        /// </summary>
        /// <param name="line">The input line</param>
        /// <returns>True if the line should be ignored, false otherwise</returns>
        public bool GetIsLineIgnored(string line)
        {
            // Check if the line is long enough to apply the rule
            if (line.Length < GetRuleLength() || line.Length > GetRuleLength() && IsRuleLengthBound)
                return false;

            // Check for start pattern
            // Checks for the validity of line start characters
            if (LineStartChars != null)
            {
                // For each character of the line start ignore pattern
                foreach (var (ruleChar, index) in LineStartChars.Select((value, i) => (value, i)))
                {
                    // Check if the rule applies
                    if (!CheckRuleCharacter(line[index], ruleChar))
                        return false;
                }
            }

            // Check for end pattern
            // Checks for the validity of line end characters
            if (LineEndChars != null)
            {
                // For each character of the line end ignore pattern
                foreach (var (ruleChar, index) in LineEndChars.Select((value, i) =>
                                                  (value, i + line.Length - LineEndChars.Length)))
                {
                    // Check if the rule applies
                    if (!CheckRuleCharacter(line[index], ruleChar))
                        return false;
                }
            }

            // If line middle character patterns isn't empty
            if (LineMidChars.Count() != 0)
            {
                // The start index of the substring
                int substringStart = LineStartChars == null ? 0 : LineStartChars.Length;
                // The length of the substring
                int substringLength = line.Length - substringStart - (LineEndChars == null ? 0 : LineEndChars.Length);

                // Get substring with start and end stripped
                var lineSlice = line.Substring(substringStart, substringLength);

                // Get the remaining rule length to apply to the line slice 
                // by subtracting the start and end rule lengths to the total length
                int remainingRuleLength = GetRuleLength() - (LineEndChars == null ? 0 : LineEndChars.Length) - substringStart;

                // For each rule string of the rule middle
                foreach (var ruleStr in LineMidChars)
                {
                    // Check if the rule is still applicable given the updated slice length
                    if (lineSlice.Length < remainingRuleLength)
                        return false;

                    // The index of the character match
                    int matchIndex = -1;

                    // Repeats this until a starting character in the line matches the pattern
                    for (int startIndex = 0; startIndex < lineSlice.Length - remainingRuleLength; ++startIndex)
                    {
                        // Whether the current starting character presents a match
                        bool isCharacterMatch = true;

                        // Check for pattern match on the current starting character
                        foreach (var (ruleChar, index) in ruleStr.Select((value, t) => (value, t)))
                        {
                            // If there is no match
                            if (!CheckRuleCharacter(lineSlice[startIndex + index], ruleChar))
                            {
                                // Set the match flag to false
                                isCharacterMatch = false;
                                break;
                            }
                        }

                        // If a match was found, break the loop
                        if (isCharacterMatch)
                        {
                            matchIndex = startIndex;
                            break;
                        }
                    }

                    // If a match was found in the nested loop
                    if (matchIndex >= 0)
                    {
                        // Updates the line slice and the remaining rule length
                        lineSlice = lineSlice.Substring(matchIndex + ruleStr.Length);
                        remainingRuleLength -= ruleStr.Length;
                    }
                    // Otherwise, quit
                    else
                        return false;

                }
            }

            // If the execution reaches the end
            // return false
            return true;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks whether a given character matches the given rule character
        /// e.g. '#' as a rule character only matches a digit in the input
        /// </summary>
        /// <param name="character">The input character</param>
        /// <param name="ruleCharacter">The rule character to check against</param>
        /// <returns>True if both characters match, false otherwise</returns>
        private bool CheckRuleCharacter(char character, char ruleCharacter)
        {
            switch (ruleCharacter)
            {
                // If the symbol is a # a digit is expected
                case '#':
                    return char.IsDigit(character);
                // If the symbol is a ^ an uppercase character is expected
                case '^':
                    return char.IsUpper(character);
                // If the symbol is a _ a lowercase character is expected
                case '_':
                    return char.IsLower(character);

                // If the symbol is not a key, compare both characters
                default:
                    // If both are the same, return true
                    if (character == ruleCharacter)
                        return true;
                    // If the character could not be compared, quit
                    else
                        return false;
            }
        }

        /// <summary>
        /// Creates the ignore pattern based on the given string
        /// </summary>
        /// <param name="ignorePattern">The raw input pattern string fed by the user</param>
        private void CreateIgnorePattern(string ignorePattern)
        {
            // Check if the ignore pattern isn't empty
            if (ignorePattern == "")
                return;

            // Remove spaces from pattern
            ignorePattern = Regex.Replace(ignorePattern, " ", "");

            // Split the pattern whenever a * is used
            // this allows it to divide the string for pattern checking
            var splitIgnorePattern = ignorePattern.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

            // If the pattern is empty, quit
            if (splitIgnorePattern.Count() == 0)
                return;

            // If the pattern has at least one element
            if (splitIgnorePattern.Count() >= 1)
            {
                // If the first character of the first pattern is not preceded by a *
                if (splitIgnorePattern[0][0] == ignorePattern[0])
                {
                    // Set the first characters of the ignore pattern
                    LineStartChars = splitIgnorePattern[0];

                    // If the pattern has only one element
                    if (splitIgnorePattern.Count() == 1 && splitIgnorePattern[0].Last() == ignorePattern.Last())
                    {
                        IsRuleLengthBound = true;
                    }
                }
                // If the pattern has only one element preceded by an asterisk
                else if (splitIgnorePattern.Count() == 1)
                    // Set the end characters of the ignore pattern
                    LineEndChars = splitIgnorePattern[0];
                else
                    // Set the first middle characters of the ignore pattern
                    LineMidChars.Add(splitIgnorePattern[0]);
            }

            // If the pattern has more than two elements, 
            // this means it has middle elements
            if (splitIgnorePattern.Count() > 2)
            {
                // Gets the amount of mid patterns
                var midPatternsCount = splitIgnorePattern.Count() - 2;

                // Add each middle pattern to the list
                for (int i = 1; i < midPatternsCount + 1; ++i)
                {
                    LineMidChars.Add(splitIgnorePattern[i]);
                }
            }

            // If the pattern has at least two elements
            if (splitIgnorePattern.Count() >= 2)
            {
                // Gets the last ignore pattern
                var lastIgnorePattern = splitIgnorePattern.Last();

                // Gets the last character of the unsplit pattern
                var lastIgnorePatternChar = ignorePattern.Last().ToString();

                // If the last character of the last pattern is not followed by a *
                if (lastIgnorePattern.EndsWith(lastIgnorePatternChar))
                    // Set the last characters of the ignore pattern
                    LineEndChars = lastIgnorePattern;
                else
                    // Set the mid characters of the ignore pattern
                    // This has to be done after all other mid characters have been added
                    // in order to maintain the order of the array
                    LineMidChars.Add(lastIgnorePattern);
            }
        }

        /// <summary>
        /// Computes the total character length, excluding '*' symbols, of the rule
        /// </summary>
        /// <returns>The minimum characters required to apply the rule</returns>
        private int GetRuleLength()
        {
            // Gets the start and end length
            int length = 0;
            if (LineStartChars != null) length += LineStartChars.Length;
            if (LineEndChars != null) length += LineEndChars.Length;

            // Adds the length of the middle characters
            LineMidChars.ForEach((t) => length += t.Length);

            return length;
        }

        #endregion
    }
}
