using EGuardian.Interfaces;
using EGuardian.Models.Contrasenia;
using EGuardian.Models.Empleados;
using EGuardian.Models.Eventos;
using EGuardian.Models.Incidencias;
using EGuardian.Models.Login;
using EGuardian.Models.Registro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGuardian.Data
{
    public class ManejadorDatos
    {
        IDataStore ServicioWeb;

        public ManejadorDatos(IDataStore servicio)
        {
            ServicioWeb = servicio;
        }

        public Task<LoginResponse> LoginAsync(Login peticion)
        {
            return ServicioWeb.LoginAsync(peticion);
        }

        public Task GetPuestosAsync()
        {
            return ServicioWeb.GetPuestosAsync();
        }

        public Task GetSectorNegocioAsync()
        {
            return ServicioWeb.GetSectorNegocioAsync();
        }

        public Task<bool> CrearEmpresaAsync(Signup peticion)
        {
            return ServicioWeb.CrearEmpresaAsync(peticion);
        }

        public Task<ResetPasswordResponse> ResetPasswordAsync(ResetPassword peticion)
        {
            return ServicioWeb.ResetPasswordAsync(peticion);
        }

        public Task<ValidarTokenResponse> ValidarTokenAsync(ValidarToken peticion)
        {
            return ServicioWeb.ValidarTokenAsync(peticion);
        }

        public Task<bool> ChangePasswordAsync(ChangePassword peticion)
        {
            return ServicioWeb.ChangePasswordAsync(peticion);
        }

        public Task<bool> CreateUserAsync(CreateUser peticion)
        {
            return ServicioWeb.CreateUserAsync(peticion);
        }

        public Task GetEmpleadosByEmpresaAsync (GetEmpleado peticion)
        {
            return ServicioWeb.GetEmpleadosByEmpresaAsync(peticion);
        }

        public Task<bool> DeleteEmpleadoAsync(DeleteEmpleado peticion)
        {
            return ServicioWeb.DeleteEmpleadoAsync(peticion);
        }

        public Task GetEventosAsync(GetEventos peticion)
        {
            return ServicioWeb.GetEventosAsync(peticion);
        }

        public Task<bool> RegistrarEventoAsync(RegistrarEvento peticion)
        {
            return ServicioWeb.RegistrarEventoAsync(peticion);
        }

        public Task<GetEventoResponse> GetEventoAsync(GetEvento peticion)
        {
            return ServicioWeb.GetEventoAsync(peticion);
        }

        public Task<RegistarIncidenteResponse> RegistrarIncidenteAsync(RegistarIncidente peticion)
        {
            return ServicioWeb.RegistrarIncidenteAsync(peticion);
        }
    }
}
