using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    public static class Evaluator
    {

        public delegate int Lookup(string v);

        private static Stack<string> operators;

        private static Stack<int> values;

        private static int currVal;

        public static int Evaluate(string exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string substring in substrings)
            {
                //Regex for finding just integers
                if (Regex.IsMatch(substring, @"(\s?[0-9]{1,}\s?)"))
                {
                    currVal = int.Parse(substring);
                    VariableIntegerInstructions(currVal);
                }
                //Regex for finding variables
                else if (Regex.IsMatch(substring, @"(\s?[A-Za-z]{1,}[0-9]{1,}\s?)"))
                {
                    currVal = variableEvaluator(substring);
                    VariableIntegerInstructions(currVal);
                }
                //Regex for finding '+' and '-' operators
                else if (Regex.IsMatch(substring, @"(\s?\+|\-\s?)"))
                {

                }
                //Regex for finding '(' left parentheses
                else if (Regex.IsMatch(substring, @"(\s?\(\s?)"))
                {

                }
                //Regex for finding ')' right parentheses
                else if (Regex.IsMatch(substring, @"(\s?\)\s?)"))
                {

                }
                
            }

            return 0;
        }

        /// <summary>
        /// Integer and Variable tokens use the same instructions, so to avoid redundant code, I included this function.
        /// 
        /// </summary>
        /// <param name="currVal">Integer or Variable Value</param>
        private static void VariableIntegerInstructions(int currVal)
        {
            if (operators.Peek().Equals("*"))
            {
                operators.Pop();
                currVal *= values.Pop();
                values.Push(currVal);
            }
            else if (operators.Peek().Equals("/"))
            {
                operators.Pop();
                currVal /= values.Pop();
                values.Push(currVal);
            }
            else
            {
                values.Push(currVal);
            }
        }

    }
}
