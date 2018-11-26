using System;
using EGuardian.Models.Eventos;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Eventos
{
    public class EventoDetalleDTViewModel : ContentView
    {
        public EventoDetalleDTViewModel(eventos evento)
        {
            Label nombrePaciente = new Label()
            {
                FontSize = 12,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                TextColor = Color.FromHex("404040"),
            };
            nombrePaciente.Text = evento.asunto;

            var contenedor = new StackLayout()
            {
                Spacing = 0,
                Padding = new Thickness(5, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    nombrePaciente,
                    new EventoHoraDTViewModel(evento)
                }

            };

            Content = contenedor;




        }
    }
}

