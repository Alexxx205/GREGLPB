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
        NpgsqlConnection conn;
        NpgsqlCommand dbcmd;
        Importation import;
        public Form1()
        {
            InitializeComponent();

            //Valeurs par défaut des champs
            txtAdrServ.Text = "localhost";  // a remplacer par l'adresse du serveur des réseaux
            txtNomBdd.Text = "Gedimat";
            txtAdrMail.Text = "test20051998@gmail.com";
            txtPortServ.Text = "5432";
            txtIdBdd.Text = "openpg";
            txtMdpBdd.Text = "openpgpwd";

            richTxtResultat.Text = ""; //On initialise a vide pour pouvoir utiliser += par la suite
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
            //--------------------------------------------------------
            // Verifications des champs remplis par l'utilisateur
            //--------------------------------------------------------
            #region Verification des champs saisis par l'utilisateur

            bool verifChampsSaisis = true;
            //Verif que le fichier source existe
            if (!File.Exists(txtFichierSource.Text))
            {
                MessageBox.Show("Fichier introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFichierSource.BackColor = Color.FromArgb(214, 255, 215);
                verifChampsSaisis = false;
            }

            //Verification du format de l'adresse ip du serveur
            string ip = txtAdrServ.Text;
            IPAddress adrIp;
            bool ok = IPAddress.TryParse(ip, out adrIp);
            if (!ok)
            {
                txtAdrServ.BackColor = Color.FromArgb(255, 196, 196);
                MessageBox.Show("Adresse IP du serveur non valide !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                verifChampsSaisis = false;
            }
            else
            {
                txtAdrServ.BackColor = Color.FromArgb(214, 255, 215);
            }

            //Verification du port du serveur (doit etre composé de chiffres uniquement)
            Regex r = new Regex("^[0-9]*$");
            Match regPortServ = r.Match(txtPortServ.Text);
            if (!regPortServ.Success)
                verifChampsSaisis = false;

            //Verification du format de l'adresse email
            Regex regexem = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); // pour l'email
            Match regexMail = regexem.Match(txtAdrMail.Text); // verifie que le mail rentre dans les critères
            if (!regexMail.Success)
            {
                MessageBox.Show("Adresse mail non valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAdrMail.BackColor = Color.FromArgb(255, 196, 196);
                verifChampsSaisis = false;
            }
            else
                txtAdrMail.BackColor = Color.FromArgb(214, 255, 215);

            #endregion


            //--------------------------------------------------------
            // Envoie des informations dans la BDD
            //--------------------------------------------------------
            if (verifChampsSaisis)
            {
                #region Connexion BDD
                //Overture de la connexion a la BDD

                //Informations de connexion
                string adresse = ip; //on prend l'adresse ip qu'on vient de vérifier
                string name = txtNomBdd.Text;
                string port = txtPortServ.Text;
                string userId = txtIdBdd.Text;
                string password = txtMdpBdd.Text;

                // Connexion à la base de données
                try
                {
                    //Connexion lors des tests
                    //conn = new NpgsqlConnection("Server=localhost;port=5432;User Id=openpg;password=openpgpwd;Database=Test;");

                    string chaineConnex = "Server=" + ip + ";port=" + txtPortServ.Text + ";User Id=" + userId + ";password=" + password + ";Database=" + name + ";";
                    conn = new NpgsqlConnection(chaineConnex);
                    conn.Open();
                    //conn = new NpgsqlConnection("Server=" + adresse + ";port=8069;User Id=" + userId + ";" + "Password=" + password + ";Database=" + name + ";");
                    dbcmd = conn.CreateCommand();

                    //Feedback utilisateur connexion reussie
                    richTxtResultat.Text += "Connexion BDD réussie !\n";

                    #endregion

                    #region Envoie vers la BDD

                    //On crée une nouvelle importation qui va aller récupérer et instancier la liste des entreprises contenues dans le fichier csv
                    import = new Importation(DateTime.Now, txtFichierSource.Text);

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
                        
                        string t = "INSERT INTO public.res_partner(name, company_id, comment, function, create_date, color, date, street, city, display_name, zip, title, country_id, parent_id, supplier, ref, email, is_company, website, customer, fax, street2, employee, credit_limit, write_date, active, tz, write_uid, lang, create_uid, phone, mobile, type, use_parent_address, user_id, birthdate, vat, state_id, commercial_partner_id, notify_email, message_last_post, opt_out, signup_type, signup_expiration, signup_token, debit_limit) VALUES('" + ent.GetRaison() + "', 1, null, null, '" + DateTime.Now.ToString() + "', 0, null, '" + ent.GetAdresse() + "', '" + ent.GetVille() + "', '" + ent.GetRaison() + "', '" + ent.GetCP() + "', null, null, null, false, '" + ent.GetCode() + "', '" + ent.GetEmail() + "', true, null, true, '" + ent.GetFax() + "', null,  false, null, '" + DateTime.Now.ToString() + "', true, null, 1, 'fr_FR', 1, '" + ent.GetTel() + "', null, false, false, null, null, '" + ent.GetReglement() + "', null, null, 'always', null, false, null, null, null, 0.0);";
                        //richTxtResultat.Text = t; //Debug
                        dbcmd.CommandText = t;
                        dbcmd.ExecuteNonQuery();

                        //Feedback utilisateur
                        richTxtResultat.Text += "Insertions dans la base réussie !\n";

                        #endregion
                    }
                }
                catch (NpgsqlException)
                {
                    MessageBox.Show("Problème d'insertion avec la base de données", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                try
                {
                    #region Generation du document contenant les erreurs

                    StreamWriter writer = new StreamWriter("Erreurs.txt"); //Chemin ou on enregistre le fichier txt
                                                                        //Ecriture du document texte contenant les erreurs
                    writer.WriteLine("Bonjour\n\tVoici les erreurs rencontrées lors de l'importation vers la base de données :\n");
                    foreach (Erreur err in import.GetLesErreurs())
                    {
                        writer.WriteLine(err.ToString());
                    }
                    writer.Close();
                    //Feedback utilisateur
                    richTxtResultat.Text += "Document généré\n";
                    #endregion

                    #region Envoie du mail contenant le rapport d'erreur

                    //Envoi du mail
                    Attachment pj = new Attachment("Erreurs.txt"); //chemin d'acces ou a été enregistré le fichier txt (voir StreamWriter ci-dessus : variable writer)
                    import.EnvoieMail(txtAdrMail.Text, pj);//Appel de la methode permettant l'envoie du mail : on renseigne l'adresse mail de destination en paramètre (qui correspond au champ txtAdrMail)

                    #endregion
                }
                catch (Exception er)
                {
                    MessageBox.Show("Erreur : "+er.Message);
                }

            }//End if(verifChampsSaisis)

        } //End btnExecClick
        
    }
}
