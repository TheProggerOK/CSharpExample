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
    public partial class UserCab : Form
    {
        public string conStr = ConfigurationManager.ConnectionStrings["BeatySalonDB"].ConnectionString.ToString();
        public int userid;
        public UserCab()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainMenu frm = new MainMenu();
            frm.u = userid;
            frm.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Form frm = new LoginForm();
            frm.Show();
            this.Close();
        }

        private void UserCab_Load(object sender, EventArgs e)
        {
            try
            {
                DataBaseClass db = new DataBaseClass();

                DataTable table = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("Select Логин, Пароль, [Номер телефона] From Пользователь where [Код пользователя] = " + userid, db.GetConnection());
                sda.Fill(table);

                if (table.Rows.Count == 1)
                {
                    LoginTB.Text = table.Rows[0].Field<string>(0);
                    PasswordTB.Text = table.Rows[0].Field<string>(1);
                    PhoneTB.Text = table.Rows[0].Field<string>(2);
                }
                else
                {
                    MessageBox.Show("Что-то пошло не так!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch { }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string login, pass, phone;
            login = LoginTB.Text;
            pass = PasswordTB.Text;
            phone = PhoneTB.Text;
            DialogResult dr = MessageBox.Show("Вы точно хотите сохранить изменения?", "Сохранение", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string updateuserinfo = "UPDATE Пользователь SET Логин = '" + login + "', Пароль = '" + pass + "',[Номер телефона] = '" + phone + "' WHERE [Код пользователя] = " + userid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(updateuserinfo, con);
                        MessageBox.Show("Изменения успешно сохранены", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        int number = com.ExecuteNonQuery();
                    }
                }
                catch
                {

                }
            }
        }
    }
}
