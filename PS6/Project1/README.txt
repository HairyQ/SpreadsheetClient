This Spreadsheet Application offers similar basic functionality to that of Excel.
The cells can be selected and changed using the arrow keys or mouse. The enter key
can be used to select the next cell down and input contents. The tab key can also
be used to select the next cell to the right. By typing once a cell is selected,
the contents of the cell can be changed. By moving selection or clicking the 
"Set Cell" button, the contents of the cell have been changed and the value 
calculated. 





This Spreadsheet implements a feature called "Combine Spreadsheets", which is capable
of comparing cell contents betweeen two separate Spreadsheets. 

To use the combine feature, open a Spreadsheet that contains data, or place data 
into a new Spreadsheet. Select Combine Spreadsheets from the menu bar, then choose
an operation: AND, or OR. When either operation is selected, an OpenFileDialog is 
created and a file can be selected. 
If AND was selected, the Spreadsheets compare
corresponding cells (e.g., Cell B1 in Spreadsheet A corresponds with Cell B1 in 
Spreadsheet B), and if the contents of both cells are the same, the open Spreadsheet
does not change that cell. In any other case, such as if the contents are different
or one is missing, the contents of that cell in Spreadsheet A is reset to an empty
string.
If OR was selected, the Spreadsheets compare corresponding cells, and if the contents
of either cell are populated, the open Spreadsheet is amended to display those 
contents. If the contents of the corresponding cells are different, the original
value of the open Spreadsheet's cell is retained.


Example: 
SpreadsheetA.Cell(B3) = "0.4" AND SpreadsheetB.Cell(B3) = "0.4" 
SpreadsheetA (the originally opened Spreadsheet) does not change Cell 3B's contents

SpreadsheetA.Cell(Z99)  = "0.4" AND SpreadsheetB.Cell(Z99) = "" 
In this case, the contents of SpreadsheetA's cell change to an empty string.

SpreadsheetA.Cell(X23) = "" OR SpreadsheetB.Cell(X23) = "Hello"
In this case, SpreadsheetA's contents of X23 become "Hello"

In all cases, the SpreadsheetB remains unchanged.


Implementation note: We found that instead of creating two separate methods to 
handle the logic of the AND and OR operations, the logic for both operations
could be incorporated into the file Open control method. Thus, a helper method
was created that is used in three different cases: modifying the Spreadsheet 
using the AND operation, doing so using the OR operation, and opening a saved file. 
