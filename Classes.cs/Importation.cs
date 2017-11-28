using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

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


        public void AjouterEntreprise(Entreprise uneEntreprise)
        {
            lesEntreprises.Add(uneEntreprise);
        }

        public void AjouterEntreprise(string unCode, string uneRaisonSociale, string uneAdresse, string unCp, 
            string uneVille, string unTel, string unFax, string unMail)
        {
            Entreprise e = new Entreprise(unCode, uneRaisonSociale, uneAdresse, unCp, uneVille, unTel, unFax, unMail);
            lesEntreprises.Add(e);
        }

        //Methode qui envoie un mail avec en piece jointe le document texte qui contient toutes les erreurs de la liste lesErreurs
        public void EnvoieMail()
        {
            MailMessage mail = new MailMessage("you@yourcompany.com", "user@hotmail.com");
            SmtpClient client = new SmtpClient();

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
    }
}
