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
using Resources;

namespace ClientGUI
{
    public partial class Spreadsheets : Form
    {
        private ClientController clientController;
        private StaticState state;
        private InitialWindow initial;
        public ListBox listBox1;

        public Spreadsheets(ClientController c, StaticState s, InitialWindow Initial)
        {
            initial = Initial;
            clientController = c;
            state = s;
            clientController.RegisterSpreadsheetListHandler(displaySheets);
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Options_OnClosing);
        }

        private void Options_OnClosing(object sender, FormClosingEventArgs e)
        {
            initial.Close();
        }

        private void displaySheets()
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBox1 = new ListBox();
                listBox1.Location = new System.Drawing.Point(12, 12);
                listBox1.Name = "ListBox1";
                listBox1.Size = new System.Drawing.Size(250, 340);
                listBox1.BackColor = System.Drawing.Color.White;
                listBox1.ForeColor = System.Drawing.Color.Black;

                foreach (string s in state.Spreadsheets)
                {
                    listBox1.Items.Add(s);
                }

                Controls.Add(listBox1);
            });
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(listBox1.SelectedItem);
            clientController.CreateAndSendMessage("open", messageBuilder.ToString());

            SpreadsheetGUI.Form1 spread = new SpreadsheetGUI.Form1(clientController, state);
            Hide();
            spread.Show();
        }
    }
}
