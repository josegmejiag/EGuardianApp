using System;
using System.Linq;
using EGuardian.Controls;
using EGuardian.Helpers;
using EGuardian.Models.Eventos;
using EGuardian.ViewModels.Eventos.Empleados;
using EGuardian.ViewModels.Eventos.Evento;
using Xamarin.Forms;

namespace EGuardian.Views.Eventos.Asistentes
{
    public class AsistentesFiltradoPage : ContentPage
    {
        bool PacientesVistaPresentado, PacientesVistaVisible, MenuPresentado,

        ABCPresentado, BusquedaActiva;
        //private FloatingActionButton menuFAB, nPacienteFAB, nCitaFAB;
        ActivityIndicator indicadorFooterPacientes;
        //Frame nPacienteMenu, nCitaMenu;
        Label tituloFooterPacientes;
        RelativeLayout Contenido;
        Image iBusquedaIndicador;
        Grid Modal;
        bool capacitador;
        ExtendedEntry BusquedaRapida;
        public ListView Pacientes;
        StackLayout HeaderPacientes;

        Label ModalInstruccion, ModalMensaje;

        void Pacientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(capacitador)
                MessagingCenter.Send<AsistentesFiltradoPage>(this, "OK");
            else
                MessagingCenter.Send<AsistentesFiltradoPage>(this, "OK_A");
            Navigation.PopModalAsync();
        }
        public AsistentesFiltradoPage(bool capacitador)
        {
            Title = "Filtrado";
            this.capacitador = capacitador;

            indicadorFooterPacientes = new ActivityIndicator
            {
                IsRunning = true,
                Color = Color.Accent,
                HorizontalOptions = LayoutOptions.End,
                WidthRequest = 20
            };
            tituloFooterPacientes = new Label
            {
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "Cargando empleados..."
            };
            Grid PacientesFooter = new Grid
            {
                HeightRequest = 60,
                Padding = new Thickness(5, 0, 5, 0),
                RowSpacing = 1,
                ColumnSpacing = 1,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };

            PacientesFooter.Children.Add(indicadorFooterPacientes, 0, 0);
            PacientesFooter.Children.Add(tituloFooterPacientes, 1, 0);

            BusquedaRapida =
                new ExtendedEntry
                {
                    Placeholder = "Buscar",
                    HasBorder = false,
                    BackgroundColor = Color.Transparent,
                    Margin = new Thickness(10, 0),
                    HeightRequest = 30,
                    PlaceholderColor = Color.FromHex("432161"),
                    FontSize = 14,
                    TextColor = Color.FromHex("432161"),
                    FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
                };

            BusquedaRapida.TextChanged += BusquedaRapida_TextChanged;

            Grid PacientesHeader = new Grid
            {
                //Padding = new Thickness(0,0,10,0),
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
            Image cancelar = new Image
            {
                //Foreground = Color.FromHex("F7B819"),
                Source = "iCancelarB.png",
                HeightRequest = 25,
                WidthRequest = 25,

            };
            TapGestureRecognizer cancelarTAP = new TapGestureRecognizer();
            cancelarTAP.Tapped += (sender, e) => { Navigation.PopModalAsync(); };
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
                /*OcultarModal();
                if (String.IsNullOrEmpty(BusquedaRapida.Text))
                {
                    await DisplayAlert("Búsqueda avanzada", "Debe ingresar una expresión para buscar en la nube.", "Aceptar");
                    return;
                }
                tituloFooterPacientes.Text = "Buscando coincidencias...";
                indicadorFooterPacientes.IsRunning = true;


                SelectForList peticion = new SelectForList
                {
                    searchText = BusquedaRapida.Text
                };
                await App.ManejadorDatos.SelectForListAsync(peticion, true);
                var PacientesActualizados = new ObservableCollection<pacientes>(App.Database.GetPacientesOrderByNombre());
                try
                {
                    VistaModelo.FiltrarPacientes(BusquedaRapida.Text.Trim(), PacientesActualizados.ToList());
                    Pacientes.ItemsSource = VistaModelo.PacientesAgrupados;
                }
                finally
                {
                    indicadorFooterPacientes.IsRunning = false;
                    if (VistaModelo.Pacientes.Count != 0)
                        tituloFooterPacientes.Text = VistaModelo.Pacientes.Count + " paciente(s) encontrados";
                    else
                    {
                        iBusquedaIndicador.IsVisible = false;
                        tituloFooterPacientes.Text = VistaModelo.Pacientes.Count + " paciente(s) encontrados";
                        ModalMensaje.Text = "Lo lamentamos, tampoco hemos encontrado resultados para tu búsqueda: '" + BusquedaRapida.Text + "' en medicloud.me (web) intente con otra expresión.";
                        ModalInstruccion.Text = "";
                        Contenido.Children.Add(Modal,
                                xConstraint: Constraint.Constant(0),
                                yConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (view.Y + view.Height); }),
                                widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                heightConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (parent.Height - view.Height); })
                            );
                    }
                }*/
            };
            buscar.GestureRecognizers.Add(buscarTAP);
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

            HeaderPacientes = new StackLayout
            {
                Padding = new Thickness(10, 10, 10, 20),
                Children = { PacientesHeader }
            };



            Modal = new Grid();
            Modal.Children.Add(new Grid
            {
                BackgroundColor = Color.Black,
                Opacity = 0.65,
                Padding = new Thickness(0, 0, 0, 0),
                WidthRequest = 200,
                HeightRequest = 200,
            });

            ModalMensaje = new Label
            {
                TextColor = Color.White,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            ModalInstruccion = new Label
            {
                TextColor = Color.White,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };
            iBusquedaIndicador = new Image
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                WidthRequest = 25,
                HeightRequest = 25,
                Source = "iBusquedaIndicador.png"
            };
            StackLayout ModalContenido = new StackLayout
            {
                Padding = 15,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    iBusquedaIndicador,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Spacing = 15,
                        Padding = 30,
                        Children =
                        {
                            ModalMensaje,
                            ModalInstruccion
                        }

                    }
                }
            };

            Modal.Children.Add(ModalContenido);





            var GestoModal = new TapGestureRecognizer();
            GestoModal.Tapped += (s, e) =>
            {
                ActualizarPacientes();
                OcultarModal();
                BusquedaRapida.Focus();
            };

            ModalContenido.GestureRecognizers.Add(GestoModal);
            Pacientes = new ListView
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //IsScrollEnable = false,
                ItemsSource = App.Database.GetEmpleados(Convert.ToInt32(Settings.session_idUsuario)).ToList(),
                ItemTemplate = new DataTemplate(typeof(EventoEmpleadosDTViewModel)),
                Margin = 0,
                RowHeight = 55, //Convert.ToInt32((App.DisplayScreenHeight / 13.533333333333333)),
                IsPullToRefreshEnabled = false,
                SeparatorVisibility = SeparatorVisibility.None,
                SeparatorColor = Color.Transparent,
                HasUnevenRows = false,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            //Pacientes.IsPullToRefreshEnabled
            Pacientes.Refreshing += Pacientes_Refreshing;
            Pacientes.ItemSelected += Pacientes_ItemSelected;
            Pacientes.Focused += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("entro a focus lista");
                try { BusquedaRapida.Unfocus(); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
            };

            Contenido = new RelativeLayout();
            Contenido.Children.Add(HeaderPacientes,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width));
            Padding = new Thickness(0, 40, 0, 0);
            Content = Contenido;
            PacientesVistaVisible = false;
            PacientesVistaPresentado = true;
        }

        private void Pacientes_Refreshing(object sender, EventArgs e)
        {
            var lista = (ListView)sender;
            ActualizarPacientes();
            mostrar_ABC();
            lista.IsRefreshing = false;
        }

        private async void OcultarModal()
        {
            Contenido.Children.Remove(Modal);
        }
        void BusquedaRapida_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*OcultarModal();
            if (e.NewTextValue == null)
            {
                BusquedaRapida.Text = "";
                return;
            }

            var PacientesActualizados = new ObservableCollection<pacientes>(App.Database.GetPacientesOrderByNombre());
            VistaModelo.FiltrarPacientes(BusquedaRapida.Text.Trim(), PacientesActualizados.ToList());
            Pacientes.ItemsSource = VistaModelo.PacientesAgrupados;
            indicadorFooterPacientes.IsRunning = false;
            if (VistaModelo.ConteoPacientes != 0)
                tituloFooterPacientes.Text = VistaModelo.ConteoPacientes + " Paciente(s) encontrados";
            else
            {
                if (BusquedaRapida.Text != "")
                {
                    iBusquedaIndicador.IsVisible = true;
                    tituloFooterPacientes.Text = VistaModelo.ConteoPacientes + " Paciente(s) encontrados";
                    ModalMensaje.Text = "No hemos encontrado resultados para tu búsqueda: '" + BusquedaRapida.Text + "'";
                    ModalInstruccion.Text = "Por favor haz clic sobre el icono de la lupa para buscar en medicloud.me (web).";
                    Contenido.Children.Add(Modal,
                            xConstraint: Constraint.Constant(0),
                            yConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (view.Y + view.Height); }),
                            widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                            heightConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (parent.Height - view.Height); })
                        );
                }
            }*/
        }



        private void ActualizarPacientes()
        {
            /*
            BusquedaRapida.Text = String.Empty;
            indicadorFooterPacientes.IsRunning = true;
            tituloFooterPacientes.Text = "Cargando pacientes...";
            VistaModelo.IsInitialized = true;
            var PacientesActualizados = new ObservableCollection<pacientes>(App.Database.GetPacientesOrderByNombre());
            try
            {
                Pacientes.ItemsSource = new ObservableCollection<PacientesAgrupacion<string, pacientes>>(from paciente in PacientesActualizados
                                                                                                         orderby paciente.First_Nm
                                                                                                         group paciente by paciente.inicialNombre into pacientesAgrupados
                                                                                                         select new PacientesAgrupacion<string, pacientes>(pacientesAgrupados.Key, pacientesAgrupados));
            }
            finally
            {
                indicadorFooterPacientes.IsRunning = false;
                if (PacientesActualizados.Count != 0)
                    tituloFooterPacientes.Text = PacientesActualizados.Count + " Paciente(s) encontrados";
                else
                    tituloFooterPacientes.Text = "No se han encontrado pacientes";
            }*/
        }

        private void mostrar_ABC()
        {

            Contenido.Children.Add(new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Spacing = 0,
                    Children =
                            {
                                new Grid
                                {
                                    BackgroundColor = Color.FromHex("E5E5E5"),
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    VerticalOptions = LayoutOptions.FillAndExpand,
                                    Padding = new Thickness(20, 0),
                            Children = { Pacientes }
                                },
                                new BoxView {BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4}
                            }
                },
                xConstraint: Constraint.Constant(0),
                // yConstraint: Constraint.Constant(0),
                yConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return view.Height; }),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                                   heightConstraint: Constraint.RelativeToView(HeaderPacientes, (parent, view) => { return (parent.Height - view.Height); })
            );
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (PacientesVistaPresentado)
            {
                mostrar_ABC();
                ActualizarPacientes();
                PacientesVistaPresentado = false;
            }
            BusquedaRapida.Focus();

        }
    }
}
