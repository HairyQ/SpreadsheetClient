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
            System.Diagnostics.Debug.WriteLine(Evaluator.Evaluate("3*4", fourDel));
        }

        public static int DelegateMethod(string s)
        {
            return 4;
        }

    }
}
