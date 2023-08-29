using System;
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
using Downtime_registration.Properties;
using System.IO.Ports;
using System.Management;

namespace Downtime_registration
{
    public partial class Form2 : Form
    {
        private string DispString;
        private string COMPORT;
        private string IDclovek;
        SerialPort SP;
        public Form2()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN zariadenie
            { 
                WindowState = FormWindowState.Normal;
                comboBox1.Font = new Font(comboBox1.Font.FontFamily, 12);
            }

            label8.Visible = false;  //koment na priloženie karty riešiteľa pre zaznamenanie reakčného času - skryté
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

            comboBox4.Items.Clear();
            try   //načita výrobky               
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT HotovyVyrobok FROM DowntimeOptions WHERE Linka = '" + Settings.Default.NazovZariadenia + "'", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        comboBox4.Items.Add(sqlReader["HotovyVyrobok"].ToString());
                    }
                    sqlReader.Close();
                }
                comboBox4.SelectedIndex = 0;
                object[] distinctItems = (from object o in comboBox4.Items select o).Distinct().ToArray(); 
                comboBox4.Items.Clear();
                comboBox4.Items.AddRange(distinctItems);

                if (comboBox4.Items.Count > 2)
                { comboBox4.SelectedIndex = -1; }
                else
                { comboBox4.SelectedIndex = 0; }
            }
            catch (Exception)
            {}

            try   //načita stanice
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT Stanica FROM DowntimeOptions WHERE Linka = '" + Settings.Default.NazovZariadenia + "'", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        comboBox1.Items.Add(sqlReader["Stanica"].ToString());
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            {}

            int ii = 0;
            while (comboBox1.Items.Count - 1 >= ii)
            {
                if (Convert.ToString(comboBox1.Items[ii]).Trim() == string.Empty)
                {
                    comboBox1.Items.RemoveAt(ii);
                    ii -= 1;
                }
                ii += 1;
            }

            int i = 0;
            while (comboBox4.Items.Count - 1 >= i)
            {
                if (Convert.ToString(comboBox4.Items[i]).Trim() == string.Empty)
                {
                    comboBox4.Items.RemoveAt(i);
                    i -= 1;
                }
                i += 1;
            }

            comboBox1.Select();

            textBox1.Text = DateTime.Now.ToString("HH:mm");
            ///////////////////////////////////////// rozdiel voči ostatným zápisom prestoja
            Settings.Default.PociatokReakcnehoCasu = Convert.ToDateTime(textBox1.Text).ToString(); //Pociatočný čas, od kedy sa ráta reakčný čas = otvorenie okna pre zadanie prestoja
            Settings.Default.Save();
            /////////////////////////////////////////
        }
        ///////////////////////////////////////// rozdiel voči ostatným zápisom prestoja
        private void button1_Click(object sender, EventArgs e) 
        {
            if (MessageBox.Show("Vrátiť späť?", "ZAPÍSANÉ DÁTA SA STRATIA", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                serialPort1.Close();
                Form1 nextForm = new Form1();
                Hide();
                nextForm.ShowDialog();
                Close();
            }
        }
        /////////////////////////////////////////

        private void cbxDesign_DrawItem(object sender, DrawItemEventArgs e)     ///formátovanie comboboxov - zarovnanie na stred
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

        private void button2_Click(object sender, EventArgs e)  ///Zápis prestoja
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
                MessageBox.Show("Prosím vyberte stanicu");
                comboBox1.Focus();
            }
            else if (comboBox2.Text.Length == 0)
            {
                MessageBox.Show("Prosím vyberte detail stanice");
                comboBox2.Focus();
            }
            else if (comboBox4.Text.Length == 0)
            {
                MessageBox.Show("Prosím vyberte výrobok");
                comboBox4.Focus();
            }
            else if (comboBox5.Text.Length == 0)
            {
                MessageBox.Show("Prosím vyberte možnosť, či operátori čakali na linke počas prestoja");
                comboBox5.Focus();
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
                            ///////////////////////////////////////// rozdiel voči ostatným zápisom prestoja
                            var PrichodRiesitela = Convert.ToDateTime(textBox7.Text);
                            var ReakcnyCas = (PrichodRiesitela - Convert.ToDateTime(Settings.Default.PociatokReakcnehoCasu));  //zapise reakcny cas do Rezerva3 + meno kto spravil check-in do Rezerva4
                            /////////////////////////////////////////
                            if (ReakcnyCas > trvanie)
                            { ReakcnyCas = new TimeSpan(00, 00, 00); }  //ak bude reakčný čas vyšší ako celkové trvanie prestoja, reakčný čas bude 0 (čistý čas riešenia prestoja vychádzal do mínusu)

                            string q = "insert into DowntimeTable (Prestoj,KategoriaPrestoja,Linka,DatumZapisu,ZaciatokPrestoja,KoniecPrestoja,TrvaniePrestoja,Zmena,HotovyVyrobok,VstupnyMaterial,Stanica,Detail,ZastavenieVyroby,PoruchuOdstranil,PricinaAleboPopisPoruchy,VykonaneOpatrenieOprava,StrataOperator,CenaOperator1hod,Status,Rezerva1,Rezerva2,Rezerva3,Rezerva4,Rezerva5)values('" + "Prestoj" + "','" + "Porucha" + "','" + Settings.Default.NazovZariadenia + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + textBox1.Text + "','" + textBox2.Text + "','" + trvanie.ToString() + "','" + zmena.ToString() + "','" + comboBox4.SelectedItem.ToString() + "','" + "N/A" + "','" + comboBox1.SelectedItem.ToString() + "','" + comboBox2.SelectedItem.ToString() + "','" + comboBox5.SelectedItem.ToString() + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + strata.ToString() + "','" + Cena1hod.ToString() + "','" + "Aktivny" + "','" + "N/A" + "','" + "N/A" + "','" + ReakcnyCas.ToString() + "','" + textBox8.Text + "','" + "N/A" + "')";

                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;

                            var w = new Form(); Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith((t) => w.Close(),
                            TaskScheduler.FromCurrentSynchronizationContext());
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            //Načíta detail staníc 1-10 do combobox2 podľa vybranej stanice v combobox1

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT DetailStanice1,DetailStanice2,DetailStanice3,DetailStanice4,DetailStanice5,DetailStanice6,DetailStanice7,DetailStanice8,DetailStanice9,DetailStanice10,DetailStanice11,DetailStanice12,DetailStanice13,DetailStanice14,DetailStanice15,DetailStanice16,DetailStanice17,DetailStanice18,DetailStanice19,DetailStanice20 FROM DowntimeOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Stanica = '" + comboBox1.SelectedItem.ToString() + "'", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        comboBox2.Items.Add(sqlReader["DetailStanice1"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice2"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice3"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice4"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice5"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice6"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice7"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice8"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice9"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice10"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice11"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice12"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice13"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice14"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice15"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice16"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice17"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice18"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice19"].ToString());
                        comboBox2.Items.Add(sqlReader["DetailStanice20"].ToString());
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),
                TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Spojenie so serverom zlyhalo");
            }
            comboBox2.Items.Add("Iné");

            int iii = 0;   //Vyčistí prázdne
            while (comboBox2.Items.Count - 1 >= iii)
            {
                if (Convert.ToString(comboBox2.Items[iii]).Trim() == string.Empty)
                {
                    comboBox2.Items.RemoveAt(iii);
                    iii -= 1;
                }
                iii += 1;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)   //vloží aktuálny čas po kliknutí do okna
        {
            textBox1.Text = DateTime.Now.ToString("HH:mm");
            textBox1.SelectAll();
        }

        private void textBox2_Enter(object sender, EventArgs e)   //info box
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 2000;
            ToolTip tt = new ToolTip();
            tt.Show("Zadajte čas v tvare: '0815' bez dvojbodky!", TB, 0, 35, VisibleTime);
        }

        private void textBox2_Click(object sender, EventArgs e)   //vloží aktuálny čas po kliknutí do okna
        {
            textBox2.Text = DateTime.Now.ToString("HH:mm");
            textBox2.SelectAll();
        }

        private void textBox1_Enter(object sender, EventArgs e)   //info box
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 2000;
            ToolTip tt = new ToolTip();
            tt.Show("Zadajte čas v tvare: '0814' bez dvojbodky!", TB, 0, 35, VisibleTime);
        }

        private void Form2_Load(object sender, EventArgs e)
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
            textBox6.Text = DispString;     //HEX zo serial portu
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

                        ///////////////////////////////////////// rozdiel voči ostatným zápisom prestoja
                        if (string.IsNullOrEmpty(textBox7.Text))   
                        {
                            textBox7.Text = DateTime.Now.ToString("HH:mm");
                            textBox8.Text = IDclovek;
                            textBox8.BackColor = Color.Green;
                            label8.Visible = false;
                            button3.Visible = false;
                            tableLayoutPanel1.BackColor = Color.Green;
                            tableLayoutPanel3.Visible = true;
                            textBox7.BackColor = Color.Green;
                            
                            //Reakčný čas - info správa
                            var ZaciatokPrestoja = Convert.ToDateTime(Settings.Default.PociatokReakcnehoCasu);
                            var PrichodRiesitela = Convert.ToDateTime(textBox7.Text);
                            var ReakcnyCas = (PrichodRiesitela - ZaciatokPrestoja);
                            var w = new Form() { Size = new Size(0, 0) };
                            Task.Delay(TimeSpan.FromSeconds(2))
                            .ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                            MessageBox.Show(w, "Váš reakčný čas je: " + ReakcnyCas.ToString());

                            textBox6.Text = "";
                        }
                        else
                        {
                            textBox3.Text = IDclovek;
                            textBox3.BackColor = Color.Green;
                            textBox6.Text = "";
                        }
                        /////////////////////////////////////////
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
        ///////////////////////////////////////// rozdiel voči ostatným zápisom prestoja
        private void button3_Click(object sender, EventArgs e) 
        {
            if (MessageBox.Show("Vrátiť späť?", "ZAPÍSANÉ DÁTA SA STRATIA", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {                             
                serialPort1.Close();
                Form1 nextForm = new Form1();
                Hide();
                nextForm.ShowDialog();
                Close();
            }
        }
        /////////////////////////////////////////
    }

}




