using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            }

            public void SetContents(Double d)
            {
                contents = d;
            }
        }

        /// <summary>
        /// Zero-argument constructor
        /// </summary>
        public Spreadsheet()
        {
            Cell c1 = new Cell("");
        }

        public override object GetCellContents(string name)
        {
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
