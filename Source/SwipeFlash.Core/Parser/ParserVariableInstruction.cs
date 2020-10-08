using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A class containing an instruction used when parsing a file's lines
    /// </summary>
    public class ParserVariableInstruction
    {
        #region Public Properties

        /// <summary>
        /// The name of the variable
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// Contains the raw string instruction for every given possible value of this variable
        /// </summary>
        public Dictionary<string, string> ValueInstructions { get; set; }

        private bool _isSyntaxValid = true;
        /// <summary>
        /// Whether this variable contains any valid instructions
        /// </summary>
        public bool IsInstructionValid => ValueInstructions.Count > 0 && _isSyntaxValid;

        #endregion

        #region Constructor

        /// <summary>
        /// A constructor taking in the raw user input
        /// </summary>
        /// <param name="rawVariableInstruction">The raw variable instruction</param>
        public ParserVariableInstruction(string rawVariableInstruction)
        {
            // Initializes the dictionary
            ValueInstructions = new Dictionary<string, string>();

            // Check if the instruction is empty
            if (rawVariableInstruction.Length == 0)
                return;

            // DEVELOPMENT
            // CHECK SYNTAX !!!!!!

            // Separate both sides of the equality
            var splitInstruction = rawVariableInstruction.Split(new string[] { "==" }, StringSplitOptions.None);

            // Get variable name
            splitInstruction[0] = Regex.Replace(splitInstruction[0], " ", "");
            VariableName = splitInstruction[0];

            // Remove braces
            splitInstruction[1] = splitInstruction[1].Replace("{", "");
            splitInstruction[1] = splitInstruction[1].Replace("}", "");

            // Separate value instructions
            var splitValueInstructions = splitInstruction[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var valueInstruction in splitValueInstructions)
            {
                // Splits the value instruction, 
                // with the left hand value as the value
                // and the right hand value as the instruction
                var splitValueInstruction = valueInstruction.Split(':');

                // Gets the expected value and formats it
                var valueStr = splitValueInstruction[0];
                var rx = new Regex("\"(.*)\"");
                valueStr = rx.Match(valueStr).Groups[1].Value;

                // Creates a new dictionary entry with the value name and the raw instruction
                ValueInstructions.Add(valueStr, splitValueInstruction[1]);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Applies the rule to a given split line
        /// </summary>
        /// <param name="splitLine">A string list containing the variable values</param>
        /// <param name="LineVars"></param>
        /// <returns></returns>
        public bool ApplyInstruction(ref List<string> splitLine,
                                     List<ParserLineVariable> LineVars)
        {
            // Finds the index of the variable in the split line
            var variableIndex = LineVars.FindIndex(variable => variable.VariableName == VariableName);

            // If the variable was not found
            if (variableIndex < 0)
                return false;

            // For each possible value
            foreach (KeyValuePair<string, string> entry in ValueInstructions)
            {
                // If the entry key matches the variable value
                if (splitLine[variableIndex] == entry.Key)
                {
                    // APPLY INSTRUCTION
                }
            }

            return true;
        }

        #endregion
    }
}
