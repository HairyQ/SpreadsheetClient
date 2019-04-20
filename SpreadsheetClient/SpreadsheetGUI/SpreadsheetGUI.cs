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
using System.Xml;
using Controller;
using Resources;
using static SpreadsheetGUI.Program;
using System.Collections;

namespace SpreadsheetGUI
{
    //Adding comment to commit
    public partial class Form1 : Form
    {
        /// <summary> Instance of the Spreadsheet to provide Model for the grid </summary>
        SS.Spreadsheet sheet;

        StaticState state;

        ClientController controller;

        bool contentsChanged = false;

        /// <summary> bool determining whether or not data has NOT been saved </summary>
        bool isChanged;

        /// <summary> Name of file to be opened / saved </summary>
        string fileName;

        public Form1(ClientController Controller, StaticState ss)
        {
            InitializeComponent();

            controller = Controller;

            state = ss;

            controller.RegisterSpreadsheetEditHandler(ChangeCellContents);

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

            fileName = "Spreadsheet";

            //Register Form1_Closing as listener to Exiting/Closing the sheet
            this.FormClosing += Form1_Closing;
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
                    Console.WriteLine("Contents: " + contents.Substring(1, contents.Length - 1));
                }
                catch (FormulaFormatException ffe)
                {
                    MessageBox.Show(ffe.Message, "Formula Format Error");
                    string s = ffe.Message;
                }
                System.Console.WriteLine("Formula reached");
                string pattern = @"[A-Za-z]{1}[\d]{1,2}";
                
                ArrayList dependencies = new ArrayList();

                foreach (Match m in Regex.Matches(contents, pattern))
                {
                    dependencies.Add(m.ToString());
                    System.Console.WriteLine(m.ToString());
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
                    //get value of cell from backing sheet
                    object valueObj = sheet.GetCellValue(name);
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
                /*

                if (state.Contents.Length > 0 && state.Contents[0] == '=')
                {
                    Formula newFormula = new Formula(state.Contents);
                    sheet.SetContentsOfCell(name, newFormula);
                }
                */
                //set CellValueField from spreadsheet
                cellValueField.Text = value;
            spreadsheetPanel1.SetValue(state.Col, state.Row, value);

            UpdateGUIFields(name);

            contentsChanged = false;
            });
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



        private void openFileMenu(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles closing the spreadsheet. Displays a message box warning about data loss if the 
        /// spreadsheet has not been saved but has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChanged)
            {
                DialogResult result = MessageBox.Show("Warning:\n\nAll unsaved changes will be lost" +
                    "\n\nClose anyway?", "Unsaved Changes", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    Close();
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Saves this spreadsheet to a ".sprd" file locally to the user's computer.
        /// Utilizes Spreadsheet's xmlWriter in the Save method to write this spreadsheet's info to the disk
        /// </summary>
        /// <param name="sender">"Save As" option from the file menu</param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {

                sfd.Title = "Save Spreadsheet Explorer";
                sfd.FileName = fileName;
                sfd.DefaultExt = ".sprd";
                sfd.Filter = "Spreadsheet|*.sprd|All Files|*.*";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    fileName = sfd.FileName;

                    if (sfd.FilterIndex == 1) //User chose option 1: ".sprd files only" is selected
                    {
                        sheet.Save(fileName.Substring(fileName.Length - 5).Equals(".sprd") ? fileName : fileName + ".sprd");
                        isChanged = false;
                    }
                    else
                    {
                        sheet.Save(fileName + ".sprd"); //Append ".sprd" if "All File Types" is selected
                        isChanged = false;
                    }
                }
            }
        }

