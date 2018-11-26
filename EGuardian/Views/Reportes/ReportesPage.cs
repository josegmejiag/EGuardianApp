using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EGuardian.Controls;
using EGuardian.Helpers;
using EGuardian.ViewModels.Incidencias;
using Plugin.Toasts;
using Refractored.XamForms.PullToRefresh;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.Views.Reportes
{
	public class ReportesPage : ContentPage
    {
        bool CajaVistaPresentado, PacientesVistaVisible, MenuPresentado, ABCPresentado, BusquedaActiva, isContextual, isSelected;
        Label notificacionCajaVacia, notificacionFiltrado;
        RelativeLayout Contenido;
        Grid ModalBackground;
        Button refrescar;
        Entry BusquedaRapida;
        StackLayout MenuContextual;
        StackLayout HeaderPacientes;
        IconView filtrar;
        StackLayout filtrarGesture;
        Grid Modal;
        ScrollView ContenidoCaja;
        ExtendedDatePicker fechaInicial;
        ExtendedDatePicker fechaFinal;
        int CountRegistros = 0;
        System.Globalization.CultureInfo globalizacion;

        PullToRefreshLayout refreshView;
        AccordionView CajaAccordion;
        string filtro = "Patient_ID";
        List<IncidenciasAgrupacionViewModel> Monedas;

        public ReportesPage()
        {
            Title = "Reporte de eventos";
            var opcionesToolBar = new ToolbarItem
            {
                Icon = "mas.png",
                Text = "Más"
            };

            refrescar = new Button
            {
                Text = "Refrescar",
                BackgroundColor = Color.Transparent,
                WidthRequest = 130,
                HeightRequest = 40,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            refrescar.Clicked += async (sender, e) =>
            {
                /*var stack = Navigation.NavigationStack;
                if (!VistaModelo.IsBusy && refrescar.IsEnabled && filtrar.IsEnabled && filtrarGesture.IsEnabled && isContextual && MenuContextual.IsVisible && !Constantes.ModalAbierto && (stack[stack.Count - 1].GetType() != typeof(Indicador)) && (stack[stack.Count - 1].GetType() != typeof(CajaDetalleModeloVista)))
                {
                    refrescar.IsEnabled = false;
                    VistaModelo.IsBusy = true;
                    MenuContextual.IsVisible = false;
                    isContextual = false;
                    BusquedaActiva = false;
                    mostrar_MenuContextual(false);
                    BusquedaActiva = true;
                    await ActualizarCaja();
                    VistaModelo.IsBusy = false;
                    refrescar.IsEnabled = true;
                }
                else
                    System.Diagnostics.Debug.WriteLine("Actualizacion activa.");*/
            };

            MenuContextual = new StackLayout
            {
                Padding = new Thickness(0, 0),
                Spacing = 0,
                BackgroundColor = Color.White,
                Children =
                {
                    refrescar,
                }
            };
            MenuContextual.IsVisible = false;

            opcionesToolBar.Clicked += OpcionesToolBar_Clicked;

            this.ToolbarItems.Add(opcionesToolBar);

            /*MessagingCenter.Subscribe<CajaDetalleModeloVista>(this, "deseleccionado", (sender) =>
            {
                isSelected = false;
                Constantes.ModalAbierto = false;
            });
            MessagingCenter.Subscribe<CajaAgrupacionModeloVista>(this, "seleccionado", async (sender) =>
            {
                var stack = Navigation.NavigationStack;
                if (!VistaModelo.IsBusy && ContenidoCaja.IsEnabled && filtrar.IsEnabled && filtrarGesture.IsEnabled && refrescar.IsEnabled && !isSelected && (stack[stack.Count - 1].GetType() != typeof(Indicador)) && (stack[stack.Count - 1].GetType() != typeof(CajaDetalleModeloVista)))
                {
                    ContenidoCaja.IsEnabled = false;
                    isSelected = true;
                    if (string.IsNullOrEmpty(sender.transaccionSeleccionada.Paciente.expediente))
                    {
                        await Navigation.PushPopupAsync(new Indicador("Obteniendo datos", Color.White));
                        SelectByID peticion = new SelectByID
                        {
                            PatientID = sender.transaccionSeleccionada.Patient_ID.ToString()
                        };
                        await App.ManejadorDatos.SelectByIDAsync(peticion);
                        sender.transaccionSeleccionada.Patient_ID = sender.transaccionSeleccionada.Patient_ID;
                        await Navigation.PopAllPopupAsync();
                    }
                    await Navigation.PushPopupAsync(new CajaDetalleModeloVista(sender.transaccionSeleccionada));
                    ContenidoCaja.IsEnabled = true;
                }
                else
                    System.Diagnostics.Debug.WriteLine("Modal abierto actualmente");
            });*/



            notificacionCajaVacia =
                new Label
                {
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                    FontSize = 11,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex("3F3F3F"),
                    Text = "NO SE HA ENCONTRADO CONTENIDO PARA MOSTRAR",
                    IsVisible = false
                };
            notificacionFiltrado =
                new Label
                {
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                    FontSize = 11,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex("B2B2B2"),
                    Text = "FILTRANDO POR PACIENTE",
                    IsVisible = false
                };
            if (Device.OS == TargetPlatform.Android)
                notificacionFiltrado.IsVisible = true;

            BusquedaRapida =
                new ExtendedEntry
                {
                    Placeholder = "Filtrar por evento",
                    HasBorder = false,
                    BackgroundColor = Color.Transparent,
                    Margin = new Thickness(10, 0),
                    PlaceholderColor = Color.FromHex("808080"),
                    FontSize = 14,
                    TextColor = Color.FromHex("3F3F3F"),
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
                };
            BusquedaRapida.TextChanged += BusquedaRapida_TextChanged;

            Grid PacientesHeader = new Grid
            {
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
            IconView cancelar = new IconView
            {
                Foreground = Color.FromHex("F7B819"),
                Source = "iCancelarA.png",
                HeightRequest = 25,
                WidthRequest = 25,

            };
            TapGestureRecognizer cancelarTAP = new TapGestureRecognizer();
            cancelarTAP.Tapped += async (sender, e) =>
            {
                cancelar.Source = "iCancelarB.png";
                BusquedaRapida.Text = "";
                await Task.Delay(500);
                cancelar.Source = "iCancelarA";
            };
            cancelar.GestureRecognizers.Add(cancelarTAP);
            Image buscar = new Image
            {
                Source = "iBusqueda.png",
                HeightRequest = 25,
                WidthRequest = 25,

            };
            TapGestureRecognizer buscarTAP = new TapGestureRecognizer();

            buscarTAP.Tapped += async (sender, e) =>
            {
                await FiltrarTransacciones();
            };
            buscar.GestureRecognizers.Add(buscarTAP);


            filtrar = new IconView
            {
                Foreground = Color.White,
                Source = "iContinuar.png",
                HeightRequest = 25,
                WidthRequest = 25,

            };
            filtrarGesture = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            TapGestureRecognizer filtrarTAP = new TapGestureRecognizer();
            filtrarTAP.Tapped += async (sender, e) =>
            {
                /*var stack = Navigation.NavigationStack;
                if (!VistaModelo.IsBusy && filtrar.IsEnabled && filtrarGesture.IsEnabled && refrescar.IsEnabled && !Constantes.ModalAbierto && (stack[stack.Count - 1].GetType() != typeof(Indicador)) && (stack[stack.Count - 1].GetType() != typeof(CajaDetalleModeloVista)))
                {
                    filtrar.IsEnabled = false;
                    filtrarGesture.IsEnabled = false;
                    VistaModelo.IsBusy = true;
                    await ActualizarCaja();
                    VistaModelo.IsBusy = false;
                    filtrar.IsEnabled = true;
                    filtrarGesture.IsEnabled = true;
                }
                else
                    System.Diagnostics.Debug.WriteLine("Actualizando o modal abierto actualmente");*/
            };
            filtrar.GestureRecognizers.Add(filtrarTAP);
            filtrarGesture.GestureRecognizers.Add(filtrarTAP);

            PacientesHeader.Children.Add(
                new RoundedBoxView.Forms.Plugin.Abstractions.RoundedBoxView
                {
                    BackgroundColor = Color.FromHex("E5E5E5"),
                    CornerRadius = 6,
                    HeightRequest = 20,
                    //WidthRequest = 128,
                }, 0, 0);
            PacientesHeader.Children.Add(BusquedaRapida, 0, 0);
            PacientesHeader.Children.Add(cancelar, 1, 0);
            PacientesHeader.Children.Add(buscar, 2, 0);



            Grid Header = new Grid
            {
                Padding = 0,
                HeightRequest = 120,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
                }
            };

            Header.Children.Add(
                new Image()
                {
                    Source = "headerReportes.png",
                    Aspect = Aspect.AspectFill
                });

            Grid Filtrado = new Grid
            {
                ColumnSpacing = 5,
                RowSpacing = 0,
                HorizontalOptions = LayoutOptions.Center,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (15, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) }
                }
            };
            globalizacion = new System.Globalization.CultureInfo("es-GT");
            fechaInicial = new ExtendedDatePicker
            {
                HasBorder = false,
                TextColor = Color.White,
                MaximumDate = DateTime.Now,
                Format = globalizacion.DateTimeFormat.ShortDatePattern,
                XAlign = TextAlignment.Center,
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };
            fechaFinal = new ExtendedDatePicker
            {
                HasBorder = false,
                TextColor = Color.White,
                MaximumDate = DateTime.Now,
                Format = globalizacion.DateTimeFormat.ShortDatePattern,
                XAlign = TextAlignment.Center,
                Font = Device.OnPlatform<Font>(Font.OfSize("OpenSans-Bold", 14), Font.OfSize("OpenSans-Bold", 14), Font.Default)
            };
            fechaInicial.Focused += (sender, e) =>
            {
                if (fechaFinal.IsFocused)
                {
                    fechaInicial.Unfocus();
                    return;
                }
                fechaInicial.TextColor = Color.FromHex("00ffff");
            };
            fechaFinal.Focused += (sender, e) =>
            {
                if (fechaInicial.IsFocused)
                {
                    fechaFinal.Unfocus();
                    return;
                }
                fechaFinal.TextColor = Color.FromHex("00ffff");
            };
            fechaInicial.Unfocused += (sender, e) =>
            {
                fechaInicial.TextColor = Color.White;
            };
            fechaFinal.Unfocused += (sender, e) =>
            {
                fechaFinal.TextColor = Color.White;
            };
            fechaInicial.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == DatePicker.DateProperty.PropertyName)
                {
                    if (fechaInicial.Date.Date > fechaFinal.Date.Date)
                    {
                        fechaFinal.Date = fechaInicial.Date;
                    }
                }
            };
            fechaFinal.PropertyChanged += async (sender, e) =>
            {
                if (e.PropertyName == DatePicker.DateProperty.PropertyName)
                {
                    if (fechaInicial.Date.Date > fechaFinal.Date.Date)
                    {
                        await DisplayAlert("¡Advertencia!", "La fecha final de filtrado debe ser mayor a la inicial.", "Aceptar");
                        fechaFinal.Date = fechaInicial.Date;
                        return;
                    }
                }
            };

            Filtrado.Children.Add(fechaInicial, 0, 0);
            Filtrado.Children.Add(
                new Label
                {
                    TextColor = Color.White,
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    Opacity = 0.90,
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                    Text = "a"
                }, 1, 0);
            Filtrado.Children.Add(fechaFinal, 2, 0);
            Filtrado.Children.Add(new BoxView { BackgroundColor = Color.White, Opacity = 0.80, HeightRequest = 0.5 }, 0, 1);
            Filtrado.Children.Add(new BoxView { BackgroundColor = Color.White, Opacity = 0.80, HeightRequest = 0.5 }, 2, 1);
            Header.Children.Add(
                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Spacing = 0,
                    Children =
                    {
                        new Label
                        {
                            TextColor = Color.White,
                            FontSize = 12,
                            HorizontalOptions = LayoutOptions.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Opacity = 0.80,
                            FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                            Text = "MOSTRANDO DE"
                        },
                        Filtrado
                    }
                });



            HeaderPacientes = new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 5),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 10,
                Children =
                {
                    Header,
                    PacientesHeader,
                    notificacionFiltrado
                }
            };


            Modal = new Grid();
            ModalBackground = new Grid
            {
                BackgroundColor = Color.Black,
                Padding = new Thickness(0, 0, 0, 0),
                WidthRequest = 200,
                HeightRequest = 200,
            };
            Modal.Children.Add(ModalBackground);

            var GestoModal = new TapGestureRecognizer();
            GestoModal.Tapped += (s, e) =>
            {
                if (BusquedaActiva)
                    BusquedaRapida.Text = "";
                OcultarModal();
            };
            Modal.GestureRecognizers.Add(GestoModal);
            ModalBackground.GestureRecognizers.Add(GestoModal);

            ContenidoCaja = new ScrollView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            refreshView = new PullToRefreshLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RefreshColor = Color.Accent,
                Content = ContenidoCaja
            };
            refreshView.RefreshCommand = RefreshCommand; ;
            //refreshView.SetBinding<CajaModeloVista>(PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);


            Contenido = new RelativeLayout();
            Contenido.Children.Add(HeaderPacientes,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width));

            Contenido.Children.Add(MenuContextual,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0));

            Contenido.Children.Add(
                    filtrarGesture,
                    Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return ((view.Width / 5) * 3.95); }),
                    Constraint.Constant(20),
                    Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return ((view.Width / 5) * 2); }),
                    Constraint.Constant(80)
                    );
            Contenido.Children.Add(
                    filtrar,
                    Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return ((view.Width / 6.2) * 5); }),
                    Constraint.Constant((120 / 2.2))
                    );
            Contenido.Children.Add(notificacionCajaVacia,
                    xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (view.Y + view.Height); }),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                    heightConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (parent.Height - view.Height); })
                                  );

            Content = Contenido;
            PacientesVistaVisible = false;
            CajaVistaPresentado = true;
        }

        ICommand refreshCommand;

        public ICommand RefreshCommand
        {
            get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
        }

        async Task ExecuteRefreshCommand()
        {
            /*;
            if (VistaModelo.IsBusy)
                return;
            var stack = Navigation.NavigationStack;
            VistaModelo.IsBusy = true;
            if (refrescar.IsEnabled && filtrar.IsEnabled && filtrarGesture.IsEnabled && !Constantes.ModalAbierto && (stack[stack.Count - 1].GetType() != typeof(Indicador)) && (stack[stack.Count - 1].GetType() != typeof(CajaDetalleModeloVista)))
            {
                ContenidoCaja.IsEnabled = false;
                await ActualizarCaja();
                ContenidoCaja.IsEnabled = true;
            }
            VistaModelo.IsBusy = false;*/
        }



        private void OpcionesToolBar_Clicked(object sender, EventArgs e)
        {
            RecargarMenuContextual();
        }


        private void mostrar_MenuContextual(bool Menu)
        {
            Modal.IsVisible = false;
            //Contenido.Children.Remove(Modal);
            if (Menu)
            {
                Contenido.Children.Remove(MenuContextual);
                MenuContextual.IsVisible = true;

                if (!CajaVistaPresentado)
                {
                    ModalBackground.Opacity = 0.3;
                    Contenido.Children.Add(Modal,
                        xConstraint: Constraint.Constant(0),
                        yConstraint: Constraint.Constant(0),
                        widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                        heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; })
                    );
                    Modal.IsVisible = true;
                }
                Contenido.Children.Add(MenuContextual,
                    xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - MenuContextual.Width); }),
                    yConstraint: Constraint.Constant(0));
            }
            else
            {
                if (!string.IsNullOrEmpty(BusquedaRapida.Text))
                {
                    string busqueda = BusquedaRapida.Text;
                    BusquedaRapida.Text = busqueda + " ";
                    BusquedaRapida.Text = busqueda;
                    BusquedaActiva = true;
                }
            }
        }

        void RecargarMenuContextual()
        {
            if (!isContextual)
            {
                mostrar_MenuContextual(true);
                MenuContextual.IsVisible = true;
                isContextual = true;
                BusquedaActiva = false;
            }
            else
            {
                mostrar_MenuContextual(false);
                MenuContextual.IsVisible = false;
                isContextual = false;
            }

        }


        async void BusquedaRapida_TextChanged(object sender, TextChangedEventArgs e)
        {
            BusquedaActiva = true;
            if (e.NewTextValue == null)
            {
                BusquedaRapida.Text = "";
                return;
            }
            await FiltrarTransacciones();
        }
        private async void OcultarModal()
        {
            if (isContextual)
            {
                MenuContextual.IsVisible = false;
                isContextual = false;
                mostrar_MenuContextual(false);
                return;
            }
        }
        private async Task ActualizarCaja()
        {
            /*await Navigation.PushPopupAsync(new Indicador("Actualizando caja", Color.White));
            BusquedaRapida.Text = String.Empty;
            VistaModelo.IsInitialized = true;
            SelectByDate peticionSelectByDate = new SelectByDate
            {
                fromDt = fechaInicial.Date.ToString(@"yyyy-MM-dd"),
                toDt = fechaFinal.Date.ToString(@"yyyy-MM-dd"),
                registerTy = "I"
            };
            await App.ManejadorDatos.SelectByDateAsync(peticionSelectByDate);
            Monedas = new List<CajaAgrupacionModeloVista>();
            foreach (var transaccion in App.Database.GetTransaccionesMonedas().ToList())
            {
                List<transacciones_caja> transacciones = App.Database.GetTransaccionesByCurrency_Cd(transaccion.Currency_Cd).ToList();
                Monedas.Add(new CajaAgrupacionModeloVista
                {
                    Moneda = transaccion.Currency_Cd,
                    Monto = transaccion.Amount,
                    Transacciones = transacciones,
                    TransaccionesOrigen = transacciones
                });
            }

            await CargarContenidoCaja();
            await Navigation.PopAllPopupAsync();
            if (CountRegistros != 0)
                ShowToast(ToastNotificationType.Info, "Caja", CountRegistros + " registro(s) listado(s)", 3);
            else
                ShowToast(ToastNotificationType.Info, "Caja", "No tienes registro(s) entre " + fechaInicial.Date.ToString(@"d", new System.Globalization.CultureInfo("es-GT")) + " y " + fechaInicial.Date.ToString(@"d", new System.Globalization.CultureInfo("es-GT")), 5);
            Constantes.ModalAbierto = false;*/
        }


        async Task FiltrarTransacciones()
        {
            /*if (Monedas == null || Monedas.Count == 0)
                return;
            foreach (var moneda in Monedas)
            {
                List<transacciones_caja> transaccionesFiltradas = new List<transacciones_caja>();

                switch (filtro)
                {
                    case "Patient_ID":
                        moneda.TransaccionesOrigen.Where(t => t.Register_Nm.ToLower().Contains(BusquedaRapida.Text.Trim().ToLower())).ToList().ForEach(t => transaccionesFiltradas.Add(t));
                        break;
                    case "DocumentNo":
                        moneda.TransaccionesOrigen.Where(t => t.vw_Doc.ToLower().Contains(BusquedaRapida.Text.Trim().ToLower())).ToList().ForEach(t => transaccionesFiltradas.Add(t));
                        break;
                    case "MethOfPay_Cd":
                        moneda.TransaccionesOrigen.Where(t => t.vw_Pymnt_Ty.ToLower().Contains(BusquedaRapida.Text.Trim().ToLower())).ToList().ForEach(t => transaccionesFiltradas.Add(t));
                        break;
                    case "ReferenceNo":
                        moneda.TransaccionesOrigen.Where(t => t.ReferenceNo.ToLower().Contains(BusquedaRapida.Text.Trim().ToLower())).ToList().ForEach(t => transaccionesFiltradas.Add(t));
                        break;
                    case "Ammount":
                        moneda.TransaccionesOrigen.Where(t => t.Amount.ToString("N", System.Globalization.CultureInfo.InvariantCulture).ToLower().Contains(BusquedaRapida.Text.Trim().ToLower())).ToList().ForEach(t => transaccionesFiltradas.Add(t));
                        break;
                }
                double monto = 0;
                moneda.Transacciones = transaccionesFiltradas;
                foreach (var transaccion in moneda.Transacciones)
                {
                    monto = monto + transaccion.Amount;
                }
                moneda.Monto = monto;
            }
            await CargarContenidoCaja();*/
        }


        private async Task CargarContenidoCaja()
        {
            ;
            CajaAccordion = new AccordionView()
            {
                FirstExpaned = true,
                DataSource = Caja()
            };
            CajaAccordion.DataBind();
            ContenidoCaja.Content = CajaAccordion;

            Contenido.Children.Add(refreshView,
                    xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (view.Y + view.Height); }),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                    heightConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (parent.Height - view.Height); })
                                   );
            CajaVacia();
        }

        void CajaVacia()
        {
            if (Monedas == null || Monedas.Count == 0)
            {
                notificacionCajaVacia.IsVisible = true;
            }
            else
            {
                bool visible = false;
                foreach (var moneda in Monedas)
                {
                    if (moneda.Transacciones.Count > 0)
                    {
                        visible = false;
                        break;
                    }
                    else
                        visible = true;
                }
                notificacionCajaVacia.IsVisible = visible;
            }
        }

        public List<AccordionSource> Caja()
        {
            CountRegistros = 0;
            List<AccordionSource> ItemsCaja = new List<AccordionSource>();
            foreach (var moneda in Monedas)
            {
                if (moneda.Cabecera.IsVisible && moneda.Contenido.IsVisible)
                {
                    ItemsCaja.Add(new AccordionSource
                    {
                        Cabecera = moneda.Cabecera,
                        Contenido = moneda.Contenido
                    });
                    CountRegistros += moneda.Transacciones.Count();
                }
            }
            return ItemsCaja;
        }

        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (CajaVistaPresentado)
            {
                await ActualizarCaja();
                CajaVistaPresentado = false;
            }
            //Constants.ModalAbierto = false;
            await Navigation.PopAllPopupAsync();
            NavigationPage.SetBackButtonTitle(this, String.Empty);
        }
    }
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}
