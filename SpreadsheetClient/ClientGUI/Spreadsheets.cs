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
    public partial class Spreadsheets : Form
    {
        private ClientController clientController;

        public Spreadsheets(ClientController c)
        {
            clientController = c;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(textBox1.Text);
            messageBuilder.Append("\n");
            clientController.SendMessage(messageBuilder.ToString());
        }
    }
}
