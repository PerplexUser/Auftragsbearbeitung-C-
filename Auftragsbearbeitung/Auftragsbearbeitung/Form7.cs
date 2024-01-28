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
    public partial class Form7 : Form
    {
        private string artikelnummer;
        private int artikelnummerleer;
        private int stueckleer;
        private string kundennummer;
        private string auftragsnummer;
        private int einauftrag = 0;
        private string firmentest;
        private string stueckzahlaustausch;
        private int lagerstueck;
        private double artikelpreis;
        private double pospreis;
        private MySqlConnection dbConnection;
        public Form7()
        {
            InitializeComponent();
            String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
            dbConnection = new MySqlConnection(ConnectionString);
            
            Kunden_lesen();
            Artikel_lesen();
            

        }

        private void Kunden_lesen()
        {
            try
            {
                listBox1.Items.Clear();
                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT Firmenname from t_kunde;";

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

        private void Artikel_lesen()
        {
            try
            {
                listBox2.Items.Clear();
                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT ArtName from t_artikel;";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    listBox2.Items.Add(reader.GetString(0));
                }
                reader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Position erstellen
            string stueck;
            string firmenname;
            string datum;

            DateTime d = DateTime.Now;
            int Tag = d.Day;
            int Monat = d.Month;
            int Jahr = d.Year;

            datum = Convert.ToString(Tag) + "." + Convert.ToString(Monat) + "." + Convert.ToString(Jahr);
            
            
            string artikelname;
            
            int jumpy = 0;
            stueck = textBox1.Text;
            int kontrollstueck = Convert.ToInt32(textBox1.Text);

            
            firmenname = listBox1.SelectedItem.ToString();
            
            artikelname = listBox2.SelectedItem.ToString();

            if (stueck == "")
            {
                jumpy = 1;
                MessageBox.Show("Bitte Stückzahl eingeben!");
            }
            if (firmenname == "")
            {
                jumpy = 1;
                MessageBox.Show("Bitte Kunde auswählen!");
            }
            if (artikelname == "")
            {
                jumpy = 1;
                MessageBox.Show("Bitte Artikel auswählen!");
            }

            if (einauftrag == 1 && firmentest != firmenname)
            {
                jumpy = 1;
                MessageBox.Show("Dieser Auftrag ist für Firma " + firmentest + " !");
                firmenname = firmentest;
            }
            // Artikelnummer holen
            try
            {

                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT ArtNr from t_artikel WHERE ArtName ='" + artikelname + "';";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    artikelnummer = reader.GetString(0);

                }
                reader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Lagerstückzahl ermitteln
            try
            {

                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT Stueck from t_artikel WHERE ArtNr ='" + artikelnummer + "';";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lagerstueck = Convert.ToInt32(reader.GetString(0));

                }
                reader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (lagerstueck < kontrollstueck)
            {
                MessageBox.Show("Der Artikel ist nicht mehr in ausreichender \n Stückzahl vorhanden. \n Bitte bestellen Sie ihn erst nach.");
                jumpy = 1;
            }

            if (lagerstueck == kontrollstueck)
            {
                MessageBox.Show("Der Artikel ist nach diesem Vorgang \n nicht mehr in ausreichender Stückzahl vorhanden. \n Bitte bestellen Sie ihn nach.");
            }




            if (jumpy == 0)
            {

                listBox1.Visible = false;
                label1.Visible = false;

                // KundenNr ermitteln
                try
                {
                    dbConnection.Open();
                    MySqlCommand commando3 = dbConnection.CreateCommand();
                    commando3.CommandText = "USE AuftragsbearbeitungDB; SELECT KuNr from t_kunde Where Firmenname = '" + firmenname + "';";

                    MySqlDataReader reader = commando3.ExecuteReader();
                    while (reader.Read())
                    {

                        kundennummer = Convert.ToString(reader.GetString(0));
                    }
                    reader.Close();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


                // Autrag in MySQL schreiben nur einmal
                if (einauftrag == 0)
                {
                    try
                    {
                        firmentest = firmenname;
                        
                        // MySQL Verbindung öffnen
                        dbConnection.Open();
                        artikelnummerleer = 0;
                        stueckleer = artikelnummerleer;
                        string sqlBefehl2 = String.Format("USE AuftragsbearbeitungDB; INSERT INTO t_auftrag ( KundenNr, Datum) VALUES (\"{0}\", \"{1}\");", kundennummer, datum);

                        // SQL Befehl erstellen und SQL Anweisung zuweisen:
                        MySqlCommand commando2 = dbConnection.CreateCommand();
                        commando2.CommandText = sqlBefehl2;

                        //dbConnection.Open(); // Verbindung zu MySQL herstellen
                        commando2.ExecuteNonQuery(); // SQL Befehl ausführen
                        einauftrag = 1;
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    dbConnection.Close();
                }


                // Artikelnummer ermitteln
                try
                {
                    dbConnection.Open();
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = "USE AuftragsbearbeitungDB; SELECT ArtNr, Preis from t_artikel Where ArtName = '" + artikelname + "';";

                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        artikelnummer = Convert.ToString(reader.GetString(0));
                        artikelpreis = Convert.ToDouble(reader.GetString(1));
                    }
                    reader.Close();
                    dbConnection.Close();
                    artikelpreis = artikelpreis / 100;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // Auftragsnummer ermitteln

                try
                {
                    dbConnection.Open();
                    MySqlCommand commando4 = dbConnection.CreateCommand();
                    commando4.CommandText = "USE AuftragsbearbeitungDB; SELECT AuftrNr from t_auftrag Where KundenNr = '" + kundennummer + "';";

                    MySqlDataReader reader = commando4.ExecuteReader();
                    while (reader.Read())
                    {

                        auftragsnummer = Convert.ToString(reader.GetString(0));
                    }
                    reader.Close();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


                pospreis = artikelpreis * Convert.ToDouble(stueck) * 100;
                // Position in MySql schreiben
                try
                {
                    // MySQL Verbindung öffnen
                    dbConnection.Open();

                    string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; INSERT INTO t_position ( ArtikelNr, Stueckzahl, PosPreis, AuftragsNr) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\" );", artikelnummer, stueck, pospreis, auftragsnummer);

                    // SQL Befehl erstellen und SQL Anweisung zuweisen:
                    MySqlCommand commando = dbConnection.CreateCommand();
                    commando.CommandText = sqlBefehl;

                    //dbConnection.Open(); // Verbindung zu MySQL herstellen
                    commando.ExecuteNonQuery(); // SQL Befehl ausführen
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dbConnection.Close();
                MessageBox.Show("Position erfolgreich dem Auftrag hinzugefügt.");

                // PositionsListBox aktualisieren
                    try
                    {
                        listBox3.Items.Clear();
                        dbConnection.Open();
                        MySqlCommand command = dbConnection.CreateCommand();
                        command.CommandText = "USE AuftragsbearbeitungDB; SELECT PosNr from t_position WHERE AuftragsNr ='" + auftragsnummer + "';";

                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {

                            listBox3.Items.Add(reader.GetString(0));
                        }
                        reader.Close();
                        dbConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                // ab hier bevor die position geschrieben wird. PROBLEM!!!
                    
                    try
                      {

                        dbConnection.Open();
                        MySqlCommand command = dbConnection.CreateCommand();
                        command.CommandText = "USE AuftragsbearbeitungDB; SELECT Stueck from t_artikel WHERE ArtNr = '" + artikelnummer + "';";

                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {

                            
                            stueckzahlaustausch = reader.GetString(0);
                            

                        }
                        reader.Close();
                        dbConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    int zahl2;
                    zahl2 = Convert.ToInt32(stueck);
                    int zahl1;
                    zahl1 = Convert.ToInt32(stueckzahlaustausch);

                    int ergebnisaustausch;
                    ergebnisaustausch = zahl1 - zahl2;

                    try
                    {
                        // MySQL Verbindung öffnen
                        dbConnection.Open();

                        string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; UPDATE t_artikel SET Stueck = \"{0}\" WHERE ArtNr = \"{1}\";", ergebnisaustausch, artikelnummer);

                        // SQL Befehl erstellen und SQL Anweisung zuweisen:
                        MySqlCommand command = dbConnection.CreateCommand();
                        command.CommandText = sqlBefehl;

                        //dbConnection.Open(); // Verbindung zu MySQL herstellen
                        command.ExecuteNonQuery(); // SQL Befehl ausführen

                        
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    dbConnection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ausgebaut
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ausgebaut
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
