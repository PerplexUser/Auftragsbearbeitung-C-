using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Auftragsbearbeitung
{
    public partial class Auftragsbearbeitung : Form
    {
        
        public Auftragsbearbeitung()
        {
            InitializeComponent();

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Kundendaten bearbeiten
            Form5 frm5 = new Form5();
            frm5.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Neukunden aufnehmen
            Neukunde frm2 = new Neukunde();
            frm2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Angebot versenden
            Form9 frm9 = new Form9();
            frm9.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Preisliste versenden
            Form10 frm10 = new Form10();
            frm10.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Auftrag stornieren
            Form8 frm8 = new Form8();
            frm8.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Auftrag erfassen
            Form7 frm7 = new Form7();
            frm7.Show();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Rechnung versenden
            Form11 frm11 = new Form11();
            frm11.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Artikel erfassen
            Form4 frm4 = new Form4();
            frm4.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Artikelliste bearbeiten
            Form6 frm6 = new Form6();
            frm6.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Datenbank Administration
            Administration frm3 = new Administration();
            frm3.Show();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Button verworfen
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Für Support & Fragen: lappi777@gmail.com");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Angebotsannahme
            Form12 frm12 = new Form12();
            frm12.Show();
        }
    }
}
