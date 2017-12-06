using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Classes.cs
{
    public class Entreprise
    {
        //Attributs de Entreprise
        private string code;
        private string raisonSociale;
        private string adresse;
        private string cp;
        private string ville;
        private string tel;
        private string fax;
        private string eMail;
        private string actif;
        private string reglement;
        private Importation lImportation;

        /// <summary>
        /// Constructeur de la classe Entreprise
        /// </summary>
        /// <param name="unCode">Code Entreprise</param>
        /// <param name="uneRaison">Raison Sociale de l'Entreprise</param>
        /// <param name="uneAdresse">Adresse de l'entreprise</param>
        /// <param name="unCp">code postale de l'entreprise</param>
        /// <param name="uneVille">ville</param>
        /// <param name="unTel">Numero de téléphone de l'entreprise</param>
        /// <param name="unFax">Numero de Fax</param>
        /// <param name="unMail">Adresse e-mail de l'entreprise</param>
        /// <param name="uneImportation">Importation concernant l'entreprise</param>
        public Entreprise(string unCode, string uneRaison, string uneAdresse, string unCp, string uneVille, string unTel, string unFax, string unMail, string unActif,string unReglement, Importation uneImportation)
        {
            this.code = unCode;
            this.raisonSociale = uneRaison;
            this.adresse = uneAdresse;
            this.cp = unCp;
            this.ville = uneVille;
            this.tel = unTel;
            this.fax = unFax;
            this.eMail = unMail;
            this.actif = unActif;
            this.reglement = unReglement.Replace("'", "");
            this.lImportation = uneImportation;
        }

        /// <summary>
        /// Verification du code
        /// </summary>
        public void verifCode()
        {
            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$"); // pour le code : ne doit être composé que de caractères alphanumériques
            Match regexCode = regex.Match(this.code); // verifie que le code rentre dans les critères
            if (!regexCode.Success)
            {
                this.lImportation.AjouterErreur(1, "Code Entreprise incorrecte", this.code, "code",
                    "Le code de l'entreprise contient des caractères non conforme au format demandé. Veuillez insérer un code valide.");
                //this.code = "";
            }
        }

        /// <summary>
        /// Verification de la raison sociale
        /// </summary>
        public void verifRaisonSoc()
        {
            //verification : la raison sociale ne doit pas comporter plus de 3 mêmes lettres consécutives
            if (!RepetitionLettres(this.raisonSociale))
            {
                this.raisonSociale = "raisonSocNonOk";
                this.lImportation.AjouterErreur(2, "Raison sociale non valide", this.code, "raison sociale",
                            "La raison sociale de l'entreprise contient une succession de plus de 3 mêmes caractères. Veuillez insérer une raison sociale valide.");
            }
            else
            {
                this.raisonSociale = this.raisonSociale.ToUpper();

                Regex regex = new Regex("^[a-zA-Z0-9 ]*$"); // tous les caractères sont des caracteres alphanumeriques
                Match regexCode = regex.Match(this.raisonSociale); // verifie que le code rentre dans les critères
                if (!regexCode.Success)
                {
                    this.raisonSociale = "rsocNonOk";
                    this.lImportation.AjouterErreur(3, "Raison sociale non valide", this.code, "raison sociale",
                        "La raison sociale de l'entreprise est non conforme au format demandé. Veuillez insérer une raison sociale valide.");
                }
            }
        }

        /// <summary>
        /// Verification du champ adresse
        /// </summary>
        public void verifAdresse()
        {
            Regex regex = new Regex("^[a-zA-Z0-9 ]*$"); // pour l'adresse : ne doit être composée que de caractères alphanumériques
            Match regexAdr = regex.Match(this.adresse); // verifie que le code rentre dans les critères
            if (!regexAdr.Success)
            {
                this.adresse = "adresseNonOk";
                this.lImportation.AjouterErreur(4, "Adresse incorrecte", this.adresse, "adresse",
                    "L'adresse de l'entreprise contient des caractères non conforme au format demandé. Veuillez insérer une adresse valide.");
            }
        }

        /// <summary>
        /// Verification du code postal
        /// </summary>
        public void verifCP()
        {
            Regex regex = new Regex("^[0-9 ]*$"); // pour le CP
            Match match = regex.Match(this.cp); // verifie que le CP rentre dans les critères
            if (!match.Success)
            {
                this.cp = "00000";
                this.lImportation.AjouterErreur(5, "Raison sociale non valide", this.code, "raison sociale",
                                "Le code postal de l'entreprise des caractères différents de chiffres. Veuillez insérer un code postal valide.");
            }
            else
            {
                if (this.tel.Length != 10)
                {
                    this.cp = "00000";
                    this.lImportation.AjouterErreur(11, "Raison sociale non valide", this.code, "raison sociale",
                                    "Le code postal de l'entreprise ne correspond pas au format (5 chiffres). Veuillez insérer un code postal valide.");
                }
            }
        }

        /// <summary>
        /// Verification du champ ville
        /// </summary>
        public void verifVille()
        {
            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9 ]*$"); // pour l'adresse : ne doit être composée que de caractères alphanumériques
            Match regexVille = regex.Match(this.ville); // verifie que le code rentre dans les critères
            if (!regexVille.Success)
            {
                this.ville = "villeNonOk";
                this.lImportation.AjouterErreur(6, "Ville incorrecte", this.ville, "ville",
                    "La ville de l'entreprise contient des caractères non conforme au format demandé. Veuillez insérer une ville valide.");
            }
        }

        /// <summary>
        /// Verification du telephone
        /// </summary>
        public void verifTel()
        {
            this.tel = Regex.Replace(this.tel, "[^a-zA-Z0-9_]", "");
            Regex regex = new Regex("^-?\\d+${10}"); // pour le tel
            Match match = regex.Match(this.tel); // verifie que le tel rentre dans les critères
            if (!match.Success)
            {
                this.tel = "0000000000";
                lImportation.AjouterErreur(7, "Numero de téléphone non valide", this.code, "Téléphone",
                     "Le numéro de téléphone de l'entreprise ne correspond pas au format attendu. Veuillez entrer un numero de téléphone valide.");
            }
            else
            {
                if(this.tel.Length !=10)
                {
                    this.tel = "0000000000";
                    lImportation.AjouterErreur(10, "Numero de téléphone non valide", this.code, "Téléphone",
                         "Le numéro de téléphone ne comporte pas 10 chiffres. Veuillez entrer un numero de téléphone complet.");
                }
            }
        }

        /// <summary>
        /// Verification du fax
        /// </summary>
        public void verifFax()
        {
            this.fax = Regex.Replace(this.fax, "[^a-zA-Z0-9_]", "");
            Regex regex = new Regex("^-?\\d+$"); // pour le fax
            Match match = regex.Match(this.fax); // verifie que le fax rentre dans les critères
            if (!match.Success)
            {
                this.fax = "0000000000";
                this.lImportation.AjouterErreur(8, "Fax non valide", this.code, "Fax",
                    "Le fax de l'entreprise ne correspond pas au format attendu. Veuillez entrer un numero de fax valide.");
            }
            else
            {
                if (this.tel.Length != 10)
                {
                    this.tel = "0000000000";
                    lImportation.AjouterErreur(12, "Fax non valide", this.code, "Fax",
                         "Le numéro de fax ne comporte pas 10 chiffres. Veuillez entrer un numero de fax de 10 chiffres.");
                }
            }
        }

        /// <summary>
        /// Verification de l'e-mail
        /// </summary>
        public void verifEmail()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); // regex correspondant au format e-mail
            Match regexMail = regex.Match(this.eMail); // verifie que le mail rentre dans les critères
            if (!regexMail.Success)
            {
                this.eMail = "mailnonok@mail.fr";
                this.lImportation.AjouterErreur(9, "Adresse e-mail incorrecte", this.code, "e-mail", 
                    "L'adresse e-mail ne correspond pas au format exemple@ex.fr/com... Veuillez insérer une adresse e-mail valide.");
            }
        }

        /// <summary>
        /// Methode qui permet de vérifier si une chaine de caractères contient une répétition de 3 fois la même 
        /// lettre à la suite
        /// </summary>
        /// <param name="chaine">Chaine de caractères à tester</param>
        /// <returns>True si il n'y a pas de répétition de 3 lettres ou plus / False si la chaine n'est pas conforme (elle contient une répétition de 3 lettres ou plus)</returns>
        public bool RepetitionLettres(string chaine)
        {
            bool resultat = true;
            string[] myArray = new string[chaine.Count()]; //Création d'un tableau de même longueur que la chaine de caractères
            int i = 0;
            
            //On convertit la chaine de caractères en un tableau contenant chacun des chars
            foreach (char c in chaine)
            {
                myArray[i] = c.ToString();
                i++; //Equivalent de i = i + 1;
            }

            //Boucle qui va parcourir toute la chaine (sauf les 2 dernières valuers pour éviter d'être out of range)
            for (int j = 0; j < myArray.Length - 2; j++)
            {
                //Si les char j, j+1 et j+2 sont les mêmes
                if (myArray[j] == myArray[j + 1] && myArray[j + 1] == myArray[j + 2])
                    resultat = false; //Il y a une répétition de 3 lettre ou plus
            }
            return resultat;
        }
        
        public string GetCode()
        {
            return this.code;
        }
        public string GetRaison()
        {
            return this.raisonSociale;
        }
        public string GetAdresse()
        {
            return this.adresse;
        }
        public string GetCP()
        {
            return this.cp;
        }
        public string GetVille()
        {
            return this.ville;
        }
        public string GetTel()
        {
            return this.tel;
        }
        public string GetFax()
        {
            return this.fax;
        }
        public string GetEmail()
        {
            return this.eMail;
        }
        public string GetActif()
        {
            return this.actif;
        }
        public string GetReglement()
        {
            return this.reglement;
        }
        

    }
}
