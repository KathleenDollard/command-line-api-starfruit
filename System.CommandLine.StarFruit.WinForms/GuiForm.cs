using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.CommandLine.StarFruit.WinForms
{
    public partial class GuiForm : Form
    {
        public GuiForm()
        {
            InitializeComponent();
        }

   

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        internal void SetCommandText(string textToMatch)
        {
            commandTextBox.Text = textToMatch;
        }
    }
}
