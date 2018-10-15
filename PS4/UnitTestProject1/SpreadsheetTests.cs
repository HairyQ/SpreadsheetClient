using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;

namespace UnitTestProject1
{
    [TestClass]
    public class SpreadsheetTests : Spreadsheet
    {
        [TestMethod]
        public void TestGetNameOfAllNonemptyCellsBasic()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetContentsOfCell("A1", "Here is a string");
            spread.SetContentsOfCell("B1", "=A1 + 45 - 553.94");
            spread.SetContentsOfCell("C1", "45.32");
            spread.SetContentsOfCell("D1", ""); //Empty Cell

            int errorCounter = 0;

            foreach(string s in spread.GetNamesOfAllNonemptyCells())
            {
                if (s.Equals("D1"))
                    errorCounter++;
            }

            Assert.IsTrue(errorCounter == 0);
        }

        [TestMethod]
        public void TestGetCellContentsBasic()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetContentsOfCell("A1", "Here is a string");
            spread.SetContentsOfCell("B1", "=A1 + 45 - 553.94");
            spread.SetContentsOfCell("C1", "45.32");
            spread.SetContentsOfCell("D1", ""); //Empty Cell

            Assert.AreEqual(spread.GetCellContents("A1"), "Here is a string");
            Assert.IsTrue((Formula)spread.GetCellContents("B1") == new Formula("A1 + 45 - 553.94"));
            Assert.AreEqual(spread.GetCellContents("C1"), 45.32);
            Assert.AreEqual(spread.GetCellContents("D1"), "");
            Assert.AreEqual(spread.GetCellContents("E1"), ""); //New Cell
        }

        [TestMethod]
        public void TestSetContentsOfCellWithString()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetContentsOfCell("A1", "Here is a regular string");
            Assert.AreEqual("Here is a regular string", spread.GetCellContents("A1"));
            spread.SetContentsOfCell("A1", "Different string");
            Assert.AreEqual("Different string", spread.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWithFormula()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetContentsOfCell("A1", "=1 + 45 + A343");
            Assert.IsTrue((Formula)spread.GetCellContents("A1") == new Formula("1 + 45 + A343"));
            spread.SetContentsOfCell("A1", "=A4 + B3 - C643");
            Assert.IsTrue((Formula)spread.GetCellContents("A1") == new Formula("A4 + B3 - C643"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWithDouble()
        {
            Spreadsheet spread = new Spreadsheet();
            double d = 45.6;

            spread.SetContentsOfCell("A1", d.ToString());
            Assert.AreEqual(d, spread.GetCellContents("A1"));
            spread.SetContentsOfCell("A1", "4312.566");
            Assert.AreEqual(4312.566, spread.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestSetContentsOfCellWithDependencies()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.SetContentsOfCell("C1", "=A1 + B1");
            spread.SetContentsOfCell("B1", "=A1 + 3");

            HashSet<String> hS = new HashSet<string>() { "A1", "B1", "C1" };
            Assert.IsTrue(spread.SetContentsOfCell("A1", "45").SetEquals(hS));

            spread.SetContentsOfCell("D1", "=A1 + B1 + C1");
            hS.Add("D1");
            Assert.IsTrue(spread.SetContentsOfCell("A1", "456").SetEquals(hS));
            
            hS.Remove("A1");
            hS.Remove("B1");
            hS.Remove("C1");
            Assert.IsTrue(spread.SetContentsOfCell("D1", "=A1 + B1 + C1").SetEquals(hS));

            hS.Add("E1");
            Assert.IsFalse(spread.SetContentsOfCell("E1", "Random String").SetEquals(hS));
        }

        [TestMethod]
        public void TestGetDirectDependentsBasic()
        {
            Spreadsheet spread = new Spreadsheet();
            PrivateObject spreadAccessor = new PrivateObject(spread);

            spread.SetContentsOfCell("A1", "=D1 + 34 + B1");
            spread.SetContentsOfCell("B1", "=23423 + D1");
            spread.SetContentsOfCell("D1", "=Z234 / 5");

            int counter = 0; //counter for keeping track of the number of variables in the IEnumberable
            HashSet<string> expectedVariables = new HashSet<string>() {"D1", "B1"};
            foreach (string s in (IEnumerable<string>)spreadAccessor.Invoke("GetDirectDependents", new string[1] {"A1"}))
            {
                Assert.IsTrue(expectedVariables.Contains(s));
                counter++;
            }
            Assert.IsTrue(counter == 2);
        }

        [TestMethod]
        public void TestGetCellValueBasic()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetContentsOfCell("A1", "1");
            spread.SetContentsOfCell("B1", "2");
            spread.SetContentsOfCell("C1", "3");
            spread.SetContentsOfCell("D1", "=A1 + B1 + C1");

            Assert.AreEqual(spread.GetCellValue("D1"), 6.0);
        }

        [TestMethod]
        public void TestXMLReaderWriterBasic()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetContentsOfCell("A1", "1");

            spread.Save("xmlfile");
            Assert.AreEqual(spread.GetSavedVersion("xmlfile"), "default");

            spread = new Spreadsheet(s => true, s => s, "RANDOM_STRING");
            spread.SetContentsOfCell("A1", "=76 + B2");
            spread.SetContentsOfCell("B2", "43");

            spread.Save("RANDOM_FILE");
            Assert.AreEqual(spread.GetSavedVersion("RANDOM_FILE"), "RANDOM_STRING");
        }

        [TestMethod]
        public void TestXMLReaderWriterWithClasspath()
        {
            Spreadsheet spread = new Spreadsheet("C:/Users/hquic/Documents/", s => true, s => s, "1.0");
            spread.SetContentsOfCell("A1", "=76 + B2");
            spread.SetContentsOfCell("B2", "43");

            spread.Save("xmlfile.xml");
            Assert.AreEqual(spread.GetSavedVersion("xmlfile.xml"), "1.0");
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //                                  Exception testing
        ///////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsInvalidNameException()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.GetCellContents("789_*s");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsNullNameException()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellInvalidNameException()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.SetContentsOfCell("789s", "45.3");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellNullNameException()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.SetContentsOfCell(null, "Here");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testUnbalancedParentheses()
        {
            Formula form = new Formula("(A3 + 4");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullArgumentSetContentsOfCellWithString()
        {
            Spreadsheet spread = new Spreadsheet();
            string newString = null;
            spread.SetContentsOfCell("A1", newString);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNoSuchFileException()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.GetSavedVersion("randomFile");
        }
    }
}
