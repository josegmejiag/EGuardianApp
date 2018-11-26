using System;
using System.Linq;
using System.Threading.Tasks;
using EGuardian.Data;
using EGuardian.Helpers;
using EGuardian.Interfaces;
using EGuardian.Models.Acciones;
using EGuardian.Models.Aplicaciones;
using EGuardian.Models.Incidencias;
using EGuardian.Services;
using EGuardian.Views.Acceso;
using EGuardian.Views.Menu;
using EGuardian.Views.Tutorial;
using Xamarin.Forms;

namespace EGuardian
{
    public partial class App : Application
    {
        public static EGuardianDatabase database;
        public static ManejadorDatos ManejadorDatos { get; set; }

        public static double DisplayScreenWidth = 0f;
        public static double DisplayScreenHeight = 0f;
        public static double NavigationBarHeight = 0f;
        public static double DisplayScaleFactor = 0f;

        public static EGuardianDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new EGuardianDatabase();
                }
                return database;
            }
        }

        public App()
        {
            MessagingCenter.Subscribe<Views.Menu.MainPage>(this, "noAutenticado", (sender) =>
            {
                MainPage = new LoginPage();
            });

            MessagingCenter.Subscribe<LoginPage>(this, "Autenticado", (sender) =>
            {
                MainPage = new MainPage();
                MessagingCenter.Send<App>(this, "Autenticado");
            });

            MessagingCenter.Subscribe<Registro>(this, "Autenticado", (sender) =>
            {
                MainPage = new MainPage();
                MessagingCenter.Send<App>(this, "Autenticado");
            });

            MessagingCenter.Subscribe<Forget>(this, "Autenticado", (sender) =>
            {
                MainPage = new MainPage();
                MessagingCenter.Send<App>(this, "Autenticado");
            });

            ManejadorDatos = new ManejadorDatos(new DataStore());
            Task.Run(() => CargaInicial());
            if (!string.IsNullOrEmpty(Settings.session_access_token))
                MainPage = new MainPage();
            else
                MainPage = new TutorialPage();


            DependencyService.Get<IService>().Start();
            Constants.FechaInicio = DateTime.Now;
            Constants.FechaFinal = DateTime.Now.AddMinutes(5);


            MessagingCenter.Subscribe<string>(this, "hello",  async sender =>
            {
                System.Diagnostics.Debug.WriteLine(sender);
                RegistarIncidente Peticion = new RegistarIncidente()
                {
                    duracion = new Random().Next(1, 100),
                    fechaInicio = Constants.fechaInicioIncidencia,
                    idaccion = new Random().Next(1, 5),
                    idaplicacion = Constants.idAplicacionIncidencia,
                    idevento = 16,
                };
                RegistarIncidenteResponse Respuesta = await ManejadorDatos.RegistrarIncidenteAsync(Peticion);
            });


        }

        public static void CloseService()
        {
            DependencyService.Get<IService>().Stop();
        }

        async Task CargaInicial()
        {
            var apps = Database.GetAplicaciones().ToList();
            if(apps.Count==0)
            {
                Database.InsertAplicacion(
                    new aplicaciones
                    {
                        idAplicacion = 1,
                        nombre = "Facebook",
                        package = "com.facebook.katana"
                    });

                Database.InsertAplicacion(
                    new aplicaciones
                    {
                        idAplicacion = 2,
                        nombre = "Google Chrome",
                        package = "com.android.chrome"
                    });


                Database.InsertAplicacion(
                    new aplicaciones
                    {
                        idAplicacion = 3,
                        nombre = "Twitter",
                        package = "com.twitter.android"
                    });


                Database.InsertAplicacion(
                    new aplicaciones
                    {
                        idAplicacion = 4,
                        nombre = "WhatsApp",
                        package = "com.whatsapp"
                    });

                Database.InsertAplicacion(
                    new aplicaciones
                    {
                        idAplicacion = 5,
                        nombre = "YouTube",
                        package = "com.google.android.youtube"
                    });
                System.Diagnostics.Debug.WriteLine("----------------TERMINO APLICACIONES----------------");
            }

            var actions = Database.GetAcciones().ToList();
            if (actions.Count == 0)
            {
                Database.InsertAccion(
                    new acciones
                    {
                        idAccion =1,
                        nombre="Sonido"
                    });

                Database.InsertAccion(
                    new acciones
                    {
                        idAccion = 2,
                        nombre = "Email"
                    });

                Database.InsertAccion(
                    new acciones
                    {
                        idAccion = 3,
                        nombre = "Vibración"
                    });

                Database.InsertAccion(
                    new acciones
                    {
                        idAccion = 4,
                        nombre = "SMS"
                    });

                Database.InsertAccion(
                    new acciones
                    {
                        idAccion = 5,
                        nombre = "Modo Avión"
                    });
                System.Diagnostics.Debug.WriteLine("----------------TERMINO ACCIONES----------------");
            }

            await App.ManejadorDatos.GetPuestosAsync();
            System.Diagnostics.Debug.WriteLine("----------------TERMINO GETPUESTOS----------------");
            MessagingCenter.Send<App>(this, "CargaInicialPuestos");

            await App.ManejadorDatos.GetSectorNegocioAsync();
            System.Diagnostics.Debug.WriteLine("----------------TERMINO GETSECTORNEGOCIOS----------------");
            MessagingCenter.Send<App>(this, "CargaInicialSectores");
        }

        protected override void OnStart()
        {            

        }

        protected override void OnSleep()
        {            
        }

        protected override void OnResume()
        {            
        }
    }
}