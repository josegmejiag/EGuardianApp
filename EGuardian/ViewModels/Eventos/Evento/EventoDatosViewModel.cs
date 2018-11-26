using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Models.Acciones;
using EGuardian.Models.Aplicaciones;
using EGuardian.Models.Empleados;
using EGuardian.Views.Eventos.Acciones;
using EGuardian.Views.Eventos.Aplicaciones;
using EGuardian.Views.Eventos.Asistentes;
using SuaveControls.Views;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Eventos.Evento
{
    public class EventoDatosViewModel : ContentView
    {
        Image AddApp, AddAction;
        DateTimeControl fechaInicio, fechaFin;
        CultureInfo globalizacion;
        ScrollView contenidoCreacionEdicion;
        ExtendedEntry asunto, lugar, idPaciente;
        bool idPacienteFocused;
        ExtendedDatePicker fecha;
        ExtendedTimePicker horaInicio, horaFin;
        DateTime hInicio = DateTime.Now;

        Grid Apps, Actions;
        ColumnDefinitionCollection AppsColumns = new ColumnDefinitionCollection();
        ColumnDefinitionCollection ActionsColumns = new ColumnDefinitionCollection();

        public EventoDatosViewModel()
        {
            idPaciente = new ExtendedEntry
            {
                Placeholder = "SELECCIONE CAPACITADOR",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                FontSize = 14,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 15, 0)
                //HeightRequest = 25,
            };

            idPaciente.Focused += IdPaciente_Focused;
            /*idPaciente.TextChanged += delegate (object sender, TextChangedEventArgs args)
            {
                pacientesLista.IsVisible = true;
                if (string.IsNullOrEmpty(idPaciente.Text))
                {
                    //pacientesLista.ItemsSource = pacientes;
                    pacientesLista.IsVisible = false;
                }

                else
                {
                    //pacientesLista.ItemsSource= pacientes.Where(t => t.nombrePila.ToLower().Contains(idPaciente.ToLower())).ToList().ForEach(t => Pacientes.Add(t));

                    var resultados = pacientes.Where(x => x.nombrePila.ToLower().Contains(idPaciente.Text.ToLower()));
                    if (resultados.Count() == 0)
                    {
                        tituloFooterPacientes.IsVisible = true;
                        pacientesLista.Footer = tituloFooterPacientes;
                        pacientesLista.ItemsSource = resultados;
                    }
                    else
                    {
                        pacientesLista.Footer = "";
                        pacientesLista.ItemsSource = resultados;

                    }
                }
            };*/

            MessagingCenter.Subscribe<AsistentesFiltradoPage>(this, "OK", (sender) =>
            {
                idPacienteFocused = false;
                empleados asistente = (empleados)sender.Pacientes.SelectedItem;
                idPaciente.Text = asistente.nombre;
                //asunto.Text = asistente.nombrePila;
                idPaciente.TextColor = Color.FromHex("3F3F3F");
                idPaciente.PlaceholderTextColor = Color.FromHex("B2B2B2");
                idPaciente.Unfocus();
                System.Diagnostics.Debug.WriteLine(idPacienteFocused);
                Constants.DatosEvento.idcapacitador = asistente.idEmpleado;
                foreach (var asis in Constants.AsistentesEvento)
                {
                    asis.rol = "Asistente";
                }
                if (Constants.AsistentesEvento.Any((asistent) => asistent.nombre.Equals(((empleados)sender.Pacientes.SelectedItem).nombre)))
                {
                    System.Diagnostics.Debug.WriteLine("Asistente ya esta en la lista");
                    Constants.AsistentesEvento.Remove((empleados)sender.Pacientes.SelectedItem); 
                }
                ((empleados)sender.Pacientes.SelectedItem).rol = "Capacitador";
                Constants.AsistentesEvento.Add((empleados)sender.Pacientes.SelectedItem);
                MessagingCenter.Send<EventoDatosViewModel>(this, "OK_B");
            });

            MessagingCenter.Subscribe<AccionesFiltradoPage>(this, "OK", (sender) =>
            {
                if(Constants.AccionesEvento.Any((action) => action.idAccion== ((acciones)sender.AccionesContenido.SelectedItem).idAccion))
                    System.Diagnostics.Debug.WriteLine("Action ya esta en la lista");
                else
                {
                    Constants.AccionesEvento.Add((acciones)sender.AccionesContenido.SelectedItem);
                    ActionsColumns.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                }
                int cnt = 0;
                Actions.Children.Clear();
                foreach(var accion in Constants.AccionesEvento)
                {
                    Actions.Children.Add(new Grid
                    {
                        WidthRequest = 25,
                        HeightRequest = 25,
                        Children =
                        {
                            new Image
                            {
                                Source = accion.icono,
                                Aspect = Aspect.AspectFit
                            }
                        }
                    }, cnt, 0);
                    cnt++;
                    System.Diagnostics.Debug.WriteLine(accion.Nombre);
                }
            });

            MessagingCenter.Subscribe<AplicacionesFiltradoPage>(this, "OK", (sender) =>
            {
                if (Constants.AplicacionesEvento.Any((app) => app.idAplicacion == ((aplicaciones)sender.AplicacionesContenido.SelectedItem).idAplicacion))
                    System.Diagnostics.Debug.WriteLine("App ya esta en la lista");
                else
                {
                    Constants.AplicacionesEvento.Add((aplicaciones)sender.AplicacionesContenido.SelectedItem);
                    AppsColumns.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                }

                int cnt = 0;
                Apps.Children.Clear();
                foreach (var aplicacion in Constants.AplicacionesEvento)
                {
                    Apps.Children.Add(new Grid
                    {
                        WidthRequest = 25,
                        HeightRequest = 25,
                        Children =
                        {
                            new Image
                            {
                                Source = aplicacion.icono,
                                Aspect = Aspect.AspectFit
                            }
                        }
                    }, cnt, 0);
                    cnt++;
                    System.Diagnostics.Debug.WriteLine(aplicacion.Nombre);
                }
            });

            idPacienteFocused = false;
            Image FiltradoPaciente = new Image
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Source = "idropdown.png",
                WidthRequest = 10
            };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine(idPacienteFocused);
                if (!idPacienteFocused && idPaciente.IsEnabled)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        idPaciente.Focus();
                    });/*
                    idPacienteFocused = true;
                    var modeloVista = new VistaModelos.Pacientes.PacientesModeloVista() { Navigation = Navigation };
                    //Navigation.PushAsync(new PacientesVista(modeloVista));
                    await Navigation.PushModalAsync(new PacientesFiltradoVista(modeloVista));
                    idPacienteFocused = false;
                    return;*/
                }
                else
                    return;
            };
            FiltradoPaciente.GestureRecognizers.Add(tapGestureRecognizer);


            AddApp = new Image
            {
                Source = "iFABPb",
                Aspect = Aspect.AspectFill,
                WidthRequest = 32,
                HeightRequest = 32,
            };

            TapGestureRecognizer AddAppTAP = new TapGestureRecognizer();
            AddAppTAP.Tapped+= AddAppTAP_Tapped;
            AddApp.GestureRecognizers.Add(AddAppTAP);

            AddAction = new Image
            {
                Source = "iFABPb",
                Aspect = Aspect.AspectFill,
                WidthRequest = 32,
                HeightRequest = 32,
            };

            TapGestureRecognizer AddActionTAP = new TapGestureRecognizer();
            AddActionTAP.Tapped += AddActionTAP_Tapped;;
            AddAction.GestureRecognizers.Add(AddActionTAP);

            asunto = new ExtendedEntry
            {
                Placeholder = "INGRESA EL NOMBRE DEL EVENTO",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                //WidthRequest = 15,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 15, 0)
            };
            asunto.SetBinding(Entry.TextProperty, "asunto");
            asunto.TextChanged += (sender, e) =>
            {
                //((ExtendedEntry)sender).Text=((ExtendedEntry)sender).Text.ToUpper();
                if (((ExtendedEntry)sender).Text.Length < 4)
                    ((ExtendedEntry)sender).TextColor = Color.Red;
                else
                    ((ExtendedEntry)sender).TextColor = Color.FromHex("3F3F3F");
            };
            asunto.Unfocused+= (sender, e) => 
            {
                Constants.DatosEvento.nombre = asunto.Text.Trim();
            };
            lugar = new ExtendedEntry
            {
                PlaceholderColor = Color.FromHex("B2B2B2"),
                TextColor = Color.FromHex("3F3F3F"),
                BackgroundColor = Color.Transparent,
                HasBorder = false,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Placeholder = "INGRESE LUGAR DEL EVENTO",
                XAlign = TextAlignment.End,
                Margin = new Thickness(0, 0, 15, 0)
            };
            lugar.TextChanged += Lugar_TextChanged;



            globalizacion = new System.Globalization.CultureInfo("es-GT");

            fecha = new ExtendedDatePicker
            {
                HasBorder = false,
                Format = globalizacion.DateTimeFormat.ShortDatePattern,
                Date = hInicio.Date,
                Margin = new Thickness(0, 5),
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("3F3F3F"),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };
            horaInicio = new ExtendedTimePicker
            {
                HasBorder = false,
                Format = globalizacion.DateTimeFormat.ShortTimePattern,
                Time = hInicio.TimeOfDay,
                Margin = new Thickness(0, 5),
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("3F3F3F"),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };
            horaInicio.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                {
                    if (horaInicio.Time.Hours.Equals(23))
                    {
                        horaFin.Time = new DateTime(hInicio.Year, hInicio.Month, hInicio.Day, horaInicio.Time.Hours, 59, 59, DateTimeKind.Local).TimeOfDay;
                    }
                    else
                    {
                        horaFin.Time = horaInicio.Time.Add(TimeSpan.FromHours(1));
                    }
                }
            };
            fechaInicio = new DateTimeControl(fecha, horaInicio);//2018-11-24T00:17:06.242Z
            Constants.DatosEvento.fechaInicio = fechaInicio.Value.ToString(@"yyyy-MM-ddTHH:mm:ss.fffZ");
            horaInicio.Unfocused+= (sender, e) => 
            {
                fechaInicio = new DateTimeControl(fecha, horaInicio);
                Constants.DatosEvento.fechaInicio = fechaInicio.Value.ToString(@"yyyy-MM-ddTHH:mm:ss.fffZ");
            };
            horaFin = new ExtendedTimePicker
            {
                HasBorder = false,
                Format = globalizacion.DateTimeFormat.ShortTimePattern,
                Time = hInicio.AddHours(1).TimeOfDay,
                Margin = new Thickness(0, 5),
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromHex("3F3F3F"),
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)

            };
            fechaFin = new DateTimeControl(fecha, horaFin);
            Constants.DatosEvento.fechaFin = fechaFin.Value.ToString(@"yyyy-MM-ddTHH:mm:ss.fffZ");
            horaFin.Unfocused+= (sender, e) => 
            {
                fechaFin = new DateTimeControl(fecha, horaFin);
                Constants.DatosEvento.fechaFin = fechaFin.Value.ToString(@"yyyy-MM-ddTHH:mm:ss.fffZ");
            };
            horaFin.PropertyChanged += async (sender, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                {
                    if (horaInicio.Time > horaFin.Time)
                    {
                        //await DisplayAlert("¡Advertencia!", "La hora final de la cita debe ser mayor a la inicial.", "Aceptar");
                        if (horaInicio.Time.Hours.Equals(23))
                        {
                            horaFin.Time = new DateTime(hInicio.Year, hInicio.Month, hInicio.Day, horaInicio.Time.Hours, 59, 59, DateTimeKind.Local).TimeOfDay;
                        }
                        else
                        {
                            horaFin.Time = horaInicio.Time.Add(TimeSpan.FromHours(1));
                        }
                        return;
                    }
                }
            };


            Grid horas = new Grid
            {
                ColumnSpacing = 10,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }

                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) }
                }
            };
            horas.Children.Add(
                new Label { Text = "Hora inicio", TextColor = Color.FromHex("3F3F3F") }, 0, 0);
            horas.Children.Add(
                new Label { Text = "Hora fin", TextColor = Color.FromHex("3F3F3F") }, 2, 0);
            horas.Children.Add(horaInicio, 0, 1);
            horas.Children.Add(
                new Image
                {
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    Source = "idropdown.png",
                    WidthRequest = 10
                }, 1, 1);
            horas.Children.Add(horaFin, 2, 1);
            horas.Children.Add(
                new Image
                {
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    Source = "idropdown.png",
                    WidthRequest = 10
                }, 3, 1);


            Grid Fechas = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 10,
                //HeightRequest = 50,
                RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (45, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };

            Grid AppsHeader = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 10,
                //HeightRequest = 50,
                RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (1, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (32, GridUnitType.Absolute) },
                }
            };

            AppsHeader.Children.Add(new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "APLICACIONES A MONITOREAR:",
                    FontSize = 13,
                    TextColor = Color.FromHex("432161"),
                    FontAttributes = FontAttributes.Bold,
                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                },0,0);

            AppsHeader.Children.Add(AddApp, 1,0);

            Grid ActionsHeader = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 10,
                //HeightRequest = 50,
                RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (45, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (32, GridUnitType.Absolute) },
                }
            };

            ActionsHeader.Children.Add(new Label
            {
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Text = "ACCIONES:",
                FontSize = 13,
                TextColor = Color.FromHex("432161"),
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            }, 0, 0);

            ActionsHeader.Children.Add(AddAction, 1, 0);


            Apps = new Grid
            {
                Padding = new Thickness(0,10),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 15,
                RowDefinitions = {
                        new RowDefinition {  Height = new GridLength (1, GridUnitType.Auto) },
                },
                ColumnDefinitions = AppsColumns
            };

            Actions = new Grid
            {
                Padding = new Thickness(0, 10),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 15,
                RowDefinitions = {
                        new RowDefinition {  Height = new GridLength (1, GridUnitType.Auto) },
                },
                ColumnDefinitions = ActionsColumns
            };

            RelativeLayout componenteInicio = new RelativeLayout();
            componenteInicio.Children.Add(
                new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                {
                    BackgroundColor = Color.FromHex("B2B2B2"),
                    CornerRadius = 6,
                },
                Constraint.Constant(-4),
                Constraint.Constant(4),
                Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
            componenteInicio.Children.Add(
                new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                {
                    BackgroundColor = Color.FromHex("E5E5E5"),
                    CornerRadius = 6,
                },
                Constraint.Constant(-4),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return (parent.Width); }),
                Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));

            Fechas.Children.Add(
                componenteInicio, 0, 0);
            Fechas.Children.Add(
                new StackLayout
                {
                    Padding = new Thickness(15, 10),
                    Spacing = 0,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        new Label
                        {
                            Text ="INICIO",
                            FontSize = 13,
                            TextColor = Color.FromHex("432161"),
                            FontAttributes = FontAttributes.Bold,
                            HorizontalOptions = LayoutOptions.Start,
                            FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                        },
                        new StackLayout
                        {
                            Spacing = 0,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            Children =
                            {
                                fecha,
                                new BoxView {BackgroundColor= Color.FromHex("432161"), HeightRequest=2},
                                horaInicio
                            }
                        }
                    }
                }, 0, 0);

            RelativeLayout componenteFinal = new RelativeLayout();
            componenteFinal.Children.Add(
                new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                {
                    BackgroundColor = Color.FromHex("B2B2B2"),
                    CornerRadius = 6,
                },
                Constraint.Constant(4),
                Constraint.Constant(4),
                Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
            componenteFinal.Children.Add(
                new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                {
                    BackgroundColor = Color.FromHex("E5E5E5"),
                    CornerRadius = 6,
                },
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
            Fechas.Children.Add(
                componenteFinal, 1, 0);
            Fechas.Children.Add(
                new StackLayout
                {
                    Padding = new Thickness(15, 10),
                    Spacing = 0,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                    new Label
                    {
                        Text ="FINAL",
                        FontSize = 13,
                        TextColor = Color.FromHex("432161"),
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.Start,
                        FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                    },
                    new StackLayout
                        {
                            Spacing = 0,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                            Children =
                            {
                            new Label
                            {
                                Text="--",
                                HorizontalOptions = LayoutOptions.Center,
                                Margin = new Thickness(0,Device.OnPlatform(5,7,0)),
                                FontSize =14,
                                TextColor = Color.FromHex("B2B2B2"),
                                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                            },
                                new BoxView {BackgroundColor= Color.FromHex("432161"), HeightRequest=2},
                                horaFin
                            }
                        }
                    }
                }, 1, 0);

            contenidoCreacionEdicion = new ScrollView
            {
                Padding = new Thickness(0, 0, 0, 15),
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
                                new StackLayout
                                {
                                    Padding = new Thickness(15,5,15,10),
                                    Spacing = 0,
                                    BackgroundColor = Color.FromHex("E5E5E5"),
                                    Children =
                                    {
                                        /*new StackLayout
                                        {
                                            Orientation = StackOrientation.Horizontal,
                                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                                            Children =
                                            {
                                                new IconView
                                                {
                                                    Source = "iNCinfo.png",
                                                    WidthRequest = 15,
                                                    Foreground = Color.FromHex("432161"),
                                                    VerticalOptions = LayoutOptions.Center
                                                },
                                                new Label
                                                {
                                                    Text = "Datos de evento",
                                                    TextColor = Color.FromHex("432161"),
                                                    FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null),
                                                    FontSize = 18,
                                                    VerticalOptions = LayoutOptions.Center
                                                }
                                            }
                                        },*/
                                        new StackLayout
                                        {
                                            Spacing = 0,
                                            Children =
                                            {
                                                new Label
                                                {
                                                    Text ="NOMBRE DEL EVENTO:*",
                                                    FontSize = 13,
                                                    TextColor = Color.FromHex("432161"),
                                                    FontAttributes = FontAttributes.Bold,
                                                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                                                },
                                                new StackLayout
                                                {
                                                    Spacing=1,
                                                    Children=
                                                    {
                                                        asunto,
                                                        new BoxView {BackgroundColor= Color.FromHex("432161"), HeightRequest=2 },
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                            }
                        },
                        new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new StackLayout
                                {
                                    Padding = new Thickness(15,5,15,10),
                                    Spacing = 0,
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
                                                    Text ="DIRECCIÓN:*",
                                                    FontSize = 13,
                                                    TextColor = Color.FromHex("432161"),
                                                    FontAttributes = FontAttributes.Bold,
                                                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                                                },
                                                new StackLayout
                                                {
                                                    Spacing=1,
                                                    Children=
                                                    {
                                                        lugar,
                                                        new BoxView {BackgroundColor= Color.FromHex("432161"), HeightRequest=2 },
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                            }
                        },
                        new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new StackLayout
                                {
                                    Padding = new Thickness(15,5,15,10),
                                    Spacing = 0,
                                    BackgroundColor = Color.FromHex("E5E5E5"),
                                    Children =
                                    {
                                        new StackLayout
                                        {
                                            Spacing = 0,
                                            Children=
                                            {
                                                new Label
                                                {
                                                    Text ="CAPACITADOR:*",
                                                    TextColor = Color.FromHex("432161"),
                                                    FontSize = 13,
                                                    FontAttributes = FontAttributes.Bold,
                                                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                                                },
                                                new StackLayout
                                                {
                                                    Spacing=1,
                                                    Children=
                                                    {
                                                        //entry, myListView ,
                                                        new Grid
                                                        {
                                                            Children=
                                                            {
                                                                new StackLayout
                                                                {
                                                                    Children=
                                                                    {
                                                                        idPaciente,
                                                                        //pacientesLista
                                                                    }
                                                                },
                                                                FiltradoPaciente
                                                            }
                                                        },
                                                        new BoxView {BackgroundColor= Color.FromHex("432161"), HeightRequest=2 },
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                            }
                        },
                        Fechas,
                        new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new StackLayout
                                {
                                    Padding = new Thickness(15,5),
                                    BackgroundColor = Color.FromHex("E5E5E5"),
                                    Children =
                                    {
                                        new StackLayout
                                        {
                                            Spacing = 0,
                                            Children =
                                            {
                                                AppsHeader,
                                                Apps
                                            }
                                        }
                                    }
                                },
                                new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                            }
                        },
                        new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new StackLayout
                                {
                                    Padding = new Thickness(15,5),
                                    BackgroundColor = Color.FromHex("E5E5E5"),
                                    Children =
                                    {
                                        new StackLayout
                                        {
                                            Spacing = 0,
                                            Children =
                                            {
                                                ActionsHeader,
                                                Actions
                                            }
                                        }
                                    }
                                },
                                new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                            }
                        }
                    }
                }
            };

            Content = contenidoCreacionEdicion;
        }

        async void IdPaciente_Focused(object sender, FocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(idPacienteFocused);
            if (!idPacienteFocused)
            {
                idPacienteFocused = true;
                await Navigation.PushModalAsync(new AsistentesFiltradoPage(true));
                idPaciente.Unfocus();
                idPacienteFocused = false;
                return;
            }
            else
            {
                idPaciente.Unfocus();
                return;
            }

        }

        async void AddActionTAP_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AccionesFiltradoPage());
        }


        async void AddAppTAP_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AplicacionesFiltradoPage());
        }

        void Lugar_TextChanged(object sender, TextChangedEventArgs e)
        {
            lugar.Text = e.NewTextValue.ToUpper();
        }
    }
}
