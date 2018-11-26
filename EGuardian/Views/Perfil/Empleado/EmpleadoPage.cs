using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EGuardian.Common;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Models.Empleados;
using EGuardian.Models.Eventos;
using EGuardian.Models.Puestos;
using EGuardian.Models.Registro;
using Plugin.Toasts;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.Views.Perfil.Empleado
{
    public class EmpleadoPage : ContentPage
    {
        ExtendedEntry nombres, apellidos;
        public ExtendedEntry correo;
        ExtendedPicker puesto, genero;
        public bool Logeado;
        Image headerBackground;
        Button guardar;
        RelativeLayout contenido2;
        Grid botonCerrar;
        StackLayout EdicionCreacion;
        ScrollView contenidoCreacionEdicion;
        List<puestos> puestos;

        public EmpleadoPage(empleados empleado)
        {
            if(string.IsNullOrEmpty(empleado.firstName))
                Title = "Nuevo empleado";
            else
                Title = "Edición de empleado";
            try
            {
                puestos = App.Database.GetPuestos().ToList();
            }
            catch (Exception ex)
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
            nombres.Text = empleado.firstName;

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
            apellidos.Text = empleado.lastName;

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
            if (puestos.Count != 0)
            {
                puesto.Items.Clear();
                foreach (var puestosT in puestos)
                {
                    puesto.Items.Add(puestosT.nombre);
                }
            }
            puesto.Focused += Puesto_Focused; ;
            genero.SelectedIndex = 0;

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
            if (!string.IsNullOrEmpty(empleado.email)) 
                puesto.SelectedIndex = 5;
            else
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
            correo.Text = empleado.email;
            if (!string.IsNullOrEmpty(empleado.email))
                correo.IsEnabled = false;
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
            if (!string.IsNullOrEmpty(empleado.email))
                guardar.Text = "ACTUALIZAR";


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

            StackLayout advertencia = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 0,
                Children =
                {
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "Al tocar el botón REGISTRAR se enviarán los datos de acceso",
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
                                Text = "al correo electrónico del empleado",
                                TextColor = Color.FromHex("B2B2B2"),
                                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                            },
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
                    Spacing = 0,
                    Children =
                    {
                        localizacion,
                        new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
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
                    await Navigation.PopAsync();
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

        private void Back_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
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
            if(guardar.Text.Equals("ACTUALIZAR"))
            {
                await Navigation.PushPopupAsync(new Indicador("Actualizando datos de empleado", Color.White));
                await Task.Delay(3000);
                ShowToast(ToastNotificationType.Success, "Actualización de empleado", "Datos de empleado actualizados con éxito.", 5);
                await Navigation.PopAsync();
                return;
            }

            await Navigation.PushPopupAsync(new Indicador("Registrando empleado", Color.White));
            try
            {
                CreateUser peticion = new CreateUser
                {
                    firstName = nombres.Text,
                    lastName = apellidos.Text,
                    email = correo.Text,
                    genero = Constants.genero[genero.Items[genero.SelectedIndex]],
                    role = 1
                };
                if (puesto.SelectedIndex == -1)
                    peticion.puesto = 0;
                else
                    peticion.puesto = puestos.Find(x => (x.nombre == puesto.Items[puesto.SelectedIndex])).id;

                var response = await App.ManejadorDatos.CreateUserAsync(peticion);
                await Navigation.PopAllPopupAsync();
                if (response)
                {
                    MessagingCenter.Send<EmpleadoPage>(this, "EmpleadoCreado");
                    ShowToast(ToastNotificationType.Success, "Registro de empleado", "Empleado registrado con éxito.", 5);
                    await Navigation.PopAsync();
                    return;
                }
                else
                {
                    ShowToast(ToastNotificationType.Error, "¡Verifique!", "Ha ocurrido algo inesperado en el registro del empleado, intentalo de nuevo.", 5);
                    return;
                }
            }
            catch
            {
                await Navigation.PopAllPopupAsync();
                ShowToast(ToastNotificationType.Error, "Nuevo empleado", "Ha ocurrido algo inesperado en el registro del empleado, inténtalo de nuevo.", 5);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetBackButtonTitle(this, String.Empty);
            NavigationPage.SetHasBackButton(this, false);
        }

    }
}