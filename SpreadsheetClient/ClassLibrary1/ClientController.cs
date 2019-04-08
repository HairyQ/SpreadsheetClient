using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Controller
{
    /// <summary>
    /// Controller that will parse data from the server to the model
    /// of the client.
    /// </summary>
    public class GameController
    {
        private Socket theServer;

        public delegate void InvalidateEventHandler();
        public delegate void ResizeEventHandler();
        public delegate void SendKeysHandler(SocketState ss);

        private event SendKeysHandler sendKeys;
        private event ResizeEventHandler resize;
        private event InvalidateEventHandler onUpdate;

        public string name;
        public int ID;

        /// <summary>
        /// Constructor to pass a world into this object
        /// </summary>
        /// <param name="w">world</param>
        public GameController()
        {
            
        }

        /// <summary>
        /// Event handler register to update drawings of the world by invalidating the 
        /// GUI. Gets called after the model has been updated
        /// </summary>
        /// <param name="s">event handler</param>
        public void RegisterUpdateHandler(InvalidateEventHandler s)
        {
            onUpdate += s;
        }

        /// <summary>
        /// Event handler register to resize the world.
        /// </summary>
        /// <param name="r">event handler</param>
        public void RegisterResizeHandler(ResizeEventHandler r)
        {
            resize += r;
        }

        /// <summary>
        /// Event handler register for key strokes to get sent to
        /// server.
        /// </summary>
        /// <param name="s">event handler</param>
        public void RegisterKeystrokeHandler(SendKeysHandler s)
        {
            sendKeys += s;
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

        /// <summary>
        /// First Contact has been established, so time to complete the handshake
        /// </summary>
        /// <param name="ss"></param>
        public void FirstContact(SocketState ss)
        {
            ss.CallMe = ReceiveStartup;

            Network.Send(ss.TheSocket, name);
            Network.GetData(ss);
        }

        /// <summary>
        /// Receives the startup data from the server (worldSize & ID)
        /// </summary>
        /// <param name="ss"></param>
        public void ReceiveStartup(SocketState ss)
        {
            string startupData = ss.SB.ToString();
            string[] parts = Regex.Split(startupData, @"(?<=[\n])");
            int ID;
            int size;

            Int32.TryParse(parts[0], out ID);
            Int32.TryParse(parts[1], out size);

            ss.SB.Clear();
            ss.CallMe = ReceiveWorld;
            Network.GetData(ss);
            resize();
        }

        /// <summary>
        /// Process the string from the socket into parts that represent objects in the game
        /// </summary>
        /// <param name="ss"></param>
        public void ReceiveWorld(SocketState ss)
        {
            string totalData = ss.SB.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            ArrayList totalMessages = new ArrayList();

            // Loop until we have processed all messages.
            // We may have received more than one.

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
            DeserializeJsonAndUpdateWorld(totalMessages);

            Network.GetData(ss);
            sendKeys(ss);
        }

        /// <summary>
        /// Method for parsing info from a Json String to an object of the correct type
        /// </summary>
        /// <param name="message">Json Message to be deserialized</param>
        public void DeserializeJsonAndUpdateWorld(ArrayList totalMessages)
        {
            foreach (string message in totalMessages)
            {
                //Parse string into JObject
                JObject obj = JObject.Parse(message);

                //Determine type by trying to find field specific to each type it could be
                JToken isShip = obj["ship"];
                JToken isProjectile = obj["proj"];
                JToken isStar = obj["star"];

                //Then check the type, and deserialize accordingly:
                
            }
            // onUpdate();
        }
    }
}