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
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Evaluator.Lookup fourDel = AlwaysFourDelegate;
            Console.WriteLine(ValidExpression("(2-3)", fourDel, -1));
            Console.WriteLine(ValidExpression("(3-2)", fourDel, 1));
            Console.WriteLine(ValidExpression("1 + 1", fourDel, 2));
            Console.WriteLine(ValidExpression("9               /        3", fourDel, 3));
            Console.WriteLine(ValidExpression("1000 /5", fourDel, 200));
            Console.WriteLine(ValidExpression("(94-22)/8", fourDel, 9));
            Console.WriteLine(ValidExpression("A2/2", fourDel, 2));
            Console.WriteLine(ValidExpression("(((A2/2)*45)/9)*10", fourDel, 100));
            Console.WriteLine(ValidExpression("5 + 6(/ (1 + 1))", fourDel, 8));
            Console.WriteLine(ValidExpression("() 17 + 3", fourDel, 20));
            Console.WriteLine(ValidExpression("17 + 3 ()", fourDel, 20));
            Console.WriteLine(ValidExpression("AhuASHBbvCA938274329834732894327483297438927483922222222222222 + 3", fourDel, 7));
            Console.WriteLine(ValidExpression("(((((14 / 2 )))))", fourDel, 7));
            Console.WriteLine(ValidExpression("2 / 3", fourDel, 0));
            Console.WriteLine(ValidExpression("18 / 14", fourDel, 1));
            Console.WriteLine(ValidExpression("(14 * 3) * (12/3)", fourDel, 168));
            Console.WriteLine(ValidExpression("2 + (12/4) - 67 + (14 * 3)", fourDel, -20));
            Console.WriteLine(ValidExpression("2 + (12/4) - 67 + (14 * 3) * (( 14 + (567 * 2)) - 10000)", fourDel, -371846));

            Console.WriteLine(InvalidExpression("*8", fourDel));
            Console.WriteLine(InvalidExpression("432 / 0", fourDel));
            Console.WriteLine(InvalidExpression("56 4 *3", fourDel));
            Console.WriteLine(InvalidExpression("5 * + 9", fourDel));
            Console.WriteLine(InvalidExpression("(3 + 5", fourDel));
            Console.WriteLine(InvalidExpression("b45z / 76", fourDel));
            Console.WriteLine(InvalidExpression("#", fourDel));
            Console.WriteLine(InvalidExpression("(3 + 5", fourDel));
            Console.WriteLine(InvalidExpression("3 + 56666666666", fourDel));
            Console.WriteLine(InvalidExpression("(3982 + 483))", fourDel));
        }

        public static int AlwaysFourDelegate(string s)
        {
            return 4;
        }

    }
}
