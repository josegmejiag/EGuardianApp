using System;
using System.Collections.Generic;
using System.Linq;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Models.Empleados;
using EGuardian.Views.Eventos.Asistentes;
using SuaveControls.Views;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Eventos.Evento
{
    public class EventoAsistentesViewModel : ContentView
    {
        FloatingActionButton AddFAB;
        RelativeLayout Contenido;
        ListView AsistentesListView;
        public EventoAsistentesViewModel()
        {

            MessagingCenter.Subscribe<AsistentesFiltradoPage>(this, "OK_A", (sender) =>
            {
                if (Constants.AsistentesEvento.Any((asistente) => asistente.nombre.Equals(((empleados)sender.Pacientes.SelectedItem).nombre)))
                    System.Diagnostics.Debug.WriteLine("Asistente ya esta en la lista");
                else
                {
                    ((empleados)sender.Pacientes.SelectedItem).rol = "Asistente";
                    Constants.AsistentesEvento.Add((empleados)sender.Pacientes.SelectedItem);
                    AsistentesListView.ItemsSource = null;
                    AsistentesListView.ItemsSource = Constants.AsistentesEvento;
                }                    
            });


            MessagingCenter.Subscribe<EventoDatosViewModel>(this, "OK_B", (sender) =>
            {
                AsistentesListView.ItemsSource = null;
                AsistentesListView.ItemsSource = Constants.AsistentesEvento;
            });


            ExtendedEntry BusquedaRapida = new ExtendedEntry
            {
                Placeholder = "Buscar",
                HasBorder = false,
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(10, 0),
                HeightRequest = 20,
                PlaceholderColor = Color.FromHex("432161"),
                FontSize = 14,
                TextColor = Color.FromHex("432161"),
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            Grid AsistentesHeader = new Grid
            {
                BackgroundColor = Color.White,
                Padding = new Thickness(10, 0, 10, 0),
                ColumnSpacing = 5,
                //VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },

                }
            };
            AsistentesHeader.Children.Add(
            new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
            {
                BackgroundColor = Color.FromHex("E5E5E5"),
                CornerRadius = 6,
                HeightRequest = 20,
                //WidthRequest = 128,
            }, 0, 0);

            Image cancelar = new Image
            {
                //Foreground = Color.FromHex("F7B819"),
                Source = "iCancelarB.png",
                HeightRequest = 25,
                WidthRequest = 25,

            };

            Image buscar = new Image
            {
                //Foreground = Color.FromHex("F7B819"),
                Source = "iBusqueda.png",
                HeightRequest = 25,
                WidthRequest = 25,

            };
            AsistentesHeader.Children.Add(BusquedaRapida, 0, 0);
            AsistentesHeader.Children.Add(cancelar, 1, 0);
            AsistentesHeader.Children.Add(buscar, 2, 0);

            AsistentesListView = new ListView
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //IsScrollEnable = false,
                ItemsSource = Constants.AsistentesEvento,
                ItemTemplate = new DataTemplate(typeof(EventoAsistentesDTViewModel)),
                Margin = 0,
                RowHeight = 55, //Convert.ToInt32((App.DisplayScreenHeight / 13.533333333333333)),
                IsPullToRefreshEnabled = false,
                SeparatorVisibility = SeparatorVisibility.None,
                SeparatorColor = Color.Transparent,
                HasUnevenRows = false,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            AsistentesListView.ItemSelected += (sender, e) =>
            {
                //DisplayAlert("Asistentes", "Se redirigirá a las alertas de " + ((asistentes)e.SelectedItem).nombre, "Aceptar");
            };

            try
            {
                AddFAB = new FloatingActionButton
                {
                    Image = "iFABPa",
                    ButtonColor = Color.FromHex("F7B819"),
                    BorderColor = Color.FromHex("F7B819"),
                    TextColor = Color.FromHex("F7B819"),
                    WidthRequest = 50,
                    HeightRequest = 50,
                };
                AddFAB.Clicked += AddFAB_Clicked;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            StackLayout ContenidoAsistentes= new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 5, 0, 5),
                Spacing = 15,
                Children =
                {
                    AsistentesHeader,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Spacing = 0,
                        Children =
                        {
                            new Grid
                            {
                                BackgroundColor = Color.FromHex("E5E5E5"),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                Padding = new Thickness(20, 0),
                                Children = { AsistentesListView }
                            },
                            new BoxView {BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4}
                        }
                    }
                }
            };

            Contenido = new RelativeLayout();
            Contenido.Children.Add(ContenidoAsistentes,
                                   Constraint.Constant(0),
                                   Constraint.Constant(0),
                                   Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                   Constraint.RelativeToParent((parent) => { return parent.Height; }));

            Contenido.Children.Add(AddFAB,
                                   Constraint.RelativeToParent((parent) => { return (parent.Width - 61); }),
                                   Constraint.RelativeToParent((parent) => { return (parent.Height - 66); }),
                                   Constraint.Constant(50),
                                   Constraint.Constant(50));

            Content = Contenido;
        }

        void AddFAB_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AsistentesFiltradoPage(false));
        }
    }
}
