using Downtime_registration.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downtime_registration
{
    public partial class NewUser : Form
    {
        private string DispString;
        private string COMPORT;
        SerialPort SP;
        public NewUser()
        {
            InitializeComponent();
        }

        private void NewUser_Load_1(object sender, EventArgs e)
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
                    }
                }
                catch (Exception)
                {
                    serialPort1.Close();
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "RFID čítačka nie je pripojená, nie je možné pridať nového užívateľa!");
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
            textBox1.Text = DispString;   //HEX karty
            label1.Visible = false;
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            button1.Visible = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox2.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                if (textBox4.Text == "1" || textBox4.Text == "0")
                {
                    if (textBox5.Text == "1" || textBox5.Text == "0")
                    {
                        if (textBox6.Text == "1" || textBox6.Text == "0")
                        {
                            using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
                            {
                                Conn.Open();
                                string CheckDuplicity = "SELECT COUNT(*) FROM access WHERE HEX = '" + textBox1.Text + "'";
                                SqlCommand cmnd = new SqlCommand(CheckDuplicity, Conn);
                                cmnd.ExecuteNonQuery();
                                int COUNT = Convert.ToInt32(cmnd.ExecuteScalar());
                                if (COUNT >= 1)
                                {
                                    var ww = new Form(); Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith((t) => ww.Close(),
                                    TaskScheduler.FromCurrentSynchronizationContext());
                                    MessageBox.Show(ww, "Tento užívateľ už existuje");
                                    return;
                                }
                                string NewUser = "insert into access (HEX,NASTAVENIE,VYMAZ,MENO,PRIEZVISKO,zalozil,datum,rezerva1,rezerva2)values('" + textBox1.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + Settings.Default.AktualnyUzivatel + "','" + DateTime.Now.ToString("yyyy - MM - dd HH: mm: ss") + "','" + textBox6.Text + "','" + Environment.MachineName + "')";
                                SqlCommand cmd = new SqlCommand(NewUser, Conn);
                                cmd.ExecuteNonQuery();
                                Conn.Close();

                                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith((t) => w.Close(),
                                TaskScheduler.FromCurrentSynchronizationContext());
                                MessageBox.Show(w, "Nový užívateľ bol vložený");
                                serialPort1.Close();
                                Form118 nextForm = new Form118();
                                Hide();
                                nextForm.ShowDialog();
                                Close();
                            }
                        }
                        else
                        {
                            var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                            TaskScheduler.FromCurrentSynchronizationContext());
                            MessageBox.Show(w, "Zadajte 1 (prístup povolený), alebo 0 (prístup zamietnutý)");
                        }
                    }
                    else
                    {
                        var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                        TaskScheduler.FromCurrentSynchronizationContext());
                        MessageBox.Show(w, "Zadajte 1 (prístup povolený), alebo 0 (prístup zamietnutý)");
                    }
                }
                else
                {
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                    TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "Zadajte 1 (prístup povolený), alebo 0 (prístup zamietnutý)");
                }
            }
            else
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Vyplňte všetky polia");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            serialPort1.Close();
            Form118 nextForm = new Form118();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
    }
}
