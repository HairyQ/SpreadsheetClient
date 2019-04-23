using System;
using System.Collections;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Resources;

namespace Controller
{
    /// <summary>
    /// Controller that will parse data from the Spreadsheet and
    /// communicate with the NetworkController to convey messages
    /// </summary>
    public class ClientController
    {
        //Commenting to commit
        public StaticState state;
        private Socket theServer;
        public string username, password;

        public delegate void SpreadsheetEditHandler();
        public delegate void SpreadsheetListHandler();
        public delegate void CircularDependencyError();
        public delegate void LoginError();

        private event SpreadsheetEditHandler displayEdit;
        private event SpreadsheetListHandler displayLists;
        private event CircularDependencyError displayError;
        private event LoginError loginError;

        /// <summary>
        /// Constructor for ClientController should take a StaticState as a parameter, so that all of the 
        /// projects can send information between themselves
        /// </summary>
        /// <param name="s"></param>
        public ClientController(StaticState s)
        {
            state = s;
        }

        public void RegisterSpreadsheetEditHandler(SpreadsheetEditHandler s)
        {
            displayEdit += s;
        }

        public void RegisterSpreadsheetListHandler(SpreadsheetListHandler s)
        {
            displayLists += s;
        }

        public void RegisterErrorHandler(CircularDependencyError c)
        {
            displayError += c;
        }

        public void RegisterLoginErrorHandler(LoginError l)
        {
            loginError += l;
        }

        /// <summary>
        /// This method is called from the GUI when the user presses the "connect" button. It passes the callMe
        /// delegate and server address to the NetworkController's ConnectToServer.
        /// </summary>
        /// <param name="ip"></param>
        public void InitiateFirstContact(string address)
        {
            myDelegate callMe = FirstContact;
            theServer = Network.ConnectToServer(callMe, address);
        }

        public void FirstContact(SocketState ss)
        {
            ss.CallMe = ReceiveSpreadsheets;
            Network.GetData(ss);
        }

        public void ReceiveStartupData(string user, string pass, string address)
        {
            username = user;
            password = pass;
            InitiateFirstContact(address);
        }

        public void CreateAndSendMessage(string type, string content)
        {
            switch (type)
            {
                case ("open"):
                    //string[] contents = Regex.Split(content, @"\`(.*?)\`");
                    string spreadsheetName = content;

                    OpenMessage newMessage = new OpenMessage(spreadsheetName, username, password);

                    string s = JsonConvert.SerializeObject(newMessage);
                    SendMessage(newMessage);
                    break;
            }
        }

        public void SendMessage(Message newMessage)
        {
            lock (state)
            {
                String message = JsonConvert.SerializeObject(newMessage) + "\n\n";
                Network.Send(theServer, message);
            }
        }

        /// <summary>
        /// Receives the startup data from the server (worldSize & ID)
        /// </summary>
        /// <param name="ss"></param>
        public void ReceiveSpreadsheets(SocketState ss)
        {
            string startupData = ss.SB.ToString();

            SpreadsheetListMessage sentMessage = JsonConvert.DeserializeObject<SpreadsheetListMessage>(startupData);
            state.Spreadsheets = sentMessage.spreadsheets;

            ss.SB.Clear();

            ss.CallMe = ReceiveServerMessages;
            Network.GetData(ss);

            displayLists.Invoke();
        }

        public void ReceiveServerMessages(SocketState ss)
        {
            string totalData = ss.SB.ToString();
            string[] parts = totalData.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            //string[] parts = Regex.Split(totalData, @"(?<=[\n][\n])");
            ArrayList totalMessages = new ArrayList();

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '}' || p[0] != '{')
                    break;

                totalMessages.Add(p);
                ss.SB.Remove(0, p.Length);
                ss.SB.Remove(0, 2);
            }

            DeserializeJsonAndUpdateWindow(totalMessages);

            Network.GetData(ss);
        }

        public void DeserializeJsonAndUpdateWindow(ArrayList messages)
        {
            foreach (string message in messages)
            {
                try
                {
                    JObject obj = JObject.Parse(message);

                    JToken isEdit = obj["spreadsheet"];
                    JToken isError = obj["code"];

                    if (isEdit != null)
                    {
                        FullSendMessage edit = JsonConvert.DeserializeObject<FullSendMessage>(message);

                        foreach (string s in edit.spreadsheet.Keys)
                        {
                            state.Col = s[0] - 65;
                            int row;
                            if (Int32.TryParse(s.Substring(1, s.Length - 1), out row))
                                state.Row = row - 1;
                            object value;
                            if (edit.spreadsheet.TryGetValue(s, out value))
                            {
                                state.Contents = value + "";
                            }
                            state.CellName = s;

                            displayEdit.Invoke();
                        }
                    }
                    else if (isError != null)
                    {
                        ErrorMessage newError = JsonConvert.DeserializeObject<ErrorMessage>(message);

                        if (newError.code == 2)
                        {
                            state.CellName = newError.source;
                            displayError.Invoke();
                        }
                        else
                        {
                            loginError.Invoke();
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }
}