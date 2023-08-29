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
using Downtime_registration.Properties;
using System.Configuration;
using System.Data.OleDb;
using System.Globalization;

namespace Downtime_registration
{
    public partial class Form3 : Form
    {
        private string Zmena;
        public Form3()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN zariadenie
            { WindowState = FormWindowState.Normal; }


            try
            {
                using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                {
                    sqlCon.Open();
                    string DT = DateTime.Now.Subtract(TimeSpan.FromHours(720)).ToString();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT TOP 100 * FROM DowntimeTable WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Status = 'Aktivny'" + "order by DatumZapisu desc", sqlCon);
                    // AND DatumZapisu >  '" + DateTime.Now.Subtract(TimeSpan.FromDays(30)).ToString("yyyy-MM-dd HH:mm:ss") + "'  
                    DataTable dtbl = new DataTable();
                    sqlDa.Fill(dtbl);
                    dataGridView2.DataSource = dtbl;

                    dataGridView2.Columns["Prestoj"].Visible = false;
                    dataGridView2.Columns["HotovyVyrobok"].Visible = false;
                    dataGridView2.Columns["VstupnyMaterial"].Visible = false;      
                    dataGridView2.Columns["ZastavenieVyroby"].Visible = false;
                    dataGridView2.Columns["StrataOperator"].Visible = false;
                    dataGridView2.Columns["CenaOperator1hod"].Visible = false; 
                    dataGridView2.Columns["Rezerva1"].Visible = false;
                    dataGridView2.Columns["Rezerva2"].Visible = false;
                    dataGridView2.Columns["Rezerva3"].Visible = false;
                    dataGridView2.Columns["Rezerva4"].Visible = false;
                    dataGridView2.Columns["Rezerva5"].Visible = false;

                    dataGridView2.Columns["Status"].Visible = false;  //schovať stlpec status po odfiltrovaní len aktívnych
                    dataGridView2.Sort(dataGridView2.Columns[4], ListSortDirection.Descending);   //zoraď prestoje od najnovších
                    dataGridView2.Columns["ID"].Visible = false;

                    //Layout prehladu
                    dataGridView2.RowTemplate.MinimumHeight = 25;
                    DataGridViewColumn column1 = dataGridView2.Columns[3];  //Linka
                    column1.Width = 120;
                    DataGridViewColumn column2 = dataGridView2.Columns[4];  //Datum
                    column2.Width = 200;
                    DataGridViewColumn column3 = dataGridView2.Columns[5];  //Zaciatok prestoja
                    column3.Width = 160;
                    DataGridViewColumn column4 = dataGridView2.Columns[6];  //Koniec prestoja
                    column4.Width = 160;
                    DataGridViewColumn column5 = dataGridView2.Columns[7];  //Trvanie
                    column5.Width = 150;
                    DataGridViewColumn column6 = dataGridView2.Columns[12];  //Detail
                    column6.Width = 250;
                    DataGridViewColumn column7 = dataGridView2.Columns[14];  //Riešiteľ
                    column7.Width = 200;
                    DataGridViewColumn column8 = dataGridView2.Columns[15];  //Popis poruchy
                    column8.Width = 850;
                    DataGridViewColumn column9 = dataGridView2.Columns[16];  //Opatrenie
                    column9.Width = 850;

                    //Názvy
                    dataGridView2.Columns[2].HeaderText = "Kategória";
                    dataGridView2.Columns[4].HeaderText = "Dátum zápisu";
                    dataGridView2.Columns[5].HeaderText = "Začiatok prestoja";
                    dataGridView2.Columns[6].HeaderText = "Koniec prestoja";
                    dataGridView2.Columns[7].HeaderText = "Trvanie";
                    dataGridView2.Columns[12].HeaderText = "Detail prestoja";
                    dataGridView2.Columns[14].HeaderText = "Riešiteľ prestoja";
                    dataGridView2.Columns[15].HeaderText = "Príčina / popis prestoja";
                    dataGridView2.Columns[16].HeaderText = "Nápravné opatrenie / akcia";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné načítať dáta z SQL databázy!");
                return;
            }

            try     //PROGRESS BAR
            {
                {
                    if (DateTime.Now >= Convert.ToDateTime("06:00:00") && DateTime.Now < Convert.ToDateTime("14:00:00")) { Zmena = "Ranná"; }
                    else if (DateTime.Now >= Convert.ToDateTime("14:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00")) { Zmena = "Poobedná"; }
                    else if (DateTime.Now >= Convert.ToDateTime("22:00:00") || DateTime.Now < Convert.ToDateTime("06:00:00")) { Zmena = "Nočná"; }

                    DateTime today = DateTime.Now.Subtract(TimeSpan.FromHours(9));  //posledných 9 hod pre filtrovanie konkrétnej zmeny z SQL
                    DateTime today1 = DateTime.Now;

                    using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
                    {
                    SqlCommand Comm1 = new SqlCommand("SELECT DATEADD(ms, SUM(DATEDIFF(ms, '00:00:00.000', convert(datetime, TrvaniePrestoja))), '00:00:00.000') as time FROM " + "DowntimeTable" + " WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Zmena = '" + Zmena + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Status = '" + "Aktivny" + "'", Conn);
                    Conn.Open();
                    string hours = Comm1.ExecuteScalar().ToString();
                    DateTime hours1 = DateTime.Parse(hours);
                    double hours2 = ((double)hours1.Ticks - 599266080000000000) / 36000000000;
                    Conn.Close();
                    
                    //Výpočet Avaliability

                    int i = 450;   //zmena 450 min
                    double ee = 60;
                    double ii = (hours2 * ee);   //sumar prestojov na zmene v hod * 60, na minúty
                    double c = (ii / i);
                    double xx = ii / i * 100;
                    double percent = xx;
                    int i1 = Convert.ToInt32(percent);
                    int i2 = (int)percent;

                    if (i2 >= 100)   //ak je viac prestojov ako je dĺžka zmeny, avaliability bude 0%
                    {
                    progressBar1.Value = (0);
                    label1.Text = "Aktuálna dostupnosť linky/zariadenia (Avaliability) na tejto zmene je " + progressBar1.Value.ToString() + "%" + "   ((450 - počet minút prestojov na tejto zmene) / 450)";
                    }
                    else
                    progressBar1.Value = (100 - i2);   //výpočet avaliability
                    label1.Text = "Aktuálna dostupnosť linky/zariadenia (Avaliability) na tejto zmene je " + progressBar1.Value.ToString() + "%" + "   ((450 - počet minút prestojov na tejto zmene) / 450)";
                   }
                }
            }
            catch (Exception)
            {                                        
                progressBar1.Value = (100);   //ak nenájde prestoje na aktuálnej zmene, avaliability bude 100% 
                label1.Text = "Aktuálna dostupnosť linky/zariadenia (Avaliability) na tejto zmene je 100 %   (na tejto zmene neboli zapísané žiadne prestoje)";
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
            Form1 nextForm = new Form1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dispose();
            Form4 nextForm = new Form4();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (e.Value.ToString() == "Porucha")
                    e.CellStyle.BackColor = Color.Gold;
                if (e.Value.ToString() == "Nekvalita")
                    e.CellStyle.BackColor = Color.IndianRed;
                if (e.Value.ToString() == "Proces")
                    e.CellStyle.BackColor = Color.Gray;
                if (e.Value.ToString() == "Materiál")
                    e.CellStyle.BackColor = Color.BlueViolet;
            }
        }
        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView2.ClearSelection();
        }
        public new void Dispose()
        {
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
        }
    }
}
