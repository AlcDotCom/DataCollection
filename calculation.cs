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

namespace Downtime_registration
{
    public partial class calculation : Form
    {
        public calculation()
        {
            InitializeComponent();
            slovenskyToolStripMenuItem.Checked = true;
            SK();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            slovenskyToolStripMenuItem.Image.Dispose();
            slovenskyToolStripMenuItem.Image = null;
            englishToolStripMenuItem.Image.Dispose();
            englishToolStripMenuItem.Image = null;
            Close();
        }

        private void slovenskyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slovenskyToolStripMenuItem.Checked = true;
            englishToolStripMenuItem.Checked = false;
            SK();
        }

        private void englishToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            slovenskyToolStripMenuItem.Checked = false;
            englishToolStripMenuItem.Checked = true;
            ENG();
        }
        void SK()
        {
            Text = "Výpočet OEE";
            button2.Text = "Späť";

            label1.Text = "OEE (Celková efektivita zariadenia) = Dostupnosť * Kvalita * Rýchlosť";
            label2.Text = "Dostupnosť = Čistý čas chodu zariadenia (min) / Plánovaný čas výroby (min)";
            label3.Text = "Kvalita = OK výrobky / Všetky vyrobené výrobky (OK + NOK)";
            label4.Text = "Rýchlosť = [ Ideálny čas cyklu (min) * Všetky vyrobené výrobky ]  /  Čistý čas chodu zariadenia (min)";
            label5.Text = "Plánovaný čas výroby = zvyčajne 450 min (táto hodnota je aktualizovaná odd. výroby)";
            label6.Text = "Čistý čas chodu zariadenia = Plánovaný čas výroby - Prestoje";
            label7.Text = "Prestoje = Prekážky chodu zariadenia registrované v eLSO, napríklad: poruchy, skúšky, nastavenia, extra prestávky, školenia, chýbajúci materiál atď.";
            label8.Text = "Ideálny čas cyklu = cyklus operácie/stanice, ktorá je najpomalšia na zariadení (táto hodnota je analyzovaná a aktualizovaná odd. inžinieringu)";
            label9.Text = "OEE je vypočítané a zaregistrované automaticky 5 minút po každej zmene o 14:05 - ranná zmena, 22:05 - poobedná zmena, 06:05 - nočná zmena";
        }
        void ENG()
        {
            Text = "OEE calculation";
            button2.Text = "Back";

            label1.Text = "OEE (Overall Equipment Effectiveness) = Availability * Quality * Performance";
            label2.Text = "Availability = Run time (min) / Planned production time (min)";
            label3.Text = "Quality = OK products / Total produced products (OK + NOK)";
            label4.Text = "Performance = [ Ideal cycle time (min) * Total produced products ]  /  Run time (min)";
            label5.Text = "Planned production time = usually 450 mins (this value is updated by production dep.)";
            label6.Text = "Run time = Planned production time – Downtime";
            label7.Text = "Downtimes = stoppages registered on eLSO app, for example: machine failures, dummy-test, set-up, extra breaks, trainings, missing material, etc.";
            label8.Text = "Ideal cycle time = cycle time of a bottleneck operation/station (this value is analyzed and updated by engineering dep.)";
            label9.Text = "OEE is calculated and registered automatically 5 mins after each shift at 14:05 – morning shift, 22:05 – afternoon shift, 06:05 – night shift";
        }
    }
}
