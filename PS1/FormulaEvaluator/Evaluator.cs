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

        private static Stack<string> operators = new Stack<string>();

        private static Stack<int> values = new Stack<int>();

        private static int currVal;

        //TODO: Handle exceptions throughout the algorithm as defined in PS1 requirements
        public static int Evaluate(string exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string substring in substrings)
            {
                //Regex for finding just integers
                if (Regex.IsMatch(substring, @"(\s?[0-9]{1,}\s?)"))
                {
                    currVal = int.Parse(substring.Trim());
                    VariableIntegerInstructions(currVal);
                }
                //Regex for finding variables
                else if (Regex.IsMatch(substring, @"(\s?[A-Za-z]{1,}[0-9]{1,}\s?)"))
                {
                    currVal = variableEvaluator(substring.Trim());
                    VariableIntegerInstructions(currVal);
                }
                //Regex for finding '+' and '-' operators
                else if (Regex.IsMatch(substring, @"(\s?\+|\-\s?)"))
                {
                    if (operators.Count > 0 && operators.Peek().Equals("+"))
                    {
                        operators.Pop();
                        values.Push(values.Pop() + values.Pop());
                        operators.Push("+");
                    }
                    else if (operators.Count > 0 && operators.Peek().Equals("-"))
                    {
                        operators.Pop();
                        int placeHolder = values.Pop();
                        values.Push(values.Pop() - placeHolder);
                        operators.Push("-");
                    }
                    else // '+' or '-' is not at the top of operators stack
                        operators.Push(Regex.IsMatch(substring, @"(\s?\+\s?)") ? "+" : "-");

                }
                //Regex for finding '*' and '/' operators
                else if (Regex.IsMatch(substring, @"(\s?\*|\/\s?)"))
                {
                    operators.Push(Regex.IsMatch(substring, @"(\s?\*\s?)") ? "*" : "/");
                }
                //Regex for finding '(' left parentheses
                else if (Regex.IsMatch(substring, @"(\s?\(\s?)"))
                {
                    operators.Push("(");
                }
                //Regex for finding ')' right parentheses
                else if (Regex.IsMatch(substring, @"(\s?\)\s?)"))
                {
                    if (operators.Peek().Equals("+"))
                    {
                        operators.Pop();
                        values.Push(values.Pop() + values.Pop());
                    }
                    else if (operators.Peek().Equals("-"))
                    {
                        operators.Pop();
                        int placeHolder = values.Pop();
                        values.Push(values.Pop() - placeHolder);
                    }

                    //Next operator should be '(' - pop it.
                    operators.Pop();

                    if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        VariableIntegerInstructions(values.Pop());
                    }
                }
                
            }
            //Final token has been processed - time to take final steps
            if (operators.Count.Equals(0))
            {
                return values.Pop();
            }
            else if (operators.Peek().Equals("+"))
            {
                return values.Pop() + values.Pop();
            }
            else if (operators.Peek().Equals("-"))
            {
                int placeHolder = values.Pop();
                return values.Pop() - placeHolder;
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
            //Exception case: The stack is empty, and can't be "peeked"
            if (operators.Count > 0 && operators.Peek().Equals("*"))
            {
                operators.Pop();
                currVal *= values.Pop();
                values.Push(currVal);
            }
            else if (operators.Count > 0 && operators.Peek().Equals("/"))
            {
                operators.Pop();
                int placeHolder = values.Pop();
                currVal = placeHolder / currVal;
                values.Push(currVal);
            }
            else
            {
                values.Push(currVal);
            }
        }

    }
}
