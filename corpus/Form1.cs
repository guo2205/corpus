using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace corpus
{
    public partial class Form1 : Form
    {
        List<corpusList> corpuslist1 = new List<corpusList>();
        List<corpusList> corpuslist2 = new List<corpusList>();
        List<corpusList> corpuslist3 = new List<corpusList>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            select_corpus.SelectedIndex = 0;
            //corpuslist.Add(new corpusList { corpus = "1", F = "2", Q = "3" });
            dataGridView1.RowHeadersDefaultCellStyle.Padding = new Padding(dataGridView1.RowHeadersWidth);
            //dataGridView1.DataSource = corpuslist;
            //dataGridView2.DataSource = corpuslist2;
            refush();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = select_corpus.SelectedIndex;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                return;
            }
            switch (index)
            {
                case 0:
                    string q = TuLingTool.postdata(textBox1.Text, "0");
                    corpusList corpuslist = new corpusList();
                    corpuslist.corpus = "图灵";
                    corpuslist.F = textBox1.Text;
                    corpuslist.Q = q;
                    this.corpuslist1.Add(corpuslist);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = this.corpuslist1;
                    textBox2.Text = q;
                    break;
                case 1:
                    MySql mysql = new MySql();
                    List<List<string>> list = mysql.GetAllSqlData("select `q` from zhijiayun_corpus where `f`='"+textBox1.Text+"'");
                    if (list.Count > 0)
                    {
                        textBox2.Text = list[0][0];
                    }
                    break;
                default:
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("不能为空");
            }
            else
            {
                MySql mysql = new MySql();
                mysql.AddSqlData("insert into `zhijiayun_corpus`(`f`,`q`) values('" + textBox4.Text + "','" + textBox3.Text + "')");
                MessageBox.Show("添加成功");
                mysql.Dispose();

                corpuslist2.Add(new corpusList { corpus = "Bot", F = textBox4.Text, Q = textBox3.Text });
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = this.corpuslist2;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox4.Text = "";
        }
        public void refush()
        {
            corpuslist3.Clear();
            MySql mysql = new MySql();
            List<List<string>> corpusTable = mysql.GetAllSqlData("select `f`,`q` from zhijiayun_corpus");
            foreach (var item in corpusTable)
            {
                corpusList list = new corpusList();
                list.F = item[0];
                list.Q = item[1];
                this.corpuslist3.Add(list);
            }
            mysql.Dispose();
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = this.corpuslist3;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            refush();
        }
    }

    class corpusList
    {
        public string corpus { get; set; }
        public string F { get; set; }
        public string Q { get; set; }
    }

    public class TuLingTool
    {
        public static string postdata(string str, string userid)
        {
            string url = "http://www.tuling123.com/openapi/api";
            string key = "2585370322d24d759f5dfe9c7dfb3ede";
            MyJson.JsonNode_Object json = new MyJson.JsonNode_Object();
            json.SetDictValue("key", key);
            json.SetDictValue("info", get_uft8(str));
            json.SetDictValue("userid", userid);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json";

            request.ContentType = "application/json";

            using (Stream outStream = request.GetRequestStream())
            {
                StreamWriter sw = new StreamWriter(outStream);
                sw.WriteLine(json);
                sw.Flush();
                sw.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream inStream = response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(inStream);
                MyJson.JsonNode_Object myjson = MyJson.Parse(sr.ReadToEnd()) as MyJson.JsonNode_Object;
                return myjson["text"].ToString();
            }
        }

        public static string get_uft8(string unicodeString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }
    }
}
