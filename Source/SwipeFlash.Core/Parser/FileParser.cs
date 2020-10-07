using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A static class containing the ParseFile method used to 
    /// get flashcard data from a given file, 
    /// applying user-specified pattern rules
    /// </summary>
    public static class FileParser
    {
        /// <summary>
        /// Parses a given file
        /// </summary>
        /// <param name="inFamilyName">The name of the new card family</param>
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
            // Create and initialize parsed flashcards array
            var parsedFlashcards = new List<ParsedFlashcardData>();

            // Create separators array
            var separators = CreateSeparatorsArray(separatorsDescription);

            #region Create Ignore Rules

            // Create and initialize ignore rule list
            var ignoreRuleList = new List<ParserIgnoreRule>();

            // Split the user input by semicolons
            var ignorePatterns = ignorePattern.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            // For each pattern
            foreach (var pattern in ignorePatterns)
            {
                // Creates the new rule
                var newRule = new ParserIgnoreRule(pattern);

                // If the rule is valid, add it to the list
                if (newRule.IsRuleValid) ignoreRuleList.Add(newRule);
            }

            #endregion

            #region Create Line Pattern

            // Split pattern description
            var splitPatternDescription = linePattern.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Gets the line description part to retrieve the variables
            var lineDescription = splitPatternDescription[0];
            lineDescription = Regex.Replace(lineDescription, " ", "");

            // Create line variables
            var lineVars = lineDescription.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var lineVarsList = new List<ParserLineVariable>();

            foreach(var lineVariable in lineVars)
            {
                // Create the new line variable struct
                var newLineVar = new ParserLineVariable();

                // Sets the optional flag
                newLineVar.IsOptional = lineVariable.Last() == '?';

                // Sets the variable name, stripping the optional flag
                newLineVar.VariableName = Regex.Replace(lineVariable, "?", "");

                // Adds the new line variable to the list
                lineVarsList.Add(newLineVar);
            }

            // Check if line variables contain the side 1 and side 2 descriptors
            // Store side variable indexes
            int side1VarIndex = lineVarsList.FindIndex(variable => variable.VariableName=="[1]");
            int side2VarIndex = lineVarsList.FindIndex(variable => variable.VariableName=="[2]");

            if (side1VarIndex < 0 || side2VarIndex < 0)
                return false;

            // Create rules list
            var patternRules = new List<string>();

            // For every rule section
            bool isFirstElement = true;
            foreach(var ruleSection in splitPatternDescription)
            {
                // If it is the first element (line description), skip it
                if (isFirstElement) { isFirstElement = false; continue; }

                // Add the split section to the pattern rules
                patternRules.Add(ruleSection);
            }

            // For each found rule, create a new variable instruction
            patternRules.ForEach((t) => 
            {
                // Create the instruction from the raw input
                var varInstruction = new ParserVariableInstruction(t);

                // If the instruction is marked as valid
                if (varInstruction.IsInstructionValid)
                {
                    // Add it to its corresponding variable
                    var matchingVariable = lineVarsList.Find(variable => variable.VariableName == varInstruction.VariableName);
                    matchingVariable.VariableInstruction = varInstruction;
                }
            });

            #endregion

            #region Parse File

            // Create reader
            StreamReader reader = File.OpenText(filePath);

            // Reads line by line until the end of the document is reached
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // Check if line is empty
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Check if line should be ignored
                // For each rule
                bool isLineIgnored = false;
                foreach (var ignoreRule in ignoreRuleList)
                {
                    // Check if the rule applies
                    if (ignoreRule.IsRuleValid && ignoreRule.GetIsLineIgnored(line))
                    {
                        isLineIgnored = true;
                        break;
                    }
                }
                if (isLineIgnored) continue;
                
                // Split the line by the given separators and tabs
                string[] items = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                // Convert line to list for easier handling
                List<string> itemsList = items.ToList();

                // Remove all whitespace elements from list
                itemsList.RemoveAll(item => string.IsNullOrWhiteSpace(item));

                // Check if the list is empty
                if (itemsList.Count == 0)
                    continue;

                // If the number of items in the line is lower than the number of variables
                if (itemsList.Count < lineVarsList.Count)
                {
                    // Matches the items to the variables
                    var matchSuccess = MatchItemsToVars(ref itemsList,
                                                        lineVarsList);
                    // If the match was not successful, skip the line
                    if (!matchSuccess)
                        continue;
                }
                else if (itemsList.Count > lineVarsList.Count)
                    continue;

                // Applies the instructions for each item in the list
                itemsList.ForEach((t) =>
                {
                    // Gets the instruction
                    var instruction = lineVarsList[itemsList.IndexOf(t)].VariableInstruction;
                    // If it is valid, applies it
                    if (instruction.IsInstructionValid) instruction.ApplyInstruction(ref itemsList, lineVarsList);

                });

                // Creates a new flashcard with the index
                parsedFlashcards.Add(new ParsedFlashcardData() { Side1Text = itemsList[side1VarIndex], Side2Text = itemsList[side2VarIndex] });
            }

            #endregion

            // Set the flashcards value to the parsed flashcards
            inFamilyData.Flashcards = parsedFlashcards;

            return true;
        }
        
        #region Private Helpers

        /// <summary>
        /// Creates the separators array from the raw user input
        /// </summary>
        /// <param name="separatorsDescription">The raw separators description fed by the user</param>
        /// <returns></returns>
        private static char[] CreateSeparatorsArray(string separatorsDescription)
        {
            char[] separators;
            // If the separators description is empty
            if (separatorsDescription.Length != 0)
            {
                // Splits the separators description string 
                // by removing spaces, commas and semicolons
                var separatorsSplit = separatorsDescription.Split(new char[] { ' ', ';', ',' });

                // Merges the resulting string array to a string and divides it to characters
                separators = string.Join("", separatorsSplit).ToCharArray();

                // Converts the array to a list, adds an element and converts back
                var separatorsList = separators.ToList();
                separatorsList.Add('\t');
                separators = separatorsList.ToArray();
            }
            else
            {
                // Creates an array containing only tabs
                separators = new char[] { '\t' };
            }

            return separators;
        }

        /// <summary>
        /// Matches line items to their expected position in the line, given the line variables list 
        /// </summary>
        /// <param name="itemsList">The line items list</param>
        /// <param name="lineVarsList">The line variables list to match</param>
        /// <returns>True if the items could successfully be matched, false otherwise</returns>
        private static bool MatchItemsToVars(ref List<string> itemsList, List<ParserLineVariable> lineVarsList)
        {
            // Counts the  amount of compulsory variables in the variable list
            var compVarsLeft = lineVarsList.FindAll(variable => variable.IsOptional == false).Count();
            
            // Creates a results array and populates it with dummy data
            var results = new List<string>(lineVarsList.Count());
            for (uint i = 0; i < lineVarsList.Count(); ++i) { results.Add(""); }

            int firstOptional = -1;
            int lastOptional = -1;

            int firstComp = -1;
            int lastComp = -1;

            // Get all sequential compulsory variables starting from the beginning
            int varIndex = 0;
            // While the active variable is not optional
            while (!lineVarsList[varIndex].IsOptional && varIndex < lineVarsList.Count())
            {
                // Set the variable value
                results[varIndex] = itemsList[varIndex];
                // Sets the index of the first found compulsory variable
                firstComp = varIndex;
                // Decrements the compulsory variables left counter
                compVarsLeft--;
                // Changes the active variable
                varIndex++;
            }
            // Sets the index of the first optional variable
            firstOptional = varIndex;

            // Get all sequential compulsory variables starting from the end
            varIndex = lineVarsList.Count() - 1;
            var itemIndex = itemsList.Count() - 1;
            while(!lineVarsList[varIndex].IsOptional && varIndex > 0)
            {
                // Set the variable value
                results[varIndex] = itemsList[itemIndex];
                // Sets the index of the last found compulsory variable
                lastComp = varIndex;
                // Decrements the compulsory variables left counter
                compVarsLeft--;
                // Changes the active variable
                varIndex--;
                // Changes the active item
                itemIndex--;
            }
            // Sets the index of the last optional variable
            lastOptional = itemIndex;

            // While there are more than one remaining optional variables 
            // and all compulsory variables haven't been found
            varIndex = firstOptional;
            itemIndex = varIndex;
            while (firstOptional != lastOptional && compVarsLeft != 0)
            {
                // Try to find a matching optional in the variable list
                var optionalVarIndex = FindOptionalVariable(itemsList[itemIndex], 
                                                            ref lineVarsList, 
                                                            varIndex, 
                                                            lastOptional);
                // If a match was found
                if (optionalVarIndex >= 0)
                {
                    // Set the variable
                    results[optionalVarIndex] = itemsList[itemIndex];
                    // Change the active item
                    itemIndex++;
                    // Change the index of the variable to the matched index
                    varIndex = optionalVarIndex;

                    // If the next element is optional,
                    if (lineVarsList[optionalVarIndex + 1].IsOptional)
                        // Change the first optional
                        firstOptional++;
                    else
                    {
                        // Set the variable of the next non-optional item
                        results[optionalVarIndex + 1] = itemsList[itemIndex];
                        // Change the first compulsory item
                        firstComp = optionalVarIndex + 1;
                        // Decrements the compulsory variables left counter
                        compVarsLeft--;
                        // Change the first optional to the following character
                        firstOptional = varIndex + 2;
                        // Change the item index
                        itemIndex++;
                    }
                }
                else
                {
                    // Increment variables
                    itemIndex++;
                    varIndex++;
                    firstOptional++;
                }
            }

            // A list containing the remaining optional line variables
            var remainingOptionals = lineVarsList.Skip(firstComp - 1).Take(lastComp - firstComp).ToList();

            // If there are more than one remaining optionals
            if (remainingOptionals.Count() > 1)
            {
                var optionalVarIndex = FindOptionalVariable(itemsList[itemIndex], ref lineVarsList, 0);
                if (optionalVarIndex >= 0) results[optionalVarIndex + firstComp - 1] = itemsList[itemIndex];
            }
            // If there is only one optional left
            else if (remainingOptionals.Count() == 1)
            {
                results[varIndex] = itemsList[itemIndex];
            }

            // Sets the items list to the results list
            itemsList = results;

            return true;
        }

        /// <summary>
        /// Finds the index of an optional variable in the line variables list
        /// </summary>
        /// <param name="varValue">The variable value string</param>
        /// <param name="lineVars">The variables of this line</param>
        /// <param name="startIndex">The index where the search begins</param>
        /// <param name="endIndex">The index where the search ends</param>
        /// <returns>Returns the index of the variable if it was found, -1 otherwise</returns>
        private static int FindOptionalVariable(string varValue, ref List<ParserLineVariable> lineVars, int startIndex, int endIndex=-1)
        {
            // For every variable index in the range
            for (int i = startIndex; i < (endIndex == -1 ? lineVars.Count() : endIndex + 1); ++i)
            {
                // If the variable isn't optional, quit
                if (!lineVars[i].IsOptional)
                    return -1;

                // Stores the instruction
                var instruction = lineVars[i].VariableInstruction;

                // For each instruction key value
                foreach(var keyValue in instruction.ValueInstructions.Keys)
                {
                    // If the variable string matches, return the index
                    if (varValue == keyValue) return i;
                }
            }

            return -1;
        }

        #endregion
    }
}
