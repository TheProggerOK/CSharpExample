using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeautySalonAdmin
{
    public partial class LoginForm : Form
    {
        public string conStr = ConfigurationManager.ConnectionStrings["BeatySalonDB"].ConnectionString.ToString();
        public int uid;
        public LoginForm()
        {
            InitializeComponent();
            
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите выйти из приложения?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
           try
           {
                    string login = LoginTB.Text;
                    string pass = PasswordTB.Text;

                    DataBaseClass db = new DataBaseClass();

                    DataTable table = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter("Select [Код пользователя] From Пользователь where Логин = '" + login + "' and Пароль = '" + pass + "'", db.GetConnection());
                    sda.Fill(table);

                    if (table.Rows.Count == 1)
                    {
                            MainMenu frm = new MainMenu();
                            uid = table.Rows[0].Field<int>(0);
                            frm.u = uid;
                            frm.Show();
                            this.Close();
                    }
                    else
                    {
                    MessageBox.Show("Введены неверные учетные данные!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
           }
            catch { }
        }
    }
}
