using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EGuardian.Common;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Helpers;
using EGuardian.Models.Puestos;
using EGuardian.Models.Usuarios;
using EGuardian.Views.Menu;
using EGuardian.Views.Perfil;
using Plugin.Toasts;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Perfil
{
    public class UsuarioDTViewModel : ContentView
    {
        ExtendedEntry nombres, apellidos, correo, direccion;
        ExtendedPicker puesto, genero;
        ExtendedDatePicker fechaNacimiento;
        Button guardar;
        int control;
        int anios, meses = 0;
        System.Globalization.CultureInfo globalizacion;
        Label edad, tipoEdad;
        ScrollView ContenidoVista;
        List<puestos> puestos;
        StackLayout Contenido;
        usuarios perfil = App.Database.GetUserById(Convert.ToInt32(Settings.session_idUsuario));

        public UsuarioDTViewModel()
        {        
            MessagingCenter.Subscribe<MainPage>(this, "DisplayAlert", (sender) =>
            {
                Focus();
                control = -1;
            });


            guardar = new Button
            {
                Text = "GUARDAR",
                FontSize = 18,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.FromHex("F7B819"),
                WidthRequest = 128,
                HeightRequest = 38,
            };
            guardar.Clicked += Guardar_Clicked;

            RelativeLayout gridBoton = new RelativeLayout
            {
                WidthRequest = 130,
                HeightRequest = 42,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
            };
            gridBoton.Children.Add(
            new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
            {
                BackgroundColor = Color.FromHex("B2B2B2"),
                CornerRadius = 6,
                HeightRequest = 40,
                WidthRequest = 128,
            }, Constraint.Constant(2), Constraint.Constant(2));
            gridBoton.Children.Add(guardar, Constraint.Constant(0), Constraint.Constant(0));



            nombres = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Text = String.IsNullOrEmpty(perfil.firstName)?String.Empty: perfil.firstName
            };

            apellidos = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Text = String.IsNullOrEmpty(perfil.lastName) ? String.Empty : perfil.lastName
            };


            genero = new ExtendedPicker
            {
                Title = "Seleccione",
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 35, 0),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };

            IconView generoDropdown = new IconView
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Source = Images.Estado,
                WidthRequest = 25,
                HeightRequest = 25,
            };
            genero.SelectedIndexChanged += (sender, e) =>
            {
                if (genero.SelectedIndex == 1)
                    generoDropdown.Foreground = Color.FromHex("E91B5C");
                else
                    generoDropdown.Foreground = Color.FromHex("0C55A2");
            };

            foreach (string generos in Constants.genero.Keys)
            {
                genero.Items.Add(generos);
            }

            if (!String.IsNullOrEmpty(perfil.genero) && perfil.genero.Equals("1"))
                genero.SelectedIndex = 1; 
            else
                genero.SelectedIndex = 0;

            correo = new ExtendedEntry
            {
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("B2B2B2"),
                HasBorder = false,
                IsEnabled = false,
                Keyboard = Keyboard.Email,
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Text = String.IsNullOrEmpty(perfil.email) ? String.Empty : perfil.email
            };

            Grid Genero = new Grid();
            Genero.Children.Add(genero);
            Genero.Children.Add(generoDropdown);
            genero.SelectedIndex = 0;

            puesto = new ExtendedPicker
            {
                Title = "Seleccione",
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 35, 0),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };

            puestos = App.Database.GetPuestos().ToList();
            if (puestos.Count != 0)
            {
                puesto.Items.Clear();
                foreach (var puestoT in puestos)
                {
                    puesto.Items.Add(puestoT.nombre);
                }
            }
            try
            {
                puesto.SelectedIndex = perfil.idPuesto;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            IconView puestoDropdown = new IconView
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Source = "idropdown.png",
                WidthRequest = 10,
                HeightRequest = 10,
                Foreground = Color.FromHex("373152")
            };

            Grid Puesto = new Grid();
            Puesto.Children.Add(puesto);
            Puesto.Children.Add(puestoDropdown);

            Grid generoPuesto = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 15,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (5, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (5, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };
            generoPuesto.Children.Add(new Label
            {
                Text = "PUESTO:",
                FontSize = 13,
                TextColor = Color.FromHex("373152"),
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            }, 0, 0);
            generoPuesto.Children.Add(new Label
            {
                Text = "GÉNERO:",
                FontSize = 13,
                TextColor = Color.FromHex("373152"),
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            }, 1, 0);
            generoPuesto.Children.Add(Puesto, 0, 2);
            generoPuesto.Children.Add(Genero, 1, 2);
            generoPuesto.Children.Add(
                new BoxView { BackgroundColor = Color.FromHex("373152"), HeightRequest = 2 }
                , 0, 4);
            generoPuesto.Children.Add(
                new BoxView { BackgroundColor = Color.FromHex("373152"), HeightRequest = 2 }
                , 1, 4);                

            StackLayout datosGenerales = new StackLayout
            {
                Padding = new Thickness(15, 15, 15, 20),
                Spacing = 15,
                BackgroundColor = Color.FromHex("E5E5E5"),
                Children =
                    {
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Children =
                            {
                                new IconView
                                {
                                    Source = "iNPadmin.png",
                                    WidthRequest = 15,
                                    Foreground = Color.FromHex("373152"),
                                    VerticalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = "Datos de Usuario",
                                    TextColor = Color.FromHex("373152"),
                                    VerticalOptions = LayoutOptions.Center,
                                    FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null),
                                    FontSize = 18
                                }
                            }
                        },
                    new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new Label
                                {
                                    Text ="NOMBRES:*",
                                    FontSize = 13,
                                    TextColor = Color.FromHex("373152"),
                                    FontAttributes = FontAttributes.Bold,
                                       FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                                },
                                new StackLayout
                                {
                                    Spacing=1,
                                    Children=
                                    {
                                        new Grid
                                        {
                                            Children=
                                            {
                                                nombres,
                                            }
                                        },
                                        new BoxView {BackgroundColor= Color.FromHex("373152"), HeightRequest=2 },
                                        new BoxView {HeightRequest=0 }
                                    }
                                }
                            }
                        },
                        new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new Label
                                {
                                    Text ="APELLIDOS:*",
                                    FontSize = 13,
                                    TextColor = Color.FromHex("373152"),
                                    FontAttributes = FontAttributes.Bold,
                                       FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                                },
                                new StackLayout
                                {
                                    Spacing=1,
                                    Children=
                                    {
                                        new Grid
                                        {
                                            Children=
                                            {
                                                apellidos,
                                            }
                                        },
                                        new BoxView {BackgroundColor= Color.FromHex("373152"), HeightRequest=2 },
                                        new BoxView {HeightRequest=0 }
                                    }
                                }
                            }
                        },
                        new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                            new StackLayout
                                {
                                    Spacing=1,
                                    Children=
                                    {
                                        generoPuesto,
                                    }
                                }
                            }
                        },
                    new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new Label
                                {
                                    Text ="CORREO ELECTRÓNICO:*",
                                    FontSize = 13,
                                    TextColor = Color.FromHex("373152"),
                                    FontAttributes = FontAttributes.Bold,
                                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                                },
                                new StackLayout
                                {
                                    Spacing=1,
                                    Children=
                                    {
                                        new Grid
                                        {
                                            Children=
                                            {
                                                correo
                                            }
                                        },
                                        new BoxView {BackgroundColor= Color.FromHex("373152"), HeightRequest=2 },
                                        new BoxView {HeightRequest=0 }
                                    }
                                }
                            }
                        }
                    }
            };



            ContenidoVista = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                Content = new StackLayout
                {
                    Spacing = 10,
                    Children =
                            {
                                new StackLayout
                                {
                                    Spacing = 0,
                                    Children =
                                    {
                                        datosGenerales,
                                        new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                                    }
                                }
                            }
                }
            };




            Contenido = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 15),
                Children = { ContenidoVista, gridBoton }
            };

            Content = Contenido;
        }
        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }
        protected async void Guardar_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(nombres.Text))
            {
                DisplayAlert("", "Por favor, indique un nombre");
                control = 0;
                return;
            }
            if (String.IsNullOrEmpty(apellidos.Text))
            {
                DisplayAlert("", "Por favor, indique un apellido");
                control = 1;
                return;
            }

            if (genero.SelectedIndex == -1)
            {
                DisplayAlert("", "Por favor, seleccione el género");
                control = 3;
                return;
            }
            if (String.IsNullOrEmpty(correo.Text))
            {
                DisplayAlert("", "Por favor, indique su correo electrónico");
                control = 4;
                return;
            }
            else
            {
                if (!Regex.Match(correo.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
                {
                    DisplayAlert("", "Por favor, indique una dirección de correo electrónico válida");
                    control = 4;
                    return;
                }
            }
            await Navigation.PushPopupAsync(new Indicador("Actualizando datos de usuario", Color.White));
            try
            {
                await Task.Delay(3000);
                await Navigation.PopAllPopupAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            ShowToast(ToastNotificationType.Success, "Usuario", "Datos de usuario actualizados exitosamente.", 7);
        }
        public void DisplayAlert(string title, string message)
        {
            string[] values = { title, message };
            MessagingCenter.Send<UsuarioDTViewModel, string[]>(this, "DisplayAlert", values);
        }
        public void Focus()
        {
            switch (control)
            {
                case 0:
                    nombres.Focus();
                    break;
                case 1:
                    apellidos.Focus();
                    break;
                case 2:
                    genero.Focus();
                    break;
                case 3:
                    correo.Focus();
                    break;
            }
        }
    }
}