using System;
using System.Collections.Generic;
using System.Linq;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using EGuardian.Models.Acciones;
using EGuardian.Models.Eventos;
using EGuardian.ViewModels.Ajustes;
using EGuardian.ViewModels.Eventos.Empleados;
using Xamarin.Forms;

namespace EGuardian.Views.Eventos.Acciones
{
    public class AccionesFiltradoPage : ContentPage
    {
        RelativeLayout Contenido;
        List<acciones> actions = App.Database.GetAcciones().ToList();
        public ExtendedListView AccionesContenido;

        void AccionesContenido_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MessagingCenter.Send<AccionesFiltradoPage>(this, "OK");
            Navigation.PopModalAsync();
        }

        public AccionesFiltradoPage()
        {
            Padding = new Thickness(0, 30, 0, 0);
            BackgroundColor = Colors.BarBackgroundColor;

            IconView cerrar = new IconView
            {
                WidthRequest = 20,
                HeightRequest = 20,
                Margin = new Thickness(0, 0, 10, 0),
                HorizontalOptions = LayoutOptions.End,
                Source = Images.Cancelar,
                Foreground = Color.FromHex("E9242A")
            };

            TapGestureRecognizer cerrarTAP = new TapGestureRecognizer();
            cerrarTAP.Tapped += CerrarTAP_Tapped;
            cerrar.GestureRecognizers.Add(cerrarTAP);

            Grid botonCerrar = new Grid
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

            StackLayout Header = new StackLayout
            {
                Padding = new Thickness(10, 0, 10, 20),
                Children = 
                {
                    new Label
                    {
                        Text = "Selecciona para agregar",
                        FontSize = 15,
                        FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                        HorizontalTextAlignment = TextAlignment.Center,
                        TextColor = Color.White,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new BoxView {BackgroundColor = Color.Transparent, HeightRequest=30}
                }
            };

            AccionesContenido = new ExtendedListView
            {
                Margin = 0,
                IsPullToRefreshEnabled = false,
                BackgroundColor = Color.Transparent,
                ItemTemplate = new DataTemplate(typeof(AppsActionsDTViewModel)),
                HasUnevenRows = false,
                RowHeight = 90,
                SeparatorColor = Color.FromHex("E5E5E5"),
                ItemsSource = actions
            };
            AccionesContenido.ItemSelected+= AccionesContenido_ItemSelected;


            Contenido = new RelativeLayout();
            Contenido.Children.Add(Header,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent(parent => parent.Width));
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
                            Children = { AccionesContenido }
                        },
                        new BoxView {BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4}
                    }
                },
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.RelativeToView(Header, (parent, view) => { return view.Height; }),
                widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                                   heightConstraint: Constraint.RelativeToView(Header, (parent, view) => { return (parent.Height - view.Height); })
            );

            Contenido.Children.Add(botonCerrar,
                    xConstraint: Constraint.Constant(-20),
                    yConstraint: Constraint.Constant(30)
           );
            Content = Contenido;
        }

        async void CerrarTAP_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}
