using System;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Models.Eventos;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Eventos
{
    public class EventoHoraDTViewModel : ContentView
    {
        public EventoHoraDTViewModel(eventos evento)
        {
            var labelStyle = new Style(typeof(Label));
            labelStyle.Setters.Add(new Setter() { Property = Label.FontSizeProperty, Value = 12 });
            labelStyle.Setters.Add(new Setter() { Property = Image.VerticalOptionsProperty, Value = LayoutOptions.Center });
            labelStyle.Setters.Add(new Setter() { Property = Label.TextColorProperty, Value = evento.estadoColor });


            var labelhourStyle = new Style(typeof(Label));
            labelhourStyle.Setters.Add(new Setter() { Property = Label.FontSizeProperty, Value = 10 });
            labelhourStyle.Setters.Add(new Setter() { Property = Image.VerticalOptionsProperty, Value = LayoutOptions.End });
            labelhourStyle.Setters.Add(new Setter() { Property = Label.TextColorProperty, Value = Color.FromHex("404040") });

            Label hora = new Label()
            {
                //cita.hora.ToString(@"hh:mm", new System.Globalization.CultureInfo("en-US")),
                Style = labelhourStyle,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)

            };
            hora.Text = evento.horaInicioCita + " - " + evento.horaFinCita;
            //hora.SetBinding(Label.TextProperty, "horaCita");
            //hora_tiempo.SetBinding(Label.TextProperty, "horaTiempoCita");
            Label estado = new Label()
            {
                Style = labelStyle,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };
            estado.Text = evento.estadoCita.ToUpper();
            //estado.SetBinding(Label.TextProperty, "estadoCita");

            var contenedor = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Padding = new Thickness(0, 0, 0, 0),
                Orientation = StackOrientation.Horizontal,
                Children = {
                    new IconView () {
                        Source = Images.Reloj,
                        Foreground = evento.estadoColor,
                        WidthRequest = 12,
                        HeightRequest = 12,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new BoxView () { Color = Color.Transparent, WidthRequest = 3, HeightRequest=1 },
                    hora,
                    new BoxView () { Color = Color.Transparent, WidthRequest = 8, HeightRequest=1},

                    estado
                }
            };

            Content = contenedor;
        }
    }
}

