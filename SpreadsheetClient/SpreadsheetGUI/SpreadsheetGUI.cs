﻿using SpreadsheetUtilities;
using SS;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Controller;
using Resources;
using System.Collections;
using System.ComponentModel;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        /// <summary> Instance of the Spreadsheet to provide Model for the grid </summary>
        SS.Spreadsheet sheet;

        StaticState state;

        ClientController controller;

        bool contentsChanged = false;

        /// <summary> bool determining whether or not data has NOT been saved </summary>
        bool isChanged;

        Form F;

        public Form1(ClientController Controller, StaticState ss, Form f)
        {
            InitializeComponent();

            controller = Controller;

            state = ss;

            controller.RegisterSpreadsheetEditHandler(ChangeCellContents);
            controller.RegisterErrorHandler(circularErrorDisplay);
            controller.RegisterLoginErrorHandler(loginErrorDisplay);

            //Register displaySelection as listener to selectionChanged
            spreadsheetPanel1.SelectionChanged += displaySelection;

            //Select cell A1
            spreadsheetPanel1.SetSelection(0, 0);

            //Move cursor to ContentsField
            cellContentsField.Focus();

            //create backing structure
            sheet = new SS.Spreadsheet(s => Regex.IsMatch(s, "^[A-Z]{1}[1-9]{1}[0-9]?$"), s => s.ToUpper(), "ps6");

            //IsChanged is false initially
            isChanged = false;

            FormClosing += new FormClosingEventHandler(Options_OnClosing);

            F = f;
        }


        /// <summary>
        /// Every time the selection changes, this method is called with the
        /// Spreadsheet as its parameter. CellName, CellValue, CellContents fields
        /// are updated accordingly.
        /// </summary>
        /// <param name="ss"></param>
        private void displaySelection(SpreadsheetPanel ss)
        {
            //get current position
            cellContentsField.Focus();

            ss.GetSelection(out int col, out int row);

            string name = GetCellName(col, row);

            UpdateGUIFields(name);
        }
        /// <summary>
        /// Given cell name, moves cursor to contents field,
        /// and updates name, contents, and value fields
        /// with values corresponding to cell
        /// </summary>
        /// <param name="name"></param>
        private void UpdateGUIFields(string name)
        {
            //move cursor to contents field
            cellContentsField.Focus();

            cellNameField.Text = name;

            //set CellContentsField from spreadsheet
            if (sheet.GetCellContents(name) is Formula)
                cellContentsField.Text = "=" + sheet.GetCellContents(name).ToString();
            else
                cellContentsField.Text = sheet.GetCellContents(name).ToString();

            cellValueField.Text = sheet.GetCellValue(name) + "";

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
            if (contentsChanged == false)
                return;

            //get current cell
            spreadsheetPanel1.GetSelection(out int col, out int row);

            //extract contents
            string contents = cellContentsField.Text;
            SendEdit(col, row, contents);
        }

        private bool isValid(string s)
        {
            if (Regex.IsMatch(s, @"^[A-Za-z]{1}[\d]{1,2}$"))
                return true;
            return false;
        }

        /// <summary>
        /// takes string contents and column and row number,
        /// converts col and row to a cell value, adds cell
        /// to backing sheet and update gui accordingly
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="contents"></param>
        private void SendEdit(int col, int row, string contents)
        {
            string name = GetCellName(col, row);

            isChanged = sheet.Changed;

            EditMessage newEdit = new EditMessage();
            newEdit.cell = name;
            if (contents.Length > 0 && contents[0] == '=')
            {
                try
                {
                    Formula newFormula = new Formula(contents.Substring(1, contents.Length - 1), s => s.ToUpper(), isValid);
                }
                catch (FormulaFormatException ffe)
                {
                    MessageBox.Show(ffe.Message, "Formula Format Error");
                    string s = ffe.Message;
                }
                string pattern = @"[A-Za-z]{1}[\d]{1,2}";
                
                ArrayList dependencies = new ArrayList();

                foreach (Match m in Regex.Matches(contents, pattern))
                {
                    dependencies.Add(m.ToString());
                }

                newEdit.dependencies = dependencies;
            }
            else
            {
                double cellDouble;
                if (Double.TryParse(contents, out cellDouble))
                {
                    EditMessageDoubleType actualMessage = new EditMessageDoubleType();
                    actualMessage.value = cellDouble;
                    actualMessage.dependencies = new ArrayList();
                    actualMessage.cell = name;
                    controller.SendMessage(actualMessage);
                    return;
                }

                newEdit.dependencies = new ArrayList();
            }
            newEdit.value = contents;
            
            if (contentsChanged)
                controller.SendMessage(newEdit);

            contentsChanged = false;
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// I added this method with the intent it be used for when cell change messages are 
        /// received from the server
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="contents"></param>
        private void ChangeCellContents()
        {
            this.Invoke((MethodInvoker)delegate
            {
                string name = state.CellName;
                string value = "";
                try
                {
                    //store contents in backing sheet
                    if (state.Contents.Length > 0 && state.Contents[0] == '=')
                        sheet.SetContentsOfCell(name, state.Contents.ToUpper());
                    else
                        sheet.SetContentsOfCell(name, state.Contents);

                    object valueObj = "";
                    //get value of cell from backing sheet
                    if (sheet.GetCellContents(name) is String && !sheet.GetCellContents(name).Equals(""))
                        valueObj = sheet.GetCellValue(name);

                    //handle formula errors
                    if (valueObj.GetType().Equals(typeof(FormulaError)))
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
                    value = ffe.Message;
                }

                cellValueField.Text = value;
                spreadsheetPanel1.SetValue(state.Col, state.Row, value);

                try
                {
                    foreach (string s in sheet.CheckValues(name))
                    {
                        int row, col;
                        Int32.TryParse(s.Substring(1, s.Length - 1), out row);
                        col = s[0] - 65;
                        spreadsheetPanel1.SetValue(col, row - 1, sheet.GetCellValue(s).ToString());
                        cellValueField.Text = sheet.GetCellValue(s) + "";
                        UpdateGUIFields(s);
                    }
                }
                catch (InvalidCastException e)
                {
                    MessageBox.Show("You can't do that");
                }

                updateCells(name);

                contentsChanged = false;
            });
        }

        private void updateCells(string name)
        {
            foreach (string s in sheet.getTheDependents(name))
            {
                int col = name[0] - 65;
                int row;
                Int32.TryParse(s.Substring(1, name.Length - 1), out row);
                spreadsheetPanel1.SetValue(col, row - 1, sheet.GetCellValue(s).ToString());
            }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            string name, contents;

            //keypress handling logic
            switch (keyData)
            {
                case Keys.Up:
                    contents = cellContentsField.Text;
                    if (contentsChanged == true)
                        SendEdit(col, row, contents);
                    row--;
                    break;
                case Keys.Enter:
                case Keys.Down:
                    contents = cellContentsField.Text;
                    if (contentsChanged == true)
                        SendEdit(col, row, contents);
                    row++;
                    break;
                case Keys.Left:
                    contents = cellContentsField.Text;
                    if (contentsChanged == true)
                        SendEdit(col, row, contents);
                    col--;
                    break;
                case Keys.Tab:
                case Keys.Right:
                    contents = cellContentsField.Text;
                    if (contentsChanged == true)
                        SendEdit(col, row, contents);
                    col++;
                    break;
                //pass key event onto regular form handling
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }

            //update selection
            if (spreadsheetPanel1.SetSelection(col, row))
            {
                name = GetCellName(col, row);
                UpdateGUIFields(name);
            }

            //event has been handled
            return true;
        }

        /// <summary>
        /// Handles closing the spreadsheet. Displays a message box warning about data loss if the 
        /// spreadsheet has not been saved but has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// No idea why this needs to exist but I guess it does. Deleting it will break the app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {
        }

        private void cellContentsField_TextChanged(object sender, EventArgs e)
        {
            contentsChanged = true;
        }

        private void circularErrorDisplay()
        {
            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show("Circular Dependency detected at cell " + state.CellName + " change not accepted");
            });
        }

        private void loginErrorDisplay()
        {
            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show("Incorrect username and/or password");
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UndoMessage newUndo = new UndoMessage();

            controller.SendMessage(newUndo);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int col, row;
            spreadsheetPanel1.GetSelection(out col, out row);
            RevertMessage newRevert = new RevertMessage();
            newRevert.cell = GetCellName(col, row);

            controller.SendMessage(newRevert);
        }

        private void Options_OnClosing(object sender, FormClosingEventArgs e)
        {
            if (Application.MessageLoop)
            {
                Application.Exit();
            }
            else
            {
                Environment.Exit(1);
            }
        }
    }
}
