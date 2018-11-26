using System;
using System.Collections.Generic;
using System.Linq;
using EGuardian.Controls;
using EGuardian.Helpers;
using EGuardian.Models.Empleados;
using EGuardian.Models.Eventos;
using EGuardian.ViewModels.Perfil.Empleado;
using EGuardian.Views.Menu;
using EGuardian.Views.Perfil;
using EGuardian.Views.Perfil.Empleado;
using SuaveControls.Views;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Perfil
{
    public class EmpleadosDTViewModel : ContentView
    {
        bool PacientesVistaPresentado, PacientesVistaVisible, MenuPresentado,
        ABCPresentado, BusquedaActiva;
        FloatingActionButton addFAB;
        //private FloatingActionButton menuFAB, nPacienteFAB, nCitaFAB;
        ActivityIndicator indicadorFooterPacientes;
        //Frame nPacienteMenu, nCitaMenu;
        Label tituloFooterPacientes;
        RelativeLayout Contenido;
        Image iBusquedaIndicador;
        Grid Modal;
        //ToolbarItem ABCToolBar;
        ExtendedEntry BusquedaRapida;
        public ListView Pacientes;
        StackLayout HeaderPacientes;
        //StackLayout ABC;
        //Grid Modal;
        Label ModalInstruccion, ModalMensaje;
        List<empleados> Empleados = App.Database.GetEmpleados(Convert.ToInt32(Settings.session_idUsuario)).ToList();

        void Pacientes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new EmpleadoPage((empleados)e.SelectedItem));
        }

        public EmpleadosDTViewModel()
        {

            MessagingCenter.Subscribe<EmpleadoPage>(this, "DisplayAlert", (sender) =>
            {
                System.Diagnostics.Debug.WriteLine("Método para obtener empleados");
            });

            MessagingCenter.Subscribe<MainPage>(this, "CargaEmpleados", (sender) =>
            {
                ActualizarPacientes();
            });

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
                //Foreground = Color.FromHex("F7B819"),
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

            Pacientes = new ListView
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //IsScrollEnable = false,
                ItemsSource = Empleados,
                ItemTemplate = new DataTemplate(typeof(EmpleadoDTViewModel)),
                Margin = 0,
                RowHeight = 55, //Convert.ToInt32((App.DisplayScreenHeight / 13.533333333333333)),
                IsPullToRefreshEnabled = false,
                SeparatorVisibility = SeparatorVisibility.None,
                SeparatorColor = Color.Transparent,
                HasUnevenRows = false,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            //Pacientes.sour
            Pacientes.ItemSelected += Pacientes_ItemSelected;
            Pacientes.Focused += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("entro a focus lista");
                try { BusquedaRapida.Unfocus(); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
            };

            try
            {
                addFAB = new FloatingActionButton
                {
                    Image = "iFABPa",
                    ButtonColor = Color.FromHex("F7B819"),
                    BorderColor = Color.FromHex("F7B819"),
                    TextColor = Color.FromHex("F7B819"),
                    //BackgroundColor = Color.FromHex("F7B819")
                };
                addFAB.Clicked += AddFAB_Clicked;;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            Contenido = new RelativeLayout();
            Contenido.Children.Add(HeaderPacientes,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width));
            Padding = new Thickness(0, 10, 0, 0);
            Content = Contenido;
            PacientesVistaVisible = false;
            PacientesVistaPresentado = true;
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
            Contenido.Children.Add(addFAB,
               xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width - 66); }),
               yConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height - 66); })
            );
        }


        void AddFAB_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EmpleadoPage(new empleados()));
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
            Empleados.Clear();
            Empleados = App.Database.GetEmpleados(Convert.ToInt32(Settings.session_idUsuario)).ToList();
            Pacientes.ItemsSource = null;
            Pacientes.ItemsSource = Empleados;
        }
    }
}
