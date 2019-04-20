using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    /// <summary>
    /// Keeps track of variables that need to be passed between GUI and controller for the sake of 
    /// separating concerns
    /// </summary>
    public class StaticState
    {
        public StaticState()
        {
            spreadsheets = new List<string>();
        }

        private List<string> spreadsheets;

        public List<string> Spreadsheets
        {
            get { return spreadsheets; }
            set { spreadsheets = value; }
        }

        private int row, col;

        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Col
        {
            get { return col; }
            set { col = value; }
        }

        private string contents;

        public string Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        private string cellName;

        public string CellName
        {
            get { return cellName; }
            set { cellName = value; }
        }
    }
}
