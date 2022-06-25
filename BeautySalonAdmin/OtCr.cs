using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace BeautySalonAdmin
{
    public partial class OtCr : Form
    {
        public string conStr = ConfigurationManager.ConnectionStrings["BeatySalonDB"].ConnectionString.ToString();
        private string TemplateFileName;
        int selectedRow;
        public OtCr()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Текстовые документы (*.docx)|*.docx|Все файлы (*.*)|*.*";
        }

        private void OtCr_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Запись". При необходимости она может быть перемещена или удалена.
            this.записьTableAdapter.Fill(this.adminBeatySalon.Запись);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Сотрудник". При необходимости она может быть перемещена или удалена.
            this.сотрудникTableAdapter.Fill(this.adminBeatySalon.Сотрудник);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "adminBeatySalon.Приходная_накладная". При необходимости она может быть перемещена или удалена.
            this.приходная_накладнаяTableAdapter.Fill(this.adminBeatySalon.Приходная_накладная);
            string se = "SELECT [Код сотрудника], Фамилия, Имя, Отчество, Пол, [Дата начала работы], [Номер телефона], [Дата рождения], [Наименование статуса], Зарплата FROM Сотрудник, [Статус сотрудника] WHERE Сотрудник.Cтатус = [Статус сотрудника].[Код статуса]";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(se, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                EmpDGW.DataSource = ds.Tables[0];
            }
            string sm = "SELECT   [Название материала], SUM([Количество материала]) AS Количество FROM     Материал, [Использование материалов] WHERE Материал.[Код материала] = [Использование материалов].Материал GROUP BY [Название материала] ORDER BY 2 DESC";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sm, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                USMODGW.DataSource = ds.Tables[0];
            }
            string sserv = "SELECT   [Название услуги], COUNT(Услуга) AS Количество FROM Услуга, [Оказанная услуга] WHERE Услуга.[Код услуги] = [Оказанная услуга].Услуга GROUP BY [Название услуги] ORDER BY 2 DESC";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sserv, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                MAKESERVODGW.DataSource = ds.Tables[0];
            }
        }

        private void EmpDGW_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedRow = e.RowIndex;
                DataGridViewRow row = EmpDGW.Rows[selectedRow];
                EIDTB.Text = row.Cells[0].Value.ToString();
                FIOTB.Text = row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString() + " " + row.Cells[3].Value.ToString();
                GenderTB.Text = row.Cells[4].Value.ToString();
                StartTB.Text = row.Cells[5].Value.ToString();
                PhoneTB.Text = row.Cells[6].Value.ToString();
                BDTB.Text = row.Cells[7].Value.ToString();
                StateTB.Text = row.Cells[8].Value.ToString();
                SalaryTB.Text = row.Cells[9].Value.ToString();
            }
            catch
            {

            }
        }

        private void MakeECButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            TemplateFileName = openFileDialog1.FileName;
            try
            {
                var eid = EIDTB.Text;
                var nam = FIOTB.Text;
                var gen = GenderTB.Text;
                var start = StartTB.Text;
                var p = PhoneTB.Text;
                var b = BDTB.Text;
                var s = StateTB.Text;
                var z = SalaryTB.Text + " рублей";
                var wordApp = new Word.Application();
                wordApp.Visible = false;
                var wordDocument = wordApp.Documents.Open(TemplateFileName);
                RWS("{TabNum}", eid, wordDocument);
                RWS("{EmpName}", nam, wordDocument);
                RWS("{Gender} ", gen, wordDocument);
                RWS("{StartWork} ", start, wordDocument);
                RWS("{Phone}", p, wordDocument);
                RWS("{BirthDate}", b, wordDocument);
                RWS("{State}", s, wordDocument);
                RWS("{Sal}", z, wordDocument);
                wordDocument.SaveAs(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Карточка сотрудника " + eid + ".docx"));
                wordApp.Visible = true;
            }
            catch { }
        }
        private void RWS(string STR, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: STR, ReplaceWith: text);
        }

        private void MakeMatButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF (*.pdf)|*.pdf";
            save.FileName = ReportControl.TabPages[1].Text;
            //save.FileName = "Отчет по использованию материалов";
            if (save.ShowDialog() == DialogResult.OK)
            {
                string FONT_LOCATION = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
                BaseFont baseFont = BaseFont.CreateFont(FONT_LOCATION, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                //iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont);
                iTextSharp.text.Font header = new iTextSharp.text.Font(baseFont, 14f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                PdfPTable table = new PdfPTable(USMODGW.ColumnCount);
                for (int f = 0; f < USMODGW.ColumnCount; f++)
                {
                    PdfPCell CellTwoHdr = new PdfPCell(new Phrase(USMODGW.Columns[f].HeaderText, header));
                    CellTwoHdr.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(CellTwoHdr);
                }
                for (int i = 0; i <= USMODGW.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < USMODGW.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(USMODGW.Rows[i].Cells[j].Value.ToString(), font));
                    }
                }
                using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A2, 40f, 40f, 40f, 40f);
                        // = PdfWriter.GetInstance(pdfDoc, ms);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        BaseFont baseFont1 = BaseFont.CreateFont("C:\\Windows\\Fonts\\timesbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        //iTextSharp.text.Font fontParagraph = new iTextSharp.text.Font(baseFont, 17, iTextSharp.text.Font.BOLD);
                        pdfDoc.Open();
                        //Заголовок 
                        PdfContentByte cb = writer.DirectContent;
                        //cb.Rectangle(10f, 10f, pdfDoc.PageSize.Width - 20f, pdfDoc.PageSize.Height - 20f);
                        cb.SetFontAndSize(baseFont1, 16);
                        cb.BeginText();
                        cb.ShowTextAligned(
                        PdfContentByte.ALIGN_LEFT, "Отчёт по использованию материалов", 450f, pdfDoc.PageSize.Height - 30f, 0);
                        cb.EndText();
                        cb.Stroke();
                        pdfDoc.Add(table);
                        pdfDoc.Close();
                        stream.Close();
                        MessageBox.Show("Экспорт завершен!", "Формирование отчета", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            //MessageBox.Show("На компютере разработчика слетела актвиация MS Office и был утрачен рабочий код!", "Какое несчастье!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
        private void MakeMSOButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF (*.pdf)|*.pdf";
            save.FileName = ReportControl.TabPages[2].Text;
            //save.FileName = "Отчет по использованию материалов";
            if (save.ShowDialog() == DialogResult.OK)
            {
                string FONT_LOCATION = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
                BaseFont baseFont = BaseFont.CreateFont(FONT_LOCATION, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                //iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont);
                iTextSharp.text.Font header = new iTextSharp.text.Font(baseFont, 14f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                PdfPTable table = new PdfPTable(MAKESERVODGW.ColumnCount);
                for (int f = 0; f < MAKESERVODGW.ColumnCount; f++)
                {
                    PdfPCell CellTwoHdr = new PdfPCell(new Phrase(MAKESERVODGW.Columns[f].HeaderText, header));
                    CellTwoHdr.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(CellTwoHdr);
                }
                for (int i = 0; i <= MAKESERVODGW.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < MAKESERVODGW.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(MAKESERVODGW.Rows[i].Cells[j].Value.ToString(), font));
                    }
                }
                using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        Document pdfDoc = new Document(PageSize.A2, 40f, 40f, 40f, 40f);
                        // = PdfWriter.GetInstance(pdfDoc, ms);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        BaseFont baseFont1 = BaseFont.CreateFont("C:\\Windows\\Fonts\\timesbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        //iTextSharp.text.Font fontParagraph = new iTextSharp.text.Font(baseFont, 17, iTextSharp.text.Font.BOLD);
                        pdfDoc.Open();
                        //Заголовок 
                        PdfContentByte cb = writer.DirectContent;
                        table.SetTotalWidth(new float[] { 200, 300 });
                        //cb.Rectangle(10f, 10f, pdfDoc.PageSize.Width - 20f, pdfDoc.PageSize.Height - 20f);
                        cb.SetFontAndSize(baseFont1, 16);
                        cb.BeginText();
                        cb.ShowTextAligned(
                        PdfContentByte.ALIGN_LEFT, "Отчёт по оказанным услугам", 450f, pdfDoc.PageSize.Height - 30f, 0);
                        cb.EndText();
                        cb.Stroke();
                        pdfDoc.Add(table);
                        pdfDoc.Close();
                        stream.Close();
                        MessageBox.Show("Экспорт завершен!", "Формирование отчета", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            //MessageBox.Show("На компютере разработчика слетела актвиация MS Office и был утрачен рабочий код!", "Какое несчастье!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void NumberOfPrihNakl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataBaseClass db = new DataBaseClass();
                DataTable table = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT [Дата прихода] FROM [Приходная накладная] WHERE [Код приходной накладной] = " + Convert.ToInt32(NumberOfPrihNakl.Text), db.GetConnection());
                sda.Fill(table);
                if (table.Rows.Count == 1)
                {
                    InvoiceDate.Text = Convert.ToString(table.Rows[0].Field<DateTime>(0));
                }
                string sim = "SELECT [Название материала], [Количество материала], [Цена материала], Сумма FROM [Приходная накладная], [Приход материалов], Материал WHERE [Приход материалов].[Приходная накладная] = " + Convert.ToInt32(NumberOfPrihNakl.Text) + " AND [Приход материалов].Материал = Материал.[Код материала] AND [Приход материалов].[Приходная накладная] = [Приходная накладная].[Код приходной накладной]";
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sim, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    InvDgv.DataSource = ds.Tables[0];
                }
            }
            catch
            {

            }
        }

        private void EmpNumCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataBaseClass db = new DataBaseClass();
                DataTable table = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Фамилия, Имя, Отчество FROM Сотрудник WHERE [Код сотрудника] = " + Convert.ToInt32(EmpNumCB.Text), db.GetConnection());
                sda.Fill(table);
                if (table.Rows.Count == 1)
                {
                    EmpFIOTB.Text = Convert.ToString(table.Rows[0].Field<string>(0)) + " " + Convert.ToString(table.Rows[0].Field<string>(1)) + " " + Convert.ToString(table.Rows[0].Field<string>(2));
                }
            }
            catch
            {

            }
        }

        private void MakeInvoiceButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF (*.pdf)|*.pdf";
            save.FileName = ReportControl.TabPages[3].Text + "№ " + NumberOfPrihNakl.Text;
            //save.FileName = "Отчет по использованию материалов";
            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string FONT_LOCATION = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
                    BaseFont baseFont = BaseFont.CreateFont(FONT_LOCATION, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    //iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont);
                    iTextSharp.text.Font header = new iTextSharp.text.Font(baseFont, 14f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    PdfPTable table = new PdfPTable(InvDgv.ColumnCount);
                    for (int f = 0; f < InvDgv.ColumnCount; f++)
                    {
                        PdfPCell CellTwoHdr = new PdfPCell(new Phrase(InvDgv.Columns[f].HeaderText, header));
                        CellTwoHdr.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(CellTwoHdr);
                    }
                    for (int i = 0; i <= InvDgv.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < InvDgv.Columns.Count; j++)
                        {
                            table.AddCell(new Phrase(InvDgv.Rows[i].Cells[j].Value.ToString(), font));
                        }
                    }
                    using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            Document pdfDoc = new Document(PageSize.A4_LANDSCAPE, 0f, 0f, 60f, 40f);
                            // = PdfWriter.GetInstance(pdfDoc, ms);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                            BaseFont baseFont1 = BaseFont.CreateFont("C:\\Windows\\Fonts\\timesbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            BaseFont baseFont2 = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            //iTextSharp.text.Font fontParagraph = new iTextSharp.text.Font(baseFont, 17, iTextSharp.text.Font.BOLD);
                            pdfDoc.Open();
                            //Заголовок 
                            PdfContentByte cb = writer.DirectContent;
                            //cb.Rectangle(10f, 10f, pdfDoc.PageSize.Width - 20f, pdfDoc.PageSize.Height - 20f);
                            cb.SetFontAndSize(baseFont1, 16);
                            cb.BeginText();
                            cb.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, "Приходная накладная № " + NumberOfPrihNakl.Text, 225f, pdfDoc.PageSize.Height - 30f, 0);
                            cb.EndText();
                            cb.Stroke();
                            PdfContentByte cb1 = writer.DirectContent;
                            cb1.SetFontAndSize(baseFont1, 14);
                            cb1.BeginText();
                            cb1.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, "От " + InvoiceDate.Text, 250f, pdfDoc.PageSize.Height - 50f, 0);
                            cb1.EndText();
                            cb1.Stroke();
                            table.SetTotalWidth(new float[] { 400, 200, 200, 200 });
                            PdfContentByte cb2 = writer.DirectContent;
                            cb2.SetFontAndSize(baseFont2, 10);
                            cb2.BeginText();
                            cb2.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, "Оформил(а) (ФИО, роспись): " + EmpFIOTB.Text + " _________", 273f, pdfDoc.Bottom, 0);
                            cb2.EndText();
                            cb2.Stroke();
                            PdfContentByte cb3 = writer.DirectContent;
                            cb3.SetFontAndSize(baseFont1, 14);
                            cb3.BeginText();
                            cb3.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, "Итого: ", 60f, pdfDoc.PageSize.Height - 100f - CalculatePdfPTableHeight(table), 0);
                            cb3.EndText();
                            cb3.Stroke();
                            PdfContentByte cb4 = writer.DirectContent;
                            cb4.SetFontAndSize(baseFont2, 14);
                            cb4.BeginText();
                            cb4.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, MainSumTB.Text, 490f, pdfDoc.PageSize.Height - 100f - CalculatePdfPTableHeight(table), 0);
                            cb4.EndText();
                            cb4.Stroke();
                            pdfDoc.Add(table);
                            pdfDoc.Close();
                            stream.Close();
                            MessageBox.Show("Экспорт завершен!", "Формирование отчета", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void SumMainButton_Click(object sender, EventArgs e)
        {
            try
            {
                double sum = 0;
                for (int i = 0; i < InvDgv.Rows.Count; ++i)
                {
                    sum += Convert.ToDouble(InvDgv.Rows[i].Cells[3].Value);
                }
                MainSumTB.Text = Convert.ToString(sum);
            }
            catch
            {

            }
        }

        private void EntryNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataBaseClass db = new DataBaseClass();
                DataTable table = new DataTable();
                DataTable cl = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT [Дата записи] FROM Запись WHERE [Код записи] = " + Convert.ToInt32(EntryNum.Text), db.GetConnection());
                sda.Fill(table);
                if (table.Rows.Count == 1)
                {
                    EntryDate.Text = Convert.ToString(table.Rows[0].Field<DateTime>(0));
                }
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT Фамилия, Имя, Отчество FROM Клиент, Запись WHERE Запись.[Код записи] = " + Convert.ToInt32(EntryNum.Text) + " AND Запись.[Код клиента] = Клиент.[Код клиента]", db.GetConnection());
                sda1.Fill(cl);
                if (cl.Rows.Count == 1)
                {
                    ClFIO.Text = Convert.ToString(cl.Rows[0].Field<string>(0)) + " " + Convert.ToString(cl.Rows[0].Field<string>(1)) + " " + Convert.ToString(cl.Rows[0].Field<string>(2));
                }
                string sim = "SELECT [Название услуги], Цена AS Стоимость FROM Услуга, Запись, [Оказанная услуга] WHERE Запись.[Код записи] = " + Convert.ToInt32(EntryNum.Text) + " AND Запись.[Код записи] = [Оказанная услуга].Запись AND Услуга.[Код услуги] = [Оказанная услуга].Услуга";
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sim, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    CheckDGV.DataSource = ds.Tables[0];
                }
            }
            catch
            {

            }
        }

        private void IsSolar_CheckedChanged(object sender, EventArgs e)
        {
            if (IsSolar.CheckState == CheckState.Checked)
            {
                SolTimeTB.ReadOnly = false;
                FillButton.Enabled = true;
            }
            else
            {
                SolTimeTB.ReadOnly = true;
                FillButton.Enabled = false;
            }
        }

        private void SolTimeTB_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double s = Convert.ToDouble(SolTimeTB.Text) * 10;
                SolSum.Text = Convert.ToString(s);
            }
            catch
            {

            }
        }

        private void MastIDCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataBaseClass db = new DataBaseClass();
                DataTable table = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT Фамилия, Имя, Отчество FROM Сотрудник WHERE [Код сотрудника] = " + Convert.ToInt32(MastIDCB.Text), db.GetConnection());
                sda.Fill(table);
                if (table.Rows.Count == 1)
                {
                    MastFIOTB.Text = Convert.ToString(table.Rows[0].Field<string>(0)) + " " + Convert.ToString(table.Rows[0].Field<string>(1)) + " " + Convert.ToString(table.Rows[0].Field<string>(2));
                }
            }
            catch
            {

            }
        }

        private void MakeCSButton_Click(object sender, EventArgs e)
        {
            try
            {
                double sum = 0;
                for (int i = 0; i < CheckDGV.Rows.Count; ++i)
                {
                    sum += Convert.ToDouble(CheckDGV.Rows[i].Cells[1].Value);
                }
                CheckSumTB.Text = Convert.ToString(sum);
            }
            catch
            {

            }
        }

        private void FillButton_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < CheckDGV.Rows.Count; ++i)
                {
                    if (Convert.ToString(CheckDGV.Rows[i].Cells[0].Value) == "Солярий")
                    {
                        CheckDGV.Rows[i].Cells[1].Value = SolSum.Text;
                    }
                }
            }
            catch
            {

            }
        }

        private void MakeCheckButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF (*.pdf)|*.pdf";
            save.FileName = ReportControl.TabPages[4].Text + " № " + EntryNum.Text;
            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string FONT_LOCATION = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
                    BaseFont baseFont = BaseFont.CreateFont(FONT_LOCATION, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    //iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont);
                    iTextSharp.text.Font header = new iTextSharp.text.Font(baseFont, 14f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    PdfPTable table = new PdfPTable(CheckDGV.ColumnCount);
                    for (int f = 0; f < CheckDGV.ColumnCount; f++)
                    {
                        PdfPCell CellTwoHdr = new PdfPCell(new Phrase(CheckDGV.Columns[f].HeaderText, header));
                        CellTwoHdr.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(CellTwoHdr);
                    }
                    for (int i = 0; i <= CheckDGV.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < CheckDGV.Columns.Count; j++)
                        {
                            table.AddCell(new Phrase(CheckDGV.Rows[i].Cells[j].Value.ToString(), font));
                        }
                    }
                    using (FileStream stream = new FileStream(save.FileName, FileMode.Create))
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            Document pdfDoc = new Document(PageSize.A4, 0f, 0f, 60f, 40f);
                            // = PdfWriter.GetInstance(pdfDoc, ms);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                            BaseFont baseFont1 = BaseFont.CreateFont("C:\\Windows\\Fonts\\timesbd.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            BaseFont baseFont2 = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            //iTextSharp.text.Font fontParagraph = new iTextSharp.text.Font(baseFont, 17, iTextSharp.text.Font.BOLD);
                            pdfDoc.Open();
                            //Заголовок 
                            PdfContentByte cb = writer.DirectContent;
                            //cb.Rectangle(10f, 10f, pdfDoc.PageSize.Width - 20f, pdfDoc.PageSize.Height - 20f);
                            cb.SetFontAndSize(baseFont1, 16);
                            cb.BeginText();
                            cb.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, "Чек № " + EntryNum.Text + " на запись от " + EntryDate.Text, 180f, pdfDoc.PageSize.Height - 30f, 0);
                            cb.EndText();
                            cb.Stroke();
                            PdfContentByte cb1 = writer.DirectContent;
                            cb1.SetFontAndSize(baseFont1, 14);
                            cb1.BeginText();
                            cb1.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, "Выдан клиенту " + ClFIO.Text, 190f, pdfDoc.PageSize.Height - 50f, 0);
                            cb1.EndText();
                            cb1.Stroke();
                            table.SetTotalWidth(new float[] { 400, 200 });
                            PdfContentByte cb2 = writer.DirectContent;
                            cb2.SetFontAndSize(baseFont2, 10);
                            cb2.BeginText();
                            cb2.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, "Оформил(а): " + MastFIOTB.Text, 350f, pdfDoc.Bottom, 0);
                            cb2.EndText();
                            cb2.Stroke();
                            PdfContentByte cb3 = writer.DirectContent;
                            cb3.SetFontAndSize(baseFont1, 14);
                            cb3.BeginText();
                            cb3.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, "Итого: ", 60f, pdfDoc.PageSize.Height - 80f - CalculatePdfPTableHeight(table), 0);
                            cb3.EndText();
                            cb3.Stroke();
                            PdfContentByte cb4 = writer.DirectContent;
                            cb4.SetFontAndSize(baseFont2, 14);
                            cb4.BeginText();
                            cb4.ShowTextAligned(
                            PdfContentByte.ALIGN_LEFT, CheckSumTB.Text, 490f, pdfDoc.PageSize.Height - 80f - CalculatePdfPTableHeight(table), 0);
                            cb4.EndText();
                            cb4.Stroke();
                            pdfDoc.Add(table);
                            pdfDoc.Close();
                            stream.Close();
                            MessageBox.Show("Экспорт завершен!", "Формирование отчета", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch
                {

                }
            }
        }
    }
}
