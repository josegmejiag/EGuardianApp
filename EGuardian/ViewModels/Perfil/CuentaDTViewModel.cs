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
using EGuardian.Models.Empresas;
using EGuardian.Models.Puestos;
using EGuardian.Models.SectorNegocio;
using EGuardian.Models.Usuarios;
using EGuardian.Views.Menu;
using EGuardian.Views.Perfil;
using Plugin.Toasts;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Perfil
{
    public class CuentaDTViewModel : ContentView
    {

        ExtendedEntry nombres, apellidos, nombreEmpresa, correo, direccion;
        ExtendedPicker colaboradores, sector, puesto, genero;
        public bool Logeado;
        int control;
        Button guardar;
        StackLayout EdicionCreacion;
        ScrollView contenidoCreacionEdicion;
        usuarios cuenta = App.Database.GetUserById(Convert.ToInt32(Settings.session_idUsuario));
        empresas cuenta1 = App.Database.GetEmpresa(Convert.ToInt32(Settings.session_idEmpresa));
        List<puestos> puestos;
        List<sectores> sectores;
        public CuentaDTViewModel()
        {
            MessagingCenter.Subscribe<MainPage>(this, "DisplayAlert", (sender) =>
            {
                Focus();
                control = -1;
            });


            nombres = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Text = String.IsNullOrEmpty(cuenta.firstName) ?String.Empty: cuenta.firstName
            };



            apellidos = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                Placeholder = "Toca para ingresa",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Text = String.IsNullOrEmpty(cuenta.lastName) ? String.Empty : cuenta.lastName
            };

            nombreEmpresa = new ExtendedEntry
            {
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                FontSize = 14,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                Text = String.IsNullOrEmpty(cuenta1.nombre) ? String.Empty : cuenta1.nombre
            };


           
            direccion = new ExtendedEntry
            {
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Text = String.IsNullOrEmpty(cuenta1.direccion) ? String.Empty : cuenta1.direccion
            };

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
                Text = String.IsNullOrEmpty(cuenta.email) ? String.Empty : cuenta.email
            };

            colaboradores = new ExtendedPicker
            {
                Title = "Seleccione",
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 35, 0),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };

            foreach (string colaboradoresT in Constants.colaboradores.Keys)
            {
                colaboradores.Items.Add(colaboradoresT);
            }

            IconView colaboradoresDropdown = new IconView
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Source = "idropdown.png",
                WidthRequest = 10,
                HeightRequest = 10,
                Foreground = Color.FromHex("373152")
            };

            Grid Colaboradores = new Grid();
            Colaboradores.Children.Add(colaboradores);
            Colaboradores.Children.Add(colaboradoresDropdown);
            colaboradores.SelectedIndex = 2;

            sector = new ExtendedPicker
            {
                Title = "Seleccione",
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 35, 0),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };


            IconView sectorDropdown = new IconView
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Source = "idropdown.png",
                WidthRequest = 10,
                HeightRequest = 10,
                Foreground = Color.FromHex("373152")
            };

            Grid Sector = new Grid();
            Sector.Children.Add(sector);
            Sector.Children.Add(sectorDropdown);
            sector.SelectedIndex = 5;


            genero = new ExtendedPicker
            {
                Title = "Seleccione",
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 35, 0),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };

            foreach (string generos in Constants.genero.Keys)
            {
                genero.Items.Add(generos);
            }

            IconView generoDropdown = new IconView
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Source = "idropdown.png",
                WidthRequest = 10,
                HeightRequest = 10,
                Foreground = Color.FromHex("373152")
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
                puesto.SelectedIndex = cuenta.idPuesto;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }


            sectores = App.Database.GetSectores().ToList();
            if (sectores.Count != 0)
            {
                sector.Items.Clear();
                foreach (var sectorT in sectores)
                {
                    sector.Items.Add(sectorT.nombre);
                }
            }


            try
            {
                sector.SelectedIndex = cuenta.idPuesto;
            }
            catch (Exception ex)
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

            Grid colaboradoresSector = new Grid
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
            colaboradoresSector.Children.Add(new Label
            {
                Text = "COLABORADORES:",
                FontSize = 13,
                TextColor = Color.FromHex("373152"),
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            }, 0, 0);
            colaboradoresSector.Children.Add(new Label
            {
                Text = "SECTOR INDUSTRIA:",
                FontSize = 13,
                TextColor = Color.FromHex("373152"),
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            }, 1, 0);
            colaboradoresSector.Children.Add(Colaboradores, 0, 2);
            colaboradoresSector.Children.Add(Sector, 1, 2);
            colaboradoresSector.Children.Add(
                new BoxView { BackgroundColor = Color.FromHex("373152"), HeightRequest = 2 }
                , 0, 4);
            colaboradoresSector.Children.Add(
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
                                    Source = "iEmpresa.png",
                                    WidthRequest = 20,
                                    Foreground = Color.FromHex("373152"),
                                    VerticalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = "Datos de la empresa",
                                    TextColor = Color.FromHex("373152"),
                                    FontSize = 18,
                                    FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null),
                                    VerticalOptions = LayoutOptions.Center
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
                                    Text ="NOMBRE:*",
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
                                                nombreEmpresa,
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
                                        Text ="DIRECCIÓN:*",
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
                                                    direccion,
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
                                    colaboradoresSector
                                        }
                                    }
                                }
                            }
                                    }
            };

            StackLayout localizacion = new StackLayout
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
                                    Text = "Datos del administrador",
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




            this.BackgroundColor = Color.White;

            contenidoCreacionEdicion = new ScrollView
            {
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
                                },
                                new StackLayout
                                {
                                    Spacing = 0,
                                    Children =
                                    {
                                        localizacion,
                                        new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                                    }
                                }
                            }
                }
            };


            EdicionCreacion = new StackLayout
            {
                Padding = new Thickness(0, 15),
                Children = { contenidoCreacionEdicion, gridBoton }
            };
            Content = EdicionCreacion;



            /*if (!Constants.CargaInicial)
                Navigation.PushPopupAsync(new Indicador("Descargando datos de sistema, por favor espere.", Color.White));
            else
            {

                try
                {
                    int index = 0;
                    foreach (var country in App.Database.GetPaises().ToList())
                    {
                        paises.Add(country.nombre, country.codigo);
                        pais.Items.Add(country.nombre);
                        paisSeleccionado.Add(country.codigo, index);
                        index++;
                    }
                    index = 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    int index = 0;
                    foreach (var speciality in App.Database.GetEspecialidades().ToList())
                    {
                        especialidades.Add(speciality.nombre.ToUpper(), speciality.idEspecialidad);
                        especialidad1.Items.Add(speciality.nombre.ToUpper());
                        especialidad2.Items.Add(speciality.nombre.ToUpper());
                        especialidad1Seleccionado.Add(speciality.idEspecialidad, index);
                        especialidad2Seleccionado.Add(speciality.idEspecialidad, index);
                        index++;
                    }
                    index = 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                CargarSeleccionables();
            }*/
        }

        void Pais_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (pais.SelectedIndex > -1)
            {
                lugar.Items.Clear();
                lugarSeleccionado.Clear();
                lugares.Clear();
                try
                {
                    int index = 0;
                    foreach (var place in App.Database.GetLugares(paises[pais.Items[pais.SelectedIndex]]).ToList())
                    {
                        lugares.Add(place.nombre, place.idLugar);
                        lugar.Items.Add(place.nombre);
                        lugarSeleccionado.Add(place.idLugar, index);
                        index++;
                    }
                    index = 0;

                    if (lugar.Items.Count == 0)
                    {
                        lugar.Items.Clear();
                        lugar.Items.Add("Información no disponible");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }*/
        }
        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(nombreEmpresa.Text))
            {
                DisplayAlert("", "Por favor, indique nombre de empresa");
                control = 0;
                return;
            }
            if (String.IsNullOrEmpty(direccion.Text))
            {
                DisplayAlert("", "Por favor, indique una dirección");
                control = 1;
                return;
            }

            if (String.IsNullOrEmpty(nombres.Text))
            {
                DisplayAlert("", "Por favor, indique un nombre");
                control = 2;
                return;
            }
            if (String.IsNullOrEmpty(apellidos.Text))
            {
                DisplayAlert("", "Por favor, indique un apellido");
                control = 3;
                return;
            }
            await Navigation.PushPopupAsync(new Indicador("Actualizando datos de empresa", Color.White));
            try
            {
                await Task.Delay(3000);
                await Navigation.PopAllPopupAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            ShowToast(ToastNotificationType.Success, "Empresa", "Datos de empresa actualizados exitosamente.", 7);

        }
        public void DisplayAlert(string title, string message)
        {
            string[] values = { title, message };
            MessagingCenter.Send<CuentaDTViewModel, string[]>(this, "DisplayAlert", values);
        }

        public void CargarSeleccionables()
        {
            /*try
            {
                if (!string.IsNullOrEmpty(cuenta.Ctry_Cd))
                    pais.SelectedIndex = paisSeleccionado[cuenta.Ctry_Cd];
                else
                    pais.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                pais.SelectedIndex = -1;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            try
            {
                if (!string.IsNullOrEmpty(cuenta.Adr_Place_ID))
                    lugar.SelectedIndex = lugarSeleccionado[cuenta.Adr_Place_ID];
                else
                    lugar.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                lugar.SelectedIndex = -1;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            try
            {
                if (!string.IsNullOrEmpty(cuenta.Specialty_1))
                    especialidad1.SelectedIndex = especialidad1Seleccionado[cuenta.Specialty_1];
                else
                    especialidad1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                especialidad1.SelectedIndex = -1;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            try
            {
                if (!string.IsNullOrEmpty(cuenta.Specialty_2))
                    especialidad2.SelectedIndex = especialidad2Seleccionado[cuenta.Specialty_2];
                else
                    especialidad2.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                especialidad2.SelectedIndex = -1;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }*/

        }

        public void Focus()
        {
            switch (control)
            {
                case 0:
                    nombreEmpresa.Focus();
                    break;
                case 1:
                    direccion.Focus();
                    break;                
                case 2:
                    nombres.Focus();
                    break;
                case 3:
                    apellidos.Focus();
                    break;
            }
        }
    }
}