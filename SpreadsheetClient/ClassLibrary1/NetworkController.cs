using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace Controller
{
    /// <summary>
    /// CallMe delegate to send SocketState back and forth from the server and client
    /// </summary>
    /// <param name="ss">socket state</param>
    public delegate void myDelegate(SocketState ss);

    /// <summary>
    /// CallMe delegate to send ConnectionState back and forth from server to client
    /// </summary>
    /// <param name="cs">connection state</param>
    public delegate void connectDelegate(ConnectionState cs);

    /// <summary>
    /// This class holds all the necessary state to represent a socket connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        /// <summary>
        /// Call me delegate
        /// </summary>
        private myDelegate callMe;
        public myDelegate CallMe
        {
            get { return callMe; }
            set { callMe = value; }
        }

        /// <summary>
        /// Placeholder socket state
        /// </summary>
        /// <param name="ss">socket state</param>
        public void placeHolder(SocketState ss) { }

        public myDelegate del;

        /// <summary>
        /// Socket of the socket state
        /// </summary>
        private Socket theSocket;
        public Socket TheSocket
        {
            get { return theSocket; }
            set { theSocket = value; }
        }

        /// <summary>
        /// Whether socket state is connected
        /// to client or not
        /// </summary>
        private bool connected;
        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        /// <summary>
        /// Message buffer of the socket state
        /// </summary>
        private byte[] messageBuffer;
        public byte[] MessageBuffer
        {
            get { return messageBuffer; }
            set { messageBuffer = value; }
        }

        /// <summary>
        /// StringBuilder of the socket state
        /// </summary>
        private StringBuilder sb;
        public StringBuilder SB
        {
            get { return sb; }
            set { sb = value; }
        }

        /// <summary>
        /// ID of the socket
        /// </summary>
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Constructor socket state
        /// </summary>
        /// <param name="s">socket</param>
        /// <param name="connectionID">id of connection</param>
        public SocketState(Socket s, int connectionID)
        {
            theSocket = s;
            id = connectionID;
            messageBuffer = new byte[1024];
            sb = new StringBuilder();
            del = placeHolder;
            callMe = del;
            connected = true;
        }
    }

    /// <summary>
    /// Placeholder for TcpListener of a connected client and
    /// a function to call when this happens.
    /// </summary>
    public class ConnectionState
    {
        /// <summary>
        /// Call me function
        /// </summary>
        private myDelegate callMe;
        public myDelegate CallMe
        {
            get { return callMe; }
            set { callMe = value; }
        }

        /// <summary>
        /// TcpListener of socket
        /// </summary>
        private TcpListener lstn;
        public TcpListener Lstn
        {
            get { return lstn; }
            set { lstn = value; }
        }

        /// <summary>
        /// Constructor to instialize this wrapper class
        /// </summary>
        /// <param name="tcpl">TcpListener</param>
        /// <param name="connCall">function connection call function</param>
        public ConnectionState(TcpListener tcpl, myDelegate connCall)
        {
            callMe = connCall;
            lstn = tcpl;
        }
    }

    /// <summary>
    /// Static network class that allows for connecting to a server from
    /// a client from port 11000.
    /// </summary>
    public static class Network
    {
        private static int DEFAULT_PORT = 11000;
        private static int IDCounter = 0;

        public static void setPort(int x)
        {
            DEFAULT_PORT = x;
        }


        /// <summary>
        /// Attempts to connect to the server via provide hostname. Saves callMe 
        /// in SocketState object for use when data arrives. Creates a socket and 
        /// then uses the BeginConnect.
        /// </summary>
        /// <param name="callMe">
        /// Delegate called when connection made.
        /// </param>
        /// <param name="hostname">
        /// Name of server to connect to.
        /// </param>
        /// <returns></returns>
        public static Socket ConnectToServer(myDelegate callMe, string hostName)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostName);

            // Create a TCP/IP socket.
            Socket socket;
            IPAddress ipAddress;

            MakeSocket(hostName, out socket, out ipAddress);

            SocketState ss = new SocketState(socket, -1);
            ss.CallMe = callMe;

            socket.BeginConnect(ipAddress, DEFAULT_PORT, ConnectedCallback, ss);

            return socket;
        }

        /// <summary>
        /// Reference by BeginConnect above and is called when socket connects to 
        /// server. Once connection established, saved callMe is then called 
        /// (originally passed in the ConnectToServer in SocketState)
        /// </summary>
        /// <param name="stateAsArObject">
        /// Contains a field "AsyncState" which contains SocketState saved away for
        /// later use.
        /// </param>
        public static void ConnectedCallback(IAsyncResult stateAsArObject)
        {
            SocketState ss = (SocketState)stateAsArObject.AsyncState;

            try
            {
                // Complete the connection.
                ss.TheSocket.EndConnect(stateAsArObject);
                System.Diagnostics.Debug.WriteLine("connected to server");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            ss.CallMe(ss);
        }

        /// <summary>
        /// Client call for more data. Called when data has finished processing
        /// in its callMe.
        /// </summary>
        /// <param name="state">
        /// SocketState where callMe is saved away.
        /// </param>
        public static void GetData(SocketState state)
        {
            try
            {
                if (state.Connected)
                    state.TheSocket.BeginReceive(state.MessageBuffer, 0, state.MessageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                state.Connected = false;
            }
        }

        /// <summary>
        /// Called when new data arrives. Checks to see how much data has arrived:
        /// 
        /// If 0: connection has been closed (presumably by the server)
        /// If > 0: get SocketState out of stateAsArObject and call the callMe provided
        /// </summary>
        /// <param name="stateAsArObject">
        /// SyncResult containing the SocketState where callMe is saved away.
        /// </param>
        public static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            SocketState ss = (SocketState)stateAsArObject.AsyncState;

            try
            {
                if (ss.Connected)
                {
                    int bytesRead = ss.TheSocket.EndReceive(stateAsArObject);

                    // If the socket is still open
                    if (bytesRead > 0)
                    {
                        string theMessage = Encoding.UTF8.GetString(ss.MessageBuffer, 0, bytesRead);
                        // Append the received data to the growable buffer.
                        // It may be an incomplete message, so we need to start building it up piece by piece
                        ss.SB.Append(theMessage);
                    }

                    ss.CallMe(ss);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                ss.Connected = false;
            }
        }

        /// <summary>
        /// Allows program to send data over a socket. Converts data into bytes and 
        /// then sends them using socket.BeginSend.
        /// </summary>
        /// <param name="socket">
        /// Socket to send data over.
        /// </param>
        /// <param name="data">
        /// Data to send.
        /// </param>
        public static void Send(Socket socket, String data)
        {
            try
            {
                byte[] serverData = Encoding.ASCII.GetBytes(data);
                socket.BeginSend(serverData, 0, serverData.Length, SocketFlags.None, SendCallback, new SocketState(socket, -1));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Helper function for Send. Extracts socket out of IAsyncResult and the
        /// calls socket.EndSend.
        /// </summary>
        /// <param name="ar">
        /// IAsyncResult to extract socket from.
        /// </param>
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                SocketState ss = (SocketState)ar.AsyncState;
                ss.TheSocket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        private static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
            ipAddress = IPAddress.None;
            socket = null;
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        throw new ArgumentException("Invalid address");
                    }
                }
                catch (Exception)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }

        /// <summary>
        /// Start TcpListener for new connections and the pass the listener, along with the callMe 
        /// delegate, to BeginAcceptSocket as the state parameter. This is not the same as a 
        /// SocketState. Upon connection request coming in the OS should invoke the AcceptNewClient
        /// as the callback method. 
        /// </summary>
        /// <param name="callMe">connection delegate</param>
        public static void ServerAwaitingClientLoop(myDelegate callMe)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, DEFAULT_PORT);
            listener.Start();
            ConnectionState cs = new ConnectionState(listener, callMe);
            listener.BeginAcceptSocket(AcceptNewClient, cs);
        }

        /// <summary>
        /// BeginAcceptSocket's callback function. When connection comes in, this method:
        /// (1) extracts the state containing the TcpListener and the callMe function from ar. 
        /// (2) creates a new socket by using lister.EndAcceptSocket
        /// (3) save socket in new SocketState
        /// (4) call the callMe method and pass it to the new SocketState. This is how the server
        ///     gets a copy of the SocketState corresponding to this client. The callMe should 
        ///     save the SocketState in a list of clients.
        /// (5) Await a new connection request (coninued in the event loop) with BeginAcceptSocket. 
        ///     This means that the networking code assumes we want to always continue the event loop.
        /// </summary>
        /// <param name="ar">contains the callMe and the TcpListener</param>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            ConnectionState cs = (ConnectionState)ar.AsyncState;
            SocketState ss = new SocketState(cs.Lstn.EndAcceptSocket(ar), ++IDCounter);
            ss.CallMe = cs.CallMe;
            ss.CallMe(ss);
            cs.Lstn.BeginAcceptSocket(AcceptNewClient, cs);
        }
    }
}
