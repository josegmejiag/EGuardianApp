using EGuardian.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Menu
{
    public class MenuDTViewModel : ExtendedViewCell
    {
        public MenuDTViewModel()
        {
            IconView iconMenu = new IconView
            {
                //WidthRequest = 25,
                HeightRequest = 26,
                HorizontalOptions = LayoutOptions.Center,
                Foreground = Color.FromHex("19164B")
            };
            iconMenu.SetBinding(IconView.SourceProperty, BaseViewModel.IconPropertyName);

            Label tituloMenu = new Label
            {
                TextColor = Color.FromHex("19164B"),
                FontSize = 16,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };
            tituloMenu.SetBinding(Label.TextProperty, BaseViewModel.TitlePropertyName);

            Grid Menu = new Grid
            {
                Padding = new Thickness(5, 10),
                ColumnSpacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (0, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (60, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                }
            };

            Menu.Children.Add(iconMenu, 0, 0);
            Menu.Children.Add(tituloMenu, 1, 0);
            View = Menu;
            SelectedBackgroundColor = Color.Transparent;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
    }
}