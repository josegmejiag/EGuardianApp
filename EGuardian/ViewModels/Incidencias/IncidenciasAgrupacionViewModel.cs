using System;
using System.Collections.Generic;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Models.Incidencias;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Incidencias
{
    public class IncidenciasAgrupacionViewModel
    {
        public IncidenciasAgrupacionViewModel() { }
        public string Moneda { get; set; }
        public double Monto { get; set; }
        public List<incidencias> TransaccionesOrigen { get; set; }
        public List<incidencias> Transacciones { get; set; }
        public incidencias transaccionSeleccionada = null;
        public StackLayout Contenido
        {
            get
            {
                ExtendedListView ContenidoCaja = new ExtendedListView
                {
                    Margin = 0,
                    IsPullToRefreshEnabled = false,
                    BackgroundColor = Color.Transparent,
                    ItemTemplate = new DataTemplate(typeof(IncidenciasDTViewModel)),
                    HasUnevenRows = false,
                    RowHeight = 90,
                    SeparatorColor = Color.FromHex("E5E5E5"),
                    ItemsSource = Transacciones
                };
                ContenidoCaja.ItemSelected += (sender, e) =>
                {
                    /*if (((ExtendedListView)sender).SelectedItem == null)
                    {
                        ((ExtendedListView)sender).SelectedItem = null;
                        return;
                    }
                    if (ContenidoCaja.IsEnabled && !Constants.ModalAbierto && !((incidencias)e.SelectedItem).isSelected)
                    {
                        ContenidoCaja.IsEnabled = false;
                        Constantes.ModalAbierto = true;
                        incidencias transaccion = (incidencias)e.SelectedItem;
                        transaccion.isSelected = true;
                        transaccionSeleccionada = transaccion;
                        MessagingCenter.Send<CajaAgrupacionModeloVista>(this, "seleccionado");
                        ContenidoCaja.IsEnabled = true;
                    }
                    else
                        System.Diagnostics.Debug.WriteLine("Modal abierto actualmente");
                    ((ExtendedListView)sender).SelectedItem = null;*/
                };

                return new StackLayout
                {
                    IsVisible = Transacciones.Count > 0 ? true : false,
                    Padding = 0,
                    BackgroundColor = Color.FromHex("E5E5E5"),
                    Spacing = -1,
                    Children =
                    {
                        ContenidoCaja,
                        new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4 }
                    }

                };
            }
        }
        public StackLayout Cabecera
        {
            get
            {
                Grid Header = new Grid
                {
                    BackgroundColor = Color.FromHex("19B877"),
                    Padding = new Thickness(20, 0, 15, 0),
                    HeightRequest = 25,
                    MinimumWidthRequest = 25,
                    RowSpacing = 0,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowDefinitions = {
                        new RowDefinition { Height = new GridLength (25, GridUnitType.Absolute) }
                },
                    ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) }
                }
                };
                Header.Children.Add(new Label
                {
                    Text = Moneda,
                    FontSize = 13,
                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    TextColor = Color.White
                }, 0, 0);

                Header.Children.Add(new Label
                {
                    Text = Monto.ToString("N", System.Globalization.CultureInfo.InvariantCulture),
                    FontSize = 13,
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                    HorizontalTextAlignment = TextAlignment.End,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.White
                }, 1, 0);

                Header.Children.Add(new Label
                {
                    Text = Moneda,
                    FontSize = 13,
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                    HorizontalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.White
                }, 2, 0);

                return new StackLayout
                {
                    IsVisible = Transacciones.Count > 0 ? true : false,
                    HeightRequest = 30,
                    MinimumWidthRequest = 30,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Spacing = 0,
                    Children =
                    {
                        new BoxView { HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.White, HeightRequest=5 },
                        Header
                    }
                };
            }
        }
    }
}
