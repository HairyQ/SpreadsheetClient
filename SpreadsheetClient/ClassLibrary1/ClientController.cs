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
        public StaticState state;
        private Socket theServer;
        public string username, password;

        public delegate void SpreadsheetListHandler();

        private event SpreadsheetListHandler displayLists;

        /// <summary>
        /// Constructor for ClientController should take a StaticState as a parameter, so that all of the 
        /// projects can send information between themselves
        /// </summary>
        /// <param name="s"></param>
        public ClientController(StaticState s)
        {
            state = s;
        }

        public void RegisterSpreadsheetListHandler(SpreadsheetListHandler s)
        {
            displayLists += s;
        }

        /// <summary>
        /// This method is called from the GUI when the user presses the "connect" button. It passes the callMe
        /// delegate and server address to the NetworkController's ConnectToServer.
        /// </summary>
        /// <param name="ip"></param>
        public void InitiateFirstContact(string address)
        {
            //////////////////////////////////////
            SpreadsheetListMessage newMessage = new SpreadsheetListMessage();
            newMessage.spreadsheets.Add("newSprd");
            newMessage.spreadsheets.Add("newSprd2");
            newMessage.spreadsheets.Add("newSprd3");
            string jsonString = JsonConvert.SerializeObject(newMessage);
            Console.WriteLine(jsonString);
            //////////////////////////////////////

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
                    string[] contents = Regex.Split(content, @"\`(.*?)\`");
                    string spreadsheetName = contents[0];
                    string username = contents[1];
                    string password = contents[2];

                    OpenMessage newMessage = new OpenMessage(spreadsheetName, username, password);

                    string s = JsonConvert.SerializeObject(newMessage);
                    Console.WriteLine(s);
                    break;
            }
        }

        public void SendMessage(Message newMessage)
        {
            String message = JsonConvert.SerializeObject(newMessage) + "\n\n";
            Network.Send(theServer, message);
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

            foreach (string s in state.Spreadsheets)
            {
                Console.WriteLine(s);
            }

            ss.SB.Clear();
            /*
            ss.CallMe = ReceiveServerMessages;
            Network.GetData(ss);
            displayLists();
            */
        }

        public void ReceiveServerMessages(SocketState ss)
        {
            string totalData = ss.SB.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            ArrayList totalMessages = new ArrayList();

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                string subString = p.Substring(2, 4);
                if (!subString.Equals("ship") && !subString.Equals("star") && !subString.Equals("proj"))
                    break;

                totalMessages.Add(p);
                ss.SB.Remove(0, p.Length);
            }
            DeserializeJsonAndUpdateWindow(totalMessages);

            Network.GetData(ss);
            //sendKeys(ss);
        }

        public void DeserializeJsonAndUpdateWindow(ArrayList messages)
        {
            foreach (string message in messages)
            {
                JObject obj = JObject.Parse(message);


            }
        }
    }
}