using System;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Models.Empleados;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Perfil.Empleado
{
    public class EmpleadoDTViewModel : ExtendedViewCell
    {
        public empleados contexto;
        public EmpleadoDTViewModel()
        {
            Label Nombre = new Label
            {
                TextColor = Color.FromHex("2C2895"),
                FontSize = 12,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };
            Nombre.SetBinding(Label.TextProperty, "nombre");
            Nombre.SetBinding(Label.TextColorProperty, "colorAsistente");

            Label Puesto = new Label
            {
                TextColor = Color.FromHex("828282"),
                FontSize = 10,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };
            Puesto.SetBinding(Label.TextProperty, "puesto");

            Label Email = new Label
            {
                TextColor = Color.FromHex("828282"),
                FontSize = 10,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };
            Email.SetBinding(Label.TextProperty, "email");

            IconView editar = new IconView
            {
                Source = Images.Editar,
                Foreground = Color.FromHex("130F4B"),
                WidthRequest = 25
            };
            /*MessagingCenter.Subscribe<EventoPage>(this, "deseleccionar", (sender) =>
            {
                isSeleccionado = false;
            });*/
            IconView eliminar = new IconView
            {
                Source = Images.Cancelar,
                Foreground = Color.FromHex("130F4B"),
                WidthRequest = 20
            };
            TapGestureRecognizer tapEliminar = new TapGestureRecognizer();

            tapEliminar.Tapped += async (sender, e) =>
            {
                try
                {
                    contexto = (empleados)BindingContext;
                    MessagingCenter.Send<EmpleadoDTViewModel>(this, "EliminarEmpleado");
                }
                catch (Exception er)
                {
                    System.Diagnostics.Debug.WriteLine(er);
                }
            };

            eliminar.GestureRecognizers.Add(tapEliminar);

            Grid Acciones = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                RowSpacing = 0,
                ColumnSpacing = 15,
                RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (1, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                }
            };
            Acciones.Children.Add(editar, 0, 0);
            Acciones.Children.Add(eliminar, 1, 0);

            Grid Item = new Grid
            {
                HeightRequest = 54,
                Padding = new Thickness(5, 5, 15, 0),
                BackgroundColor = Color.Transparent,
                ColumnSpacing = 0,
                RowSpacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (54, GridUnitType.Absolute) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) }
                }
            };

            Item.Children.Add(new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 0,
                Children =
                {
                    Nombre,
                    Email,
                    Puesto
                }
            }, 0, 0);
            Item.Children.Add(new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    Acciones
                }
            }, 1, 0);

            View = new StackLayout
            {
                HeightRequest = 55,
                Padding = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
                Spacing = 0,
                Children =
                {
                    Item,
                    new BoxView { BackgroundColor = Color.FromHex("C8C8C8"), HeightRequest = 1 },
                    new BoxView { BackgroundColor = Color.Transparent, HeightRequest = 0 }
                }
            };
            SelectedBackgroundColor = Color.Transparent;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
    }
}