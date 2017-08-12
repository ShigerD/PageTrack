using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace WebTrack
{
    public partial class PageTrack : Form
    {
        /****/
        string respHtml=null;
        /**step1**/
        public const int Step1DataLength = 100;
        string[] Step1SCstring = new string[Step1DataLength];
        string[] Step1X = new string[Step1DataLength];
        double[] Step1getJME = new double[Step1DataLength];
        public int Step1getNum = 0;
        string filePath = System.Environment.CurrentDirectory + "\\data";

        


        public PageTrack()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "http://www.91job.gov.cn/jobfair91/view/id/26962";
            //step1: get html from url
            textBox2.Text = "utf-8";
        }
        void track()
        {
            string urlToCrawl = textBox1.Text;
            string htmlCharset =textBox2.Text;
            //generate http request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(urlToCrawl);
            //use GET method to get url's html
            req.Method = "GET";
            //use request to get response
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
           
            //use songtaste's html's charset GB2312 to decode html
            //otherwise will return messy code
            Encoding htmlEncoding = Encoding.GetEncoding(htmlCharset);
            StreamReader sr = new StreamReader(resp.GetResponseStream(), htmlEncoding);
            //read out the returned html
            respHtml = sr.ReadToEnd();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            btnTrack.Enabled = false;
            bgw_track.RunWorkerAsync();

        }


        private void bgw_track_DoWork(object sender, DoWorkEventArgs e)
        {
            track();
            //  Step1AllOpreations();
            string moutfile = @"./outHtml/";
            if (!Directory.Exists(moutfile))
            {
                Directory.CreateDirectory(moutfile);
            }

            string mfilepath = @"./outHtml/track.html";
            if (File.Exists(mfilepath))
            {
                File.Delete(mfilepath);
            }
            File.AppendAllText(mfilepath, respHtml);
        }
        private void bgw_track_complete(object sender, RunWorkerCompletedEventArgs e)
        {
            richTextBox1.Text = respHtml;
            tb_outpath.Text=  System.Environment.CurrentDirectory + "/outHtml/track.html";
            btnTrack.Enabled = true;
        }

        public void Step1AllOpreations()
        {
            Step1getScode();

            Step1scode2X(Step1getNum);

            Step1getJMEway();

            Step1SaveToFile();

            for (int k = 0; k< Step1getNum; k++)
            {
                //Step1SCstring[j] = ExtractNum(foundSCode[j].Value);
               //  listBox1.Items.Add(Step1X[k]);
               //  listBox2.Items.Add(Step1X[k] + "|" + Step1SCstring[k] + "|" + DateTime.Now.Year + "|" + Step1getJME[k]);
               //  listBox3.Items.Add(Step1X[k] + "|" + Step1SCstring[k] + "|" + Step1getWebDate() + "|" + Step1getJME[k]);
            }
        }
        public void Step1SaveToFile()
        {
            string filePath1;
            string filePath2;
            string filePath3;
            string step1FlagFile;
            string test = Step1getWebDate();
            /** **/
            textBox1.Text = filePath;
             filePath = textBox1.Text;
             if(filePath=="")
              {
             MessageBox.Show("请输入文件路径！");
                }
             else
             {
             if(!Directory.Exists(filePath))
             Directory.CreateDirectory(filePath);
             filePath1 = filePath + "\\" +"10.txt";
                // File.Delete(filePath1);
                step1FlagFile = filePath + "\\" + "step1FlagFile.txt";
                File.AppendAllText(step1FlagFile, " ");
                StreamReader SR = new StreamReader(step1FlagFile, Encoding.Default);
                string datelast = SR.ReadToEnd().Trim();
                SR.Close();
                /****/
                if (datelast == test)
                {
                    MessageBox.Show(Step1getWebDate() + "日step1数据已经更新过");
                    return;
                }

                for (int k = 0; k < Step1getNum; k++)
             {
                 try
                 {
                     File.AppendAllText(filePath1, Step1X[k] + "|" + Step1SCstring[k] + "|" + Step1getWebDate() + "|" + Step1getJME[k] + "\r\n");
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
             }
             //MessageBox.Show(filePath1+"文件追加成功");
             filePath2 = filePath + "\\"   + "2.txt";
             File.Delete(filePath2);
             for (int k = 0; k < Step1getNum; k++)
             {      
                 try
                 {
                     File.AppendAllText(filePath2, Step1X[k] + "|" + Step1SCstring[k] + "|" + Step1getWebDate() + "|" + Step1getJME[k] + "\r\n");
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
             }
            // MessageBox.Show(filePath2 + "文件操作成功");
             filePath3 = filePath + "\\"+"龙虎.EBK";
             File.Delete(filePath3);
             for (int k = 0; k < Step1getNum; k++)
             {
              try
                 {
                 File.AppendAllText(filePath3, Step1SCstring[k] + "\r\n");
                 }
              catch(Exception ex)
                  {
                     MessageBox.Show(ex.Message);
                  }
             }
                File.Delete(step1FlagFile);
                File.AppendAllText(step1FlagFile, test);
                MessageBox.Show(Step1getWebDate() + "Step1文件操作成功");         
        }     
     }
        /****/
        public void Step1getScode()
        {
            /**/
            string SCode = @"""SCode"":""(?<SCode>.+?)""";
            MatchCollection foundSCode = (new Regex(SCode)).Matches(respHtml);
            Step1getNum = foundSCode.Count;
  
            for (int j = 0; j < Step1getNum; j++)
            {
                Step1SCstring[j] = ExtractNum(foundSCode[j].Value);
         //       listBox1.Items.Add(SCstring[j]);
            }
        }
        /****/
        public void Step1scode2X(int num)
        {
            for (int k = 0; k < num; k++)
            {
                // SCstring[k] = ExtractNum(foundSCode[k].Value);
                if (string.Compare(Step1SCstring[k].Substring(0, 1), "4") < 0) Step1X[k] = "0";
                else Step1X[k] = "1";
                /**
                            if (SCstring[k].Substring(0, 1) =="0") getX[k]="0";
                            else if (SCstring[k].Substring(0, 1) == "3") getX[k] = "0";
                            else if (SCstring[k].Substring(0, 1) == "6") getX[k] = "1";
                **/
               // listBox3.Items.Add(getX[k]);
            }
        }
        /****/
        public void Step1getJMEway()
        {
            string[] Jmstring = new string[Step1DataLength];
            string JmMoney = @"""JmMoney"":""(?<JmMoney>.+?)""";
            MatchCollection foundJmMoney = (new Regex(JmMoney)).Matches(respHtml);
 
            /**/
            for (int j = 0; j < foundJmMoney.Count; j++)
            {
                Jmstring[j] = ExtractNum(foundJmMoney[j].Value);
            
            }
            /**/
            
            for (int k = 0; k < foundJmMoney.Count; k++)
            {
                Step1getJME[k] = double.Parse(Jmstring[k].Trim());
                Step1getJME[k] = Step1getJME[k] / 10000.0;
                Step1getJME[k] = Math.Round(Step1getJME[k], 0);
               // listBox4.Items.Add(getJME[k]);
            }
        }
        /****/

        string Step1getWebDate()
        {
            //"Tdate":"2016-11-04"
           // string[] Jmstring = new string[DataLength];
            string JmMoney = @"""Tdate"":""(?<Tdate>.+?)""";
            MatchCollection foundJmMoney = (new Regex(JmMoney)).Matches(respHtml);
            string dataOut = ExtractDate(foundJmMoney[0].Value);
            /**
                    for (int i=0; i<DataLength; i++)
                    {                 
                        listBox2.Items.Add(foundJmMoney[i].Value);
                    }
            **/
            /**
            for (int j = 0; j < foundJmMoney.Count; j++)
            {
                Jmstring[j] = ExtractNum(foundJmMoney[j].Value);
                listBox2.Items.Add(Jmstring[j]);
            }
            **/
            return dataOut;
        }
       
        /**/
        string Step1getInternetDate()
        {
            string DateOut;
            DateTime nowtime =  DateTime.Now;

            int year = nowtime.Year;
            int month = nowtime.Month;
            int day = nowtime.Day;
            string Ystring=year.ToString();
            string Mstring=month.ToString();
            string Dstring=day.ToString();
            
            if (month < 10) Mstring = "0" + Mstring;
            if (day < 10)   Dstring = "0" + Dstring;

            DateOut = Ystring+Mstring+Dstring;

            return DateOut;
        }
        /**/
        public string ExtractNum(string sourse)
        {
            string strSplit2 = Regex.Replace(sourse, "[A-z]", "", RegexOptions.IgnoreCase);
            strSplit2 = Regex.Replace(strSplit2, "[:]", "", RegexOptions.IgnoreCase);
            strSplit2 = Regex.Replace(strSplit2, "[\"]", "", RegexOptions.IgnoreCase);
            return strSplit2;
        }

        public string ExtractDate(string sourse)
        {
            string strSplit2 = Regex.Replace(sourse, "[A-z]", "", RegexOptions.IgnoreCase);
            strSplit2 = Regex.Replace(strSplit2, "[:]", "", RegexOptions.IgnoreCase);
            strSplit2 = Regex.Replace(strSplit2, "[\"]", "", RegexOptions.IgnoreCase);
            strSplit2 = Regex.Replace(strSplit2, "-", "", RegexOptions.IgnoreCase);
            return strSplit2;
        }

    }
}
