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
        /// Cell class for setting and getting cell's contents which could be of type Formula, String or Double,
        /// as well as determining Cell's name
        /// </summary>
        public class Cell
        {
            private String name;
            private Object contents;
            private Object value;

            public Cell(String newName) { name = newName; contents = ""; } //Cell constructor

            public Object GetContents() { return contents; }

            public void SetContents(object o) { contents = o; }
        }

        /// <summary>
        /// Dictionary mapping cell names to cell objects.
        /// Contains all cells whose values have been 'changed', thus a list of all initialized cells
        /// </summary>
        private Dictionary<string, Cell> allCells;

        /// <summary>
        /// Dependency graph representing dependencies between cells according to their formulae
        /// </summary>
        private DependencyGraph dependencies;

        /// <summary>
        /// Field determining whether or not this version of this Spreadsheet has been 'changed'
        /// </summary>
        public override bool Changed { get => changed;
            protected set => changed = value; }

        /// <summary>
        /// Native bool determining whether or not this version of this spreadsheet has been changed
        /// </summary>
        private bool changed;

        /// <summary>
        /// Zero-argument public constructor
        /// 
        /// This constructor passes a default validator, normalizer, and version to AbstractSpreadsheet's constructor.
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            allCells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
            changed = false;
        }

        /// <summary>
        /// Three-argument constructor
        /// 
        /// Allows user to define validator and normalizer delegates, and defines current version of Spreadsheet
        /// </summary>
        /// <param name="isValid">Function that returns bool based on validity of variable name</param>
        /// <param name="normalize">Function that returns a "normalized" version of a variable name</param>
        /// <param name="version">String representation of the current version of this spreadsheet</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) 
            : base(isValid, normalize, version)
        {
            allCells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
            changed = false;
        }


        /// <summary>
        /// Four-argument constructor
        /// 
        /// Allows user to defind validator and normalzier delegates, and defines current version of Spreadsheet
        /// as well as defines a file path for saving this version of the Spreadsheet
        /// </summary>
        /// <param name="filePath">Path defining where the Spreadsheet should be saved</param>
        /// <param name="isValid">Function that returns bool based on validity of variable name</param>
        /// <param name="normalize">Function that returns a "normalized" version of a variable name</param>
        /// <param name="version">String representation of the current version of this spreadsheet</param>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            allCells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
            changed = false;
        }

        /// <summary>
        /// Public accessor for cell contents
        /// </summary>
        /// <param name="name">Name of this cell</param>
        /// <returns>Contents of a cell, whether the contents be of type String, Double, or Formula</returns>
        public override object GetCellContents(string name)
        {
            name = Normalize(name); //Normalize name according to user's normalizer
            CheckIfNullOrInvalidVariableName(name);

            if (!allCells.ContainsKey(name)) //Empty cell implies 'empty string' contents
                return "";

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
        protected override ISet<string> SetCellContents(string name, double number)
        { return SetCellHelper(name, number); }

        /// <summary>
        /// Sets the contents of a cell, given that cell's name, to that cell's value in the
        /// form of a string
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="text">String Value for Cell</param>
        /// <returns>HashSet containing cell's name and the names of all dependent cells to that cell</returns>
        protected override ISet<string> SetCellContents(string name, string text)
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException();

            foreach (String s in formula.GetVariables())
            {
                dependencies.AddDependency(s, name);
            }

            return SetCellHelper(name, formula);
        }

        /// <summary>
        /// Finds the dependents of a given cell that are considered "direct" dependents (the variables 
        /// directly referenced by the formula in this cell)
        /// </summary>
        /// <param name="name">Name of cell</param>
        /// <returns>An Enumeration of the direct dependents of cell's formula</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
                throw new ArgumentNullException();

            name = Normalize(name); //Normalize name of cell according to user
            CheckIfNullOrInvalidVariableName(name); //Check validity of cell name

            if (GetCellContents(name).GetType() == typeof(Formula))
            {
                foreach (string s in ((Formula)GetCellContents(name)).GetVariables())
                {
                    yield return s;
                }
            }
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

        /// <summary>
        /// Public accessor method for setting the contents of any cell to a Formula, Double, or String.
        /// First, normalizes and checks the validity of name, then parses content into whatever type
        /// is implied by its string representation (Formula, Double, or String), then passes name and
        /// content to the appropriate setCellContents() helper method (according to content's type), 
        /// and returns the resulting Set of cells that depend, directly or indirectly, on (name's) cell
        /// </summary>
        /// <param name="name">Name of given cell</param>
        /// <param name="content">Cell's new contents</param>
        /// <returns></returns>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            changed = true; //Spreadsheet has now officially been changed
            double newDouble;   //Initialized double in case content is of type double

            name = Normalize(name); //Normalize name according to user's normalize delegate or default Function
            CheckIfNullOrInvalidVariableName(name);

            if (content == "")  //Corner case - if we try to check its characters, exception will be thrown
                return SetCellContents(name, content);

            if (content.ToCharArray()[0] == '=')                    //Content is Formula
                return SetCellContents(name, new Formula(content.Substring(1), Normalize, IsValid));

            else if (Double.TryParse(content, out newDouble))       //Content is Double
                return SetCellContents(name, newDouble);

            else                                                    //Content is String
                return SetCellContents(name, content);
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
            if (!Regex.IsMatch(name, @"^[A-Za-z]+[\d]+$"))
                throw new InvalidNameException();

            if (!IsValid(name)) //name is invalid according to user
                throw new InvalidNameException();
        }
    }
}
