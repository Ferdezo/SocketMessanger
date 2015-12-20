using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;


namespace CommunicationManager
{
    public class SocketManager
    {
        Socket socket;
        EndPoint epSender, epReceiver;
        IReceive receiver { get; set; }

        public SocketManager(IReceive receiver = null)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.receiver = receiver;
        }

        public void CloseSocket()
        {
            socket.Close();
        }

        public String GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1"; //localhost
        }

        public void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                int size = socket.EndReceiveFrom(aResult, ref epReceiver);
                if (size > 0)
                {
                    byte[] receivedData = new byte[1464];
                    receivedData = (byte[])aResult.AsyncState;

                    ASCIIEncoding eEncoding = new ASCIIEncoding();
                    string receivedMessage = eEncoding.GetString(receivedData);
                    if (receiver != null)
                        receiver.ReceiveMessage(receivedMessage); //notify receiving the message
                }
            }
            catch (SocketException se)
            {
                receiver.ReceiveMessage("Wiadomość nie wysłana, odbiorca nie połączył się jeszcze prawidłowo");
            }

            byte[] buffer = new byte[1500];
            socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epReceiver, new AsyncCallback(MessageCallBack), buffer);
        }

        public void StartConnection(String senderIP, String senderPort, String receiverIP, String receiverPort)
        {
            epSender = new IPEndPoint(IPAddress.Parse(senderIP), Convert.ToInt32(senderPort));
            socket.Bind(epSender);

            epReceiver = new IPEndPoint(IPAddress.Parse(receiverIP), Convert.ToInt32(receiverPort));
            socket.Connect(epReceiver);

            byte[] buffer = new byte[1500];
            socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epReceiver, new AsyncCallback(MessageCallBack), buffer);
        }

        public void SendMessage(String message)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] msg = new byte[1500];
            msg = enc.GetBytes(message);
            socket.Send(msg);
        }

        public void SetReceiver(IReceive receiver)
        {
            this.receiver = receiver;
        }
    }
}