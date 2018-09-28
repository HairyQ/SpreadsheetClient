using System;
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

            spread.SetCellContents("A", "Here is a string");
            spread.SetCellContents("B", new Formula("A1 + 45 - 553.94"));
            spread.SetCellContents("C", 45.32);
            spread.SetCellContents("D", ""); //Empty Cell

            int errorCounter = 0;

            foreach(string s in spread.GetNamesOfAllNonemptyCells())
            {
                if (s.Equals("D"))
                    errorCounter++;
            }

            Assert.IsTrue(errorCounter == 0);
        }

        [TestMethod]
        public void TestGetCellContentsBasic()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetCellContents("A", "Here is a string");
            spread.SetCellContents("B", new Formula("A1 + 45 - 553.94"));
            spread.SetCellContents("C", 45.32);
            spread.SetCellContents("D", ""); //Empty Cell

            Assert.AreEqual(spread.GetCellContents("A"), "Here is a string");
            Assert.IsTrue((Formula)spread.GetCellContents("B") == new Formula("A1 + 45 - 553.94"));
            Assert.AreEqual(spread.GetCellContents("C"), 45.32);
            Assert.AreEqual(spread.GetCellContents("D"), "");
            Assert.AreEqual(spread.GetCellContents("E"), ""); //New Cell
        }

        [TestMethod]
        public void TestSetCellContentsWithString()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetCellContents("A", "Here is a regular string");
            Assert.AreEqual("Here is a regular string", spread.GetCellContents("A"));
            spread.SetCellContents("A", "Different string");
            Assert.AreEqual("Different string", spread.GetCellContents("A"));
        }

        [TestMethod]
        public void TestSetCellContentsWithFormula()
        {
            Spreadsheet spread = new Spreadsheet();

            spread.SetCellContents("A", new Formula("1 + 45 + A343"));
            Assert.IsTrue((Formula)spread.GetCellContents("A") == new Formula("1 + 45 + A343"));
            spread.SetCellContents("A", new Formula("A4 + B3 - C643_"));
            Assert.IsTrue((Formula)spread.GetCellContents("A") == new Formula("A4 + B3 - C643_"));
        }

        [TestMethod]
        public void TestSetCellContentsWithDouble()
        {
            Spreadsheet spread = new Spreadsheet();
            double d = 45.6;

            spread.SetCellContents("A", d);
            Assert.AreEqual(d, spread.GetCellContents("A"));
            spread.SetCellContents("A", 4312.566);
            Assert.AreEqual(4312.566, spread.GetCellContents("A"));
        }

        [TestMethod]
        public void TestGetDirectDependents()
        {

        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //                                  Exception testing
        ///////////////////////////////////////////////////////////////////////////////////////

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsInvalidNameException()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.GetCellContents("789s");
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
        public void TestSetCellContentsInvalidNameException()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.SetCellContents("789s", 45.3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsNullNameException()
        {
            Spreadsheet spread = new Spreadsheet();
            spread.SetCellContents(null, "Here");
        }

        //TODO
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetDirectDependentsArgumentNullException()
        {
            Spreadsheet spread = new Spreadsheet();
        }
    }
}
