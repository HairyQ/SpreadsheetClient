using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.Program;

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
            sheet = new SS.Spreadsheet(s => Regex.IsMatch(s, "^[A-Z]{1}[1-99]{1}$"), s => s.ToUpper(), "ps6");
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
            spreadsheetPanel1.GetSelection(out int col, out int row);

            string name = GetCellName(col, row);
            string contents = cellContentsField.Text;
            string value = "";

            try
            {
                sheet.SetContentsOfCell(name, contents);

                object valueObj = sheet.GetCellValue(name);

                if (valueObj is Type FormulaError)
                {
                    FormulaError fe = (FormulaError)valueObj;
                    value = fe.Reason;
                }
                else
                    value = valueObj.ToString();
            }
            catch (Exception ffe)
            {
                //create error message window
                MessageBox.Show(ffe.Message, "Formula Format Error");
                string s = ffe.Message;
            }

            //string value = sheet.GetCellValue(name).ToString();


            //update gui representation
            cellValueField.Text = value;
            spreadsheetPanel1.SetValue(col, row, value);
        }

        private void openFileMenu(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Creates a new window with a fresh spreadsheet, running on the same thread as the current sheet
        /// </summary>
        /// <param name="sender">"new" menu button object that sent the event</param>
        /// <param name="e">Event to be handled</param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetApplicationContext.GetAppContext().RunForm(new Form1());
        }

        /// <summary>
        /// Handles closing the spreadsheet. Displays a message box warning about data loss if the 
        /// spreadsheet has not been saved but has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sheet.Changed)
            {
                DialogResult result = MessageBox.Show("WARNING:\n\nSAll unsaved changes will be lost" +
                    "\n\nClose anyway?", "Unsaved Changes", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    Close();
            }
        }
    }
}