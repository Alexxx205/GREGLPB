using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes.cs
{
    public class Erreur
    {
        private int codeErreur;
        private string nom;
        private string codeLigneConcernee;
        private string champConcerne;
        private string description;

        /// <summary>
        /// Contructeur
        /// </summary>
        /// <param name="unCode">Code de l'erreur</param>
        /// <param name="unNom">Nom de l'erreur</param>
        /// <param name="unCodeLigne">Code de l'entreprise concernée par l'erreur</param>
        /// <param name="unChamp">Champ concerné par l'erreur</param>
        /// <param name="uneDesc">Description de l'erreur</param>
        public Erreur(int unCode, string unNom, string unCodeLigne, string unChamp, string uneDesc)
        {
            this.codeErreur = unCode;
            this.nom = unNom;
            this.codeLigneConcernee = unCodeLigne;
            this.champConcerne = unChamp;
            this.description = uneDesc;
        }

        /// <summary>
        /// Methode qui permet de générer la chaine concernant une erreur (pour fichier erreur)
        /// </summary>
        /// <returns>Chaine erreur</returns>
        public override string ToString()
        {
            string strErreur = "Erreur type " + this.codeErreur + " : " + this.nom + " pour l'entreprise ayant le code : " + this.codeLigneConcernee
                + " concernant le champ : " + this.champConcerne + ".\nDescription :" + this.description + "\n\n";
            return strErreur;
        }

    }
}
