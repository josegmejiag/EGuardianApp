using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CarouselView.FormsPlugin.Abstractions;
using EGuardian.Common;
using EGuardian.Common.Resources;
using EGuardian.Data;
using EGuardian.Helpers;
using EGuardian.Models.Eventos;
using EGuardian.ViewModels.Eventos;
using EGuardian.Views.Eventos.Evento;
using EGuardian.Views.Menu;
using Plugin.Toasts;
using Refractored.XamForms.PullToRefresh;
using Rg.Plugins.Popup.Extensions;
using SuaveControls.Views;
using Xamarin.Forms;

namespace EGuardian.Views.Eventos
{
	public class EventosPage : ContentPage
	{
        Boolean AgendaVistaPresentado, MenuPresentado, CalendarioPresentado, isContextual;
        private EventosViewModel VistaModelo { get { return BindingContext as EventosViewModel; } }
        StackLayout Calendario, MenuContextual, horario, headerPadding, headerContenido;
        Grid Modal, semana, headerBotones;
        FloatingActionButton menuFAB;
        RelativeLayout Contenido, header, headerCarousel;
        System.Globalization.CultureInfo globalizacion;
        CarouselViewControl headerCarouselContenido;
        Button anterior, siguiente, refrescar, ocultar, btdia, btsemana, btmes, bthoy;
        PullToRefreshLayout refreshView;
        ToolbarItem opcionesToolbar;
        DateTime fromDate, toDate;
        List<Semana> semanaHeader;
        ScrollView listaCitas;

