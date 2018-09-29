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

            public Object GetContents()
            {
                return contents;
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

            public void SetContents(object o)
            {
                contents = o;
            }
        }

        private Dictionary<string, Cell> allCells;

        private DependencyGraph dependencies;

        /// <summary>
        /// Zero-argument public constructor
        /// </summary>
        public Spreadsheet()
        {
            allCells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
        }

        public override object GetCellContents(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidNameException();
            
            //Regex for matching to variables
            if (!Regex.IsMatch(name, @"^[A-Za-z_]{1,}[A-Za-z_|\d]*$"))
                throw new InvalidNameException();

            if (!allCells.ContainsKey(name))
            {
                return "";
            }

            return allCells[name].GetContents();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (String name in allCells.Keys)
            {
                yield return name;
            }
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellHelper(name, number);
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            return SetCellHelper(name, text);
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            foreach (String s in formula.GetVariables())
            {
                dependencies.AddDependency(s, name);
            }

            return SetCellHelper(name, formula);
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper method for setting the contents of any cell - uses an object parameter to 
        /// allow any type of object to be passed in. Should help improve readability
        /// </summary>
        /// <param name="name">Name of the cell to be added or changed</param>
        /// <param name="contents"></param>
        /// <returns></returns>
        private ISet<string> SetCellHelper(string name, object contents)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidNameException();

            //Regex for matching to variables
            if (!Regex.IsMatch(name, @"^[A-Za-z_]{1,}[A-Za-z_|\d]*$"))
                throw new InvalidNameException();
            
            if (allCells.ContainsKey(name))
            {
                allCells[name].SetContents(contents);
            }
            else
            {
                if (contents.Equals(""))
                {
                    return new HashSet<string>() { name };
                }
 
                Cell newCell = new Cell(name);
                newCell.SetContents(contents);
                allCells.Add(name, newCell);
            }

            HashSet<string> retSet = new HashSet<string>();
            retSet.Add(name);   //The Cell's own name should be in the Set
            foreach (String s in dependencies.GetDependents(name))
            {
                retSet.Add(s);
            }

            return retSet;
        }
    }
}
