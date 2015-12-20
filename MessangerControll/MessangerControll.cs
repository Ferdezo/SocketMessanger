using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationManager;

namespace MessangerControll
{
    public partial class MessangerControll: UserControl, IReceive, IDisposable
    {
        private SocketManager socketManager;
        private bool connected = false;

        public MessangerControll()
        {
            InitializeComponent();
            try
            {
                socketManager = new SocketManager(this);
                SetSenderIP(socketManager.GetLocalIP());
                SetReceiverIP(socketManager.GetLocalIP());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!");
            }
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            try
            {
                socketManager.SendMessage(GetTextMessage());
                ClearTextMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!");
            }

            AddMsgToList("M1: " + GetTextMessage());
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                socketManager.StartConnection(GetSenderIp(), GetSenderPort(), GetReceiverIP(), GetReceiverPort());
                AddMsgToList("<< CONNECTED >>");
                DisableConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!");
            }
        }

        //Implementation IReceive
        public void ReceiveMessage(String msg)
        {
            try
            {
                AddMsgToList("M2: " + msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd!");
            }
        }

        public void AddMsgToList(String str)
        {
            this.Invoke((MethodInvoker)(() => listMessage.Items.Add(str)));
        }

        public String GetSenderIp() { return senderIP.Text; }

        public void SetSenderIP(String str) { senderIP.Text = str; }

        public String GetSenderPort() { return senderPort.Text; }

        public void SetSenderPort(String str) { senderPort.Text = str; }

        public String GetReceiverIP() { return senderIP.Text; }

        public void SetReceiverIP(String str) { receiverIP.Text = str; }

        public String GetReceiverPort() { return receiverPort.Text; }

        public void SetReceiverPort(String str) { receiverPort.Text = str; }

        public String GetTextMessage() { return textMessage.Text; }

        public void ClearTextMessage() { textMessage.Text = ""; }

        public void DisableConnection() { connectBtn.Enabled = false; }
    }
}
