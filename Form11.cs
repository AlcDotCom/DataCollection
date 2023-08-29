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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downtime_registration
{
    public partial class Form11 : Form
    {
        private string DispString;
        private string COMPORT;
        SerialPort SP;
        public Form11()
        {
            InitializeComponent();
            textBox1.Text = "";
            Settings.Default.rfidsetup = "3";
            Settings.Default.AktualnyUzivatel = "";
            Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            Dispose();
            _1 nextForm = new _1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "papluh")
            {
                serialPort1.Close();
                Dispose();
                Form12 nextForm = new Form12(); 
                Hide();
                nextForm.ShowDialog();
                Close();
                return;
            }

            try
            {
                using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand Comm1 = new SqlCommand("SELECT NASTAVENIE FROM access WHERE HEX = '" + textBox1.Text.ToString() + "'", Conn);
                    Conn.Open();
                    string pristup = Comm1.ExecuteScalar().ToString();
                    Settings.Default.rfidsetup = pristup;
                    Settings.Default.AktualnyUzivatel = textBox1.Text;
                    Settings.Default.Save();
                    Conn.Close();
                }
            }
            catch (Exception) { }

            if (Settings.Default.rfidsetup == "1")
            {
                serialPort1.Close();
                Dispose();
                Form12 nextForm = new Form12();
                Hide();
                nextForm.ShowDialog();
                Close();
                return;
            }
            else if (Settings.Default.rfidsetup == "0")
            {
                MessageBox.Show("Nemáte zriedný prístup do nastavení, pre zriadenie prístupu kontaktuje inžiniering");
                textBox1.Text = "";
                textBox1.Focus();
                Settings.Default.rfidsetup = "3";
                Settings.Default.Save();
                return;
            }
            else
            {
                MessageBox.Show("Nesprávne heslo");
                textBox1.Text = "";
                textBox1.Focus();
            }
        }

        private void Form11_Load(object sender, EventArgs e)
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
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                    TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "RFID čítačka nie je pripojená, vstup pomocou karty nie je možný!");
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
        textBox6.Text = DispString;   //HEX číslo zo serial portu
        }
        public new void Dispose()
        {
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length >= 5)
            {
                textBox1.Text = textBox6.Text;
                button2.Select();
            }
            else
            {
                textBox6.Text = "";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
            }
        }
    }
}
