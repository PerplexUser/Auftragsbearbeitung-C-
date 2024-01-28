using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using MySql.Data.MySqlClient;

namespace Auftragsbearbeitung
{
    public partial class Form9 : Form
    {
        private int Stueckzahl;
        private double Preis;
        private double Gesamtpreis;
        private string kundennummer;
        private string artikelnummer;
        private string firmenname;
        private string artikelname;
        private string APName;
        private string APGender;
        private string StrasseNr;
        private string Plz;
        private string Ort;
        private string datum;
        private int jumpy;
        private int lagerstueck;
        private MySqlConnection dbConnection;
        public Form9()
        {
            InitializeComponent();
            String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
            dbConnection = new MySqlConnection(ConnectionString);
            Kunden_laden();
            Artikel_laden();
            DirectoryInfo info = Directory.CreateDirectory(@"C:\\Auftragsbearbeitung");
            DirectoryInfo info1 = Directory.CreateDirectory(@"C:\\Auftragsbearbeitung\\Angebote");
        }

        private void Kunden_laden()
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

        private void Artikel_laden()
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
            // Angebot erstellen
            Stueckzahl = Convert.ToInt32(textBox1.Text);
            Preis = Convert.ToDouble(textBox2.Text);
            Gesamtpreis = Convert.ToDouble(Stueckzahl) * Preis;

            firmenname = listBox1.SelectedItem.ToString();
            artikelname = listBox2.SelectedItem.ToString();


            if (listBox1.SelectedIndex == -1 || listBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte Empfänger und Artikel auswählen.");
                jumpy = 1;
            }

            // Lagerstückzahl ermitteln
            try
            {

                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT Stueck from t_artikel WHERE ArtName ='" + artikelname + "';";

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
            // Vergleich


            if (lagerstueck <= Stueckzahl)
            {
                MessageBox.Show("Der Artikel ist nicht mehr in ausreichender \n Stückzahl vorhanden. \n Bitte bestellen Sie ihn nach.");
                jumpy = 1;
            }



            if (jumpy == 0)
            {
                // ArtikelNr ermitteln
                try
                {
                    dbConnection.Open();
                    MySqlCommand commando = dbConnection.CreateCommand();
                    commando.CommandText = "USE AuftragsbearbeitungDB; SELECT ArtNr from t_artikel Where ArtName = '" + artikelname + "';";

                    MySqlDataReader reader = commando.ExecuteReader();
                    while (reader.Read())
                    {

                        artikelnummer = Convert.ToString(reader.GetString(0));
                    }
                    reader.Close();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // KundenNr ermitteln
                try
                {
                    dbConnection.Open();
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = "USE AuftragsbearbeitungDB; SELECT KuNr from t_kunde Where Firmenname = '" + firmenname + "';";

                    MySqlDataReader reader = command.ExecuteReader();
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

                // Angebot in MySQL schreiben
                try
                {
                    // MySQL Verbindung öffnen
                    dbConnection.Open();
                    string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; INSERT INTO t_angebot ( ArtikelNr, KundenNr, AngPreis, Stkzahl) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\");", artikelnummer, kundennummer, Preis, Stueckzahl);

                    // SQL Befehl erstellen und SQL Anweisung zuweisen:
                    MySqlCommand commando = dbConnection.CreateCommand();
                    commando.CommandText = sqlBefehl;
                    commando.ExecuteNonQuery(); // SQL Befehl ausführen
                    
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                dbConnection.Close();

                // Angebot schriftlich erstellen

                // Kundendaten einlesen
                

                try
                {

                    dbConnection.Open();
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = "USE AuftragsbearbeitungDB; SELECT APName, APGender, StrasseNr, Plz, Ort from t_kunde WHERE Firmenname = '" + firmenname + "';";

                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        APName = reader.GetString(0);
                        APGender = reader.GetString(1);
                        StrasseNr = reader.GetString(2);
                        Plz = reader.GetString(3);
                        Ort = reader.GetString(4);

                    }
                    reader.Close();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DateTime d = DateTime.Now;
                int Tag = d.Day;
                int Monat = d.Month;
                int Jahr = d.Year;
                datum = Convert.ToString(Tag) + "." + Convert.ToString(Monat) + "." + Convert.ToString(Jahr);

                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Auftragsbearbeitung\\Angebote\\Angebot_KundenNr" + kundennummer + ".txt", false);
                file.WriteLine("                                                                             " + datum);
                file.WriteLine("                                                              ");
                file.WriteLine("   An                                                         ");
                file.WriteLine("   " + firmenname);
                file.WriteLine("   " + StrasseNr);
                file.WriteLine("   " + Plz + " " + Ort);
                file.WriteLine("   ");
                file.WriteLine("   z.Hd. " + APGender + " " + APName);
                file.WriteLine("   ");
                file.WriteLine("   ");
                file.WriteLine("   Von");
                file.WriteLine("   SECUDRIVE GmbH");
                file.WriteLine("   Musterstrasse 1");
                file.WriteLine("   12345 Musterhausen");
                file.WriteLine("   ");
                file.WriteLine("   ");
                file.WriteLine("   ");
                file.WriteLine("   ");
                if (APGender == "Herr")
                {
                    file.WriteLine("   Sehr geehter Herr " + APName + ".");
                }
                if (APGender == "Frau")
                {
                    file.WriteLine("   Sehr geehte Frau " + APName + ".");
                }
                file.WriteLine("   ");
                file.WriteLine("   Wie vereinbart erhalten Sie unser Exclusiv-Angebot für Bestandskunden.");
                file.WriteLine("   ");
                double gesamtausgabe;
                gesamtausgabe = Gesamtpreis / 100;
                double preisausgabe;
                preisausgabe = Preis / 100;


                file.WriteLine("   " + Stueckzahl + " Stk. " + artikelname + " für nur " + gesamtausgabe + "Euro.");
                file.WriteLine("   Das macht einen Einzelpreis von " + preisausgabe + "Euro.");
                file.WriteLine("   ");
                file.WriteLine("   Bei Interesse nutzen Sie bitte wie gewohnt unsere Kunden-Hotline 030-12345");
                file.WriteLine("   Mit freundlichen Grüßen");
                file.WriteLine("   ");
                file.WriteLine("   Ihr SECUDRIVE - Kundenservice");

                // Datei erstellt
                file.Close();
                string Programmname = "notepad.exe";
                string Parameter = "c:\\Auftragsbearbeitung\\Angebote\\Angebot_KundenNr" + kundennummer + ".txt";
                System.Diagnostics.Process.Start(Programmname, Parameter);
                this.Close();
            }
            jumpy = 0;

        }
    }
}
