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
    }
}
