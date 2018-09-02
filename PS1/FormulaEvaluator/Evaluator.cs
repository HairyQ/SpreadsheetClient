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

        private static Stack<string> values;

        private static int currVal;

        public static int Evaluate(string exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string substring in substrings)
            {
                //Regex for finding variables
                if (Regex.IsMatch(substring, @"\s?[A-Za-z]{1,}[0-9]{1,}\s?"))
                {
                    currVal = variableEvaluator(substring);
                }



            }

            return 0;
        }

    }
}
