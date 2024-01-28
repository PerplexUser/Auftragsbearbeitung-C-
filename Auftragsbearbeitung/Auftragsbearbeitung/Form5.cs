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
    public partial class Form5 : Form
    {
        private int Edit;
        private MySqlConnection dbConnection;
        public Form5()
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
            Edit = 1;
            // Kundendaten bearbeiten - Textfelder öffnen
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox5.ReadOnly = false;
            textBox6.ReadOnly = false;
            textBox7.ReadOnly = false;
            textBox8.ReadOnly = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kundendaten speichern
            string KuNr;
            string Firma;
            string StrNr;
            string Plz;
            string Ort;
            string APGender = "Zwitter";
            string APVorname;
            string APName;
            string Telefon;
            int Jumpy = 0;

            KuNr = textBox1.Text;
            Firma = textBox2.Text;
            StrNr = textBox3.Text;
            Plz = textBox4.Text;
            Ort = textBox5.Text;
            APVorname = textBox6.Text;
            APName = textBox7.Text;
            Telefon = textBox8.Text;

            if(radioButton1.Checked == true)
            {
                APGender = "Herr";
            }
            if(radioButton2.Checked == true)
            {
                APGender = "Frau";
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
                    string sqlBefehl = String.Format("USE AuftragsbearbeitungDB; DELETE FROM t_kunde WHERE KuNr = \"{0}\";", KuNr);

                    // SQL Befehl erstellen und SQL Anweisung zuweisen:
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = sqlBefehl;

                    //dbConnection.Open(); // Verbindung zu MySQL herstellen
                    command.ExecuteNonQuery(); // SQL Befehl ausführen




                    string sqlBefehl1 = String.Format("USE AuftragsbearbeitungDB; INSERT INTO t_kunde ( KuNr, Firmenname, StrasseNr, Plz, Ort, APGender, APVorname, APName, Telefon) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\" );", KuNr, Firma, StrNr, Plz, Ort, APGender, APVorname, APName, Telefon);
                    
                    // SQL Befehl erstellen und SQL Anweisung zuweisen:
                    MySqlCommand commando = dbConnection.CreateCommand();
                    commando.CommandText = sqlBefehl1;

                    //dbConnection.Open(); // Verbindung zu MySQL herstellen
                    commando.ExecuteNonQuery(); // SQL Befehl ausführen
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                // MySQL Verbindung schließen
                dbConnection.Close();
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                textBox8.ReadOnly = true;
                MessageBox.Show("Kunde erfolgreich bearbeitet.");
                }




        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Edit == 1)
            {
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;
                textBox8.ReadOnly = true;
            }
                string Eintrag;
                string Gender;
                Eintrag = listBox1.SelectedItem.ToString();

                try
                {

                    dbConnection.Open();
                    MySqlCommand command = dbConnection.CreateCommand();
                    command.CommandText = "USE AuftragsbearbeitungDB; SELECT KuNr, Firmenname, StrasseNr, Plz, Ort, APGender, APVorname, APName, Telefon from t_kunde where Firmenname = '" + Eintrag + "';";

                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        textBox1.Text = reader.GetString(0);

                        textBox2.Text = reader.GetString(1);
                        textBox3.Text = reader.GetString(2);
                        textBox4.Text = reader.GetString(3);
                        textBox5.Text = reader.GetString(4);
                        Gender = reader.GetString(5);
                        if (Gender == "Herr")
                        {
                            radioButton1.Checked = true;
                        }
                        if (Gender == "Frau")
                        {
                            radioButton2.Checked = true;
                        }
                        textBox6.Text = reader.GetString(6);
                        textBox7.Text = reader.GetString(7);
                        textBox8.Text = reader.GetString(8);
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
