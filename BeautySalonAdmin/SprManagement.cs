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
    public partial class SprManagement : Form
    {
        public string conStr = ConfigurationManager.ConnectionStrings["BeatySalonDB"].ConnectionString.ToString();
        int stateid, selectedRow, uslid, matid, holeid, placeid, empid, clid;
        public SprManagement()
        {
            InitializeComponent();
        }

        private void SprManagement_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Зал". При необходимости она может быть перемещена или удалена.
            this.залTableAdapter.Fill(this.adminBeatySalon.Зал);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Статус_сотрудника". При необходимости она может быть перемещена или удалена.
            this.статус_сотрудникаTableAdapter.Fill(this.adminBeatySalon.Статус_сотрудника);
            string se = "SELECT *FROM [Статус сотрудника]";
            string see = "SELECT [Код сотрудника], Фамилия, Имя, Отчество, Пол, [Дата начала работы], [Номер телефона], [Дата рождения], [Наименование статуса] AS [Текущий статус], Зарплата FROM Сотрудник, [Статус сотрудника] WHERE Сотрудник.Cтатус = [Статус сотрудника].[Код статуса]";
            string sc = "SELECT *FROM Клиент";
            string sm = "Select *FROM Услуга";
            string smat = "Select *FROM Материал";
            string sh = "Select *FROM Зал";
            string sp = "SELECT [Код места], [Название зала], [Название места] FROM Зал, Место WHERE Зал.[Код зала] = Место.[Код зала]";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                StateDGW.DataSource = ds.Tables[0];
            }
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(see, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                EmpDGW.DataSource = ds.Tables[0];
            }
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sc, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                ClientDGW.DataSource = ds.Tables[0];
            }
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sm, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                UslDGW.DataSource = ds.Tables[0];
            }
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(smat, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                MatDGW.DataSource = ds.Tables[0];
            }
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sh, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                HallDGW.DataSource = ds.Tables[0];
            }
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sp, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                PlaceDGW.DataSource = ds.Tables[0];
            }
        }

        private void AddState_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string sn = StateNameTB.Text;
                try
                {
                    string ase = "INSERT INTO [Статус сотрудника]([Наименование статуса]) VALUES('" + sn + "')";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ase, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM [Статус сотрудника]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Добавлен статус сотрудника с наименованием " + sn, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        StateDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EditState_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string sn = StateNameTB.Text;
                try
                {
                    string ese = "UPDATE [Статус сотрудника] SET [Наименование статуса] = '" + sn + "' WHERE [Код статуса] = " + stateid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ese, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM [Статус сотрудника]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Изменения успешно сохранены", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        StateDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteState_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dse = "DELETE FROM [Статус сотрудника] WHERE [Код статуса] = " + stateid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dse, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM [Статус сотрудника]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        StateDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void AddUsl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string sn = NameUTB.Text;
                string du = DescUTB.Text;
                double pu = Convert.ToDouble(PriceUTB.Text);
                try
                {
                    string ase = "INSERT INTO Услуга([Название услуги], [Описание услуги], Цена) VALUES('" + sn + "', '" + du + "', " + pu + ")";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ase, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string sm = "Select *FROM Услуга";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sm, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Добавлена услуга " + sn, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UslDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UslDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = UslDGW.Rows[selectedRow];
            uslid = Convert.ToInt32(row.Cells[0].Value);
            NameUTB.Text = row.Cells[1].Value.ToString();
            DescUTB.Text = row.Cells[2].Value.ToString();
            PriceUTB.Text = row.Cells[3].Value.ToString();
        }

        private void DelUsl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dse = "DELETE FROM Услуга WHERE [Код услуги] = " + uslid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dse, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Услуга";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UslDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SWButton_Click(object sender, EventArgs e)
        {
            
        }

        private void BDButton_Click(object sender, EventArgs e)
        {
            
        }

        private void SWDate_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void AddMat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string sn = MatNameTB.Text;
                int pu = Convert.ToInt32(MatCTB.Text);
                try
                {
                    string ase = "INSERT INTO Материал([Название материала], Количество) VALUES('" + sn + "', " + pu + ")";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ase, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string sm = "Select *FROM Материал";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sm, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Добавлен материал " + sn, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MatDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EdMat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string sn = MatNameTB.Text;
                int pu = Convert.ToInt32(MatCTB.Text);
                try
                {
                    string ese = "UPDATE Материал SET [Название материала] = '" + sn + "',  Количество = " + pu + "WHERE [Код материала] = " + matid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ese, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Материал";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Изменения успешно сохранены", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MatDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DelMat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dse = "DELETE FROM Материал WHERE [Код материала] = " + matid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dse, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Материал";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MatDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddHole_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string sn = HallNameTB.Text;
                try
                {
                    string ase = "INSERT INTO Зал([Название зала]) VALUES('" + sn + "')";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ase, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string sm = "Select *FROM Зал";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sm, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Добавлен зал " + sn, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HallDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EdHall_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string sn = HallNameTB.Text;
                try
                {
                    string ese = "UPDATE Зал SET [Название зала] = '" + sn + "' " + "WHERE [Код зала] = " + holeid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ese, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Зал";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Изменения успешно сохранены", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HallDGW.DataSource = ds.Tables[0];
                    }
            }
                catch
            {
                MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        }

        private void DelHall_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dse = "DELETE FROM Зал WHERE [Код зала] = " + holeid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dse, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Зал";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HallDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddPlace_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    DataBaseClass dbase = new DataBaseClass();
                    DataTable mater = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sm = new SqlDataAdapter("Select [Код зала] From Зал where [Название зала] = '" + HallCB.Text + "'", dbase.GetConnection());
                    sm.Fill(mater);

                    if (mater.Rows.Count == 1)
                    {
                        mcil.Text = Convert.ToString(mater.Rows[0].Field<int>(0));
                    }
                    else
                    {
                        MessageBox.Show("Что-то пошло не так!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    int hid = Convert.ToInt32(mcil.Text);
                    string mn = NamePlaceTB.Text;
                    string ase = "INSERT INTO Место([Код зала], [Название места]) VALUES(" + hid + ", '" + mn + "')";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ase, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string sp = "SELECT [Код места], [Название зала], [Название места] FROM Зал, Место WHERE Зал.[Код зала] = Место.[Код зала]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sp, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Добавлено место " + mn, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PlaceDGW.DataSource = ds.Tables[0];
                    }
            }
                catch
            {
                MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        }

        private void HallCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            NowHallTB.Text = HallCB.Text;
        }

        private void EdPlace_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string mn = NamePlaceTB.Text;
                try
                {
                    string hn = NowHallTB.Text;
                    DataBaseClass dbase = new DataBaseClass();
                    DataTable mater = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sm = new SqlDataAdapter("Select [Код зала] From Зал where [Название зала] = '" + hn + "'", dbase.GetConnection());
                    sm.Fill(mater);

                    if (mater.Rows.Count == 1)
                    {
                        mcil.Text = Convert.ToString(mater.Rows[0].Field<int>(0));
                    }
                    else
                    {
                        MessageBox.Show("Что-то пошло не так!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    int hid = Convert.ToInt32(mcil.Text);
                    string ese = "UPDATE Место SET [Название места] = '" + mn + "', [Код зала] = " + hid + "WHERE [Код места] = " + placeid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ese, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string sp = "SELECT [Код места], [Название зала], [Название места] FROM Зал, Место WHERE Зал.[Код зала] = Место.[Код зала]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sp, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Изменения успешно сохранены", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PlaceDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DelPlace_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dse = "DELETE FROM Место WHERE [Код места] = " + placeid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dse, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string sp = "SELECT [Код места], [Название зала], [Название места] FROM Зал, Место WHERE Зал.[Код зала] = Место.[Код зала]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sp, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PlaceDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddEmp_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string sd = Convert.ToString(StartDatePicker.Value.ToShortDateString());
                string dob = Convert.ToString(EmpBDPicker.Value.ToShortDateString());
                DateTime sw = Convert.ToDateTime(sd);
                DateTime BirthDay = Convert.ToDateTime(dob);
                DataBaseClass dbase = new DataBaseClass();
                    DataTable state = new DataTable();
                    SqlDataAdapter sstate = new SqlDataAdapter("Select [Код статуса] From [Статус сотрудника] where [Наименование статуса] = '" + NowStateETB.Text + "'", dbase.GetConnection());
                    sstate.Fill(state);

                    if (state.Rows.Count == 1)
                    {
                        sil.Text = Convert.ToString(state.Rows[0].Field<int>(0));
                    }
                int sid = Convert.ToInt32(sil.Text);
                double sal = Convert.ToDouble(SalaryTB.Text);
                    string ws = sw.ToString("MM.dd.yyyy");
                string bird = BirthDay.ToString("MM.dd.yyyy");
               
                    string ai = "INSERT INTO [dbo].[Сотрудник] ([Фамилия], [Имя], [Отчество], [Пол], [Дата начала работы], [Номер телефона], [Дата рождения], [Cтатус], [Зарплата]) VALUES(@fam, @nam, @ot, @gen, '" + ws + "', @ph, '" + bird + "', @sid, @sal)";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ai, con);
                        com.Parameters.Add("@fam", SqlDbType.VarChar).Value = EFTB.Text;
                        com.Parameters.Add("@nam", SqlDbType.VarChar).Value = ENTB.Text;
                        com.Parameters.Add("@ot", SqlDbType.VarChar).Value = EOTB.Text;
                        com.Parameters.Add("@gen", SqlDbType.VarChar).Value = EGCB.Text;
                        com.Parameters.Add("@ph", SqlDbType.VarChar).Value = EPTB.Text;
                        com.Parameters.Add("@sid", SqlDbType.Int).Value = sid;
                        com.Parameters.Add("@sal", SqlDbType.Money).Value = sal;
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT [Код сотрудника], Фамилия, Имя, Отчество, Пол, [Дата начала работы], [Номер телефона], [Дата рождения], [Наименование статуса] AS [Текущий статус], Зарплата FROM Сотрудник, [Статус сотрудника] WHERE Сотрудник.Cтатус = [Статус сотрудника].[Код статуса]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        EmpDGW.DataSource = ds.Tables[0];
                        MessageBox.Show("Добавлен сотрудник " + EFTB.Text + " " + ENTB.Text + " " + EOTB.Text + " с датой рождения " + dob + ", устроившийся на работу " + sd, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                    }
            }
                catch
            {
                MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        }

        private void NewStateTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            NowStateETB.Text = NewStateCB.Text;
        }

        private void EdEmp_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    DataBaseClass dbase = new DataBaseClass();
                    DataTable state = new DataTable();
                    SqlDataAdapter sstate = new SqlDataAdapter("Select [Код статуса] From [Статус сотрудника] where [Наименование статуса] = '" + NowStateETB.Text + "'", dbase.GetConnection());
                    sstate.Fill(state);

                    if (state.Rows.Count == 1)
                    {
                        sil.Text = Convert.ToString(state.Rows[0].Field<int>(0));
                    }
                    int sid = Convert.ToInt32(sil.Text);
                    double sal = Convert.ToDouble(SalaryTB.Text);
                    string ai = "UPDATE [dbo].[Сотрудник] SET [Фамилия] = @fam, [Имя] = @nam, [Отчество] = @ot, [Номер телефона] = @ph, [Cтатус] = @sid, [Зарплата] = @sal WHERE [Код сотрудника] = " + empid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ai, con);
                        com.Parameters.Add("@fam", SqlDbType.VarChar).Value = EFTB.Text;
                        com.Parameters.Add("@nam", SqlDbType.VarChar).Value = ENTB.Text;
                        com.Parameters.Add("@ot", SqlDbType.VarChar).Value = EOTB.Text;
                        com.Parameters.Add("@ph", SqlDbType.VarChar).Value = EPTB.Text;
                        com.Parameters.Add("@sid", SqlDbType.Int).Value = sid;
                        com.Parameters.Add("@sal", SqlDbType.Money).Value = sal;
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT [Код сотрудника], Фамилия, Имя, Отчество, Пол, [Дата начала работы], [Номер телефона], [Дата рождения], [Наименование статуса] AS [Текущий статус], Зарплата FROM Сотрудник, [Статус сотрудника] WHERE Сотрудник.Cтатус = [Статус сотрудника].[Код статуса]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        EmpDGW.DataSource = ds.Tables[0];
                        MessageBox.Show("Изменения успешно сохранены", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DelEmp_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dse = "DELETE FROM Сотрудник WHERE [Код сотрудника] = " + empid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dse, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT [Код сотрудника], Фамилия, Имя, Отчество, Пол, [Дата начала работы], [Номер телефона], [Дата рождения], [Наименование статуса] AS [Текущий статус], Зарплата FROM Сотрудник, [Статус сотрудника] WHERE Сотрудник.Cтатус = [Статус сотрудника].[Код статуса]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        EmpDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddClient_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dob = Convert.ToString(ClientBDPicker.Value.ToShortDateString());
                    DateTime BirthDay = Convert.ToDateTime(dob);
                    DataBaseClass dbase = new DataBaseClass();
                    string bird = BirthDay.ToString("MM.dd.yyyy");
                    string ai = "INSERT INTO [dbo].[Клиент] ([Фамилия], [Имя], [Отчество], [Пол], [Номер телефона], [Дата рождения]) VALUES(@fam, @nam, @ot, @gen, @ph, '" + bird + "')";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ai, con);
                        com.Parameters.Add("@fam", SqlDbType.VarChar).Value = CFTB.Text;
                        com.Parameters.Add("@nam", SqlDbType.VarChar).Value = CNTB.Text;
                        com.Parameters.Add("@ot", SqlDbType.VarChar).Value = COTB.Text;
                        com.Parameters.Add("@gen", SqlDbType.VarChar).Value = CGCB.Text;
                        com.Parameters.Add("@ph", SqlDbType.VarChar).Value = CPTB.Text;
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Клиент";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        ClientDGW.DataSource = ds.Tables[0];
                        MessageBox.Show("Добавлен клиент " + CFTB.Text + " " + CNTB.Text + " " + COTB.Text + " с датой рождения " + dob, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
            }
                catch
            {
                MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        }

        private void ClientDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = ClientDGW.Rows[selectedRow];
            clid = Convert.ToInt32(row.Cells[0].Value);
            CFTB.Text = row.Cells[1].Value.ToString();
            CNTB.Text = row.Cells[2].Value.ToString();
            COTB.Text = row.Cells[3].Value.ToString();
            CPTB.Text = row.Cells[5].Value.ToString();
        }

        private void DelClient_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dse = "DELETE FROM Клиент WHERE [Код клиента] = " + clid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dse, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Клиент";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClientDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EdClient_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    DataBaseClass dbase = new DataBaseClass();
                    string ai = "UPDATE [dbo].[Клиент] SET [Фамилия] = @fam, [Имя] = @nam, [Отчество] = @ot, [Номер телефона] = @ph WHERE [Код клиента] = " + clid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ai, con);
                        com.Parameters.Add("@fam", SqlDbType.VarChar).Value = CFTB.Text;
                        com.Parameters.Add("@nam", SqlDbType.VarChar).Value = CNTB.Text;
                        com.Parameters.Add("@ot", SqlDbType.VarChar).Value = COTB.Text;
                        com.Parameters.Add("@gen", SqlDbType.VarChar).Value = CGCB.Text;
                        com.Parameters.Add("@ph", SqlDbType.VarChar).Value = CPTB.Text;
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Клиент";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        ClientDGW.DataSource = ds.Tables[0];
                        MessageBox.Show("Изменения успешно сохранены", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EmpDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = EmpDGW.Rows[selectedRow];
            empid = Convert.ToInt32(row.Cells[0].Value);
            EFTB.Text = row.Cells[1].Value.ToString();
            ENTB.Text = row.Cells[2].Value.ToString();
            EOTB.Text = row.Cells[3].Value.ToString();
            EPTB.Text = row.Cells[6].Value.ToString();
            NowStateETB.Text = row.Cells[8].Value.ToString();
            SalaryTB.Text = row.Cells[9].Value.ToString();
        }

        private void PlaceDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = PlaceDGW.Rows[selectedRow];
            placeid = Convert.ToInt32(row.Cells[0].Value);
            NowHallTB.Text = row.Cells[1].Value.ToString();
            NamePlaceTB.Text = row.Cells[2].Value.ToString();
        }

        private void HoleDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = HallDGW.Rows[selectedRow];
            holeid = Convert.ToInt32(row.Cells[0].Value);
            HallNameTB.Text = row.Cells[1].Value.ToString();
        }

        private void MatDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = MatDGW.Rows[selectedRow];
            matid = Convert.ToInt32(row.Cells[0].Value);
            MatNameTB.Text = row.Cells[1].Value.ToString();
            MatCTB.Text = row.Cells[2].Value.ToString();
        }

        private void EdUsl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                string sn = NameUTB.Text;
                string du = DescUTB.Text;
                double pu = Convert.ToDouble(PriceUTB.Text);
                try
                {
                    string ese = "UPDATE Услуга SET [Название услуги] = '" + sn + "', [Описание услуги] = '" + du + "', Цена = " + pu + "WHERE [Код услуги] = " + uslid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ese, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM Услуга";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Изменения успешно сохранены", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UslDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void StateDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = StateDGW.Rows[selectedRow];
            stateid = Convert.ToInt32(row.Cells[0].Value);
            StateNameTB.Text = row.Cells[1].Value.ToString();
        }
    }
}
