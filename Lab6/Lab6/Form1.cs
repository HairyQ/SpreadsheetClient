using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            double percentage = Double.Parse(this.PercentageInput.Text) / 100;
            double bill_total = Double.Parse(textBox1.Text);
            double tip = bill_total * percentage;
            textBox2.Text = tip + "";
            this.Total_Label.Text = "Total Bill: " + (bill_total + tip);
        }

        private void PercentageInput_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
