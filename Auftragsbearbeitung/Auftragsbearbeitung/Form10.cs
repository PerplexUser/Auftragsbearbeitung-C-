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
    public partial class Form10 : Form
    {
        private string firma;
        private int jumpy;
        private string KuNr;
        private string APVorname;
        private string APName;
        private string APGender;
        private string StrasseNr;
        private string Plz;
        private string Ort;
        private string Telefon;
        private string ArtNr;
        private string ArtName;
        private string Preis;
        private string datum;
        private string atr1 = "Artikelnummer";
        private string art2 = "Artikelname";
        private string art3 = "Preis";
        
        private MySqlConnection dbConnection;
        public Form10()
        {
            InitializeComponent();
            String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
            dbConnection = new MySqlConnection(ConnectionString);
            Kunden_laden();
            DirectoryInfo info = Directory.CreateDirectory(@"C:\\Auftragsbearbeitung");
            DirectoryInfo info1 = Directory.CreateDirectory(@"C:\\Auftragsbearbeitung\\Preislisten");
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

        private void button1_Click(object sender, EventArgs e)
        {
            // Preisliste erstellen
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Bitte Empfänger auswählen.");
                jumpy = 1;
            }


            if (jumpy == 0)
            {
                firma = listBox1.SelectedItem.ToString();
                // Kundendaten einlesen

                try
                {
                    
                    dbConnection.Open();
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = "USE AuftragsbearbeitungDB; SELECT KuNr, APVorname, APName, APGender, StrasseNr, Plz, Ort, Telefon from t_kunde WHERE Firmenname = '" + firma + "';";

                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        KuNr = reader.GetString(0);
                        APVorname = reader.GetString(1);
                        APName = reader.GetString(2);
                        APGender = reader.GetString(3);
                        StrasseNr = reader.GetString(4);
                        Plz = reader.GetString(5);
                        Ort = reader.GetString(6);
                        Telefon = reader.GetString(7);

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
                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\Auftragsbearbeitung\\Preislisten\\Preisliste_KundenNr" + KuNr + ".txt", false);
                file.WriteLine("                                                                             " + datum);
                file.WriteLine("                                                              ");
                file.WriteLine("   An                                                         ");
                file.WriteLine("   " + firma);
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
                file.WriteLine("                          ---=== PREISLISTE ===---");
                file.WriteLine("   ");
                string blaaa1 = "Artikelnummer";
                //file.WriteLine("{0, -6}{1, -2}" + "{2, -30}" + "{3, -20}", art1, art2, art3);
                file.WriteLine("   " + "{0, -30}" + "{1, -30}" + "{2, -30}", blaaa1, art2, art3);
                file.WriteLine("   ");
                file.WriteLine("________________________________________________________________________");

                // Artikel einlesen

                try
                {

                    dbConnection.Open();
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = "USE AuftragsbearbeitungDB; SELECT ArtNr, ArtName, Preis from t_artikel;";

                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        ArtNr = reader.GetString(0);
                        ArtName = reader.GetString(1);
                        Preis = reader.GetString(2);
                        //file.WriteLine("   " + ArtNr + "               " + ArtName + "         " + Preis);
                        file.WriteLine("   " + "{0, -30}" + "{1, -30}" + "{2, -30}", ArtNr, ArtName, Preis);
                    }
                    reader.Close();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                // Datei erstellt
                file.Close();
                string Programmname = "notepad.exe";
                string Parameter = "c:\\Auftragsbearbeitung\\Preislisten\\Preisliste_KundenNr" + KuNr + ".txt";
                System.Diagnostics.Process.Start(Programmname, Parameter);
                this.Close();

               

                


            }
            jumpy = 0;

        }
    }
}
