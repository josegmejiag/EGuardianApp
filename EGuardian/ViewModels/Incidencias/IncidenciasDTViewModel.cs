using System;
using EGuardian.Controls;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Incidencias
{
    public class IncidenciasDTViewModel : ViewCell
    {
        public IncidenciasDTViewModel()
        {
            Label title = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 14,
                TextColor = Color.FromHex("3F3F3F"),
                FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null)
            };
            title.SetBinding(Label.TextProperty, "Register_Nm");

            Label diagnostico = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                TextColor = Color.FromHex("3F3F3F"),
                FontSize = 12
            };
            diagnostico.SetBinding(Label.TextProperty, "Fecha");

            Label tratamiento = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.FromHex("3F3F3F"),
                FontSize = 12
            };
            tratamiento.SetBinding(Label.TextProperty, "Monto");

            Grid grabadoActualizadoGrid = new Grid
            {
                RowSpacing = 1,
                ColumnSpacing = 15,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (100, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };
            grabadoActualizadoGrid.Children.Add(
                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 5,
                    Children =
                    {
                        diagnostico
                    }
                }, 0, 0);
            grabadoActualizadoGrid.Children.Add(
                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 5,
                    Children =
                    {
                        new Label
                        {
                            Text = "MONTO:",
                            HorizontalOptions = LayoutOptions.Center,
                            FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                            HorizontalTextAlignment = TextAlignment.End,
                            TextColor = Color.FromHex("3F3F3F"),
                            FontSize = 12
                        },
                        tratamiento
                    }
                }, 1, 0);


            BoxView separador = new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("432161"), WidthRequest = 2 };
            separador.SetBinding(BoxView.BackgroundColorProperty, "BackgroundColor");

            IconView metodoPago = new IconView
            {
                Foreground = Color.FromHex("19B877"),
                Source = "iEfectivo.png",
                HeightRequest = 25,
                WidthRequest = 25,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
            };
            metodoPago.SetBinding(IconView.ForegroundProperty, "BackgroundColor");
            metodoPago.SetBinding(IconView.SourceProperty, "MetodoPago");


            Grid gridContenido = new Grid
            {
                RowSpacing = 5,
                Padding = new Thickness(15, 5, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (100, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (100, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength (100, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                }
            };

            gridContenido.Children.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing = 0,
                Children = { title }
            }, 0, 0);
            gridContenido.Children.Add(grabadoActualizadoGrid, 0, 1);
            gridContenido.Children.Add(new Label
            {
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                TextColor = Color.FromHex("B2B2B2"),
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Text = "TOCA PARA VER MÁS"
            }, 0, 2);

            Grid grid = new Grid
            {
                Padding = new Thickness(10, 5, 10, 5),
                RowSpacing = 0,
                ColumnSpacing = 0,
                MinimumHeightRequest = 89,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (79, GridUnitType.Absolute) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };
            grid.Children.Add(
                new StackLayout
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    WidthRequest = 50,
                    Padding = 0,
                    Spacing = 0,
                    Children = { metodoPago }
                }
                , 0, 0);
            grid.Children.Add(
                new StackLayout
                {
                    Padding = 0,
                    Spacing = 0,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        separador,
                        gridContenido
                    }
                }, 1, 0);

            this.View = new StackLayout
            {
                HeightRequest = 90,
                MinimumHeightRequest = 90,
                BackgroundColor = Color.FromHex("E5E5E5"),
                Padding = 0,
                Spacing = 0,
                Children =
                {
                    grid,
                    new BoxView () {VerticalOptions = LayoutOptions.End, Color = Color.FromHex("B2B2B2"), HeightRequest=1, Margin = new Thickness(25,0), HorizontalOptions = LayoutOptions.FillAndExpand },
                }
            };
        }
    }
}