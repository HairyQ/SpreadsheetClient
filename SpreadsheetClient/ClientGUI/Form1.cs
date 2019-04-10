using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;

namespace ClientGUI
{
    public partial class Form1 : Form
    {
        public ClientController clientController;
        private string serverAddressAndLoginInfo;

        public Form1()
        {
            InitializeComponent();
            clientController = new ClientController();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(textBox1.Text);
            messageBuilder.Append(textBox2.Text);
            messageBuilder.Append(textBox3.Text);
            messageBuilder.Append(textBox4.Text);
            messageBuilder.Append("\n");

            serverAddressAndLoginInfo = messageBuilder.ToString();

            clientController.ReceiveStartupData(serverAddressAndLoginInfo, textBox1.Text);

            Spreadsheets newSpreads = new Spreadsheets(clientController);
            this.Hide();
            newSpreads.Show();
        }
    }
}
