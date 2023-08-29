using Downtime_registration.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlClient;

namespace Downtime_registration
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();

            try   //načíta už používané zariadenie
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT Linka FROM DowntimeOptions", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        comboBox1.Items.Add(sqlReader["Linka"].ToString()); 
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            { }
            object[] distinctItems1 = (from object o in comboBox1.Items select o).Distinct().ToArray();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(distinctItems1);

            if(Settings.Default.NazovZariadenia != "") //ak už je nastavená linka, zobrazí aktuálne nastavenú linku
            {
                try
                {
                    int index = comboBox1.Items.IndexOf(Settings.Default.NazovZariadenia);
                    comboBox1.SelectedIndex = index;
                }
                catch (Exception) {}
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form12 nextForm = new Form12();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Settings.Default.NazovZariadenia = comboBox1.Text;
            Settings.Default.Save();
            MessageBox.Show("Zmeny boli uložené");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Connection.ConnectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Pripojenie je OK");
                }
                catch (SqlException)
                {
                    MessageBox.Show("Pripojenie nebolo úspešné");
                }
            }
        }
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
    }
}
