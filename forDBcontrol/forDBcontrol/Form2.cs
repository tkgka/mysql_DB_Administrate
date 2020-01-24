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
        public Form2()
        {
            InitializeComponent();
        }

        public string db;
        

        private void Form2_Load(object sender, EventArgs e)
        {
            Form1 mainForm = Owner as Form1; // unboxing
            //Form1 mainForm = (Form1)Owner; // 같음
        

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
    }
    }

