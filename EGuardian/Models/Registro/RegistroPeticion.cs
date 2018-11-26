using System;
using System.Collections.Generic;

namespace EGuardian.Models.Registro
{
    public class Signup
    {
        public string apellidosUsuario { get; set; }
        public string contrasenia { get; set; }
        public string direccionEmpresa { get; set; }
        public string email { get; set; }
        public int genero { get; set; }
        public List<int> idNegocios { get; set; }
        public string nombreEmpresa { get; set; }
        public string nombreUsuario { get; set; }
        public int numeroColaboradoresEmpresa { get; set; }
        public int puesto { get; set; }
    }
}