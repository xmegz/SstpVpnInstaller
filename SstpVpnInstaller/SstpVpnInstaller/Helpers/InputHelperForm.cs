using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SstpVpnInstaller
{
    public partial class InputHelperForm : Form
    {
        public InputHelperForm()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Abort;
            this.ActiveControl = this.toolStripTextBox1.Control;
            this.Text += Application.ProductName + " V" + Application.ProductVersion;
        }

        private void Ok()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Ok();
        }

        private void toolStripTextBox1_Enter(object sender, EventArgs e)
        {
            Ok();
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Ok();
            }
        }
    }
}
