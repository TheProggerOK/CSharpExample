using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeautySalonAdmin
{
    public partial class MainMenu : Form
    {
        public int u;
        public MainMenu()
        {
            InitializeComponent();

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Form frm = new LoginForm();
            frm.Show();
            this.Close();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
        }

        private void CabButton_Click(object sender, EventArgs e)
        {
            UserCab frm = new UserCab();
            frm.userid = u;
            frm.Show();
            this.Close();
        }

        private void SprManagement_Click(object sender, EventArgs e)
        {
            SprManagement frm = new SprManagement();
            frm.Show();
        }

        private void DocManagement_Click(object sender, EventArgs e)
        {
            DocMan frm = new DocMan();
            frm.Show();
        }

        private void Reports_Click(object sender, EventArgs e)
        {
            OtCr frm = new OtCr();
            frm.Show();
        }
    }
}
