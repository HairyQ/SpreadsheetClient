using System;
using System.Collections;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTester
{
    [TestClass]
    public class FormulaTester
    {
        /// <summary>
        /// Delegate I will be using in a lot of my methods. Improves readability
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private double LookupDel(string s)
        {
            if (s.Equals("A_3"))
            {
                return 3.2;
            }
            else if (s.Equals("AD488"))
            {
                return 4.99992;
            }
            else
            {
                throw new ArgumentException("No value for variable");
            }
        }

        /// <summary>
        /// Test Method I created for PS5... Didn't help much
        /// 
        /// </summary>
        [TestMethod]
        public void TestExtraTest()
        {
            double lookupDelegate(string s)
            {
                if (s.Equals("A1"))
                    return 1;
                else if (s.Equals("B1"))
                    return 2;
                else if (s.Equals("C1"))
                    return 3;
                else
                    return 0;
            }

            Formula instance = new Formula("A1 + B1 + C1");
            Double result = (Double)instance.Evaluate(lookupDelegate);
            Assert.AreEqual(result, 6);
        }

        /// <summary>
        /// Creates a new instance of a formula object - tests to see that 
        /// </summary>
        [TestMethod]
        public void TestConstructorSetDelegates()
        {
            Func<string, string> normalizer = s => s.ToLower(); //Converts each string to lowercase
            bool validator(string s)
            {
                if (s.Equals("abcd1fdgdgjkt"))
                    return true;
                return false;
            }
            Func<string, double> lookupDel = s => 4.0;
            
            //No assertions for the next lines - shouldn't throw exception
            Formula instance = new Formula("ABCD1FDGdgjkT + 1");
            instance = new Formula("ABCD1FDGdgjkT + 1", normalizer, null);
            instance = new Formula("abcd1fdgdgjkt + 1", null, validator);
            instance = new Formula("ABCD1FDGdgjkT + 1", normalizer, validator);
        }

        [TestMethod]
        public void TestEvaluateSimpleAdditionSpacesVariables()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("1 + 1");
            Assert.AreEqual(2.0, formulaInstance.Evaluate(lookup));

            formulaInstance = new Formula("1+1");
            Assert.AreEqual(2.0, formulaInstance.Evaluate(lookup));

            //Working with variables
            lookup = LookupDel;

            formulaInstance = new Formula("1+A_3");
            Assert.AreEqual("4.2", formulaInstance.Evaluate(lookup).ToString());

            formulaInstance = new Formula("AD488 + 3");
            Assert.AreEqual("7.99992", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateSimpleSubtractionSpaces()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("2 - 1");
            Assert.AreEqual("1", formulaInstance.Evaluate(lookup).ToString());

            formulaInstance = new Formula("2-1");
            Assert.AreEqual("1", formulaInstance.Evaluate(lookup).ToString());

            //Working with variables
            lookup = LookupDel;

            formulaInstance = new Formula("A_3 - 1");
            Assert.AreEqual("2.2", formulaInstance.Evaluate(lookup).ToString());

            formulaInstance = new Formula("3 - AD488");
            Assert.AreEqual("-1.99992", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateSimpleMultiplicationSpaces()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("2 * 4");
            Assert.AreEqual("8", formulaInstance.Evaluate(lookup).ToString());

            formulaInstance = new Formula("2*1");
            Assert.AreEqual("2", formulaInstance.Evaluate(lookup).ToString());

            //Working with variables:
            lookup = LookupDel;

            formulaInstance = new Formula("A_3 * 3");
            Assert.AreEqual("9.6", formulaInstance.Evaluate(lookup).ToString());

            formulaInstance = new Formula("0.5 * AD488");
            Assert.AreEqual("2.49996", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateSimpleDivisionSpaces()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("20 / 4");
            Assert.AreEqual("5", formulaInstance.Evaluate(lookup).ToString());

            formulaInstance = new Formula("20/4");
            Assert.AreEqual("5", formulaInstance.Evaluate(lookup).ToString());

            //Working with variables:
            lookup = LookupDel;

            formulaInstance = new Formula("A_3 / (1/3)");
            Assert.AreEqual("9.6", formulaInstance.Evaluate(lookup).ToString());

            formulaInstance = new Formula("AD488 / 2");
            Assert.AreEqual("2.49996", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexAdditionAndSpacing()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("(3+7)+5.4 + 9 + (8 + 4)");
            Assert.AreEqual("36.4", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexSubtractionAndSpacing()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("(234 -68) - 4.567 - 100-43- 0.43");
            Assert.AreEqual("18.003", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexMultiplicationAndSpacing()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("23 * (5 * 2) * 0.1 * 100 * 0.01");
            Assert.AreEqual("23", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexDivisionAndSpacing()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("1000.40 /2 / 0.1/ (12 / 4) / (1/1/3)");
            Assert.AreEqual("5002", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexAdditionWithVariables()
        {
            Func<string, double> lookup = LookupDel;

            //A_3 = 3.2
            //AD488 = 4.99992

            Formula formulaInstance = new Formula("(A_3+7)+5.4 + AD488 + (8 + 4)");
            Assert.AreEqual("32.59992", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexSubtractionWithVariables()
        {
            Func<string, double> lookup = LookupDel;

            //A_3 = 3.2
            //AD488 = 4.99992

            Formula formulaInstance = new Formula("(A_3-7)-5.4 - AD488 - (8 - 4)");
            Assert.AreEqual("-18.19992", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexMultiplicationWithVariables()
        {
            Func<string, double> lookup = LookupDel;

            //A_3 = 3.2
            //AD488 = 4.99992

            Formula formulaInstance = new Formula("(A_3*7)*5.4 * AD488 * (8 * 4)");
            Assert.AreEqual("19353.2903424", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexDivisionWithVariables()
        {
            Func<string, double> lookup = LookupDel;

            //A_3 = 3.2
            //AD488 = 4.99992

            Formula formulaInstance = new Formula("(A_3/7)/5.4 / AD488 / (8 / 4)");
            double value = (double)formulaInstance.Evaluate(lookup);
            Assert.AreEqual("0.00846574391751114", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexEquationsNoVariables()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("8.6 - (64 / (5 + 3.9) * 3)");
            Assert.AreEqual("-12.9730337078652", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexEquationsWithVariables()
        {
            double LookupDelegate(string s)
            {
                if (s.Equals("GHE_43"))
                {
                    return 3.9;
                } else if (s.Equals("dfsa6f"))
                {
                    return 64;
                } else
                {
                    throw new ArgumentException("invalid variable");
                }
            }

            Func<string, double> lookup = LookupDelegate;

            Formula formulaInstance = new Formula("8.6 - (dfsa6f / (5 + GHE_43) * 3)");
            Assert.AreEqual("-12.9730337078652", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestGetVariablesBasic()
        {
            Formula instance = new Formula("X + 23 - (4/FDS78)");

            int xCounter = 0;
            int FDS78Counter = 0;

            foreach (string s in instance.GetVariables())
            {
                if (s.Equals("X"))
                    xCounter++;
                else if (s.Equals("FDS78"))
                    FDS78Counter++;
            }

            Assert.IsTrue(xCounter == 1);
            Assert.IsTrue(FDS78Counter == 1);
        }

        [TestMethod]
        public void TestGetVariablesDifferentCasesLowercaseNormalizer()
        {
            Func<string, string> normalizer = s => s.ToLower();
            Func<string, bool> validator = s => true;
            Formula instance = new Formula("X + x - 43 * X", normalizer, validator);

            int counter = 0;
            foreach (string s in instance.GetVariables())
            {
                counter++;
                Assert.IsTrue(s.Equals("x"));
            }
            Assert.IsTrue(counter == 1);
        }

        [TestMethod]
        public void TestGetVariablesDifferentCasesNoNormalizer()
        {
            Formula instance = new Formula("X + x - 43 * X");

            int capitalCounter = 0;
            int lowercaseCounter = 0;
            foreach (string s in instance.GetVariables())
            {
                if (s.Equals("x"))
                    lowercaseCounter++;
                else if (s.Equals("X"))
                    capitalCounter++;
            }
            Assert.IsTrue(capitalCounter == 1);
            Assert.IsTrue(lowercaseCounter == 1);
        }

        [TestMethod]
        public void TestGetVariablesWithNoVariables()
        {
            Formula instance = new Formula("12 + 8 - 43 * 2");

            Assert.IsFalse(instance.GetVariables().GetEnumerator().MoveNext());
        }

        [TestMethod]
        public void TestToStringBasicWithAndWithoutSpaces()
        {
            Formula instance = new Formula("43 + x");
            Formula instance2 = new Formula("43+x");

            Assert.IsTrue(instance.ToString().Equals("43+x"));
            Assert.AreEqual(instance.ToString(), instance2.ToString());
        }

        [TestMethod]
        public void TestToStringWithUppercaseNormalizer()
        {
            Func<string, string> normalizer = s => s.ToUpper();
            Formula instance = new Formula("x + 43 + X", normalizer, null);

            Assert.AreEqual(instance.ToString(), "X+43+X");
        }

        [TestMethod]
        public void TestToStringComplexFormulaSpaceTrimming()
        {
            Formula instance = new Formula("3242     *  CS889/8/(9 *43)");

            Assert.AreEqual(instance.ToString(), "3242*CS889/8/(9*43)");
        }

        [TestMethod]
        public void TestToStringUppercaseNormalizer()
        {
            Func<string, string> normalizer = s => s.ToUpper();
            Formula instance = new Formula("x + X * NJ42bCd", normalizer, null);

            Assert.AreEqual(instance.ToString(), "X+X*NJ42BCD");
        }

        [TestMethod]
        public void TestEqualsReturnsFalseOnNoFormula()
        {
            Formula f = new Formula("1+1");

            Assert.IsFalse(f == null);
            Assert.IsFalse(f.Equals("Hello"));

        }

        [TestMethod]
        public void TestEqualsWithAndWithoutSpaces()
        {
            Formula instance = new Formula("2 +      3");
            Formula instance2 = new Formula("2+3");

            Assert.IsTrue(instance.Equals(instance2));
        }

        [TestMethod]
        public void TestEqualsUnequalFormulae()
        {
            Formula instance = new Formula("2+3/5");
            Formula instance2 = new Formula("2+3");

            Assert.IsFalse(instance.Equals(instance2));
        }

        [TestMethod]
        public void TestEqualsWithUppercaseNormalizer()
        {
            Func<string, string> normalizer = s => s.ToUpper();

            Assert.IsTrue(new Formula("a1+b2", normalizer, null).Equals(new Formula("A1 + B2")));
            Assert.IsFalse(new Formula("a1+b2").Equals(new Formula("A1 + B2")));
        }

        [TestMethod]
        public void TestEqualsMismatchedTokens()
        {
            Assert.IsFalse(new Formula("A3 + B3").Equals(new Formula("B3 + A3")));
            Assert.IsFalse(new Formula("14.3 + B43 - 100 + Bf43").Equals(new Formula("14.3-B43+100+Bf43")));
        }

        [TestMethod]
        public void TestEqualsSameValueDiffFloats()
        {
            Assert.IsTrue(new Formula("X + 43.000").Equals(new Formula("X + 43.0")));
        }

        [TestMethod]
        public void TestEqualsOperatorUnequalFormulae()
        {
            Formula instance = new Formula("2+3/5");
            Formula instance2 = new Formula("2+3");

            Assert.IsFalse(instance == instance2);
        }

        [TestMethod]
        public void TestEqualsOperatorWithUppercaseNormalizer()
        {
            Func<string, string> normalizer = s => s.ToUpper();

            Assert.IsTrue(new Formula("a1+b2", normalizer, null) == new Formula("A1 + B2"));
            Assert.IsFalse(new Formula("a1+b2") == new Formula("A1 + B2"));
        }

        [TestMethod]
        public void TestEqualOperatorsMismatchedTokens()
        {
            Assert.IsFalse(new Formula("A3 + B3") == new Formula("B3 + A3"));
            Assert.IsFalse(new Formula("14.3 + B43 - 100 + Bf43") == new Formula("14.3-B43+100+Bf43"));
        }

        [TestMethod]
        public void TestEqualsOperatorSameValueDiffFloats()
        {
            string s = ((double)43.0).ToString();
            string x = ((double)43.0000).ToString();
            Assert.IsTrue(new Formula("X + 43.000") == new Formula("X + 43.0"));
        }

        [TestMethod]
        public void TestEqualsOperatorBasicWithSpaces()
        {
            Assert.IsTrue(new Formula("1 + 3.4 -    B43") == new Formula("1+3.4-B43"));
        }

        [TestMethod]
        public void TestEqualsOperatorNullValues()
        {
            Formula nullForm = null;
            Formula nullForm2 = null;
            Formula instance = new Formula("1 + 2");

            Assert.IsTrue(nullForm == nullForm2);
            Assert.IsFalse(instance == nullForm);
            Assert.IsFalse(nullForm == instance);
        }

        [TestMethod]
        public void TestNotEqualOperatorBasic()
        {
            Formula instance = new Formula("2+3/5");
            Formula instance2 = new Formula("2+3");

            Assert.IsTrue(instance != instance2);
        }

        [TestMethod]
        public void TestNotEqualOperatorWithUppercaseNormalizer()
        {
            Func<string, string> normalizer = s => s.ToUpper();

            Assert.IsFalse(new Formula("a1+b2", normalizer, null) != new Formula("A1 + B2"));
            Assert.IsTrue(new Formula("a1+b2") != new Formula("A1 + B2"));
        }

        [TestMethod]
        public void TestNotEqualOperatorsMismatchedTokens()
        {
            Assert.IsTrue(new Formula("A3 + B3") != (new Formula("B3 + A3")));
            Assert.IsTrue(new Formula("14.3 + B43 - 100 + Bf43") != new Formula("14.3-B43+100+Bf43"));
        }

        [TestMethod]
        public void TestNotEqualOperatorSameValueDiffFloats()
        {
            Assert.IsFalse(new Formula("X + 43.000") != new Formula("X + 43.0"));
        }

        [TestMethod]
        public void TestNotEqualOperatorBasicWithSpaces()
        {
            Assert.IsFalse(new Formula("1 + 3.4 -    B43") != new Formula("1+3.4-B43"));
        }

        [TestMethod]
        public void TestNotEqualOperatorNullValues()
        {
            Formula nullForm = null;
            Formula nullForm2 = null;
            Formula instance = new Formula("1 + 2");

            Assert.IsFalse(nullForm != nullForm2);
            Assert.IsTrue(instance != nullForm);
            Assert.IsTrue(nullForm != instance);
        }

        [TestMethod]
        public void TestGetHashCodeEqualFormulae()
        {
            Formula instance = new Formula("A6 + (43.56 / fdasD90)");
            Formula instance2 = new Formula("A6+(43.56/fdasD90)");

            Assert.AreEqual(instance.GetHashCode(), instance2.GetHashCode());

            instance2 = instance;

            Assert.AreEqual(instance.GetHashCode(), instance2.GetHashCode());
        }

        /// <summary>
        /// This test method is used to determine how "good" my hash function is - a failed
        /// test implies a poor hash function, not necessarily that GetHashCode() doesn't
        /// work. The limit for i in the first for loop can be adjusted to determine the 
        /// quality of the hash code.
        /// </summary>
        [TestMethod]
        public void TestGetHashCodeQuality()
        {
            int currHashCode = -1;

            Random rand = new Random(23423);
            ArrayList list = new ArrayList(); 
            for (int i = 0; i < 100; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < i; j++)
                {
                    sb.Append(rand.NextDouble() + "+");
                }
                sb.Append(rand.NextDouble());
                Formula f = new Formula(sb.ToString());
                Assert.IsFalse(list.Contains(f.GetHashCode()));
                list.Add(f.GetHashCode());
                currHashCode = f.GetHashCode();
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        ///Formula Errors
        /////////////////////////////////////////////////////////////////////////////////


        [TestMethod]
        public void TestTooFewOperatorsAddition()
        {
            Func<string, double> lookup = s => 0;
            Formula instance = new Formula("3.45 +");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));
        }

        [TestMethod]
        public void TestTooFewOperatorsSubtraction()
        {
            Func<string, double> lookup = s => 0;
            Formula instance = new Formula("3.45 -");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));

            instance = new Formula("(3.45 - )");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));
        }

        [TestMethod]
        public void TestMismatchedParentheses()
        {
            Func<string, double> lookup = s => 0;
            Formula instance = new Formula("3.45 - 43)");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));

            instance = new Formula("3.45 +)");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));

            instance = new Formula("(3.45 +))");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));

            instance = new Formula("(3.45 +))))))))))))))))))))))");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));
        }

        [TestMethod]
        public void TestTooManyOrTooFewVariables()
        {
            Func<string, double> lookup = s => 0;
            Formula instance = new Formula("45 A568");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));

            instance = new Formula("");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));
        }

        [TestMethod]
        public void TestTooManyOperators()
        {
            Func<string, double> lookup = s => 0;
            Formula instance = new Formula("+ 544 -");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));

            instance = new Formula("4553 + - z");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));
        }

        [TestMethod]
        public void TestDivideByZero()
        {
            Func<string, double> lookup = s => 0;
            Formula instance = new Formula("5/0");
            Assert.IsInstanceOfType(instance.Evaluate(lookup), typeof(FormulaError));
        }

        /////////////////////////////////////////////////////////////////////////////////
        ///Exception Handling
        /////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidVariableException()
        {
            Func<string, double> lookup = s => 0;
            Func<string, bool> validator = s => false;
            Formula instance = new Formula("X + 544", null, validator);
        }
    }
}
