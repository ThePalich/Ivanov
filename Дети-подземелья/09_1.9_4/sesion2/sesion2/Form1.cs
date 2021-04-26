using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sesion2
{
    public partial class Form1 : Form
    {
        string conStr = @"data source=vc-stud-mssql1;user id=user79_db;password=user79;MultipleActiveResultSets=True;App=EntityFramework";
        SqlConnection connection;
        public int n = 0;
        public int id = 0;
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("По убыванию");
            comboBox1.Items.Add("По Возрастанию");
            comboBox1.Items.Add("Без сортировки");
            comboBox2.Items.Add("");



            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;




        }
        public void load_data()
        {
            int chislo = 0;
            flowLayoutPanel1.Controls.Clear();
            connection = new SqlConnection(conStr);
            connection.Open();


            string q_1 = "SELECT Name FROM Manufacturer";

            SqlCommand _cmd2 = new SqlCommand(q_1);
            _cmd2.Connection = connection;
            SqlDataReader _rd1 = _cmd2.ExecuteReader();
            while (_rd1.Read())
            {
                comboBox2.Items.Add(_rd1.GetValue(0));
            }
            _rd1.Close();



            List<string> filters_list = new List<string>();
            string filters = "";
            string sort = "Id";
            
            if (comboBox1.Text == "Без сортировки")
            {
                sort = "Id";
            }
            if (comboBox1.Text == "По убыванию")
            {
                sort = "Cost desc";
            }
            if (comboBox1.Text == "По Возрастанию")
            {
                sort = "Cost";
            }
            
            if (textBox4.Text != "")
            {
                filters_list.Add("Title like N'" + textBox4.Text + "%'");
            }
            if (comboBox2.Text != "")
            {
                chislo = comboBox2.SelectedIndex;
                filters_list.Add(" ManufacturerID='" + chislo + "'");
            }

            if (filters_list.Count > 0)
            {
                filters = "WHERE " + String.Join(" and ", filters_list.ToArray());
            }

            string q_ = "SELECT COUNT(*) FROM Product  " + filters;

            SqlCommand _cmd = new SqlCommand(q_);
            _cmd.Connection = connection;
            SqlDataReader _rd = _cmd.ExecuteReader();

            _rd.Read();
            int c = Int32.Parse(_rd.GetValue(0).ToString());
            if (n <= 0 || c < n)
            {
                label2.Text = " из 1";
                numericUpDown1.Maximum = 1;
                label1.Text = "Показано записей: " + n + " из " + c;
            }
            else
            {
                label2.Text = " из " + (c / n);
                numericUpDown1.Maximum = c / n;
                label1.Text = "Показано записей: " + n* numericUpDown1.Value + " из " + c;
            }


           
           string q = "SELECT ID,Title,Cost,Description,MainImagePath,IsActive,ManufacturerID FROM Product " + filters + " ORDER BY " + sort + " OFFSET " + ((numericUpDown1.Value - 1) * n) + " ROWS FETCH NEXT " + (n <= 0 ? c : n) + " ROWS ONLY";
            SqlCommand cmd = new SqlCommand(q);
            cmd.Connection = connection;
            SqlDataReader rd = cmd.ExecuteReader();
            
            while (rd.Read())
            {

                id = Convert.ToInt32(rd.GetValue(0));

                flowLayoutPanel1.Controls.Add(new UserControl1("D:\\IS1-31\\Дети-подземелья\\09_1.9_4\\Сессия 1\\Товары автосервиса\\" + rd.GetValue(4).ToString().Substring(29), rd.GetValue(1).ToString(), rd.GetValue(2).ToString(), rd.GetValue(5).ToString()));


            }

            rd.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            n = 9;
            load_data();

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            load_data();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            load_data();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_data();
        }

       

        private void flowLayoutPanel1_DoubleClick_1(object sender, EventArgs e)
        {
            

        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
           
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_data();
        }
    }
}
