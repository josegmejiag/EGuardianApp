using System;
using Xamarin.Forms;

namespace EGuardian.Models.Eventos
{
    public class asistentes
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public Color colorAsistente 
        { 
            get 
            { 
                return genero.Equals("0") ? Color.FromHex("0C55A2") : Color.FromHex("E91B5C");
            } 
        }
        public string nombre
        {
            get
            {
                return nombres+" "+apellidos;
            }
        }
        public string genero { get; set; }
        public string email { get; set; }
        public string puesto { get; set; }
        public string rol { get; set; }
        public string Rol
        {
            get
            {
                return rol.ToUpper();
            }
        }
    }
}
