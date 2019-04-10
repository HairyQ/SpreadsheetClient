using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public void SendMessage(string message)
        {
            Network.Send(theServer, message);
        }

        /// <summary>
        /// Receives the startup data from the server (worldSize & ID)
        /// </summary>
        /// <param name="ss"></param>
        public void ReceiveSpreadsheets(SocketState ss)
        {
            string startupData = ss.SB.ToString();
            string[] parts = Regex.Split(startupData, @"(?<=[\n])");

            ss.SB.Clear();
            //ss.CallMe = ReceiveWorld;
            Network.GetData(ss);
        }
    }
}