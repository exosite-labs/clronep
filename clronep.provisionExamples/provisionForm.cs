using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace clronep.provisionExamples
{
    public partial class provisionForm : Form
    {
        public string vendorname;
        public string vendortoken;
        public string clonecik;
        public string cloneportalcik;
        public string portalcik;
        public provisionForm()
        {
            InitializeComponent();
            vendorname = vendortoken = clonecik = cloneportalcik = portalcik = "Fill out the form!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vendorname = this.vendorNameTextBox.Text;
            vendortoken = this.vendorTokenTextBox.Text;
            clonecik = this.cloneCIKTextBox.Text;
            cloneportalcik = this.clonePortalCIKTextBox.Text;
            portalcik = this.portalCIKTextBox.Text;
            this.Close();
        }

        private void clonePortalCIKTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
