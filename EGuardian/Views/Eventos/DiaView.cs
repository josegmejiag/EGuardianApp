using System;
using EGuardian.Common.Resources;
using EGuardian.Views.Menu;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Forms;

namespace EGuardian.Views.Eventos
{
    public class DiaView : ContentView
    {
        System.Globalization.CultureInfo globalizacion;
        DateTime fecha;
        public DateTime fechaSeleccionada;
        Label mes, dia1, dia2, dia3, dia4, dia5, dia6, dia7;
        Grid grid;
        public DiaView()
        {
            MessagingCenter.Subscribe<MainPage>(this, "Unsubscribe", (sender) =>
            {
                MessagingCenter.Unsubscribe<Dia>(this, "selecciono");
            });

            fechaSeleccionada = fecha = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("DIA ACTUAL" + fecha.ToString());
            System.Diagnostics.Debug.WriteLine("DIA SELECCIONADO" + fechaSeleccionada.ToString());
            globalizacion = new System.Globalization.CultureInfo("es-GT");
            StackLayout anterior = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    new Image
                    {
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Source = Images.Anterior,
                        WidthRequest = 15,
                        HeightRequest = 15,
                    }
                }
            };
            StackLayout siguiente = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    new Image
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        Source = Images.Siguiente,
                        WidthRequest = 15,
                        HeightRequest = 15,
                    }
                }
            };
            TapGestureRecognizer tapAnterior = new TapGestureRecognizer();
            tapAnterior.Tapped += (sender, e) => { Anterior_Clicked(); };
            TapGestureRecognizer tapSiguiente = new TapGestureRecognizer();
            tapSiguiente.Tapped += (sender, e) => { Siguiente_Clicked(); };
            anterior.GestureRecognizers.Add(tapAnterior);
            siguiente.GestureRecognizers.Add(tapSiguiente);
            MessagingCenter.Subscribe<Dia>(this, "selecciono", (sender) =>
            {
                if (fechaSeleccionada.Date != sender.fecha.Date)
                {
                    System.Diagnostics.Debug.WriteLine(fechaSeleccionada);
                    fechaSeleccionada = sender.fecha;
                    actualizar();
                }
                MessagingCenter.Send<DiaView>(this, "selecciono");
            });
            mes = new Label
            {
                Text = globalizacion.DateTimeFormat.GetMonthName(fecha.Month).ToUpper() + " " + fecha.Year,
                TextColor = Color.FromHex("432161"),
                FontSize = 22,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)               
            };
            Grid grid2 = new Grid
            {
                Padding = 20,
                RowSpacing = 1,
                ColumnSpacing = 5,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (100, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };
            grid2.Children.Add(
                anterior
                , 0, 0);
            grid2.Children.Add(
                mes
                , 1, 0);
            grid2.Children.Add(
                siguiente
                , 2, 0);

            dia1 = new Label()
            {
                Text = "D",
                TextColor = Color.FromHex("432161"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            dia2 = new Label()
            {
                Text = "L",
                TextColor = Color.FromHex("432161"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            dia3 = new Label()
            {
                Text = "M",
                TextColor = Color.FromHex("432161"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            dia4 = new Label()
            {
                Text = "M",
                TextColor = Color.FromHex("432161"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            dia5 = new Label()
            {
                Text = "J",
                TextColor = Color.FromHex("432161"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            dia6 = new Label()
            {
                Text = "V",
                TextColor = Color.FromHex("432161"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)

            };

            dia7 = new Label()
            {
                Text = "S",
                TextColor = Color.FromHex("432161"),
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            grid = new Grid
            {

                RowSpacing = 0,
                ColumnSpacing = 5,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (30, GridUnitType.Absolute) }
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
            actualizar();
            Padding = new Thickness(5, 5, 5, 5);
            Content = new StackLayout
            {
                Padding = 0,
                Spacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    grid2,
                    grid
                }
            };
        }

        void actualizar()
        {
            grid.Children.Clear();
            grid.Children.Add(dia1, 0, 0);
            grid.Children.Add(dia2, 1, 0);
            grid.Children.Add(dia3, 2, 0);
            grid.Children.Add(dia4, 3, 0);
            grid.Children.Add(dia5, 4, 0);
            grid.Children.Add(dia6, 5, 0);
            grid.Children.Add(dia7, 6, 0);
            int fila = 1;
            for (int i = 1; i <= DateTime.DaysInMonth(fecha.Year, fecha.Month); i++)
            {
                DateTime diaActual = new DateTime(fecha.Year, fecha.Month, i);
                if (fechaSeleccionada.Date == diaActual.Date)
                    grid.Children.Add(new Dia(diaActual, true), (int)diaActual.DayOfWeek, fila);
                else
                    grid.Children.Add(new Dia(diaActual, false), (int)diaActual.DayOfWeek, fila);
                if ((int)diaActual.DayOfWeek == 6)
                    fila++;
            }
        }

        void Anterior_Clicked()
        {
            fecha.Date.AddMonths(-1);
            fecha = fecha.AddMonths(-1);
            mes.Text = globalizacion.DateTimeFormat.GetMonthName(fecha.Month).ToUpper() + " " + fecha.Year;
            actualizar();
        }

        void Siguiente_Clicked()
        {
            fecha = fecha.AddMonths(1);
            mes.Text = globalizacion.DateTimeFormat.GetMonthName(fecha.Month).ToUpper() + " " + fecha.Year;
            actualizar();
        }
    }

    class Dia : ContentView
    {
        Button dia;
        public DateTime fecha;
        public Dia(DateTime fecha, bool seleccionado)
        {
            this.fecha = fecha;
            Grid grid = new Grid
            {

                RowSpacing = 1,
                ColumnSpacing = 5,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (30, GridUnitType.Absolute) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };
            var circulo = new CircleImage
            {
                BorderThickness = 0,
                HeightRequest = 25,
                WidthRequest = 25,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            dia = new Button
            {
                BackgroundColor = Color.Transparent,
                TextColor = Color.FromHex("B2B2B2"),
                Text = this.fecha.Day.ToString(),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };
            grid.Children.Add(circulo, 0, 0);
            grid.Children.Add(dia, 0, 0);

            if (fecha.Date == DateTime.Now.Date)
            {
                circulo.Source = Images.Actual;
                dia.TextColor = Color.White;
            }
            if (seleccionado)
            {
                circulo.Source = Images.Seleccionada;
                dia.TextColor = Color.White;
            }
            dia.Clicked += Dia_Clicked;
            Content = grid;
        }

        void Dia_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<Dia>(this, "selecciono");
        }
    }
}

