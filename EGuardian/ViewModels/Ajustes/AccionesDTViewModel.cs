using System;
using System.Collections.Generic;
using System.Linq;
using EGuardian.Controls;
using EGuardian.Models.Acciones;
using EGuardian.Models.Incidencias;
using Plugin.Toasts;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Ajustes
{
    public class AccionesDTViewModel : ContentView
    {
        List<acciones> actions = App.Database.GetAcciones().ToList();
        public AccionesDTViewModel()
        {
            ExtendedListView ContenidoAcciones = new ExtendedListView
            {
                Margin = 0,
                IsPullToRefreshEnabled = false,
                BackgroundColor = Color.Transparent,
                ItemTemplate = new DataTemplate(typeof(AppsActionsDTViewModel)),
                HasUnevenRows = false,
                RowHeight = 90,
                SeparatorColor = Color.FromHex("E5E5E5"),
                ItemsSource = actions
            };

            Content = ContenidoAcciones;
        }


        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }


        public void DisplayAlert(string title, string message)
        {
            string[] values = { title, message };
            MessagingCenter.Send<AccionesDTViewModel, string[]>(this, "DisplayAlert", values);
        }
    }
}