        /// <summary>
        /// Opens a saved Spreadsheet from the user's disk, and writes the data to the already open 
        /// Spreadsheet, rather than opening a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sheet.Changed)
            {
                DialogResult result = MessageBox.Show("Warning:\n\nAll unsaved changes will be lost" +
                    "\n\nOpen new file anyway?", "Unsaved Changes", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    OpenAndCombineFileViewer("");
                    isChanged = false;
                }
            }
            else
            {
                OpenAndCombineFileViewer("");
                isChanged = false;
            }
        }

        /// <summary>
        /// Helper method that provides an OpenFileDialog for opening a file from the disk
        /// </summary>
        /// <param name="booleanOperator"></param>
        private void OpenAndCombineFileViewer(string booleanOperator)
        {
            /// If sprd file was opened from file explorer, the file path is sent as an argument to main, which
            /// calls openFile() with that file path as a parameter, which calls this method using the same argument
            /// for the parameter. Therefore booleanOperator is a valid filePath for the target file.
            if (!booleanOperator.Equals("") && !booleanOperator.Equals("AND") && !booleanOperator.Equals("OR"))
            {
                openAndCombineHelper(booleanOperator, booleanOperator);
                isChanged = false;
                fileName = Path.GetFileName(booleanOperator);
            }
            else
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Open Spreadsheet";
                    ofd.DefaultExt = ".sprd";
                    ofd.Filter = "Spreadsheets|*.sprd|All Files|*.*";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        //Clear current sheet if opening new file (opposed to combining)
                        if (!booleanOperator.Equals("AND") && !booleanOperator.Equals("OR"))
                            spreadsheetPanel1.Clear();

                        fileName = Path.GetFileName(ofd.FileName);
                        //Populate correct cells from opened file

                        openAndCombineHelper(ofd.FileName, booleanOperator);


                        if (!booleanOperator.Equals("AND") && !booleanOperator.Equals("OR"))
                            isChanged = false;
                    }
                }
            }
        }

        /// <summary>
        /// Helper method that handles the logic behind "combining" spreadsheets
        /// 
        /// If the user has a Spreadsheet open, Spreadsheet A, and they choose the menu option Combine Spreadsheets,
        /// they are given an option between two operators: AND and OR. After choosing an operator, they choose a 
        /// Spreadsheet file, Spreadsheet B. 
        /// 
        /// If the user chooses AND, the contents of occupied cells in Spreadsheet A are checked against their 
        /// corresponding cells in Spreadsheet B. Following the logic of boolean operation, if both cells contain
        /// the same value, Spreadsheet A (the Spreadsheet the user had open originally) retains those cells' 
        /// contents, while all other cells' contents are reset to an empty string. If corresponding cells contain 
        /// different contents, Spreadsheet A's cell's contents are reset to an empty string.
        /// 
        /// If the user chooses OR, the contents are again checked between both Spreadsheets' corresponding cells, 
        /// and if any cells contain contents in either Spreadsheet, Spreadsheet A is amended so that all of those
        /// cells that were populated in Spreadsheet B are also populated in Spreadsheet A, and it contains all of
        /// its cells' original contents. If corresponding cells contain different contents, Spreadsheet A's cell's
        /// original contents are retained.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="booleanOperator"></param>
        private void openAndCombineHelper(string filePath, string booleanOperator)
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType is XmlNodeType element)
                    {
                        if (reader.Name.Equals("Cell"))
                        {
                            string cellName = reader.GetAttribute(0);
                            string cellContents = reader.GetAttribute(1);

                            int row;
                            int col = cellName[0] - 65;
                            Int32.TryParse(cellName.Substring(1), out row);

                            if (booleanOperator.Equals("AND"))
                            {
                                if (!sheet.GetCellContents(cellName).ToString().Equals(cellContents))
                                    SendEdit(col, row - 1, "");

                            }
                            else if (booleanOperator.Equals("OR"))
                            {
                                if (sheet.GetCellContents(cellName).ToString().Equals("") && !cellContents.Equals(""))
                                    SendEdit(col, row - 1, cellContents);

                                else if (!sheet.GetCellContents(cellName).ToString().Equals("") && cellContents.Equals(""))
                                    SendEdit(col, row - 1, sheet.GetCellContents(cellName).ToString());

                            }
                            else
                            {
                                SendEdit(col, row - 1, cellContents);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Public accessor to allow the main method to open a file when the user double-clicks a sprd file from their file explorer
        /// </summary>
        /// <param name="fileName"></param>
        public void openFile(string fileName)
        {
            OpenAndCombineFileViewer(fileName);
        }

        /// <summary>
        /// User clicked "AND" operation for combining spreadsheets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAndCombineFileViewer("AND");
        }

        /// <summary>
        /// User clicked "OR" operation for combining spreadsheets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenAndCombineFileViewer("OR");
        }

        /// <summary>
        /// No idea why this needs to exist but I guess it does. Deleting it will break the app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Help Menu Item MessageBox for Spreadsheet navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navigationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Navigation:\n\nMove selection of the cells of this Spreadsheet using:\n\nArrow keys; mouse\n\nTo set " +
                "value of cell:\nClick \"Set Value\" button\n\n\n" +
                "Note: Moving cell selection to a different cell sets the contents and value of the cell. Allowed inputs include:\n\n" +
                "Formulas (By typing '=' into the cell),\nStrings (Words and Sentences),\nNumbers");
        }

        /// <summary>
        /// Help Menu Item MessageBox for Opening and Closing Spreadsheets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savingAndOpeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Saving and Opening:\n\nTo Save: Select File > Save As from the menu bar to save this spreadsheet as a *.sprd " +
                "file locally to your computer\n\nOpening: Select File > Open to navigate through your files until you find the specified " +
                "spreadsheet file with extension \"*.sprd\". Select the file and click \"OK\" to open the file.\n\nThe save and open file " +
                "dialog boxes allow you to choose between just files of type \"*.sprd\" and All File Types");
        }

        /// <summary>
        /// Help Menu Item MessageBox for describing Spreadsheet combination operations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combiningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Combining Spreadsheets:\n\nBy selecting \"Combine Spreadsheets\" from the file menu, you are " +
                "able to select between two operations: AND and OR\n\nAND: By selecting AND, and selecting a saved Spreadsheet " +
                "from your computer, every cell that contains the same contents on this Spreadsheet as the corresponding cell " +
                "on the selected spreadsheet will remain on the sheet, and any other cells will reset their contents.\n\nOR: By" +
                " selecting OR, and selecting a saved Spreadsheet from your computer, any cell that is populated on either sheet" +
                " that is not populated on the other becomes populated on this Spreadsheet with whatever contents were present." +
                " If the corresponding cell contains different contents on both sheets, the contents of the cell on this sheet don't " +
                "change");
        }

        /// <summary>
        /// Listener for Close() action, when the user closes from the 'X' in the top right corner
        /// 
        /// If data on the Spreadsheet has been edited but not saved, provides a warning message giving
        /// the user a chance to save their data before closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Form1_Closing(object sender, CancelEventArgs e)
        {
            if (isChanged)
            {
                DialogResult result = MessageBox.Show("Warning:\n\nAll unsaved changes will be lost" +
                        "\n\nClose anyway?", "Unsaved Changes", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void cellContentsField_TextChanged(object sender, EventArgs e)
        {
            contentsChanged = true;
        }
    }
}
