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
    public partial class Form11 : Form
    {
        private string auftragsnummer;
        private string kundennummer;
        private string kundenname;
        private string datum;
        private string APName;
        private string APGender;
        private string StrasseNr;
        private string Plz;
        private string Ort;

        private string artikelnummer;
        private double pospreis;
        private string stueckzahl;
        private double gesamtpreis;

        private int jumpy;

        private MySqlConnection dbConnection;
        public Form11()
        {
            InitializeComponent();
            String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
            dbConnection = new MySqlConnection(ConnectionString);
            Auftrag_laden();
            DirectoryInfo info = Directory.CreateDirectory(@"C:\\Auftragsbearbeitung");
            DirectoryInfo info1 = Directory.CreateDirectory(@"C:\\Auftragsbearbeitung\\Rechnungen");
        }

        private void Auftrag_laden()
        {
            try
            {
                listBox1.Items.Clear();
                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT AuftrNr from t_auftrag;";

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

        private void button1_Click(object sender, EventArgs e)
        {
            // Rechnung erstellen
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte Empfänger auswählen.");
                jumpy = 1;
            }

            if (jumpy == 0)
            {
                // Datum ermitteln
                DateTime d = DateTime.Now;
                int Tag = d.Day;
                int Monat = d.Month;
                int Jahr = d.Year;
                datum = Convert.ToString(Tag) + "." + Convert.ToString(Monat) + "." + Convert.ToString(Jahr);

                // Rechnung schriftlich Teil1
                try
                {

                    dbConnection.Open();
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = "USE AuftragsbearbeitungDB; SELECT APName, APGender, StrasseNr, Plz, Ort from t_kunde WHERE Firmenname = '" + kundenname + "';";

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

                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Auftragsbearbeitung\\Rechnungen\\Rechnung_KundenNr" + kundennummer + ".txt", false);
                file.WriteLine("                                                                             " + datum);
                file.WriteLine("                                                              ");
                file.WriteLine("   An                                                         ");
                file.WriteLine("   " + kundenname);
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
                file.WriteLine("   Wie vereinbart erhalten Sie die Rechnung für ihren Auftrag mit der AuftragsNr: " + auftragsnummer);

                // Preis und Gesamtpreis ermitteln
                try
                {
                    dbConnection.Open();
                    MySqlCommand commando = dbConnection.CreateCommand();
                    commando.CommandText = "USE AuftragsbearbeitungDB; SELECT ArtikelNr, Stueckzahl, PosPreis from t_position Where AuftragsNr = '" + auftragsnummer + "';";

                    MySqlDataReader reader = commando.ExecuteReader();
                    while (reader.Read())
                    {

                        artikelnummer = Convert.ToString(reader.GetString(0));
                        stueckzahl = Convert.ToString(reader.GetString(1));
                        pospreis = Convert.ToDouble(reader.GetString(2));
                        pospreis = pospreis / 100;
                        // in Rechnung schreiben
                        file.WriteLine("   ");
                        file.WriteLine("   ArtikelNr: " + artikelnummer + "     Stückzahl: " + stueckzahl + "     Positionspreis: " + pospreis + " Euro");
                        
                        gesamtpreis = gesamtpreis + pospreis;

                    }
                    reader.Close();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                file.WriteLine("   ");
                file.WriteLine("   ");
                file.WriteLine("   Der Gesamtpreis ihrer Bestellung liegt somit bei " + gesamtpreis + " Euro,");
                file.WriteLine("   ");
                file.WriteLine("   bitte überweisen sie den Betrag innerhalb der nächsten 14 Tagen ");
                file.WriteLine("   auf unser Firmenkonto: IBAN DE69 5012 1300 0000 0569 78 BIC GENODE51BH2 ");
                file.WriteLine("   ");
                file.WriteLine("   Bei weiteren Wünschen und Fragen nutzen Sie bitte wie ");
                file.WriteLine("   gewohnt unsere Kunden-Hotline 030-12345");
                file.WriteLine("   Mit freundlichen Grüßen");
                file.WriteLine("   ");
                file.WriteLine("   Ihr SECUDRIVE - Kundenservice");

                // Datei erstellt
                file.Close();
                string Programmname = "notepad.exe";
                string Parameter = "c:\\Auftragsbearbeitung\\Rechnungen\\Rechnung_KundenNr" + kundennummer + ".txt";
                System.Diagnostics.Process.Start(Programmname, Parameter);
                

                // Rechnung in MySQL eintragen
                try
                {
                    gesamtpreis = gesamtpreis * 100;
                    // MySQL Verbindung öffnen
                    dbConnection.Open();
                    string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; INSERT INTO t_rechnung ( KundNr, AuftragsNr, Gesamtpreis, Datum) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\");", kundennummer, auftragsnummer, gesamtpreis , datum);
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

                // Einträge des Auftrags löschen
                
                try
                {

                    // MySQL Verbindung öffnen
                    dbConnection.Open();

                    string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; DELETE FROM t_position WHERE AuftragsNr = \"{0}\";", auftragsnummer);

                    // SQL Befehl erstellen und SQL Anweisung zuweisen:
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = sqlBefehl;

                    //dbConnection.Open(); // Verbindung zu MySQL herstellen
                    command.ExecuteNonQuery(); // SQL Befehl ausführen


                    string sqlBefehl1 = String.Format("USE AuftragsbearbeitungDB; DELETE FROM t_auftrag WHERE AuftrNr = \"{0}\";", auftragsnummer);

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

                this.Close();
            }
            jumpy = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            auftragsnummer = listBox1.SelectedItem.ToString();

            // kundennummer ermitteln
            try
            {
                
                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT KundenNr from t_auftrag WHERE AuftrNr ='" + auftragsnummer + "';";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    kundennummer = reader.GetString(0);

                }
                reader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // kundenname ermitteln
            try
            {

                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT Firmenname from t_kunde WHERE KuNr ='" + kundennummer + "';";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    kundenname = reader.GetString(0);
                    textBox1.Text = reader.GetString(0);

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
