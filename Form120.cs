using Downtime_registration.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downtime_registration
{
    public partial class Form120 : Form
    {
        public Form120()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN zariadenie
            { WindowState = FormWindowState.Normal; }

            if (Settings.Default.nastavenieHV == "1")
            { Text = "Nastavenie hotových výrobkov"; }
            else if (Settings.Default.nastavenieJM == "1")
            { Text = "Nastavenie jednotlivých Materiálov"; }
            else if (Settings.Default.nastavenieST == "1")
            { Text = "Nastavenie staníc"; }
            else if (Settings.Default.nastavenieMAT == "1")
            { Text = "Nastavenie kategórií pre Materiál"; }
            else if (Settings.Default.nastaveniePROC == "1")
            { Text = "Nastavenie kategórií pre Proces"; }
            else if (Settings.Default.nastavenieNEK == "1")
            { Text = "Nastavenie kategórií pre Nekvalitu"; }
            else if (Settings.Default.nastavenieCENA == "1")
            { Text = "Nastavenie ceny 1 hod na zariadení"; }
            else if (Settings.Default.nastavenieCYKLUS == "1")
            { Text = "Nastavenie ideálneho času cyklu"; }
            else if (Settings.Default.nastavenieOP == "1")
            { Text = "Nastavenie operácii"; }
            else { Text = "Nastavenie"; }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
            Form12 nextForm = new Form12();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void Form120_Load(object sender, EventArgs e)
        {
            try
            { PopulateDataGridView(); }
            catch (Exception) { }
        }
        void PopulateDataGridView()
        {
            try
            {
                if (Settings.Default.nastavenieCYKLUS == "1")   //Ideálny čas cyklu
                {
                    dataGridView1.Visible = false;
                    dataGridView2.Visible = true;
                    dataGridView3.Visible = false;
                    label1.Visible = true;
                    label1.Text = "Ak sa na zariadení vyrába viacej výrobkov s tým istým cyklom, zadaj iba jeden pre všetky, ak ich je viac, zadaj ich jednotlivo pre každé PN bez lomena";
                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM CycleTime WHERE Linka = '" + Settings.Default.NazovZariadenia + "'", sqlCon);
                        DataTable dtbl = new DataTable();
                        sqlDa.Fill(dtbl);
                        dataGridView2.DataSource = dtbl;
                    }

                    //allowed to add more cycles as OEE need to be calculated separatedy based on particular product and its cycle
                    //if (dataGridView2.CurrentRow != null)  
                    //    dataGridView2.AllowUserToAddRows = false;
                }
                else if (Settings.Default.nastavenieOP == "1")
                {
                    dataGridView1.Visible = false;
                    dataGridView2.Visible = false;
                    dataGridView3.Visible = true;
                    label1.Visible = true;
                    label1.Text = "Zadajte názvy operácii, na ktorých pracujú operátori. Zadajte názov operácie aj v prípade, že na zariadení je len 1 operátor!";
                    label2.Visible = true;
                    label2.Text = "Operácii, ktorá je zodpovedná za 5S na konci zmeny, pridajte do názvu prívlastok ' + 5S'";
                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Operacie WHERE Linka = '" + Settings.Default.NazovZariadenia + "'", sqlCon);
                        DataTable dtbl = new DataTable();
                        sqlDa.Fill(dtbl);
                        dataGridView3.DataSource = dtbl;
                    }
                    if (dataGridView3.CurrentRow != null)
                        dataGridView3.AllowUserToAddRows = false;
                }
                else
                {
                    dataGridView1.Visible = true;
                    dataGridView2.Visible = false;
                    dataGridView3.Visible = false;
                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM DowntimeOptions", sqlCon);
                        DataTable dtbl = new DataTable();
                        sqlDa.Fill(dtbl);
                        dataGridView1.DataSource = dtbl;

                        if (Settings.Default.nastavenieHV == "1")  //Hotové výrobky
                        {
                            dataGridView1.Columns["txtHotovyVyrobok"].Visible = true;
                            dtbl.DefaultView.RowFilter = String.Format("HotovyVyrobok <> '{0}' AND Linka LIKE '{1}'", "", Settings.Default.NazovZariadenia.ToString());
                            label1.Visible = true;
                            label1.Text = "*Výrobky, ktoré sa najčastejšie vyrábajú vložte na začiatok";
                        }
                        else if (Settings.Default.nastavenieJM == "1")  //Jednotlivý materiál
                        {
                            dataGridView1.Columns["txtJednotlivyMaterial"].Visible = true;
                            dtbl.DefaultView.RowFilter = String.Format("JednotlivyMaterial <> '{0}' AND Linka LIKE '{1}'", "", Settings.Default.NazovZariadenia.ToString());
                        }
                        else if (Settings.Default.nastavenieST == "1")  //Stanice a deatily staníc
                        {
                            dataGridView1.Columns["txtStanica"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice1"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice2"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice3"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice4"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice5"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice6"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice7"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice8"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice9"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice10"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice11"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice12"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice13"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice14"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice15"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice16"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice17"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice18"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice19"].Visible = true;
                            dataGridView1.Columns["txtDetailStanice20"].Visible = true;
                            dtbl.DefaultView.RowFilter = String.Format("Stanica <> '{0}' AND Linka LIKE '{1}'", "", Settings.Default.NazovZariadenia.ToString());
                            label1.Visible = true;
                            label1.Text = "*Prvú stanicu pomenuj *Všeobecné* a pri zadávaní nových stanic vlož najprv názov stanice a až následne detail stanice!";
                        }
                        else if (Settings.Default.nastavenieMAT == "1")  //kategórie materiál
                        {
                            dataGridView1.Columns["txtKategorieMATERIAL"].Visible = true;
                            dtbl.DefaultView.RowFilter = String.Format("KategorieMATERIAL <> '{0}' AND Linka LIKE '{1}'", "", Settings.Default.NazovZariadenia.ToString());
                        }
                        else if (Settings.Default.nastaveniePROC == "1")  //kategórie procesu
                        {
                            dataGridView1.Columns["txtKategoriePROCES"].Visible = true;
                            dtbl.DefaultView.RowFilter = String.Format("KategoriePROCES <> '{0}' AND Linka LIKE '{1}'", "", Settings.Default.NazovZariadenia.ToString());
                        }
                        else if (Settings.Default.nastavenieNEK == "1")  //kategórie nekvality
                        {
                            dataGridView1.Columns["txtKategorieNEKVALITA"].Visible = true;
                            dtbl.DefaultView.RowFilter = String.Format("KategorieNEKVALITA <> '{0}' AND Linka LIKE '{1}'", "", Settings.Default.NazovZariadenia.ToString());
                        }
                        else if (Settings.Default.nastavenieCENA == "1")  //vstupná cena 1 hod pre výpočet ceny prestojov
                        {
                            dataGridView1.Columns["txtCena1hod"].Visible = true;
                            dtbl.DefaultView.RowFilter = String.Format("Cena1hod <> '{0}' AND Linka LIKE '{1}'", "", Settings.Default.NazovZariadenia.ToString());
                            label1.Visible = true;
                            label1.Text = "*Cena môže byť len jedna a v jednom poli! Zadávaj cenu 1 hodiny v eurách X počet operátorov na tejto linke / zariadení";
                            if (dataGridView1.CurrentRow != null)
                                dataGridView1.AllowUserToAddRows = false;
                        }
                        else
                        {}
                    }
                }
            }
            catch (Exception)
            {}
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                {
                    sqlCon.Open();
                    DataGridViewRow dgvRow = dataGridView1.CurrentRow;
                    SqlCommand sqlCmd = new SqlCommand("OptionsTableAddOrEdit", sqlCon); 
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@ID", dgvRow.Cells["txtID"].Value == DBNull.Value ? "" : dgvRow.Cells["txtID"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Linka", Settings.Default.NazovZariadenia.ToString());   //pri novom zázname vloží aktuálnu linku do stĺpca Linka v sql
                    sqlCmd.Parameters.AddWithValue("@Stanica", dgvRow.Cells["txtStanica"].Value == DBNull.Value ? "" : dgvRow.Cells["txtStanica"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice1", dgvRow.Cells["txtDetailStanice1"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice1"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice2", dgvRow.Cells["txtDetailStanice2"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice2"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice3", dgvRow.Cells["txtDetailStanice3"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice3"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice4", dgvRow.Cells["txtDetailStanice4"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice4"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice5", dgvRow.Cells["txtDetailStanice5"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice5"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice6", dgvRow.Cells["txtDetailStanice6"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice6"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice7", dgvRow.Cells["txtDetailStanice7"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice7"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice8", dgvRow.Cells["txtDetailStanice8"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice8"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice9", dgvRow.Cells["txtDetailStanice9"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice9"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice10", dgvRow.Cells["txtDetailStanice10"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice10"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@KategoriePROCES", dgvRow.Cells["txtKategoriePROCES"].Value == DBNull.Value ? "" : dgvRow.Cells["txtKategoriePROCES"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@KategorieNEKVALITA", dgvRow.Cells["txtKategorieNEKVALITA"].Value == DBNull.Value ? "" : dgvRow.Cells["txtKategorieNEKVALITA"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@KategorieMATERIAL", dgvRow.Cells["txtKategorieMATERIAL"].Value == DBNull.Value ? "" : dgvRow.Cells["txtKategorieMATERIAL"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@HotovyVyrobok", dgvRow.Cells["txtHotovyVyrobok"].Value == DBNull.Value ? "" : dgvRow.Cells["txtHotovyVyrobok"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@JednotlivyMaterial", dgvRow.Cells["txtJednotlivyMaterial"].Value == DBNull.Value ? "" : dgvRow.Cells["txtJednotlivyMaterial"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@Cena1hod", dgvRow.Cells["txtCena1hod"].Value == DBNull.Value ? "" : dgvRow.Cells["txtCena1hod"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@rezerva1", dgvRow.Cells["txtrezerva1"].Value == DBNull.Value ? "" : dgvRow.Cells["txtrezerva1"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@rezerva2", dgvRow.Cells["txtrezerva2"].Value == DBNull.Value ? "" : dgvRow.Cells["txtrezerva2"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@rezerva3", dgvRow.Cells["txtrezerva3"].Value == DBNull.Value ? "" : dgvRow.Cells["txtrezerva3"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice11", dgvRow.Cells["txtDetailStanice11"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice11"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice12", dgvRow.Cells["txtDetailStanice12"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice12"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice13", dgvRow.Cells["txtDetailStanice13"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice13"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice14", dgvRow.Cells["txtDetailStanice14"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice14"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice15", dgvRow.Cells["txtDetailStanice15"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice15"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice16", dgvRow.Cells["txtDetailStanice16"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice16"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice17", dgvRow.Cells["txtDetailStanice17"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice17"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice18", dgvRow.Cells["txtDetailStanice18"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice18"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice19", dgvRow.Cells["txtDetailStanice19"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice19"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@DetailStanice20", dgvRow.Cells["txtDetailStanice20"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDetailStanice20"].Value.ToString());

                    sqlCmd.ExecuteNonQuery();
                    PopulateDataGridView();
                }
            }
        }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                {
                    try
                    {
                        sqlCon.Open();
                        DataGridViewRow dgvRow = dataGridView2.CurrentRow;
                        SqlCommand sqlCmd = new SqlCommand("CycleTimeAddOrEdit", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ID", dgvRow.Cells["txID"].Value == DBNull.Value ? "" : dgvRow.Cells["txID"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Linka", Settings.Default.NazovZariadenia.ToString());   //pri novom zázname vloží aktuálnu linku do stĺpca Linka v sql
                        sqlCmd.Parameters.AddWithValue("@Vyrobok", dgvRow.Cells["txVyrobok"].Value == DBNull.Value ? "" : dgvRow.Cells["txVyrobok"].Value.ToString());
                        if (dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[3].Value.ToString() == "")
                        {
                            sqlCmd.Parameters.AddWithValue("@CasCyklu", "0");
                        }
                        else
                        {
                            sqlCmd.Parameters.AddWithValue("@CasCyklu", Convert.ToDouble(dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[3].Value, CultureInfo.InvariantCulture)); 
                        }
                        sqlCmd.Parameters.AddWithValue("@Rezerva1", dgvRow.Cells["txRezerva1"].Value == DBNull.Value ? "" : dgvRow.Cells["txRezerva1"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Rezerva2", dgvRow.Cells["txRezerva2"].Value == DBNull.Value ? "" : dgvRow.Cells["txRezerva2"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Rezerva3", dgvRow.Cells["txRezerva3"].Value == DBNull.Value ? "" : dgvRow.Cells["txRezerva3"].Value.ToString());
                        sqlCmd.ExecuteNonQuery();
                        PopulateDataGridView();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Nesprávny formát");
                        return;
                    }
                }
            }
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView3.CurrentRow != null)
            {
                using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                {
                    try
                    {
                        sqlCon.Open();
                        DataGridViewRow dgvRow = dataGridView3.CurrentRow;
                        SqlCommand sqlCmd = new SqlCommand("OperacieAddOrEdit", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ID", dgvRow.Cells["opID"].Value == DBNull.Value ? "" : dgvRow.Cells["opID"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Linka", Settings.Default.NazovZariadenia.ToString());   //pri novom zázname vloží aktuálnu linku
                        sqlCmd.Parameters.AddWithValue("@OP1", dgvRow.Cells["opOP1"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP1"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP2", dgvRow.Cells["opOP2"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP2"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP3", dgvRow.Cells["opOP3"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP3"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP4", dgvRow.Cells["opOP4"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP4"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP5", dgvRow.Cells["opOP5"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP5"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP6", dgvRow.Cells["opOP6"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP6"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP7", dgvRow.Cells["opOP7"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP7"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP8", dgvRow.Cells["opOP8"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP8"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP9", dgvRow.Cells["opOP9"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP9"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP10", dgvRow.Cells["opOP10"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP10"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP11", dgvRow.Cells["opOP11"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP11"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@OP12", dgvRow.Cells["opOP12"].Value == DBNull.Value ? "" : dgvRow.Cells["opOP12"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@PridalUpravil", Environment.MachineName + " & " + Environment.UserName);
                        sqlCmd.Parameters.AddWithValue("@DatumZmeny", DateTime.Now);
                        sqlCmd.Parameters.AddWithValue("@Rezerva1", dgvRow.Cells["opRezerva1"].Value == DBNull.Value ? "" : dgvRow.Cells["opRezerva1"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Rezerva2", dgvRow.Cells["opRezerva2"].Value == DBNull.Value ? "" : dgvRow.Cells["opRezerva2"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Rezerva3", dgvRow.Cells["opRezerva3"].Value == DBNull.Value ? "" : dgvRow.Cells["opRezerva3"].Value.ToString());
                        sqlCmd.ExecuteNonQuery();
                        PopulateDataGridView();
                    }
                    catch (Exception)
                    {}
                }
            }
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs vymaz)
        {
            if (dataGridView1.CurrentRow.Cells["txtID"].Value != DBNull.Value)
            {
                if (MessageBox.Show("Vymazať označený riadok?", "VÝMAZ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        SqlCommand sqlCmd = new SqlCommand("OptionsTableDeleteById", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridView1.CurrentRow.Cells["txtID"].Value));
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                else
                    vymaz.Cancel = true;
            }
            else
                vymaz.Cancel = true;
        }

        private void dataGridView2_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs vymaz)
        {
            if (dataGridView2.CurrentRow.Cells["txID"].Value != DBNull.Value)
            {
                if (MessageBox.Show("Vymazať označený riadok?", "VÝMAZ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        SqlCommand sqlCmd = new SqlCommand("CycleTimeDelete", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridView2.CurrentRow.Cells["txID"].Value));
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                else
                    vymaz.Cancel = true;
            }
            else
                vymaz.Cancel = true;
        }
    }
}
