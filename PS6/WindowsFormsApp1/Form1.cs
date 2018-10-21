using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    
    public partial class Form1 : Form
    {
        SS.Spreadsheet sheet;

        public Form1()
        {
            InitializeComponent();

            //Register displaySelection as listener to selectionChanged
            spreadsheetPanel1.SelectionChanged += displaySelection;

            //Select cell A1
            spreadsheetPanel1.SetSelection(0, 0);

            //Move cursor to ContentsField
            cellContentsField.Focus();

            //enter acts as set button
            this.AcceptButton = setButton;

            //create backing structure
            sheet = new SS.Spreadsheet(s => Regex.IsMatch(s, "^[A-Z]{1}[1-9]{1}[0-9]?$"), s => s.ToUpper(), "ps6");
        }

        private void selectionChanged(object sender, EventArgs e)
        {
            MessageBox.Show("selection changed");
        }
        /// <summary>
        /// Every time the selection changes, this method is called with the
        /// Spreadsheet as its parameter. CellName, CellValue, CellContents fields
        /// are updated accordingly.
        /// </summary>
        /// <param name="ss"></param>
        private void displaySelection(SpreadsheetPanel ss)
        {
            cellContentsField.Focus();

            ss.GetSelection(out int col, out int row);

            string name = GetCellName(col, row);

            cellNameField.Text = name;
            
            //set CellContentsField from spreadsheet
            if (sheet.GetCellContents(name) is Formula)
                cellContentsField.Text = "=" + sheet.GetCellContents(name).ToString();
            else
                cellContentsField.Text = sheet.GetCellContents(name).ToString();

            //set CellValueField from spreadsheet
            cellValueField.Text = sheet.GetCellValue(name).ToString();

            //highlight current contents
            cellContentsField.SelectionStart = 0;
            cellContentsField.SelectionLength = cellContentsField.Text.Length;
        }

        /// <summary>
        /// Converts col and row ints from grid to 
        /// corresponding spreadsheet cell name
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetCellName(int col, int row)
        {
            string name = "";

            //convert column index to ASCII char
            //65 offset so 0=A and 26=Z
            char c = (char)(col + 65);

            //Name is char from column followed by int from row
            name += c;
            name += row + 1;

            return name;
        }


        private void setButton_Click(object sender, EventArgs e)
        {
            //get current cell
            spreadsheetPanel1.GetSelection(out int col, out int row);

            //extract contents contents
            string contents = cellContentsField.Text;

            SetCell(col, row, contents);
        }

        /// <summary>
        /// takes string contents and column and row number,
        /// converts col and row to a cell value, adds cell
        /// to backing sheet and update gui accordingly
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="contents"></param>
        private void SetCell(int col, int row,  string contents)
        {
            string name = GetCellName(col, row);
            string value = "";

            try
            {
                //store contents in backing sheet
                sheet.SetContentsOfCell(name, contents);

                //get value of cell from backing sheet
                object valueObj = sheet.GetCellValue(name);

                //handle formula errors
                if (valueObj is Type FormulaError)
                {
                    FormulaError fe = (FormulaError)valueObj;
                    value = fe.Reason;
                }

                //store value of cell
                else
                    value = valueObj.ToString();
            }
            catch (FormulaFormatException ffe)
            {
                //create error message window
                MessageBox.Show(ffe.Message, "Formula Format Error");
                string s = ffe.Message;
            }

            //update gui representation
            cellValueField.Text = value;
            spreadsheetPanel1.SetValue(col, row, value);
        }

        private void openFileMenu(object sender, EventArgs e)
        {
            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //capture up arrow key
            if (keyData == Keys.Up)
            {
                spreadsheetPanel1.GetSelection(out int col, out int row);
                row++;
                string name = GetCellName(col, row);
                MessageBox.Show("You pressed Up arrow key");
                return true;
            }
            //capture down arrow key
            if (keyData == Keys.Down)
            {
                MessageBox.Show("You pressed Down arrow key");
                return true;
            }
            //capture left arrow key
            if (keyData == Keys.Left)
            {
                MessageBox.Show("You pressed Left arrow key");
                return true;
            }
            //capture right arrow key
            if (keyData == Keys.Right)
            {
                MessageBox.Show("You pressed Right arrow key");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}