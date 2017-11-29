using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;

namespace Classes.cs
{
    public class Importation
    {
        private DateTime dateImport;
        private string fichierImporte;
        List<Entreprise> lesEntreprises;
        List<Erreur> lesErreurs;

        public Importation(DateTime uneDate, string unCheminFichier)
        {
            this.dateImport = uneDate;
            this.fichierImporte = unCheminFichier;
            this.lesEntreprises = new List<Entreprise>();
            this.lesErreurs = new List<Erreur>();


            //Recuperation des données contenues dans le fichier csv

            //test avec le fichier
            //using (var reader = new StreamReader(@"Z:\SIO2\PPE_SIO2\Contexte_INTERWAY_GEDIMAT\MissionsSLAM\Ressources\clientCSV.csv"))

            using (var reader = new StreamReader(unCheminFichier))
            {
                List<string> listCode = new List<string>();
                List<string> listRaisonSoc = new List<string>();
                List<string> listCp = new List<string>();
                List<string> listVille = new List<string>();
                List<string> listTel = new List<string>();
                List<string> listFax = new List<string>();
                List<string> listActif = new List<string>();
                List<string> listReglement = new List<string>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    listCode.Add(values[0]);
                    listRaisonSoc.Add(values[1]);
                    listCp.Add(values[2]);
                    listVille.Add(values[3]);
                    listTel.Add(values[4]);
                    listFax.Add(values[5]);
                    listActif.Add(values[6]);
                    listReglement.Add(values[7]);

                }
                for (int i = 1; i < listCode.Count; i++)
                {
                    //Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", listCode[i], listRaisonSoc[i], listCp[i], listVille[i], listTel[i], listFax[i], listActif[i], listReglement[i]);
                    //Console.WriteLine("---{0}", i);

                    Entreprise e = new Entreprise(listCode[i], listRaisonSoc[i], listCp[i], listVille[i], listTel[i], listFax[i], listActif[i], listReglement[i], this);
                    lesEntreprises.Add(e);
                }
            }

        }

        public void AjouterErreur(int unCode, string unNom, string unCodeLigne, string unChamp, string uneDesc)
        {
            Erreur e = new Erreur(unCode, unNom, unCodeLigne, unChamp, uneDesc);
            lesErreurs.Add(e);
        }

        public void AjouterErreur(Erreur uneErreur)
        {
            lesErreurs.Add(uneErreur);
        }

        //Methode qui envoie un mail avec en piece jointe le document texte qui contient toutes les erreurs de la liste lesErreurs
        public void EnvoieMail()
        {
            MailMessage mail = new MailMessage("you@yourcompany.com", "user@hotmail.com");
            SmtpClient client = new SmtpClient();

            //A modifier
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.google.com";

            mail.Subject = "this is a test email.";
            mail.Body = "this is my test email body";

            Attachment fichier = new Attachment("F:\\Test.txt");
            mail.Attachments.Add(fichier);
            client.Send(mail);
        }

        public List<Entreprise> GetLesEntreprises()
        {
            return this.lesEntreprises;
        }
    }
}
