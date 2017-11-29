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
        //yo
        private string code;
        private string raisonSociale;
        private string adresse;
        private string cp;
        private string ville;
        private string tel;
        private string fax;
        private string eMail;
        private Importation lImportation;

        public Entreprise(string unCode, string uneRaison, string uneAdresse, string unCp, string uneVille, string unTel, string unFax, string unMail, Importation uneImportation)
        {
            this.code = unCode;
            this.raisonSociale = uneRaison;
            this.adresse = uneAdresse;
            this.cp = unCp;
            this.ville = uneVille;
            this.tel = unTel;
            this.fax = unFax;
            this.eMail = unMail;
            this.lImportation = uneImportation;
        }

        /// <summary>
        /// Verification de l'e-mail
        /// </summary>
        public void verifEmail()
        {
            Regex regexem = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); // regex correspondant au format e-mail
            Match regexMail = regexem.Match(this.eMail); // verifie que le mail rentre dans les critères
            if (!regexMail.Success)
            {
                this.eMail = "";
                this.lImportation.AjouterErreur(1, "Adresse e-mail incorrecte", this.code, "e-mail", "L'adresse e-mail ne correspond pas au format exemple@ex.fr/com... Veuillez en insérer une valide.");
            }
        }

        /// <summary>
        /// Verification du code
        /// </summary>
        public void verifCode()
        {
            Regex regexem = new Regex("^[a-zA-Z][a-zA-Z0-9]*$"); // pour le code : ne doit être composé que de caractères alphanumériques
            Match regexCode = regexem.Match(this.code); // verifie que le code rentre dans les critères
            if (!regexCode.Success)
            {
                this.code = "";
                this.lImportation.AjouterErreur(2, "Code Entreprise incorrecte", this.code, "code", "Le code de l'entreprise contient des caractères non conforme au format demandé. Veuillez insérer un code valide");
            }
        }

        /// <summary>
        /// Verification du fax
        /// </summary>
        public void verifFax()
        {
            Regex regex = new Regex("^-?\\d+$"); // pour le fax
            Match match = regex.Match(this.fax); // verifie que le fax rentre dans les critères
            if (!match.Success)
                this.fax = "";
            this.lImportation.AjouterErreur(3, "Fax non valide", this.code, "Fax", "Le fax de l'entreprise ne correspond pas au format attendu.");

        }

        /// <summary>
        /// Verification du telephone
        /// </summary>
        public void verifTel()
        {
            Regex regex = new Regex("^-?\\d+${10}"); // pour le tel
            Match match = regex.Match(this.tel); // verifie que le tel rentre dans les critères
            if (!match.Success)
                this.tel = "";

        }

        /// <summary>
        /// Verification du code postal
        /// </summary>
        public void verifCP()
        {
            Regex regex = new Regex("^-?\\d+$"); // pour le CP
            Match match = regex.Match(this.cp); // verifie que le CP rentre dans les critères
            if (!match.Success)
                this.cp = "";
        }

        /// <summary>
        /// Methode qui permet de vérifier si une chaine de caractères contient une répétition de 3 fois la même 
        /// lettre à la suite
        /// </summary>
        /// <param name="chaine">Chaine de caractères à tester</param>
        /// <returns>True si il n'y a pas de répétition de 3 lettres ou plus / False si la chaine n'est pas conforme (elle contient une répétition de 3 lettres ou plus)</returns>
        public bool repetitionLettres(string chaine)
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

        /// <summary>
        /// Verification de la raison sociale
        /// </summary>
        public void verifRaisonSoc()
        {
            //verification : la raison sociale ne doit pas comporter plus de 3 mêmes lettres consécutives
            if (!repetitionLettres(this.raisonSociale))
                this.raisonSociale = "";
            else
            {
                this.raisonSociale = this.raisonSociale.ToUpper();

                Regex regexem = new Regex("/^[ A-Za-z0-9_@./#&+-]*$/"); // tous les caractères sont des lettres
                Match regexCode = regexem.Match(this.raisonSociale); // verifie que le code rentre dans les critères
                if (!regexCode.Success)
                {
                    this.raisonSociale = "";
                }
            }
        }

        public void verifAdresse()
        {

        }

        public void verifVille()
        {

        }

        public string getEmail()
        {
            return this.eMail;
        }
        public string getCode()
        {
            return this.code;
        }
        public string getRaison()
        {
            return this.raisonSociale;
        }
        public string getCP()
        {
            return this.cp;
        }

    }
}
