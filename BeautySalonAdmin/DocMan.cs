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
    public partial class DocMan : Form
    {
        public string conStr = ConfigurationManager.ConnectionStrings["BeatySalonDB"].ConnectionString.ToString();
        string InvDate, InvTime;
        int invid, selectedRow, matid, mc, entid, sr;
        public DocMan()
        {
            InitializeComponent();
        }

        private void InvoiceTime_ValueChanged(object sender, EventArgs e)
        {
            InvoiceDateTB.Text = DateOfInvoice.SelectionStart.ToShortDateString().ToString() + " " + InvoiceTime.Value.ToShortTimeString().ToString();
        }

        private void InvoiceDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = InvoiceDGW.Rows[selectedRow];
            invid = Convert.ToInt32(row.Cells[0].Value);
            InvoiceDateTB.Text = row.Cells[1].Value.ToString();
            string sim = "SELECT [Дата прихода], [Название материала], [Количество материала], [Цена материала], Сумма FROM [Приходная накладная], [Приход материалов], Материал WHERE [Приход материалов].[Приходная накладная] = " + invid + " AND [Приход материалов].Материал = Материал.[Код материала] AND [Приход материалов].[Приходная накладная] = [Приходная накладная].[Код приходной накладной]";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sim, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                MaterialInvoiceDGV.DataSource = ds.Tables[0];
            }
        }

        private void AddInvoice_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                DateTime ind = Convert.ToDateTime(InvoiceDateTB.Text);

                try
                {
                    string ai = "INSERT INTO [Приходная накладная]([Дата прихода]) VALUES('" + ind.ToString("MM.dd.yyyy HH:mm") + "')";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ai, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string si = "SELECT *FROM [Приходная накладная]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(si, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Добавлена приходная накладная с датой " + ind, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvoiceDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateInvoiceDate_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                DateTime ind = Convert.ToDateTime(InvoiceDateTB.Text);
                try
                {
                    string uin = "UPDATE [Приходная накладная] SET [Дата прихода] = '" + ind.ToString("MM.dd.yyyy HH:mm") + "' WHERE [Код приходной накладной] = " + invid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(uin, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string si = "SELECT *FROM [Приходная накладная]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(si, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Изменения успешно сохранены", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvoiceDGW.DataSource = ds.Tables[0];
                    }
                    string sim = "SELECT [Дата прихода], [Название материала], [Количество материала], [Цена материала], Сумма FROM [Приходная накладная], [Приход материалов], Материал WHERE [Приход материалов].[Приходная накладная] = " + invid + " AND [Приход материалов].Материал = Материал.[Код материала] AND [Приход материалов].[Приходная накладная] = [Приходная накладная].[Код приходной накладной]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sim, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MaterialInvoiceDGV.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DelInvoice_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dsi = "DELETE FROM [Приходная накладная] WHERE [Код приходной накладной] = " + invid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dsi, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT *FROM [Приходная накладная]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvoiceDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PriceTB_TextChanged(object sender, EventArgs e)
        {
            int c;
            double p, s;
            try
            {
                p = Convert.ToDouble(PriceTB.Text);
                c = Convert.ToInt32(CITB.Text);
                s = p * c;
                SumTB.Text = Convert.ToString(s);
            }
            catch
            {

            }
        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.местоTableAdapter.FillBy(this.adminBeatySalon.Место);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void HoleCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MestoCB.Items.Clear();
                DataBaseClass db = new DataBaseClass();
                DataTable hol = new DataTable();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter sh = new SqlDataAdapter("Select [Код зала] From Зал where [Название зала] = '" + HoleCB.Text + "'", db.GetConnection());
                sh.Fill(hol);

                if (hol.Rows.Count == 1)
                {
                    HoleID.Text = Convert.ToString(hol.Rows[0].Field<int>(0));
                }
                int h = Convert.ToInt32(HoleID.Text);
                string sme = "SELECT Место.[Название места] FROM Место, Зал WHERE Место.[Код зала] = Зал.[Код зала] AND Зал.[Код зала] = @hid";
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(sme, con);
                    com.Parameters.Add("@hid", SqlDbType.Int).Value = h;
                    DataTable mesta = new DataTable();
                    mesta.Load(com.ExecuteReader());
                    DataRow dr = null;
                    for (int i = 0; i < mesta.Rows.Count; i++)
                    {
                        dr = mesta.Rows[i];
                        MestoCB.Items.Add(dr[0].ToString());
                    }
                    con.Close();
                }
            }
            catch
            {
                
            }
        }

        private void CLCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataBaseClass db = new DataBaseClass();
                DataTable CL = new DataTable();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter sh = new SqlDataAdapter("Select Фамилия, Имя, Отчество From Клиент where [Код клиента] = " + Convert.ToInt32(CLCB.Text), db.GetConnection());
                sh.Fill(CL);

                if (CL.Rows.Count == 1)
                {
                    FIOCLTB.Text = CL.Rows[0].Field<string>(0) + " " + CL.Rows[0].Field<string>(1) + " " + CL.Rows[0].Field<string>(2);
                }
            }
            catch
            {
              
            }
        }

        private void TimeEntr_ValueChanged(object sender, EventArgs e)
        {
             DateTimeEnterTB.Text = DateEntr.SelectionStart.ToShortDateString().ToString() + " " + TimeEntr.Value.ToShortTimeString().ToString();
        }

        private void DateEntr_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTimeEnterTB.Text = DateEntr.SelectionStart.ToShortDateString().ToString() + " " + TimeEntr.Value.ToShortTimeString().ToString();
        }

        private void AddEntr_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                DateTime ind = Convert.ToDateTime(DateTimeEnterTB.Text);

                try
                {
                    DataBaseClass db = new DataBaseClass();
                    DataTable hol = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sh = new SqlDataAdapter("Select [Код зала] From Зал where [Название зала] = '" + HoleCB.Text + "'", db.GetConnection());
                    sh.Fill(hol);
                    if (hol.Rows.Count == 1)
                    {
                        HoleID.Text = Convert.ToString(hol.Rows[0].Field<int>(0));
                    }
                    DataTable mes = new DataTable();
                    SqlDataAdapter sm = new SqlDataAdapter("Select [Код места] From Место where [Название места] = '" + MestoCB.Text + "'", db.GetConnection());
                    sm.Fill(mes);
                    if (mes.Rows.Count == 1)
                    {
                       Mestid.Text = Convert.ToString(hol.Rows[0].Field<int>(0));
                    }
                    int h = Convert.ToInt32(HoleID.Text);
                    int m = Convert.ToInt32(Mestid.Text);
                    int c = Convert.ToInt32(CLCB.Text);
                    string ai = "INSERT INTO Запись([Дата записи], [Номер зала], [Номер места], [Код клиента]) VALUES('" + ind.ToString("MM.dd.yyyy HH:mm") + "', @h, @m, @c)";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ai, con);
                        com.Parameters.Add("@h", SqlDbType.Int).Value = h;
                        com.Parameters.Add("@m", SqlDbType.Int).Value = m;
                        com.Parameters.Add("@c", SqlDbType.Int).Value = c;
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT [Код записи], [Дата записи], [Название зала], [Название места], Фамилия AS [Фамилия клиента], Имя AS [Имя клиента], Отчество AS [Отчество клиента] FROM Запись, Клиент, Место, Зал WHERE Запись.[Номер зала] = Зал.[Код зала] AND Запись.[Номер места] = Место.[Код места] AND Запись.[Код клиента] = Клиент.[Код клиента]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Добавлена запись клиента " + FIOCLTB.Text + " на дату " + ind, "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        EntDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EdEntr_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите изменить запись?", "Редактирование", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                DateTime ind = Convert.ToDateTime(DateTimeEnterTB.Text);

                try
                {
                    DataBaseClass db = new DataBaseClass();
                    DataTable hol = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sh = new SqlDataAdapter("Select [Код зала] From Зал where [Название зала] = '" + HoleCB.Text + "'", db.GetConnection());
                    sh.Fill(hol);
                    if (hol.Rows.Count == 1)
                    {
                        HoleID.Text = Convert.ToString(hol.Rows[0].Field<int>(0));
                    }
                    DataTable mes = new DataTable();
                    SqlDataAdapter sm = new SqlDataAdapter("Select [Код места] From Место where [Название места] = '" + MestoCB.Text + "'", db.GetConnection());
                    sm.Fill(mes);
                    if (mes.Rows.Count == 1)
                    {
                        Mestid.Text = Convert.ToString(hol.Rows[0].Field<int>(0));
                    }
                    int h = Convert.ToInt32(HoleID.Text);
                    int m = Convert.ToInt32(Mestid.Text);
                    int c = Convert.ToInt32(CLCB.Text);
                    string ai = "UPDATE Запись SET [Дата записи] = '" + ind.ToString("MM.dd.yyyy HH:mm") + "', [Номер зала] = @h, [Номер места] = @m, [Код клиента] = @c WHERE [Код записи] = " + entid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(ai, con);
                        com.Parameters.Add("@h", SqlDbType.Int).Value = h;
                        com.Parameters.Add("@m", SqlDbType.Int).Value = m;
                        com.Parameters.Add("@c", SqlDbType.Int).Value = c;
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT [Код записи], [Дата записи], [Название зала], [Название места], Фамилия AS [Фамилия клиента], Имя AS [Имя клиента], Отчество AS [Отчество клиента] FROM Запись, Клиент, Место, Зал WHERE Запись.[Номер зала] = Зал.[Код зала] AND Запись.[Номер места] = Место.[Код места] AND Запись.[Код клиента] = Клиент.[Код клиента]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Изменения успешно сохранены!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        EntDGW.DataSource = ds.Tables[0];
                    }
            }
                catch
            {
                MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        }

        private void MastCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataBaseClass db = new DataBaseClass();
                DataTable CL = new DataTable();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter sh = new SqlDataAdapter("Select Фамилия, Имя, Отчество From Сотрудник where [Код сотрудника] = " + Convert.ToInt32(MastCB.Text), db.GetConnection());
                sh.Fill(CL);

                if (CL.Rows.Count == 1)
                {
                    FIOMCB.Text = CL.Rows[0].Field<string>(0) + " " + CL.Rows[0].Field<string>(1) + " " + CL.Rows[0].Field<string>(2);
                }
            }
            catch
            {

            }
        }

        private void AddMSButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    DataBaseClass db = new DataBaseClass();
                    DataTable mater = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sm = new SqlDataAdapter("Select [Код услуги] From Услуга where [Название услуги] = '" + ServCB.Text + "'", db.GetConnection());
                    sm.Fill(mater);

                    if (mater.Rows.Count == 1)
                    {
                        serid.Text = Convert.ToString(mater.Rows[0].Field<int>(0));
                    }
                    else
                    {
                        MessageBox.Show("Что-то пошло не так!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    int m = Convert.ToInt32(MastCB.Text);
                    int s = Convert.ToInt32(serid.Text);
                    string aim = "INSERT INTO [Оказанная услуга](Запись, Услуга, Мастер) VALUES(@ent, @s, @m)";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(aim, con);
                        com.Parameters.Add("@ent", SqlDbType.Int).Value = entid;
                        com.Parameters.Add("@s", SqlDbType.Int).Value = s;
                        com.Parameters.Add("@m", SqlDbType.Int).Value = m;
                        int number = com.ExecuteNonQuery();
                        MessageBox.Show("Запись успешно добавлена!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        con.Close();
                    }
                    string se = "SELECT [Дата записи], [Название услуги], Фамилия AS [Фамилия мастера], Имя AS [Имя мастера], Отчество AS [Отчество мастера] FROM Запись, Сотрудник, Услуга, [Оказанная услуга] WHERE Запись.[Код записи] = [Оказанная услуга].Запись AND [Оказанная услуга].Услуга = Услуга.[Код услуги] AND [Оказанная услуга].Мастер = Сотрудник.[Код сотрудника] AND [Оказанная услуга].Запись = " + entid;
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MakeServDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddUsMButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    DataBaseClass db = new DataBaseClass();
                    DataTable mater = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sm = new SqlDataAdapter("Select [Код материала], Количество From Материал where [Название материала] = '" + USMCB.Text + "'", db.GetConnection());
                    sm.Fill(mater);

                    if (mater.Rows.Count == 1)
                    {
                        mcil.Text = Convert.ToString(mater.Rows[0].Field<int>(0));
                        matsprc.Text = Convert.ToString(mater.Rows[0].Field<int>(1));
                    }
                    else
                    {
                        MessageBox.Show("Что-то пошло не так!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    int tecmc = Convert.ToInt32(matsprc.Text);
                    int usmatc = Convert.ToInt32(USMCTB.Text);
                    if (tecmc < usmatc)
                    {
                        int nh = usmatc - tecmc;
                        MessageBox.Show("На складе не хвтает " + nh + " ед. материала " + USMCB.Text + "!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        int newmc = tecmc - usmatc;
                        int m = Convert.ToInt32(mcil.Text);
                        string aim = "INSERT INTO [Использование материалов](Материал, Запись, [Количество материала]) VALUES(@mat, @ent, @cm)";
                        using (SqlConnection con = new SqlConnection(conStr))
                        {
                            con.Open();
                            SqlCommand com = new SqlCommand(aim, con);
                            com.Parameters.Add("@mat", SqlDbType.Int).Value = m;
                            com.Parameters.Add("@ent", SqlDbType.Int).Value = entid;
                            com.Parameters.Add("@cm", SqlDbType.Int).Value = usmatc;
                            int number = com.ExecuteNonQuery();
                            MessageBox.Show("Запись успешно добавлена!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            con.Close();
                        }
                        string usm = "SELECT [Дата записи], [Название материала], [Количество материала] FROM Запись, Материал, [Использование материалов] WHERE Запись.[Код записи] = [Использование материалов].Запись AND [Использование материалов].Материал = Материал.[Код материала] AND [Использование материалов].Запись = " + entid;
                        using (SqlConnection connection = new SqlConnection(conStr))
                        {
                            connection.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter(usm, connection);
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);
                            USMDGW.DataSource = ds.Tables[0];
                        }
                        string umc = "UPDATE Материал SET Количество = '" + newmc + "' WHERE [Код материала] = " + m;
                        using (SqlConnection cons = new SqlConnection(conStr))
                        {
                            cons.Open();
                            SqlCommand com = new SqlCommand(umc, cons);
                            int number = com.ExecuteNonQuery();
                            cons.Close();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DateOfInvoice_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void EntDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sr = e.RowIndex;
            DataGridViewRow row = EntDGW.Rows[sr];
            entid = Convert.ToInt32(row.Cells[0].Value);
            DateTimeEnterTB.Text = row.Cells[1].Value.ToString();
            string se = "SELECT [Дата записи], [Название услуги], Фамилия AS [Фамилия мастера], Имя AS [Имя мастера], Отчество AS [Отчество мастера] FROM Запись, Сотрудник, Услуга, [Оказанная услуга] WHERE Запись.[Код записи] = [Оказанная услуга].Запись AND [Оказанная услуга].Услуга = Услуга.[Код услуги] AND [Оказанная услуга].Мастер = Сотрудник.[Код сотрудника] AND [Оказанная услуга].Запись = " + entid;
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                MakeServDGW.DataSource = ds.Tables[0];
            }
            string usm = "SELECT [Дата записи], [Название материала], [Количество материала] FROM Запись, Материал, [Использование материалов] WHERE Запись.[Код записи] = [Использование материалов].Запись AND [Использование материалов].Материал = Материал.[Код материала] AND [Использование материалов].Запись = " + entid;
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(usm, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                USMDGW.DataSource = ds.Tables[0];
            }
        }

        private void DelEntr_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string dsi = "DELETE FROM [Запись] WHERE [Код записи] = " + entid;
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(dsi, con);
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string se = "SELECT [Код записи], [Дата записи], [Название зала], [Название места], Фамилия AS [Фамилия клиента], Имя AS [Имя клиента], Отчество AS [Отчество клиента] FROM Запись, Клиент, Место, Зал WHERE Запись.[Номер зала] = Зал.[Код зала] AND Запись.[Номер места] = Место.[Код места] AND Запись.[Код клиента] = Клиент.[Код клиента]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно удалена!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        EntDGW.DataSource = ds.Tables[0];
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CITB_TextChanged(object sender, EventArgs e)
        {
            int c;
            double p, s;
            try
            {
                p = Convert.ToDouble(PriceTB.Text);
                c = Convert.ToInt32(CITB.Text);
                s = p * c;
                SumTB.Text = Convert.ToString(s);
            }
            catch
            {

            }
        }

        private void AddIMButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы точно хотите добавить запись?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    DataBaseClass db = new DataBaseClass();
                    DataTable mater = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sm = new SqlDataAdapter("Select Количество, [Код материала] From Материал where [Название материала] = '" + MateICB.Text + "'", db.GetConnection());
                    sm.Fill(mater);

                    if (mater.Rows.Count == 1)
                    {
                        mcil.Text = Convert.ToString(mater.Rows[0].Field<int>(0));
                        mcid.Text = Convert.ToString(mater.Rows[0].Field<int>(1));
                    }
                    else
                    {
                        MessageBox.Show("Что-то пошло не так!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    DataTable inv = new DataTable();
 
                    int cm = Convert.ToInt32(CITB.Text);
                    int m = Convert.ToInt32(mcil.Text);
                    int mid = Convert.ToInt32(mcid.Text);
                    int newmc = m + Convert.ToInt32(CITB.Text);
                    double pm = Convert.ToDouble(PriceTB.Text);
                    double sum = Convert.ToDouble(SumTB.Text);
                    string aim = "INSERT INTO [Приход материалов](Материал, [Приходная накладная], [Количество материала], [Цена материала], Сумма) VALUES(@mat, @inv, @cm, @pm, @s)";
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand(aim, con);
                        com.Parameters.Add("@mat", SqlDbType.Int).Value = mid;
                        com.Parameters.Add("@inv", SqlDbType.Int).Value = invid;
                        com.Parameters.Add("@cm", SqlDbType.Int).Value = cm;
                        com.Parameters.Add("@pm", SqlDbType.Money).Value = pm;
                        com.Parameters.Add("@s", SqlDbType.Money).Value = sum;
                        int number = com.ExecuteNonQuery();
                        con.Close();
                    }
                    string sim = "SELECT [Дата прихода], [Название материала], [Количество материала], [Цена материала], Сумма FROM [Приходная накладная], [Приход материалов], Материал WHERE [Приход материалов].[Приходная накладная] = " + invid + " AND [Приход материалов].Материал = Материал.[Код материала] AND [Приход материалов].[Приходная накладная] = [Приходная накладная].[Код приходной накладной]";
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sim, connection);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        MessageBox.Show("Запись успешно добавлена!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MaterialInvoiceDGV.DataSource = ds.Tables[0];
                    }
                    string umc = "UPDATE Материал SET Количество = '" + newmc + "' WHERE [Код материала] = " + mid;
                    using (SqlConnection cons = new SqlConnection(conStr))
                    {
                        cons.Open();
                        SqlCommand com = new SqlCommand(umc, cons);
                        int number = com.ExecuteNonQuery();
                        cons.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DateOfInvoice_DateSelected(object sender, DateRangeEventArgs e)
        {
            InvoiceDateTB.Text = DateOfInvoice.SelectionStart.ToShortDateString().ToString() + " " + InvoiceTime.Value.ToShortTimeString().ToString();
        }

        private void DocMan_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.adminBeatySalon.Сотрудник);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Услуга". При необходимости она может быть перемещена или удалена.
            this.услугаTableAdapter.Fill(this.adminBeatySalon.Услуга);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Запись". При необходимости она может быть перемещена или удалена.
            this.записьTableAdapter.Fill(this.adminBeatySalon.Запись);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Использование_материалов". При необходимости она может быть перемещена или удалена.
            this.использование_материаловTableAdapter.Fill(this.adminBeatySalon.Использование_материалов);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Место". При необходимости она может быть перемещена или удалена.
            this.местоTableAdapter.Fill(this.adminBeatySalon.Место);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Зал". При необходимости она может быть перемещена или удалена.
            this.залTableAdapter.Fill(this.adminBeatySalon.Зал);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Клиент". При необходимости она может быть перемещена или удалена.
            this.клиентTableAdapter.Fill(this.adminBeatySalon.Клиент);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Материал". При необходимости она может быть перемещена или удалена.
            this.материалTableAdapter.Fill(this.adminBeatySalon.Материал);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Приходная_накладная". При необходимости она может быть перемещена или удалена.
            this.приходная_накладнаяTableAdapter.Fill(this.adminBeatySalon.Приходная_накладная);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Приходная_накладная". При необходимости она может быть перемещена или удалена.
            DateTimeEnterTB.Text = DateEntr.SelectionStart.ToShortDateString().ToString() + " " + TimeEntr.Value.ToShortTimeString().ToString();
            InvoiceDateTB.Text = DateOfInvoice.SelectionStart.ToShortDateString().ToString() + " " + InvoiceTime.Value.ToShortTimeString().ToString();
            string si = "SELECT *FROM [Приходная накладная]";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(si, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                InvoiceDGW.DataSource = ds.Tables[0];
            }
            string se = "SELECT [Код записи], [Дата записи], [Название зала], [Название места], Фамилия AS [Фамилия клиента], Имя AS [Имя клиента], Отчество AS [Отчество клиента] FROM Запись, Клиент, Место, Зал WHERE Запись.[Номер зала] = Зал.[Код зала] AND Запись.[Номер места] = Место.[Код места] AND Запись.[Код клиента] = Клиент.[Код клиента]";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                EntDGW.DataSource = ds.Tables[0];
            }
        }
    }
}
