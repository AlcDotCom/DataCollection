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
    public partial class PCBprodRelease : Form
    {
        private string COMPORTQR;
        private string COMPORTRFID;
        private string DispStringQR;
        private string DispStringRFID;
        private string Zmena;
        private string InputPCBname;
        private string InputPCBcode;
        private string Batch;
        private string SN;
        private string PressHeight1_1Result;
        private string PressHeight1_3Result;
        private string VizualInspectionResult;

        //strings identifying products by read data in QR///
        private string QRhintenBASE =   "2001980";
        private string QRhintenMIDDLE = "2001890";
        private string QRhintenHIGH =   "2001880";
        private string QRunderhood =    "30094220";
        private string QRunderhoodE44 = "30012902";
        //////////////////////////////////////////////

        SerialPort SP;
        public PCBprodRelease()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" || textBox4.Text != "")
            {
                if (MessageBox.Show("Vrátiť späť?", "ZAPÍSANÉ DÁTA SA STRATIA", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    serialPort1.Close();
                    serialPort2.Close();
                    _1 nextForm = new _1();
                    Hide();
                    nextForm.ShowDialog();
                    Close();
                }
            }
            else
            {
                serialPort1.Close();
                serialPort2.Close();
                _1 nextForm = new _1();
                Hide();
                nextForm.ShowDialog();
                Close();
            }
        }

        private void PCBprodRelease_Load(object sender, EventArgs e)
        {
            try  //test ping
            {
                using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
                    connection.Open();
            }
            catch (SqlException)
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                 TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Pripojenie k serveru zlyhalo, nie je možné zapísať uvoľnenie výroby!");
            }

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                string rightCOMport = (string)queryObj["DeviceID"] + (string)queryObj["PNPDeviceID"] + queryObj["Name"];
                if (rightCOMport.Contains("05E0&PID"))  /////////// meno QR scanner (device manager)
                {
                    //QR reader Honeywell 1400g -//- HID\VID_0C2E&PID_0B81&MI_01\7&34ab42f0&0&0000
                    string devicename = (string)queryObj["Name"];
                    string toFind = "COM";
                    int start = devicename.IndexOf(toFind);
                    if (start >= 0)
                    { string COMwithParenthesis = devicename.Substring(start);
                    string COMport = COMwithParenthesis.Replace(")", "");
                    COMPORTQR = COMport; }
                }

                if (rightCOMport.Contains("VID_04D8&PID_FC27&MI_02"))  /////////// RFID reader (device manager)
                {
                    string devicename = (string)queryObj["Name"];
                    string toFind = "COM";
                    int start = devicename.IndexOf(toFind);
                    string COMwithParenthesis = devicename.Substring(start);
                    string COMport = COMwithParenthesis.Replace(")", "");
                    COMPORTRFID = COMport;
                }
            }

            SP = new SerialPort();
            if (SP.IsOpen == false)
            {
                try
                {
                    serialPort1.PortName = COMPORTQR;
                    serialPort1.BaudRate = 9600;
                    serialPort1.DataBits = 8;
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    serialPort1.ReadTimeout = 5000;
                    if (serialPort1.IsOpen)
                    { DispStringQR = ""; }
                }
                catch (Exception)
                {
                    serialPort1.Close();
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "Skener QR kódov nie je pripojený, nie je možné zapísať uvoľnenie výroby!");
                }

                try
                {
                    serialPort2.PortName = COMPORTRFID;
                    serialPort2.BaudRate = 9600;
                    serialPort2.DataBits = 8;
                    serialPort2.Parity = Parity.None;
                    serialPort2.StopBits = StopBits.One;
                    serialPort2.Open();
                    serialPort2.ReadTimeout = 500;
                    if (serialPort2.IsOpen)
                    { DispStringRFID = ""; }
                }
                catch (Exception)
                {
                    serialPort2.Close();
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "RFID čítačka nie je pripojená, nie je možné zapísať uvoľnenie výroby!");
                }
            }
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(RFID_DataReceivedQR);
            serialPort2.DataReceived += new SerialDataReceivedEventHandler(RFID_DataReceivedRFID);
        }
        private void RFID_DataReceivedQR(object sender, SerialDataReceivedEventArgs e)
        {
            if (DispStringQR.Length >= 50)
            {
                serialPort1.Close();
            }
            else
            {
                DispStringQR = serialPort1.ReadExisting();
                Invoke(new EventHandler(DisplayTextQR));
            }
        }
        private void RFID_DataReceivedRFID(object sender, SerialDataReceivedEventArgs e)
        {
            if (DispStringRFID.Length >= 50)
            {
                serialPort2.Close();
            }
            else
            {
                DispStringRFID = serialPort2.ReadLine();
                Invoke(new EventHandler(DisplayTextRFID));
            }
        }
        private void DisplayTextQR(object sender, EventArgs e)
        {
            //DispStringQR = (DispStringQR.Remove(DispStringQR.Length - 3, 3));
            textBox1.Text = DispStringQR;

            if(textBox1.TextLength > 10)
            {textBox1.BackColor = Color.Green; }
        }
        private void DisplayTextRFID(object sender, EventArgs e)
        {
            DispStringRFID = (DispStringRFID.Remove(0, 4));
            textBox2.Text = DispStringRFID;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length >= 5)
            {
                try
                {
                    textBox3.Text = "";
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT id_doch,meno,priez FROM cardsX WHERE card_id LIKE '%" + textBox2.Text.ToString() + "%'", sqlConnection);
                        sqlConnection.Open();
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            string USER = (sqlReader["id_doch"].ToString().TrimStart(new Char[] { '0' }) + " " + sqlReader["meno"].ToString() + " " + sqlReader["priez"].ToString());
                            textBox3.Text = USER;   
                            textBox3.BackColor = Color.Green;
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception) 
                {
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "Spojenie so serverom zlyhalo");
                }
            }
            else
            { textBox2.Text = ""; }
        }

        private void cbxDesign_DrawItem(object sender, DrawItemEventArgs e) 
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();
                if (e.Index >= 0)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    Brush brush = new SolidBrush(cbx.ForeColor);
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                if (textBox1.Text.Length < 12) //usually 19 chars
                { 
                    MessageBox.Show("Neplatný QR kód"); 
                    textBox1.Text = "";
                    tableLayoutPanel1.Visible = false;
                    comboBox1.Items.Clear();
                    comboBox1.SelectedIndex = -1;
                    return; 
                }

                //any unknown/new product with more than 11 chars
                tableLayoutPanel1.Visible = false;
                comboBox1.Items.Clear();
                comboBox1.Items.Add("TOP/vrchná strana");
                comboBox1.Items.Add("BOT/spodná strana");
                comboBox1.SelectedIndex = -1;

                InputPCBname = "N/A";
                InputPCBcode = "N/A";
                Batch = "N/A";
                SN = "N/A";

                //HINTEN base////////////////////////////////////////////////////////////////////////////////////////////
                if (textBox1.Text.Contains(QRhintenBASE))
                {
                    label2.Text = "Popis: PCB HINTEN Základná verzia";
                    string PN = "Číslo materiálu: " + textBox1.Text.Remove(textBox1.Text.Length - 8);
                    string PN1 = PN.Insert(PN.Length - 2, "/");
                    label3.Text = PN1;
                    string batch = textBox1.Text.Remove(textBox1.Text.Length - 5);
                    string batch1 = batch.Substring(batch.Length - 3);

                    string year = batch1.Substring(2,1);
                    string week = batch1.Remove(batch1.Length - 1);
                    label4.Text = "Partita: " + batch1 + " (Týždeň " + week + ", Rok 202" + year + ")";

                    label5.Text = "Sériové číslo: " + textBox1.Text.Substring(textBox1.Text.Length - 5);
                    pictureBox1.Image = Resources.HINTEN_BASIC;
                    tableLayoutPanel1.Visible = true;
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("TOP/vrchná strana");
                    comboBox1.SelectedIndex = 0;

                    InputPCBname = "PCB HINTEN Základná verzia";
                    InputPCBcode = textBox1.Text.Remove(textBox1.Text.Length - 8);
                    Batch = batch1;
                    SN = textBox1.Text.Substring(textBox1.Text.Length - 5);
                }
                //HINTEN middle////////////////////////////////////////////////////////////////////////////////////////////
                if (textBox1.Text.Contains(QRhintenMIDDLE))
                {
                    label2.Text = "Popis: PCB HINTEN Stredná verzia";
                    string PN = "Číslo materiálu: " + textBox1.Text.Remove(textBox1.Text.Length - 8);
                    string PN1 = PN.Insert(PN.Length - 2, "/");
                    label3.Text = PN1;
                    string batch = textBox1.Text.Remove(textBox1.Text.Length - 5);
                    string batch1 = batch.Substring(batch.Length - 3);
                    string year = batch1.Substring(2, 1);
                    string week = batch1.Remove(batch1.Length - 1);
                    label4.Text = "Partita: " + batch1 + " (Týždeň " + week + ", Rok 202" + year + ")";
                    label5.Text = "Sériové číslo: " + textBox1.Text.Substring(textBox1.Text.Length - 5);
                    pictureBox1.Image = Resources.HINTEN_MIDDLE;
                    tableLayoutPanel1.Visible = true;
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("TOP/vrchná strana");
                    comboBox1.SelectedIndex = 0;

                    InputPCBname = "PCB HINTEN Stredná verzia";
                    InputPCBcode = textBox1.Text.Remove(textBox1.Text.Length - 8);
                    Batch = batch1;
                    SN = textBox1.Text.Substring(textBox1.Text.Length - 5);
                }
                //HINTEN high////////////////////////////////////////////////////////////////////////////////////////////
                if (textBox1.Text.Contains(QRhintenHIGH))
                {
                    label2.Text = "Popis: PCB HINTEN Plná verzia";
                    string PN = "Číslo materiálu: " + textBox1.Text.Remove(textBox1.Text.Length - 8);
                    string PN1 = PN.Insert(PN.Length - 2, "/");
                    label3.Text = PN1;
                    string batch = textBox1.Text.Remove(textBox1.Text.Length - 5);
                    string batch1 = batch.Substring(batch.Length - 3);
                    string year = batch1.Substring(2, 1);
                    string week = batch1.Remove(batch1.Length - 1);
                    label4.Text = "Partita: " + batch1 + " (Týždeň " + week + ", Rok 202" + year + ")";
                    label5.Text = "Sériové číslo: " + textBox1.Text.Substring(textBox1.Text.Length - 5);
                    pictureBox1.Image = Resources.HINTEN_FULL;
                    tableLayoutPanel1.Visible = true;
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("TOP/vrchná strana");
                    comboBox1.SelectedIndex = 0;

                    InputPCBname = "PCB HINTEN Plná verzia";
                    InputPCBcode = textBox1.Text.Remove(textBox1.Text.Length - 8);
                    Batch = batch1;
                    SN = textBox1.Text.Substring(textBox1.Text.Length - 5);
                }
                //UNDERHOOD////////////////////////////////////////////////////////////////////////////////////////////
                if (textBox1.Text.Contains(QRunderhood))
                {
                    tableLayoutPanel1.Visible = false;
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("TOP/vrchná strana");
                    comboBox1.Items.Add("BOT/spodná strana");
                    comboBox1.SelectedIndex = -1;
                    comboBox1.Focus();
                }
                //UNDERHOODE44//////////////////////////////////////////////////////////////////////////////////////////
                if (textBox1.Text.Contains(QRunderhoodE44))
                {
                    tableLayoutPanel1.Visible = false;
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("TOP/vrchná strana");
                    comboBox1.Items.Add("BOT/spodná strana");
                    comboBox1.SelectedIndex = -1;
                    comboBox1.Focus();
                }
            }
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains(QRunderhood) || textBox1.Text.Contains(QRunderhoodE44))
            {
                if(comboBox1.SelectedIndex == 0)  //TOP - 1.cycle
                {
                    label2.Text = "Popis: PCB UNDERHOOD 1. cyklus";
                    string PN = "Číslo materiálu: " + textBox1.Text.Remove(textBox1.Text.Length - 8);
                    string PN1 = PN.Insert(PN.Length - 2, "/");
                    label3.Text = PN1;
                    string batch = textBox1.Text.Remove(textBox1.Text.Length - 5);
                    string batch1 = batch.Substring(batch.Length - 3);
                    string year = batch1.Substring(2, 1);
                    string week = batch1.Remove(batch1.Length - 1);
                    label4.Text = "Partita: " + batch1 + " (Týždeň " + week + ", Rok 202" + year + ")";
                    label5.Text = "Sériové číslo: " + textBox1.Text.Substring(textBox1.Text.Length - 5);
                    pictureBox1.Image = Resources.UNDER_1;
                    tableLayoutPanel1.Visible = true;

                    InputPCBname = "PCB UNDERHOOD 1. cyklus";
                    InputPCBcode = textBox1.Text.Remove(textBox1.Text.Length - 8);
                    Batch = batch1;
                    SN = textBox1.Text.Substring(textBox1.Text.Length - 5);
                }
                if (comboBox1.SelectedIndex == 1)  //BOT - 2.cycle
                {
                    label2.Text = "Popis: PCB UNDERHOOD 2. cyklus";
                    string PN = "Číslo materiálu: " + textBox1.Text.Remove(textBox1.Text.Length - 8);
                    string PN1 = PN.Insert(PN.Length - 2, "/");
                    label3.Text = PN1;
                    string batch = textBox1.Text.Remove(textBox1.Text.Length - 5);
                    string batch1 = batch.Substring(batch.Length - 3);
                    string year = batch1.Substring(2, 1);
                    string week = batch1.Remove(batch1.Length - 1);
                    label4.Text = "Partita: " + batch1 + " (Týždeň " + week + ", Rok 202" + year + ")";
                    label5.Text = "Sériové číslo: " + textBox1.Text.Substring(textBox1.Text.Length - 5);
                    pictureBox1.Image = Resources.UNDER_2;
                    tableLayoutPanel1.Visible = true;

                    InputPCBname = "PCB UNDERHOOD 2. cyklus";
                    InputPCBcode = textBox1.Text.Remove(textBox1.Text.Length - 8);
                    Batch = batch1;
                    SN = textBox1.Text.Substring(textBox1.Text.Length - 5);
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) //OK
        {
            if(checkBox1.Checked == true)
            { checkBox1.BackColor = Color.Green; checkBox2.Checked = false; }
            else { checkBox1.BackColor = Color.DarkGray; }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) //NOK
        {
            if (checkBox2.Checked == true)
            { checkBox2.BackColor = Color.Red; checkBox1.Checked = false; textBox4.BackColor = Color.White; }
            else
            {
                checkBox2.BackColor = Color.DarkGray;
                if (checkBox3.Checked == false && checkBox5.Checked == false)
                { textBox4.BackColor = Color.LightGray; }
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) //OK
        {
            if (checkBox4.Checked == true)
            { checkBox4.BackColor = Color.Green; checkBox3.Checked = false; }
            else { checkBox4.BackColor = Color.DarkGray; }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) //NOK
        {
            if (checkBox3.Checked == true)
            { checkBox3.BackColor = Color.Red; checkBox4.Checked = false; textBox4.BackColor = Color.White; }
            else
            {
                checkBox3.BackColor = Color.DarkGray;
                if (checkBox2.Checked == false && checkBox5.Checked == false)
                { textBox4.BackColor = Color.LightGray; }
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e) //OK
        {
            if (checkBox6.Checked == true)
            { checkBox6.BackColor = Color.Green; checkBox5.Checked = false; }
            else { checkBox6.BackColor = Color.DarkGray; }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e) //NOK
        {
            if (checkBox5.Checked == true)
            { checkBox5.BackColor = Color.Red; checkBox6.Checked = false; textBox4.BackColor = Color.White; }
            else
            {
                checkBox5.BackColor = Color.DarkGray;
                if (checkBox2.Checked == false && checkBox3.Checked == false)
                { textBox4.BackColor = Color.LightGray; }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text =="")
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Naskenujte datamatrix kód PCB dosky pomocou skenera");
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Vyberte stranu PCB dosky");
                comboBox1.Focus();
                return;
            }
            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Zadajte výsledok výšky lisovania - 1,1 mm");
                return;
            }
            if (checkBox3.Checked == false && checkBox4.Checked == false)
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Zadajte výsledok výšky lisovania - 1,3 mm");
                return;
            }
            if (checkBox5.Checked == false && checkBox6.Checked == false)
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Zadajte výsledok vizuálnej kontroly");
                return;
            }
            if (checkBox2.Checked == true || checkBox3.Checked == true || checkBox5.Checked == true)
            {
                if (textBox4.Text == "")
                {
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "Zadajte popis NOK výsledku");
                    textBox4.Focus();
                    return;
                }
            }
            if (textBox3.Text == "")
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Priložte svoju kartu k čítačke");
                textBox3.Focus();
                return;
            }

            if (DateTime.Now >= Convert.ToDateTime("06:00:00") && DateTime.Now < Convert.ToDateTime("14:00:00")) { Zmena = "Ranná"; }
            else if (DateTime.Now >= Convert.ToDateTime("14:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00")){ Zmena = "Poobedná"; }
            else if (DateTime.Now >= Convert.ToDateTime("22:00:00") || DateTime.Now < Convert.ToDateTime("06:00:00")) { Zmena = "Nočná"; }

            if (checkBox1.Checked == true) { PressHeight1_1Result = "OK"; } else { PressHeight1_1Result = "NOK"; }
            if (checkBox4.Checked == true) { PressHeight1_3Result = "OK"; } else { PressHeight1_3Result = "NOK"; }
            if (checkBox6.Checked == true) { VizualInspectionResult = "OK"; } else { VizualInspectionResult = "NOK"; }

            try
            {
                using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
                {
                    Conn.Open();
                    string NewRecord = "insert into PCBproductionRelease (Date,Hour,Shift,DM,InputPCBname,InputPCBcode,Batch,SN,PCBside,PressHeight1_1Result,PressHeight1_3Result,VizualInspectionResult,Notes,InspectionDoneBy)values('" + DateTime.Now.ToString("yyyy - MM - dd HH: mm: ss") + "','" + DateTime.Now.ToString("HH:mm") + "','" + Zmena + "','" + textBox1.Text + "','" + InputPCBname + "','" + InputPCBcode + "','" + Batch + "','" + SN + "','" + comboBox1.SelectedItem + "','" + PressHeight1_1Result + "','" + PressHeight1_3Result + "','" + VizualInspectionResult + "','" + textBox4.Text + "','" + textBox3.Text + "')";
                    SqlCommand cmd = new SqlCommand(NewRecord, Conn);
                    cmd.ExecuteNonQuery();
                    Conn.Close();

                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "Zápis uvoľnenia výroby bol zaznamenaný");
                    serialPort1.Close();
                    serialPort2.Close();
                    _1 nextForm = new _1(); Hide();
                    nextForm.ShowDialog();Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Pripojenie k serveru zlyhalo, zápis nebol zaznamenaný!");
                return;
            }
        }
    }
}
