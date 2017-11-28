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

        public Erreur(int unCode, string unNom, string unCodeLigne, string unChamp, string uneDesc)
        {
            this.codeErreur = unCode;
            this.nom = unNom;
            this.codeLigneConcernee = unCodeLigne;
            this.champConcerne = unChamp;
            this.description = uneDesc;
        }

        public override string ToString()
        {
            string strErreur = "Erreur " + this.codeErreur + " : " + this.nom + " pour l'entreprise ayant le code : " + this.codeLigneConcernee
                + " concernant le champ : " + this.champConcerne + ".\nDescription :" + this.description + "\n\n";
            return strErreur;
        }

    }
}
