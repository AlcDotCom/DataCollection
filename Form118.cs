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
    public partial class Form118 : Form
    {
        public Form118()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN zariadenie
            { WindowState = FormWindowState.Normal; }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
            Form12 nextForm = new Form12();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void Form118_Load(object sender, EventArgs e)
        {
            try {PopulateDataGridView();
            }
            catch (Exception)
            {}
        }

        void PopulateDataGridView()
        {
            using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM access", sqlCon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dataGridView1.DataSource = dtbl;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                {
                    sqlCon.Open();
                    DataGridViewRow dgvRow = dataGridView1.CurrentRow;
                    SqlCommand sqlCmd = new SqlCommand("AccessTableAddOrEdit", sqlCon);   
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@ID", dgvRow.Cells["txtID"].Value == DBNull.Value ? "" : dgvRow.Cells["txtID"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@HEX", dgvRow.Cells["txtHEX"].Value == DBNull.Value ? "" : dgvRow.Cells["txtHEX"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@NASTAVENIE", dgvRow.Cells["txtNASTAVENIE"].Value == DBNull.Value ? "" : dgvRow.Cells["txtNASTAVENIE"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@VYMAZ", dgvRow.Cells["txtVYMAZ"].Value == DBNull.Value ? "" : dgvRow.Cells["txtVYMAZ"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@MENO", dgvRow.Cells["txtMENO"].Value == DBNull.Value ? "" : dgvRow.Cells["txtMENO"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@PRIEZVISKO", dgvRow.Cells["txtPRIEZVISKO"].Value == DBNull.Value ? "" : dgvRow.Cells["txtPRIEZVISKO"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@zalozil", "Zmenil: " + Settings.Default.AktualnyUzivatel);
                    sqlCmd.Parameters.AddWithValue("@datum", DateTime.Now.ToString("yyyy - MM - dd HH: mm: ss"));
                    sqlCmd.Parameters.AddWithValue("@rezerva1", dgvRow.Cells["txtrezerva1"].Value == DBNull.Value ? "" : dgvRow.Cells["txtrezerva1"].Value.ToString());
                    sqlCmd.Parameters.AddWithValue("@rezerva2", Environment.MachineName);
                    sqlCmd.Parameters.AddWithValue("@rezerva3", dgvRow.Cells["txtrezerva3"].Value == DBNull.Value ? "" : dgvRow.Cells["txtrezerva3"].Value.ToString());

                    sqlCmd.ExecuteNonQuery();
                    PopulateDataGridView();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewUser nextForm = new NewUser();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
    }
}
