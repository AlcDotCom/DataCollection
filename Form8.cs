﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO.Ports;
using Downtime_registration.Properties;
using System.Management;

namespace Downtime_registration
{
    public partial class Form8 : Form
    {
        private string DispString;
        private string COMPORT;
        private string IDclovek;
        SerialPort SP;
        public Form8()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN
            { WindowState = FormWindowState.Normal; }

            comboBox5.SelectedIndex = 0;
            {
                try  //počiatočný ping serveru
                {
                    using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
                        connection.Open();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, prestoj nie je možné zaregistrovať");
                    return;
                }
            }

            try   //vstupné materiály
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT JednotlivyMaterial FROM DowntimeOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "'", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        comboBox1.Items.Add(sqlReader["JednotlivyMaterial"].ToString());
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            {}
            comboBox1.Items.Add("Iné");

            try   //Detail / upresnenie
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT KategorieMATERIAL FROM DowntimeOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "'", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        comboBox2.Items.Add(sqlReader["KategorieMATERIAL"].ToString());
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            {}
            comboBox2.Items.Add("Iné");


            int i = 0;
            while (comboBox1.Items.Count - 1 >= i)
            {
                if (Convert.ToString(comboBox1.Items[i]).Trim() == string.Empty)
                {
                    comboBox1.Items.RemoveAt(i);

                    i -= 1;
                }
                i += 1;
            }

            int ii = 0;
            while (comboBox2.Items.Count - 1 >= ii)
            {
                if (Convert.ToString(comboBox2.Items[ii]).Trim() == string.Empty)
                {
                    comboBox2.Items.RemoveAt(ii);

                    ii -= 1;
                }
                ii += 1;
            }

            textBox1.Text = DateTime.Now.ToString("HH:mm");
            comboBox1.Select();
        }
        private void button1_Click(object sender, EventArgs e)   
        {
            serialPort1.Close();
            Form1 nextForm = new Form1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }

        private void cbxDesign_DrawItem(object sender, DrawItemEventArgs e)    ///formátovanie comboboxov - zarovnanie na stred
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
            textBox1.MaxLength = 5;
            if (textBox1.Text.Length == 2 && !textBox1.Text.Contains(":"))
                textBox1.Text += ":";
            textBox1.SelectionStart = textBox1.Text.Length;

            int count = textBox1.Text.Length - textBox1.Text.Replace(":", "").Length;
            if (count == 2)
                textBox1.Text = textBox1.Text.Remove(Convert.ToInt32(textBox1.Text.IndexOf(':')), 1);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.MaxLength = 5;
            if (textBox2.Text.Length == 2 && !textBox2.Text.Contains(":"))
                textBox2.Text += ":";
            textBox2.SelectionStart = textBox2.Text.Length;

