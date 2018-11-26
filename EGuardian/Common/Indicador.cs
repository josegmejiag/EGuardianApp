using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace EGuardian.Common
{
    public class Indicador : PopupPage
    {
        ActivityIndicator indicadorFooterCitas;
        Grid CitasFooter;
        public Indicador(string mensaje, Color color)
        {
            indicadorFooterCitas = new ActivityIndicator
            {
                IsRunning = true,
                Color = color,
                HorizontalOptions = LayoutOptions.End,
                WidthRequest = 50
            };

            Label tituloFooterCitas = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = mensaje,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = color,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                FontSize = 25
            };
            CitasFooter = new Grid
            {
                Padding = new Thickness(5, 0, 5, 0),
                RowSpacing = 1,
                ColumnSpacing = 1,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                }
            };
            CitasFooter.Children.Add(indicadorFooterCitas, 0, 0);
            CitasFooter.Children.Add(tituloFooterCitas, 1, 0);
            Content = CitasFooter;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected virtual Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(0.5);
        }

        protected virtual Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1); ;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}
