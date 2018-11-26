using System;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Eventos
{
    public class HeaderDTViewModel : ContentView
    {
        public HeaderDTViewModel()
        {
            Label diaSemana = new Label
            {
                TextColor = Color.FromHex("F7B819"),
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null),
            };
            Label fecha = new Label
            {
                TextColor = Color.White,
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
            };
            diaSemana.SetBinding(Label.TextProperty, "diaSemana");
            fecha.SetBinding(Label.TextProperty, "fecha");

            Content = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 0,
                Children =
                {
                    new StackLayout
                    {
                        Padding = new Thickness(20, 0),
                        Spacing = 0,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Children = {diaSemana, fecha }

                    },
                }
            };
        }
    }
}