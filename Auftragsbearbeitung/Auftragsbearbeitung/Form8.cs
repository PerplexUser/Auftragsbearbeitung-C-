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
    public partial class Form8 : Form
    {
        private string kundennummer;
        private string auftragsnummer;
        private string firmenname;
        private string datum;
        private MySqlConnection dbConnection;
        public Form8()
        {
            InitializeComponent();
            String ConnectionString = string.Format("SERVER=localhost;UID=root;Password=root;");
            dbConnection = new MySqlConnection(ConnectionString);
            Auftrag_lesen();
        }


        private void Auftrag_lesen()
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
            // STORNIEREN
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
                Auftrag_lesen();
                MessageBox.Show("Auftrag wurde gelöscht.");
                this.Close();



        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            auftragsnummer = listBox1.SelectedItem.ToString();
         
            try
            {
                
                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT KundenNr, Datum from t_auftrag WHERE AuftrNr = '" + auftragsnummer + "';";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    kundennummer = reader.GetString(0);
                    datum = reader.GetString(1);
                }
                reader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            textBox2.Text = kundennummer;
            textBox3.Text = datum.Substring(0,10);

            try
            {

                dbConnection.Open();
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = "USE AuftragsbearbeitungDB; SELECT Firmenname from t_kunde WHERE KuNr = '" + kundennummer + "';";

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    firmenname = reader.GetString(0);
                    
                }
                reader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            textBox1.Text = firmenname;



        }
    }
}
