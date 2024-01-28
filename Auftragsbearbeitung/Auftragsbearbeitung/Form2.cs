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
    public partial class Neukunde : Form
    {
        private MySqlConnection dbConnection;
        public Neukunde()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Firma;
            string StrNr;
            string Plz;
            string Ort;
            string Gender = "Zwitter";
            string APVorname;
            string APName;
            string Telefon;
            int Jumpy = 0;

            Firma = textBox1.Text;
            StrNr = textBox2.Text;
            Plz = textBox3.Text;
            Ort = textBox4.Text;
            APVorname = textBox5.Text;
            APName = textBox6.Text;
            Telefon = textBox7.Text;

            if(radioButton1.Checked == true)
            {
                Gender = "Herr";
            }
            if(radioButton2.Checked == true)
            {
                Gender = "Frau";
            }

            if (radioButton1.Checked == true && radioButton2.Checked == true)
            {
                Jumpy = 1;
                MessageBox.Show(" Bitte Herr ODER Frau auswählen! ", " ...::ERROR:::::.... ");
            }

            if (radioButton1.Checked == false && radioButton2.Checked == false)
            {
                Jumpy = 1;
                MessageBox.Show(" Bitte Herr oder Frau auswählen! ", " ...::ERROR:::::.... ");
            }


            if (Jumpy == 0)
            {
                String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
                dbConnection = new MySqlConnection(ConnectionString);
                try
                {
                    // MySQL Verbindung öffnen
                    dbConnection.Open();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                try
                {
                    string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; INSERT INTO t_kunde ( Firmenname, StrasseNr, Plz, Ort, APGender, APVorname, APName, Telefon) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\" );", Firma, StrNr, Plz, Ort, Gender, APVorname, APName, Telefon);

                    // SQL Befehl erstellen und SQL Anweisung zuweisen:
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = sqlBefehl;

                    //dbConnection.Open(); // Verbindung zu MySQL herstellen
                    command.ExecuteNonQuery(); // SQL Befehl ausführen
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                // MySQL Verbindung schließen
                dbConnection.Close();
                MessageBox.Show("Neukunde erfolgreich eingetragen.");
                this.Close();
            }
        }

    }
}
