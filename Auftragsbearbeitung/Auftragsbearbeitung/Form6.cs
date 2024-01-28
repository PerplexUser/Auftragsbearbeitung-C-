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
    public partial class Form6 : Form
    {
        private int Edit;
        private MySqlConnection dbConnection;
        public Form6()
        {
            InitializeComponent();
            String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
            dbConnection = new MySqlConnection(ConnectionString);
            Daten_lesen();
        }

        private void Daten_lesen()
        {
            try
            {
                listBox1.Items.Clear();
                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT ArtName from t_artikel;";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    listBox1.Items.Add(reader.GetString(0));
                }
                reader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Artikel speichern
            string ArtNr;
            string ArtName;
            int Stueckzahl;
            double Preis;
            int Jumpy = 0;
            
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;

            ArtNr = textBox1.Text;
            ArtName = textBox2.Text;
            Stueckzahl = Convert.ToInt32(textBox3.Text);
            // ACHTUNG!!! PREIS WIRD MAL 100 IN DIE MYSQL EINGETRAGEN
            // ACHTUNG!!! DAS MUSS BEIM AUSLESEN UND RECHNEN BEDACHT WERDEN!!!
            Preis = Convert.ToDouble(textBox4.Text) * 100;
            // ACHTUNG!!!
            // ACHTUNG!!! xD

            if (ArtName == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Bitte Felder ausfüllen.", "...:: ERROR:::::...");
                Jumpy = 1;
            }
            if (Jumpy == 0)
            {
                try
                {
                    // MySQL Verbindung öffnen
                    dbConnection.Open();

                    string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; DELETE FROM t_artikel WHERE ArtNr = \"{0}\";", ArtNr);

                    // SQL Befehl erstellen und SQL Anweisung zuweisen:
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = sqlBefehl;

                    //dbConnection.Open(); // Verbindung zu MySQL herstellen
                    command.ExecuteNonQuery(); // SQL Befehl ausführen





                    string sqlBefehl1 = String.Format("USE AuftragsbearbeitungDB; INSERT INTO t_artikel ( ArtNr, ArtName, Stueck, Preis) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\" );", ArtNr, ArtName, Stueckzahl, Preis);

                    // SQL Befehl erstellen und SQL Anweisung zuweisen:
                    MySqlCommand commando = dbConnection.CreateCommand();
                    commando.CommandText = sqlBefehl1;

                    //dbConnection.Open(); // Verbindung zu MySQL herstellen
                    commando.ExecuteNonQuery(); // SQL Befehl ausführen
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dbConnection.Close();
                MessageBox.Show("Artikel erfolgreich bearbeitet.");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Artikel bearbeiten
            Edit = 1;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Edit == 1)
            {   
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;   
            }
            string Eintrag;
            double Preis;
            
            Eintrag = listBox1.SelectedItem.ToString();

            try
            {

                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT ArtNr, ArtName, Stueck, Preis from t_artikel where ArtName = '" + Eintrag + "';";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    textBox1.Text = reader.GetString(0);
                    textBox2.Text = reader.GetString(1);
                    textBox3.Text = reader.GetString(2);
                    Preis = Convert.ToDouble(reader.GetString(3))/100;
                    textBox4.Text = Convert.ToString(Preis);
                }

                reader.Close();
                dbConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