            int count = textBox2.Text.Length - textBox2.Text.Replace(":", "").Length;
            if (count == 2)
                textBox2.Text = textBox2.Text.Remove(Convert.ToInt32(textBox2.Text.IndexOf(':')), 1);
        }
        private void button2_Click(object sender, EventArgs e)           
        {
            var zaciatokprestoja222 = DateTime.Now;
            DateTime t0 = Convert.ToDateTime("00:00:00");
            DateTime t1 = Convert.ToDateTime("06:00:00");
            DateTime t2 = Convert.ToDateTime("14:00:00");
            DateTime t3 = Convert.ToDateTime("22:00:00");
            string zmena;

            if (zaciatokprestoja222 >= t1 && zaciatokprestoja222 < t2)
            {
                zmena = "Ranná";
            }
            else if (zaciatokprestoja222 >= t2 && zaciatokprestoja222 < t3)
            {
                zmena = "Poobedná";
            }
            else if (zaciatokprestoja222 >= t3)
            {
                zmena = "Nočná";
            }
            else if (zaciatokprestoja222 < t1)
            {
                zmena = "Nočná";
            }
            else
            {
                zmena = "N/A";
            }
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Prosím zadajte začiatok prestoja");
                textBox1.Focus();
                return;
            }
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Prosím zadajte koniec prestoja");
                textBox2.Focus();
                return;
            }
            if (textBox1.Text.Length < 5)
            {
                MessageBox.Show("Prosím zadajte začiatok prestoja v správnom tvare HH:MM");
                textBox1.Focus();
                return;
            }
            if (textBox2.Text.Length < 5)
            {
                MessageBox.Show("Prosím zadajte koniec prestoja v správnom tvare HH:MM");
                textBox2.Focus();
                return;
            }
            try
            {
                var koniecprestoja1 = Convert.ToDateTime(textBox2.Text);
                var zaciatokprestoja1 = Convert.ToDateTime(textBox1.Text);
                var rozdiel11 = (koniecprestoja1 - zaciatokprestoja1);
            }
            catch (Exception)
            {
                MessageBox.Show("Prosím zadajte správny formát času HH:MM");
                return;
            }
            TimeSpan trvanie = new TimeSpan();
            var koniecprestoja = Convert.ToDateTime(textBox2.Text);
            var zaciatokprestoja = Convert.ToDateTime(textBox1.Text);
            var dopolnoci = (zaciatokprestoja - koniecprestoja);
            TimeSpan polnoc = new TimeSpan(24, 00, 00);
            if (zaciatokprestoja > koniecprestoja)
            {
                trvanie = (polnoc - dopolnoci);
            }
            else
            {
                trvanie = (koniecprestoja - zaciatokprestoja);
            }

            TimeSpan indikator = new TimeSpan(4, 00, 00);
            TimeSpan indikator2 = new TimeSpan(8, 00, 00);

            if (trvanie > indikator && trvanie < indikator2) //upozorní ak užívateľ zadá prísliš dlhý prestoj
            {
                DialogResult dialogResult = MessageBox.Show("POZOR: Zadali ste prestoj ktorý trval: " + trvanie.ToString() + " hodín, Pokračovať?", "OVERENIE ČASU PRESTOJA", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                { }
                else if (dialogResult == DialogResult.No)
                {
                return;
                }
            }

            if (trvanie > indikator2) //nie je možné zadať prestoj dlhší ako 8 hod
            {
                MessageBox.Show("POZOR: Zadali ste prestoj s trvaním: " + trvanie.ToString() + " hodín, prestoj dlhší ako 8 hodín, nie je možé registrovať");
                return;
            }

            //Zistí cenu hodiny z SQL pre túto linku / zariadenie
            Settings.Default.CenaZsql = "0";
            Settings.Default.Save();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT Cena1hod FROM DowntimeOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Cena1hod > '" + 1 + "'  ", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        Settings.Default.CenaZsql = (sqlReader["Cena1hod"].ToString());
                        Settings.Default.Save();
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Nebolo možné načítať cenu z SQL");
                return;
            }

            try  //Overí formát ceny v SQL
            {
                double OverenieFormatuCeny = Convert.ToDouble(Settings.Default.CenaZsql);
            }
            catch (Exception)
            {
                MessageBox.Show("Zadaná cena v SQL nie je v správnom formáte");
                return;
            }

            //Výpočet ceny prestoja v eurách
            double Cena1hod = Convert.ToDouble(Settings.Default.CenaZsql);
            double strata1 = (double)trvanie.Ticks / 36000000000;
            double strata = strata1 * Cena1hod;

            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Prosím zadajte začiatok prestoja");
                textBox1.Focus();
            }
            else if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Prosím zadajte koniec prestoja");
                textBox2.Focus();
            }
            else if (textBox3.Text.Length == 0)
            {
                MessageBox.Show("Prosím priložte svoju kartu k čítačke");
                textBox3.Focus();
            }
            else if (textBox4.Text.Length == 0)
            {
                MessageBox.Show("Prosím zadajte príčinu, alebo popis poruchy");
                textBox4.Focus();
            }
            else if (textBox5.Text.Length == 0)
            {
                MessageBox.Show("Prosím zadajte vykonané opatrenie / opravu");
                textBox5.Focus();
            }
            else if (comboBox1.Text.Length == 0)
            {
                MessageBox.Show("Prosím vyberte materiál");
                comboBox1.Focus();
            }
             else if (comboBox2.Text.Length == 0)
            {
                MessageBox.Show("Prosím vyberte detail / upresnenie");
                comboBox2.Focus();
            }
            else                                                                      
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        string NazovZariadenia = Settings.Default.NazovZariadenia;
                        DateTime time = DateTime.Now;
                        string format = "yyyy-MM-dd HH:mm:ss";

                        string q = "insert into DowntimeTable (Prestoj,KategoriaPrestoja,Linka,DatumZapisu,ZaciatokPrestoja,KoniecPrestoja,TrvaniePrestoja,Zmena,HotovyVyrobok,VstupnyMaterial,Stanica,Detail,ZastavenieVyroby,PoruchuOdstranil,PricinaAleboPopisPoruchy,VykonaneOpatrenieOprava,StrataOperator,CenaOperator1hod,Status,Rezerva1,Rezerva2,Rezerva3,Rezerva4,Rezerva5)values('" + "Prestoj" + "','" + "Materiál" + "','" + NazovZariadenia.ToString() + "','" + time.ToString(format) + "','" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + trvanie.ToString() + "','" + zmena.ToString() + "','" + "N/A" + "','" + comboBox1.SelectedItem.ToString() + "','" + "N/A" + "','" + comboBox2.SelectedItem.ToString() + "','" + comboBox5.SelectedItem.ToString() + "','" + textBox3.Text.ToString() + "','" + textBox4.Text.ToString() + "','" + textBox5.Text.ToString() + "','" + strata.ToString() + "','" + Cena1hod.ToString() + "','" + "Aktivny" + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";

                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery();
                        Cursor = Cursors.Default;
                        var w = new Form() { Size = new Size(0, 0) };
                        Task.Delay(TimeSpan.FromSeconds(1))
                        .ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                        MessageBox.Show(w, "Prestoj bol zaregistrovaný");
                    }
                    }
                }
                catch (Exception)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Spojenie so serverom zlyhalo, prestoj nebol zaregistrovaný!");
                    return;
                }

                serialPort1.Close();
                Form1 nextForm = new Form1();
                Hide();
                nextForm.ShowDialog();
                Close();
            }
        }                          
        private void textBox1_Leave(object sender, EventArgs e)
        {
            DateTime myDate;
            if (textBox1.Text.Length > 0)
            {
                if (DateTime.TryParse(textBox1.Text, out myDate))
                { }
                else
                {
                    MessageBox.Show("Neplatný formát času, zadajte čas v tvare HH:MM");
                    textBox1.Clear();
                }
            }
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            DateTime myDate;
            if (textBox2.Text.Length > 0)
            {
                if (DateTime.TryParse(textBox2.Text, out myDate))
                { }
                else
                {
                    MessageBox.Show("Neplatný formát času, zadajte čas v tvare HH:MM");
                    textBox2.Clear();
                }
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString("HH:mm");
            textBox1.SelectAll();
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 2000;

            ToolTip tt = new ToolTip();
            tt.Show("Zadajte cas v tvare: '0815' bez dvojbodky!", TB, 0, 35, VisibleTime);
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 2000;

            ToolTip tt = new ToolTip();
            tt.Show("Zadajte cas v tvare: '0814' bez dvojbodky!", TB, 0, 35, VisibleTime);
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.ToString("HH:mm");
            textBox2.SelectAll();
        }
        private void Form8_Load(object sender, EventArgs e)
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
                        textBox3.Text = "";
                    }
                }
                catch (Exception)
                {
                    serialPort1.Close();
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "RFID čítačka nie je pripojená, nie je možné zapísať prestoj!");
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
            textBox6.Text = DispString;   //HEX zo serial portu
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length >= 5)
            {
                try
                {
                    textBox3.Text = "";
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
                        textBox3.Text = IDclovek;   //MENO
                        textBox3.BackColor = Color.Green;
                    }
                }
                catch (Exception)
                {
                    var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                    TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(w, "Spojenie so serverom zlyhalo");
                }
            }
            else
            {
                textBox6.Text = "";
            }
        }
    }
}


