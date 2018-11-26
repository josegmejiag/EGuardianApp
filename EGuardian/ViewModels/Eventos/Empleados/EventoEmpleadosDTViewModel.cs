using System;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Eventos.Empleados
{
    public class EventoEmpleadosDTViewModel : ExtendedViewCell
    {
        public EventoEmpleadosDTViewModel()
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
            Item.Children.Add(new IconView
            {
                Source = Images.Agregar,
                Foreground = Color.FromHex("130F4B"),
                WidthRequest = 25
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