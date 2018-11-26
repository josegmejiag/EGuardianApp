using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EGuardian.Common;
using EGuardian.Controls;
using EGuardian.Helpers;
using EGuardian.Models.Contrasenia;
using EGuardian.Models.Usuarios;
using EGuardian.Views.Ajustes;
using EGuardian.Views.Menu;
using Plugin.Toasts;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Ajustes
{
    public class ContraseniaDTViewModel : ContentView
    {    
        int control;
        Button guardar;
        ExtendedEntry contrasenia, contraseniaConfirmacion, contraseniaAnterior;
        StackLayout EdicionCreacion;
        ScrollView contenidoCreacionEdicion;

        public ContraseniaDTViewModel()
        {
            MessagingCenter.Subscribe<MainPage>(this, "DisplayAlert", (sender) =>
            {
                SetFocus();
                control = -1;
            });
            contrasenia = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                IsPassword = true,
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Margin = new Thickness(0, 0, 35, 0)
            };
            contrasenia.TextChanged += (sender, e) =>
            {
                if (!String.IsNullOrEmpty(contraseniaConfirmacion.Text) && !contraseniaConfirmacion.Text.Equals(contrasenia.Text))
                {
                    contraseniaConfirmacion.TextColor = Color.FromHex("E9242A");
                    contrasenia.TextColor = Color.FromHex("3F3F3F");
                }
                else
                {
                    contrasenia.TextColor = Color.FromHex("53A946");
                    contraseniaConfirmacion.TextColor = Color.FromHex("53A946");
                }
            };

            contraseniaConfirmacion = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                IsPassword = true,
                Placeholder = "Toca para confirmar contraseña",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Margin = new Thickness(0, 0, 35, 0)
            };
            contraseniaConfirmacion.TextChanged += (sender, e) =>
            {
                if (!contraseniaConfirmacion.Text.Equals(contrasenia.Text))
                {
                    contraseniaConfirmacion.TextColor = Color.FromHex("E9242A");
                    contrasenia.TextColor = Color.FromHex("3F3F3F");
                }
                else
                {
                    contrasenia.TextColor = Color.FromHex("53A946");
                    contraseniaConfirmacion.TextColor = Color.FromHex("53A946");
                }
            };

            IconView contraseniaView = new IconView
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Source = "iPassView.png",
                Foreground = Color.FromHex("B2B2B2"),
                WidthRequest = 25
            };
            IconView contraseniaConfirmacionView = new IconView
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Source = "iPassView.png",
                Foreground = Color.FromHex("B2B2B2"),
                WidthRequest = 25
            };

            TapGestureRecognizer contraseniaViewTAP = new TapGestureRecognizer();
            TapGestureRecognizer contraseniaConfirmacionViewTAP = new TapGestureRecognizer();
            contraseniaViewTAP.Tapped += (sender, e) =>
            {
                if (contrasenia.IsPassword)
                {
                    contrasenia.IsPassword = false;
                    contraseniaView.Foreground = Color.FromHex("007D8C");

                }
                else
                {
                    contrasenia.IsPassword = true;
                    contraseniaView.Foreground = Color.FromHex("B2B2B2");
                }
            };

            contraseniaConfirmacionViewTAP.Tapped += (sender, e) =>
            {
                if (contraseniaConfirmacion.IsPassword)
                {
                    contraseniaConfirmacion.IsPassword = false;
                    contraseniaConfirmacionView.Foreground = Color.FromHex("007D8C");
                }
                else
                {
                    contraseniaConfirmacion.IsPassword = true;
                    contraseniaConfirmacionView.Foreground = Color.FromHex("B2B2B2");
                }
            };

            contraseniaView.GestureRecognizers.Add(contraseniaViewTAP);
            contraseniaConfirmacionView.GestureRecognizers.Add(contraseniaConfirmacionViewTAP);

            contraseniaAnterior = new ExtendedEntry
            {
                TextColor = Color.FromHex("3F3F3F"),
                HasBorder = false,
                IsPassword = true,
                Placeholder = "Toca para ingresar",
                PlaceholderColor = Color.FromHex("B2B2B2"),
                XAlign = TextAlignment.End,
                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null),
                FontSize = 14,
                Margin = new Thickness(0, 0, 35, 0)
            };


            IconView contraseniaAnteriorView = new IconView
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Source = "iPassView.png",
                Foreground = Color.FromHex("B2B2B2"),
                WidthRequest = 25
            };


            TapGestureRecognizer contraseniaAnteriorViewTAP = new TapGestureRecognizer();

            contraseniaAnteriorViewTAP.Tapped += (sender, e) =>
            {
                if (contraseniaAnterior.IsPassword)
                {
                    contraseniaAnterior.IsPassword = false;
                    contraseniaAnteriorView.Foreground = Color.FromHex("007D8C");
                }
                else
                {
                    contraseniaAnterior.IsPassword = true;
                    contraseniaAnteriorView.Foreground = Color.FromHex("B2B2B2");
                }
            };

            contraseniaAnteriorView.GestureRecognizers.Add(contraseniaAnteriorViewTAP);


            StackLayout datosGenerales = new StackLayout
            {
                Padding = new Thickness(15, 15, 15, 20),
                Spacing = 15,
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
                                    Source = "iSeguridad.png",
                                    WidthRequest = 15,
                                    HeightRequest = 15,
                                    Foreground = Color.FromHex("007D8C"),
                                    VerticalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = "Ingresa y confirma\r\ntu nueva contraseña",
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    TextColor = Color.FromHex("007D8C"),
                                    FontFamily = Device.OnPlatform("OpenSans-ExtraBold", "OpenSans-ExtraBold", null),
                                    FontSize = 18,
                                    VerticalOptions = LayoutOptions.Center
                                }
                            }
                        },
                    new StackLayout
                        {
                            Spacing = 0,
                            Children =
                            {
                                new Label
                                {
                                    Text ="CONTRASEÑA ANTERIOR:*",
                                    FontSize = 13,
                                    TextColor = Color.FromHex("007D8C"),
                                    FontAttributes = FontAttributes.Bold,
                                    FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                                },
                                new StackLayout
                                {
                                    Spacing=1,
                                    Children=
                                    {
                                        new Grid
                                            {
                                                Children=
                                                {
                                                    contraseniaAnterior,
                                                    contraseniaAnteriorView
                                                }
                                            },
                                        new BoxView {BackgroundColor= Color.FromHex("007D8C"), HeightRequest=2 },
                                        new BoxView {HeightRequest=0 }
                                    }
                                }
                            }
                        },
                        new StackLayout
                    {
                        Spacing = 0,
                        Children =
                        {
                            new Label
                            {
                                Text ="NUEVA CONTRASEÑA:*",
                                FontSize = 13,
                                TextColor = Color.FromHex("007D8C"),
                                FontAttributes = FontAttributes.Bold,
                                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                            },
                            new StackLayout
                            {
                                Spacing=1,
                                Children=
                                {
                                    new Grid
                                    {
                                        Children=
                                        {
                                            contrasenia,
                                            contraseniaView
                                        }
                                    },
                                    new BoxView {BackgroundColor= Color.FromHex("007D8C"), HeightRequest=2 },
                                    new BoxView {HeightRequest=0 }
                                }
                            }
                        }
                    },
                    new StackLayout
                    {
                        Spacing = 0,
                        Children =
                        {
                            new Label
                            {
                                Text ="CONFIRMAR CONTRASEÑA:*",
                                FontSize = 13,
                                TextColor = Color.FromHex("007D8C"),
                                FontAttributes = FontAttributes.Bold,
                                FontFamily = Device.OnPlatform("OpenSans-Bold", "OpenSans-Bold", null)
                            },
                            new StackLayout
                            {
                                Spacing=1,
                                Children=
                                {
                                    new Grid
                                    {
                                        Children=
                                        {
                                            contraseniaConfirmacion,
                                            contraseniaConfirmacionView
                                        }
                                    },
                                    new BoxView {BackgroundColor= Color.FromHex("007D8C"), HeightRequest=2 },
                                    new BoxView {HeightRequest=0 }
                                }
                            }
                        }
                    }
                        }
            };

            guardar = new Button
            {
                Text = "CONFIRMAR",
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
            gridBoton.Children.Add(guardar, Constraint.Constant(0), Constraint.Constant(0));




            this.BackgroundColor = Color.White;

            contenidoCreacionEdicion = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
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
                        }
                    }
                }
            };


            EdicionCreacion = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 15),
                Children = { contenidoCreacionEdicion, gridBoton }
            };
            Content = EdicionCreacion;
        }

        private async void ShowToast(ToastNotificationType type, string titulo, string descripcion, int tiempo)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
            bool tapped = await notificator.Notify(type, titulo, descripcion, TimeSpan.FromSeconds(tiempo));
        }

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(contraseniaAnterior.Text))
            {
                DisplayAlert("", "Por favor, indique su contraseña anterior");
                control = 0;
                return;
            }
            if (String.IsNullOrEmpty(contrasenia.Text))
            {
                DisplayAlert("", "Por favor, indique su nueva contraseña");
                control = 1;
                return;
            }
            else
            {
                if (!Regex.Match(contrasenia.Text, @"^\S{6,15}$").Success)
                {
                    DisplayAlert("", "La contraseña debe tener entre 6 y 15 caracteres.");
                    control = 1;
                    return;
                }
            }
            if (String.IsNullOrEmpty(contraseniaConfirmacion.Text))
            {
                DisplayAlert("", "Por favor, confirme su nueva contraseña.");
                control = 2;
                return;
            }

            if ((!String.IsNullOrEmpty(contrasenia.Text) && !String.IsNullOrEmpty(contraseniaConfirmacion.Text)) && !contraseniaConfirmacion.Text.Equals(contrasenia.Text))
            {
                DisplayAlert("", "Las contraseñas no coinciden");
                control = 2;
                return;
            }
            await Navigation.PushPopupAsync(new Indicador("Actualizando contraseña", Color.White));
             try
             {
                ChangePassword peticion = new ChangePassword
                {
                    oldPassword = contraseniaAnterior.Text.Trim(),
                    newPassword = contrasenia.Text.Trim()
                 };

                var Respuesta = await App.ManejadorDatos.ChangePasswordAsync(peticion);
                await Navigation.PopPopupAsync();
                 
                 if (Respuesta)
                 {
                    MessagingCenter.Send<ContraseniaDTViewModel>(this, "noAutenticado");
                    ShowToast(ToastNotificationType.Success, "Cambio de contraseña", "Contraseña actualizada con éxito, cerrando sesión por seguridad", 5);
                }
                 else
                    ShowToast(ToastNotificationType.Error, "Cambio de contraseña", "Ha ocurrido algo inesperado en la actualización de contraseña, inténtalo de nuevo.", 5);
            }
             catch
             {
                ShowToast(ToastNotificationType.Error, "Cambio de contraseña", "Ha ocurrido algo inesperado en la actualización de contraseña, inténtalo de nuevo.", 5);
            }
            await Navigation.PopAllPopupAsync();
        }

        public void DisplayAlert(string title, string message)
        {
            string[] values = { title, message };
            MessagingCenter.Send<ContraseniaDTViewModel, string[]>(this, "DisplayAlert", values);
        }

        public void SetFocus()
        {
            switch (control)
            {
                case 0:
                    contraseniaAnterior.Focus();
                    break;
                case 1:
                    contrasenia.Focus();
                    break;
                case 2:
                    contraseniaConfirmacion.Focus();
                    break;                
            }
        }
    }
}