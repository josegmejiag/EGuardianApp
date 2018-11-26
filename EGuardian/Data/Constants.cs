using System;
using System.Collections.Generic;
using System.Text;
using EGuardian.Models.Acciones;
using EGuardian.Models.Aplicaciones;
using EGuardian.Models.Empleados;
using EGuardian.Models.Eventos;

namespace EGuardian.Data
{
    public static class Constants
    {
        public static readonly string releaseEGuardianAPI = "http://68.183.17.17:8080/";
        public static readonly string debugEGuardianAPI = "http://68.183.17.17:8080/";

        public static readonly string Endpoint_EGuardianAPI = releaseEGuardianAPI;

        public static readonly string Endpoint_API = Endpoint_EGuardianAPI + "eguardian/";

        public static readonly string Endpoint_Admin_CreateUser = Endpoint_API + "admin/createUser";
        public static readonly string Endpoint_Admin_DeleteUser = Endpoint_API + "admin/deleteUser?";
        public static readonly string Endpoint_Auth_Login = Endpoint_API + "auth/login";
        public static readonly string Endpoint_Auth_ResetPassword = Endpoint_API + "auth/resetpassword?";
        public static readonly string Endpoint_Auth_ChangePassword = Endpoint_API + "auth/change-password";
        public static readonly string Endpoint_Auth_Validar_Token = Endpoint_API + "auth/validar_Token";
        public static readonly string Endpoint_Evento_GetEmpleadosByEmpresa = Endpoint_API + "evento/getEmpleadosByEmpresa?";
        public static readonly string Endpoint_Evento_GetEvento = Endpoint_API + "evento/getEvento?";
        public static readonly string Endpoint_Evento_GetEventosByEmpresa = Endpoint_API + "evento/getEventosByEmpresa?";
        public static readonly string Endpoint_Evento_Registro = Endpoint_API + "evento/registro";
        public static readonly string Endpoint_Evento_RegistarIncidente = Endpoint_API + "evento/RegistarIncidente";
        public static readonly string Endpoint_Registro_GetPuestos = Endpoint_API + "registro/getPuestos";
        public static readonly string Endpoint_Registro_GetSectorNegocio = Endpoint_API + "registro/getSectorNegocio";
        public static readonly string Endpoint_Registro_CrearEmpresa = Endpoint_API + "registro/crearEmpresa";

        public static string MenuException = "No se reconoce el menu";


        public static DateTime FechaInicio;
        public static DateTime FechaFinal;

        public static bool RedSocialPresentada;
        public static bool ExisteConexionAInternet = true;
        public static double NavigationBarHeight = 0f;

        public static int isEnableSelected = 1;
        public static int isEnableUnSelected = 0;


        public static int idAplicacionIncidencia = 0;
        public static string fechaInicioIncidencia = String.Empty;

        public static bool PantallaAbierta = false;

        public static List<empleados> AsistentesEvento = new List<empleados>();
        public static List<acciones> AccionesEvento = new List<acciones>();
        public static List<aplicaciones> AplicacionesEvento = new List<aplicaciones>();
        public static RegistrarEvento DatosEvento = new RegistrarEvento();


        public static Dictionary<string, int> genero = new Dictionary<string, int>
        {
            { "MASCULINO", 0 },
            { "FEMENINO", 1 }
        };


        public static Dictionary<string, int> colaboradores = new Dictionary<string, int>
        {
            { "De 0 a 10", 10 },
            { "De 11 a 100", 100 },
            { "De 101 a 500", 500 },
            { "Más de 501 ", 501 }
        };

        public static int[] horas = {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24};

        public static Dictionary<string, string> estados = new Dictionary<string, string>
        {
            { "En espera"           ,   "0" },
            { "En reunión"          ,   "1" },
            { "Cancelada"           ,   "2" }
        };
    }
}