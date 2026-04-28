using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PrimeiraTela
{
    internal class conexao
    {
        private static string conect = "server=localhost;user=root;pwd=;database=sistemaconfeitaria";

        public MySqlConnection Conectar()
        {
          return new MySqlConnection(conect);

         }
    }
}
