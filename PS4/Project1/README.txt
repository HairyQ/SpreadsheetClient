Author: Harrison Quick
Date Created: 09-26-18

Comment 09/29/18:
The Spreadsheet class is designed to initialize and define the relationships between cells in a spreadsheet.
There are several public methods that spreadsheet implements that are appropriate for setting and getting the
contents of cells. Cells in this spreadsheet are able to contain, as their contents, a String, Double, or
Formula (contents of cells are different from values in the case of Formulas). Much of the implementation I had
to consider throughout this project was centered around passing different types of objects to the setContents()
methods. I found that a single helper method was able to cut down on the amount of copy-pasted code within the
spreadsheet.

Although when one looks at a spreadsheet, it seems like infinite cells already exist and can be "set" to some 
value, I found that cells only had to be initialized in the Spreadsheet class when SetCellContents() was called.
This way, in using a HashMap, I am quickly able to determine whether or not a cell has already been initialized,
and if not, initialize a new one. From the perspecitve of the GUI, this is the same as clicking on an empty 
(uninitialized) cell.