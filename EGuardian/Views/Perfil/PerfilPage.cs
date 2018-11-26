using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarouselView.FormsPlugin.Abstractions;
using EGuardian.Helpers;
using EGuardian.Models.Empleados;
using EGuardian.ViewModels.Perfil;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.Views.Perfil
{
	public class PerfilPage : ContentPage
    {
        Image headerBackground;
        Grid tabs;
        Label usuario, cuenta, contrasenia, username;
        StackLayout Usuario, Cuenta, Contrasenia;
        RelativeLayout Contenido;
        CarouselViewControl CarouselContenido;

        public PerfilPage()
        {
            Title = "Perfil & Colaboradores";
            NavigationPage.SetBackButtonTitle(this, "");
            headerBackground = new Image()
            {
                Source = "headerPerfil.png",
                Aspect = Aspect.AspectFill
            };

            Contenido = new RelativeLayout();
            Contenido.Children.Add(headerBackground,
               xConstraint: Constraint.Constant(0),
               yConstraint: Constraint.Constant(0),
               widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                            heightConstraint: Constraint.Constant(100)
           );

            tabs = new Grid
            {
                HeightRequest = 40,
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = 0,
                BackgroundColor = Color.FromHex("23593A"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (40, GridUnitType.Star) }
                        },
                ColumnDefinitions = {
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                        }
            };

            usuario = new Label
            {
                TextColor = Color.FromHex("187041"),
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                Text = "USUARIO",
                FontSize = 12,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            username = new Label
            {
                TextColor = Color.FromHex("187041"),
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            /*perfiles perfil = App.database.GetPerfil(Convert.ToInt32(Settings.session_idUsuario));
            if ((perfil != null) || !string.IsNullOrEmpty(perfil.Short_Nm))
            {
                if (perfil.Short_Nm.Length <= 3)
                    username.Text = "(" + perfil.Short_Nm.ToUpper() + ")";
                else
                    username.Text = "(" + perfil.Short_Nm.Substring(0, 3).ToUpper() + ")";
            }
            else
                username.Text = "(" + Settings.session_User_Nm.Substring(0, 3).ToUpper() + ")";*/
            cuenta = new Label
            {
                TextColor = Color.White,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                Text = "EMPRESA",
                FontSize = 12,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            contrasenia = new Label
            {
                TextColor = Color.White,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                Text = "COLABORADORES",
                FontSize = 12,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };

            Usuario = new StackLayout
            {
                Spacing = 0,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    new StackLayout{VerticalOptions = LayoutOptions.CenterAndExpand, Spacing = 0, Children={usuario, username}}
                }
            };
            Cuenta = new StackLayout
            {
                Spacing = 0,
                BackgroundColor = Color.FromHex("19164B"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, Spacing = 0, Children = { cuenta, new Label { FontSize = 12 } } } }
            };
            Contrasenia = new StackLayout
            {
                Spacing = 0,
                BackgroundColor = Color.FromHex("19164B"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { new StackLayout { VerticalOptions = LayoutOptions.CenterAndExpand, Spacing = 0, Children = { contrasenia, new Label { FontSize = 12 } } } }
            };

            tabs.Children.Add(Usuario, 0, 0);
            tabs.Children.Add(Cuenta, 1, 0);
            tabs.Children.Add(Contrasenia, 2, 0);

            TapGestureRecognizer UsuarioTAP = new TapGestureRecognizer();
            TapGestureRecognizer CuentaTAP = new TapGestureRecognizer();
            TapGestureRecognizer ContraseniaTAP = new TapGestureRecognizer();
            UsuarioTAP.Tapped += (sender, e) =>
            {
                if (CarouselContenido.Position != 0)
                    CarouselContenido.Position=0;
            };
            CuentaTAP.Tapped += (sender, e) =>
            {
                if (CarouselContenido.Position != 1)
                    CarouselContenido.Position=1;
            };
            ContraseniaTAP.Tapped += (sender, e) =>
            {
                if (CarouselContenido.Position != 2)
                    CarouselContenido.Position = 2;
            };
            Usuario.GestureRecognizers.Add(UsuarioTAP);
            Cuenta.GestureRecognizers.Add(CuentaTAP);
            Contrasenia.GestureRecognizers.Add(ContraseniaTAP);

            Contenido.Children.Add(tabs,
               xConstraint: Constraint.Constant(0),
               yConstraint: Constraint.Constant(90),
               widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; })//,
                                                                                                 //heightConstraint: Constraint.Constant(30)
           );

            CarouselContenido = new CarouselViewControl
            {
                ItemsSource = new List<int> { 1, 2, 3 },
                ItemTemplate = new PerfilDTViewModel(),
                InterPageSpacing = 10,
                //HeightRequest = 70,
                Orientation = CarouselViewOrientation.Horizontal,
            };
            CarouselContenido.PositionSelected += CarouselContenido_PositionSelected;

            Contenido.Children.Add(CarouselContenido,
                        xConstraint: Constraint.Constant(0),
                        yConstraint: Constraint.Constant(130),
                        widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                        heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 130); })
                    );                                  
            Content = Contenido;

        }

        void CarouselContenido_PositionSelected(object sender, EventArgs e)
        {
            try
            {
                switch (CarouselContenido.Position)
                {
                    case 0:
                        Usuario.BackgroundColor = Color.White;
                        Cuenta.BackgroundColor = Color.FromHex("19164B");
                        Contrasenia.BackgroundColor = Color.FromHex("19164B");
                        usuario.TextColor = Color.FromHex("19164B");
                        username.TextColor = Color.FromHex("19164B");
                        cuenta.TextColor = Color.White;
                        contrasenia.TextColor = Color.White;
                        break;
                    case 1:
                        Usuario.BackgroundColor = Color.FromHex("19164B");
                        Cuenta.BackgroundColor = Color.White;
                        Contrasenia.BackgroundColor = Color.FromHex("19164B");
                        usuario.TextColor = Color.White;
                        username.TextColor = Color.White;
                        cuenta.TextColor = Color.FromHex("19164B");
                        contrasenia.TextColor = Color.White;
                        break;
                    case 2:
                        Usuario.BackgroundColor = Color.FromHex("19164B");
                        Cuenta.BackgroundColor = Color.FromHex("19164B");
                        Contrasenia.BackgroundColor = Color.White;
                        usuario.TextColor = Color.White;
                        cuenta.TextColor = Color.White;
                        username.TextColor = Color.White;
                        contrasenia.TextColor = Color.FromHex("19164B");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        protected async override void OnAppearing()
        {
            await Navigation.PopAllPopupAsync();
        }
    }
}
