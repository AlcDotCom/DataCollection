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
//using Excel = Microsoft.Office.Interop.Excel;
//using Word = Microsoft.Office.Interop.Word;

namespace Downtime_registration
{
    public partial class EditNOK : Form
    {
        public EditNOK()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN zariadenie
            { WindowState = FormWindowState.Normal; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NOKregistration nextForm = new NOKregistration();
            Hide();
            nextForm.ShowDialog();
            Close();
        }

        private void EditNOK_Load(object sender, EventArgs e)
        {
            try
            {  PopulateDataGridView();  }
            catch (Exception) { }
        }

        void PopulateDataGridView()
        {
            if (Settings.Default.UpravitKategorieAleboZapisyNOK == "KategorieChyb")
            {
                Text = "Úprava / Výmaz kategórie chýb";
                dataGridView1.Visible = true;
                try
                {
                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia + "' order by Operacia asc", sqlCon);
                        DataTable dtbl = new DataTable();
                        sqlDa.Fill(dtbl);
                        dataGridView1.DataSource = dtbl;
                    }
                }
                catch (Exception) { }
            }
            if (Settings.Default.UpravitKategorieAleboZapisyNOK == "ZapisaneChyby")
            {
                Text = "Výmaz zapísaných chýb";
                dataGridView2.Visible = true;
                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                try
                {
                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT TOP 1000 * FROM NokRegistration WHERE Linka = '" + Settings.Default.NazovZariadenia + "' order by DatumZapisu desc ", sqlCon);
                        DataTable dtbl = new DataTable();
                        sqlDa.Fill(dtbl);
                        dataGridView2.DataSource = dtbl;
                    }
                }
                catch (Exception) { }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (Settings.Default.UpravitKategorieAleboZapisyNOK == "KategorieChyb")
            {
                if (dataGridView1.CurrentRow != null)
                {
                    using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                    {
                        sqlCon.Open();
                        DataGridViewRow dgvRow = dataGridView1.CurrentRow;
                        SqlCommand sqlCmd = new SqlCommand("NOKTableAddOrEdit", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ID", dgvRow.Cells["txtID"].Value == DBNull.Value ? "" : dgvRow.Cells["txtID"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Linka", dgvRow.Cells["txtLinka"].Value == DBNull.Value ? "" : dgvRow.Cells["txtLinka"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Operacia", dgvRow.Cells["txtOperacia"].Value == DBNull.Value ? "" : dgvRow.Cells["txtOperacia"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@KodChyby", dgvRow.Cells["txtKodChyby"].Value == DBNull.Value ? "" : dgvRow.Cells["txtKodChyby"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@PopisChyby", dgvRow.Cells["txtPopisChyby"].Value == DBNull.Value ? "" : dgvRow.Cells["txtPopisChyby"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@LimitNaZmenu", dgvRow.Cells["txtLimitNaZmenu"].Value == DBNull.Value ? "" : dgvRow.Cells["txtLimitNaZmenu"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@LimitNaZmenu1", dgvRow.Cells["txtLimitNaZmenu1"].Value == DBNull.Value ? "" : dgvRow.Cells["txtLimitNaZmenu1"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@LimitNaZmenu2", dgvRow.Cells["txtLimitNaZmenu2"].Value == DBNull.Value ? "" : dgvRow.Cells["txtLimitNaZmenu2"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Autor", dgvRow.Cells["txtAutor"].Value == DBNull.Value ? "" : dgvRow.Cells["txtAutor"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@DatumPridania", dgvRow.Cells["txtDatumPridania"].Value == DBNull.Value ? "" : dgvRow.Cells["txtDatumPridania"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Rezerva1", dgvRow.Cells["txtRezerva1"].Value == DBNull.Value ? "" : dgvRow.Cells["txtRezerva1"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Rezerva2", dgvRow.Cells["txtRezerva2"].Value == DBNull.Value ? "" : dgvRow.Cells["txtRezerva2"].Value.ToString());
                        sqlCmd.Parameters.AddWithValue("@Rezerva3", dgvRow.Cells["txtRezerva3"].Value == DBNull.Value ? "" : dgvRow.Cells["txtRezerva3"].Value.ToString());

                        sqlCmd.ExecuteNonQuery();
                        PopulateDataGridView();
                    }
                }
            }
            //if (Settings.Default.UpravitKategorieAleboZapisyNOK == "ZapisaneChyby")  --predpríprava na upravu aj zapisaných chýb ak by bolo treba, aktuálne je prístupné len mazanie s RowFullSelect mode
            //{}
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs vymaz)
        {
                if (dataGridView1.CurrentRow.Cells["txtID"].Value != DBNull.Value)
                {
                    if (MessageBox.Show("Vymazať označenú kategóriu chyby?", "VÝMAZ CHYBY", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                        {
                            sqlCon.Open();
                            SqlCommand sqlCmd = new SqlCommand("NOKTableDeleteById", sqlCon);
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
        private void button1_Click(object sender, EventArgs e)  //export to excel turned off due to false virus detection by win
        {
            //try
            //{   //+ reference Microsoft.office.interop.Excel required + EditColumns -> SortMode -> Programmatic for each column
            //    copyAlltoClipboard();
            //    Excel.Application xlexcel;
            //    Excel.Workbook xlWorkBook;
            //    Excel.Worksheet xlWorkSheet;
            //    object misValue = System.Reflection.Missing.Value;
            //    xlexcel = new Excel.Application();
            //    xlexcel.Visible = true;
            //    xlWorkBook = xlexcel.Workbooks.Add(misValue);
            //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //    Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
            //    CR.Select();
            //    xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            //}
            //catch (Exception) { MessageBox.Show("Excel nie je podporovaný na tomto PC"); }
        }
        private void copyAlltoClipboard()
        {
            //if (Settings.Default.UpravitKategorieAleboZapisyNOK == "KategorieChyb")
            //{
            //    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
            //    dataGridView1.SelectAll();
            //    DataObject dataObj = dataGridView1.GetClipboardContent();
            //    if (dataObj != null)
            //        Clipboard.SetDataObject(dataObj);
            //    dataGridView1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            //}
            //if (Settings.Default.UpravitKategorieAleboZapisyNOK == "ZapisaneChyby")
            //{
            //    dataGridView2.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
            //    dataGridView2.SelectAll();
            //    DataObject dataObj = dataGridView2.GetClipboardContent();
            //    if (dataObj != null)
            //        Clipboard.SetDataObject(dataObj);
            //    dataGridView2.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            //}
        }
        private void dataGridView2_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs vymaz)
        {
            if (dataGridView2.CurrentRow.Cells["ID"].Value != DBNull.Value)
            {
                if (MessageBox.Show("Vymazať označenú chybu?", "VÝMAZ CHYBY", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dataGridView2.SelectedRows)  //maže všetky označené riadky v dgv
                    {
                        using (SqlConnection sqlCon = new SqlConnection(Connection.ConnectionString))
                        {
                            sqlCon.Open();
                            SqlCommand sqlCmd = new SqlCommand("RegisteredNOKTableDeleteById", sqlCon);
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.AddWithValue("@ID", (int)row.Cells["ID"].Value);
                            sqlCmd.ExecuteNonQuery();
                        }
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
