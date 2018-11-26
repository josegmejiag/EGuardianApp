using System;
using Xamarin.Forms;
using SQLite;

namespace EGuardian.Models.Empleados
{
    public class empleados
    {
        public empleados() { }
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int idEmpleado { get; set; }
        public int idUsuario { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public Color colorAsistente
        {
            get
            {
                return genero.Equals("1") ? Color.FromHex("E91B5C") : Color.FromHex("0C55A2");
            }
        }
        public string lastName { get; set; }
        public string nombre
        {
            get
            {
                return firstName + " " + lastName;
            }
        }
        public string email { get; set; }
        public int genero { get; set; }
        public string phoneNumber { get; set; }
        public bool enabled { get; set; }
        public string lastPasswordResetDate { get; set; }
        public string authority { get; set; }
        public int idPuesto { get; set; }
        public string nombrePuesto { get; set; }
        public string rol { get; set; }
        public string puesto
        {
            get
            {
                return nombrePuesto;
            }
        }
    }
}
