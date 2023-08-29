using Downtime_registration.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Management;

namespace Downtime_registration
{
    public partial class OPERATIONS : Form
    {
        public static string Meno = "";
        public static string Zmena = "";
        public static string ButtonPrihlasMa = "";
        public static string ButtonOdhlasMa = "";
        public static string MenoOdhlasovaneho = "";
        private string DispString;
        private string COMPORT;
        private string IDclovek;
        SerialPort SP;
        public OPERATIONS()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN zariadenie
            { WindowState = FormWindowState.Normal; }

            LoadOperations();
            MarkOperations();
            NacitaniePrihlasenych();
            Settings.Default.VyberOperacie = 0;
            Settings.Default.OP = 0;
            Settings.Default.Save();

            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(ShowShift);
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void ShowShift(object sender, EventArgs e)
        {
            if (DateTime.Now >= Convert.ToDateTime("06:00:00") && DateTime.Now < Convert.ToDateTime("14:00:00"))
            { label1.Text = "Ranná zmena"; label1.ForeColor = Color.Green; }
            else if (DateTime.Now >= Convert.ToDateTime("14:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00"))
            { label1.Text = "Poobedná zmena"; label1.ForeColor = Color.Blue; }
            else if (DateTime.Now >= Convert.ToDateTime("22:00:00") || DateTime.Now < Convert.ToDateTime("06:00:00"))
            { label1.Text = "Nočná zmena"; label1.ForeColor = Color.OrangeRed; }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            serialPort1.Close();
            _1 nextForm = new _1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        void LoadOperations()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT OP1,OP2,OP3,OP4,OP5,OP6,OP7,OP8,OP9,OP10,OP11,OP12 FROM Operacie WHERE Linka = '" + Settings.Default.NazovZariadenia + "'", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        string operacia1 = (sqlReader["OP1"].ToString());
                        if (operacia1 != "")
                        {
                            button1.Visible = true; button1.Text = operacia1;
                            if (button1.Text.Contains("+ 5S")) { panel1.BackColor = Color.Yellow; }
                        }
                        string operacia2 = (sqlReader["OP2"].ToString());
                        if (operacia2 != "")
                        {
                            button3.Visible = true; button3.Text = operacia2;
                            if (button3.Text.Contains("+ 5S")) { panel2.BackColor = Color.Yellow; }
                        }
                        string operacia3 = (sqlReader["OP3"].ToString());
                        if (operacia3 != "")
                        {
                            button4.Visible = true; button4.Text = operacia3;
                            if (button4.Text.Contains("+ 5S")) { panel3.BackColor = Color.Yellow; }
                        }
                        string operacia4 = (sqlReader["OP4"].ToString());
                        if (operacia4 != "")
                        {
                            button5.Visible = true; button5.Text = operacia4;
                            if (button5.Text.Contains("+ 5S")) { panel4.BackColor = Color.Yellow; }
                        }
                        string operacia5 = (sqlReader["OP5"].ToString());
                        if (operacia5 != "")
                        {
                            button6.Visible = true; button6.Text = operacia5;
                            if (button6.Text.Contains("+ 5S")) { panel5.BackColor = Color.Yellow; }
                        }
                        string operacia6 = (sqlReader["OP6"].ToString());
                        if (operacia6 != "")
                        {
                            button7.Visible = true; button7.Text = operacia6;
                            if (button7.Text.Contains("+ 5S")) { panel6.BackColor = Color.Yellow; }
                        }
                        string operacia7 = (sqlReader["OP7"].ToString());
                        if (operacia7 != "")
                        {
                            button8.Visible = true; button8.Text = operacia7;
                            if (button8.Text.Contains("+ 5S")) { panel7.BackColor = Color.Yellow; }
                        }
                        string operacia8 = (sqlReader["OP8"].ToString());
                        if (operacia8 != "")
                        {
                            button9.Visible = true; button9.Text = operacia8;
                            if (button9.Text.Contains("+ 5S")) { panel8.BackColor = Color.Yellow; }
                        }
                        string operacia9 = (sqlReader["OP9"].ToString());
                        if (operacia9 != "")
                        {
                            button10.Visible = true; button10.Text = operacia9;
                            if (button10.Text.Contains("+ 5S")) { panel9.BackColor = Color.Yellow; }
                        }
                        string operacia10 = (sqlReader["OP10"].ToString());
                        if (operacia10 != "")
                        {
                            button11.Visible = true; button11.Text = operacia10;
                            if (button11.Text.Contains("+ 5S")) { panel10.BackColor = Color.Yellow; }
                        }
                        string operacia11 = (sqlReader["OP11"].ToString());
                        if (operacia11 != "")
                        {
                            button12.Visible = true; button12.Text = operacia11;
                            if (button12.Text.Contains("+ 5S")) { panel11.BackColor = Color.Yellow; }
                        }
                        string operacia12 = (sqlReader["OP12"].ToString());
                        if (operacia12 != "")
                        {
                            button13.Visible = true; button13.Text = operacia12;
                            if (button13.Text.Contains("+ 5S")) { panel12.BackColor = Color.Yellow; }
                        }
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            { }
        }
        void MarkOperations()
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
        }

