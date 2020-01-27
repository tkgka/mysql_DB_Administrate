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
    public partial class Form3 : Form
    {
        MySqlConnection conn;
        
        public Form3()
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
        public string TEXT;
        public int check = 0;


        private void CREATEDB(string Text)
        {
            {
                try
                {
                    conn = new MySqlConnection("server=localhost;port=3306;uid=root;pwd=root");
                    conn.Open();
                    //string strqry = "select table_name from information_schema.tables where TABLE_SCHEMA='GSM'";
                    string strqry = "CREATE DATABASE "+Text;

                    MySqlCommand cmd = new MySqlCommand(strqry, conn);
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader R = cmd.ExecuteReader();

                    TEXT = Text;
                    R.Dispose();
                    conn.Close();
                    MessageBox.Show("DB 생성됨");
                    check = 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    if(ex.Message == "Can't create database '"+Text+"'; database exists")
                    {
                        try
                        {
                            conn = new MySqlConnection("server=localhost;port=3306;uid=root;pwd=root");
                            conn.Open();
                            //string strqry = "select table_name from information_schema.tables where TABLE_SCHEMA='GSM'";
                            string strqry = "DROP DATABASE " + Text;

                            MySqlCommand cmd = new MySqlCommand(strqry, conn);
                            cmd.CommandType = CommandType.Text;
                            MySqlDataReader R = cmd.ExecuteReader();

                            TEXT = Text;
                            R.Dispose();
                            conn.Close();
                            DialogResult result = MessageBox.Show("제거 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            
                                MessageBox.Show("DB 제거됨");
                            
                            check = 1;
                        }
                        catch (Exception E)
                        {
                            MessageBox.Show(E.Message);
                        }


                    }


                }
            }
        }




        private void Form3_Load(object sender, EventArgs e)
        {
            getTable();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CREATEDB(textBox1.Text);
            Close();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.Text;
        }
    }
}
