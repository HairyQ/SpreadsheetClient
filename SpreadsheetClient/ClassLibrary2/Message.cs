using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public abstract class Message
    {
        public string type;
    }

    public class OpenMessage : Message
    {
        public string spreadsheetName, username, password;

        public OpenMessage(string SpreadSheetName, string Username, string Password)
        {
            type = "open";
            spreadsheetName = SpreadSheetName;
            username = Username;
            password = Password;
        }
    }

    public class FullSendMessage : Message
    {
        public List<string> spreadsheet;

        public FullSendMessage(List<string> Spreadsheet)
        {
            type = "full send";
            spreadsheet = Spreadsheet;
        }
    }
}
