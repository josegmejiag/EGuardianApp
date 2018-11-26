using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Helpers;
using EGuardian.ViewModels.Menu;
using EGuardian.Views.Acceso;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EGuardian.Views.Menu
{
    public class Menu : ContentPage
    {
        public ExtendedListView ListViewMenu { get; private set; }
        public MenuVistaModelo modeloVista;
        Label nombreEmpresa;

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public Menu()
        {
            Icon = Images.MenuIcon;
            Title = Strings.MenuTitle;
            BindingContext = modeloVista = new MenuVistaModelo();
            BackgroundColor = Colors.MenuBackground;

            MessagingCenter.Subscribe<Views.Menu.MainPage>(this, "noAutenticado", (sender) =>
            {
                nombreEmpresa.Text = String.Empty;
            });
            MessagingCenter.Subscribe<LoginPage>(this, "Autenticado", (sender) =>
            {
                nombreEmpresa.Text = Settings.session_nombreEmpresa;
            });

            MessagingCenter.Subscribe<Registro>(this, "Autenticado", (sender) =>
            {
                nombreEmpresa.Text = Settings.session_nombreEmpresa;
            });

            MessagingCenter.Subscribe<Forget>(this, "Autenticado", (sender) =>
            {
                nombreEmpresa.Text = Settings.session_nombreEmpresa;
            });

            nombreEmpresa = new Label
            {           
                Text = Settings.session_nombreEmpresa,
                TextColor = Color.White,
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null)
            };


            ListViewMenu = new ExtendedListView
            {
                IsScrollEnable = false,
                Margin = 0,
                ItemsSource = modeloVista.Menus,
                RowHeight = Convert.ToInt32((App.DisplayScreenHeight / 13.533333333333333)),
                IsPullToRefreshEnabled = false,
                SeparatorVisibility = SeparatorVisibility.None,
                SeparatorColor = Color.White,
                HasUnevenRows = false,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            Grid Header = new Grid
            {
                AutomationId = "Settings",
                Padding = new Thickness(15, 15),
                ColumnSpacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (0, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (0, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (40, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) }
                }
            };

            Header.Children.Add(new Image
            {
                Margin = new Thickness(5, 0),
                Source = "iAvatar.png",
                WidthRequest = 75,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start
            }, 0, 0);
            Header.Children.Add(nombreEmpresa, 0, 1);

            ListViewMenu.ItemTemplate = new DataTemplate(typeof(MenuDTViewModel));

            Content = new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Spacing = 20,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    new Grid
                    {
                        HeightRequest = 150,
                        MinimumHeightRequest = 150,
                        Padding = 0,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children=
                        {
                            new Image { Source = "bk_profile.png", Aspect = Aspect.Fill},
                            Header
                        }
                    },
                    ListViewMenu
                }
            };

            ListViewMenu.SelectedItem = modeloVista.Menus[0];
            ListViewMenu.ItemSelected += ListViewMenu_ItemSelected;                                                            
        }

        private async void ListViewMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var id = (int)((EGuardian.Models.Menu.MenuItem)e.SelectedItem).Id;
            await RootPage.NavigateFromMenu(id);
        }

        private void CerrarSesion_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}