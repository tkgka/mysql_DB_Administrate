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
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;
        public Form3()
        {
            InitializeComponent();
        }

        private void data_dont_exist(string select, string a)
        {
            conn = new MySqlConnection("server=localhost;port=3306;database=" + comboBox1.Text + ";uid=root;pwd=root");
            dataSet.Clear();
            try
            {
                dataAdapter = new MySqlDataAdapter(select, conn);
                dataAdapter.Fill(dataSet, a);
                UserGridView.DataSource = dataSet.Tables[a];
            }
            catch (Exception E)
            {
                //MessageBox.Show(E.Message);
            }
        }
        public int count = 0;


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
                        count += 1;
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
        private void getPrimary() 
        {
            comboBox2.Items.Clear();
            comboBox2.Text = UserGridView.Rows[0].Cells[0].FormattedValue.ToString();
           for (int i = 0; i < UserGridView.Rows.Count - 1; i++)
            {
                comboBox2.Items.Add(UserGridView.Rows[i].Cells[0].FormattedValue.ToString());
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
                            DialogResult result = MessageBox.Show("제거 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                string strqry = "DROP DATABASE " + Text;
                            
                            MySqlCommand cmd = new MySqlCommand(strqry, conn);
                            cmd.CommandType = CommandType.Text;
                            MySqlDataReader R = cmd.ExecuteReader();

                            TEXT = Text;
                            R.Dispose();
                            conn.Close();
                                MessageBox.Show("DB 제거됨");
                            }
                           
                            else
                            {
                                MessageBox.Show("취소됨");
                            }


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

        private void create_Table(string Text,string contents)
        {
            try
            {
                conn = new MySqlConnection("server=localhost;port=3306;database=" + Text + ";uid=root;pwd=root");
                conn.Open();
                //string strqry = "select table_name from information_schema.tables where TABLE_SCHEMA='GSM'";
                string strqry = "CREATE TABLE `" + textBox2.Text + "` (" + contents + ");";

                MySqlCommand cmd = new MySqlCommand(strqry, conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader R = cmd.ExecuteReader();

                TEXT = Text;
                R.Dispose();
                conn.Close();
                MessageBox.Show("Table 생성됨");
                check = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.Message == "Table '"+textBox2.Text+"' already exists")
                {
                    try
                    {
                        conn = new MySqlConnection("server=localhost;port=3306;database=" + Text + ";uid=root;pwd=root");
                        conn.Open();
                        //string strqry = "select table_name from information_schema.tables where TABLE_SCHEMA='GSM'";

                        DialogResult result = MessageBox.Show("제거 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            string strqry = "DROP TABLE " + textBox2.Text;

                            MySqlCommand cmd = new MySqlCommand(strqry, conn);
                            cmd.CommandType = CommandType.Text;
                            MySqlDataReader R = cmd.ExecuteReader();

                            TEXT = Text;
                            R.Dispose();
                            conn.Close();


                            MessageBox.Show("table 제거됨");
                        }
                        else
                        {
                            MessageBox.Show("취소됨");
                        }
                        check = 1;
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show(E.Message);
                    }

                }


            }

            
        }

            
        private void Form3_Load(object sender, EventArgs e)
        {
            getTable();
            dataSet = new DataSet();

            DataTable table = new DataTable();
            table.Columns.Add("Field", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Nullable", typeof(string));

            UserGridView.DataSource = table;

            DataTable Foreign = new DataTable();
            Foreign.Columns.Add("ForeignKey", typeof(string));
            Foreign.Columns.Add("REFERENCES", typeof(string));
            Foreign.Columns.Add("column", typeof(string));

            ForeignGridView.DataSource = Foreign;


            // column을 추가합니다.

            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if ((DBradio.Checked) || (textBox2.Text != "") && (TableRadio.Checked)) {
                string text = "";
                //text = UserGridView[1 , 0].Value + "";


                for (int i = 0; i < UserGridView.Rows.Count - 1; i++)
                {
                    text += "`" + UserGridView.Rows[i].Cells[0].FormattedValue.ToString() + "` ";
                    text += UserGridView.Rows[i].Cells[1].FormattedValue.ToString() + " ";
                    text += " COLLATE utf8_bin ";
                    if ((UserGridView.Rows[i].Cells[2].FormattedValue.ToString() == "NO") || (UserGridView.Rows[i].Cells[2].FormattedValue.ToString() == "no"))
                    {
                        text += "NOT NULL,";
                    } else
                    {
                        text += ",";
                    }
                }
                text += "PRIMARY KEY (`" + comboBox2.Text + "`)";

                if (ForeignGridView.Rows[0].Cells[0].FormattedValue.ToString() != "") { 
                    for (int i = 0; i < ForeignGridView.Rows.Count - 1; i++){
                           text += ", foreign key (`" + ForeignGridView.Rows[i].Cells[0].FormattedValue.ToString() + "`) references "
                            + ForeignGridView.Rows[i].Cells[1].FormattedValue.ToString() + " (`" + ForeignGridView.Rows[i].Cells[2].FormattedValue.ToString() + "`)";
                        }

                    MessageBox.Show(text);
                }


                if (TableRadio.Checked)
                {
                    create_Table(textBox1.Text, text);
                }
                else if (DBradio.Checked)
                {
                    CREATEDB(textBox1.Text);
                }




                //MessageBox.Show(text);

                DialogResult result = MessageBox.Show("창을 닫으시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Close();
                }
                else
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("Field", typeof(string));
                    table.Columns.Add("Type", typeof(string));
                    table.Columns.Add("Nullable", typeof(string));

                    UserGridView.DataSource = table;

                    DataTable Foreign = new DataTable();
                    Foreign.Columns.Add("ForeignKey", typeof(string));
                    Foreign.Columns.Add("REFERENCES", typeof(string));
                    Foreign.Columns.Add("column", typeof(string));

                    ForeignGridView.DataSource = Foreign;

                }
            }
            else
            {
                MessageBox.Show("모든 조건을 입력하세요");
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.Text;

            string Table = textBox2.Text;
            string sel = "desc " + Table;

            data_dont_exist(sel, Table);

        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Field", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Nullable", typeof(string));
            UserGridView.DataSource = table;

            DataTable Foreign = new DataTable();
            Foreign.Columns.Add("ForeignKey", typeof(string));
            Foreign.Columns.Add("REFERENCES", typeof(string));
            Foreign.Columns.Add("column", typeof(string));

            ForeignGridView.DataSource = Foreign;
        }

        private void UserGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            if(UserGridView.Rows.Count > 0)
            {
                getPrimary();
            }

            if ((UserGridView.Rows[e.RowIndex].Cells[2].FormattedValue.ToString() != "NO") && (UserGridView.Rows[e.RowIndex].Cells[2].FormattedValue.ToString() != "no"))
            {
                UserGridView.Rows[e.RowIndex].Cells[2].Value = "yes";
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

            
            
            if ((UserGridView.Rows[0].Cells[0].FormattedValue.ToString() == "") || (UserGridView.Columns.Count > 5))
            {
                
                
                DataTable table = new DataTable();
                table.Columns.Add("Field", typeof(string));
                table.Columns.Add("Type", typeof(string));
                table.Columns.Add("Nullable", typeof(string));
                UserGridView.DataSource = table;


                string Table = textBox2.Text;
                string sel = "desc " + Table;

                data_dont_exist(sel, Table);
            }
        }
    }
}
