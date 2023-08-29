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
    public partial class NOK_OP : Form
    {
        private int TotalNokReset1 =0; private int TotalNokReset2 = 0; private int TotalNokReset3 = 0; private int TotalNokReset4 = 0; private int TotalNokReset5 = 0;
        private int TotalNokReset6 = 0; private int TotalNokReset7 = 0; private int TotalNokReset8 = 0; private int TotalNokReset9 = 0; private int TotalNokReset10 = 0;

        public NOK_OP()
        {
            InitializeComponent();

            if (Settings.Default.NazovZariadenia == "LHINTEN")  //prispôsobenie pre LHINTEN
            { WindowState = FormWindowState.Normal; }

            Text = "Zápis NOK na operácii " + Settings.Default.Operacia;
            textBox19.Text = Settings.Default.IDforNOKreg;
            textBox42.Text = Settings.Default.Operacia;
            textBox43.Text = Settings.Default.NazovZariadenia;

            Settings.Default.AktualnaZmena = "";
            GetActualSHIFT();
            textBox44.Text = Settings.Default.AktualnaZmena;

            Settings.Default.NOKAktualnaStrana = "1";Settings.Default.NOKPocetStran = "1";Settings.Default.Save();
            comboBox1.Items.Clear();

            LoadNOKcodeToCBX(); LoadNOKcodesToPage1();
            LoadNOKdescription();
            UpdateNumOfNOK();
            LoadEscalationLimits();
            UpdateEscalationButtonAndMessage();

            var count = comboBox1.Items.Count;  //zisti počet straán pre chyby 10,20,30
            if (count < 11)
            {
                Settings.Default.NOKPocetStran = "1";
                Settings.Default.Save();
            }
            else if (count > 10 && count < 21)
            {
                button12.Visible = true;
                label19.Visible = true;
                label19.Text = "Strana 1 z 2";
                Settings.Default.NOKPocetStran = "2";Settings.Default.Save();
            }
            else if (count > 20 && count < 1000)
            {
                button12.Visible = true;
                label19.Visible = true;
                label19.Text = "Strana 1 z 3";
                Settings.Default.NOKPocetStran = "3"; Settings.Default.Save();
            }

            void LoadNOKcodeToCBX()   //načíta chyby pre danú operáciu do pomocného cbx
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString))
                    {
                        SqlCommand sqlCmd = new SqlCommand("SELECT KodChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "'", sqlConnection);
                        sqlConnection.Open();
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {comboBox1.Items.Add(sqlReader["KodChyby"].ToString());}
                        sqlReader.Close();
                    }
                }
                catch (Exception){}
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NOKregistration nextForm = new NOKregistration();
            Hide();
            nextForm.ShowDialog();
            Close();
        }

        public void UpdateNumOfNOK()   //Súčet NOK len na danej zmene
        {
            DateTime today = DateTime.Now.Subtract(TimeSpan.FromHours(9));
            DateTime today1 = DateTime.Now;
            GetActualSHIFT();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString + ";MultipleActiveResultSets=True"))  //otvorí spojenie pre multi-command
                {
                    sqlConnection.Open();
                    //1. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox18.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox4.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //2. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox3.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox25.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //3. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox2.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox27.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //4. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox5.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox29.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //5. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox6.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox31.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //6. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox8.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox33.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //7. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox9.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox35.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //8. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox10.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox37.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //9. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND KodChyby = '" + textBox11.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox39.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    //10. Súčet NOK
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT SUM (cast(Pocet as int)) FROM NokRegistration WHERE DatumZapisu BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Linka = '" + Settings.Default.NazovZariadenia + "' AND Operacia = '" + Settings.Default.Operacia + "' AND PopisChyby = '" + textBox23.Text + "' AND KodChyby = '" + textBox12.Text + "' AND Zmena = '" + Settings.Default.AktualnaZmena + "'", sqlConnection))
                    {
                        textBox41.Text = sqlCmd.ExecuteScalar().ToString();
                    }
                    sqlConnection.Close();  //zavrie multi-command spojenie
                }
            }
            catch (Exception)
            { }
        }

        void LoadEscalationLimits()
        {
            DateTime today = DateTime.Now.Subtract(TimeSpan.FromHours(9)); DateTime today1 = DateTime.Now;
            GetActualSHIFT();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString + ";MultipleActiveResultSets=True"))  //otvorí spojenie pre multi-command
                {
                    sqlConnection.Open();

                    //1. Limit
                    try
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox1.Text + "' AND NOKcode = '" + textBox18.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        {if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset1 = 0; textBox7.BackColor = Color.White; } else { TotalNokReset1 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox7.BackColor = Color.DarkOrange; }}
                    }       
                    catch (Exception) { TotalNokReset1 = 0; textBox7.BackColor = Color.White;}
                    
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox1.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox7.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset1).ToString();}
                    }

                    try //2. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox13.Text + "' AND NOKcode = '" + textBox3.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        { if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset2 = 0; textBox24.BackColor = Color.White; } else { TotalNokReset2 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox24.BackColor = Color.DarkOrange; }}
                    }           
                    catch (Exception) { TotalNokReset2 = 0; textBox24.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox13.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox24.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset2).ToString();}
                    }

                    try //3. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox14.Text + "' AND NOKcode = '" + textBox2.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        { if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset3 = 0; textBox26.BackColor = Color.White; } else { TotalNokReset3 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox26.BackColor = Color.DarkOrange; } }
                    }        
                    catch (Exception) { TotalNokReset3 = 0; textBox26.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox14.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox26.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset3).ToString();}
                    }

                    try //4. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox15.Text + "' AND NOKcode = '" + textBox5.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        { if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset4 = 0; textBox28.BackColor = Color.White; } else { TotalNokReset4 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox28.BackColor = Color.DarkOrange; }}
                    }         
                    catch (Exception) { TotalNokReset4 = 0; textBox28.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox15.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox28.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset4).ToString();}
                    }

                    try //5. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox16.Text + "' AND NOKcode = '" + textBox6.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        {if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset5 = 0; textBox30.BackColor = Color.White; } else { TotalNokReset5 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox30.BackColor = Color.DarkOrange; }}
                    }           
                    catch (Exception) { TotalNokReset5 = 0; textBox30.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox16.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox30.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset5).ToString();}
                    }

                    try //6. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox17.Text + "' AND NOKcode = '" + textBox8.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        {if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset6 = 0; textBox32.BackColor = Color.White; } else { TotalNokReset6 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox32.BackColor = Color.DarkOrange; }}
                    }          
                    catch (Exception) { TotalNokReset6 = 0; textBox32.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox17.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox32.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset6).ToString();} 
                    }

                    try //7. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox20.Text + "' AND NOKcode = '" + textBox9.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        {if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset7= 0; textBox34.BackColor = Color.White; } else { TotalNokReset7 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox34.BackColor = Color.DarkOrange; }}
                    }       
                    catch (Exception) { TotalNokReset7 = 0; textBox34.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox20.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox34.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset7).ToString();}
                    }

                    try //8. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox21.Text + "' AND NOKcode = '" + textBox10.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        { if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset8 = 0; textBox36.BackColor = Color.White; } else { TotalNokReset8 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox36.BackColor = Color.DarkOrange; }}
                    }          
                    catch (Exception) { TotalNokReset8 = 0; textBox36.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox21.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox36.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset8).ToString();}
                    }

                    try //9. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox22.Text + "' AND NOKcode = '" + textBox11.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        {if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset9 = 0; textBox38.BackColor = Color.White; } else { TotalNokReset9 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox38.BackColor = Color.DarkOrange; }}
                    }   
                    catch (Exception) { TotalNokReset9 = 0; textBox38.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox22.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox38.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset9).ToString();}
                    }

                    try //10. Limit
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT LAST_VALUE(NumOfNOK) OVER (ORDER BY ID DESC) FROM NOKescalations WHERE Date BETWEEN '" + today.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + today1.ToString("yyyy-MM-dd HH:mm:ss") + "' AND Line = '" + Settings.Default.NazovZariadenia + "' AND Operation = '" + textBox42.Text + "' AND NOKdescription = '" + textBox23.Text + "' AND NOKcode = '" + textBox12.Text + "' AND Shift = '" + Settings.Default.AktualnaZmena + "' ", sqlConnection))
                        {if (sqlCmd.ExecuteScalar().ToString() == "") { TotalNokReset10 = 0; textBox40.BackColor = Color.White; } else { TotalNokReset10 = Convert.ToInt32(sqlCmd.ExecuteScalar()); textBox40.BackColor = Color.DarkOrange; }}
                    }        
                    catch (Exception) { TotalNokReset10 = 0; textBox40.BackColor = Color.White; }

                    using (SqlCommand sqlCmd = new SqlCommand("SELECT LimitNaZmenu FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND PopisChyby = '" + textBox23.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {textBox40.Text = (Convert.ToInt32((sqlReader["LimitNaZmenu"])) + TotalNokReset10).ToString();}
                    }
                    sqlConnection.Close();  //zavrie multi-command spojenie
                }
            }
            catch (Exception){}
        }

        public void GetActualSHIFT()
        {
            if (DateTime.Now >= Convert.ToDateTime("06:00:00") && DateTime.Now < Convert.ToDateTime("14:00:00")) { Settings.Default.AktualnaZmena = "Ranná"; Settings.Default.Save(); }
            else if (DateTime.Now >= Convert.ToDateTime("14:00:00") && DateTime.Now < Convert.ToDateTime("22:00:00")) { Settings.Default.AktualnaZmena = "Poobedná"; Settings.Default.Save(); }
            else if (DateTime.Now >= Convert.ToDateTime("22:00:00") || DateTime.Now < Convert.ToDateTime("06:00:00")) { Settings.Default.AktualnaZmena = "Nočná"; Settings.Default.Save(); }
        }

        private void button2_Click(object sender, EventArgs e)  //+1 pre 1. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox18.Text;
            string popischyby = textBox1.Text;
            string limitnazmenu = textBox7.Text;
            if (textBox1.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    UpdateNumOfNOK(); UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)  //+1 pre 2. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox3.Text;
            string popischyby = textBox13.Text;
            string limitnazmenu = textBox24.Text;
            if (textBox13.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    UpdateNumOfNOK(); UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)  //+1 pre 3. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox2.Text;
            string popischyby = textBox14.Text;
            string limitnazmenu = textBox26.Text;
            if (textBox14.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;
                        }
                    }
                    UpdateNumOfNOK(); UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button5_Click(object sender, EventArgs e)  //+1 pre 4. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox5.Text;
            string popischyby = textBox15.Text;
            string limitnazmenu = textBox28.Text;
            if (textBox15.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;
                        }
                    }
                    UpdateNumOfNOK(); UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button6_Click(object sender, EventArgs e)  //+1 pre 5. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox6.Text;
            string popischyby = textBox16.Text;
            string limitnazmenu = textBox30.Text;
            if (textBox16.Text != "") 
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;
                        }
                    }
                    UpdateNumOfNOK(); UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button7_Click(object sender, EventArgs e)  //+1 pre 6. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox8.Text;
            string popischyby = textBox17.Text;
            string limitnazmenu = textBox32.Text;
            if (textBox17.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;
                        }
                    }
                    UpdateNumOfNOK(); UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button8_Click(object sender, EventArgs e)  //+1 pre 7. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox9.Text;
            string popischyby = textBox20.Text;
            string limitnazmenu = textBox34.Text;
            if (textBox20.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;
                        }
                    }
                    UpdateNumOfNOK(); UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button9_Click(object sender, EventArgs e)  //+1 pre 8. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox10.Text;
            string popischyby = textBox21.Text;
            string limitnazmenu = textBox36.Text;
            if (textBox21.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;
                        }
                    }
                    UpdateNumOfNOK(); UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button10_Click(object sender, EventArgs e)  //+1 pre 9. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox11.Text;
            string popischyby = textBox22.Text;
            string limitnazmenu = textBox38.Text;
            if (textBox22.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;
                        }
                    }
                    UpdateNumOfNOK();UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }

        private void button11_Click(object sender, EventArgs e)  //+1 pre 10. chybu
        {
            Cursor = Cursors.WaitCursor;
            GetActualSHIFT();
            string kodchyby = textBox12.Text;
            string popischyby = textBox23.Text;
            string limitnazmenu = textBox40.Text;
            if (textBox23.Text != "")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";
                            string q = "insert into NokRegistration (Linka,Operacia,KodChyby,PopisChyby,Zmena,LimitNaZmenu,LimitNaZmenu1,LimitNaZmenu2,Pocet,Zapisal,DatumZapisu,Rezerva1,Rezerva2,Rezerva3)values('" + Settings.Default.NazovZariadenia + "','" + Settings.Default.Operacia + "','" + kodchyby + "','" + popischyby + "','" + Settings.Default.AktualnaZmena + "','" + limitnazmenu + "','" + "N/A" + "','" + "N/A" + "','" + "1" + "','" + Settings.Default.IDforNOKreg + "','" + time.ToString(format) + "','" + "N/A" + "','" + "N/A" + "','" + "N/A" + "')";
                            SqlCommand cmd = new SqlCommand(q, con);
                            cmd.ExecuteNonQuery();
                            Cursor = Cursors.Default;
                        }
                    }
                    UpdateNumOfNOK();UpdateEscalationButtonAndMessage();
                }
                catch (Exception)
                {
                    MessageBox.Show("Spojenie so serverom zlyhalo, chyba nebola zaregistrovaná!");
                }
            }
            else
            { }
            Cursor = Cursors.Default;
        }
        void UpdateEscalationButtonAndMessage()
        {
            button14.Visible = false; button15.Visible = false; button16.Visible = false; button17.Visible = false; button18.Visible = false;
            button19.Visible = false; button20.Visible = false; button21.Visible = false; button22.Visible = false; button23.Visible = false;
            try //1.chyba
            {
                int pocet = Int32.Parse(textBox4.Text);
                int limit = Int32.Parse(textBox7.Text);
                if (pocet >= limit)
                {
                    textBox4.BackColor = Color.Red;
                    label4.Visible = true; button14.Visible = true;
                }
                else
                {
                    textBox4.BackColor = Color.Gray;
                    label4.Visible = false; button14.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox4.BackColor = Color.Gray;
                label4.Visible = false;
            }
            try //2.chyba
            {
                int pocet = Int32.Parse(textBox25.Text);
                int limit = Int32.Parse(textBox24.Text);
                if (pocet >= limit)
                {
                    textBox25.BackColor = Color.Red;
                    label11.Visible = true; button15.Visible = true;
                }
                else
                {
                    textBox25.BackColor = Color.Gray;
                    label11.Visible = false; button15.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox25.BackColor = Color.Gray;
                label11.Visible = false;
            }
            try //3.chyba
            {
                int pocet = Int32.Parse(textBox27.Text);
                int limit = Int32.Parse(textBox26.Text);
                if (pocet >= limit)
                {
                    textBox27.BackColor = Color.Red;
                    label20.Visible = true; button16.Visible = true;
                }
                else
                {
                    textBox27.BackColor = Color.Gray;
                    label20.Visible = false; button16.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox27.BackColor = Color.Gray;
                label20.Visible = false;
            }
            try //4.chyba
            {
                int pocet = Int32.Parse(textBox29.Text);
                int limit = Int32.Parse(textBox28.Text);
                if (pocet >= limit)
                {
                    textBox29.BackColor = Color.Red;
                    label21.Visible = true; button17.Visible = true;
                }
                else
                {
                    textBox29.BackColor = Color.Gray;
                    label21.Visible = false; button17.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox29.BackColor = Color.Gray;
                label21.Visible = false;
            }
            try //5.chyba
            {
                int pocet = Int32.Parse(textBox31.Text);
                int limit = Int32.Parse(textBox30.Text);
                if (pocet >= limit)
                {
                    textBox31.BackColor = Color.Red;
                    label22.Visible = true; button18.Visible = true;
                }
                else
                {
                    textBox31.BackColor = Color.Gray;
                    label22.Visible = false; button18.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox31.BackColor = Color.Gray;
                label22.Visible = false;
            }
            try //6.chyba
            {
                int pocet = Int32.Parse(textBox33.Text);
                int limit = Int32.Parse(textBox32.Text);
                if (pocet >= limit)
                {
                    textBox33.BackColor = Color.Red;
                    label23.Visible = true; button19.Visible = true;
                }
                else
                {
                    textBox33.BackColor = Color.Gray;
                    label23.Visible = false; button19.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox33.BackColor = Color.Gray;
                label23.Visible = false;
            }
            try //7.chyba
            {
                int pocet = Int32.Parse(textBox35.Text);
                int limit = Int32.Parse(textBox34.Text);
                if (pocet >= limit)
                {
                    textBox35.BackColor = Color.Red;
                    label24.Visible = true; button20.Visible = true;
                }
                else
                {
                    textBox35.BackColor = Color.Gray;
                    label24.Visible = false; button20.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox35.BackColor = Color.Gray;
                label24.Visible = false;
            }
            try //8.chyba
            {
                int pocet = Int32.Parse(textBox37.Text);
                int limit = Int32.Parse(textBox36.Text);
                if (pocet >= limit)
                {
                    textBox37.BackColor = Color.Red;
                    label25.Visible = true; button21.Visible = true;
                }
                else
                {
                    textBox37.BackColor = Color.Gray;
                    label25.Visible = false; button21.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox37.BackColor = Color.Gray;
                label25.Visible = false;
            }
            try //9.chyba
            {
                int pocet = Int32.Parse(textBox39.Text);
                int limit = Int32.Parse(textBox38.Text);
                if (pocet >= limit)
                {
                    textBox39.BackColor = Color.Red;
                    label26.Visible = true; button22.Visible = true;
                }
                else
                {
                    textBox39.BackColor = Color.Gray;
                    label26.Visible = false; button22.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox39.BackColor = Color.Gray;
                label26.Visible = false;
            }
            try //10.chyba
            {
                int pocet = Int32.Parse(textBox41.Text);
                int limit = Int32.Parse(textBox40.Text);
                if (pocet >= limit)
                {
                    textBox41.BackColor = Color.Red;
                    label27.Visible = true; button23.Visible = true;
                }
                else
                {
                    textBox41.BackColor = Color.Gray;
                    label27.Visible = false; button23.Visible = false;
                }
            }
            catch (FormatException)
            {
                textBox41.BackColor = Color.Gray;
                label27.Visible = false; 
            }
        }

        void LoadNOKdescription()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Connection.ConnectionString + ";MultipleActiveResultSets=True"))  //otvorí spojenie pre multi-command
                {
                    sqlConnection.Open();
                    //1. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox18.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox1.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //2. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox3.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox13.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //3. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox2.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox14.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //4. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox5.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox15.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //5. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox6.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox16.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //6. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox8.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox17.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //7. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox9.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox20.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //8. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox10.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox21.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //9. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox11.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox22.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    //10. kód chyby
                    using (SqlCommand sqlCmd = new SqlCommand("SELECT PopisChyby FROM NokOptions WHERE Linka = '" + Settings.Default.NazovZariadenia.ToString() + "' AND Operacia = '" + Settings.Default.Operacia.ToString() + "' AND KodChyby = '" + textBox12.Text.ToString() + "'", sqlConnection))
                    {
                        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            textBox23.Text = (sqlReader["PopisChyby"].ToString());
                        }
                    }
                    sqlConnection.Close();  //zavrie multi-command spojenie
                }
            }
            catch (Exception)
            { }
        }

        void LoadNOKcodesToPage1()   //načíta chyby do textboxov prvých 10 na 1. strane
        {
            try
            {
                //1. chyba
                comboBox1.SelectedIndex = 0;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox18.Text = comboBox1.SelectedItem.ToString();}
                //2. chyba
                comboBox1.SelectedIndex = 1;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox3.Text = comboBox1.SelectedItem.ToString();}
                //3. chyba
                comboBox1.SelectedIndex = 2;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else {textBox2.Text = comboBox1.SelectedItem.ToString(); }
                //4. chyba
                comboBox1.SelectedIndex = 3;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{ textBox5.Text = comboBox1.SelectedItem.ToString();}
                //5. chyba
                comboBox1.SelectedIndex = 4;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{ textBox6.Text = comboBox1.SelectedItem.ToString(); }
                //6. chyba
                comboBox1.SelectedIndex = 5;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox8.Text = comboBox1.SelectedItem.ToString();}
                //7. chyba
                comboBox1.SelectedIndex = 6;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{ textBox9.Text = comboBox1.SelectedItem.ToString();}
                //8. chyba
                comboBox1.SelectedIndex = 7;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox10.Text = comboBox1.SelectedItem.ToString();}
                //9. chyba
                comboBox1.SelectedIndex = 8;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else { textBox11.Text = comboBox1.SelectedItem.ToString(); }
                //10. chyba
                comboBox1.SelectedIndex = 9;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox12.Text = comboBox1.SelectedItem.ToString();}
            }
            catch (Exception){ }
        }

        void LoadNOKcodesToPage2()   //načíta chyby do textboxov 10-20 na 2. strane
        {
            try
            {
                //11. chyba
                comboBox1.SelectedIndex = 10;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox18.Text = comboBox1.SelectedItem.ToString();}
                //12. chyba
                comboBox1.SelectedIndex = 11;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{ textBox3.Text = comboBox1.SelectedItem.ToString(); }
                //13. chyba
                comboBox1.SelectedIndex = 12;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox2.Text = comboBox1.SelectedItem.ToString();}
                //14. chyba
                comboBox1.SelectedIndex = 13;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox5.Text = comboBox1.SelectedItem.ToString();}
                //15. chyba
                comboBox1.SelectedIndex = 14;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox6.Text = comboBox1.SelectedItem.ToString();}
                //16. chyba
                comboBox1.SelectedIndex = 15;
                if (comboBox1.SelectedItem.ToString() == "") { }
                else{ textBox8.Text = comboBox1.SelectedItem.ToString(); }
                //17. chyba
                comboBox1.SelectedIndex = 16;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{ textBox9.Text = comboBox1.SelectedItem.ToString();}
                //18. chyba
                comboBox1.SelectedIndex = 17;
                if (comboBox1.SelectedItem.ToString() == "") { }
                else{ textBox10.Text = comboBox1.SelectedItem.ToString();}
                //19. chyba
                comboBox1.SelectedIndex = 18;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{ textBox11.Text = comboBox1.SelectedItem.ToString(); }
                //20. chyba
                comboBox1.SelectedIndex = 19;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox12.Text = comboBox1.SelectedItem.ToString(); }
            }
            catch (Exception)
            { }
        }
        void LoadNOKcodesToPage3()   //načíta chyby do textboxov 20-30 na 3. strane
        {
            try
            {
                //21. chyba
                comboBox1.SelectedIndex = 20;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox18.Text = comboBox1.SelectedItem.ToString();}
                //22. chyba
                comboBox1.SelectedIndex = 21;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else { textBox3.Text = comboBox1.SelectedItem.ToString();}
                //23. chyba
                comboBox1.SelectedIndex = 22;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox2.Text = comboBox1.SelectedItem.ToString(); }
                //24. chyba
                comboBox1.SelectedIndex = 23;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else {textBox5.Text = comboBox1.SelectedItem.ToString(); }
                //25. chyba
                comboBox1.SelectedIndex = 24;
                if (comboBox1.SelectedItem.ToString() == "") { }
                else{ textBox6.Text = comboBox1.SelectedItem.ToString();}
                //26. chyba
                comboBox1.SelectedIndex = 25;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{ textBox8.Text = comboBox1.SelectedItem.ToString(); }
                //27. chyba
                comboBox1.SelectedIndex = 26;
                if (comboBox1.SelectedItem.ToString() == "") { }
                else{ textBox9.Text = comboBox1.SelectedItem.ToString();}
                //28. chyba
                comboBox1.SelectedIndex = 27;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else { textBox10.Text = comboBox1.SelectedItem.ToString(); }
                //29. chyba
                comboBox1.SelectedIndex = 28;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox11.Text = comboBox1.SelectedItem.ToString(); }
                //30. chyba
                comboBox1.SelectedIndex = 29;
                if (comboBox1.SelectedItem.ToString() == ""){ }
                else{textBox12.Text = comboBox1.SelectedItem.ToString(); }
            }
            catch (Exception)
            { }
        }

        void ClearTXTboxes()
        {
            textBox1.Text = "";textBox13.Text = "";textBox14.Text = "";textBox15.Text = "";textBox16.Text = "";textBox17.Text = "";textBox20.Text = "";textBox21.Text = "";textBox22.Text = "";textBox23.Text = "";
            textBox18.Text = "";textBox3.Text = "";textBox2.Text = "";textBox5.Text = "";textBox6.Text = "";textBox8.Text = "";textBox9.Text = "";textBox10.Text = "";textBox11.Text = "";textBox12.Text = "";
            textBox4.Text = "0";textBox25.Text = "0";textBox27.Text = "0";textBox29.Text = "0";textBox31.Text = "0";textBox33.Text = "0";textBox35.Text = "0";textBox37.Text = "0";textBox39.Text = "0";textBox41.Text = "0";
            textBox4.BackColor = Color.Gray;
            textBox25.BackColor = Color.Gray;
            textBox27.BackColor = Color.Gray;
            textBox29.BackColor = Color.Gray;
            textBox31.BackColor = Color.Gray;
            textBox33.BackColor = Color.Gray;
            textBox35.BackColor = Color.Gray;
            textBox37.BackColor = Color.Gray;
            textBox39.BackColor = Color.Gray;
            textBox41.BackColor = Color.Gray;
            textBox7.Text = "";textBox24.Text = "";textBox26.Text = "";textBox28.Text = "";textBox30.Text = "";textBox32.Text = "";textBox34.Text = "";textBox36.Text = "";textBox38.Text = "";textBox40.Text = "";
        }

            private void button12_Click(object sender, EventArgs e)
            {
            Cursor = Cursors.WaitCursor;
            if (Settings.Default.NOKAktualnaStrana == "1") //zobrazenie 2. strany z 1.
            {
                ClearTXTboxes();
                LoadNOKcodesToPage2();
                LoadNOKdescription();
                UpdateNumOfNOK();
                LoadEscalationLimits();
                UpdateEscalationButtonAndMessage();

                Settings.Default.NOKAktualnaStrana = "2";
                label3.Text = "11.";label6.Text = "12.";label7.Text = "13.";label9.Text = "14.";label13.Text = "15.";label14.Text = "16.";label15.Text = "17.";label16.Text = "18.";label17.Text = "19.";label18.Text = "20.";
                if (Settings.Default.NOKPocetStran == "2")
                {
                    button12.Visible = false;
                    button13.Visible = true;
                    label19.Visible = true;
                    label19.Text = "Strana 2 z 2";
                }
                else if(Settings.Default.NOKPocetStran == "3")
                {
                    button12.Visible = true;
                    button13.Visible = true;
                    label19.Visible = true;
                    label19.Text = "Strana 2 z 3";
                }
                Cursor = Cursors.Default;
                return;

            }
            if (Settings.Default.NOKAktualnaStrana == "2")  //zobrazenie 3. strany z 2.
            {
                ClearTXTboxes();
                LoadNOKcodesToPage3();
                LoadNOKdescription();
                UpdateNumOfNOK();
                LoadEscalationLimits();
                UpdateEscalationButtonAndMessage();

                Settings.Default.NOKAktualnaStrana = "3";
                label3.Text = "21."; label6.Text = "22."; label7.Text = "23."; label9.Text = "24."; label13.Text = "25."; label14.Text = "26."; label15.Text = "27."; label16.Text = "28."; label17.Text = "29."; label18.Text = "30.";
                button12.Visible = false;
                button13.Visible = true;
                label19.Visible = true;
                label19.Text = "Strana 3 z 3";
            }
            Cursor = Cursors.Default;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (Settings.Default.NOKAktualnaStrana == "2")  //zobrazenie 1. strany späť z 2.
            {
                ClearTXTboxes();
                LoadNOKcodesToPage1();
                LoadNOKdescription();
                UpdateNumOfNOK();
                LoadEscalationLimits();
                UpdateEscalationButtonAndMessage();

                Settings.Default.NOKAktualnaStrana = "1";
                label3.Text = "1."; label6.Text = "2."; label7.Text = "3."; label9.Text = "4."; label13.Text = "5."; label14.Text = "6."; label15.Text = "7."; label16.Text = "8."; label17.Text = "9."; label18.Text = "10.";
                button12.Visible = true;
                button13.Visible = false;
                label19.Visible = true;

                if (Settings.Default.NOKPocetStran == "2")
                {
                    label19.Visible = true;
                    label19.Text = "Strana 1 z 2";
                }
                else if (Settings.Default.NOKPocetStran == "3")
                {
                    label19.Visible = true;
                    label19.Text = "Strana 1 z 3";
                }
                Cursor = Cursors.Default;
                return;
            }

            if (Settings.Default.NOKAktualnaStrana == "3")  //zobrazenie 2. strany späť z 3.
            {
                ClearTXTboxes();
                LoadNOKcodesToPage2();
                LoadNOKdescription();
                UpdateNumOfNOK();
                LoadEscalationLimits();
                UpdateEscalationButtonAndMessage();

                Settings.Default.NOKAktualnaStrana = "2";
                label3.Text = "11."; label6.Text = "12."; label7.Text = "13."; label9.Text = "14."; label13.Text = "15."; label14.Text = "16."; label15.Text = "17."; label16.Text = "18."; label17.Text = "19."; label18.Text = "20.";
                button12.Visible = true;
                button13.Visible = true;
                label19.Visible = true;
                label19.Text = "Strana 2 z 3";
            }
            Cursor = Cursors.Default;
        }

        private void button14_Click(object sender, EventArgs e)  //record of escalation Reset for 1.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";   
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox4.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox18.Text) + "','" + textBox1.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!");}
        }

        private void button15_Click(object sender, EventArgs e)  //record of escalation Reset for 2.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";  
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox25.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox3.Text) + "','" + textBox13.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }

        private void button16_Click(object sender, EventArgs e)  //record of escalation Reset for 3.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";                
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox27.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox2.Text) + "','" + textBox14.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }

        private void button17_Click(object sender, EventArgs e)  //record of escalation Reset for 4.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";             
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox29.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox5.Text) + "','" + textBox15.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }

        private void button18_Click(object sender, EventArgs e)  //record of escalation Reset for 5.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";      
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox31.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox6.Text) + "','" + textBox16.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }

        private void button19_Click(object sender, EventArgs e)  //record of escalation Reset for 6.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";                     
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox33.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox8.Text) + "','" + textBox17.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }

        private void button20_Click(object sender, EventArgs e)  //record of escalation Reset for 7.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";                          
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox35.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox9.Text) + "','" + textBox20.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }

        private void button21_Click(object sender, EventArgs e)  //record of escalation Reset for 8.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";                  
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox37.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox10.Text) + "','" + textBox21.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }

        private void button22_Click(object sender, EventArgs e)  //record of escalation Reset for 9.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";    
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox39.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox11.Text) + "','" + textBox22.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }

        private void button23_Click(object sender, EventArgs e)  //record of escalation Reset for 10.NOK
        {
            GetActualSHIFT();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        DateTime time = DateTime.Now; string format = "yyyy-MM-dd HH:mm:ss";    
                        string q = "insert into NOKescalations (NumOfNOK,Operation,NOKcode,NOKdescription,Date,Line,Shift,ResetDoneBy)values('" + Convert.ToInt32(textBox41.Text) + "','" + textBox42.Text + "','" + Convert.ToInt32(textBox12.Text) + "','" + textBox23.Text + "','" + time.ToString(format) + "','" + Settings.Default.NazovZariadenia + "','" + Settings.Default.AktualnaZmena + "','" + textBox19.Text + "')";
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery(); LoadEscalationLimits(); UpdateEscalationButtonAndMessage();
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Spojenie so serverom zlyhalo, nie je možné vykonať reset eskalácie!"); }
        }
    }

}
