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
            Console.WriteLine(Evaluator.Evaluate("4-2", fourDel));
        }

        public static int DelegateMethod(string s)
        {
            return 4;
        }

    }
}
