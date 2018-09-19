using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    /// <summary>
    /// @author - Harrison Quick - u1098604
    /// Class for evaluating simple arithmetic expressions. Valid operators include:[ +, -, *, /, (, ) ]. 
    /// The class contains one method, Evaluate, which may call two helper methods to complete arithmetic 
    /// operations on a inputted string. Evaluator also uses a delegate to allow the use of different 
    /// variable lookup methods.
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Delegate that returns an int based on a string composed of any number of letters followed
        /// by any number of integers
        /// </summary>
        /// <param name="v">variable string to be converted into a value</param>
        /// <returns>an int if the variable passed in is correlated with one</returns>
        public delegate int Lookup(string v);

        /// <summary>
        /// Stack for holding operator tokens passed in the expression
        /// </summary>
        private static Stack<string> operators = new Stack<string>();

        /// <summary>
        /// Stack for holding integers (and integers associated with variables) passed in the expression
        /// </summary>
        private static Stack<int> values = new Stack<int>();

        /// <summary>
        /// int for keeping track of the value passed into the algorithm or changed by the algorithm
        /// </summary>
        private static int currVal;

        /// <summary>
        /// Algorithm that evaluates simple arithmetic expressions (ie (4 + 5) / 3). Valid operator tokens include:[ +, -, *, /, (, ) ]
        /// </summary>
        /// <param name="exp">Expression inputted by the user to be evaluated by the algorithm</param>
        /// <param name="variableEvaluator">Delegate object for looking up the value of variables within the expression</param>
        /// <returns>the value of the passed expression in the form of an int</returns>
        public static int Evaluate(string exp, Lookup variableEvaluator)
        {
            operators.Clear();
            values.Clear();

            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string substring in substrings)
            {
                //Regex for finding variables
                if (Regex.IsMatch(substring, @"(\s?[A-Za-z]{1,}[0-9]{1,}\s?)"))
                {
                    currVal = variableEvaluator(substring.Trim());
                    VariableIntegerInstructions(currVal);
                }
                //Regex for finding just integers
                else if (Regex.IsMatch(substring, @"(\s?[0-9]{1,}\s?)"))
                {
                    if (int.TryParse(substring.Trim(), out currVal))
                    {
                        VariableIntegerInstructions(currVal);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid integer token");
                    }
                }
                //Regex for finding '+' and '-' operators
                else if (Regex.IsMatch(substring, @"(\s?\+|\-\s?)"))
                {
                    if (operators.Count > 0 && operators.Peek().Equals("+"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("Operators must have two operands");
                        }
                        operators.Pop();
                        values.Push(values.Pop() + values.Pop());
                        operators.Push(substring);
                    }
                    else if (operators.Count > 0 && operators.Peek().Equals("-"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("Operators must have two operands");
                        }
                        operators.Pop();
                        int placeHolder = values.Pop();
                        values.Push(values.Pop() - placeHolder);
                        operators.Push(substring);
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
                    if (!operators.Contains("("))
                    {
                        throw new ArgumentException("Mismatched Parentheses");
                    }

                    if (operators.Peek().Equals("+"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("Operators must have two operands");
                        }
                        operators.Pop();
                        values.Push(values.Pop() + values.Pop());
                    }
                    else if (operators.Peek().Equals("-"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("Operators must have two operands");
                        }
                        operators.Pop();
                        int placeHolder = values.Pop();
                        values.Push(values.Pop() - placeHolder);
                    }

                    //Next operator should be '(' - pop it.
                    if (operators.Count > 0 && !operators.Pop().Equals("("))
                    {
                        throw new ArgumentException("Each set of parentheses must have an opening and closing parenthesis");
                    }
                    
                    if (operators.Count > 0 && (operators.Peek().Equals("*") || operators.Peek().Equals("/")))
                    {
                        VariableIntegerInstructions(values.Pop());
                    }
                }
                
            }
            //Final token has been processed - time to take final steps
            if (operators.Count.Equals(0))
            {
                if (!values.Count.Equals(1))
                {
                    throw new ArgumentException("Expression contains too many or too few integers and variables");
                }
                int retVal = values.Pop();
                return retVal;
            }
            else if (operators.Count > 0 && operators.Peek().Equals("+"))
            {
                if (!values.Count.Equals(2))
                {
                    throw new ArgumentException("Expression contains too many or too few integers and variables");
                }
                if (operators.Count >= 2)
                {
                    throw new ArgumentException("Expression contains too many operators");
                }
                int retVal = values.Pop() + values.Pop();

                return retVal;
            }
            else if (operators.Count > 0 && operators.Peek().Equals("-"))
            {
                if (!values.Count.Equals(2))
                {
                    throw new ArgumentException("Expression contains too many or too few integers and variables");
                }
                int placeHolder = values.Pop();
                int retVal = values.Pop() - placeHolder;

                return retVal;
            }
            else
            {
                if (operators.Count > 0)
                {
                    throw new ArgumentException("Expression contains too many operators per operand");
                }
                throw new ArgumentException("Expression contains too few operators or has other syntax problems");
            }
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
                try
                {
                    operators.Pop();
                    currVal *= values.Pop();
                    values.Push(currVal);
                } catch (InvalidOperationException e)
                {
                    throw new ArgumentException("Expressions cannot start with operators", e);
                }
            }
            else if (operators.Count > 0 && operators.Peek().Equals("/"))
            {
                try
                {
                    operators.Pop();
                    int placeHolder = values.Pop();
                    currVal = placeHolder / currVal;
                    values.Push(currVal);
                } catch (DivideByZeroException e)
                {
                    throw new ArgumentException("Cannot divide by 0", e);
                }
            }
            else
            {
                values.Push(currVal);
            }
        }

    }
}
