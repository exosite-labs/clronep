using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace clronep.examples
{
    public partial class OnePV1Form : Form
    {
        public string cik;
        public OnePV1Form()
        {
            InitializeComponent();
            cik = "You didn't fill out the form!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cik = this.textBox1.Text;
            this.Close();
        }
    }
}
