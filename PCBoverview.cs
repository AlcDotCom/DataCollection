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
    public partial class PCBoverview : Form
    {
        public PCBoverview()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _1 nextForm = new _1(); Hide();
            nextForm.ShowDialog();Close();
        }

        private void PCBoverview_Load(object sender, EventArgs e)
        {
            try { PopulateDataGridView(); }
            catch (SqlException)
            {
                var w = new Form(); Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((t) => w.Close(),TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, "Pripojenie k serveru zlyhalo, nie je možné prehliadať zápisy z uvoľnenia výroby");
            }
        }
        void PopulateDataGridView()
        {
            using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT TOP 50 * FROM PCBproductionRelease order by Date desc", sqlCon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl); dataGridView1.DataSource = dtbl;
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            { column.SortMode = DataGridViewColumnSortMode.NotSortable; }
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Spare1"].Visible = false;
            dataGridView1.Columns["Spare2"].Visible = false;
            dataGridView1.Columns["Spare3"].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Dátum zápisu";
            dataGridView1.Columns[2].HeaderText = "Hodina zápisu";
            dataGridView1.Columns[3].HeaderText = "Zmena";
            dataGridView1.Columns[4].HeaderText = "Dáta z DM kódu";
            dataGridView1.Columns[5].HeaderText = "Popis";
            dataGridView1.Columns[6].HeaderText = "Číslo materiálu";
            dataGridView1.Columns[7].HeaderText = "Partita";
            dataGridView1.Columns[8].HeaderText = "Sériové číslo";
            dataGridView1.Columns[9].HeaderText = "Strana PCB dosky";
            dataGridView1.Columns[10].HeaderText = "Výška lisovania 1,1 mm";
            dataGridView1.Columns[11].HeaderText = "Výška lisovania 1,3 mm";
            dataGridView1.Columns[12].HeaderText = "Vizuálna kontrola";
            dataGridView1.Columns[13].HeaderText = "Popis";
            dataGridView1.Columns[14].HeaderText = "Kontrolu vykonal";
        }
    }
}
