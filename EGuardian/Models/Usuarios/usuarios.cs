using System;
using SQLite;

namespace EGuardian.Models.Usuarios
{
    public class usuarios
    {
        public usuarios() { }
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string genero { get; set; }
        public string phoneNumber { get; set; }
        public bool enabled { get; set; }
        public string lastPasswordResetDate { get; set; }
        public string authority { get; set; }
        public int idPuesto { get; set; }
        public string nombrePuesto { get; set; }
        public string fechaUltimoInicio { get; set; }
    }
}