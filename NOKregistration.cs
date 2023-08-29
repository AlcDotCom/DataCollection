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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downtime_registration
{
    public partial class NOKregistration : Form
    {
        private int FailureCode =1;
        public NOKregistration()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN zariadenie
            { WindowState = FormWindowState.Normal; }

            textBox5.Text = Settings.Default.NazovZariadenia;
            textBox19.Text = Settings.Default.IDforNOKreg;
            Settings.Default.Operacia = "";
            Settings.Default.Save();
            Cursor = Cursors.Default;

            if (Settings.Default.NOKPovolenieUpravit == "1")
            { button17.Visible = true;
              button18.Visible = true; }
            else
            { button17.Visible = false;
              button17.Visible = false; }

            var teraz = DateTime.Now;
            DateTime t0 = Convert.ToDateTime("00:00:00");
            DateTime t1 = Convert.ToDateTime("06:00:00");
            DateTime t2 = Convert.ToDateTime("14:00:00");
            DateTime t3 = Convert.ToDateTime("22:00:00");
            string zmena;
            if (teraz >= t1 && teraz < t2)
            {zmena = "Ranná";}
            else if (teraz >= t2 && teraz < t3)
            {zmena = "Poobedná";}
            else if (teraz >= t3)
            { zmena = "Nočná";}
            else if (teraz < t1)
            {zmena = "Nočná";}
            else
            {zmena = "N/A";}
            textBox6.Text = zmena;

                try   //načíta už zaregistrované operácie na tejto linke / zariadení
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT Operacia FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "'", sqlConnection);
                        sqlConnection.Open();
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                        while (sqlReader.Read())
                        {
                         comboBox1.Items.Add(sqlReader["Operacia"].ToString());  //vloží operácie do skryteho cbx len pre účel načítania buttons
                         comboBox2.Items.Add(sqlReader["Operacia"].ToString());  //vloží operácie do cbx pre výber z možností na zadávanie nových chýb
                        }
                        sqlReader.Close();
                    }
                }
                catch (Exception)
                {}

                object[] distinctItems = (from object o in comboBox1.Items select o).Distinct().ToArray();  //odstráni duplicity z cbx1
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(distinctItems);

                object[] distinctItems1 = (from object o in comboBox2.Items select o).Distinct().ToArray();  //odstráni duplicity z cbx2
                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(distinctItems1);

                var count = comboBox1.Items.Count;  //zabráni pridať dalšiu operáciu ak ich je už 14
                if (count > 13)
                {
                comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
                label3.Visible = true;
                }

                try
                {
                    //1. operácia
                    comboBox1.SelectedIndex = 0;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button6.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button6.Text = OP;
                    }

                    //2. operácia
                    comboBox1.SelectedIndex = 1;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button7.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button7.Text = OP;
                    }

                    //3. operácia
                    comboBox1.SelectedIndex = 2;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button9.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button9.Text = OP;
                    }

                    //4. operácia
                    comboBox1.SelectedIndex = 3;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button10.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button10.Text = OP;
                    }

                    //5. operácia
                    comboBox1.SelectedIndex = 4;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button11.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button11.Text = OP;
                    }

                    //6. operácia
                    comboBox1.SelectedIndex = 5;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button12.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button12.Text = OP;
                    }

                    //7. operácia
                    comboBox1.SelectedIndex = 6;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button1.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button1.Text = OP;
                    }

                    //8. operácia
                    comboBox1.SelectedIndex = 7;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button3.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button3.Text = OP;
                    }

                    //9. operácia
                    comboBox1.SelectedIndex = 8;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button4.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button4.Text = OP;
                    }

                    //10. operácia
                    comboBox1.SelectedIndex = 9;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button5.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button5.Text = OP;
                    }

                    //11. operácia
                    comboBox1.SelectedIndex = 10;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button13.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button13.Text = OP;
                    }

                    //12. operácia
                    comboBox1.SelectedIndex = 11;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button14.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button14.Text = OP;
                    }

                    //13. operácia
                    comboBox1.SelectedIndex = 12;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button15.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button15.Text = OP;
                    }

                    //14. operácia
                    comboBox1.SelectedIndex = 13;
                    if (comboBox1.SelectedItem.ToString() == "")
                    { }
                    else
                    {
                        button16.Visible = true;
                        string OP = comboBox1.SelectedItem.ToString();
                        button16.Text = OP;
                    }
                }
                catch (Exception)
                {}
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
        private void button2_Click(object sender, EventArgs e)
        {
            _1 nextForm = new _1();
            Hide();
            nextForm.ShowDialog();
            Close();
        }
        private void button8_Click(object sender, EventArgs e)  //Pridať novú chybu
        {
            comboBox3.Items.Clear();
            try   //načíta už zaregistrované chyby na vybranej operácii pre overenie max poctu (30)
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand sqlCmd = new SqlCommand("SELECT KodChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + comboBox2.Text + "'", sqlConnection);
                    sqlConnection.Open();
                    SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        comboBox3.Items.Add(sqlReader["KodChyby"].ToString());  //vloží chyby do skryteho cbx
                    }
                    sqlReader.Close();
                }
            }
            catch (Exception)
            {}
            object[] distinctItems = (from object o in comboBox3.Items select o).Distinct().ToArray();  //odstráni duplicity
            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(distinctItems);
            var count = comboBox3.Items.Count;
            if (count >= 30)
            { MessageBox.Show("Pre operáciu " + comboBox2.SelectedItem + " bolo zadaných maximum (30) chýb, ak je to možné, vymažte nepotrebné"); return; }

            try //určí nový kód chyby, ak nie je žiadna definovaná zadá 1,ak existujú definuje navyššie číslo +1
            {
                using (SqlConnection Conn = new SqlConnection(Connection.ConnectionString))
                {
                    SqlCommand Com = new SqlCommand("SELECT MAX(CAST(KodChyby AS DECIMAL(10, 0))) FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + comboBox2.Text + "'   ", Conn);
                    Conn.Open(); string a = Com.ExecuteScalar().ToString(); Conn.Close();
                    if (a != "") { FailureCode = Convert.ToInt32(a)+1; }
                }
            }
            catch (Exception){ FailureCode = 1; }

            if (comboBox2.Text.Length == 0)
            {
                MessageBox.Show("Zadajte operáciu");
                comboBox2.Focus();
                return;
            }
            else if (textBox20.Text.Length == 0)
            {
                MessageBox.Show("Zadajte popis chyby");
                textBox20.Focus();
                return;
            }
            else
            {
                var index = comboBox1.FindStringExact(comboBox2.Text);  //Overí či bol zadaná nová operácia porovnaním dvoch cbx
                if (index > -1)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        using (SqlConnection con = new SqlConnection(Connection.ConnectionString))  //zapis chyby do už vytvorenej operácie
                        {
                            con.Open();
                            if (con.State == ConnectionState.Open)
                            {
                                DateTime time = DateTime.Now;
                                string format = "yyyy-MM-dd HH:mm:ss";
                                string q = "insert into NokOptions (Linka,Operacia,KodChyby,PopisChyby,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Autor,DatumPridania,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + comboBox2.Text.ToString() + "','" + FailureCode.ToString() + "','" + textBox20.Text.ToString() + "','" + "10" + "','" + "N/A" + "','" + "N/A" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                                SqlCommand cmd = new SqlCommand(q, con);
                                cmd.ExecuteNonQuery();
                                Cursor = Cursors.Default;
                                MessageBox.Show("Nová chyba bola úspešne pridaná");

                                //vykoná Reload po pridaní novej chyby / novej chyby na novej operácii
                                NOKregistration nextForm = new NOKregistration();
                                Hide();
                                nextForm.ShowDialog();
                                Close();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola pridaná!");
                        return;
                    }
                    textBox20.Text = "";
                    comboBox2.SelectedIndex = -1;
                    textBox22.Text = "";
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Zadali ste novú operáciu, pokračovať?", "NOVÁ OPERÁCIA", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            Cursor = Cursors.WaitCursor;
                            using (SqlConnection con = new SqlConnection(Connection.ConnectionString))  //zapis chyby do na novo vytvorenej operácie
                            {
                                con.Open();
                                if (con.State == ConnectionState.Open)
                                {
                                    DateTime time = DateTime.Now;
                                    string format = "yyyy-MM-dd HH:mm:ss";

                                    string q = "insert into NokOptions (Linka,Operacia,KodChyby,PopisChyby,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Autor,DatumPridania,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + comboBox2.Text.ToString() + "','" + FailureCode.ToString() + "','" + textBox20.Text.ToString() + "','" + "10" + "','" + "N/A" + "','" + "N/A" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";

                                    SqlCommand cmd = new SqlCommand(q, con);
                                    cmd.ExecuteNonQuery();
                                    Cursor = Cursors.Default;
                                    MessageBox.Show("Nová chyba bola úspešne pridaná");

                                    //vykoná Reload po pridaní novej chyby / novej chyby na novej operácii
                                    NOKregistration nextForm = new NOKregistration();
                                    Hide();
                                    nextForm.ShowDialog();
                                    Close();
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola pridaná!");
                            return;
                        }
                        textBox20.Text = "";
                        comboBox2.SelectedIndex = -1;
                        textBox22.Text = "";
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                     return;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)  //1. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button6.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button7_Click(object sender, EventArgs e)  //2. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button7.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button9_Click(object sender, EventArgs e)  //3. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button9.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button10_Click(object sender, EventArgs e)  //4. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button10.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button11_Click(object sender, EventArgs e)  //5. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button11.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button12_Click(object sender, EventArgs e)  //6. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button12.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)  //7. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button1.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)  //8. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button3.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)  //9. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button4.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button5_Click(object sender, EventArgs e)  //10. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button5.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button13_Click(object sender, EventArgs e)  //11. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button13.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button14_Click(object sender, EventArgs e)  //12. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button14.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button15_Click(object sender, EventArgs e)  //13. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button15.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button16_Click(object sender, EventArgs e)  //14. operácia
        {
            Cursor = Cursors.WaitCursor;
            Settings.Default.Operacia = button16.Text;
            Settings.Default.Save();
            NOK_OP nextForm = new NOK_OP();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Settings.Default.UpravitKategorieAleboZapisyNOK = "KategorieChyb";
            Settings.Default.Save();
            Cursor = Cursors.WaitCursor;
            EditNOK nextForm = new EditNOK();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
         comboBox2.DroppedDown = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Settings.Default.UpravitKategorieAleboZapisyNOK = "ZapisaneChyby";
            Settings.Default.Save();
            Settings.Default.Save();
            Cursor = Cursors.WaitCursor;
            EditNOK nextForm = new EditNOK();
            Hide();
            nextForm.ShowDialog();
            Close();
            Cursor = Cursors.Default;
        }
    }
}

