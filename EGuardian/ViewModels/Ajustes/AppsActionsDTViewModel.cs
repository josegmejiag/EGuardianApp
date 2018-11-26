using System;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Ajustes
{
    public class AppsActionsDTViewModel : ViewCell
    {
        public AppsActionsDTViewModel()
        {
            Label title = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 14,
                TextColor = Color.FromHex("3F3F3F"),
                FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null)
            };
            title.SetBinding(Label.TextProperty, "Nombre");

            Label subtitle = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                TextColor = Color.FromHex("3F3F3F"),
                FontSize = 8
            };
            subtitle.SetBinding(Label.TextProperty, "Package");
            subtitle.SetBinding(Label.IsVisibleProperty, "IsVisible");




            BoxView separador = new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("432161"), WidthRequest = 2 };
            separador.SetBinding(BoxView.BackgroundColorProperty, "BackgroundColor");

            IconView icono = new IconView
            {
                Foreground = Color.FromHex("19B877"),
                HeightRequest = 35,
                WidthRequest = 35,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
            };
            icono.SetBinding(IconView.ForegroundProperty, "BackgroundColor");
            icono.SetBinding(IconView.SourceProperty, "icono");
            icono.SetBinding(VisualElement.IsVisibleProperty, "isVisible");

            Image icono2 = new Image
            {
                Source = Images.Navegador,
                //IsVisible = false,
                HeightRequest = 35,
                WidthRequest = 35,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
            };
            icono2.SetBinding(VisualElement.IsVisibleProperty, "mostrarChrome");

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
                    WidthRequest = 80,
                    Padding = 0,
                    Spacing = 0,
                Children = { new Grid{Children = { icono2, icono,  } } }
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
                        new StackLayout
                        {
                            Padding = 0,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.Center,
                            Spacing = 0,
                            Children = { title, subtitle }
                        }
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