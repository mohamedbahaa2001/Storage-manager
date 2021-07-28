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
    public partial class Form2 : Form
    {
        //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="C:\Users\Mohamed Bahaa\source\repos\Store Manager\Database1.mdf";Integrated Security=True
        public Form2()
        {
            InitializeComponent();
            newProductText.Text = 0.ToString();
            newPriceText.Text = (0).ToString();
            soldtext.Text = (0).ToString();
            dateText.Enabled = false;
            IDText.Enabled = false;
            //StoredTxt.Enabled = false;

            ShowData();
        }
        //new connection from D:/ Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="D:\Backups\source\repos\Store Manager\Database1.mdf";Integrated Security=True
        //old connection Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mohamed Bahaa\source\repos\Store Manager\Database1.mdf;Integrated Security=True
        // connection
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\source\repos\Storage Manage\Database1.mdf;Integrated Security=True");
        SqlDataAdapter DataAdapter;
        DataTable DataTable;
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        #region search button
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //بحث علي المنتج بالاسم فقط
                SqlCommand cmd;
                SqlDataReader dr;
                con.Open();
                cmd = new SqlCommand("select * from ProductsDB where المنتج = N'" + ProductNameTxt.Text + "'", con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    IDText.Text = dr.GetValue(0).ToString();
                    StoredTxt.Text = dr.GetValue(2).ToString();
                    PriceTxt.Text = dr.GetValue(4).ToString();
                    dateText.Text = dr.GetValue(3).ToString();
                    soldtext.Text = dr.GetValue(5).ToString();
                }
                else
                {
                    MessageBox.Show("المنتج ليس موجود");
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

        //show data function
        #region show data function
        public void ShowData()
        {
            DataAdapter = new SqlDataAdapter("select * from ProductsDB", con);

            DataTable = new DataTable();

            DataAdapter.Fill(DataTable);
            dataGridView1.DataSource = DataTable;
        }
        #endregion

        #region form2 load function and auto complete
        private void Form2_Load(object sender, EventArgs e)
        {
            // auto compete text
            #region Auto Complete Text
            SqlCommand cmd = new SqlCommand("select المنتج from ProductsDB", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            AutoCompleteStringCollection mycollection = new AutoCompleteStringCollection();
            while (dr.Read())
            {
                mycollection.Add(dr.GetString(0));
            }
            ProductNameTxt.AutoCompleteCustomSource = mycollection;
            con.Close();
            #endregion

        }
        #endregion
        // update button
        #region update data in database
        private void update_Click(object sender, EventArgs e)
        {
            try
            {
                if (newProductText.Text == 0.ToString())
                {
                    MessageBox.Show("ادخل القيم الجديدة للمنتج");
                }
                else
                {
                    con.Open();
                    SqlCommand command = con.CreateCommand();
                    command.CommandType = CommandType.Text;
                    //تحويل الكليمات من عربي لانجليزي نجح بالطريقة دي اقدر احط براحتي اللي انا عايزو بال أي دي
                    command.CommandText = "update ProductsDB set [في المخزن] = '" + (int.Parse(StoredTxt.Text) + int.Parse(newProductText.Text)).ToString() + "'  where Id= '" + IDText.Text + "' ";
                    // command.CommandText = "update ProductsDB set [السعر] = '" + (int.Parse(PriceTxt.Text) + int.Parse(newPriceText.Text)).ToString() + "'  where Id= '" + IDText.Text + "' ";
                    command.ExecuteNonQuery();
                    con.Close();
                    ShowData();

                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "update ProductsDB set [السعر] = '" + (int.Parse(PriceTxt.Text) + int.Parse(newPriceText.Text)).ToString() + "'  where Id= '" + IDText.Text + "' ";
                    command.ExecuteNonQuery();
                    con.Close();
                    ShowData();
                    MessageBox.Show("تم التحديث");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                con.Close();//safety close
            }

        }
        #endregion

        #region add new product to storage
        private void button4_Click(object sender, EventArgs e)
        {
            //add new product to storage

            try
            {
                // check if the product is already excists in the DataBase
                SqlDataAdapter da = new SqlDataAdapter("select المنتج from ProductsDB where المنتج = N'" + ProductNameTxt.Text + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    MessageBox.Show("المنتج موجود بالفعل");
                }
                else
                {
                    if (StoredTxt.Text != string.Empty || PriceTxt.Text != null || PriceTxt.Text != string.Empty)
                    {
                        if (StoredTxt.Text == 0.ToString() || PriceTxt.Text == 0.ToString())
                        {
                            MessageBox.Show("من فضلك ادخل  الكمية وسعر المنتج المراد حفظه");
                        }
                        else
                        {
                            try
                            {

                                con.Open();
                                SqlCommand command = con.CreateCommand();
                                command.CommandType = CommandType.Text;
                                command.CommandText = "insert into ProductsDB values(N'" + ProductNameTxt.Text + "','" + StoredTxt.Text + "','" + dateTimePicker1.Value.ToString() + "','" + PriceTxt.Text + "','" + soldtext.Text + "')";
                                command.ExecuteNonQuery();
                                con.Close();
                                ShowData();
                                MessageBox.Show("تم ادخال بيانات المنتج بنجاح");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                con.Close(); // for safety reasons for the program
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("تأكد من ادخال السعر او الكمية المراد تخزينها");
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                con.Close(); // for safety reasons for the program

            }


        }
        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.ShowDialog();
            this.Close();
        }
        //safty before closing
       #region safty before closing
        #endregion 
    }
}


