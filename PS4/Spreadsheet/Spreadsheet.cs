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
        /// <summary>
        /// This will be modified to fit specifications for PS5
        /// 
        /// Cell class for setting and getting cell's contents which could be of type Formula, String or Double,
        /// as well as determining Cell's name
        /// </summary>
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

            public void SetContents(object o)
            {
                contents = o;
            }
        }

        private Dictionary<string, Cell> allCells;

        private DependencyGraph dependencies;

        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        /// <summary>
        /// Zero-argument public constructor
        /// </summary>
        public Spreadsheet()
        {
            allCells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
        }

        /// <summary>
        /// Public accessor for cell contents
        /// </summary>
        /// <param name="name">Name of this cell</param>
        /// <returns>Contents of a cell, whether the contents be of type String, Double, or Formula</returns>
        public override object GetCellContents(string name)
        {
            CheckIfNullOrInvalidVariableName(name);

            if (!allCells.ContainsKey(name))
            {
                return "";
            }

            return allCells[name].GetContents();
        }

        /// <summary>
        /// Numerates through allCells, the collection of Cells that have thus far been initialized, 
        /// or "changed" in the context of a spreadsheet
        /// </summary>
        /// <returns>IENumerable object numerating all "changed" (initialized) cells</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (String name in allCells.Keys)
            {
                yield return name;
            }
        }

        /// <summary>
        /// Sets the contents of a cell, given that cell's name, to that cell's value in the
        /// form of a double
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="number">Double Value of Cell</param>
        /// <returns>HashSet containing cell's name and the names of all dependent cells to that cell</returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellHelper(name, number);
        }

        /// <summary>
        /// Sets the contents of a cell, given that cell's name, to that cell's value in the
        /// form of a string
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="text">String Value for Cell</param>
        /// <returns>HashSet containing cell's name and the names of all dependent cells to that cell</returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            return SetCellHelper(name, text);
        }

        /// <summary>
        /// Sets the contents of a cell, given that cell's name, to that cell's value in the
        /// form of a Formula
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="formula">Formula Value for Cell</param>
        /// <returns>HashSet containing cell's name and the names of all dependent cells to that cell</returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException();

            foreach (String s in formula.GetVariables())
            {
                dependencies.AddDependency(s, name);
            }

            return SetCellHelper(name, formula);
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
                throw new ArgumentNullException();

            CheckIfNullOrInvalidVariableName(name);

            if (GetCellContents(name).GetType() == typeof(Formula))
            {
                foreach (string s in ((Formula)GetCellContents(name)).GetVariables())
                {
                    yield return s;
                }
            }
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
            CheckIfNullOrInvalidVariableName(name);

            if (allCells.ContainsKey(name))
            {
                allCells[name].SetContents(contents);
            }
            else
            {
                if (contents.Equals("")) //Has no variables, thus no dependents
                {
                    return new HashSet<string>() { name };
                }
 
                Cell newCell = new Cell(name);
                newCell.SetContents(contents);
                allCells.Add(name, newCell);
            }

            HashSet<string> retSet = new HashSet<string>();
            retSet.Add(name);   //The Cell's own name should be in the Set
            foreach (String s in dependencies.GetDependents(name)) //Add each dependent of Cell
            {
                retSet.Add(s);
            }

            return retSet;
        }

        /// <summary>
        /// Helper method to improve readability - determines if name isnull or not a valid variable name - 
        /// throws exception in either case
        /// </summary>
        /// <param name="name">Cell name</param>
        private void CheckIfNullOrInvalidVariableName(string name)
        {
            if (name == null) //name is null
                throw new InvalidNameException();

            //Regex for finding valid variable names
            if (!Regex.IsMatch(name, @"^[A-Za-z_]{1,}[A-Za-z_|\d]*$"))
                throw new InvalidNameException();
        }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            throw new NotImplementedException();
        }
    }
}
