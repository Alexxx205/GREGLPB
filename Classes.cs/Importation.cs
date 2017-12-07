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
        private DateTime dateImport;    //date de l'importation
        private string fichierImporte;  //chemin du fichier importé
        List<Entreprise> lesEntreprises;//liste contenant l'ensemble de entreprises a importer
        List<Erreur> lesErreurs;        //liste contenant l'ensemble des erreurs rencontrées lors de l'importation

        public Importation(DateTime uneDate, string unCheminFichier)
        {
            this.dateImport = uneDate;
            this.fichierImporte = unCheminFichier;
            this.lesEntreprises = new List<Entreprise>();
            this.lesErreurs = new List<Erreur>();


            //Recuperation des données contenues dans le fichier csv pour ajouter à la liste lesEntreprises

            //test avec un fichier test
            //using (var reader = new StreamReader(@"Z:\SIO2\PPE_SIO2\Contexte_INTERWAY_GEDIMAT\MissionsSLAM\Ressources\clientCSV.csv"))

            //on récupère la liste ds entreprises grace au fichier csv dont le chemin est spécifié dans l'attribut unCheminFichier
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
                bool ok; //vérificateur de doublons
                for (int i = 1; i < listCode.Count; i++)
                {
                    //Test console
                    /*Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", listCode[i], listRaisonSoc[i], listCp[i], listVille[i], listTel[i], listFax[i], listActif[i], listReglement[i]);
                    Console.WriteLine("---{0}", i);*/
                    ok = true;
                    foreach(Entreprise e in lesEntreprises)
                    {
                        if (listCode[i] == e.GetCode())
                        {
                            this.AjouterErreur(14, "Ligne en double", e.GetCode(), "code", "L'un des codes entreprise à été repéré à plusieurs reprises, les doublon n'ont pas été pris en compte");
                            ok = false;
                        }

                    }
                    if (ok)
                    {
                        //Creation de l'objet Entreprise pour l'ajouter à la liste
                        Entreprise e = new Entreprise(listCode[i], listRaisonSoc[i], listAdr[i], listCp[i], listVille[i], listTel[i], listFax[i], listMail[i], listActif[i], listReglement[i], this);
                        lesEntreprises.Add(e);
                    }

                }
            }
        }

        /// <summary>
        /// Methode qui permet d'ajouter une erreur à la liste lesErreurs
        /// </summary>
        /// <param name="unCode">Numero de l'erreur déclenchée</param>
        /// <param name="unNom">Nom de l'erreur</param>
        /// <param name="unCodeLigne">Code de la ligne de l'entreprise concernée</param>
        /// <param name="unChamp">Champ depuis lequel l'erreur provient</param>
        /// <param name="uneDesc">Description de l'erreur, informations</param>
        public void AjouterErreur(int unCode, string unNom, string unCodeLigne, string unChamp, string uneDesc)
        {
            Erreur e = new Erreur(unCode, unNom, unCodeLigne, unChamp, uneDesc);
            lesErreurs.Add(e);
        }

        /// <summary>
        /// Methode qui permet d'ajouter une erreur a la liste lesErreurs
        /// </summary>
        /// <param name="uneErreur">Objet erreur</param>
        public void AjouterErreur(Erreur uneErreur)
        {
            lesErreurs.Add(uneErreur);
        }

        /// <summary>
        /// Methode qui envoie un mail avec en piece jointe le document texte qui contient toutes les erreurs de la liste lesErreurs
        /// </summary>
        /// <param name="unePJ">Piece jointe de l'e-mail : fichier texte contenant les erreurs</param>
        public void EnvoieMail(string mailDestination, Attachment unePJ)
        {

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network; //On envoie par reseau a un servuer SMTP
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("test21101997@gmail.com", "21101997"); //Authentification pour utiliser l'adresse mail qui va envoyer le mail

            MailMessage mm = new MailMessage("test21101997@gmail.com", mailDestination); //adresse d'envoie et de destination
            mm.Subject = "Rapport d'erreur importation BDD Gedimat";             //Objet de l'e-mail
            mm.Body = "Bonjour vous trouverez ci-joint un document texte contenant l'ensemble des erreurs trouvés durant l'importation des données dansa la base.\nFichier concerné : "+this.fichierImporte+"\nDate de l'importation : "+this.dateImport.ToString();   //Contenu de l'e-mail
            Attachment attachment = unePJ;      //declaration de la piece jointe
            mm.Attachments.Add(attachment);     //ajout de la pj


            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm); //Envoie
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
