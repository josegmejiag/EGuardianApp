using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EGuardian.Common;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Helpers;
using EGuardian.Models.Empresas;
using EGuardian.Models.Login;
using EGuardian.Models.Puestos;
using EGuardian.Models.Registro;
using EGuardian.Models.SectorNegocio;
using EGuardian.Models.Usuarios;
using Plugin.Toasts;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.Views.Acceso
{
	public class Registro : ContentPage
    {
        ExtendedEntry nombres, apellidos, contraseniaConfirmacion, nombreEmpresa;
        public ExtendedEntry correo, contrasenia;
        ExtendedEntry direccion;
        ExtendedPicker puesto, genero, colaboradores, sector;
        public bool Logeado;
        Image headerBackground;
        Button guardar;
        RelativeLayout contenido2;
        Grid botonCerrar;
        StackLayout EdicionCreacion;
        ScrollView contenidoCreacionEdicion;
        List<puestos> puestos;
        List<sectores> sectores;

        public Registro()
        {
            try
            {
                puestos = App.Database.GetPuestos().ToList();
                sectores = App.Database.GetSectores().ToList();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }

            MessagingCenter.Subscribe<App>(this, "CargaInicialPuestos", (sender) =>
            {
                puestos = null;
                puestos = App.Database.GetPuestos().ToList();
                if (puestos.Count != 0)
                {
                    puesto.Items.Clear();
                    foreach (var puestoT in puestos)
                    {
                        puesto.Items.Add(puestoT.nombre);
                    }
                }
            });

            MessagingCenter.Subscribe<App>(this, "CargaInicialSectores", (sender) =>
            {
                sectores = null;
                sectores = App.Database.GetSectores().ToList();
                if (sectores.Count != 0)
                {
                    sector.Items.Clear();
                    foreach (var sectorT in sectores)
                    {
                        sector.Items.Add(sectorT.nombre);
                    }
                }
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
            };

            apellidos = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14
            };

            contrasenia = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                IsPassword = false,
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Margin = new Thickness(0, 0, 35, 0)
            };

            contraseniaConfirmacion = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                IsPassword = false,
                Placeholder = "Toca para confirmar contraseña",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Margin = new Thickness(0, 0, 35, 0)
            };

            contrasenia.TextChanged += (sender, e) =>
            {
                if (!String.IsNullOrEmpty(contraseniaConfirmacion.Text) && !contraseniaConfirmacion.Text.Equals(contrasenia.Text))
                {
                    contraseniaConfirmacion.TextColor = Color.FromHex("E9242A");
                    contrasenia.TextColor = Color.FromHex("3F3F3F");
                }
                else
                {
                    contrasenia.TextColor = Color.FromHex("53A946");
                    contraseniaConfirmacion.TextColor = Color.FromHex("53A946");
                }
            };

            contraseniaConfirmacion.TextChanged += (sender, e) =>
            {
                if (!contraseniaConfirmacion.Text.Equals(contrasenia.Text))
                {
                    contraseniaConfirmacion.TextColor = Color.FromHex("E9242A");
                    contrasenia.TextColor = Color.FromHex("3F3F3F");
                }
                else
                {
                    contrasenia.TextColor = Color.FromHex("53A946");
                    contraseniaConfirmacion.TextColor = Color.FromHex("53A946");
                }
            };

            IconView contraseniaView = new IconView
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Source = "iPassView.png",
                Foreground = Color.FromHex("B2B2B2"),
                WidthRequest = 25
            };

            IconView contraseniaConfirmacionView = new IconView
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Source = "iPassView.png",
                Foreground = Color.FromHex("B2B2B2"),
                WidthRequest = 25
            };

            TapGestureRecognizer contraseniaViewTAP = new TapGestureRecognizer();
            TapGestureRecognizer contraseniaConfirmacionViewTAP = new TapGestureRecognizer();
            contraseniaViewTAP.Tapped += (sender, e) =>
            {
                if (contrasenia.IsPassword)
                {
                    contrasenia.IsPassword = false;
                    contraseniaView.Foreground = Color.FromHex("373152");

                }
                else
                {
                    contrasenia.IsPassword = true;
                    contraseniaView.Foreground = Color.FromHex("B2B2B2");
                }
            };

            contraseniaConfirmacionViewTAP.Tapped += (sender, e) =>
            {
                if (contraseniaConfirmacion.IsPassword)
                {
                    contraseniaConfirmacion.IsPassword = false;
                    contraseniaConfirmacionView.Foreground = Color.FromHex("373152");
                }
                else
                {
                    contraseniaConfirmacion.IsPassword = true;
                    contraseniaConfirmacionView.Foreground = Color.FromHex("B2B2B2");
                }
            };
            contraseniaView.GestureRecognizers.Add(contraseniaViewTAP);
            contraseniaConfirmacionView.GestureRecognizers.Add(contraseniaConfirmacionViewTAP);

            nombreEmpresa = new ExtendedEntry
            {
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                FontSize = 14,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };
            
            direccion = new ExtendedEntry
            {
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14
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
            colaboradores.SelectedIndex = -1;

            sector = new ExtendedPicker
            {
                Title = "Seleccione",
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 35, 0),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };
            sector.Focused+= Sector_Focused;;
            sector.Items.Add("Información no disponible");
            if (sectores.Count!=0)
            {
                sector.Items.Clear();
                foreach (var sectorT in sectores)
                {
                    sector.Items.Add(sectorT.nombre);
                }
            }

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
            sector.SelectedIndex = -1;

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
            puesto.Items.Add("Información no disponible");
            if(puestos.Count!=0)
            {
                puesto.Items.Clear();
                foreach (var puestosT in puestos)
                {
                    puesto.Items.Add(puestosT.nombre);
                }
            }
            puesto.Focused+= Puesto_Focused;;


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
            puesto.SelectedIndex = -1;

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

            correo = new ExtendedEntry
            {
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                Keyboard = Keyboard.Email,
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14
            };                            

            guardar = new Button
            {
                Text = "REGISTRAR",
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

            Label terminosCondiciones = new Label
            {
                Text = "términos y condiciones",
                FontSize = 11,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                TextColor = Color.FromHex("8D72B3")
            };
            StackLayout advertencia = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 0,
                Children =
                {
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "Al hacer clic sobre el botón REGISTRAR,",
                        FontSize = 11,
                        TextColor = Color.FromHex("B2B2B2"),
                        FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                        HorizontalTextAlignment = TextAlignment.Center
                    },
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 0,
                        Children =
                        {
                            new Label
                            {
                                FontSize = 11,
                                Text = "acepas nuestras políticas de ",
                                TextColor = Color.FromHex("B2B2B2"),
                                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                            },
                            terminosCondiciones
                        }
                    }
                }
            };

            TapGestureRecognizer terminosCondicionesTAP = new TapGestureRecognizer();
            terminosCondicionesTAP.Tapped += (s, e) =>
            {
                Device.OpenUri(new Uri("https://google.com.gt/"));
            };
            terminosCondiciones.GestureRecognizers.Add(terminosCondicionesTAP);

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
                        },
                        new StackLayout
                            {
                                Spacing = 0,
                                Children =
                                {
                                    new Label
                                    {
                                        Text ="CONTRASEÑA:*",
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
                                                    contrasenia,
                                                    contraseniaView
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
                                        Text ="CONFIRMAR CONTRASEÑA:*",
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
                                                    contraseniaConfirmacion,
                                                    contraseniaConfirmacionView
                                                }
                                            },
                                            new BoxView {BackgroundColor= Color.FromHex("373152"), HeightRequest=2 },
                                            new BoxView {HeightRequest=0 }
                                        }
                                    }
                                }
                            },
                            new BoxView {HeightRequest=25 },
                            advertencia
                    }
            };
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


            contenido2 = new RelativeLayout();
            headerBackground = new Image()
            {
                Source = "headerRegistro.png",
                Aspect = Aspect.AspectFill
            };

            IconView cerrar = new IconView
            {
                Margin = new Thickness(0, 0, 10, 0),
                HorizontalOptions = LayoutOptions.End,
                Source = Images.Cancelar,
                Foreground = Color.FromHex("E9242A"),
                WidthRequest = 15,
                HeightRequest = 15
            };
            TapGestureRecognizer cerrarTAP = new TapGestureRecognizer();
            cerrarTAP.Tapped += async (sender, e) =>
            {
                bool accion = await DisplayAlert("", "¿Desea cerrar sin guardar los cambios?", "Cerrar", "Cancelar");
                if (accion)
                    await Navigation.PopModalAsync();
            };
            cerrar.GestureRecognizers.Add(cerrarTAP);
            EdicionCreacion = new StackLayout
            {
                Padding = new Thickness(0, 5, 0, 15),
                Spacing = 15,
                Children = { contenidoCreacionEdicion, gridBoton }
            };

            botonCerrar = new Grid
            {
                HeightRequest = 35,
                WidthRequest = 80,
                Children =
                    {
                        new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                        {
                            BackgroundColor = Color.White,
                            CornerRadius = 6
                        },
                        cerrar
                    }
            };
            botonCerrar.GestureRecognizers.Add(cerrarTAP);


            contenido2.Children.Add(headerBackground,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(0),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                    heightConstraint: Constraint.Constant(120)
                   );

            contenido2.Children.Add(botonCerrar,
                                    xConstraint: Constraint.Constant(-15),
                                    yConstraint: Constraint.Constant(20)
               );

            contenido2.Children.Add(
                    EdicionCreacion,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(120),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                    heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 120); })
                   );

            Content = contenido2;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            contrasenia.IsPassword = true;
            contraseniaConfirmacion.IsPassword = true;
        }

        void Puesto_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                if (puestos.Count != 0)
                    puestos = App.Database.GetPuestos().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }

        void Sector_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                if (sectores.Count != 0)
                    sectores = App.Database.GetSectores().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }

        private async void Login(string email, string password)
        {
             await Navigation.PushPopupAsync(new Indicador("Iniciando sesión", Color.White));
            Login peticion = new Login
            {
                username = email,
                password = password
            };
            LoginResponse Session = new LoginResponse();
            Session = await App.ManejadorDatos.LoginAsync(peticion);
            if (Session == null)
            {
                await Navigation.PopAllPopupAsync();
                ShowToast(ToastNotificationType.Error, "Inconvenientes de conexión", "Tu registro fue exitoso, sin embargo existen inconvenientes en la conexión; intente iniciar sesión más tarde.", 7);
                MessagingCenter.Send<Registro>(this, "Registro");
                await Navigation.PopModalAsync();
            }
            else if (string.IsNullOrEmpty(Session.access_token))
            {
                await Navigation.PopAllPopupAsync();
                ShowToast(ToastNotificationType.Error, "Registro exitoso", "A ocurrido algo inesperado en el inicio de sesión; intente iniciar sesión más tarde.", 5);
                MessagingCenter.Send<Registro>(this, "Registro");
                await Navigation.PopModalAsync();
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

                ShowToast(ToastNotificationType.Success, "¡Bienvenido!", "Tu registro fue exitoso, nos estamos preparando para tu primer uso.", 7);
                MessagingCenter.Send<Registro>(this, "Autenticado");
            }
        }

        string getContrasenia()
        {
			/*System.Diagnostics.Debug.WriteLine("HASH(MD5) " + MD5.GetHashString(contrasenia.Text.Trim() + Constantes.Salt_Text.Trim()));
            return MD5.GetHashString(contrasenia.Text.Trim() + Constantes.Salt_Text.Trim());*/
			return contrasenia.Text.Trim();
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(nombreEmpresa.Text))
            {
                await DisplayAlert("", "Por favor, indique nombre de la empresa", "Aceptar");
                nombreEmpresa.Focus();
                return;
            }

            if (String.IsNullOrEmpty(direccion.Text))
            {
                await DisplayAlert("", "Por favor, indique una dirección", "Aceptar");
                direccion.Focus();
                return;
            }

            if (String.IsNullOrEmpty(nombres.Text))
            {
                await DisplayAlert("", "Por favor, indique un nombre", "Aceptar");
                nombres.Focus();
                return;
            }
            if (String.IsNullOrEmpty(apellidos.Text))
            {
                await DisplayAlert("", "Por favor, indique un apellido", "Aceptar");
                apellidos.Focus();
                return;
            }
            if (genero.SelectedIndex == -1)
            {
                await DisplayAlert("", "Por favor, seleccione el género", "Aceptar");
                genero.Focus();
                return;
            }
            if (String.IsNullOrEmpty(correo.Text))
            {
                await DisplayAlert("", "Por favor, indique su correo electrónico ", "Aceptar");
                correo.Focus();
                return;
            }
            else
            {
                if (!Regex.Match(correo.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
                {
                    await DisplayAlert("", "Por favor, indique una dirección de correo electrónico válida", "Aceptar");
                    correo.Focus();
                    return;
                }
            }
            if (String.IsNullOrEmpty(contrasenia.Text))
            {
                await DisplayAlert("", "Por favor, indique su contraseña", "Aceptar");
                contrasenia.Focus();
                return;
            }
            else
            {
                if (!Regex.Match(contrasenia.Text, @"^\S{6,15}$").Success)
                {
                    await DisplayAlert("", "La contraseña debe tener entre 6 y 15 caracteres.", "Aceptar");
                    contrasenia.Focus();
                    return;
                }
            }
            if (String.IsNullOrEmpty(contraseniaConfirmacion.Text))
            {
                await DisplayAlert("", "Por favor, confirme su contraseña", "Aceptar");
                contraseniaConfirmacion.Focus();
                return;
            }

            if ((!String.IsNullOrEmpty(contrasenia.Text) && !String.IsNullOrEmpty(contraseniaConfirmacion.Text)) && !contraseniaConfirmacion.Text.Equals(contrasenia.Text))
            {
                await DisplayAlert("Verifique", "Las contraseñas no coinciden", "Aceptar");
                contraseniaConfirmacion.Focus();
                return;
            }
            await Navigation.PushPopupAsync(new Indicador("Creando cuenta", Color.White));
            try
            {                               
                Signup peticion = new Signup
                {
                    nombreEmpresa = nombreEmpresa.Text,
                    direccionEmpresa = direccion.Text,
                    nombreUsuario = nombres.Text,
                    apellidosUsuario = apellidos.Text,
                    email = correo.Text,
                    contrasenia = getContrasenia(),
                    genero = Constants.genero[genero.Items[genero.SelectedIndex]]
            };

                if (colaboradores.SelectedIndex == -1)
                    peticion.numeroColaboradoresEmpresa = 0;
                else
                    peticion.numeroColaboradoresEmpresa = Constants.colaboradores[colaboradores.Items[colaboradores.SelectedIndex]];

                if (sector.SelectedIndex == -1)
                    peticion.idNegocios = new List<int>{0};
                else
                    peticion.idNegocios = new List<int>{sectores.Find(x => (x.nombre == sector.Items[sector.SelectedIndex])).id };

                if (puesto.SelectedIndex == -1)
                    peticion.puesto = 0;
                else
                    peticion.puesto = puestos.Find(x => (x.nombre == puesto.Items[puesto.SelectedIndex])).id;
                    
                var response = await App.ManejadorDatos.CrearEmpresaAsync(peticion);
                await Navigation.PopAllPopupAsync();
                if (response)
                {
                    Login(correo.Text, getContrasenia());
                    return;
                }
                else
                {
                    await DisplayAlert("¡Verifique!", "Ha ocurrido algo inesperado en tu registro, intentalo de nuevo.", "Aceptar");
                    return;
                }
            }
            catch
            {
                await Navigation.PopAllPopupAsync();
                await DisplayAlert("¡Ha ocurrido algo inesperado!", "Intentalo de nuevo", "Aceptar");
            }
        }

    }
}