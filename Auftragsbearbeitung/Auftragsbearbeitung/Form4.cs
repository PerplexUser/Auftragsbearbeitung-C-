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
    public partial class Form4 : Form
    {
        private MySqlConnection dbConnection;
        public Form4()
        {
            InitializeComponent();

            String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
            dbConnection = new MySqlConnection(ConnectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ArtName;
            int Stueckzahl;
            double Preis;
            int Jumpy = 0;

            ArtName = textBox1.Text;
            Stueckzahl = Convert.ToInt32(textBox2.Text);
            // ACHTUNG!!! PREIS WIRD MAL 100 IN DIE MYSQL EINGETRAGEN
            // ACHTUNG!!! DAS MUSS BEIM AUSLESEN UND RECHNEN BEDACHT WERDEN!!!
            Preis = Convert.ToDouble(textBox3.Text)*100;
            // ACHTUNG!!!
            // ACHTUNG!!! xD

            if (ArtName == "" || textBox2.Text == "" || textBox3.Text == "")
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

                    string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; INSERT INTO t_artikel ( ArtName, Stueck, Preis) VALUES (\"{0}\", \"{1}\", \"{2}\" );", ArtName, Stueckzahl, Preis);

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
                MessageBox.Show("Artikel erfolgreich eingetragen.");
                this.Close();




            }
        }
    }
}
