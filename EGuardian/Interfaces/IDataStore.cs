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

namespace EGuardian.Interfaces
{
    public interface IDataStore
    {
        Task<LoginResponse> LoginAsync(Login peticion);
        Task GetPuestosAsync();
        Task GetSectorNegocioAsync();
        Task<bool> CrearEmpresaAsync(Signup peticion);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPassword peticion);
        Task GetEmpleadosByEmpresaAsync(GetEmpleado peticion);
        Task<ValidarTokenResponse> ValidarTokenAsync(ValidarToken peticion);
        Task<bool> ChangePasswordAsync(ChangePassword peticion);
        Task<bool> CreateUserAsync (CreateUser peticion);
        Task<bool> DeleteEmpleadoAsync(DeleteEmpleado peticion);
        Task GetEventosAsync(GetEventos peticion);
        Task<bool> RegistrarEventoAsync(RegistrarEvento peticion);
        Task<GetEventoResponse> GetEventoAsync(GetEvento peticion);
        Task<RegistarIncidenteResponse> RegistrarIncidenteAsync(RegistarIncidente peticion);
    }
}