using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Controller;
using Resources;

namespace ClientGUI
{
    public partial class InitialWindow : Form
    {
        public StaticState state;
        public ClientController clientController;
        private string serverAddressAndLoginInfo;

        public InitialWindow()
        {
            InitializeComponent();
            state = new StaticState();
            clientController = new ClientController(state);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Send username, password and IP to clientcontroller
            clientController.ReceiveStartupData(textBox3.Text, textBox4.Text, textBox1.Text);

            Spreadsheets newSpreads = new Spreadsheets(clientController, state);
            this.Hide();
            newSpreads.Show();
        }
    }
}
