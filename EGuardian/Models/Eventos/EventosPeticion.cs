using System;
using System.Collections.Generic;

namespace EGuardian.Models.Eventos
{
    public class GetEventos
    {
        public int idEmpresa { get; set; }
        public string parametros
        {
            get
            {
                return "idEmpresa=" + idEmpresa;
            }
        }
    }

    public class GetEvento
    {
        public int idEvento { get; set; }
        public string parametros
        {
            get
            {
                return "idEvento=" + idEvento;
            }
        }
    }


    public class RegistrarEvento
    {
        public RegistrarEvento()
        {
            acciones = new List<int>();
            aplicaciones = new List<int>();
            usuarios = new List<int>();
        }
        public List<int> acciones { get; set; }
        public List<int> aplicaciones { get; set; }
        public string fechaFin { get; set; }
        public string fechaInicio { get; set; }
        public int idEmpresa { get; set; }
        public int idcapacitador { get; set; }
        public string nombre { get; set; }
        public List<int> usuarios { get; set; }
    }

    public class GetEventoResponse
    {
        public List<usuariosEvento> usuarios { get; set; }
        public List<accionesEvento> acciones { get; set; }
        public List<aplicacionesEvento> aplicaciones { get; set; }
    }

    public class usuariosEvento
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string genero { get; set; }
        public string phoneNumber { get; set; }
        public bool enabled { get; set; }
        public string nombrePuesto { get; set; }
        public string lastPasswordResetDate { get; set; }
        public List<authorities> authorities { get; set; }
    }

    public class authorities
    {
        public string authority { get; set; }
    }

    public class accionesEvento
    {
        public int idAccion { get; set; }
        public string nombre { get; set; }
    }

    public class aplicacionesEvento
    {
        public int idAplicacion { get; set; }
        public string nombre { get; set; }
        public string tipo { get; set; }
        public string icono { get; set; }
    }

    public class GetEventosResponse
    {
        public string duracion { get; set; }
        public empresa empresa { get; set; }
        public int status { get; set; }
        public string fechaFin { get; set; }
        public string fechaInicio { get; set; }
        public int idEvento { get; set; }
        public int idUserCreator { get; set; }
        public string nombre { get; set; }
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
}
