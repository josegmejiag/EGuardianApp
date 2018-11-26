using EGuardian.Common;
using EGuardian.Common.Resources;
using EGuardian.Helpers;
using EGuardian.Models.Empleados;
using EGuardian.Models.Menu;
using EGuardian.ViewModels.Ajustes;
using EGuardian.ViewModels.Perfil;
using EGuardian.ViewModels.Perfil.Empleado;
using EGuardian.Views.Ajustes;
using EGuardian.Views.API;
using EGuardian.Views.Eventos;
using EGuardian.Views.Incidencias;
using EGuardian.Views.Perfil;
using EGuardian.Views.Perfil.Empleado;
using EGuardian.Views.Reportes;
using Plugin.Toasts;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EGuardian.Views.Menu
{
    public class MainPage : MasterDetailPage
    {        
        Menu Menu = new Menu();
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            Title = Strings.MenuTitle;
            Master = Menu;
            MasterBehavior = MasterBehavior.Popover;
            NavigateFromMenu((int)MenuItemType.Eventos);
            MessagingCenter.Subscribe<ContraseniaDTViewModel>(this, "noAutenticado", async (sender) =>
            {
                await NavigateFromMenu((int)MenuItemType.Salir);
            });

            MessagingCenter.Subscribe<EmpleadoPage>(this, "EmpleadoCreado", async (sender) =>
            {
                await NavigateFromMenu((int)MenuItemType.Perfil);
                MessagingCenter.Send<MainPage>(this, "CargaEmpleados");
            });
            MessagingCenter.Subscribe<UsuarioDTViewModel, string[]>(this, "DisplayAlert", async (sender, values) =>
            {
                await DisplayAlert(values[0], values[1], "Aceptar");
                MessagingCenter.Send<MainPage>(this, "DisplayAlert");
            });
            MessagingCenter.Subscribe<CuentaDTViewModel, string[]>(this, "DisplayAlert", async (sender, values) =>
            {
                await DisplayAlert(values[0], values[1], "Aceptar");
                MessagingCenter.Send<MainPage>(this, "DisplayAlert");
            });

            MessagingCenter.Subscribe<ContraseniaDTViewModel, string[]>(this, "DisplayAlert", async (sender, values) =>
            {
                await DisplayAlert(values[0], values[1], "Aceptar");
                MessagingCenter.Send<MainPage>(this, "DisplayAlert");
            });

            MessagingCenter.Subscribe<EmpleadoDTViewModel>(this, "EliminarEmpleado", async (sender) =>
            {
                await Navigation.PushPopupAsync(new Indicador("Eliminando empleado", Color.White));
                DeleteEmpleado Peticion = new DeleteEmpleado
                {
                    idUser = sender.contexto.idEmpleado,
                };
                var Respuesta = await App.ManejadorDatos.DeleteEmpleadoAsync(Peticion);
                if (Respuesta)
                {
                    ShowToast(ToastNotificationType.Success, "Eliminación de empleado", "El empleado se ha eliminado con éxito.", 5);
                    await NavigateFromMenu((int)MenuItemType.Perfil);
                    MessagingCenter.Send<MainPage>(this, "CargaEmpleados");
                }
                else
                {
                    ShowToast(ToastNotificationType.Error, "Eliminación de empleado", "Ha ocurrido algo inesperado eliminando al empleado, inténtalo de nuevo.", 5);
                    IsPresented = false;
                }

            });
        }

        public async Task NavigateFromMenu(int id)
        {
            if((int)MenuItemType.Salir==id)
            {
                Settings.session_access_token = null;
                Settings.session_idUsuario = null;
                Settings.session_idEmpresa = null;
                Settings.session_nombreEmpresa = null;
                Settings.session_username = null;
                Settings.session_expires_in = null;
                Settings.session_authority = null;

                if (string.IsNullOrEmpty((Settings.session_access_token)))
                {
                    MessagingCenter.Send<MainPage>(this, "noAutenticado");
                }
                return;
            }
            if ((int)MenuItemType.Perfil == id)
            {
                await Navigation.PushPopupAsync(new Indicador("Obteniendo tus datos", Color.White));
                GetEmpleado Peticion = new GetEmpleado
                {
                    idEmpresa = Convert.ToInt32(Settings.session_idEmpresa)
                };
                await App.ManejadorDatos.GetEmpleadosByEmpresaAsync(Peticion);
                await Navigation.PopAllPopupAsync();
            }
            if (!MenuPages.ContainsKey(id))
                AddMenuPage(id);
            if ((int)MenuItemType.Eventos == id)
            {
                MenuPages.Remove(id);
                AddMenuPage(id);
            }
            var menu = MenuPages[id];
            if (menu != null && Detail != menu)
            {
                menu.BarTextColor = Colors.BarTextColor;
                menu.BarBackgroundColor = Colors.BarBackgroundColor;
                Detail = menu;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }

        private void AddMenuPage(int id)
        {
            switch (id)
            {
                case (int)MenuItemType.Eventos:
                    MenuPages.Add(id, new NavigationPage(new EventosPage()));
                    break;
                case (int)MenuItemType.Incidencias:
                    MenuPages.Add(id, new NavigationPage(new IncidenciasPage()));
                    break;
                case (int)MenuItemType.Perfil:
                    MenuPages.Add(id, new NavigationPage(new PerfilPage()));
                    break;
                case (int)MenuItemType.Reportes:
                    MenuPages.Add(id, new NavigationPage(new ReportesPage()));
                    break;
                case (int)MenuItemType.API:
                    MenuPages.Add(id, new NavigationPage(new APIPage()));
                    break;
                case (int)MenuItemType.Ajustes:                
                    MenuPages.Add(id, new NavigationPage(new AjustesPage()));
                    break;                
            }
        }

        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }
    }
}