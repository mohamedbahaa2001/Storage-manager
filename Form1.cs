using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Store_Manager
{
    public partial class Form1 : Form
    {
        //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="C:\Users\Mohamed Bahaa\source\repos\Store Manager\Database1.mdf";Integrated Security=True
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\source\repos\Storage Manage\Database1.mdf;Integrated Security=True");
        SqlDataAdapter DataAdapter;
        DataTable DataTable;

        //show data
        #region show data
        public void ShowData()
        {
            DataAdapter = new SqlDataAdapter("select * from ProductsDB", con);

            DataTable = new DataTable();

            DataAdapter.Fill(DataTable);
            dataGridView1.DataSource = DataTable;
        }
        #endregion

        //show data in datagrid view 2 in acciuntant tab
        #region show data in accuntant tab
        public void ShowDataInDatagridView2()
        {
            DataAdapter = new SqlDataAdapter("select * from EndDayDB", con);

            DataTable = new DataTable();

            DataAdapter.Fill(DataTable);
            dataGridView2.DataSource = DataTable;
        }
        #endregion
        public Form1()
        {
            InitializeComponent();
            ShowData();
            ShowDataInDatagridView2();
            totalSumtxt.Enabled = false;
            totalSumtxt.Text = (0).ToString();
            totalMasroftxt.Enabled = false;
            totalMasroftxt.Text = (0).ToString();
            totalPriceTxt.Enabled = false;
            totalPriceTxt.Text = (0).ToString();
            datetxt.Enabled = false;
            instoragetxt.Enabled = false;
            soldtxt.Enabled = false;
            IDTxt.Enabled = false;
            sumTxt.Enabled = false;
            masrofText.Text = (0).ToString();
            soldtxt.Text = (0).ToString();
            sumTxt.Text = (0).ToString();
            instoragetxt.Text = (0).ToString();
    
            #region Auto Complete Text
            SqlCommand cmd = new SqlCommand("select المنتج from ProductsDB", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            AutoCompleteStringCollection mycollection = new AutoCompleteStringCollection();
            while (dr.Read())
            {
                mycollection.Add(dr.GetString(0));
            }
            ProductTxt.AutoCompleteCustomSource = mycollection;
            con.Close();

            #endregion
            ShowData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //safety for sumtext (magmo3)
            if (sumTxt.Text != 0.ToString() && sumTxt.Text != null && sumTxt.Text != string.Empty)
            {
                totalSumtxt.Text = (Convert.ToInt32(totalSumtxt.Text) + Convert.ToInt32(sumTxt.Text)).ToString();
            }
            //safety for pricetext
            if (pricetxt.Text != 0.ToString() && pricetxt.Text != null && pricetxt.Text != string.Empty)
            {
                totalPriceTxt.Text = (Convert.ToInt32(totalPriceTxt.Text) + Convert.ToInt32(pricetxt.Text)).ToString();
            }
            //safety for masrof text
            if (masrofText.Text != 0.ToString() && masrofText.Text != null && masrofText.Text != string.Empty)
            {
                totalMasroftxt.Text = (Convert.ToInt32(totalMasroftxt.Text) + Convert.ToInt32(masrofText.Text)).ToString();
            }

            #region Auto Complete Text
            SqlCommand cmd = new SqlCommand("select المنتج from ProductsDB", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            AutoCompleteStringCollection mycollection = new AutoCompleteStringCollection();
            while (dr.Read())
            {
                mycollection.Add(dr.GetString(0));
            }
            ProductTxt.AutoCompleteCustomSource = mycollection;
            con.Close();

            #endregion
            ShowData();
        }
        //safty check before closing
        #region check before closing
        #endregion

        //insert to database
        #region open the insert into database section
        private void InsertIntoStorage_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }
        #endregion

        //زرار احضار البيانات
        #region get data from database
        private void getData_Click(object sender, EventArgs e)
        {
            ShowData();
            try
            {
                //بحث علي المنتج بالاسم فقط
                SqlCommand cmd;
                SqlDataReader dr;
                con.Open();
                cmd = new SqlCommand("select * from ProductsDB where المنتج = N'" + ProductTxt.Text + "'", con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    IDTxt.Text = dr.GetValue(0).ToString();
                    instoragetxt.Text = dr.GetValue(2).ToString();
                    pricetxt.Text = dr.GetValue(4).ToString();
                    datetxt.Text = dr.GetValue(3).ToString();
                    soldtxt.Text = dr.GetValue(5).ToString();
                }
                else
                {
                    MessageBox.Show("حدث مشكلة في البحث");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();

            }
        }
        #endregion

        //زرار احسب
        #region calculate button
        private void calculate_Click(object sender, EventArgs e)
        {
            ShowData();
            if (willSelltxt.Text == null || willSelltxt.Text == 0.ToString() || willSelltxt.Text == string.Empty)
            {
                MessageBox.Show("عذرا تأكد من كمية المنتج الذي تريد بيعه");
            }
            else
            {
                sumTxt.Text = (((int.Parse(willSelltxt.Text)) * (int.Parse(pricetxt.Text))) - int.Parse(masrofText.Text)).ToString();
                instoragetxt.Text = (int.Parse(instoragetxt.Text) - int.Parse(willSelltxt.Text)).ToString();
                soldtxt.Text = (int.Parse(soldtxt.Text) + int.Parse(willSelltxt.Text)).ToString();
            }
        }
        #endregion
        //زرار بيع
        #region sell button
        private void sell_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                //تحويل الكليمات من عربي لانجليزي نجح بالطريقة دي اقدر احط براحتي اللي انا عايزو بال أي دي
                command.CommandText = "update ProductsDB set [في المخزن] = '" + instoragetxt.Text + "'  where Id= '" + IDTxt.Text + "' ";
                // command.CommandText = "update ProductsDB set [السعر] = '" + (int.Parse(PriceTxt.Text) + int.Parse(newPriceText.Text)).ToString() + "'  where Id= '" + IDText.Text + "' ";
                command.ExecuteNonQuery();
                con.Close();
                ShowData();

                
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "update ProductsDB set [تم بيعه] = '" + soldtxt.Text + "'  where Id= '" + IDTxt.Text + "' ";
                command.ExecuteNonQuery();
                con.Close();
                ShowData();
                MessageBox.Show("تم حفظ البيعة بنجاح في المخزن");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                con.Close();//safety close
            }
        }
        #endregion

        //زرار الجرد
        #region clasify button export to excel button
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;
                worksheet.Name = "output";
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                var saveFileDialoge = new SaveFileDialog();
                saveFileDialoge.FileName = "SavedFile";
                //saveFileDialoge.DefaultExt = ".csv";
                if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialoge.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                app.Quit();
                MessageBox.Show("تم استخراج ملف الجرد");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        //زرار التنشيط
        #region refresh data
        private void refresh_Click(object sender, EventArgs e)
        {
            ShowData();
            ShowDataInDatagridView2();

        }
        #endregion
        //زرار انهاء اليوم
        #region end day buttom
        private void EndDay_Click(object sender, EventArgs e)
        {
            if (totalMasroftxt.Text != null && totalMasroftxt.Text != string.Empty && totalMasroftxt.Text != 0.ToString()
               && totalPriceTxt.Text != null && totalPriceTxt.Text != string.Empty && totalPriceTxt.Text != 0.ToString()
               && totalSumtxt.Text != null && totalSumtxt.Text != string.Empty && totalSumtxt.Text != 0.ToString())
            {
                try
                {

                    con.Open();
                    SqlCommand command = con.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "insert into EndDayDB values('" + int.Parse(totalPriceTxt.Text) + "','" + int.Parse(totalMasroftxt.Text) + "','" + int.Parse(totalSumtxt.Text) + "','" + dateTimePicker1.Value.ToString() + "')";
                    command.ExecuteNonQuery();
                    con.Close();
                    ShowData();
                    ShowDataInDatagridView2();
                    MessageBox.Show("تم ادخال بيانات اليوم");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    con.Close(); // for safety reasons for the program
                }
            }
            else
            {
                MessageBox.Show("من فضلك ادخل  قم بعملية البيع اولا");
            }
        }
        #endregion

        //second tab in program
        #region search button at end of day in acountant tab
        private void search_Click(object sender, EventArgs e)
        {

            DataAdapter = new SqlDataAdapter("select * from EndDayDB where التاريخ = '" + dateTimePicker2.Value.ToString() + "'", con);

            DataTable = new DataTable();

            DataAdapter.Fill(DataTable);
            dataGridView2.DataSource = DataTable;
        }
        #endregion

        #region get back all data in accountant tab
        private void button1_Click(object sender, EventArgs e)
        {
            ShowDataInDatagridView2();

        }
        #endregion

        #region export to excel file end of the day
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;
                worksheet.Name = "output";

                for (int i = 1; i < dataGridView2.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = dataGridView2.Columns[i - 1].HeaderText;
                }
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                    }
                }
                var saveFileDialoge = new SaveFileDialog();
                saveFileDialoge.FileName = "الجرد";
                //saveFileDialoge.DefaultExt = ".csv";
                if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialoge.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                app.Quit();
                MessageBox.Show("تم استخراج ملف الجرد");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