        private void button1_Click(object sender, EventArgs e)  //1. OP
        {
            button1.BackColor = Color.Yellow;
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 1;
            Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)  //2. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            button3.BackColor = Color.Yellow;
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 2;
            Settings.Default.Save();
        }

        private void button4_Click(object sender, EventArgs e) //3. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            button4.BackColor = Color.Yellow;
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 3;
            Settings.Default.Save();
        }

        private void button5_Click(object sender, EventArgs e)  //4. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            button5.BackColor = Color.Yellow;
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 4;
            Settings.Default.Save();
        }

        private void button6_Click(object sender, EventArgs e)  //5. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            button6.BackColor = Color.Yellow;
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 5;
            Settings.Default.Save();
        }

        private void button7_Click(object sender, EventArgs e)  //6. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            button7.BackColor = Color.Yellow;
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 6;
            Settings.Default.Save();
        }

        private void button8_Click(object sender, EventArgs e) //7. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            button8.BackColor = Color.Yellow;
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 7;
            Settings.Default.Save();
        }

        private void button9_Click(object sender, EventArgs e)  //8. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            button9.BackColor = Color.Yellow;
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 8;
            Settings.Default.Save();
        }

        private void button10_Click(object sender, EventArgs e) //9. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            button10.BackColor = Color.Yellow;
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 9;
            Settings.Default.Save();
        }

        private void button11_Click(object sender, EventArgs e)  //10. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            button11.BackColor = Color.Yellow;
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 10;
            Settings.Default.Save();
        }

        private void button12_Click(object sender, EventArgs e)  //11. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            button12.BackColor = Color.Yellow;
            if (textBox12.Text == "") { button13.BackColor = Color.SteelBlue; } else { button13.BackColor = Color.Green; }
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 11;
            Settings.Default.Save();
        }

        private void button13_Click(object sender, EventArgs e)  //12. OP
        {
            if (textBox1.Text == "") { button1.BackColor = Color.SteelBlue; } else { button1.BackColor = Color.Green; }
            if (textBox2.Text == "") { button3.BackColor = Color.SteelBlue; } else { button3.BackColor = Color.Green; }
            if (textBox3.Text == "") { button4.BackColor = Color.SteelBlue; } else { button4.BackColor = Color.Green; }
            if (textBox4.Text == "") { button5.BackColor = Color.SteelBlue; } else { button5.BackColor = Color.Green; }
            if (textBox5.Text == "") { button6.BackColor = Color.SteelBlue; } else { button6.BackColor = Color.Green; }
            if (textBox6.Text == "") { button7.BackColor = Color.SteelBlue; } else { button7.BackColor = Color.Green; }
            if (textBox7.Text == "") { button8.BackColor = Color.SteelBlue; } else { button8.BackColor = Color.Green; }
            if (textBox8.Text == "") { button9.BackColor = Color.SteelBlue; } else { button9.BackColor = Color.Green; }
            if (textBox9.Text == "") { button10.BackColor = Color.SteelBlue; } else { button10.BackColor = Color.Green; }
            if (textBox10.Text == "") { button11.BackColor = Color.SteelBlue; } else { button11.BackColor = Color.Green; }
            if (textBox11.Text == "") { button12.BackColor = Color.SteelBlue; } else { button12.BackColor = Color.Green; }
            button13.BackColor = Color.Yellow;
            Settings.Default.VyberOperacie = 1;
            Settings.Default.OP = 12;
            Settings.Default.Save();
        }

        private void OPERATIONS_Load(object sender, EventArgs e)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                string rightCOMport = (string)queryObj["DeviceID"] + (string)queryObj["PNPDeviceID"] + queryObj["Name"];
                if (rightCOMport.Contains("VID_04D8&PID_FC27&MI_02"))  // RFID reader's name (device manager)
                {
                    string devicename = (string)queryObj["Name"];
                    string toFind = "COM"; int start = devicename.IndexOf(toFind);
                    string COMwithParenthesis = devicename.Substring(start);
                    COMPORT = COMwithParenthesis.Replace(")", "");
                }
            }
            SP = new SerialPort();
            if (SP.IsOpen == false)
            {
                try
                {
                    serialPort1.PortName = COMPORT;
                    serialPort1.BaudRate = 9600;
                    serialPort1.DataBits = 8;
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    serialPort1.ReadTimeout = 500;
                    if (serialPort1.IsOpen)
                    {
                        DispString = "";
                        textBox13.Text = "";
                    }
                }
                catch (Exception)
                {
                    serialPort1.Close();
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "RFID čítačka nie je pripojená, prihlásenie/odhlásenie nie je možné!");
                }
            }
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(RFID_DataReceived);
        }
        private void RFID_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (DispString.Length >= 30)
            {
                serialPort1.Close();
            }
            else
            {
                DispString = serialPort1.ReadLine();
                DispString = (DispString.Remove(0, 4));   // zmaže predvolbu 4200,4100...
                Invoke(new EventHandler(DisplayText));
            }
        }
        private void DisplayText(object sender, EventArgs e)
        {
            textBox13.Text = DispString;     //HEX zo serial portu
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox13.Text.Length >= 5)
            {
                if (Settings.Default.VyberOperacie == 1)
                {
                    try
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                        {
                            SqlCommand sqlCmd = new SqlCommand("SELECT id_doch,meno,priez FROM cardsX WHERE card_id LIKE '%" + textBox13.Text.ToString() + "%'", sqlConnection);
                            sqlConnection.Open();
                            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                            while (sqlReader.Read())
                            {
                                IDclovek = (sqlReader["meno"].ToString() + " " + sqlReader["priez"].ToString() + " " + sqlReader["id_doch"].ToString().TrimStart(new Char[] { '0' }));
                            }
                            sqlReader.Close();
                            Meno = IDclovek;
                            PrihlasenieDoOperacie();
                            textBox13.Text = "";
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Nie je možné sa prihlásiť k operácii, vyskytol sa problém s pripojením k serveru");
                    }
                }
                else
                {
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(1.2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "Vyberte operáciu");
                    textBox13.Text = "";
                }
            }
            else
            {
                textBox13.Text = "";
            }
        }
        void ZistiZmenu()
        {
            var teraz = DateTime.Now;
            DateTime t1 = Convert.ToDateTime("06:00:00");
            DateTime t2 = Convert.ToDateTime("14:00:00");
            DateTime t3 = Convert.ToDateTime("22:00:00");
            if (teraz >= t1 && teraz < t2)
            {
                Zmena = "Ranna";
            }
            else if (teraz >= t2 && teraz < t3)
            {
                Zmena = "Poobedna";
            }
            else if (teraz >= t3 || teraz < t1)
            {
                Zmena = "Nocna";
            }
        }
        void NacitaniePrihlasenych()
        {
            ZistiZmenu();
            if (button1.Text != "N/A")
            {
                try //1.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button1.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox1.Text = prihlaseny; textBox1.Visible = true; button1.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button3.Text != "N/A")
            {
                try //2.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button3.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox2.Text = prihlaseny; textBox2.Visible = true; button3.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button3.Text != "N/A")
            {
                try //3.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button4.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox3.Text = prihlaseny; textBox3.Visible = true; button4.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button5.Text != "N/A")
            {
                try //4.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button5.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox4.Text = prihlaseny; textBox4.Visible = true; button5.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button6.Text != "N/A")
            {
                try //5.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button6.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox5.Text = prihlaseny; textBox5.Visible = true; button6.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button7.Text != "N/A")
            {
                try //6.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button7.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox6.Text = prihlaseny; textBox6.Visible = true; button7.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button8.Text != "N/A")
            {
                try //7.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button8.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox7.Text = prihlaseny; textBox7.Visible = true; button8.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button9.Text != "N/A")
            {
                try //8.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button9.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox8.Text = prihlaseny; textBox8.Visible = true; button9.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button10.Text != "N/A")
            {
                try //9.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button10.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox9.Text = prihlaseny; textBox9.Visible = true; button10.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button11.Text != "N/A")
            {
                try //10.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button11.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox10.Text = prihlaseny; textBox10.Visible = true; button11.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button12.Text != "N/A")
            {
                try //11.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button12.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox11.Text = prihlaseny; textBox11.Visible = true; button12.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }

            if (button13.Text != "N/A")
            {
                try //12.OP
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Meno FROM OperaciePrihlasenie WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + button13.Text + "' AND Odhlasenie IS NULL AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", sqlConnection);
                        sqlConnection.Open(); SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string prihlaseny = (sqlReader["Meno"].ToString());
                            if (prihlaseny != "")
                            {
                                textBox12.Text = prihlaseny; textBox12.Visible = true; button13.BackColor = Color.Green;
                            }
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) { }
            }
        }


        void PrihlasenieDoOperacie()
        {
            ZistiZmenu();

            if (Settings.Default.OP == 1)//prihlásenie do 1.OP (BTN1 TXTBX1)
            {
                //ODHLASENIE
                if (textBox1.Text == Meno)  // iba ODHLASENIE - ak už je užívateľ prihlásený na vybranej operácii
                {
                    ButtonOdhlasMa = button1.Text; OdhlasMa();
                    textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else //ODHLASENIE z inej operácie
                {
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue;}
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue;}
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue;}
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue;}
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue;}
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue;}
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue;}
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue;}
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue;}
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue;}
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue;}
                   
                    if (textBox1.Text == "")//ak na operácii nikto nie je prihlasi ma
                    {
                        textBox1.Text = Meno; ButtonPrihlasMa = button1.Text; PrihlasMa();
                        button1.BackColor = Color.Green; textBox1.Visible = true;
                    }
                    else //ak na operácii už niekto je, odhlási ho a prihlási ma
                    {
                        MenoOdhlasovaneho = textBox1.Text; ButtonOdhlasMa = button1.Text; OdhlasKolegu();
                        textBox1.Text = Meno; ButtonPrihlasMa = button1.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }


            if (Settings.Default.OP == 2)//prihlásenie do 2.OP (BTN3 TXTBX2)
            {
                if (textBox2.Text == Meno)
                {
                    ButtonOdhlasMa = button3.Text; OdhlasMa();
                    textBox2.Text = ""; textBox2.Visible = false; button3.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else 
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue;}
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue;}
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue;}
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue;}
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue;}
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue;}
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue;}
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue;}
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue;}
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue;}
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue;}

                    if (textBox2.Text == "")
                    {
                        textBox2.Text = Meno; ButtonPrihlasMa = button3.Text; PrihlasMa();
                        button3.BackColor = Color.Green; textBox2.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox2.Text; ButtonOdhlasMa = button3.Text; OdhlasKolegu();
                        textBox2.Text = Meno; ButtonPrihlasMa = button3.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 3)//prihlásenie do 3.OP (BTN4 TXTBX3)
            {
                if (textBox3.Text == Meno)
                {
                    ButtonOdhlasMa = button4.Text; OdhlasMa();
                    textBox3.Text = ""; textBox3.Visible = false; button4.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox3.Text == "")
                    {
                        textBox3.Text = Meno; ButtonPrihlasMa = button4.Text; PrihlasMa();
                        button4.BackColor = Color.Green; textBox3.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox3.Text; ButtonOdhlasMa = button4.Text; OdhlasKolegu();
                        textBox3.Text = Meno; ButtonPrihlasMa = button4.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 4)//prihlásenie do 4.OP (BTN5 TXTBX4)
            {
                if (textBox4.Text == Meno)
                {
                    ButtonOdhlasMa = button5.Text; OdhlasMa();
                    textBox4.Text = ""; textBox4.Visible = false; button5.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox4.Text == "")
                    {
                        textBox4.Text = Meno; ButtonPrihlasMa = button5.Text; PrihlasMa();
                        button5.BackColor = Color.Green; textBox4.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox4.Text; ButtonOdhlasMa = button5.Text; OdhlasKolegu();
                        textBox4.Text = Meno; ButtonPrihlasMa = button5.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 5)//prihlásenie do 5.OP (BTN6 TXTBX5)
            {
                if (textBox5.Text == Meno)
                {
                    ButtonOdhlasMa = button6.Text; OdhlasMa();
                    textBox5.Text = ""; textBox5.Visible = false; button6.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox5.Text == "")
                    {
                        textBox5.Text = Meno; ButtonPrihlasMa = button6.Text; PrihlasMa();
                        button6.BackColor = Color.Green; textBox5.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox5.Text; ButtonOdhlasMa = button6.Text; OdhlasKolegu();
                        textBox5.Text = Meno; ButtonPrihlasMa = button6.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 6)//prihlásenie do 6.OP (BTN7 TXTBX6)
            {
                if (textBox6.Text == Meno)
                {
                    ButtonOdhlasMa = button7.Text; OdhlasMa();
                    textBox6.Text = ""; textBox6.Visible = false; button7.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox6.Text == "")
                    {
                        textBox6.Text = Meno; ButtonPrihlasMa = button7.Text; PrihlasMa();
                        button7.BackColor = Color.Green; textBox6.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox6.Text; ButtonOdhlasMa = button7.Text; OdhlasKolegu();
                        textBox6.Text = Meno; ButtonPrihlasMa = button7.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 7)//prihlásenie do 7.OP (BTN8 TXTBX7)
            {
                if (textBox7.Text == Meno)
                {
                    ButtonOdhlasMa = button8.Text; OdhlasMa();
                    textBox7.Text = ""; textBox7.Visible = false; button8.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox7.Text == "")
                    {
                        textBox7.Text = Meno; ButtonPrihlasMa = button8.Text; PrihlasMa();
                        button8.BackColor = Color.Green; textBox7.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox7.Text; ButtonOdhlasMa = button8.Text; OdhlasKolegu();
                        textBox7.Text = Meno; ButtonPrihlasMa = button8.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 8)//prihlásenie do 8.OP (BTN9 TXTBX8)
            {
                if (textBox8.Text == Meno)
                {
                    ButtonOdhlasMa = button9.Text; OdhlasMa();
                    textBox8.Text = ""; textBox8.Visible = false; button9.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox8.Text == "")
                    {
                        textBox8.Text = Meno; ButtonPrihlasMa = button9.Text; PrihlasMa();
                        button9.BackColor = Color.Green; textBox8.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox8.Text; ButtonOdhlasMa = button9.Text; OdhlasKolegu();
                        textBox8.Text = Meno; ButtonPrihlasMa = button9.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 9)//prihlásenie do 9.OP (BTN10 TXTBX9)
            {
                if (textBox9.Text == Meno)
                {
                    ButtonOdhlasMa = button10.Text; OdhlasMa();
                    textBox9.Text = ""; textBox9.Visible = false; button10.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox9.Text == "")
                    {
                        textBox9.Text = Meno; ButtonPrihlasMa = button10.Text; PrihlasMa();
                        button10.BackColor = Color.Green; textBox9.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox9.Text; ButtonOdhlasMa = button10.Text; OdhlasKolegu();
                        textBox9.Text = Meno; ButtonPrihlasMa = button10.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 10)//prihlásenie do 10.OP (BTN11 TXTBX10)
            {
                if (textBox10.Text == Meno)
                {
                    ButtonOdhlasMa = button11.Text; OdhlasMa();
                    textBox10.Text = ""; textBox10.Visible = false; button11.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox10.Text == "")
                    {
                        textBox10.Text = Meno; ButtonPrihlasMa = button11.Text; PrihlasMa();
                        button11.BackColor = Color.Green; textBox10.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox10.Text; ButtonOdhlasMa = button11.Text; OdhlasKolegu();
                        textBox10.Text = Meno; ButtonPrihlasMa = button11.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 11)//prihlásenie do 11.OP (BTN12 TXTBX11)
            {
                if (textBox11.Text == Meno)
                {
                    ButtonOdhlasMa = button12.Text; OdhlasMa();
                    textBox11.Text = ""; textBox11.Visible = false; button12.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox12.Text == Meno) { ButtonOdhlasMa = button13.Text; OdhlasMa(); textBox12.Visible = false; textBox12.Text = ""; button13.BackColor = Color.SteelBlue; }

                    if (textBox11.Text == "")
                    {
                        textBox11.Text = Meno; ButtonPrihlasMa = button12.Text; PrihlasMa();
                        button12.BackColor = Color.Green; textBox11.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox11.Text; ButtonOdhlasMa = button12.Text; OdhlasKolegu();
                        textBox11.Text = Meno; ButtonPrihlasMa = button12.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }

            if (Settings.Default.OP == 12)//prihlásenie do 12.OP (BTN13 TXTBX12)
            {
                if (textBox12.Text == Meno)
                {
                    ButtonOdhlasMa = button13.Text; OdhlasMa();
                    textBox12.Text = ""; textBox12.Visible = false; button13.BackColor = Color.SteelBlue; Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save(); return;
                }
                else
                {
                    if (textBox1.Text == Meno) { ButtonOdhlasMa = button1.Text; OdhlasMa(); textBox1.Visible = false; textBox1.Text = ""; button1.BackColor = Color.SteelBlue; }
                    if (textBox2.Text == Meno) { ButtonOdhlasMa = button3.Text; OdhlasMa(); textBox2.Visible = false; textBox2.Text = ""; button3.BackColor = Color.SteelBlue; }
                    if (textBox3.Text == Meno) { ButtonOdhlasMa = button4.Text; OdhlasMa(); textBox3.Visible = false; textBox3.Text = ""; button4.BackColor = Color.SteelBlue; }
                    if (textBox4.Text == Meno) { ButtonOdhlasMa = button5.Text; OdhlasMa(); textBox4.Visible = false; textBox4.Text = ""; button5.BackColor = Color.SteelBlue; }
                    if (textBox5.Text == Meno) { ButtonOdhlasMa = button6.Text; OdhlasMa(); textBox5.Visible = false; textBox5.Text = ""; button6.BackColor = Color.SteelBlue; }
                    if (textBox6.Text == Meno) { ButtonOdhlasMa = button7.Text; OdhlasMa(); textBox6.Visible = false; textBox6.Text = ""; button7.BackColor = Color.SteelBlue; }
                    if (textBox7.Text == Meno) { ButtonOdhlasMa = button8.Text; OdhlasMa(); textBox7.Visible = false; textBox7.Text = ""; button8.BackColor = Color.SteelBlue; }
                    if (textBox8.Text == Meno) { ButtonOdhlasMa = button9.Text; OdhlasMa(); textBox8.Visible = false; textBox8.Text = ""; button9.BackColor = Color.SteelBlue; }
                    if (textBox9.Text == Meno) { ButtonOdhlasMa = button10.Text; OdhlasMa(); textBox9.Visible = false; textBox9.Text = ""; button10.BackColor = Color.SteelBlue; }
                    if (textBox10.Text == Meno) { ButtonOdhlasMa = button11.Text; OdhlasMa(); textBox10.Visible = false; textBox10.Text = ""; button11.BackColor = Color.SteelBlue; }
                    if (textBox11.Text == Meno) { ButtonOdhlasMa = button12.Text; OdhlasMa(); textBox11.Visible = false; textBox11.Text = ""; button12.BackColor = Color.SteelBlue; }

                    if (textBox12.Text == "")
                    {
                        textBox12.Text = Meno; ButtonPrihlasMa = button13.Text; PrihlasMa();
                        button13.BackColor = Color.Green; textBox12.Visible = true;
                    }
                    else
                    {
                        MenoOdhlasovaneho = textBox12.Text; ButtonOdhlasMa = button13.Text; OdhlasKolegu();
                        textBox12.Text = Meno; ButtonPrihlasMa = button13.Text; PrihlasMa();
                    }
                    Settings.Default.OP = 0; Settings.Default.VyberOperacie = 0; Settings.Default.Save();
                }
            }
        }
        void PrihlasMa()
        {
            using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
            {
                Conn.Open();
                try
                {
                    string In = "insert into OperaciePrihlasenie (Linka,Operacia,Meno,Zmena,Prihlasenie)values('" + Settings.Default.NazovZariadenia + "','" + ButtonPrihlasMa + "','" + Meno + "','" + Zmena + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    SqlCommand cmd = new SqlCommand(In, Conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception) { }
            }
        }
        void OdhlasMa()
        {
            using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
            {
                Conn.Open();
                try
                {
                    string In = "UPDATE OperaciePrihlasenie SET Odhlasenie = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Meno = '" + Meno + "' AND Operacia = '" + ButtonOdhlasMa + "' AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' AND Odhlasenie IS NULL";
                    SqlCommand cmd = new SqlCommand(In, Conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception) { }
            }
        }
        void OdhlasKolegu()
        {
            using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
            {
                Conn.Open();
                try
                {
                    string In = "UPDATE OperaciePrihlasenie SET Odhlasenie = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Meno = '" + MenoOdhlasovaneho + "' AND Operacia = '" + ButtonOdhlasMa + "' AND Zmena = '" + Zmena + "' AND Prihlasenie > '" + DateTime.Now.Subtract(TimeSpan.FromHours(9)).ToString("yyyy-MM-dd HH:mm:ss") + "' AND Odhlasenie IS NULL";
                    SqlCommand cmd = new SqlCommand(In, Conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception) { }
            }
        }

    }
}
