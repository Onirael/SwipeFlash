using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SwipeFlash.Core
{
    /// <summary>
    /// An struct containing an instruction to apply when a variable has a given value
    /// </summary>
    public struct ParserValueInstruction
    {
        /// <summary>
        /// The variable to assign to
        /// </summary>
        public int AssignedVar;

        /// <summary>
        /// The elements of the instruction operation
        /// </summary>
        public List<string> InstructionElements;

        /// <summary>
        /// The operators to apply
        /// </summary>
        public List<char> Operators;
    }

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
        public Dictionary<string, List<ParserValueInstruction>> ValueInstructions { get; set; }

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
        public ParserVariableInstruction(string rawVariableInstruction,
                                         ref List<ParserLineVariable> lineVars)
        {
            // Initializes the dictionary
            ValueInstructions = new Dictionary<string, List<ParserValueInstruction>>();

            // Check if the instruction is empty
            if (rawVariableInstruction.Length == 0)
                return;

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

                // Creates the value instruction
                var newInstruction = MakeValueInstruction(splitValueInstruction[1], ref lineVars);

                // If the value already has an instruction
                if (ValueInstructions.ContainsKey(valueStr))
                    // Adds the instruction to the list
                    ValueInstructions[valueStr].Add(newInstruction);
                else
                    // Create a new list
                    ValueInstructions.Add(valueStr, new List<ParserValueInstruction>() { newInstruction });
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
                                     List<ParserLineVariable> lineVars)
        {
            // Finds the index of the variable in the split line
            var variableIndex = lineVars.FindIndex(variable => variable.VariableName == VariableName);

            // If the variable was not found
            if (variableIndex < 0)
                return false;

            // For each possible value
            foreach (KeyValuePair<string, List<ParserValueInstruction>> entry in ValueInstructions)
            {
                // If the entry key matches the variable value
                if (splitLine[variableIndex] == entry.Key)
                {
                    var instructionList = entry.Value;

                    // For each instruction
                    foreach (var instruction in instructionList)
                    {
                        // For each operator in the operators array
                        foreach (var item in instruction.Operators.Select((operatorChar, index) => new { index, operatorChar }))
                        {
                            var opElement1 = instruction.InstructionElements[item.index];
                            // If the element starts and ends with \" remove the quotation marks
                            if (opElement1[0] == '\"' && opElement1.Last() == '\"')
                                opElement1 = opElement1.Replace("\"", "");
                            // If it is a variable name, get the variable
                            else
                            {
                                // Gets the variable
                                int varIndex = lineVars.FindIndex(variable => variable.VariableName == opElement1);
                                opElement1 = splitLine[varIndex];
                            }

                            var opElement2 = instruction.InstructionElements[item.index + 1];
                            // If the element starts and ends with \" remove the quotation marks
                            if (opElement2[0] == '\"' && opElement2.Last() == '\"')
                                opElement2 = opElement2.Replace("\"", "");
                            // If it is a variable name, get the variable
                            else
                            {
                                // Gets the variable
                                int varIndex = lineVars.FindIndex(variable => variable.VariableName == opElement2);
                                opElement2 = splitLine[varIndex];
                            }

                            // Sets the appropriate variable to its new value
                            splitLine[instruction.AssignedVar] = ApplyOperation(item.operatorChar, opElement1, opElement2);
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Creates the value instruction struct for the given instruction
        /// </summary>
        /// <param name="instruction">The raw instruction</param>
        /// <param name="lineVars">The line variable names</param>
        /// <returns>Returns the instruction struct</returns>
        private ParserValueInstruction MakeValueInstruction(string instruction,
                                                            ref List<ParserLineVariable> lineVars)
        {
            var valueInstruction = new ParserValueInstruction();

            // Splits the instruction by the equal sign
            var splitInstruction = instruction.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

            // If the instruciton is invalid, quit
            if (splitInstruction.Length != 2)
            {
                _isSyntaxValid = false;
                return valueInstruction;
            }

            // Find the variable to apply instruction to
            valueInstruction.AssignedVar = lineVars.FindIndex(variable => variable.VariableName == splitInstruction[0]);

            // If the assigned variable could not be found, quit
            if (valueInstruction.AssignedVar < 0)
            {
                _isSyntaxValid = false;
                return valueInstruction;
            }

            // Creates the valid operators array
            var validOperators = new char[] { '+', '-' };

            // Creates the operators array
            valueInstruction.Operators = splitInstruction[1].ToList().FindAll(element => validOperators.Contains(element));

            // Separates the operation elements
            valueInstruction.InstructionElements = splitInstruction[1].Split(validOperators, StringSplitOptions.RemoveEmptyEntries).ToList();

            // If the operators and elements count is incompatible
            if (valueInstruction.InstructionElements.Count() != valueInstruction.Operators.Count() + 1)
            {
                _isSyntaxValid = false;
                return valueInstruction;
            }

            // For each element
            foreach (var element in valueInstruction.InstructionElements)
            {
                // If the element is not in quotation marks
                if (element[0] != '\"')
                {
                    // If the element could not be found in the variables, quit
                    if (lineVars.FindIndex(variable => variable.VariableName == element) < 0)
                    {
                        _isSyntaxValid = false;
                        return valueInstruction;
                    }
                }
            }

            return valueInstruction;
        }

        /// <summary>
        /// Applies an operator to two given elements 
        /// </summary>
        /// <param name="operatorChar">The operator to apply</param>
        /// <param name="element1">The left hand side element</param>
        /// <param name="element2">The right hand side element</param>
        /// <returns>The resulting string</returns>
        private string ApplyOperation(char operatorChar, string element1, string element2)
        {
            switch(operatorChar)
            {
                case '+':
                    return element1 + element2;
                case '-':
                    return element1.Replace(element2, "");

                default:
                    return element1;
            }
        }

        #endregion
    }
}
