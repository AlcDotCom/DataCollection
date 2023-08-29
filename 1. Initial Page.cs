using Downtime_registration.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downtime_registration
{
    public partial class _1 : Form
    {
        public static int InitialStart = 0;
        public _1()
        {
            InitializeComponent();
            try  //počiatočný ping serveru
            {
                using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
                    connection.Open();
            }
            catch (Exception) 
            {
                MessageBox.Show("Spojenie so serverom zlyhalo, overte pripojenie k sieti.");
            }



            Text = "eLSO - Úvod - " + Settings.Default.NazovZariadenia;
            label5.Text = Settings.Default.NazovZariadenia;
            Settings.Default.NOKPovolenieUpravit = "";  // pre vstup do zapisu NOK
            Settings.Default.Process = 1;
            Settings.Default.Save();

            if (Settings.Default.NazovZariadenia == "LPCB")  //viditeľný zápis len pre zariadenie PCB
            { label7.Visible = true; pictureBox6.Visible = true; }

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN
            {
                if (InitialStart == 0)
                { WindowState = FormWindowState.Minimized; InitialStart = 1; }
                else
                { WindowState = FormWindowState.Normal; }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dispose();
            Form1 nextForm = new Form1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        public new void Dispose()
        {
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
            pictureBox2.Image.Dispose();
            pictureBox2.Image = null;
            pictureBox3.Image.Dispose();
            pictureBox3.Image = null;
            pictureBox4.Image.Dispose();
            pictureBox4.Image = null;
            pictureBox5.Image.Dispose();
            pictureBox5.Image = null;
            pictureBox6.Image.Dispose();
            pictureBox6.Image = null;
            tableLayoutPanel1.Dispose();
            tableLayoutPanel1 = null;
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Dispose();
            IDforNOKreg nextForm = new IDforNOKreg();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Dispose();
            Form11 nextForm = new Form11();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Dispose();
            PCBmenu nextForm = new PCBmenu();
            Hide();
            nextForm.ShowDialog();
            Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)  //overí, či sú na danej linke zadefinované nejaké operácie na prihlásenie
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
                        string check = (sqlReader["OP1"].ToString() + sqlReader["OP2"].ToString() + sqlReader["OP3"].ToString() + sqlReader["OP4"].ToString() + sqlReader["OP5"].ToString() + sqlReader["OP6"].ToString() + sqlReader["OP7"].ToString() + sqlReader["OP8"].ToString() + sqlReader["OP9"].ToString() + sqlReader["OP10"].ToString() + sqlReader["OP11"].ToString() + sqlReader["OP12"].ToString());
                        if (check != "")
                        {
                            Dispose();
                            OPERATIONS nextForm = new OPERATIONS();
                            Hide();
                            nextForm.ShowDialog();
                            Close();
                        }
                        else
                        {
                            var w = new Form(); Task.Delay(TimeSpan.FromSeconds(1.2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                            MessageBox.Show(w, "Na tejto linke/zariadení nie sú definované žiadne operácie");
                        }
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(1.2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Na tejto linke/zariadení nie sú definované žiadne operácie");
            }
        }

    }
}
