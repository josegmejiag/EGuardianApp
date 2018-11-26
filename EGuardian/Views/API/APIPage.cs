using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGuardian.Common;
using EGuardian.Common.Resources;
using EGuardian.Controls;
using Plugin.Messaging;
using Plugin.Toasts;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.Views.API
{
	public class APIPage : ContentPage
    {
        Image headerBackground;
        RelativeLayout contenido2, gridBoton;
        Button guardar;
        StackLayout EdicionCreacion;
        ScrollView contenidoCreacionEdicion;


        public APIPage()
        {
            Title = "Información & API";
            NavigationPage.SetBackButtonTitle(this, "");

            Grid grid3 = new Grid
            {
                RowSpacing = 5,
                ColumnSpacing = 15,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength (1, GridUnitType.Auto) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                }
            };

            IconView iTelefono = new IconView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "iTelefono.png",
                WidthRequest = 15,
                HeightRequest = 15,
                Foreground = Color.FromHex("09818D")
            };
            Label telefono = new Label
            {
                Text = "+(502) 4210-4671",
                FontSize = 13,
                TextColor = Color.FromHex("3F3F3F"),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };

            IconView iCorreo = new IconView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "iCorreo.png",
                WidthRequest = 15,
                HeightRequest = 15,
                Foreground = Color.FromHex("09818D")
            };

            Label correo = new Label
            {
                Text = "josegmejiag@gmail.com",
                FontSize = 13,
                TextColor = Color.FromHex("3F3F3F"),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };

            grid3.Children.Add(
                iTelefono, 0, 0);
            grid3.Children.Add(
                iCorreo, 1, 0);
            grid3.Children.Add(
                telefono, 0, 1);
            grid3.Children.Add(
                correo, 1, 1);
            grid3.Children.Add(
                new BoxView { BackgroundColor = Color.FromHex("09818D"), HeightRequest = 1 }
                , 0, 2);
            grid3.Children.Add(
                new BoxView { BackgroundColor = Color.FromHex("09818D"), HeightRequest = 1 }
                , 1, 2);


            TapGestureRecognizer telefonoTAP = new TapGestureRecognizer();
            telefonoTAP.Tapped += (sender, e) =>
            {
                try
                {
                    /*if (Device.OS == TargetPlatform.Android)
                        DependencyService.Get<iPhoneCall>().MakeQuickCall("+(502)42104671");
                    else if (Device.OS == TargetPlatform.iOS)*/
                    {
                        var phoneCallTask = CrossMessaging.Current.PhoneDialer;
                        if (phoneCallTask.CanMakePhoneCall)
                            phoneCallTask.MakePhoneCall("+(502)42104671");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };

            telefono.GestureRecognizers.Add(telefonoTAP);
            iTelefono.GestureRecognizers.Add(telefonoTAP);

            TapGestureRecognizer correoTAP = new TapGestureRecognizer();
            correoTAP.Tapped += (sender, e) =>
            {
                var emailTask = CrossMessaging.Current.EmailMessenger;
                if (emailTask.CanSendEmail)
                {
                    // Send simple e-mail to single receiver without attachments, CC, or BCC.
                    emailTask.SendEmail("josegmejiag@gmail.com", "[EGuardian App] Solicitud de información", "");

                    // Send a more complex email with the EmailMessageBuilder fluent interface.
                    var email = new EmailMessageBuilder()
                      .To("josegmejiag@gmail.com")
                      .Subject("[EGuardian App] Solicitud de información")
                      .Build();

                    emailTask.SendEmail(email);
                }
            };

            correo.GestureRecognizers.Add(correoTAP);
            iCorreo.GestureRecognizers.Add(correoTAP);
            IconView iDireccion = new IconView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "iUbicacion.png",
                WidthRequest = 15,
                HeightRequest = 15,
                Foreground = Color.FromHex("09818D")
            };

            Label url = new Label
            {
                Text = "https://www.linkedin.com/in/josemejiait",
                FontSize = 13,
                TextColor = Color.FromHex("3F3F3F"),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
            };
            IconView iUrl = new IconView
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "iWeb.png",
                WidthRequest = 13,
                HeightRequest = 13,
                Foreground = Color.FromHex("09818D")
            };
            TapGestureRecognizer urlTAP = new TapGestureRecognizer();
            urlTAP.Tapped += (s, e) =>
            {
                Device.OpenUri(new Uri("https://www.linkedin.com/in/josemejiait"));
            };
            url.GestureRecognizers.Add(urlTAP);
            iUrl.GestureRecognizers.Add(urlTAP);





            StackLayout datosGenerales = new StackLayout
            {
                Padding = new Thickness(15, 15, 15, 20),
                Spacing = 10,
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
                                    Source = "info.png",
                                    WidthRequest = 20,
                                    Foreground = Color.FromHex("09818D"),
                                    VerticalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = "Información",
                                    TextColor = Color.FromHex("09818D"),
                                    FontSize = 18,
                                    FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null),
                                    VerticalOptions = LayoutOptions.Center
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
                                    Children=
                                    {
                                        new Grid
                                        {
                                            Children=
                                            {
                                                grid3,
                                            }
                                        }
                                    }
                                }
                            }
                        },
                    new StackLayout
                        {
                            Spacing = 5,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Children =
                            {
                                iUrl,
                                url,
                                new BoxView { BackgroundColor = Color.FromHex("09818D"), HeightRequest = 1 }
                            }
                        },

                                    }
            };

            guardar = new Button
            {
                Text = "VER ARCHIVO",
                FontSize = 18,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.FromHex("53A946"),
                WidthRequest = 128,
                HeightRequest = 38,
            };
            guardar.Clicked += Guardar_Clicked;


            gridBoton = new RelativeLayout
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

            StackLayout localizacion = new StackLayout
            {
                Padding = new Thickness(15, 15, 15, 20),
                Spacing = 15,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
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
                                    Source = "iAyuda.png",
                                    WidthRequest = 20,
                                    Foreground = Color.FromHex("03558A"),
                                    VerticalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = "API",
                                    TextColor = Color.FromHex("03558A"),
                                    VerticalOptions = LayoutOptions.Center,
                                    FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null),
                                    FontSize = 18
                                }                                
                            }
                        },
                    new Label
                    {
                        FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null),
                        FontSize = 11,
                        HorizontalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.FromHex("3F3F3F"),
                        Text = "Obten información de nuestra API"
                    },
                        gridBoton                        
                    }
            };

            this.BackgroundColor = Color.White;

            contenidoCreacionEdicion = new ScrollView
            {
                Padding = 0,
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
                                        datosGenerales,
                                        new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                                    }
                                },
                                new StackLayout
                                {
                                    Spacing = 0,
                                    Children =
                                    {
                                        localizacion,
                                        new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("B3B3B3"), HeightRequest=4},
                                    }
                                }
                            }
                }
            };


            contenido2 = new RelativeLayout();
            headerBackground = new Image()
            {
                Source = "headerConfiguracion.png",
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

            EdicionCreacion = new StackLayout
            {
                Padding = new Thickness(0, 15),
                Children = { contenidoCreacionEdicion }
            };


            contenido2.Children.Add(headerBackground,
                       xConstraint: Constraint.Constant(0),
                       yConstraint: Constraint.Constant(0),
                       widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                    heightConstraint: Constraint.Constant(120)
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

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new Indicador("Obteniendo información", Color.White));
            await Navigation.PushAsync(new APIDetallePage());
            await Task.Delay(3000);
            await Navigation.PopAllPopupAsync();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
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
    }
}
