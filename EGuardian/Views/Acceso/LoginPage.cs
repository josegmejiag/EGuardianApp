using EGuardian.Controls;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Plugin.Toasts;
using Xamarin.Forms;
using EGuardian.Helpers;
using EGuardian.Models.Login;
using EGuardian.Models.Usuarios;
using EGuardian.Models.Empresas;

namespace EGuardian.Views.Acceso
{
    public class LoginPage : ContentPage
    {
        ExtendedEntry Usuario, Contrasenia;
        Label forget;
        Button login;
        public bool Logeado;

        public LoginPage()
        {
            MessagingCenter.Subscribe<Registro>(this, "Registro", (sender) =>
            {
                Usuario.Text = sender.correo.Text;
                Contrasenia.Text = sender.contrasenia.Text;
            });

            MessagingCenter.Subscribe<Forget>(this, "Forget", (sender) =>
            {
                Usuario.Text = sender.Usuario.Text;
                Contrasenia.Text = sender.contrasenia.Text;
            });

            /*MessagingCenter.Subscribe<CambioContraseniaModeloVista>(this, "Cambio", (sender) =>
            {
                Usuario.Text = sender.emailLogin;
                Contrasenia.Focus();
            });*/

            var logo = new Image
            {
                Source = "logo.png",
                WidthRequest = 150
            };
            var iUsuario = new Grid
            {
                Padding = new Thickness(3, 0, 0, 0),
                InputTransparent = true,
                Children = { new Image { Source = "email.png", WidthRequest = 15 } }
            };

            Usuario = new ExtendedEntry()
            {
                AutomationId = "LoginUser",
                Keyboard = Keyboard.Email,
                Placeholder = "Correo electrónico",
                PlaceholderColor = Color.FromHex("91a5af"),
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                TextColor = Color.FromHex("91a5af"),
                BackgroundColor = Color.White,
                Text = string.Empty,
                HeightRequest = 50,
                HasBorder = false
            };
            var gUsuario = new Grid
            {
                Padding = new Thickness(Device.OnPlatform(10, 5, 5), 0, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { Usuario }
            };

            var email = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Horizontal,
                Children = { iUsuario, gUsuario }
            };

            var iContrasenia = new Grid
            {
                InputTransparent = true,
                Padding = new Thickness(3, 0, 0, 0),
                Children = { new Image { Source = "key.png", WidthRequest = 15 } }
            };

            Contrasenia = new ExtendedEntry()
            {
                AutomationId = "LoginPass",
                Placeholder = "Contraseña",
                PlaceholderColor = Color.FromHex("91a5af"),
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                TextColor = Color.FromHex("91a5af"),
                BackgroundColor = Color.Transparent,
                Text = string.Empty,
                HeightRequest = 50,
                HasBorder = false,
            };

            var gContrasenia = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(Device.OnPlatform(10, 5, 5), 0, 0, 0),
                Children = { Contrasenia }
            };

            var pass = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Horizontal,
                Children = { iContrasenia, gContrasenia }
            };