        public EventosPage()
        {
            this.BindingContext = new EventosViewModel();
            Title = "Eventos";
            isContextual = false;
            fromDate = toDate = DateTime.Now;
            opcionesToolbar = new ToolbarItem
            {
                Icon = "mas.png",
                Text = "Opciones"
            };
            opcionesToolbar.Clicked += OpcionesToolBar_Clicked;
            this.ToolbarItems.Add(opcionesToolbar);
            NavigationPage.SetBackButtonTitle(this, "");
            MessagingCenter.Subscribe<MainPage>(this, "Unsubscribe", (sender) =>
            {
                MessagingCenter.Unsubscribe<DiaView>(this, "selecciono");
                MessagingCenter.Unsubscribe<EventoPage>(this, "Aceptar");
                //MessagingCenter.Unsubscribe<ServicioWeb>(this, "500");
                MessagingCenter.Unsubscribe<DiaHeader>(this, "selecciono");
                /*MessagingCenter.Unsubscribe<ServicioWeb>(this, "145");
                MessagingCenter.Unsubscribe<ServicioWeb>(this, "747");
                MessagingCenter.Unsubscribe<ServicioWeb>(this, "131");
                MessagingCenter.Unsubscribe<ServicioWeb>(this, "Logout");
                MessagingCenter.Unsubscribe<ServicioWeb>(this, "Red");
                MessagingCenter.Unsubscribe<ServicioWeb>(this, "RespuestaFallida");
                MessagingCenter.Unsubscribe<ServicioWeb>(this, "Desconocido");*/
                MessagingCenter.Unsubscribe<EventoPage>(this, "Eliminar");
                /*MessagingCenter.Unsubscribe<PacienteNuevo_EdicionVista>(this, "Guardar");
                MessagingCenter.Unsubscribe<PacienteActionDTModeloVista>(this, "Accion");*/
            });
            /*
            MessagingCenter.Subscribe<PacienteActionDTModeloVista>(this, "Accion", async (sender) =>
                {
                    if (sender.Menu != MenuTipo.Agenda)
                        return;
                    var stack = Navigation.NavigationStack;
                    if ((stack[stack.Count - 1].GetType() != typeof(Indicador)) && (stack[stack.Count - 1].GetType() != typeof(PacienteExpedienteView)) && (stack[stack.Count - 1].GetType() != typeof(PacienteNuevo_EdicionVista)))
                    {
                        await Navigation.PopModalAsync();
                        await Navigation.PushPopupAsync(new Indicador("Obteniendo datos de paciente.", Color.White));
                        var paciente = App.Database.GetPaciente(Convert.ToInt16(sender.paciente.id));
                        Modelos.Pacientes.SelectByID peticionByID = new Modelos.Pacientes.SelectByID
                        {
                            PatientID = paciente.Patient_ID.ToString()
                        };
                        await App.ManejadorDatos.SelectByIDAsync(peticionByID);
                        paciente = App.Database.GetPaciente(Convert.ToInt16(sender.paciente.id));

                        History peticionHistory = new History
                        {
                            patientID = paciente.Patient_ID
                        };
                        Images peticionImages = new Images
                        {
                            patientID = paciente.Patient_ID
                        };

                        MedHist peticionMedHist = new MedHist
                        {
                            patientID = paciente.Patient_ID
                        };
                        Referrals peticionReferrals = new Referrals
                        {
                            patientID = paciente.Patient_ID
                        };

                        await App.ManejadorDatos.HistoryAsync(peticionHistory);
                        await App.ManejadorDatos.ImagesAsync(peticionImages);
                        await App.ManejadorDatos.MedHistAsync(peticionMedHist);
                        await App.ManejadorDatos.ReferralsAsync(peticionReferrals);
                        await Navigation.PushAsync(new PacienteExpedienteView(paciente));
                    }
                    else
                        System.Diagnostics.Debug.WriteLine("Actualizando o pantalla abierta");
                });*/


            MessagingCenter.Subscribe<EventoPage>(this, "Aceptar", async (sender) =>
            {
                ShowToast(ToastNotificationType.Success,"¡Evento registrado con éxito!", string.Empty, 5);
                    /*if (sender.evento.calendarID == 0)*/
                        
                    /*else
                        ShowToast(ToastNotificationType.Success,
                                  "¡Cita actualizada con éxito!", "Evento actalizado para " + sender.evento.asunto,
                                  10);*/

               await ActualizarCitas();

            });

            /*MessagingCenter.Subscribe<ServicioWeb>(this, "500", (sender) =>
            {
                ShowToast(ToastNotificationType.Warning,
                          "¡Lo lamentamos!", "Presentamos inconvenientes para obtener tus datos, inténtalo luego.",
                          10);
            });*/
            MessagingCenter.Subscribe<DiaView>(this, "selecciono", async (sender) =>
            {
                if (sender.fechaSeleccionada.Date == fromDate.Date && sender.fechaSeleccionada.Date == toDate.Date)
                    OcultarModal();
                else
                {
                    fromDate = toDate = sender.fechaSeleccionada;
                    RecargarCalendario(false);
                    await ActualizarCitas();
                    AddMenuFAB();
                    headerCarousel.IsVisible = true;
                    semana.IsVisible = false;
                    bthoy.IsVisible = true;
                    btdia.BackgroundColor = Color.White;
                    btdia.TextColor = Color.FromHex("432161");
                    btsemana.BackgroundColor = Color.Transparent;
                    btsemana.TextColor = Color.White;
                    btmes.BackgroundColor = Color.Transparent;
                    btmes.TextColor = Color.White;
                }
            });
            MessagingCenter.Subscribe<DiaHeader>(this, "selecciono", async (sender) =>
            {
                if (sender.fechaSeleccionada.Date == fromDate.Date && sender.fechaSeleccionada.Date == toDate.Date)
                    OcultarModal();
                else
                {
                    fromDate = toDate = sender.fechaSeleccionada;
                    await ActualizarCitas();
                }
            });
            /*MessagingCenter.Subscribe<ServicioWeb>(this, "145", (sender) =>
            {
                ShowToast(ToastNotificationType.Warning,
                          "¡Eliminación de paciente!", sender.Result_Msg,
                          10);
            });
            MessagingCenter.Subscribe<ServicioWeb>(this, "747", (sender) =>
            {
                ShowToast(ToastNotificationType.Error,
                          "¡Inconvenientes al grabar cita!", "Ha ocurrido algo inoportuno grabando la cita, inténtalo de nuevo.",
                          10);
            });
            MessagingCenter.Subscribe<ServicioWeb>(this, "131", (sender) =>
            {
                System.Diagnostics.Debug.WriteLine("entro aqui");
                ShowToast(ToastNotificationType.Error,
                          "¡Inconvenientes al obtener Pacientes!", "Ingrese o refine la búsqueda.  Demasiados pacientes",
                          10);
            });
            MessagingCenter.Subscribe<ServicioWeb>(this, "Logout", async (sender) =>
            {
                await DisplayAlert("Sesión", sender.Result_Msg.Substring(0, sender.Result_Msg.ToLower().IndexOf("t=")), "Aceptar");
                Medicloud.Helpers.Settings.session_Session_Token = null;
                Medicloud.Helpers.Settings.session_User_Nm = null;
                Medicloud.Helpers.Settings.session_Account_Nm = null;
                Medicloud.Helpers.Settings.session_Ctry_Cd = null;
                if (string.IsNullOrEmpty((Medicloud.Helpers.Settings.session_Session_Token)))
                {
                    if ((Device.OS == TargetPlatform.iOS) || (Device.OS == TargetPlatform.Android))
                    {
                        MessagingCenter.Send<AgendaVista>(this, "noAutenticado");
                    }
                }
            });
            MessagingCenter.Subscribe<ServicioWeb>(this, "Red", (sender) =>
            {
                ShowToast(ToastNotificationType.Error,
                          "Inconvenientes de conexión", "Por favor, revisa tu conexión de internet o inténtalo luego.",
                          10);
            });
            MessagingCenter.Subscribe<ServicioWeb>(this, "RespuestaFallida", (sender) =>
            {
                ShowToast(ToastNotificationType.Warning,
                          "¡Lo lamentamos!", "Ha ocurrido algo inoportuno en la trasferencia de datos, inténtalo de nuevo.",
                          10);
            });
            MessagingCenter.Subscribe<ServicioWeb>(this, "Desconocido", (sender) =>
            {
                ShowToast(ToastNotificationType.Warning,
                          "¡Algo salió mal!", "Lamentamos los inconvenientes, inténtalo luego.",
                          10);
            });*/




            MessagingCenter.Subscribe<EventoPage>(this, "Eliminar", async (sender) =>
            {
                await Navigation.PopAsync();
                ShowToast(ToastNotificationType.Success,
                          "Citas", "La cita para " + sender.evento.asunto + " se ha eliminado correctamente.",
                          5);
                await Navigation.PopAllPopupAsync();
                await ActualizarCitas();
            });
            /*MessagingCenter.Subscribe<PacienteNuevo_EdicionVista>(this, "Guardar", async (sender) =>
            {
                if (sender.menu == MenuTipo.Agenda)
                {
                    foreach (var paciente in sender.NuevoPaciente)
                    {
                        Modelos.Pacientes.SelectByID peticion = new Modelos.Pacientes.SelectByID
                        {
                            PatientID = paciente.Patient_ID
                        };
                        await App.ManejadorDatos.SelectByIDAsync(peticion);
                        if (sender.paciente.Patient_ID == 0)
                            ShowToast(ToastNotificationType.Success, "¡Paciente registrado con éxito!", "Registrado con el identificador " + paciente.Patient_ID, 10);
                        else
                            ShowToast(ToastNotificationType.Success, "¡Paciente actualizado con éxito!", "Paciente " + sender.paciente.nombrePila + " actualizado.", 10);
                    }
                }
            });*/

            Calendario = new StackLayout
            {
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.Fill,
                IsVisible = false,
                Children =
                    {
                        new DiaView(),
                    }
            };
            globalizacion = new System.Globalization.CultureInfo("es-GT");

            refrescar = new Button
            {
                Text = "Refrescar",
                BackgroundColor = Color.Transparent,
                WidthRequest = 130,
                HeightRequest = 40,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };
            refrescar.Clicked += Refrescar_Clicked;
            ocultar = new Button
            {
                Text = "Ocultar",
                BackgroundColor = Color.Transparent,
                WidthRequest = 130,
                HeightRequest = 40,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };
            ocultar.Clicked += Ocultar_Clicked; ;

            MenuContextual = new StackLayout
            {
                Padding = new Thickness(0, 0),
                Spacing = 0,
                BackgroundColor = Color.White,
                IsVisible = false,
                Children =
                    {
                        refrescar,
                        ocultar
                    }
            };

            Modal = new Grid
            {
                BackgroundColor = Color.FromHex("242a34"),
                Opacity = 0.5,
                Padding = new Thickness(0, 0, 0, 0),
                WidthRequest = 200,
                HeightRequest = 200,
            };

            TapGestureRecognizer GestoModal = new TapGestureRecognizer();
            GestoModal.Tapped += (s, e) => { OcultarModal(); };
            Modal.GestureRecognizers.Add(GestoModal);

            Contenido = new RelativeLayout();
            Contenido.Children.Add(MenuContextual,
                                        xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - MenuContextual.Width); }),
                                        yConstraint: Constraint.Constant(0));

            headerBotones = new Grid
            {
                Padding = new Thickness(25, 20, 25, 0),
                ColumnSpacing = 10,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                        new RowDefinition { Height = new GridLength (25, GridUnitType.Absolute) }},
                ColumnDefinitions = {
                        new ColumnDefinition { Width = new GridLength (30, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength (30, GridUnitType.Auto) },
                        new ColumnDefinition { Width = new GridLength (30, GridUnitType.Star) }}
            };
            btdia = new Button
            {
                Text = "Día",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.FromHex("F7B819"),
                WidthRequest = 80,
                BorderColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                BorderWidth = 2,
                BorderRadius = 8,
                FontSize = 12,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)

            };
            btdia.Clicked += Btdia_Clicked;

            btsemana = new Button
            {
                Text = "Semana",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Transparent,
                WidthRequest = 80,
                BorderColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                BorderWidth = 2,
                BorderRadius = 8,
                FontSize = 12,

            };
            btsemana.Clicked += Btsemana_Clicked;
            btmes = new Button
            {
                Text = "Mes",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Transparent,
                WidthRequest = 80,
                BorderColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                BorderWidth = 2,
                BorderRadius = 8,
                FontSize = 12,
            };
            btmes.Clicked += Btmes_Clicked;

            headerBotones.Children.Add(btdia, 0, 0);
            headerBotones.Children.Add(btsemana, 1, 0);
            headerBotones.Children.Add(btmes, 2, 0);

            headerCarouselContenido = new CarouselViewControl
            {
                ItemsSource = semanaHeader,
                ItemTemplate = new DataTemplate(typeof(HeaderDTViewModel)),
                InterPageSpacing = 10,
                HeightRequest = 70,
                Orientation = CarouselViewOrientation.Horizontal,
            };
            headerCarouselContenido.PositionSelected += HeaderCarouselContenido_PositionSelected;

            anterior = new Button
            {
                Image = Images.AnteriorC,
                HorizontalOptions = LayoutOptions.End,
                WidthRequest = 15,
                BackgroundColor = Color.Transparent
            };
            anterior.Clicked += Anterior_Clicked;

            siguiente = new Button
            {
                Image = Images.SiguienteC,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 15,
                BackgroundColor = Color.Transparent
            };
            siguiente.Clicked += Siguiente_Clicked;

            headerCarousel = new RelativeLayout();
            headerCarousel.Children.Add(headerCarouselContenido, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return parent.Width; }));

            headerCarousel.Children.Add(
                anterior,
                Constraint.RelativeToView(headerCarouselContenido, (parent, view) => { return (view.Width / 5); }),
                Constraint.RelativeToView(headerCarouselContenido, (parent, view) => { return (view.Height / 5); })
                );
            headerCarousel.Children.Add(
                siguiente,
                Constraint.RelativeToView(headerCarouselContenido, (parent, view) => { return ((view.Width / 5) * 4); }),
                Constraint.RelativeToView(headerCarouselContenido, (parent, view) => { return (view.Height / 5); })
                );
            bthoy = new Button
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 80,
                BorderColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                BorderWidth = 2,
                BorderRadius = 8,
                FontSize = 12,
                HeightRequest = 25
            };
            bthoy.Clicked += Bthoy_Clicked; ;
            if (fromDate.Date == DateTime.Now.Date)
            {
                bthoy.Text = "HOY";
                bthoy.TextColor = Color.White;
                bthoy.BackgroundColor = Color.Transparent;
            }
            else
            {
                bthoy.Text = "IR A HOY";
                bthoy.TextColor = Color.White;
                bthoy.BackgroundColor = Color.FromHex("F7B819"); 
            }

            headerPadding = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 15 };

            semana = new Grid
            {
                IsVisible = false,
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = new Thickness(15, 0),
                BackgroundColor = Color.FromHex("8D72B3"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                            new RowDefinition { Height = new GridLength (100, GridUnitType.Auto) }
                        },
                ColumnDefinitions = {
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                        }
            };

            Image headerBackground = new Image()
            {
                Source = "headerEventos",
                Aspect = Aspect.AspectFill
            };

            headerContenido = new StackLayout
            {
                Padding = 0,
                BackgroundColor = Color.Transparent,
                Children =
                    {
                        headerBotones,
                        headerCarousel,
                        bthoy,
                        headerPadding,
                        semana
                    }
            };

            header = new RelativeLayout();
            header.Children.Add(headerBackground, Constraint.Constant(0), Constraint.Constant(0),
                                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                heightConstraint: Constraint.RelativeToView(headerContenido, (parent, view) => { return parent.Height - (parent.Height - view.Height); }));
            header.Children.Add(headerContenido, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return parent.Width; }));

            Contenido.Children.Add(header,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width)
            );

            horario = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            listaCitas = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = new Thickness(0, 5),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        horario
                    }
                }
            };
            refreshView = new PullToRefreshLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RefreshColor = Color.Accent,
                Content = listaCitas
            };
            refreshView.RefreshCommand = RefreshCommand; ;
            refreshView.SetBinding<EventosViewModel>(PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);

            Content = Contenido;
            AgendaVistaPresentado = true;
            actualizarsemana();
            try
            {
                menuFAB = new FloatingActionButton
                {
                    Image = "iFABPa",
                    ButtonColor = Color.FromHex("F7B819"),
                    BorderColor = Color.FromHex("F7B819"),
                    TextColor = Color.FromHex("F7B819"),
                    //BackgroundColor = Color.FromHex("F7B819")
                };
                menuFAB.Clicked += MenuFAB_Clicked;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        ICommand refreshCommand;

        public ICommand RefreshCommand
        {
            get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
        }

        async Task ExecuteRefreshCommand()
        {
            if (VistaModelo.IsBusy)
                return;
            VistaModelo.IsBusy = true;
            var stack = Navigation.NavigationStack;
            if (refrescar.IsEnabled && !Constants.PantallaAbierta && refreshView.IsEnabled /*&& (stack[stack.Count - 1].GetType() != typeof(Indicador)) && (stack[stack.Count - 1].GetType() != typeof(PacienteNuevo_EdicionVista))*/ && (stack[stack.Count - 1].GetType() != typeof(EventoPage)))
            {
                refreshView.IsEnabled = false;
                await ActualizarCitas();
                refreshView.IsEnabled = true;
            }
            else
                System.Diagnostics.Debug.WriteLine("Actualizando o pantalla abierta actualmente");
            VistaModelo.IsBusy = false;
        }

        private void actualizarsemana()
        {
            semanaHeader = new List<Semana>();
            semana.Children.Clear();
            for (int i = 0; i <= 6; i++)
            {
                var fechaSemana = fromDate.AddDays(-(Convert.ToInt32(fromDate.DayOfWeek) - i));
                bool seleccionado = false;
                if (fechaSemana.Day == fromDate.Day)
                    seleccionado = true;
                semanaHeader.Add(new Semana { index = i, Date = fechaSemana.Date });
                semana.Children.Add(
                    new DiaHeader(fechaSemana.Date, seleccionado), i, 0);
            }
            headerCarouselContenido.Position = Convert.ToInt32(fromDate.DayOfWeek);
            headerCarouselContenido.ItemsSource = semanaHeader;
        }

        private void mostrar_MenuContextual(bool Menu)
        {
            Contenido.Children.Remove(menuFAB);
            Contenido.Children.Remove(Modal);
            if (Menu)
            {
                Contenido.Children.Remove(MenuContextual);
                MenuContextual.IsVisible = true;

                if (!AgendaVistaPresentado)
                {
                    Modal.Opacity = 0.3;
                    Contenido.Children.Add(Modal,
                        xConstraint: Constraint.Constant(0),
                        yConstraint: Constraint.Constant(0),
                        widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                        heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; })
                    );
                }
                Contenido.Children.Add(MenuContextual,
                    xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - MenuContextual.Width); }),
                    yConstraint: Constraint.Constant(0));
            }
            else
            {
                if (MenuPresentado)
                {
                    Contenido.Children.Add(Modal,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(0),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                       heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; })
                   );
                }
                Modal.Opacity = 0.5;
            }
        }

        void RecargarMenuContextual()
        {
            if (!isContextual)
            {
                menuFAB.IsEnabled = false;
                mostrar_MenuContextual(true);
                MenuContextual.IsVisible = true;
                isContextual = true;
            }
            else
            {
                menuFAB.IsEnabled = true;
                mostrar_MenuContextual(false);
                MenuContextual.IsVisible = false;
                isContextual = false;
            }

        }

        void Ocultar_Clicked(object sender, EventArgs e)
        {
            MenuContextual.IsVisible = false;
            menuFAB.IsEnabled = true;
            isContextual = false;
            headerPadding.IsVisible = true;
            mostrar_MenuContextual(false);
            try
            {
                if (ocultar.Text.Equals("Ocultar"))
                {
                    ocultar.Text = "Mostrar";
                    headerCarousel.IsVisible = false;
                    semana.IsVisible = false;
                    bthoy.IsVisible = false;
                    if (Calendario.IsVisible)
                        RecargarCalendario(false);
                }
                else
                {
                    ocultar.Text = "Ocultar";
                    if (btmes.BackgroundColor == Color.FromHex("F7B819"))
                        RecargarCalendario(true);
                    else
                    {
                        headerCarousel.IsVisible = true;
                        if (btsemana.BackgroundColor == Color.White)
                        {
                            semana.IsVisible = true;
                            headerPadding.IsVisible = false;
                        }
                        else
                            bthoy.IsVisible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " " + ex.StackTrace);
            }
            AddMenuFAB();
            if (!Calendario.IsVisible)
                OcultarModal();
        }

        async void Refrescar_Clicked(object sender, EventArgs e)
        {
            var stack = Navigation.NavigationStack;
            if (refrescar.IsEnabled && !VistaModelo.IsBusy && !Constants.PantallaAbierta && refreshView.IsEnabled && (stack[stack.Count - 1].GetType() != typeof(Indicador)) /*&& (stack[stack.Count - 1].GetType() != typeof(PacienteNuevo_EdicionVista))*/ && (stack[stack.Count - 1].GetType() != typeof(EventoPage)))
            {
                VistaModelo.IsBusy = true;
                refreshView.IsEnabled = false;
                refrescar.IsEnabled = false;
                MenuContextual.IsVisible = false;
                menuFAB.IsEnabled = true;
                isContextual = false;
                mostrar_MenuContextual(false);
                AddMenuFAB();
                await ActualizarCitas();
                OcultarModal();
                refrescar.IsEnabled = true;
                VistaModelo.IsBusy = false;
                refreshView.IsEnabled = true;
            }
            else
                System.Diagnostics.Debug.WriteLine("Actualizando o pantalla abierta actualmente");
        }

        void OpcionesToolBar_Clicked(object sender, EventArgs e)
        {
            if (CalendarioPresentado)
            {
                RecargarCalendario(false);
            }
            RecargarMenuContextual();
            AddMenuFAB();
        }

        void Anterior_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (headerCarouselContenido.Position > 0)
                    headerCarouselContenido.Position = headerCarouselContenido.Position - 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        void Siguiente_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (headerCarouselContenido.Position < (int)headerCarouselContenido.ItemsSource.GetCount() - 1)
                    headerCarouselContenido.Position = headerCarouselContenido.Position + 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        void Btdia_Clicked(object sender, EventArgs e)
        {
            try
            {
                ocultar.Text = "Ocultar";
                semana.IsVisible = false;
                actualizarsemana();
                bthoy.IsVisible = true;
                headerCarousel.IsVisible = true;
                if (Calendario.IsVisible)
                {
                    RecargarCalendario(false);
                    AddMenuFAB();
                }
                btdia.BackgroundColor = Color.FromHex("F7B819");
                btdia.TextColor = Color.White;
                btsemana.BackgroundColor = Color.Transparent;
                btsemana.TextColor = Color.White;
                btmes.BackgroundColor = Color.Transparent;
                btmes.TextColor = Color.White;
                headerPadding.IsVisible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        void Btsemana_Clicked(object sender, EventArgs e)
        {
            try
            {
                ocultar.Text = "Ocultar";
                btdia.BackgroundColor = Color.Transparent;
                Task.Delay(500);
                btdia.TextColor = Color.White;
                btsemana.BackgroundColor = Color.FromHex("F7B819");
                btsemana.TextColor = Color.White;
                btmes.BackgroundColor = Color.Transparent;
                btmes.TextColor = Color.White;
                semana.IsVisible = true;
                actualizarsemana();
                bthoy.IsVisible = false;
                headerCarousel.IsVisible = true;
                if (Calendario.IsVisible)
                {
                    RecargarCalendario(false);
                    AddMenuFAB();
                }
                headerPadding.IsVisible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        async void Btmes_Clicked(object sender, EventArgs e)
        {
            try
            {
                ocultar.Text = "Ocultar";
                if (Calendario.IsVisible)
                {
                    btdia.BackgroundColor = Color.Transparent;
                    btdia.TextColor = Color.White;
                    btsemana.BackgroundColor = Color.Transparent;
                    btsemana.TextColor = Color.White;
                    btmes.BackgroundColor = Color.FromHex("F7B819");
                    btmes.TextColor = Color.White;
                    RecargarCalendario(false);
                    await ActualizarCitas();
                    semana.IsVisible = false;
                    bthoy.IsVisible = true;
                    headerCarousel.IsVisible = true;
                    headerPadding.IsVisible = true;
                }
                else
                {
                    btdia.BackgroundColor = Color.Transparent;
                    btdia.TextColor = Color.White;
                    btsemana.BackgroundColor = Color.Transparent;
                    btsemana.TextColor = Color.White;
                    btmes.BackgroundColor = Color.FromHex("F7B819");
                    btmes.TextColor = Color.White;
                    semana.IsVisible = false;
                    headerCarousel.IsVisible = false;
                    bthoy.IsVisible = false;
                    headerPadding.IsVisible = true;
                    RecargarCalendario(true);
                }
                AddMenuFAB();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        void Bthoy_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (fromDate.Date != DateTime.Now.Date)
                {
                    fromDate = toDate = DateTime.Now;
                    ActualizarCitas();
                    semana.IsVisible = false;
                    bthoy.Text = "HOY";
                    bthoy.TextColor = Color.White;
                    bthoy.BackgroundColor = Color.FromHex("F7B819");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        async void HeaderCarouselContenido_PositionSelected(object sender, EventArgs e)
        {
            try
            {
                if (fromDate.Date != semanaHeader[semanaHeader.FindIndex(p => p.index.Equals(((CarouselViewControl)sender).Position))].Date.Date)
                {
                    System.Diagnostics.Debug.WriteLine("CAMBIO POSICION");
                    fromDate = toDate = semanaHeader[semanaHeader.FindIndex(p => p.index.Equals(((CarouselViewControl)sender).Position))].Date.Date;
                    await ActualizarCitas();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        async void MenuFAB_Clicked(object sender, EventArgs e)
        {
            Constants.AccionesEvento.Clear();
            Constants.AplicacionesEvento.Clear();
            Constants.AsistentesEvento.Clear();
            Constants.DatosEvento = new RegistrarEvento();
            await Navigation.PushAsync(new EventoPage(DateTime.Now,new eventos()));
        }

        /*void Calendar_DateClicked(object sender, DateTimeEventArgs e)
        {

        }*/



        void Citas_Refreshing(object sender, EventArgs e)
        {
            //var lista = (ListView)sender;
            //ActualizarCitas();
            //lista.IsRefreshing = false;
        }              

        private async void OcultarMenus()
        {
            MenuPresentado = false;
            mostrar_Menu(false);
        }

        private async Task ActualizarCitas()
        {
            await Navigation.PushPopupAsync(new Indicador("Actualizando eventos", Color.White));
            GetEventos peticion = new GetEventos
            {
                idEmpresa = Convert.ToInt32(Settings.session_idEmpresa)
            };
            await App.ManejadorDatos.GetEventosAsync(peticion);
            AgendaVacia();
            Constants.PantallaAbierta = false;
        }


        private void AgendaVacia()
        {
            actualizarsemana();
            horario.Children.Clear();
            List<eventos> eventos = App.Database.GetEventos(Convert.ToInt32(Settings.session_idUsuario)).ToList();
            var ConteoEventos = eventos.Where(evento => evento.Fecha.Date.Equals(fromDate.Date)).Count();
            if (ConteoEventos == 0)
            {
                if (fromDate.Date == DateTime.Now.Date)
                {
                    ShowToast(ToastNotificationType.Info, "Eventos", "No tienes eventos para hoy", 3);
                    bthoy.Text = "HOY";
                    bthoy.TextColor = Color.FromHex("432161");
                    bthoy.BackgroundColor = Color.White;
                }
                else
                {
                    ShowToast(ToastNotificationType.Info, "Eventos", "No tienes eventos para el " + fromDate.Day + " de " + globalizacion.DateTimeFormat.GetMonthName(fromDate.Month).ToLower(), 3);
                    bthoy.Text = "IR A HOY";
                    bthoy.TextColor = Color.White;
                    bthoy.BackgroundColor = Color.Transparent;
                }
            }
            else
            {
                if (fromDate.Date == DateTime.Now.Date)
                {
                    ShowToast(ToastNotificationType.Info, "Eventos", "Tienes " + ConteoEventos + " eventos para hoy", 3);
                    bthoy.Text = "HOY";
                    bthoy.TextColor = Color.FromHex("432161");
                    bthoy.BackgroundColor = Color.White;
                }
                else
                {
                    ShowToast(ToastNotificationType.Info, "Eventos", "Tienes " + ConteoEventos + " eventos para el " + fromDate.Day + " de " + globalizacion.DateTimeFormat.GetMonthName(fromDate.Month).ToLower(), 3);
                    bthoy.Text = "IR A HOY";
                    bthoy.TextColor = Color.White;
                    bthoy.BackgroundColor = Color.Transparent;
                }
            }

            try
            {
                foreach (var HORA in Constants.horas)
                {
                    //System.Diagnostics.Debug.WriteLine("POSICION ALTO " + (horario.Height));
                    if (HORA == DateTime.Now.Hour)
                    {
                        //System.Diagnostics.Debug.WriteLine("POSICION ALTO " + (horario.Height));
                        //listaCitas.ScrollToAsync(x, (horario.Height), true);
                    }
                    horario.Children.Add(new HoraView(new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, HORA, 0, 0, DateTimeKind.Local), eventos));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                VistaModelo.IsBusy = false;
            }
            horario.Children.Add(new StackLayout { HeightRequest = 60 });
            Navigation.PopAllPopupAsync();
        }

        private async void OcultarModal()
        {
            if (isContextual)
            {
                MenuContextual.IsVisible = false;
                menuFAB.IsEnabled = true;
                isContextual = false;
                mostrar_MenuContextual(false);
                AddMenuFAB();
                return;
            }
            else if (CalendarioPresentado)
            {
                menuFAB.IsEnabled = true;
                mostrar_Calendario(false);
                AddMenuFAB();
                Calendario.IsVisible = false;
                CalendarioPresentado = false;
                AgendaVistaPresentado = false;
                semana.IsVisible = false;
                bthoy.IsVisible = true;
                headerCarousel.IsVisible = true;
                btdia.BackgroundColor = Color.White;
                btdia.TextColor = Color.FromHex("432161");
                btsemana.BackgroundColor = Color.Transparent;
                btsemana.TextColor = Color.White;
                btmes.BackgroundColor = Color.Transparent;
                btmes.TextColor = Color.White;
                return;
            }
            else if (MenuPresentado)
            {
                MenuPresentado = false;
                mostrar_Menu(false);
                return;
            }
        }



        private void mostrar_Calendario(bool calendario)
        {
            Contenido.Children.Remove(menuFAB);
            Contenido.Children.Remove(Modal);
            if (calendario)
            {
                //Contenido.Children.Remove(refreshView);
                Contenido.Children.Add(Calendario,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.RelativeToView(header, (parent, view) => { return view.Height; }),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width));
                //heightConstraint: Constraint.RelativeToView(header, (parent, view) => { return (parent.Height - view.Height); }));
                Contenido.Children.Add(refreshView,
                    xConstraint: Constraint.Constant(0),
                                           yConstraint: Constraint.RelativeToView(Calendario, (parent, view) => { return (view.Y + view.Height); }),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                    heightConstraint: Constraint.RelativeToView(Calendario, (parent, view) => { return (parent.Height - view.Height); })
                );
                Contenido.Children.Add(Modal,
                    xConstraint: Constraint.Constant(0),
                                       yConstraint: Constraint.RelativeToView(Calendario, (parent, view) => { return (view.Y + view.Height); }),
                    widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToView(Calendario, (parent, view) => { return (parent.Height - view.Height); })
                );
            }
            else
            {
                if (MenuPresentado)
                {
                    Contenido.Children.Add(Modal,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(0),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                       heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; })
                   );
                }
                Contenido.Children.Add(refreshView,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.RelativeToView(header, (parent, view) => { return view.Height; }),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                    heightConstraint: Constraint.RelativeToView(header, (parent, view) => { return (parent.Height - view.Height); })
                );
            }
        }

        private void mostrar_Menu(bool menu)
        {
            Contenido.Children.Remove(menuFAB);
            Contenido.Children.Remove(Modal);
            if (menu)
            {
                Contenido.Children.Add(Modal,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; })
                );
            }
            Contenido.Children.Add(menuFAB,
                xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - menuFAB.Width) - 16; }),
                yConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - menuFAB.Height) - 16; })
            );
        }

        private void AddMenuFAB()
        {
            Contenido.Children.Add(menuFAB,
                xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - menuFAB.Width) - 16; }),
                yConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - menuFAB.Height) - 16; })
            );
        }


        private void RecargarCalendario(bool visible)
        {
            if (visible)
            {
                menuFAB.IsEnabled = false;
                mostrar_Calendario(true);
                Calendario.IsVisible = true;
                CalendarioPresentado = true;
            }
            else
            {
                menuFAB.IsEnabled = true;
                mostrar_Calendario(false);
                Calendario.IsVisible = false;
                CalendarioPresentado = false;
            }
        }

        private void RecargarCarousel()
        {
            //mostrar_Calendario(false);
            /*
            refreshView = new PullToRefreshLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RefreshColor = Color.Accent,
                Content = listaCitas
            };
            refreshView.RefreshCommand = RefreshCommand; ;
            refreshView.SetBinding<AgendaModeloVista>(PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);


            Contenido.Children.Add(refreshView,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.RelativeToView(header, (parent, view) => { return view.Height; }),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                    heightConstraint: Constraint.RelativeToView(header, (parent, view) => { return (parent.Height - view.Height); })
                );
            
*/
            OcultarModal();
            headerCarousel.Children.Remove(anterior);
            headerCarousel.Children.Remove(siguiente);
            headerCarousel.Children.Remove(headerCarouselContenido);
            headerCarouselContenido = new CarouselViewControl
            {
                ItemsSource = semanaHeader,
                ItemTemplate = new DataTemplate(typeof(HeaderDTViewModel)),
                InterPageSpacing = 10,
                HeightRequest = 70,
                Orientation = CarouselViewOrientation.Horizontal,
            };
            headerCarouselContenido.PositionSelected += HeaderCarouselContenido_PositionSelected;
            headerCarousel.Children.Add(headerCarouselContenido, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return parent.Width; }));

            headerCarousel.Children.Add(
                anterior,
                Constraint.RelativeToView(headerCarouselContenido, (parent, view) => { return (view.Width / 5); }),
                Constraint.RelativeToView(headerCarouselContenido, (parent, view) => { return (view.Height / 5); })
                );
            headerCarousel.Children.Add(
                siguiente,
                Constraint.RelativeToView(headerCarouselContenido, (parent, view) => { return ((view.Width / 5) * 4); }),
                Constraint.RelativeToView(headerCarouselContenido, (parent, view) => { return (view.Height / 5); })
                );
            actualizarsemana();
        }
        /*
        private void Cal_DateClicked(object sender, DateTimeEventArgs e)
        {
            normalFab.IsEnabled = true;
            mostrar_Calendario(false);
            stack2.IsVisible = false;
            //sCal.Icon = "iCalendars";
            Visible = false;
            //DisplayAlert("Fecha seleccionada", e.DateTime.ToString(), "OK");        

            Cita separador = new Cita()
            {
                citaID = 1,
                estado = CitaStatus.SEPERADOR,

            };
            DateTime actual = new DateTime();
            cardstack.Children.Clear();
            foreach (var pas in Items2)
            {
                /*if (pas.fecha.Equals(e.DateTime.Day.ToString()))
                {
                    var dia = Convert.ToDateTime(pas.hora).Date;
                    if (!actual.Equals(dia))
                    {
                        separador.mensajeEstado = getSelectedDate(dia.DayOfWeek, dia.Day);
                        cardstack.Children.Add(new DiaAgrupacoinView(separador));
                    }
                string nom = pas.nombre;*//*i
                string es = "";
                CitaStatus ess = CitaStatus.SIN_CONFIRMAR;
                if (pas.estado.Equals("0"))
                {
                    es = "Aun no confirma";
                    ess = CitaStatus.SIN_CONFIRMAR;
                }
                else if (pas.estado.Equals("1"))
                {
                    es = "Confirmado";
                    ess = CitaStatus.CONFIRMADO;
                }
                else if (pas.estado.Equals("2"))
                {
                    es = "En sala de espera";
                    ess = CitaStatus.EN_SALA_ESPERA;
                }

                Cita citaW = new Cita()
                {
                    citaID = 1,
                    //paciente = pas.nombre,
                    estado = ess,
                    mensajeEstado = es,
                    //hora = Convert.ToDateTime(pas.hora),
                    //fecha = pas.fecha,
                    //archivo = pas.archivo,
                    //genero = pas.genero,
                    //fecha_nacimiento = pas.fecha_nacimiento,
                    //edad = pas.edad,
                    //telefono = pas.telefono,
                    //correo = pas.correo,
                    asunto = pas.asunto,
                    diagnostico = pas.diagnostico,
                    tratamiento = pas.tratamiento
                };
                cardstack.Children.Add(new CitaView(citaW));
                //actual = dia;
                //}                
            }
        }
*/
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Constants.PantallaAbierta = false;
            try
            {
                VistaModelo.IsInitialized = true;
                if (AgendaVistaPresentado)
                {
                    RecargarCalendario(false);
                    AddMenuFAB();
                    AgendaVistaPresentado = false;
                    await ActualizarCitas();
                }
                else if (Device.OS != TargetPlatform.Android)
                    RecargarCarousel();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private String obtenerFecha()
        {
            var fecha = char.ToUpper(globalizacion.DateTimeFormat.GetDayName(fromDate.DayOfWeek).ToString()[0]) + globalizacion.DateTimeFormat.GetDayName(fromDate.DayOfWeek).ToString().Substring(1) + " " + fromDate.Day + "/" + fromDate.Month + "/" + fromDate.Year;
            return fecha;
        }
        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }
    }

    class Semana
    {
        System.Globalization.CultureInfo globalizacion = new System.Globalization.CultureInfo("es-GT");
        public int index { get; set; }
        public DateTime Date { get; set; }
        public string diaSemana { get { return dayOfWeek(Date); } }
        public string fecha { get { return getSelectedDate(Date); } }
        private String getSelectedDate(DateTime fecha)
        {
            var date = fecha.Day + " - " + globalizacion.DateTimeFormat.GetMonthName(fecha.Month).ToUpper() + " - " + fecha.Year;
            return date;
        }
        private String dayOfWeek(DateTime fecha)
        {
            var date = globalizacion.DateTimeFormat.GetDayName(fecha.DayOfWeek).ToString().ToUpper();
            return date;
        }
    }

    class DiaHeader : ContentView
    {
        System.Globalization.CultureInfo globalizacion = new System.Globalization.CultureInfo("es-GT");
        Label dia, diaSemana;
        public DateTime fecha;
        public DateTime fechaSeleccionada;
        public DiaHeader(DateTime fecha, bool seleccionado)
        {
            this.fecha = fecha;
            StackLayout diaSL = new StackLayout
            {
                Padding = new Thickness(0, 5),
                Spacing = 0,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            switch (Convert.ToInt32(this.fecha.DayOfWeek))
            {
                case 0:
                    diaSL.BackgroundColor = Color.FromHex("47384C");
                    break;
                case 1:
                    diaSL.BackgroundColor = Color.FromHex("533F62");
                    break;
                case 2:
                    diaSL.BackgroundColor = Color.FromHex("5F4576");
                    break;
                case 3:
                    diaSL.BackgroundColor = Color.FromHex("6B4B8C");
                    break;
                case 4:
                    diaSL.BackgroundColor = Color.FromHex("7752A0");
                    break;
                case 5:
                    diaSL.BackgroundColor = Color.FromHex("7F5AA5");
                    break;
                case 6:
                    diaSL.BackgroundColor = Color.FromHex("8160A9");
                    break;
                default:
                    diaSL.BackgroundColor = Color.Transparent;
                    break;
            }
            diaSemana = new Label
            {
                TextColor = Color.White,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                Text = globalizacion.DateTimeFormat.GetDayName(this.fecha.DayOfWeek).ToString().ToUpper().Substring(0, 3),
                FontSize = 11,
                HorizontalOptions = LayoutOptions.Center,
            };
            dia = new Label
            {
                //FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                TextColor = Color.White,
                Text = this.fecha.ToString(@"dd"),
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center,
            };

            diaSL.Children.Add(diaSemana);
            diaSL.Children.Add(dia);

            if (fecha.Date == DateTime.Now.Date)
            {
                diaSemana.Text = "HOY";
                diaSemana.TextColor = Color.Yellow;
                dia.TextColor = Color.Transparent;
                diaSemana.FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null);
            }
            if (seleccionado)
            {
                if (fecha.Date == DateTime.Now.Date)
                    dia.TextColor = Color.Transparent;
                else
                    dia.TextColor = Color.FromHex("432161");
                diaSemana.TextColor = Color.FromHex("432161");
                diaSL.BackgroundColor = Color.White;
            }
            BackgroundColor = Color.Transparent;
            //dia.Clicked += Dia_Clicked;
            Content = diaSL;
            TapGestureRecognizer diaTAP = new TapGestureRecognizer();
            diaTAP.Tapped += Dia_Clicked;
            diaSL.GestureRecognizers.Add(diaTAP);
        }

        void Dia_Clicked(object sender, EventArgs e)
        {
            fechaSeleccionada = fecha;
            MessagingCenter.Send<DiaHeader>(this, "selecciono");
        }
    }
}