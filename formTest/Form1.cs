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
            /*IPAddress adrIp;
            bool ok = IPAddress.TryParse(ip, out adrIp);
            if (ok)
                txtAdrServ.BackColor = Color.FromArgb(214, 255, 215);
            else
            {
                txtAdrServ.BackColor = Color.FromArgb(255, 196, 196);
                MessageBox.Show("Adresse IP du serveur non valide !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */

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

            //Overture de la connexion a la BDD

            //Informations de connexion
            string adresse = ip; //on prend l'adresse ip qu'on vient de vérifier
            string name = txtNomBdd.Text;
            string port = txtPortServ.Text;
            string userId = txtIdBdd.Text;
            string password = txtMdpBdd.Text;

            // Connexion à la base de données
            /*try
            {*/
            //Connexion lors des tests
            //conn = new NpgsqlConnection("Server=localhost;port=5432;User Id=openpg;password=openpgpwd;Database=Test;");

                string chaineConnex = "Server=" + ip + ";port=" + txtPortServ.Text + ";User Id=" + userId + ";password=" + password + ";Database=" + name + ";";
                conn = new NpgsqlConnection(chaineConnex);
                conn.Open();
                //conn = new NpgsqlConnection("Server=" + adresse + ";port=8069;User Id=" + userId + ";" + "Password=" + password + ";Database=" + name + ";");
                dbcmd = conn.CreateCommand();

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
                    //string req = "INSERT INTO res_partner(ref, name, street, state_id, street2, phone, fax, email, type, vat) VALUES ('"+ent.GetCode()+"','"+ent.GetRaison() + "','"+ent.GetAdresse() + "','" + ent.GetCP() + "','" + ent.GetVille() + "','" + ent.GetTel() + "','" + ent.GetFax() + "','" + ent.GetEmail() + "','" + ent.GetActif() + "','" + ent.GetReglement() +"');";
                    string req = "INSERT INTO res_partner(ref, name, street, state_id, street2, phone, fax, email, type, vat,"+
                        " company_id, create_date, color, display_name, supplier, is_company, customer, write_date, active, write_uid, lang, type, commercial_partner_id, notify_email) "+
                        "VALUES ('ABC', 'testEnt', 'adr', '38200', 'Vienne', '0203040506', '2345678901', 'mail@mail.com', 'oui', 'cheque'"+
                        "'1', "+DateTime.Now+ ", '0', 'ABC', 'false', 'true', 'true', '" + DateTime.Now + "', 'true', '1', 'fr_FR', 'contact', '2', 'always');";

                    //MessageBox.Show(ent.GetCode() + " " + ent.GetRaison() + " " + ent.GetAdresse() + " " + ent.GetCP() + " " + ent.GetVille() + " " + ent.GetTel() + " " + ent.GetFax() + " " + ent.GetEmail() + " " + ent.GetActif() + " " + ent.GetReglement()); //debug/test
                    //MessageBox.Show(ent.GetReglement()); //Testing
                    string t = "INSERT INTO public.res_partner(name, company_id, comment, function, create_date, color, date, street, city, display_name, zip, title, country_id, parent_id, supplier, ref, email, is_company, website, customer, fax, street2, employee, credit_limit, write_date, active, tz, write_uid, lang, create_uid, phone, mobile, type, use_parent_address, user_id, birthdate, vat, state_id, commercial_partner_id, notify_email, message_last_post, opt_out, signup_type, signup_expiration, signup_token, debit_limit) VALUES('"+ent.GetRaison()+"', 1, null, null, '"+DateTime.Now.ToString()+"', 0, null, '"+ent.GetAdresse()+"', '"+ent.GetVille()+"', '"+ent.GetRaison()+"', '"+ent.GetCP()+"', null, null, null, false, '"+ent.GetCode()+"', '"+ent.GetEmail()+"', true, null, true, '"+ent.GetFax()+"', null,  false, null, '"+DateTime.Now.ToString()+"', true, null, 1, 'fr_FR', 1, '"+ent.GetTel()+"', null, false, false, null, null, '"+ent.GetReglement()+"', null, null, 'always', null, false, null, null, null, 0.0);";
                    //richTxtResultat.Text = t;
                    dbcmd.CommandText = t;
                    dbcmd.ExecuteNonQuery();
                }
            /*}
            catch (NpgsqlException ex)
            {
                //MessageBox.Show("Problème d'insertion avec la base de données", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/

            /*try
            {*/
                StreamWriter writer = new StreamWriter("Test.txt"); //Chemin ou on enregistre le fichier txt
                                                                    //Ecriture du document texte contenant les erreurs
                writer.WriteLine("Bonjour :)\n\tVoici les erreurs rencontrées lors de l'importation vers la base de données :\n");
                foreach (Erreur err in import.GetLesErreurs())
                {
                    writer.WriteLine(err.ToString());
                }
                writer.Close();
                
                //Envoi du mail
                Attachment pj = new Attachment("Test.txt"); //chemin d'acces ou a été enregistré le fichier txt (voir StreamWriter ci-dessus : variable writer)
                import.EnvoieMail(txtAdrMail.Text, pj);//Appel de la methode permettant l'envoie du mail : on renseigne l'adresse mail de destination en paramètre (qui correspond au champ txtAdrMail)
            /*}
            catch(Exception er)
            {
                MessageBox.Show("Erreur : "+er.Message);
            }*/
            

        }
        
    }
}
