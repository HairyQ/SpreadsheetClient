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
        Spreadsheet sheet;

        public Form1()
        {
            InitializeComponent();

            //Register displaySelection as listener to selectionChanged
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);

            //enter acts as set button
            this.AcceptButton = setButton;

            sheet = new Spreadsheet(s => Regex.IsMatch(s, "^[A-Z]{1}[1-99]{1}$"), s => s.ToUpper(), "ps6");
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

            //set CellContents from spreadsheet
            if (sheet.GetCellContents(name) is SpreadsheetUtilities.Formula)
                cellContentsField.Text = "=" + sheet.GetCellContents(name).ToString();
            else
                cellContentsField.Text = sheet.GetCellContents(name).ToString();

            //set CellValue from spreadsheet
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
            spreadsheetPanel1.GetSelection(out int col, out int row);

            string name = GetCellName(col, row);
            string contents = cellContentsField.Text;

            sheet.SetContentsOfCell(name, contents);

            string value = sheet.GetCellValue(name).ToString();

            //update gui representation
            cellValueField.Text = value;
            spreadsheetPanel1.SetValue(col, row, value);
        }
    }
}