using System;
using System.Collections.Generic;

namespace EGuardian.Models.Empleados
{
    public class CreateUser
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public int genero { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public int puesto { get; set; }
        public int role { get; set; }
    }

    public class GetEmpleado
    {
        public int idEmpresa { get; set; }
        public string parametros
        {
            get
            {
                return "idEmpresa="+idEmpresa;
            }
        }
    }

    public class DeleteEmpleado
    {
        public int idUser { get; set; }
        public string parametros
        {
            get
            {
                return "idUser=" + idUser;
            }
        }
    }

    public class GetEmpleadoResponse
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string genero { get; set; }
        public string phoneNumber { get; set; }
        public bool enabled { get; set; }
        public int idPuesto { get; set; }
        public string nombrePuesto { get; set; }
        public string lastPasswordResetDate { get; set; }
        public List<authorities> authorities { get; set; }
    }

    public class authorities
    {
        public string authority { get; set; }
    }
}
