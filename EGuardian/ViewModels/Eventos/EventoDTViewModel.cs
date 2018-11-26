using System;
using EGuardian.Common;
using EGuardian.Data;
using EGuardian.Models.Eventos;
using EGuardian.Views.Eventos.Evento;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Eventos
{
    public class EventoDTViewModel : ContentView
    {
        public eventos evento;
        public string Subject = string.Empty;
        public EventoDTViewModel(eventos evento)
        {
            this.evento = evento;
            Grid grid = new Grid
            {
                Padding = new Thickness(0, 5, 15, 5),
                RowSpacing = 1,
                ColumnSpacing = 1,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (60, GridUnitType.Absolute) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (10, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                }
            };
            grid.Children.Add(new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = evento.estadoColor, WidthRequest = 10 }, 0, 0);
            grid.Children.Add(new EventoDetalleDTViewModel(evento), 1, 0);
            grid.Children.Add(new EventoActionDTViewModel(evento), 2, 0);
            Padding = new Thickness(5, 5, 0, 5);
            Content = grid;


            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += async (sender, e) =>
            {
                var stack = Navigation.NavigationStack;
                if (this.IsEnabled && !Constants.PantallaAbierta && (stack[stack.Count - 1].GetType() != typeof(Indicador)) /*&& (stack[stack.Count - 1].GetType() != typeof(PacienteNuevo_EdicionVista))*/ && (stack[stack.Count - 1].GetType() != typeof(EventoPage)))
                {
                    this.IsEnabled = false;
                    Constants.PantallaAbierta = true;
                    await Navigation.PopAllPopupAsync();
                    await Navigation.PushPopupAsync(new Indicador("Obteniendo evento", Color.White));
                    GetEvento peticion = new GetEvento
                    {
                        idEvento = evento.idEvento
                    };
                    GetEventoResponse Respuesta = await App.ManejadorDatos.GetEventoAsync(peticion);
                    Constants.AccionesEvento.Clear();
                    Constants.AplicacionesEvento.Clear();
                    Constants.AsistentesEvento.Clear();
                    Constants.DatosEvento = new RegistrarEvento();
                    await Navigation.PopAllPopupAsync();
                    await Navigation.PushAsync(new EventoPage(Convert.ToDateTime(evento.FechaInicio), evento));
                    this.IsEnabled = true;
                }
                else
                    System.Diagnostics.Debug.WriteLine("Pantalla abierta");

            };

            grid.GestureRecognizers.Add(tap);
        }
    }
}

