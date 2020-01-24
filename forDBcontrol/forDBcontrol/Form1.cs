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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        MySqlConnection conn;
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;
        string[] eggArr = new string[10000];
        string Table = "";
        string sel = "";
        string data = "";
        
        private void data_dont_exist(string select, string a)
        {
            dataSet.Clear();
            try
            {
                dataAdapter = new MySqlDataAdapter(select, conn);
                dataAdapter.Fill(dataSet, a);
                UserGridView.DataSource = dataSet.Tables[a];
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 Dig = new Form2();
            //form2 에서 form1을 참소할 수 있는 참조 저장

            Dig.Owner = this; // this = form1
            Dig.ShowDialog();//form2 띄우기
            data = Dig.db;
            var dataBase = "server=localhost;port=3306;database="+ data +";uid=root;pwd=root";
            
            
            conn = new MySqlConnection(dataBase);

            dataSet = new DataSet();

  /*          
            try
            {
                dataAdapter = new MySqlDataAdapter(sel, conn);
                dataAdapter.Fill(dataSet, Table);
                UserGridView.DataSource = dataSet.Tables[Table];
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message);
            }
*/
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    connection.Text = "DB: " + Dig.db;
                    connection.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
//                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            UserGridView.Visible = true;
            getTable();
           
            
            
        }

        private void getTablecomboBox()
        {
            
            
            Array.Resize(ref eggArr, 10000);
            try
            {
                conn.Open();
                //string strqry = "select table_name from information_schema.tables where TABLE_SCHEMA='GSM'";
                string strqry = "show columns from "+comboBox2.Text;
                MySqlCommand cmd = new MySqlCommand(strqry, conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader R = cmd.ExecuteReader();

                if (R.HasRows)
                {   
                    int i = 0;
                    while (R.Read())
                    {
                        eggArr[i] = R.GetString(0);
                        //comboBox1.Items.Add(eggArr[i]);
                        i++;
                        // comboBox1.Items.Add("@" + R.GetString(0));

                    }
                    Array.Resize(ref eggArr, i);
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
    private void getTable()
        {
            comboBox2.Items.Clear();
            try
            {
                conn.Open();
                //string strqry = "select table_name from information_schema.tables where TABLE_SCHEMA='GSM'";
                string strqry = "show tables";

                MySqlCommand cmd = new MySqlCommand(strqry, conn);
                cmd.CommandType = CommandType.Text;
                MySqlDataReader R = cmd.ExecuteReader();

                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        comboBox2.Items.Add(R.GetString(0));
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



        private void Button4_Click(object sender, EventArgs e)
        {
            Form2 Dig = new Form2();
            //form2 에서 form1을 참소할 수 있는 참조 저장

            Dig.Owner = this; // this = form1
            Dig.ShowDialog();//form2 띄우기
            this.data = Dig.db;
            
                conn = new MySqlConnection("server=localhost;port=3306;database=" + data + ";uid=root;pwd=root");
                getTable();
                MessageBox.Show("DB: " + Dig.db);
                connection.Text = "DB: " + Dig.db;
            
            
            
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            Table = comboBox2.Text;
            sel = "select * from " + comboBox2.Text;
            MessageBox.Show(comboBox2.Text + " Table 선택됨");
            data_dont_exist(sel, Table);
            getTablecomboBox();
        }

        private void Update_btn_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("업데이트 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            string sq = "";
            for (int i = 0; i < (eggArr.Length) - 1; i++)
            {
                sq += eggArr[i] + "=" + "@" + eggArr[i];
                sq += ",";
            }
            sq += eggArr[(eggArr.Length) - 1] + "=" + "@" + eggArr[(eggArr.Length) - 1];


            string sql = "update " + comboBox2.Text + " set " + sq + " where " + eggArr[0] + " = @" + eggArr[0];

            dataAdapter.UpdateCommand = new MySqlCommand(sql, conn);

            try
            {
                for (int i = 0; i <= (eggArr.Length) - 1; i++)
                {
                    dataAdapter.UpdateCommand.Parameters.AddWithValue("@" + eggArr[i], UserGridView.SelectedRows[0].Cells[eggArr[i]].Value.ToString());
                }
                conn.Open();
                if (dataAdapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    //비연결형
                    dataSet.Clear();
                    dataAdapter.Fill(dataSet, Table); // DB -> 메모리(dataset)
                    UserGridView.DataSource = dataSet.Tables[Table];
                    
                    MessageBox.Show("데이터 수정됨");
                }
                else
                {
                    MessageBox.Show("수정된 데이터가 없음");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();

            }
        }

        private void UserGridView_KeyDown_1(object sender, KeyEventArgs e)
        {


        
                if (e.KeyCode == Keys.Delete)
                {

                    string sql = "delete from " + comboBox2.Text + " where " + eggArr[0] + "=@" + eggArr[0];
                    dataAdapter.DeleteCommand = new MySqlCommand(sql, conn);
                    try
                    {
                        string id = (string)UserGridView.SelectedRows[0].Cells[eggArr[0]].Value;
                        dataAdapter.DeleteCommand.Parameters.AddWithValue("@" + eggArr[0], id);
                    }
                    catch
                    {
                        int id = (int)UserGridView.SelectedRows[0].Cells[eggArr[0]].Value;
                        dataAdapter.DeleteCommand.Parameters.AddWithValue("@" + eggArr[0], id);
                    }
                    //var selectedRows = dataGridView1.SelectedRows;

                    try
                    {
                        conn.Open();

                        if (dataAdapter.DeleteCommand.ExecuteNonQuery() > 0)
                        {
                            dataSet.Clear();
                            dataAdapter.Fill(dataSet, Table);
                            UserGridView.DataSource = dataSet.Tables[Table];
                            MessageBox.Show("삭제 성공!");
                        }
                        else
                        {

                            MessageBox.Show("삭제된 데이터가 없습니다");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();

                    }

                }
                if (e.KeyCode == Keys.Enter)
                {
                    string q = "";
                    string qe = "";
                    for (int i = 0; i < (eggArr.Length) - 1; i++)
                    {
                        q += eggArr[i];
                        qe += "@" + eggArr[i];
                        q += ", ";
                        qe += ", ";
                    }
                    q += eggArr[(eggArr.Length) - 1];
                    qe += "@" + eggArr[(eggArr.Length) - 1];


                    DialogResult result = MessageBox.Show("추가 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    string query = "insert into " + comboBox2.Text + "(" + q + ") values(" + qe + ")";


                    dataAdapter.InsertCommand = new MySqlCommand(query, conn);
                    try
                    {
                        for (int i = 0; i <= (eggArr.Length) - 1; i++)
                        {
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@" + eggArr[i], UserGridView.SelectedRows[0].Cells[eggArr[i]].Value.ToString());
                        }
                    }
                    catch
                    {
                        MessageBox.Show("error");
                    }
                    try
                    {
                        if (dataAdapter.Update(dataSet, Table) > 0)
                        {
                            conn.Open();
                            dataSet.Clear();
                            dataAdapter.Fill(dataSet, Table);
                            UserGridView.DataSource = dataSet.Tables[Table];
                            MessageBox.Show("INSERT COMPLETE!");
                        }
                        else
                            MessageBox.Show("검색된 데이터가 없습니다.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult result = MessageBox.Show("업데이트 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    string sq = "";
                    for (int i = 0; i < (eggArr.Length) - 1; i++)
                    {
                        sq += eggArr[i] + "=" + "@" + eggArr[i];
                        sq += ",";
                    }
                    sq += eggArr[(eggArr.Length) - 1] + "=" + "@" + eggArr[(eggArr.Length) - 1];


                    string sql = "update " + comboBox2.Text + " set " + sq + " where " + eggArr[0] + " = @" + eggArr[0];

                    dataAdapter.UpdateCommand = new MySqlCommand(sql, conn);

                    try
                    {
                        for (int i = 0; i <= (eggArr.Length) - 1; i++)
                        {
                            dataAdapter.UpdateCommand.Parameters.AddWithValue("@" + eggArr[i], UserGridView.SelectedRows[0].Cells[eggArr[i]].Value.ToString());
                        }
                        conn.Open();
                        if (dataAdapter.UpdateCommand.ExecuteNonQuery() > 0)
                        {
                            //비연결형
                            dataSet.Clear();
                            dataAdapter.Fill(dataSet, Table); // DB -> 메모리(dataset)
                            UserGridView.DataSource = dataSet.Tables[Table];
                            MessageBox.Show("데이터 수정됨");
                        }
                        else
                        {
                            MessageBox.Show("수정된 데이터가 없음");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();

                    }
                }
            
        }

        private void Insert_btn_Click_1(object sender, EventArgs e)
        {
            


                string q = "";
                string qe = "";
                for (int i = 0; i < (eggArr.Length) - 1; i++)
                {
                    q += eggArr[i];
                    qe += "@" + eggArr[i];
                    q += ", ";
                    qe += ", ";
                }
                q += eggArr[(eggArr.Length) - 1];
                qe += "@" + eggArr[(eggArr.Length) - 1];


                DialogResult result = MessageBox.Show("추가 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                string query = "insert into " + comboBox2.Text + "(" + q + ") values(" + qe + ")";


                dataAdapter.InsertCommand = new MySqlCommand(query, conn);
                try
                {
                    for (int i = 0; i <= (eggArr.Length) - 1; i++)
                    {
                        dataAdapter.InsertCommand.Parameters.AddWithValue("@" + eggArr[i], UserGridView.SelectedRows[0].Cells[eggArr[i]].Value.ToString());
                    }
                }
                catch
                {
                    MessageBox.Show("error");
                }
                try
                {
                    if (dataAdapter.Update(dataSet, Table) > 0)
                    {
                        conn.Open();
                        dataSet.Clear();
                        dataAdapter.Fill(dataSet, Table);
                        UserGridView.DataSource = dataSet.Tables[Table];
                        MessageBox.Show("INSERT COMPLETE!");
                    }
                    else
                        MessageBox.Show("검색된 데이터가 없습니다.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            
        }

        private void Delete_btn_Click_1(object sender, EventArgs e)
        {
            

                string sql = "delete from " + comboBox2.Text + " where " + eggArr[0] + "=@" + eggArr[0];
                dataAdapter.DeleteCommand = new MySqlCommand(sql, conn);
                try
                {
                    string id = (string)UserGridView.SelectedRows[0].Cells[eggArr[0]].Value;
                    dataAdapter.DeleteCommand.Parameters.AddWithValue("@" + eggArr[0], id);
                }
                catch
                {
                    int id = (int)UserGridView.SelectedRows[0].Cells[eggArr[0]].Value;
                    dataAdapter.DeleteCommand.Parameters.AddWithValue("@" + eggArr[0], id);
                }
                //var selectedRows = dataGridView1.SelectedRows;

                try
                {
                    conn.Open();

                    if (dataAdapter.DeleteCommand.ExecuteNonQuery() > 0)
                    {
                        dataSet.Clear();
                        dataAdapter.Fill(dataSet, Table);
                        UserGridView.DataSource = dataSet.Tables[Table];
                        MessageBox.Show("삭제 성공!");
                    }
                    else
                    {

                        MessageBox.Show("삭제된 데이터가 없습니다");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();

                }
            
        }
    }
}
