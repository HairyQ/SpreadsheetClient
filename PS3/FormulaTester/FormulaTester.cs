using System;
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
        /// Creates a new instance of a formula object - tests to see that 
        /// </summary>
        [TestMethod]
        public void TestConstructorSetDelegates()
        {
            Func<string, string> normalizer = s => s.ToLower(); //Converts each string to lowercase
            bool validator (string s)
            {
                if (s.Equals("abcd1fdgdgjkt"))
                    return true;
                return false;
            }
            Func<string, double> lookupDel = s => 4.0;

            //No assertion - the validator just shouldn't throw an exception:
            Formula instance = new Formula("ABCD1FDGdgjkT + 1", normalizer, validator);

            //Again, should not throw an exception
            instance = new Formula("ABCD1FDGdgjkT + 1");
            instance = new Formula("ABCD1FDGdgjkT + 1", normalizer, null);
            instance = new Formula("ABCD1FDGdgjkT + 1", null, validator);
        }

        [TestMethod]
        public void TestEvaluateSimpleAdditionSpacesVariables()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("1 + 1");
            Assert.AreEqual("2", formulaInstance.Evaluate(lookup).ToString());

            formulaInstance = new Formula("1+1");
            Assert.AreEqual("2", formulaInstance.Evaluate(lookup).ToString());

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
            Assert.AreEqual("8", formulaInstance.Evaluate(lookup).ToString());

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
            Assert.AreEqual("19353.29034", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestEvaluateComplexDivisionWithVariables()
        {
            Func<string, double> lookup = LookupDel;

            //A_3 = 3.2
            //AD488 = 4.99992

            Formula formulaInstance = new Formula("((((A_3/7)/5.4 / AD488 / (8 / 4))))");
            Assert.AreEqual("0.008465743918", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestComplexEquationsNoVariables()
        {
            Func<string, double> lookup = s => 0;

            Formula formulaInstance = new Formula("8.6 - (64 / (5 + 3.9) * 3)");
            Assert.AreEqual("-12.97303371", formulaInstance.Evaluate(lookup).ToString());
        }
        
        [TestMethod]
        public void TestComplexEquationsWithVariables()
        {
            double LookupDel(string s)
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

            Func<string, double> lookup = LookupDel;

            Formula formulaInstance = new Formula("8.6 - (dfsa6f / (5 + GHE_43) * 3)");
            Assert.AreEqual("-12.97303371", formulaInstance.Evaluate(lookup).ToString());
        }

        [TestMethod]
        public void TestGetVariablesBasic()
        {


        }

        [TestMethod]
        public void TestToString()
        {


        }

        [TestMethod]
        public void TestEquals()
        {


        }

        [TestMethod]
        public void TestEqualsOperator
            
            ()
        {


        }

        [TestMethod]
        public void TestNotEqualOperator()
        {


        }

        [TestMethod]
        public void TestGetHashCode()
        {


        }

    }
}
