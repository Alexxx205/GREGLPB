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


            //Recuperation des données contenues dans le fichier csv pour ajouter à la liste lesEntreprises

            //test avec le fichier
            //using (var reader = new StreamReader(@"Z:\SIO2\PPE_SIO2\Contexte_INTERWAY_GEDIMAT\MissionsSLAM\Ressources\clientCSV.csv"))

            using (var reader = new StreamReader(unCheminFichier))
            {
                List<string> listCode = new List<string>();
                List<string> listRaisonSoc = new List<string>();
                List<string> listAdr = new List<string>();
                List<string> listCp = new List<string>();
                List<string> listVille = new List<string>();
                List<string> listTel = new List<string>();
                List<string> listFax = new List<string>();
                List<string> listMail = new List<string>();
                List<string> listActif = new List<string>();
                List<string> listReglement = new List<string>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    listCode.Add(values[0]);
                    listRaisonSoc.Add(values[1]);
                    listAdr.Add(values[2]);
                    listCp.Add(values[3]);
                    listVille.Add(values[4]);
                    listTel.Add(values[5]);
                    listFax.Add(values[6]);
                    listMail.Add(values[7]);
                    listActif.Add(values[8]);
                    listReglement.Add(values[9]);

                }
                for (int i = 1; i < listCode.Count; i++)
                {
                    //Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", listCode[i], listRaisonSoc[i], listCp[i], listVille[i], listTel[i], listFax[i], listActif[i], listReglement[i]);
                    //Console.WriteLine("---{0}", i);

                    Entreprise e = new Entreprise(listCode[i], listRaisonSoc[i],listAdr[i], listCp[i], listVille[i], listTel[i], listFax[i],listMail[i], listActif[i], listReglement[i], this);
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

        /// <summary>
        /// Methode qui envoie un mail avec en piece jointe le document texte qui contient toutes les erreurs de la liste lesErreurs
        /// </summary>
        /// <param name="unePJ">Piece jointe de l'e-mail : fichier texte contenant les erreurs</param>
        public void EnvoieMail(Attachment unePJ)
        {

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("test21101997@gmail.com", "21101997");

            MailMessage mm = new MailMessage("test21101997@gmail.com", "test20051998@gmail.com");
            mm.Subject = "Gedimat";
            mm.Body = "Message bonjour ! :)";
            Attachment attachment = unePJ;
            mm.Attachments.Add(attachment);


            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }

        public List<Entreprise> GetLesEntreprises()
        {
            return this.lesEntreprises;
        }

        public List<Erreur> GetLesErreurs()
        {
            return this.lesErreurs;
        }

    }
}
