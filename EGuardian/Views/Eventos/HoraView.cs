using System;
using System.Collections.Generic;
using EGuardian.Common;
using EGuardian.Data;
using EGuardian.Models.Eventos;
using EGuardian.ViewModels.Eventos;
using EGuardian.Views.Eventos.Evento;
using Xamarin.Forms;

namespace EGuardian.Views.Eventos
{
    public class HoraView : ContentView
    {
        RelativeLayout Contenido;
        public HoraView(DateTime hora, List<eventos> evento)
        {
            StackLayout contenido = new StackLayout() { Spacing = 0, Padding = 0 };
            TapGestureRecognizer doubletap = new TapGestureRecognizer();
            doubletap.NumberOfTapsRequired = 2;
            doubletap.Tapped += (sender, e) =>
            {
                var stack = Navigation.NavigationStack;
                if (this.IsEnabled && !Constants.PantallaAbierta && (stack[stack.Count - 1].GetType() != typeof(Indicador)) /*&& (stack[stack.Count - 1].GetType() != typeof(PacienteNuevo_EdicionVista))*/ && (stack[stack.Count - 1].GetType() != typeof(EventoPage)))
                {
                    this.IsEnabled = false;
                    Constants.PantallaAbierta = true;
                    Constants.AccionesEvento.Clear();
                    Constants.AplicacionesEvento.Clear();
                    Constants.AsistentesEvento.Clear();
                    Constants.DatosEvento = new RegistrarEvento();
                    Navigation.PushAsync(new EventoPage(hora, new eventos()));
                    this.IsEnabled = true;
                }
            };
            bool segundo = false;
            foreach (var Evento in evento)
            {
                if ((Evento.Fecha.Date.Equals(hora.Date))&&(Evento.horaInicioCita.StartsWith(hora.ToString(@"hh"))) && (Evento.horaTiempoCita.StartsWith(hora.ToString(@"tt", new System.Globalization.CultureInfo("es-GT")).ToLower())))
                {
                    if (!segundo)
                    {
                        contenido.Children.Add(new EventoDTViewModel(Evento));
                        segundo = true;
                    }
                    else
                    {
                        contenido.Children.Add(new StackLayout { HeightRequest = 1, Padding = new Thickness(3, 0), Children = { new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest = 1 } } });
                        contenido.Children.Add(new EventoDTViewModel(Evento));
                    }
                }
            }
            if (contenido.Children.Count == 0)
            {
                contenido.Children.Add(new Label
                {
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Color.FromHex("B3B3B3"),
                    FontAttributes = FontAttributes.Bold,
                    Text = "DOBLE TAP PARA AGREGAR EVENTO",
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
                });
                contenido.HeightRequest = 60;
                contenido.GestureRecognizers.Add(doubletap);
            }
            Label lHora1 = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("432161"),
                FontSize = 15,
                Text = hora.ToString(@"hh"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };
            Label lHora2 = new Label
            {
                VerticalOptions = LayoutOptions.End,
                TextColor = Color.FromHex("432161"),
                FontSize = 10,
                Text = hora.ToString(@"tt", new System.Globalization.CultureInfo("en-US")).ToUpper(),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };
            if (hora.Hour == DateTime.Now.Hour)
            {
                lHora1.TextColor = Color.FromHex("53A946");
                lHora2.TextColor = Color.FromHex("53A946");
            }
            Grid grid = new Grid
            {
                BackgroundColor = Color.FromHex("E2E3E3"),
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (60, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };
            grid.Children.Add(
                new StackLayout
                {
                    HeightRequest = 60,
                    WidthRequest = 50,
                    Padding = 0,
                    Spacing = 0,
                    Children =
                    {
                        new StackLayout
                        {
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Orientation = StackOrientation.Horizontal,
                            Padding = 0,
                            Spacing = 1,
                            Children=
                            {
                                lHora1, lHora2
                            }
                        }
                    }
                }
                , 0, 0);
            grid.Children.Add(
                new StackLayout
                {
                    Spacing = 0,
                    Padding = new Thickness(0, 5),
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        new StackLayout
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Children=
                            {
                                new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("432161"), WidthRequest = 2 },
                            }
                        },
                        new StackLayout
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,                            
                            Spacing=0,                            
                            Children=
                            {
                                contenido
                            }
                        }
                    }
                }, 1, 0);

            this.Padding = new Thickness(0, 0, 0, 5);
            this.BackgroundColor = Color.Transparent;

            RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView casilla = new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
            {
                BackgroundColor = Color.FromHex("919BB3"),
                HeightRequest = 10,
                WidthRequest = 100,
                BorderColor = Color.Blue,
                CornerRadius = 10
            };
            RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView sombra = new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
            {
                BackgroundColor = Color.FromHex("919BB3"),
                Opacity = 0.5,
                HeightRequest = 10,
                WidthRequest = 100,
                BorderColor = Color.Blue,
                CornerRadius = 10
            };

            Contenido = new RelativeLayout();

            Contenido.Children.Add(sombra,
                xConstraint: Constraint.Constant(2),
                yConstraint: Constraint.Constant(2),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height)
            );

            Contenido.Children.Add(casilla,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent(parent => (parent.Width - 2)),
                heightConstraint: Constraint.RelativeToParent(parent => (parent.Height - 2))
            );

            Contenido.Children.Add(grid,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width)
            );

            Content = new StackLayout
            {
                Spacing = 0,
                Padding = 0,
                Children =
                {
                    grid,
                    new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4 }
                }
            };
        }
    }
}