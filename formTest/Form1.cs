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
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using Npgsql;
using Classes.cs;

namespace formTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Valeurs par défaut des champs
            txtAdrServ.Text = "localhost";
            txtNomBdd.Text = "Gedimat";
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string path = "";
            OpenFileDialog file = new OpenFileDialog();
            file.Filter ="CSV files (*.csv)|*.csv";
            if(file.ShowDialog() == DialogResult.OK)
            {
                path = file.FileName;
            }
            txtFichierSource.Text = path;

        }

        private void txtFichierSource_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAdrServ_TextChanged(object sender, EventArgs e)
        {
            /*string adr = txtAdrServ.Text;
            Regex expression = new Regex("/^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/");
            Match match = expression.Match(adr);
            if(match.Success)
            {
                MessageBox.Show(match.Value);
            }*/

            

        }
        
        private void btnExec_Click(object sender, EventArgs e)
        {
            //Verification du format de l'adresse ip du serveur
            string ip = txtAdrServ.Text;
            IPAddress adrIp;
            bool ok = IPAddress.TryParse(ip, out adrIp);
            if (ok)
                txtAdrServ.BackColor = Color.FromArgb(214, 255, 215);
            else
            {
                txtAdrServ.BackColor = Color.FromArgb(255, 196, 196);
                MessageBox.Show("Adresse IP du serveur non valide !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //Verification du format de l'adresse email
            Regex regexem = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); // pour l'email
            Match regexMail = regexem.Match(txtAdrMail.Text); // verifie que le mail rentre dans les critères
            if (!regexMail.Success)
            {
                MessageBox.Show("Adresse mail non valide !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAdrMail.BackColor = Color.FromArgb(255, 196, 196);
            }
            else
                txtAdrMail.BackColor = Color.FromArgb(214, 255, 215);

            
            //On crée une nouvelle importation qui va aller récupérer et instancier la liste des entreprises contenues dans le fichier csv
            Importation import = new Importation(DateTime.Now, txtFichierSource.Text);

            List<Entreprise> listEntreprises = import.GetLesEntreprises();
            foreach (Entreprise ent in listEntreprises)
            {
                //On effectue les vérifications des champs avant leur insertion dans la base de données
                ent.verifRaisonSoc();
                ent.verifAdresse();
                ent.verifCP();
                ent.verifVille();
                ent.verifTel();
                ent.verifFax();
                ent.verifEmail();
                ent.verifCode();

                //Insertion dans la base de données
                try
                {
                    //Mettre les informations de connexion
                    string adresse = ip;
                    string name = txtNomBdd.Text;
                    string userId = "openpg";
                    string password = "openpgpwd";

                    // Connexion à la base de données
                    NpgsqlConnection conn = new NpgsqlConnection("Server=" + adresse + ";port=5432;User Id=" + userId + ";" + "Password=" + password + ";Database=" + name + ";");

                    NpgsqlCommand dbcmd = conn.CreateCommand();

                    string req = "INSERT INTO res_partner(ref, name, street, state_id, street2, phone, fax, email, type, vat) VALUES ("+ent.GetCode()+","+ent.GetRaison() + ","+ent.GetAdresse() + "," + ent.GetCP() + "," + ent.GetVille() + "," + ent.GetTel() + "," + ent.GetFax() + "," + ent.GetEmail() + "," + ent.GetActif() + "," + ent.GetReglement() +");";
                    dbcmd.CommandText = req;
                    dbcmd.ExecuteNonQuery();
                }
                catch(NpgsqlException)
                {
                    MessageBox.Show("Problème d'insertion avec la base de données", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            foreach (Erreur err in import.GetLesErreurs())
            {
                StreamWriter writer = new StreamWriter("F:\\Test.txt"); //Chemin ou on enregistre le fichier txt
                //Ecriture du document texte contenant les erreurs
                writer.WriteLine("Bonjour :)\n\tVoici les erreurs rencontrées lors de l'importation vers la base de données :\n");
                writer.WriteLine(err.ToString());
                writer.Close();
            }


            //Envoi du mail
            Attachment pj = new Attachment("F:\\Test.txt"); //chemin d'acces ou a été enregistré le fichier txt (voir l.132)
            import.EnvoieMail(pj);

            

        }
    }
}
