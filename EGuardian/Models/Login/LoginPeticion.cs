using System;
using System.Collections.Generic;
using System.Text;

namespace EGuardian.Models.Login
{
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public user user { get; set; }
        public empresa empresa { get; set; }
        public puesto puesto { get; set; }
    }

    public class user
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string genero { get; set; }
        public string phoneNumber { get; set; }
        public bool enabled { get; set; }
        public string lastPasswordResetDate { get; set; }
        public List<authorities> authorities { get; set; }
    }

    public class empresa
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public int numeroColaboradores { get; set; }
        public string telefono { get; set; }
        public string logo { get; set; }
        public string descripcion { get; set; }
        public int status { get; set; }
    }

    public class puesto
    {
        public int idPuesto { get; set; }
        public string nombre { get; set; }
    }

    public class authorities
    {
        public string authority { get; set; }
    }
}