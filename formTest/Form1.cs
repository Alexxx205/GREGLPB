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
using Npgsql;
using Classes.cs;

namespace formTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

            //Mettre les informations de connexion
            string adresse = ip;
            string name = txtNomBdd.Text;
            string userId = "";
            string password = "";

            NpgsqlConnection conn = new NpgsqlConnection("Server=" + adresse + ";User Id=" + userId + ";" + "Password=" + password + ";Database=" + name + ";");

            //On crée une nouvelle importation qui va aller récupérer et instancier la liste des entreprises contenues dans le fichier csv
            Importation import = new Importation(DateTime.Now, txtFichierSource.Text);

            List<Entreprise> listEntreprises = import.GetLesEntreprises();
            foreach (Entreprise ent in listEntreprises)
            {
                ent.verifCode();
                ent.verifRaisonSoc();
                ent.verifAdresse();
                ent.verifCP();
                ent.verifVille();
                ent.verifTel();
                ent.verifFax();
                ent.verifEmail();
            }

            

        }
    }
}