            var ingreso = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10, 1, 10, 1),
                Spacing = 0,
                Children =
                {
                    email,
                    new BoxView {BackgroundColor= Color.FromHex("a2b3bb"), HeightRequest=0.5 },
                    pass
                }
            };

            var fIngreso = new Frame
            {
                WidthRequest = 300,
                Padding = new Thickness(0, 0, 0, 0),
                BackgroundColor = Color.White,
                OutlineColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HasShadow = false,
                Content = ingreso
            };

            login = new Button
            {
                AutomationId = "Login",
                Text = "INGRESAR",
                TextColor = Color.FromHex("373152"),
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.FromHex("ffffff"),
                CornerRadius = 7,
                WidthRequest = 300,
                HeightRequest = 50
            };
            login.Clicked += Login_Clicked;

            var indicador = new ActivityIndicator
            {
                IsVisible = true,
                IsRunning = true,
                BindingContext = this,
                Color = Color.FromHex("f7efd9"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            indicador.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            forget = new Label
            {
                Text = "Restablecer contraseña",
                TextColor = Color.FromHex("f7efd9"),
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 20,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => Forget_Clicked();
            forget.GestureRecognizers.Add(tap);

            var acceso = new StackLayout
            {
                Spacing = 40,
                Children = { login, indicador, forget }
            };

            var registro = new Button
            {
                Text = "CREAR EMPRESA",
                TextColor = Color.White,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 20,
                VerticalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.FromHex("f7b819"),
                HeightRequest = 65,
                BorderRadius = 0
            };
            registro.Clicked += Registro_Clicked;
            var content_ingreso = new StackLayout
            {
                Spacing = 50,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = { logo, fIngreso }
            };

            var content_login = new StackLayout
            {
                Spacing = 20,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = { content_ingreso, acceso }
            };
            var content = new StackLayout
            {
                Spacing = 20,
                Padding = new Thickness(0, 50, 0, 0),
                Children = { content_login, registro }
            };
            var contenido = new ScrollView
            {
                Content = content
            };
            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    new Image
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Aspect = Aspect.AspectFill,
                        Source="login_bk",
                    },
                    contenido
                }
            };
        }

        private void Registro_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Registro());
        }
        string getContrasenia()
        {
            /*System.Diagnostics.Debug.WriteLine("HASH(MD5) " + MD5.GetHashString(Contrasenia.Text.Trim() + Constantes.Salt_Text.Trim()));
            return MD5.GetHashString(Contrasenia.Text.Trim() + Constantes.Salt_Text.Trim());*/
            return Contrasenia.Text.Trim();
        }
        private async void Login_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Usuario.Text))
            {
                await DisplayAlert("", "Por favor, indique su correo electrónico ", "Aceptar");
                Usuario.Focus();
                return;
            }
            else
            {
                if (!Regex.Match(Usuario.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
                {
                    await DisplayAlert("", "Por favor, indique una dirección de correo electrónico válida", "Aceptar");
                    Usuario.Focus();
                    return;
                }
            }
            if (String.IsNullOrEmpty(Contrasenia.Text))
            {
                await DisplayAlert("", "Por favor, indique su contraseña", "Aceptar");
                Contrasenia.Focus();
                return;
            }
            this.IsBusy = true;
            login.IsEnabled = false;
            login.IsVisible = false;

            Login peticion = new Login
            {
                username = Usuario.Text,
                password = getContrasenia()
            };
            LoginResponse Session = new LoginResponse();
            Session = await App.ManejadorDatos.LoginAsync(peticion);
            if (Session==null)
            {
                ShowToast(ToastNotificationType.Error, "Inconvenientes de conexión", "Lo lamentamos, existen inconvenientes en la conexión; intente más tarde.", 7);
                login.IsVisible = true;
                login.IsEnabled = true;
                this.IsBusy = false;
            }
            else if (string.IsNullOrEmpty(Session.access_token))
            {
                ShowToast(ToastNotificationType.Error, "Verifique sus datos de inicio de sesión", "A ocurrido algo inesperado en el inicio de sesión", 5);
                login.IsVisible = true;
                login.IsEnabled = true;
                this.IsBusy = false;
            }
            else
            {
                Settings.session_access_token = Session.access_token;
                Settings.session_expires_in = Session.expires_in.ToString();
                Settings.session_username = Session.user.username;
                Settings.session_authority = Session.user.authorities[0].authority;
                usuarios usuario = App.Database.GetUser(peticion.username);
                if (usuario == null)
                {
                    usuario = new usuarios
                    {
                        email = peticion.username
                    };
                    Logeado = false;
                }
                else
                    Logeado = true;
                usuario.idUsuario = Session.user.id;
                usuario.fechaUltimoInicio = DateTime.Now.ToString();
                usuario.username = Settings.session_username;
                usuario.firstName = Session.user.firstName;
                usuario.lastName = Session.user.lastName;
                usuario.phoneNumber = Session.user.phoneNumber;
                usuario.enabled = Session.user.enabled;
                usuario.lastPasswordResetDate = Session.user.lastPasswordResetDate;
                usuario.genero = Session.user.genero;
                usuario.idPuesto = Session.puesto.idPuesto;
                usuario.nombrePuesto = Session.puesto.nombre;
                var res = App.Database.InsertUsuario(usuario);
                if (!Logeado)
                    usuario = App.Database.GetUser(peticion.username);
                Settings.session_idUsuario = usuario.id.ToString();

                empresas empresa = App.Database.GetEmpresa(Session.empresa.id);
                if (empresa == null)
                {
                    empresa = new empresas
                    {
                        idEmpresa = Session.empresa.id
                    };
                    Logeado = false;
                }
                else
                    Logeado = true;

                empresa.nombre = Session.empresa.nombre;
                empresa.direccion = Session.empresa.direccion;
                empresa.numeroColaboradores = Session.empresa.numeroColaboradores;
                empresa.telefono = Session.empresa.telefono;
                empresa.logo = Session.empresa.logo;
                empresa.descripcion = Session.empresa.descripcion;
                empresa.status = Session.empresa.status;
                res = App.Database.InsertEmpresa(empresa);
                if (!Logeado)
                    empresa = App.Database.GetEmpresa(Session.empresa.id);
                Settings.session_idEmpresa = empresa.idEmpresa.ToString();
                Settings.session_nombreEmpresa = empresa.nombre;

                this.IsBusy = false;
                MessagingCenter.Send<LoginPage>(this, "Autenticado");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Contrasenia.IsPassword = true;
        }

        private async void Forget_Clicked()
        {
            await Navigation.PushPopupAsync(new Forget());
        }

        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }
    }
}