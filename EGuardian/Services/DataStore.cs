using EGuardian.Data;
using EGuardian.Interfaces;
using EGuardian.Models;
using EGuardian.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EGuardian.Models.SectorNegocio;
using EGuardian.Models.Puestos;
using EGuardian.Models.Registro;
using EGuardian.Models.Contrasenia;
using EGuardian.Helpers;
using Xamarin.Forms;
using EGuardian.Models.Empleados;
using EGuardian.Models.Eventos;
using EGuardian.Models.Incidencias;

namespace EGuardian.Services
{
    public class DataStore : IDataStore
    {
        HttpClient EGuardianAPI;
        public LoginResponse Usuario { get; private set; }
        public RegistarIncidenteResponse Incidencia { get; private set; }
        public ResetPasswordResponse Contrasenia { get; private set; }
        public List<GetEmpleadoResponse> Empleados { get; private set; }
        public List<GetEventosResponse> Eventos { get; private set; }
        public GetEventoResponse Evento { get; private set; }
        public ValidarTokenResponse ResetContrasenia { get; private set; }
        public List<GetPuestosResponse> Puestos { get; private set; }
        public List<GetSectorNegocioResponse> Sectores { get; private set; }

        public DataStore()
        {
            EGuardianAPI = new HttpClient();
            EGuardianAPI.MaxResponseContentBufferSize = 256000;
            MessagingCenter.Subscribe<App>(this, "Autenticado", (sender) =>
            {
                EGuardianAPI.DefaultRequestHeaders.Clear();
                EGuardianAPI.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.session_access_token);
            });
        }

        public async Task<LoginResponse> LoginAsync(Login peticion)
        {
            Usuario = new LoginResponse();
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(
                    Constants.Endpoint_Auth_Login, 
                    new StringContent(JObject.FromObject(peticion).ToString(), Encoding.UTF8, "application/json"));                    
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                Usuario = JsonConvert.DeserializeObject<LoginResponse>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return Usuario;
        }

        public async Task<RegistarIncidenteResponse> RegistrarIncidenteAsync(RegistarIncidente peticion)
        {
            Incidencia = new RegistarIncidenteResponse();
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(
                    Constants.Endpoint_Evento_RegistarIncidente,
                    new StringContent(JObject.FromObject(peticion).ToString(), Encoding.UTF8, "application/json"));
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                Incidencia = JsonConvert.DeserializeObject<RegistarIncidenteResponse>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return Incidencia;
        }

        public async Task<bool> CrearEmpresaAsync(Signup peticion)
        {
            try
            {
                StringContent request= new StringContent(JObject.FromObject(peticion).ToString(), Encoding.UTF8, "application/json");
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Registro_CrearEmpresa, request);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                if (respuesta.Contains("SUCCESS"))
                    return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return false;
        }

        public async Task<bool> RegistrarEventoAsync(RegistrarEvento peticion)
        {
            try
            {
                StringContent request = new StringContent(JObject.FromObject(peticion).ToString(), Encoding.UTF8, "application/json");
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Evento_Registro, request);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                if (respuesta.Contains("SUCCESS"))
                    return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return false;
        }

