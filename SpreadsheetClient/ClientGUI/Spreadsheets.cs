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
        public ListBox listBox1;

        public Spreadsheets(ClientController c, StaticState s)
        {
            clientController = c;
            state = s;
            clientController.RegisterSpreadsheetListHandler(displaySheets);
            InitializeComponent();
        }

        private void displaySheets()
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBox1 = new ListBox();
                listBox1.Location = new System.Drawing.Point(12, 12);
                listBox1.Name = "ListBox1";
                listBox1.Size = new System.Drawing.Size(245, 200);
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
        }
    }
}
