using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet

    {
        public class Cell
        {

            private String name;
            private Object contents;
            private Object value;

            public Cell(String newName)
            {
                name = newName;
                contents = "";
            }

            public void SetContents(Formula formula)
            {
                contents = formula;
            }

            public void SetContents(String sentence)
            {
                contents = sentence;
                value = sentence;
            }

            public void SetContents(Double d)
            {
                contents = d;
                value = d;
            }
        }

        private Dictionary<string, Cell> allCells;

        /// <summary>
        /// Zero-argument public constructor
        /// </summary>
        public Spreadsheet()
        {
            allCells = new Dictionary<string, Cell>();
        }

        public override object GetCellContents(string name)
        {
            //Regex for matching to variables
            if (!Regex.IsMatch(name, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*") || name.Equals(null))
                throw new InvalidNameException();

           
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
    }
}
