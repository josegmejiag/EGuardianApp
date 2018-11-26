using CarouselView.FormsPlugin.Abstractions;
using EGuardian.Models.Tutorial;
using EGuardian.ViewModels.Tutorial;
using EGuardian.Views.Acceso;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Views.Tutorial
{
    public class TutorialPage : ContentPage
    {
        CarouselViewControl tutorialCarousel;
        List<TutorialItem> contenidoCarousel;
        Button comenzar;
        StackLayout Indicator1, Indicator2, Indicator3, Indicator4;
        Grid indicadores;

        public TutorialPage()
        {
            BackgroundColor = Color.FromHex("312851");
            contenidoCarousel = new List<TutorialItem>();
            contenidoCarousel.Add(
                new TutorialItem
                {
                    index = 0,
                    texto = "EGuardian\r\nControla tu inversión",
                    imagen = "tutorialItem1.png",
                    Padding = new Thickness(0, 50, 0, 0)
                });
            contenidoCarousel.Add(
                new TutorialItem
                {
                    index = 1,
                    texto = "¿Tiene idea de lo que hacen\r\nsus empleados?",
                    imagen = "tutorialItem2.png",
                    Padding = 0//new Thickness(0, 350, 0, 0)
                });
            contenidoCarousel.Add(
                new TutorialItem
                {
                    index = 2,
                    texto = "¿Está seguro de que su empleado\r\nAprovecha su inversión?",
                    imagen = "tutorialItem3.png",
                    Padding = 0//new Thickness(0, 350, 0, 0)
                });
            contenidoCarousel.Add(
                new TutorialItem
                {
                    index = 3,
                    texto = "¡Este al tanto en todo momento!",
                    imagen = "tutorialItem4.png",
                    Padding = 0//new Thickness(0, 350, 0, 0)
                });

            tutorialCarousel = new CarouselViewControl
            {
                ItemsSource = contenidoCarousel,
                ItemTemplate = new DataTemplate(typeof(TutorialViewModel)),
                InterPageSpacing = 0,
                HeightRequest = 70,
                Orientation = CarouselViewOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };


            Indicator1 = new StackLayout
            {                
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children ={
                    new Image
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Source = "indicator_fill"}
                    },

            };

            Indicator2 = new StackLayout
            {
                Spacing = 0,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children ={
                    new Image
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Source = "indicator_fill"}
                    }
            };
            Indicator3 = new StackLayout
            {                
                Spacing = 0,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children ={
                    new Image
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Source = "indicator_fill"}
                    }
            };
            Indicator4 = new StackLayout
            {                
                Spacing = 0,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =              {
                    new Image
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Source = "indicator_fill"                    }
                    }
            };

            TapGestureRecognizer tapIndicator1 = new TapGestureRecognizer();
            tapIndicator1.Tapped += (sender, e) =>
            {
                if (tutorialCarousel.Position != 0)
                    tutorialCarousel.Position=0;
            };
            TapGestureRecognizer tapIndicator2 = new TapGestureRecognizer();
            tapIndicator2.Tapped += (sender, e) =>
            {
                if (tutorialCarousel.Position != 1)
                    tutorialCarousel.Position=1;
            };
            TapGestureRecognizer tapIndicator3 = new TapGestureRecognizer();
            tapIndicator3.Tapped += (sender, e) =>
            {
                if (tutorialCarousel.Position != 2)
                    tutorialCarousel.Position=2;
            };
            TapGestureRecognizer tapIndicator4 = new TapGestureRecognizer();
            tapIndicator4.Tapped += (sender, e) =>
            {
                if (tutorialCarousel.Position != 3)
                    tutorialCarousel.Position=3;
            };

            Indicator1.GestureRecognizers.Add(tapIndicator1);
            Indicator2.GestureRecognizers.Add(tapIndicator2);
            Indicator3.GestureRecognizers.Add(tapIndicator3);
            Indicator4.GestureRecognizers.Add(tapIndicator4);

            indicadores = new Grid
            {
                HeightRequest = 20,
                RowSpacing = 0,
                ColumnSpacing = 5,
                Padding = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) }
                }
            };

            indicadores.Children.Add(Indicator1, 0, 0);
            indicadores.Children.Add(Indicator2, 1, 0);
            indicadores.Children.Add(Indicator3, 2, 0);
            indicadores.Children.Add(Indicator4, 3, 0);

            comenzar = new Button
            {
                Text = "SIGUIENTE",
                AutomationId = "comenzar",
                WidthRequest = 300,
                HeightRequest = 50,
                FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null),
                FontSize = 20,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.FromHex("F7B819")
            };
            comenzar.Clicked += Comenzar_Clicked;

            RelativeLayout Contenido = new RelativeLayout();
            Contenido.Children.Add(tutorialCarousel,
                                   Constraint.Constant(0),
                                   Constraint.Constant(0),
                                   Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                   Constraint.RelativeToParent((parent) => { return parent.Height; })
                                  );

            Contenido.Children.Add(indicadores,
                                   Constraint.RelativeToParent((parent) => { return (parent.Width / 2) - 50; }),
                                   Constraint.RelativeToParent((parent) => { return parent.Height - (parent.Height / 7.35) - 60; })
                                  );

            Contenido.Children.Add(comenzar,
                                   Constraint.RelativeToParent((parent) => { return (parent.Width / 2) - 150; }),
                                   Constraint.RelativeToParent((parent) => { return parent.Height - (parent.Height / 7.35); })
                                  );
            Content = Contenido;
            tutorialCarousel.PositionSelected += tutorialCarousel_PositionSelected;
        }

        void Comenzar_Clicked(object sender, EventArgs e)
        {
            if (comenzar.Text.Equals("COMENZAR"))
                Navigation.PushModalAsync(new LoginPage());
            else
                tutorialCarousel.Position = tutorialCarousel.Position + 1;
        }

        void defaultIndicators()
        {
            ((Image)Indicator1.Children[0]).Source = "indicator_fill";
            ((Image)Indicator2.Children[0]).Source = "indicator_fill";
            ((Image)Indicator3.Children[0]).Source = "indicator_fill";
            ((Image)Indicator4.Children[0]).Source = "indicator_fill";
        }

        void tutorialCarousel_PositionSelected(object sender, EventArgs e)
        {
            try
            {
                switch (tutorialCarousel.Position)
                {
                    case 0:
                        defaultIndicators();
                        ((Image)Indicator1.Children[0]).Source = "indicator";
                        comenzar.Text = "SIGUIENTE";
                        break;
                    case 1:
                        defaultIndicators();
                        ((Image)Indicator2.Children[0]).Source = "indicator";
                        comenzar.Text = "SIGUIENTE";
                        break;
                    case 2:
                        defaultIndicators();
                        ((Image)Indicator3.Children[0]).Source = "indicator";
                        comenzar.Text = "SIGUIENTE";
                        break;
                    case 3:
                        defaultIndicators();
                        ((Image)Indicator4.Children[0]).Source = "indicator";
                        comenzar.Text = "COMENZAR";
                        break;
                    default:
                        defaultIndicators();
                        ((Image)Indicator1.Children[0]).Source = "indicator";
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}