        public async Task<bool> DeleteEmpleadoAsync(DeleteEmpleado peticion)
        {
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Admin_DeleteUser + peticion.parametros, null);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                if (respuesta.Contains("SUCCESS"))
                    return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return false;
        }

        public async Task<bool> CreateUserAsync(CreateUser peticion)
        {
            try
            {
                StringContent request = new StringContent(JObject.FromObject(peticion).ToString(), Encoding.UTF8, "application/json");
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Admin_CreateUser, request);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                if (respuesta.Contains("SUCCESS"))
                    return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return false;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPassword peticion)
        {
            Contrasenia = new ResetPasswordResponse();
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Auth_ResetPassword+peticion.parametros, null);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                Contrasenia = JsonConvert.DeserializeObject<ResetPasswordResponse>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return Contrasenia;
        }

        public async Task<GetEventoResponse> GetEventoAsync(GetEvento peticion)
        {
            Evento = new GetEventoResponse();
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Evento_GetEvento+peticion.parametros, null);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                Evento = JsonConvert.DeserializeObject<GetEventoResponse>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return Evento;
        }

        public async Task GetEventosAsync(GetEventos peticion)
        {
            Eventos = new List<GetEventosResponse>();
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Evento_GetEventosByEmpresa + peticion.parametros, null);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                Eventos = JsonConvert.DeserializeObject<List<GetEventosResponse>>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            var eventos = App.Database.GetEventos(Convert.ToInt32(Settings.session_idUsuario)).ToList();
            if (eventos.Count != 0)
                App.Database.DeleteAllEventos();

            try
            {
                foreach (var evento in Eventos)
                {
                    App.Database.InsertEvento(
                        new eventos
                        {
                            estado = evento.status,
                            idUsuario = Convert.ToInt32(Settings.session_idUsuario),
                            asunto = string.IsNullOrEmpty(evento.nombre)?string.Empty: evento.nombre.ToUpper(),
                            fechaFin = evento.fechaFin,
                            fechaInicio = evento.fechaInicio,
                            duracion = evento.duracion,
                            idUserCreator = evento.idUserCreator,
                            idEvento = evento.idEvento
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        public async Task GetEmpleadosByEmpresaAsync(GetEmpleado peticion)
        {
            Empleados = new List<GetEmpleadoResponse>();
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Evento_GetEmpleadosByEmpresa+peticion.parametros, null);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                Empleados = JsonConvert.DeserializeObject<List<GetEmpleadoResponse>>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            var empleados = App.Database.GetEmpleados(Convert.ToInt32(Settings.session_idUsuario)).ToList();
            if (empleados.Count != 0)
                App.Database.DeleteAllEmpleados();

            try
            {
                foreach (var empleado in Empleados)
                {
                    App.Database.InsertEmpleado(
                        new empleados
                        {
                            idEmpleado = empleado.id,
                            idUsuario = Convert.ToInt32(Settings.session_idUsuario),
                            username = empleado.username,
                            firstName = empleado.firstName,
                            lastName = empleado.lastName,
                            email = empleado.email,
                            genero = string.IsNullOrEmpty(empleado.genero)?0: Convert.ToInt32(empleado.genero),
                            phoneNumber = empleado.phoneNumber,
                            enabled = empleado.enabled,
                            lastPasswordResetDate = empleado.lastName,
                            authority = empleado.authorities.Count==0?null: empleado.authorities[0].authority,
                            idPuesto = 0,
                            nombrePuesto = ((List<puestos>)App.Database.GetPuesto())[0].nombre
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }            
        }

        public async Task<ValidarTokenResponse> ValidarTokenAsync(ValidarToken peticion)
        {
            ResetContrasenia = new ValidarTokenResponse();
            try
            {
                StringContent request = new StringContent(JObject.FromObject(peticion).ToString(), Encoding.UTF8, "application/json");
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Auth_Validar_Token, request);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                ResetContrasenia = JsonConvert.DeserializeObject<ValidarTokenResponse>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return ResetContrasenia;
        }

        public async Task<bool> ChangePasswordAsync(ChangePassword peticion)
        {
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(
                    Constants.Endpoint_Auth_ChangePassword,
                    new StringContent(JObject.FromObject(peticion).ToString(), Encoding.UTF8, "application/json"));
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                if (respuesta.Contains("SUCCESS"))
                    return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            return false;
        }

        public async Task GetPuestosAsync()
        {
            Puestos = new List<GetPuestosResponse>();
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Registro_GetPuestos, null);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                Puestos = JsonConvert.DeserializeObject<List<GetPuestosResponse>>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
            if(Puestos.Count!=0)
            {
                await Task.Run(() => { App.Database.DeleteAllPuestos(); });
                foreach (var Puesto in Puestos)
                {
                    puestos puesto = new puestos
                    {
                        id = Puesto.idCatalogoPuestos,
                        nombre = Puesto.nombre
                    };
                    try
                    {
                        if (App.Database.InsertPuesto(puesto) == 1)
                            System.Diagnostics.Debug.WriteLine("CORRECTO: ¡Se ha realizado correctamente la insersion de datos!");
                        else
                            System.Diagnostics.Debug.WriteLine("ERROR: ¡Ha ocurrido un error inesperado al insercion de datos!");

                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
                    }
                }
            }                
        }

        public async Task GetSectorNegocioAsync()
        {
            Sectores = new List<GetSectorNegocioResponse>();
            try
            {
                var solicitud = await EGuardianAPI.PostAsync(Constants.Endpoint_Registro_GetSectorNegocio, null);
                solicitud.EnsureSuccessStatusCode();
                string respuesta = await solicitud.Content.ReadAsStringAsync();
                Sectores = JsonConvert.DeserializeObject<List<GetSectorNegocioResponse>>(respuesta);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }

            if (Sectores.Count != 0)
            {
                await Task.Run(() => { App.Database.DeleteAllSectores(); });
                foreach (var Sector in Sectores)
                {
                    sectores sector = new sectores
                    {
                        id = Sector.idSectorNegocio,
                        nombre = Sector.nombre
                    };
                    try
                    {
                        if (App.Database.InsertSector(sector) == 1)
                            System.Diagnostics.Debug.WriteLine("CORRECTO: ¡Se ha realizado correctamente la insersion de datos!");
                        else
                            System.Diagnostics.Debug.WriteLine("ERROR: ¡Ha ocurrido un error inesperado al insercion de datos!");

                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
                    }
                }
            }
        }
    }
}