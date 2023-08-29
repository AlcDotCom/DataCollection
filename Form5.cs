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
using System.IO.Ports;
using Downtime_registration.Properties;

namespace Downtime_registration
{
    public partial class Form5 : Form
    {
        public static int RowIndex = 0;
        public Form5()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN
            { WindowState = FormWindowState.Normal; }
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            try
            { PopulateDataGridView();}
            catch (Exception) 
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Pripojenie k serveru zlyhalo, nie je možné pokračovať");
            }
        }
        void PopulateDataGridView()
            {
                using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT TOP 100 * FROM DowntimeTable WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Status = 'Aktivny' order by DatumZapisu desc", sqlCon);   
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
                }
            }
        private void button2_Click(object sender, EventArgs e)
        {
            Form1 nextForm = new Form1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                RowIndex = dataGridView1.CurrentCell.RowIndex;
                using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                {
                    sqlCon.Open();
                    DataGridViewRow dgvRow = dataGridView1.CurrentRow;
                    SqlCommand sqlCmd = new SqlCommand("DowntimeTableAddOrEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@ID", dgvRow.Cells["txtID"].Value == DBNull.Value ? "" : dgvRow.Cells["txtID"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Prestoj", dgvRow.Cells["txtPrestoj"].Value == DBNull.Value ? "" : dgvRow.Cells["txtPrestoj"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@KategoriaPrestoja", dgvRow.Cells["txtKategoriaPrestoja"].Value == DBNull.Value ? "" : dgvRow.Cells["txtKategoriaPrestoja"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Linka", dgvRow.Cells["txtLinka"].Value == DBNull.Value ? "" : dgvRow.Cells["txtLinka"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DatumZapisu", dgvRow.Cells["txtDatumZapisu"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDatumZapisu"].Value);
                    sqlCmd.Parameters.AddWithValue("@ZaciatokPrestoja", dgvRow.Cells["txtZaciatokPrestoja"].Value == DBNull.Value ? "" : dgvRow.Cells["txtZaciatokPrestoja"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@KoniecPrestoja", dgvRow.Cells["txtKoniecPrestoja"].Value == DBNull.Value ? "" : dgvRow.Cells["txtKoniecPrestoja"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@TrvaniePrestoja", dgvRow.Cells["txtTrvaniePrestoja"].Value == DBNull.Value ? "" : dgvRow.Cells["txtTrvaniePrestoja"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Zmena", dgvRow.Cells["txtZmena"].Value == DBNull.Value ? "" : dgvRow.Cells["txtZmena"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@HotovyVyrobok", dgvRow.Cells["txtHotovyVyrobok"].Value == DBNull.Value ? "" : dgvRow.Cells["txtHotovyVyrobok"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@VstupnyMaterial", dgvRow.Cells["txtVstupnyMaterial"].Value == DBNull.Value ? "" : dgvRow.Cells["txtVstupnyMaterial"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Stanica", dgvRow.Cells["txtStanica"].Value == DBNull.Value ? "" : dgvRow.Cells["txtStanica"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Detail", dgvRow.Cells["txtDetail"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetail"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@ZastavenieVyroby", dgvRow.Cells["txtZastavenieVyroby"].Value == DBNull.Value ? "" : dgvRow.Cells["txtZastavenieVyroby"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@PoruchuOdstranil", dgvRow.Cells["txtPoruchuOdstranil"].Value == DBNull.Value ? "" : dgvRow.Cells["txtPoruchuOdstranil"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@PricinaAleboPopisPoruchy", dgvRow.Cells["txtPricinaAleboPopisPoruchy"].Value == DBNull.Value ? "" : dgvRow.Cells["txtPricinaAleboPopisPoruchy"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@VykonaneOpatrenieOprava", dgvRow.Cells["txtVykonaneOpatrenieOprava"].Value == DBNull.Value ? "" : dgvRow.Cells["txtVykonaneOpatrenieOprava"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@StrataOperator", dgvRow.Cells["txtStrataOperator"].Value == DBNull.Value ? "" : dgvRow.Cells["txtStrataOperator"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@CenaOperator1hod", dgvRow.Cells["txtCenaOperator1hod"].Value == DBNull.Value ? "" : dgvRow.Cells["txtCenaOperator1hod"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Status", dgvRow.Cells["txtStatus"].Value == DBNull.Value ? "" : dgvRow.Cells["txtStatus"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Rezerva1", dgvRow.Cells["txtRezerva1"].Value == DBNull.Value ? "" : dgvRow.Cells["txtRezerva1"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Rezerva2", dgvRow.Cells["txtRezerva2"].Value == DBNull.Value ? "" : dgvRow.Cells["txtRezerva2"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Rezerva3", dgvRow.Cells["txtRezerva3"].Value == DBNull.Value ? "" : dgvRow.Cells["txtRezerva3"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Rezerva4", dgvRow.Cells["txtRezerva4"].Value == DBNull.Value ? "" : dgvRow.Cells["txtRezerva4"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Rezerva5", dgvRow.Cells["txtRezerva5"].Value == DBNull.Value ? "" : dgvRow.Cells["txtRezerva5"].Value.ToString());
                    sqlCmd.ExecuteNonQuery();
                    PopulateDataGridView();
                    dataGridView1.CurrentCell = dataGridView1[5, Convert.ToInt32(RowIndex)];
                }
            }
        }
        private void dataGridView1_UserDeletingRow_1(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["txtID"].Value != DBNull.Value)
            {
                if (MessageBox.Show("Vymazať označený prestoj?", "VÝMAZ PRESTOJA", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    try
                    {
                        using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
                        {   
                            //Osobne cislo
                            SqlCommand Comm1 = new SqlCommand("SELECT id_doch FROM cardsX WHERE card_id LIKE '%" + Settings.Default.vymazavac.ToString() + "%'", Conn);
                            Conn.Open();
                            string osobnecislo = Comm1.ExecuteScalar().ToString();
                            String OScislo = osobnecislo.TrimStart(new Char[] { '0' });

                            //Meno
                            SqlCommand Comm2 = new SqlCommand("SELECT id_doch FROM cardsX WHERE card_id LIKE '%" + Settings.Default.vymazavac.ToString() + "%'", Conn);
                            string meno = Comm2.ExecuteScalar().ToString();

                            //Priezvisko
                            SqlCommand Comm3 = new SqlCommand("SELECT priez FROM cardsX WHERE card_id LIKE '%" + Settings.Default.vymazavac.ToString() + "%'", Conn);
                            string priezvisko = Comm3.ExecuteScalar().ToString();

                            string IDclovek = meno + " " + priezvisko + " " + OScislo;
                            Settings.Default.vymazavac = IDclovek;
                            Settings.Default.Save();

                            Conn.Close();
                        }
                    }
                    catch (Exception)   //ak nenájde HEX, vymazáva cez heslo 
                    {
                        Settings.Default.vymazavac = "cez heslo";
                        Settings.Default.Save();
                    }

                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        SqlCommand sqlCmd = new SqlCommand("DowntimeTableDeleteById", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridView1.CurrentRow.Cells["txtID"].Value));

                        sqlCmd.Parameters.AddWithValue("@Rezerva1", Settings.Default.vymazavac.ToString());
                        sqlCmd.Parameters.AddWithValue("@Rezerva2", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        sqlCmd.ExecuteNonQuery();
                    }
                }
                else
                    e.Cancel = true;
            }
            else
                e.Cancel = true;
        }
    }
}
