using System;
using System.Collections.Generic;
using CarouselView.FormsPlugin.Abstractions;
using EGuardian.Common;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Data;
using EGuardian.Helpers;
using EGuardian.Models.Eventos;
using EGuardian.ViewModels.Eventos.Evento;
using Plugin.Toasts;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.Views.Eventos.Evento
{
    public class EventoPage : ContentPage
    {
        CarouselViewControl CarouselContenido;
        List<eventoItemSource> EventoItemSource = new List<eventoItemSource>();
        //List<pacientes> pacientes;
        Image headerBackground;
        Button aceptar, cerrar, editar, cambiarEstado;

        ExtendedDateTime fechaInicio;
        ExtendedDateTime fechaFin;
        ExtendedEditor diagnostico;

        Grid Modal, tabs, botonEditar, estadoCita;
        IconView eCita;
        ExtendedPicker estado;
        //ListView pacientesLista;
        //public List<CitaRespuesta> NuevaCita;
        //pacientes paciente = new pacientes();

        bool isContextual, isAppearing;
        public eventos evento;
        eventos eventoContexto;
        RelativeLayout Contenido;
        ScrollView contenidoVisualizacion;
        StackLayout MenuContextual, EdicionCreacion, Visualizacion, EventoDatos, EventoAsistentes;
        DateTime hInicio;
        public EventoPage(DateTime hInicio, eventos contexto)
        {
            this.eventoContexto = contexto;
            this.hInicio = hInicio;
            this.evento = new eventos
            {
                idUserCreator = eventoContexto.idUserCreator,
                idUsuario = eventoContexto.idUsuario,
                idEvento = eventoContexto.idEvento,
                //idPaciente = eventoContexto.idPaciente,
                asunto = eventoContexto.asunto,
                ubicacion = eventoContexto.ubicacion,
                estado = eventoContexto.estado,
                fechaInicio = eventoContexto.fechaInicio,
                fechaFin = eventoContexto.fechaFin,
                //diagnostico = eventoContexto.diagnostico,
                //tratamiento = eventoContexto.tratamiento,
            };
            isContextual = false;
            /*MessagingCenter.Subscribe<RootPagina>(this, "Unsubscribe", (sender) =>
            {
                MessagingCenter.Unsubscribe<PacientesFiltradoVista>(this, "OK");
            });
            MessagingCenter.Subscribe<PacientesFiltradoVista>(this, "OK", (sender) =>
            {
                idPacienteFocused = false;
                paciente = (pacientes)sender.Pacientes.SelectedItem;
                idPaciente.Text = paciente.nombrePila;
                asunto.Text = paciente.nombrePila;
                idPaciente.TextColor = Color.FromHex("3F3F3F");
                idPaciente.PlaceholderTextColor = Color.FromHex("B2B2B2");
                idPaciente.Unfocus();
                System.Diagnostics.Debug.WriteLine(idPacienteFocused);
            });*/

            var personDataTemplate = new DataTemplate(() =>
            {
                var nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, "nombrePila");
                return new ViewCell { View = nameLabel };
            });

            /*pacientesLista = new ListView()
            {
                HeightRequest = 50,
                HasUnevenRows = true,
                IsVisible = false,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = personDataTemplate
            };
            pacientesLista.ItemsSource = pacientes;
            pacientesLista.ItemSelected += (sender, e) =>
            {
                paciente = (pacientes)((ListView)sender).SelectedItem;
                idPaciente.Text = paciente.nombrePila; ;
                pacientesLista.IsVisible = false;
                //((ListView)sender).SelectedItem = null;
            };*/

            var mas = new ToolbarItem
            {
                Icon = "mas.png",
                Text = "Opciones",
                Order = ToolbarItemOrder.Primary
            };
            mas.Clicked += Mas_Clicked;



            aceptar = new Button
            {
                Text = "CREAR",
                FontSize = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.FromHex("F7B819"),
                WidthRequest = 128,
                HeightRequest = 38,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };
            aceptar.Clicked += Aceptar_Clicked;
            cerrar = new Button
            {
                Text = "ATRÁS",
                FontSize = 18,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.FromHex("F7B819"),
                WidthRequest = 128,
                HeightRequest = 38,
            };
            cerrar.Clicked += Aceptar_Clicked;
            cambiarEstado = new Button
            {
                Text = "INICIAR",
                FontSize = 18,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.FromHex("53A946"),
                WidthRequest = 128,
                HeightRequest = 38,
            };
            cambiarEstado.Clicked += CambiarEstado_Clicked;;

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
            gridBoton.Children.Add(aceptar, Constraint.Constant(0), Constraint.Constant(0));

            RelativeLayout gridBotonCambiarEstado = new RelativeLayout
            {
                WidthRequest = 130,
                HeightRequest = 42,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
            };
            gridBotonCambiarEstado.Children.Add(
            new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
            {
                BackgroundColor = Color.FromHex("B2B2B2"),
                CornerRadius = 6,
                HeightRequest = 40,
                WidthRequest = 128,
            }, Constraint.Constant(2), Constraint.Constant(2));
            gridBotonCambiarEstado.Children.Add(cambiarEstado, Constraint.Constant(0), Constraint.Constant(0));

            RelativeLayout gridBotonCerrar = new RelativeLayout
            {
                WidthRequest = 130,
                HeightRequest = 42,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
            };
            gridBotonCerrar.Children.Add(
            new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
            {
                BackgroundColor = Color.FromHex("B2B2B2"),
                CornerRadius = 6,
                HeightRequest = 40,
                WidthRequest = 128,
            }, Constraint.Constant(2), Constraint.Constant(2));
            gridBotonCerrar.Children.Add(cerrar, Constraint.Constant(0), Constraint.Constant(0));

            Grid BotonesCambiarEstadoCerrar = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 10,
                RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (1, GridUnitType.Auto) },
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };
            BotonesCambiarEstadoCerrar.Children.Add(gridBotonCerrar, 0, 0); 
            BotonesCambiarEstadoCerrar.Children.Add(gridBotonCambiarEstado, 1, 0);



            editar = new Button
            {
                Text = "Editar",
                BackgroundColor = Color.Transparent,
                WidthRequest = 130,
                HeightRequest = 40,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };
            editar.Clicked += async (sender, e) =>
            {/*
                if (idPaciente.IsEnabled && asunto.IsEnabled && lugar.IsEnabled && fecha.IsEnabled && horaInicio.IsEnabled && horaFin.IsEnabled && diagnostico.IsEnabled && estado.IsEnabled)
                {
                    bool accion = await DisplayAlert("", "¿Desea cancelar la edición sin guardar los cambios?", "Cerrar", "Cancelar");
                    if (accion)
                    {
                        asunto.Text = eventoContexto.asunto;
                        lugar.Text = eventoContexto.ubicacion;
                        estado.SelectedIndex = eventoContexto.estado;
                        fecha.Date = Convert.ToDateTime(eventoContexto.fechaInicio);
                        horaInicio.Time = Convert.ToDateTime(eventoContexto.fechaInicio).TimeOfDay;
                        horaFin.Time = Convert.ToDateTime(eventoContexto.fechaFin).TimeOfDay;
                        this.Title = "Detalle de evento";
                        editar.Text = "Editar";
                        OcultarModal();
                        idPaciente.IsEnabled = false;
                        asunto.IsEnabled = false;
                        lugar.IsEnabled = false;
                        estado.IsEnabled = false;
                        fecha.IsEnabled = false;
                        horaInicio.IsEnabled = false;
                        horaFin.IsEnabled = false;
                        aceptar.Text = "ATRÁS";
                        diagnostico.IsEnabled = false;
                        eCita.WidthRequest = 20;
                        eCita.HeightRequest = 20;
                        estadoCita.IsVisible = true;
                        Contenido.Children.Remove(botonEditar);
                        eCita.Margin = new Thickness(10, 0, 0, 0);
                        eCita.Source = Images.Editar;
                        eCita.Foreground = Color.FromHex("432161");
                        eCita.HorizontalOptions = LayoutOptions.Start;
                        Contenido.Children.Add(botonEditar,
                        xConstraint: Constraint.RelativeToView(headerBackground, (parent, view) => { return (view.Width - 55); }),
                        yConstraint: Constraint.RelativeToView(headerBackground, (parent, view) => { return (view.Height - 50); })
                           );
                        Contenido.Children.Remove(EdicionCreacion);
                        Contenido.Children.Add(
                        Visualizacion,
                           xConstraint: Constraint.Constant(0),
                           yConstraint: Constraint.Constant(120),
                           widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                        heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 120); })
                       );
                    }
                }
                else
                {
                    editar.Text = "Cancelar edición";
                    this.Title = "Edición de evento";
                    OcultarModal();
                    idPaciente.IsEnabled = true;
                    asunto.IsEnabled = true;
                    lugar.IsEnabled = true;
                    estado.IsEnabled = true;
                    fecha.IsEnabled = true;
                    horaInicio.IsEnabled = true;
                    horaFin.IsEnabled = true;
                    aceptar.Text = "GUARDAR";
                    diagnostico.IsEnabled = true;
                    eCita.Margin = new Thickness(0, 0, 10, 0);
                    eCita.HorizontalOptions = LayoutOptions.End;
                    eCita.WidthRequest = 15;
                    eCita.HeightRequest = 15;
                    eCita.Source = Images.Cancelar;
                    eCita.Foreground = Color.FromHex("E9242A");
                    estadoCita.IsVisible = false;
                    Contenido.Children.Remove(botonEditar);
                    Contenido.Children.Add(botonEditar,
                                            xConstraint: Constraint.Constant(-15),
                                            yConstraint: Constraint.Constant(20)
                       );
                    Contenido.Children.Remove(Visualizacion);
                    Contenido.Children.Add(
                    EdicionCreacion,
                           xConstraint: Constraint.Constant(0),
                           yConstraint: Constraint.Constant(120),
                           widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                        heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 120); })
                       );
                }
                */
            };
            Button eliminar = new Button
            {
                Text = "Cancelar evento",
                BackgroundColor = Color.Transparent,
                WidthRequest = 130,
                HeightRequest = 40,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };
            eliminar.Clicked += async (sender, e) =>
            {
                OcultarModal();
                var accion = await DisplayAlert("", "¿Desea cancelar el evento " + this.evento.asunto + "?", "Cancelar evento", "Atras");
                if (accion)
                {
                    //await Navigation.PushPopupAsync(new Indicador("Eliminando cita", Color.White));
                    /*Medicloud.Modelos.Citas.Delete peticion = new Medicloud.Modelos.Citas.Delete { calendarID = this.cita.calendarID };
                    await App.ManejadorDatos.DeleteAsync(peticion);
                    MessagingCenter.Send<CitaNueva_EdicionVista>(this, "Eliminar");*/
                }
            };
            MenuContextual = new StackLayout
            {
                Padding = new Thickness(0, 0),
                Spacing = 0,
                //BackgroundColor = Color.FromHex("26a5e8")
                BackgroundColor = Color.White,
                IsVisible = false,
                Children =
                {
                    editar,
                    eliminar
                }
            };

            Modal = new Grid
            {
                BackgroundColor = Color.FromHex("242a34"),
                Opacity = 0.1,
                Padding = new Thickness(0, 0, 0, 0),
                WidthRequest = 200,
                HeightRequest = 200,
            };
            var GestoModal = new TapGestureRecognizer();
            GestoModal.Tapped += (s, es) =>
            {
                OcultarModal();
            };
            Modal.GestureRecognizers.Add(GestoModal);


            tabs = new Grid
            {
                HeightRequest = 30,
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = 0,
                BackgroundColor = Color.FromHex("23593A"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (30, GridUnitType.Star) }
                        },
                ColumnDefinitions = {
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                        }
            };

            EventoDatos = new StackLayout
            {
                Spacing = 0,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand, 
                        Spacing = 0, 
                        Children=
                        {
                            new Label
                            {
                                TextColor = Color.FromHex("19164B"),
                                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                                Text = "DATOS DEL EVENTO",
                                FontSize = 12,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center,
                            }
                        }
                    }
                }
            };
            EventoAsistentes = new StackLayout
            {
                Spacing = 0,
                BackgroundColor = Color.FromHex("F7B819"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = 
                {
                    new StackLayout 
                    { 
                        VerticalOptions = LayoutOptions.CenterAndExpand, 
                        Spacing = 0, 
                        Children = 
                        {
                            new Label
                            {
                                TextColor = Color.FromHex("19164B"),
                                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                                Text = "EMPLEADOS ASISTENTES",
                                FontSize = 12,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center,
                            }
                        } 
                    }
                }
            };

            tabs.Children.Add(EventoDatos, 0, 0);
            tabs.Children.Add(EventoAsistentes, 1, 0);

            TapGestureRecognizer EventoDatosTAP = new TapGestureRecognizer();
            TapGestureRecognizer EventoAsistentesTAP = new TapGestureRecognizer();
            EventoDatosTAP.Tapped += (sender, e) =>
            {
                if (CarouselContenido.Position ==1)
                    CarouselContenido.Position = 0;
            };
            EventoAsistentesTAP.Tapped += (sender, e) =>
            {
                if (CarouselContenido.Position == 0)
                    CarouselContenido.Position = 1;
            };
            EventoDatos.GestureRecognizers.Add(EventoDatosTAP);
            EventoAsistentes.GestureRecognizers.Add(EventoAsistentesTAP);








            estado = new ExtendedPicker
            {
                Title = "Seleccione estado",
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                Margin = new Thickness(0, 0, 30, 0),
                XAlign = TextAlignment.End,
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };
            foreach (var Estado in Constants.estados)
            {
                estado.Items.Add(Estado.Value + " - " + Estado.Key.ToUpper());
            }
            estado.SelectedIndex = 0;
            diagnostico = new ExtendedEditor
            {
                Margin = new Thickness(0, 0, 15, 0),
                Text = "SELECCIONE CAPACITADOR",
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                TextColor = Color.FromHex("B2B2B2"),
                FontSize = 14,
                BackgroundColor = Color.Transparent,
                XAlign = TextAlignment.End,
            };
            diagnostico.Focused += Observaciones_Focused;
            diagnostico.Unfocused += Observaciones_Unfocused;
            diagnostico.TextChanged += Observaciones_TextChanged;
            Image diagnosticoDropdown = new Image
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Source = "idropdownEditor.png",
                WidthRequest = 10
            };

            IconView estadoDropdown = new IconView
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                Source = Images.Estado,
                Foreground = evento.estadoColor,
                WidthRequest = 25,
                HeightRequest = 25,
                //BackgroundColor = Color.Red
            };
            Grid Estados = new Grid();
            Estados.Children.Add(estado);
            Estados.Children.Add(estadoDropdown);

            /*
            IconView estadoDropdown = new IconView
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Source = "iCirculo.png",
                WidthRequest = 20
            };*/
            TapGestureRecognizer diagnosticoTAP = new TapGestureRecognizer();
            TapGestureRecognizer estadoTAP = new TapGestureRecognizer();
            diagnosticoTAP.Tapped += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    diagnostico.Focus();
                });
            };
            estadoTAP.Tapped += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    estado.Focus();
                });
            };
            diagnosticoDropdown.GestureRecognizers.Add(diagnosticoTAP);
            estadoDropdown.GestureRecognizers.Add(estadoTAP);

            BackgroundColor = Color.White;






            RelativeLayout componenteInicioVisualizacion = new RelativeLayout();
            componenteInicioVisualizacion.Children.Add(
                new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                {
                    BackgroundColor = Color.FromHex("B2B2B2"),
                    CornerRadius = 6,
                },
                Constraint.Constant(-4),
                Constraint.Constant(4),
                Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
            componenteInicioVisualizacion.Children.Add(
                new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                {
                    BackgroundColor = Color.FromHex("E5E5E5"),
                    CornerRadius = 6,
                },
                Constraint.Constant(-4),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return (parent.Width); }),
                Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));


            EventoItemSource.Add( new eventoItemSource{ id = 1, evento = this.evento });
            EventoItemSource.Add( new eventoItemSource{ id = 2, evento = this.evento });


            CarouselContenido = new CarouselViewControl
            {            
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ItemsSource = EventoItemSource,
                ItemTemplate = new EventoTabsDTModeloVista(),
                InterPageSpacing = 10,
                //HeightRequest = 70,
                Orientation = CarouselViewOrientation.Horizontal,
            };
            CarouselContenido.PositionSelected += CarouselContenido_PositionSelected;

            Contenido = new RelativeLayout();
            Contenido.Children.Add(MenuContextual,
                                        xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - MenuContextual.Width); }),
                                        yConstraint: Constraint.Constant(0));
            headerBackground = new Image()
            {
                Source = "headerEventos",
                Aspect = Aspect.AspectFill
            };
            eCita = new IconView
            {
                WidthRequest = 20,
                HeightRequest = 20,
            };

            EdicionCreacion = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 10, 0,15),
                Children = { CarouselContenido, gridBoton
                }
            };



            botonEditar = new Grid
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
                        eCita
                    }
            };
            Contenido.Children.Add(headerBackground,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(0),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                    heightConstraint: Constraint.Constant(120)
                   );
            estado.SelectedIndexChanged += (sender, e) =>
            {
                //evento.estado = estado.SelectedIndex;
                estadoDropdown.Foreground = evento.estadoColor;
            };

            if (this.evento.idEvento != 0)
            {
                /*paciente = this.cita.Paciente;
                if (!string.IsNullOrEmpty(paciente.nombrePila))
                    idPaciente.Text = paciente.nombrePila;*/
                this.Title = "Detalle de evento";
                //aceptar.IsVisible = false;
                cerrar.Text = "ATRÁS";
                eCita.Margin = new Thickness(10, 0, 0, 0);
                eCita.Source = Images.Editar;
                eCita.Foreground = Color.FromHex("432161");
                eCita.HorizontalOptions = LayoutOptions.Start;
                Contenido.Children.Add(botonEditar,
                xConstraint: Constraint.RelativeToView(headerBackground, (parent, view) => { return (view.Width - 55); }),
                yConstraint: Constraint.RelativeToView(headerBackground, (parent, view) => { return (view.Height - 50); })
                   );
                //fecha.IsEnabled = false;
                //asunto.IsEnabled = false;
                //lugar.IsEnabled = false;
                //idPaciente.IsEnabled = false;
                diagnostico.IsEnabled = false;
                estado.IsEnabled = false;
                //horaInicio.IsEnabled = false;
                //horaFin.IsEnabled = false;
                //asunto.Text = evento.asunto;
                //lugar.Text = evento.ubicacion;
                estado.SelectedIndex = evento.estado;
                //fecha.Date = Convert.ToDateTime(cita.fechaCita);
                //horaInicio.Time = Convert.ToDateTime(cita.horaCita).TimeOfDay;
                //horaFin.Time = Convert.ToDateTime(evento.fechaFin).TimeOfDay;
                /*if (!String.IsNullOrEmpty(this.evento.diagnostico) && !this.cita.diagnostico.Equals(".") && !this.cita.diagnostico.Equals("null") && !this.cita.diagnostico.Equals("NULL") && !this.cita.diagnostico.Equals("Null"))
                    diagnostico.Text = this.evento.diagnostico;
                diagnostico.TextColor = this.evento.colorobservaciones;*/
                this.ToolbarItems.Add(mas);

                estadoCita = new Grid
                {
                    HeightRequest = 35,
                    WidthRequest = 185,
                    Children =
                    {
                        new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                        {
                            BackgroundColor = Color.White,
                            CornerRadius = 6
                        },
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                            Padding = new Thickness(10,0),
                            Children=
                            {
                                new Label
                                {
                                    Text =this.evento.estadoCita.ToUpper(),
                                    FontSize = 13,
                                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    TextColor = Color.FromHex("3F3F3F"),
                                    VerticalTextAlignment = TextAlignment.Center,
                                    HorizontalOptions = LayoutOptions.End,
                                    VerticalOptions = LayoutOptions.Center
                                },
                                new IconView
                                {
                                    VerticalOptions = LayoutOptions.Center,
                                    HorizontalOptions = LayoutOptions.End,
                                    Source = Images.Estado,
                                    Foreground = this.evento.estadoColor,
                                    WidthRequest = 10,
                                    HeightRequest = 10,
                                }
                            }
                        }
                    }
                };
                Contenido.Children.Add(estadoCita,
                       xConstraint: Constraint.Constant(-15),
                    yConstraint: Constraint.RelativeToView(headerBackground, (parent, view) => { return (view.Height - 50); })
               );

                Grid AsuntoEvento = new Grid
                {
                    BackgroundColor = Color.Transparent,
                    Padding = new Thickness (20,5),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        new Label
                        {
                            Opacity = 0.7,
                            Text =this.evento.asunto + "\r\n-- "+this.evento.capacitador+" --",
                            FontSize = 13, 
                            FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null), 
                            HorizontalTextAlignment = TextAlignment.Center, 
                            VerticalTextAlignment = TextAlignment.Center,
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                        },
                    }
                };
                Contenido.Children.Add(AsuntoEvento,
                       xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                   widthConstraint : Constraint.RelativeToParent((parent) => { return parent.Width; }),
                   heightConstraint: Constraint.Constant(70)
               );

                Grid FechasVisualizacion = new Grid
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    ColumnSpacing = 10,
                    RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (60, GridUnitType.Auto) },
                },
                    ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
                };

                Grid AppsActionsVisualizacion = new Grid
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    ColumnSpacing = 10,
                    RowDefinitions = {
                    new RowDefinition {  Height = new GridLength (60, GridUnitType.Auto) },
                },
                    ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
                };
                RelativeLayout componenteAppsVisualizacion = new RelativeLayout();
                componenteAppsVisualizacion.Children.Add(
                    new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                    {
                        BackgroundColor = Color.FromHex("B2B2B2"),
                        CornerRadius = 6,
                    },
                    Constraint.Constant(-4),
                    Constraint.Constant(4),
                    Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                    Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
                componenteAppsVisualizacion.Children.Add(
                    new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                    {
                        BackgroundColor = Color.FromHex("E5E5E5"),
                        CornerRadius = 6,
                    },
                    Constraint.Constant(-4),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return (parent.Width); }),
                    Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));

                AppsActionsVisualizacion.Children.Add(
                    componenteAppsVisualizacion, 0, 0);

                Grid Apps = new Grid
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    ColumnSpacing = 5,
                    RowDefinitions = {
                        new RowDefinition {  Height = new GridLength (1, GridUnitType.Auto) },
                },
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                    }
                };
                Apps.Children.Add(new Grid
                {
                    WidthRequest = 25,
                    HeightRequest = 25,
                    Children = 
                    {
                        new Image
                        {
                            Source = Images.WhatsApp,
                            Aspect = Aspect.AspectFit
                        }
                    }
                },0,0);
                Apps.Children.Add(new Grid
                {
                    WidthRequest = 25,
                    HeightRequest = 25,
                    Children =
                    {
                        new Image
                        {
                            Source = Images.Twitter,
                            Aspect = Aspect.AspectFit
                        }
                    }
                }, 1, 0);

                Apps.Children.Add(new Grid
                {
                    WidthRequest = 25,
                    HeightRequest = 25,
                    Children =
                    {
                        new Image
                        {
                            Source = Images.Facebook,
                            Aspect = Aspect.AspectFit
                        }
                    }
                }, 2, 0);

                Apps.Children.Add(new Grid
                {
                    WidthRequest = 25,
                    HeightRequest = 25,
                    Children =
                    {
                        new Image
                        {
                            Source = Images.Youtube,
                            Aspect = Aspect.AspectFit
                        }
                    }
                }, 0, 1);

                Apps.Children.Add(new Grid
                {
                    WidthRequest = 25,
                    HeightRequest = 25,
                    Children =
                    {
                        new Image
                        {
                            Source = Images.Navegador,
                            Aspect = Aspect.AspectFit
                        }
                    }
                }, 1, 1);


                Grid Actions = new Grid
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    ColumnSpacing = 5,
                    RowDefinitions = {
                        new RowDefinition {  Height = new GridLength (1, GridUnitType.Auto) },
                },
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                    }
                };
                Actions.Children.Add(new Grid
                {
                    WidthRequest = 25,
                    HeightRequest = 25,
                    Children =
                    {
                        new Image
                        {
                            Source = Images.Apagar,
                            Aspect = Aspect.AspectFit
                        }
                    }
                }, 0, 0);
                Actions.Children.Add(new Grid
                {
                    WidthRequest = 25,
                    HeightRequest = 25,
                    Children =
                    {
                        new Image
                        {
                            Source = Images.Correo,
                            Aspect = Aspect.AspectFit
                        }
                    }
                }, 1, 0);

                Actions.Children.Add(new Grid
                {
                    WidthRequest = 25,
                    HeightRequest = 25,
                    Children =
                    {
                        new Image
                        {
                            Source = Images.SMS,
                            Aspect = Aspect.AspectFit
                        }
                    }
                }, 2, 0);

                AppsActionsVisualizacion.Children.Add(
                    new StackLayout
                    {
                        Padding = new Thickness(15, 5, 15, 10),
                        Spacing = 0,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children =
                        {
                        new StackLayout
                        {
                            Spacing = 5,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Children =
                            {
                                new StackLayout
                                {
                                    VerticalOptions = LayoutOptions.Start,
                                    Orientation = StackOrientation.Horizontal,
                                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                                    Children =
                                    {
                                        new IconView
                                        {
                                            Source = Images.Acceso,
                                            WidthRequest = 15,
                                            HeightRequest = 20,
                                            Foreground = Color.FromHex("432161"),
                                            VerticalOptions = LayoutOptions.Center
                                        }
                                    }
                                },
                                Apps
                            }
                        }
                        }
                    }, 0, 0);


                RelativeLayout componenteActionsVisualizacion = new RelativeLayout();
                componenteActionsVisualizacion.Children.Add(
                    new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                    {
                        BackgroundColor = Color.FromHex("B2B2B2"),
                        CornerRadius = 6,
                    },
                    Constraint.Constant(4),
                    Constraint.Constant(4),
                    Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                    Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
                componenteActionsVisualizacion.Children.Add(
                    new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                    {
                        BackgroundColor = Color.FromHex("E5E5E5"),
                        CornerRadius = 6,
                    },
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                    Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
                AppsActionsVisualizacion.Children.Add(
                    componenteActionsVisualizacion, 1, 0);
                AppsActionsVisualizacion.Children.Add(
                    new StackLayout
                    {
                        Padding = new Thickness(15, 5, 15, 10),
                        Spacing = 0,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children =
                        {
                    new StackLayout
                        {
                            Spacing = 5,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Children =
                            {
                                new StackLayout
                                {
                                    VerticalOptions = LayoutOptions.Start,
                                    Orientation = StackOrientation.Horizontal,
                                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                                    Children =
                                    {
                                        new IconView
                                        {
                                            Source = Images.Alertas,
                                            WidthRequest = 18,
                                            HeightRequest = 23,
                                            Foreground = Color.FromHex("432161"),
                                            VerticalOptions = LayoutOptions.Center
                                        }
                                    }
                                },
                                Actions
                            }
                        }
                        }
                    }, 1, 0);


                FechasVisualizacion.Children.Add(
                    componenteInicioVisualizacion, 0, 0);
                FechasVisualizacion.Children.Add(
                    new StackLayout
                    {
                        Padding = new Thickness(15, 5, 15, 10),
                        Spacing = 0,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children =
                        {
                        new StackLayout
                        {
                            Spacing = 0,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
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
                                            Source = Images.Calendario,
                                            WidthRequest = 15,
                                            HeightRequest = 20,
                                            Foreground = Color.FromHex("432161"),
                                            VerticalOptions = LayoutOptions.Center
                                        }
                                    }
                                },
                                new Label
                                {
                                    Text =Convert.ToDateTime(evento.FechaInicio).ToString(@"dd/MM/yyyy"),
                                    FontSize = 13,
                                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    TextColor = Color.FromHex("3F3F3F")},
                                //new BoxView {BackgroundColor= Color.FromHex("432161"), HeightRequest=1},
                            }
                        }
                        }
                    }, 0, 0);
                RelativeLayout componenteFinalVisualizacion = new RelativeLayout();
                componenteFinalVisualizacion.Children.Add(
                    new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                    {
                        BackgroundColor = Color.FromHex("B2B2B2"),
                        CornerRadius = 6,
                    },
                    Constraint.Constant(4),
                    Constraint.Constant(4),
                    Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                    Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
                componenteFinalVisualizacion.Children.Add(
                    new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                    {
                        BackgroundColor = Color.FromHex("E5E5E5"),
                        CornerRadius = 6,
                    },
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return (parent.Width + 4); }),
                    Constraint.RelativeToParent((parent) => { return (parent.Height - 4); }));
                FechasVisualizacion.Children.Add(
                    componenteFinalVisualizacion, 1, 0);
                FechasVisualizacion.Children.Add(
                    new StackLayout
                    {
                        Padding = new Thickness(15, 5, 15, 10),
                        Spacing = 0,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children =
                        {
                    new StackLayout
                        {
                            Spacing = 0,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
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
                                            Source = Images.Hora,
                                            WidthRequest = 15,
                                            HeightRequest = 20,
                                            Foreground = Color.FromHex("432161"),
                                            VerticalOptions = LayoutOptions.Center
                                        }
                                    }
                                },
                                new Label
                                {
                                    Text =this.evento.horarioCita,
                                    FontSize = 13,
                                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    TextColor = Color.FromHex("3F3F3F")
                                },
                                //new BoxView {BackgroundColor= Color.FromHex("432161"), HeightRequest=1 },
                            }
                        }
                        }
                    }, 1, 0);

                StackLayout headerAsistentes = new StackLayout
                {
                    Spacing = 10,
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                                            {
                                                new IconView
                                                {
                                                    Source = Images.Asistentes,
                                                    WidthRequest = 20,
                                                    HeightRequest = 25,
                                                    Foreground = Color.FromHex("432161"),
                                                    VerticalOptions = LayoutOptions.Center
                                                },
                                                new IconView
                                                {
                                                    VerticalOptions = LayoutOptions.End,
                                                    Source = "idropdown.png",
                                                    WidthRequest = 10,
                                                    HeightRequest = 10,
                                                    Foreground=Color.FromHex("432161")
                                                }
                                            }
                };
                ExtendedEntry BusquedaRapida = new ExtendedEntry
                {
                    Placeholder = "Buscar",
                    HasBorder = false,
                    BackgroundColor = Color.Transparent,
                    Margin = new Thickness(10, 0),
                    PlaceholderColor = Color.FromHex("808080"),
                    FontSize = 14,
                    TextColor = Color.FromHex("3F3F3F"),
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
                };

                Grid AsistentesHeader = new Grid
                {
                    IsVisible = false,
                    Padding = new Thickness(10, 0, 10, 0),
                    ColumnSpacing = 5,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
                },
                    ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },

                }
                };
                AsistentesHeader.Children.Add(
                new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                {
                    BackgroundColor = Color.White,//Color.FromHex("E5E5E5"),
                    CornerRadius = 6,
                    HeightRequest = 20,
                    //WidthRequest = 128,
                }, 0, 0);

                Image cancelar = new Image
                {
                    //Foreground = Color.FromHex("F7B819"),
                    Source = "iCancelarB.png",
                    HeightRequest = 25,
                    WidthRequest = 25,

                };

                Image buscar = new Image
                {
                    //Foreground = Color.FromHex("F7B819"),
                    Source = "iBusqueda.png",
                    HeightRequest = 25,
                    WidthRequest = 25,

                };
                AsistentesHeader.Children.Add(BusquedaRapida, 0, 0);
                AsistentesHeader.Children.Add(cancelar, 1, 0);
                AsistentesHeader.Children.Add(buscar, 2, 0);

                TapGestureRecognizer tapAsistentes = new TapGestureRecognizer();
                tapAsistentes.Tapped+= (sender, e) => 
                {
                    if (AsistentesHeader.IsVisible)
                        AsistentesHeader.IsVisible = false;
                    else
                        AsistentesHeader.IsVisible = true;
                };
                headerAsistentes.GestureRecognizers.Add(tapAsistentes);

                ListView AsistentesListView = new ListView
                {
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    //IsScrollEnable = false,
                    ItemsSource = this.evento.Asistentes,
                    ItemTemplate = new DataTemplate(typeof(AsistenteDetalleDTViewModel)),
                    Margin = 0,
                    RowHeight = 55, //Convert.ToInt32((App.DisplayScreenHeight / 13.533333333333333)),
                    IsPullToRefreshEnabled = false,
                    SeparatorVisibility = SeparatorVisibility.None,
                    SeparatorColor = Color.Transparent,
                    HasUnevenRows = false,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };
                AsistentesListView.ItemSelected+= (sender, e) => 
                {
                    DisplayAlert("Asistentes","Se redirigirá a las alertas de "+((asistentes)e.SelectedItem).nombres,"Aceptar");
                };





                contenidoVisualizacion = new ScrollView
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
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
                                    Padding = new Thickness(40,5,40,5),
                                    Spacing = 0,
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
                                                    Source = Images.Ubicacion,
                                                    WidthRequest = 15,
                                                    HeightRequest = 20,
                                                    Foreground = Color.FromHex("432161"),
                                                    VerticalOptions = LayoutOptions.Center
                                                }                                                
                                            }
                                        },
                                        new StackLayout
                                        {
                                            //Spacing = 10,
                                            Children =
                                            {
                                                    new Label {Text = !String.IsNullOrEmpty(this.evento.ubicacion)?this.evento.ubicacion.ToUpper():String.Empty, FontSize = 13, FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null), HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.FromHex("3F3F3F")},
                                                //new BoxView {BackgroundColor= Color.FromHex("432161"), HeightRequest=1 },
                                            }
                                        }
                                    }
                                },
                                new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                            }
                        },
                        FechasVisualizacion,
                        AppsActionsVisualizacion,
                        new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new StackLayout
                                {
                                    Padding = new Thickness(20,15,20,15),
                                    Spacing = 15,
                                    BackgroundColor = Color.FromHex("E5E5E5"),
                                    Children =
                                    {
                                        headerAsistentes,
                                        AsistentesHeader,
                                        AsistentesListView
                                    }
                                },
                                new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                            }
                        },
                    }
                    }
                };



                Visualizacion = new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 15)
                };
                Visualizacion.Children.Add(contenidoVisualizacion);
                Visualizacion.Children.Add(BotonesCambiarEstadoCerrar);

                Contenido.Children.Add(
                    Visualizacion,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(120),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                    heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 120); })
                   );



            }
            else
            {
                Title = "Nuevo evento";
                eCita.Margin = new Thickness(0, 0, 10, 0);
                eCita.HorizontalOptions = LayoutOptions.End;
                eCita.Source = Images.Cancelar;
                eCita.Foreground = Color.FromHex("E9242A");
                eCita.WidthRequest = 15;
                eCita.HeightRequest = 15;
                Contenido.Children.Add(botonEditar,
                                        xConstraint: Constraint.Constant(-15),
                                        yConstraint: Constraint.Constant(20)
                   );
                Contenido.Children.Add(tabs,
                   xConstraint: Constraint.Constant(0),
                   yConstraint: Constraint.Constant(90),
                   widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; })//,
                                                                                                     //heightConstraint: Constraint.Constant(30)
               );
                Contenido.Children.Add(EdicionCreacion,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(120),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                    heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 120); })
                   );
            }

            TapGestureRecognizer eCitaTAP = new TapGestureRecognizer();
            eCitaTAP.Tapped += ECitaTAP_Tapped;
            eCita.GestureRecognizers.Add(eCitaTAP);
            botonEditar.GestureRecognizers.Add(eCitaTAP);
            Content = Contenido;
        }

        void CarouselContenido_PositionSelected(object sender, PositionSelectedEventArgs e)
        {
            try
            {
                switch (CarouselContenido.Position)
                {
                    case 0:
                        EventoDatos.BackgroundColor = Color.White;
                        EventoAsistentes.BackgroundColor = Color.FromHex("F7B819");
                        break;
                    case 1:
                        EventoDatos.BackgroundColor = Color.FromHex("F7B819");
                        EventoAsistentes.BackgroundColor = Color.White;
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

        async void ECitaTAP_Tapped(object sender, EventArgs e)
        {
            if (/*idPaciente.IsEnabled && asunto.IsEnabled && lugar.IsEnabled && fecha.IsEnabled && horaInicio.IsEnabled && horaFin.IsEnabled &&*/ diagnostico.IsEnabled && estado.IsEnabled)
            {
                if (diagnostico.IsFocused)
                {
                    diagnostico.Unfocus();
                    return;
                }
                if (this.evento.idEvento != 0)
                {

                    bool accion = await DisplayAlert("", "¿Desea cancelar la edición sin guardar los cambios?", "Cerrar", "Cancelar");
                    if (accion)
                    {
                        estado.SelectedIndex = eventoContexto.estado;

                        this.Title = "Detalle de evento";
                        editar.Text = "Editar";
                        estado.IsEnabled = false;
                        aceptar.Text = "ATRÁS";
                        diagnostico.IsEnabled = false;
                        eCita.WidthRequest = 20;
                        eCita.HeightRequest = 20;
                        estadoCita.IsVisible = true;
                        Contenido.Children.Remove(botonEditar);
                        eCita.Margin = new Thickness(10, 0, 0, 0);
                        eCita.Source = Images.Editar;
                        eCita.Foreground = Color.FromHex("432161");
                        eCita.HorizontalOptions = LayoutOptions.Start;
                        Contenido.Children.Remove(tabs);
                        Contenido.Children.Add(botonEditar,
                        xConstraint: Constraint.RelativeToView(headerBackground, (parent, view) => { return (view.Width - 55); }),
                        yConstraint: Constraint.RelativeToView(headerBackground, (parent, view) => { return (view.Height - 50); })
                           );
                        Contenido.Children.Remove(EdicionCreacion);
                        Contenido.Children.Add(
                        Visualizacion,
                           xConstraint: Constraint.Constant(0),
                           yConstraint: Constraint.Constant(120),
                           widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                        heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 120); })
                       );
                    }
                }
                else
                {
                    bool accion = new bool();
                    if (/*string.IsNullOrEmpty(idPaciente.Text) && string.IsNullOrEmpty(asunto.Text) && string.IsNullOrEmpty(lugar.Text) &&*/ (string.IsNullOrEmpty(diagnostico.Text) || diagnostico.Text.Equals("INGRESE ALGUNAS OBSERVACIONES")) && estado.Items[estado.SelectedIndex].Equals("0 - PENDIENTE") /*&& fecha.Date.Day == DateTime.Now.Day && horaInicio.Time.Hours.Equals(hInicio.Hour) && horaFin.Time.Hours.Equals(hInicio.AddHours(1).Hour)*/)
                        accion = true;
                    else
                        accion = await DisplayAlert("", "¿Desea cerrar sin guardar los cambios?", "Cerrar", "Cancelar");
                    if (accion)
                        await Navigation.PopAsync();
                    return;
                }
            }
            else
            {
                this.Title = "Edición de evento";
                editar.Text = "Cancelar edición";
                /*idPaciente.IsEnabled = true;
                asunto.IsEnabled = true;
                lugar.IsEnabled = true;*/
                estado.IsEnabled = true;
                /*fecha.IsEnabled = true;
                horaInicio.IsEnabled = true;
                horaFin.IsEnabled = true;*/
                aceptar.Text = "GUARDAR";
                diagnostico.IsEnabled = true;
                eCita.Margin = new Thickness(0, 0, 10, 0);
                eCita.HorizontalOptions = LayoutOptions.End;
                eCita.WidthRequest = 15;
                eCita.HeightRequest = 15;
                eCita.Source = Images.Cancelar;
                eCita.Foreground = Color.FromHex("E9242A");
                estadoCita.IsVisible = false;
                Contenido.Children.Remove(botonEditar);
                Contenido.Children.Add(botonEditar,
                                        xConstraint: Constraint.Constant(-15),
                                        yConstraint: Constraint.Constant(20)
                   );
                Contenido.Children.Add(tabs,
                   xConstraint: Constraint.Constant(0),
                   yConstraint: Constraint.Constant(90),
                   widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; })//,
                                                                                                     //heightConstraint: Constraint.Constant(30)
               );
                Contenido.Children.Remove(Visualizacion);
                Contenido.Children.Add(
                EdicionCreacion,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(120),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                    heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 120); })
                   );
                /*Contenido.Children.Remove(Visualizacion);
                Contenido.Children.Add(
                EdicionCreacion,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(120),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                    heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 120); })
                   );*/
            }
        }

        void CambiarEstado_Clicked(object sender, EventArgs e)
        {
        }


        void OcultarModal()
        {
            MenuContextual.IsVisible = false;
            isContextual = false;
            Contenido.Children.Remove(MenuContextual);
            Contenido.Children.Remove(Modal);
        }



        void Mas_Clicked(object sender, EventArgs e)
        {
            if (!isContextual)
            {
                MenuContextual.IsVisible = true;
                Contenido.Children.Remove(MenuContextual);
                Contenido.Children.Remove(Modal);
                isContextual = true;
                Contenido.Children.Add(Modal,
                        xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(0),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                       heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; })
                   );
                Contenido.Children.Add(MenuContextual,
                                        xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - MenuContextual.Width); }),
                                        yConstraint: Constraint.Constant(0));
            }
            else
            {
                MenuContextual.IsVisible = false;
                isContextual = false;
                Contenido.Children.Remove(MenuContextual);
                Contenido.Children.Remove(Modal);

            };
        }

        private void Observaciones_Unfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
            {
                if (diagnostico.Text.Equals("") || string.IsNullOrEmpty(diagnostico.Text))
                {
                    diagnostico.Text = "INGRESE ALGUNAS OBSERVACIONES";
                    diagnostico.TextColor = Color.FromHex("B2B2B2");
                    diagnostico.XAlign = TextAlignment.End;
                }
            }
        }

        private void Observaciones_Focused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                if (diagnostico.Text.Equals("INGRESE ALGUNAS OBSERVACIONES"))
                {
                    diagnostico.Text = string.Empty;
                    diagnostico.TextColor = Color.FromHex("3F3F3F");
                    diagnostico.XAlign = TextAlignment.End;
                }
            }
        }



        private void Observaciones_TextChanged(object sender, TextChangedEventArgs e)
        {
            diagnostico.InvalidateLayout();
            diagnostico.XAlign = TextAlignment.End;
        }



        private async void Aceptar_Clicked(object sender, EventArgs e)
        {
            //if (!idPaciente.IsEnabled && !asunto.IsEnabled && !lugar.IsEnabled && !fecha.IsEnabled && !horaInicio.IsEnabled && !horaFin.IsEnabled && !diagnostico.IsEnabled && !estado.IsEnabled)
            /*{
                await Navigation.PopAsync();
                return;
            }*/

            /*else {
                 if (String.IsNullOrEmpty(asunto.Text) || asunto.Text.Length < 4)
                 {
                     await DisplayAlert("", "Por favor, ingrese asunto de evento.", "Aceptar");
                     asunto.PlaceholderTextColor = Color.Red;
                     asunto.TextColor = Color.Red;
                     return;
                 }
                 if (String.IsNullOrEmpty(lugar.Text))
                 {
                     await DisplayAlert("", "Por favor, ingrese lugar de evento.", "Aceptar");
                     lugar.PlaceholderTextColor = Color.Red;
                     return;
                 }
                 if (estado.SelectedIndex == -1)
                 {
                     await DisplayAlert("", "Por favor, selecciona un estado.", "Aceptar");
                     return;
                 }
                 if (horaInicio.Time.Hours.Equals(0) || horaFin.Time.Hours.Equals(0))
                 {
                     var accion = await DisplayAlert("Confirmación de horario", "¿Desea registrar la cita para las 0 horas?", "Si", "No");
                     if (!accion) return;
                 }
                 /*
                 if (diagnostico.Text.Equals("Ingrese alguna observación") || String.IsNullOrEmpty(diagnostico.Text))
                 {
                     await DisplayAlert("", "Por favor, ingrese alguna observación de la cita.", "Aceptar");
                     diagnostico.Focus();
                     return;
                 }*/
            await Navigation.PushPopupAsync(new Indicador("Guardando evento", Color.White));

            RegistrarEvento peticion = new RegistrarEvento
            {
                idEmpresa = Convert.ToInt32(Settings.session_idEmpresa),
                nombre=Constants.DatosEvento.nombre,
                idcapacitador= Constants.DatosEvento.idcapacitador,
                fechaInicio= Constants.DatosEvento.fechaInicio,
                fechaFin= Constants.DatosEvento.fechaFin
            };

            foreach(var app in Constants.AplicacionesEvento)
            {
                peticion.aplicaciones.Add(app.idAplicacion);
            }

            foreach (var action in Constants.AccionesEvento)
            {
                peticion.acciones.Add(action.idAccion);
            }

            foreach (var asistente in Constants.AsistentesEvento)
            {
                peticion.usuarios.Add(asistente.idEmpleado);
            }

            try
            {
                var Respuesta = await App.ManejadorDatos.RegistrarEventoAsync(peticion);
                await Navigation.PopAllPopupAsync();
                if(Respuesta)
                {
                    MessagingCenter.Send<EventoPage>(this, "Aceptar");
                    await Navigation.PopAsync();
                    return;
                }
                else
                {
                    ShowToast(ToastNotificationType.Error, "¡Lo lamentamos!", "Ha ocurrido algo inesperado grabando el evento, intentalo de nuevo.", 7);
                    return;
                }
            }
            catch
            {
                await Navigation.PopAllPopupAsync();
                ShowToast(ToastNotificationType.Error,"¡Ha ocurrido algo inesperado!", "Intentalo de nuevo",7);
            }        
        }

        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }

        protected async override void OnAppearing()
        {
            if(!isAppearing)
            {
                NavigationPage.SetBackButtonTitle(this, String.Empty);
                NavigationPage.SetHasBackButton(this, false);
                CarouselContenido.Position = 1;
                CarouselContenido.Position = 0;
                isAppearing = true;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Constants.PantallaAbierta = false;
        }
    }
}