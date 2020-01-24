using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace forDBcontrol
{
    public partial class Form2 : Form
    {

        MySqlConnection conn;
        MySqlDataAdapter dataAdapter;
        public Form2()
        {
            InitializeComponent();
        }


        
        private void getTable()
        {
            comboBox1.Items.Clear();
            try
            {
                conn = new MySqlConnection("server=localhost;port=3306;uid=root;pwd=root");
                conn.Open();
                //string strqry = "select table_name from information_schema.tables where TABLE_SCHEMA='GSM'";
                string strqry = "show databases";

                MySqlCommand cmd = new MySqlCommand(strqry, conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader R = cmd.ExecuteReader();

                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        comboBox1.Items.Add(R.GetString(0));
                        // comboBox1.Items.Add("@" + R.GetString(0));

                    }
                }
                else
                {
                    MessageBox.Show("테이블이 하나도 없습니다");
                }
                R.Dispose();
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string db;
        

        private void Form2_Load(object sender, EventArgs e)
        {
            Form1 mainForm = Owner as Form1; // unboxing
            //Form1 mainForm = (Form1)Owner; // 같음
            
            getTable();
        }




        private void Button1_Click_1(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                db = textBox2.Text;


                Close();//form2 닫기
            }
            else
            {
                MessageBox.Show("텍스트를 입력하세요");
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox1.Text;
        }


        

    }
    }

