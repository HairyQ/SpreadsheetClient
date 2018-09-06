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
            Evaluator.Lookup fourDel = AlwaysFourDelegate;
            System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("(94-22)/8", fourDel)); //Should be 9
            System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("A2/2", fourDel)); //Should be 2
            System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("(((A2/2)*45)/9)*10", fourDel)); //Should be 100
            System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("5+6(/(1+1))", fourDel)); //Should be 8

            //System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("*8", fourDel)); //Should throw exception "Cannot start with an operator"
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
