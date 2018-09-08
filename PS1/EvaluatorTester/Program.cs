using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

namespace EvaluatorTester
{
    /// <summary>
    /// 
    /// Small testing Class to help my grade on PS1
    /// </summary>
    class Program
    {

        /// <summary>
        /// Testing function for comparing the result of an evaluated string with its expected result
        /// </summary>
        /// <param name="expr">Expression the user inputs for evaluation by FormulaEvaluator</param>
        /// <param name="lookup">Delegate variable lookup object to be passed to FormulaEvaluator.Evaluate</param>
        /// <param name="expected">Expected int result, signifying the code works</param>
        /// <returns>True if the expected result is equal to the Evaluated expression's result, false otherwise</returns>
        private static bool ValidExpression(string expr, Evaluator.Lookup lookup, int expected)
        {
            try
            {
                return Evaluator.Evaluate(expr, lookup) == expected;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Testing function that expects user inputs to return Argument Exceptions - cases where the program should throw an exception
        /// </summary>
        /// <param name="expr">Expression the user inpouts for evaluation by FormulaEvaluator</param>
        /// <param name="lookup">Delegate variable lookup object to be passed to FormulaEvaluator.Evaluate</param>
        /// <returns>true if the program throws the correct exception, false otherwise</returns>
        private static bool InvalidExpression(string expr, Evaluator.Lookup lookup)
        {
            try
            {
                Evaluator.Evaluate(expr, lookup);
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Main basically just calls InvalidExpression() and ValidExpression() over and over, printing the result to the console so that I can determine which tests worked, and which ones didn't
        /// </summary>
        /// <param name="args">Idk, the bit stream or something</param>
        static void Main(string[] args)
        {
            Evaluator.Lookup fourDel = AlwaysFourDelegate;
            Console.WriteLine(ValidExpression("(2-3)", fourDel, -1));
            Console.WriteLine(ValidExpression("(3-2)", fourDel, 1));
            Console.WriteLine(ValidExpression("1 + 1", fourDel, 2));
            Console.WriteLine(ValidExpression("(94-22)/8", fourDel, 9));
            Console.WriteLine(ValidExpression("A2/2", fourDel, 2));
            Console.WriteLine(ValidExpression("(((A2/2)*45)/9)*10", fourDel, 100));
            Console.WriteLine(ValidExpression("5 + 6(/ (1 + 1))", fourDel, 8));

            Console.WriteLine(InvalidExpression("*8", fourDel));
            Console.WriteLine(InvalidExpression("432 / 0", fourDel));
            Console.WriteLine(InvalidExpression("56 4 *3", fourDel));
            Console.WriteLine(InvalidExpression("5 * + 9", fourDel));
            Console.WriteLine(InvalidExpression("(3 + 5", fourDel));


            Console.Read();
            //System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("*8", fourDel)); //Should throw exception "Cannot start with an operator"

            //INvalid:
            //two operands in a row
            //two operators in a row
            //unbalanced ()
            //Invalid token "b45z" "&*"
            //  /0
            //<2 operands for an operator
            //Missing variable

            //Valid:
            //1 + 1 (basic)
            //order of operations (2-3) vs (3-2)
            //test all basic operators individually and with order of operations
            //Parentheses changing OOO
            //giant literal integer - TryParse will throw
            //can use valid variables
            //invalid formula followed by a valid formula
        }

        public static int AlwaysFourDelegate(string s)
        {
            return 4;
        }
        public static int DelegateMethod2(string s)
        {
            return 0;
        }

    }
}
