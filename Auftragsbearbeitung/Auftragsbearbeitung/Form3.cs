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
    public partial class Administration : Form
    {
        private MySqlConnection dbConnection;
        public Administration()
        {
            InitializeComponent();

            String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
            dbConnection = new MySqlConnection(ConnectionString);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Jumpy = 0;

            if (radioButton1.Checked == true && radioButton2.Checked == true)
            {
                Jumpy = 1;
                MessageBox.Show(" Bitte nur eine Aktion auswählen! ", " ...::ERROR:::::.... ");
            }
            if (radioButton1.Checked == false && radioButton2.Checked == false)
            {
                Jumpy = 1;
                MessageBox.Show(" Bitte eine Aktion auswählen! ", " ...::ERROR:::::.... ");
            }
            if (textBox1.Text != "root")
            {
                Jumpy = 1;
                MessageBox.Show(" Das Passwort ist falsch! ", " ...::ERROR:::::.... ");
            }

            if (Jumpy == 0)
            {
                // Aktion hier rein

                if (radioButton1.Checked == true)
                {
                    // Datenbank löschen

                    try
                    {
                        MySqlCommand command = dbConnection.CreateCommand();
                        command.CommandText = "DROP DATABASE if exists AuftragsbearbeitungDB;";

                        dbConnection.Open(); // Verbindung zu MySQL herstellen
                        command.ExecuteNonQuery();  // SQL Befehl ausführen - ExecuteNonQuery benutzen wenn es keine Abfrage ist.
                        dbConnection.Close(); // Verbindung zu MySQL schliessen
                        MessageBox.Show("Datenbank erfolgreich gelöscht.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if (radioButton2.Checked == true)
                {
                    // Datenbank und Tabellen erstellen
                   
                    try
                    {
                        MySqlCommand command = dbConnection.CreateCommand();
                        command.CommandText = "CREATE DATABASE if not exists AuftragsbearbeitungDB;";

                        dbConnection.Open(); // Verbindung zu MySQL herstellen
                        command.ExecuteNonQuery();

                        string sqlBefehl =
                                  "USE AuftragsbearbeitungDB;"
                                + "CREATE TABLE t_kunde ("
                                + "KuNr INTEGER PRIMARY KEY AUTO_INCREMENT,"
                                + "Firmenname VARCHAR(40),"
                                + "APVorname VARCHAR(40),"
                                + "APName VARCHAR(40),"
                                + "APGender VARCHAR(10),"
                                + "StrasseNr VARCHAR(40),"
                                + "Plz VARCHAR(10),"
                                + "Ort VARCHAR(40),"
                                + "Telefon VARCHAR(40)"
                                + ");";

                        // SQL Befehl erstellen und SQL Anweisung zuweisen:
                        MySqlCommand command2 = dbConnection.CreateCommand();
                        command2.CommandText = sqlBefehl;
                        command2.ExecuteNonQuery(); // SQL Befehl ausführen

                        string sqlBefehl2 =
                                  "USE AuftragsbearbeitungDB;"
                                + "CREATE TABLE t_artikel ("
                                + "ArtNr INTEGER PRIMARY KEY AUTO_INCREMENT,"
                                + "ArtName VARCHAR(40),"
                                + "Stueck INTEGER(40),"
                                + "Preis DECIMAL(6,2)"
                                + ");";
                        // SQL Befehl2 erstellen und SQL Anweisung zuweisen:
                        MySqlCommand command3 = dbConnection.CreateCommand();
                        command3.CommandText = sqlBefehl2;
                        command3.ExecuteNonQuery(); // SQL Befehl ausführen

                        string sqlBefehl3 =
                                  "USE AuftragsbearbeitungDB; CREATE TABLE t_angebot (AngbNr INTEGER PRIMARY KEY AUTO_INCREMENT,ArtikelNr INTEGER,KundenNr INTEGER,AngPreis DECIMAL(6,2),Stkzahl INTEGER,FOREIGN KEY (`ArtikelNr`) REFERENCES t_artikel (`ArtNr`),FOREIGN KEY (`KundenNr`) REFERENCES t_kunde (`KuNr`));";
                        
                        // SQL Befehl3 erstellen und SQL Anweisung zuweisen:
                        MySqlCommand command4 = dbConnection.CreateCommand();
                        command4.CommandText = sqlBefehl3;
                        command4.ExecuteNonQuery(); // SQL Befehl ausführen

                        string sqlBefehl5 =
                                 "USE AuftragsbearbeitungDB;CREATE TABLE t_auftrag (AuftrNr INTEGER PRIMARY KEY AUTO_INCREMENT,KundenNr INTEGER,Datum DATE,FOREIGN KEY (`KundenNr`) REFERENCES t_kunde (`KuNr`));";
                        // SQL Befehl5 erstellen und SQL Anweisung zuweisen:
                        MySqlCommand command6 = dbConnection.CreateCommand();
                        command6.CommandText = sqlBefehl5;
                        command6.ExecuteNonQuery(); // SQL Befehl ausführen

                        string sqlBefehl4 =
                                  "USE AuftragsbearbeitungDB;CREATE TABLE t_position (PosNr INTEGER PRIMARY KEY AUTO_INCREMENT,ArtikelNr INTEGER, Stueckzahl INTEGER, PosPreis DECIMAL(6,2),AuftragsNr INTEGER,FOREIGN KEY (`ArtikelNr`) REFERENCES t_artikel (`ArtNr`),FOREIGN KEY (`AuftragsNr`) REFERENCES t_auftrag (`AuftrNr`));";
                        
                        // SQL Befehl4 erstellen und SQL Anweisung zuweisen:
                        MySqlCommand command5 = dbConnection.CreateCommand();
                        command5.CommandText = sqlBefehl4;
                        command5.ExecuteNonQuery(); // SQL Befehl ausführen


                        string sqlBefehl6 =
                                  "USE AuftragsbearbeitungDB;CREATE TABLE t_rechnung (ReNr INTEGER PRIMARY KEY AUTO_INCREMENT,KundNr INTEGER,AuftragsNr INTEGER,Gesamtpreis DECIMAL(6,2),Datum DATE,FOREIGN KEY (`KundNr`) REFERENCES t_kunde (`KuNr`));";
                        
                        // SQL Befehl6 erstellen und SQL Anweisung zuweisen:
                        MySqlCommand command7 = dbConnection.CreateCommand();
                        command7.CommandText = sqlBefehl6;
                        command7.ExecuteNonQuery(); // SQL Befehl ausführen

                        dbConnection.Close(); // Verbindung zu MySQL schließen
                        MessageBox.Show("Datenbank erfolgreich erstellt.");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


                }

            }

        }
    }
}
