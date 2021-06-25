using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upswing
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "Admin") && (textBox2.Text == "M0t2p@55e!"))
            {
                UpswingApp app = new UpswingApp();
                app.Show();
                this.Hide();
            }
        }
    }
}