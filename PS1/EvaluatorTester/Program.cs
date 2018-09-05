using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

namespace EvaluatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Evaluator.Lookup fourDel = DelegateMethod;
            System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("(94-22)/8", fourDel)); //Should be 9
            System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("*8", fourDel)); //Should throw exception "Cannot start with an operator"
        }

        public static int DelegateMethod(string s)
        {
            return 4;
        }

    }
}
