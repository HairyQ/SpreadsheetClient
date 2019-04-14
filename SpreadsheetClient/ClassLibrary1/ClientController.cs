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
        private Socket theServer;
        public string serverAddressAndLoginInfo;

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

        public void ReceiveStartupData(string startupData, string address)
        {
            serverAddressAndLoginInfo = startupData;
            InitiateFirstContact(address);
        }

        /// <summary>
        /// First Contact has been established, so time to complete the handshake
        /// </summary>
        /// <param name="ss"></param>
        public void FirstContact(SocketState ss)
        {
            ss.CallMe = ReceiveSpreadsheets;
        
            Network.Send(ss.TheSocket, serverAddressAndLoginInfo);
            Network.GetData(ss);
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

                    OpenMessage newMessage = new OpenMessage(type, spreadsheetName, username, password);

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

            FullSendMessage sentMessage = JsonConvert.DeserializeObject<FullSendMessage>(startupData);
            //TODO: Get the list of spreadsheets to the GUI!!!

            ss.SB.Clear();
            ss.CallMe = ReceiveServerMessages;
            Network.GetData(ss);
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