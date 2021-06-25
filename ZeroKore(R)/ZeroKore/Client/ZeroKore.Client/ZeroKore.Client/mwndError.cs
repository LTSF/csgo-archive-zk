using System;
using System.Windows.Forms;

namespace ZeroKore.Client
{
    public partial class mwndError : Form
    {
        public mwndError(Exception err)
        {
            InitializeComponent();

            lblError.Text = err.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
