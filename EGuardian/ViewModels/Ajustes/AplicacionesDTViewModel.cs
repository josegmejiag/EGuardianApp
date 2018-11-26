using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Models.Aplicaciones;
using EGuardian.Models.Incidencias;
using Plugin.Toasts;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Ajustes
{
    public class AplicacionesDTViewModel : ContentView
    {
        List<aplicaciones> apps = App.Database.GetAplicaciones().ToList();
        public AplicacionesDTViewModel()
        {             
            ExtendedListView ContenidoAplicaciones = new ExtendedListView
            {
                Margin = 0,
                IsPullToRefreshEnabled = false,
                BackgroundColor = Color.Transparent,
                ItemTemplate = new DataTemplate(typeof(AppsActionsDTViewModel)),
                HasUnevenRows = false,
                RowHeight = 90,
                SeparatorColor = Color.FromHex("E5E5E5"),
                ItemsSource = apps
            };

            Content = ContenidoAplicaciones;
        }


        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }


        public void DisplayAlert(string title, string message)
        {
            string[] values = { title, message };
            MessagingCenter.Send<AplicacionesDTViewModel, string[]>(this, "DisplayAlert", values);
        }
    }
}