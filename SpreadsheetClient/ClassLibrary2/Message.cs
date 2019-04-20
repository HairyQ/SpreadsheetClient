using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Resources
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Message { }

    [JsonObject(MemberSerialization.OptIn)]
    public class OpenMessage : Message
    {
        [JsonProperty(PropertyName = "type")]
        public string type = "open";

        [JsonProperty(PropertyName = "name")]
        public string spreadsheetName;

        [JsonProperty(PropertyName = "username")]
        public string username;

        [JsonProperty(PropertyName = "password")]
        public string password;

        public OpenMessage(string SpreadSheetName, string Username, string Password)
        {
            spreadsheetName = SpreadSheetName;
            username = Username;
            password = Password;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class EditMessage : Message
    {
        [JsonProperty(PropertyName = "type")]
        public string type = "edit";

        [JsonProperty(PropertyName = "cell")]
        public string cell;

        [JsonProperty(PropertyName = "value")]
        public string value;

        [JsonProperty(PropertyName = "dependencies")]
        public ArrayList dependencies;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class EditMessageDoubleType : Message
    {
        [JsonProperty(PropertyName = "type")]
        public string type = "edit";

        [JsonProperty(PropertyName = "cell")]
        public string cell;

        [JsonProperty(PropertyName = "value")]
        public double value;

        [JsonProperty(PropertyName = "dependencies")]
        public ArrayList dependencies;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class UndoMessage : Message
    {
        [JsonProperty(PropertyName = "type")]
        public string type = "undo";
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RevertMessage : Message
    {
        [JsonProperty(PropertyName = "type")]
        public string type = "revert";

        [JsonProperty(PropertyName = "cell")]
        public string cell;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorMessage : Message
    {
        [JsonProperty(PropertyName = "type")]
        public string type = "error";

        [JsonProperty(PropertyName = "code")]
        public int code;

        [JsonProperty(PropertyName = "source")]
        public string source;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SpreadsheetListMessage : Message
    {
        public SpreadsheetListMessage()
        {
            type = "list";
            spreadsheets = new List<string>();
        }

        [JsonProperty(PropertyName = "type")]
        public string type = "list";

        [JsonProperty(PropertyName = "spreadsheets")]
        public List<string> spreadsheets;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class FullSendMessage : Message
    {
        [JsonProperty(PropertyName = "type")]
        public string type = "full send";

        [JsonProperty(PropertyName = "spreadsheet")]
        public Dictionary<string, object> spreadsheet;
    }
}
