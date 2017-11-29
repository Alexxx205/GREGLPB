using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Classes.cs
{
    public class Connexion
    {
        /// <summary>
        /// Constructeur de la classe Connexion qui permet la connexion à la BDD
        /// </summary>
        /// <param name="adresse">Adresse IP du serveur</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="password">Mot de passe utilisateur</param>
        /// <param name="name">Nom de la base</param>
        /// <returns></returns>
        public static NpgsqlConnection Connect(string adresse, string userId, string password, string name)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=" + adresse + ";User Id=" + userId + ";" + "Password=" + password + ";Database=" + name + ";");
            return conn;
        }

        public Connexion()
        {

        }
    }
}
