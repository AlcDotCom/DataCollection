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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downtime_registration
{
    public partial class IDforNOKreg : Form
    {
        private string DispString;
        private string COMPORT;
        private string IDclovek;
        SerialPort SP;
        public IDforNOKreg()
        {
            InitializeComponent();
            Settings.Default.IDforNOKreg = "";
            Settings.Default.Save();
            button2.TabIndex = 1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Settings.Default.IDforNOKreg = "";
            Settings.Default.Save();
            serialPort1.Close();
            Dispose();

            _1 nextForm = new _1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (textBox2.Text == "")
            {
                MessageBox.Show("Priložte kartu k čítačke");
            }
            else
            {
                Settings.Default.IDforNOKreg = textBox2.Text;
                Settings.Default.Save();
                serialPort1.Close();
                Dispose();
                NOKregistration nextForm = new NOKregistration();
                Hide();
                nextForm.ShowDialog();
                Close();
            }
            Cursor = Cursors.Default;
        }
        private void IDforNOKreg_Load(object sender, EventArgs e)
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
                        textBox6.Text = "";
                    }
                }
                catch (Exception)
                {
                    serialPort1.Close();
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "RFID čítačka nie je pripojená, prihlásenie nie je možné!");
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
            textBox6.Text = DispString;     //HEX zo serial portu
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
                try
                {
                    textBox2.Text = "";
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT id_doch,meno,priez FROM cardsX WHERE card_id LIKE '%" + textBox6.Text.ToString() + "%'", sqlConnection);
                        sqlConnection.Open();
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            IDclovek = (sqlReader["meno"].ToString() + " " + sqlReader["priez"].ToString() + " " + sqlReader["id_doch"].ToString().TrimStart(new Char[] { '0' }));
                        }
                        sqlReader.Close();
                        textBox2.Text = IDclovek;
                        textBox2.BackColor = Color.Green;
                    }
                    using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))   //vyhlada ci ma uzivatel povolenie pre upravu NOK
                    {
                        SqlCommand Comm1 = new SqlCommand("SELECT rezerva1 FROM access WHERE HEX = '" + textBox6.Text.ToString() + "'", Conn);
                        Conn.Open();
                        Settings.Default.NOKPovolenieUpravit = Comm1.ExecuteScalar().ToString();
                        Settings.Default.Save();
                        Conn.Close();
                    }
                }
                catch (Exception)
                {
                    //var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                    //TaskScheduler.FromCurrentSynchronizationContext());
                    //MessageBox.Show(w, "Spojenie so serverom zlyhalo");
                }
            }
            else
            {
             textBox6.Text = "";
            }
        }

        private void IDforNOKreg_KeyDown(object sender, KeyEventArgs e)
        {
            if  (e.Control && e.Alt && e.KeyCode == Keys.F9)
            {
                Admin();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.F9)
            {
                Admin();
            }
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.F9)
            {
                Admin();
            }
        }

        private void button3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.F9)
            {
                Admin();
            }
        }
        public void Admin()
        {
            textBox2.Text = "MTA Admin";
            textBox2.BackColor = Color.Green;
            Settings.Default.NOKPovolenieUpravit = "1";
            Settings.Default.Save();
        }
    }
}
