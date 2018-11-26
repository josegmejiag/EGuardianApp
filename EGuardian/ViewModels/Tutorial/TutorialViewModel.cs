using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace EGuardian.ViewModels.Tutorial
{
    public class TutorialViewModel : ContentView
    {
        public TutorialViewModel()
        {
            Label texto = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.White,
                FontSize = 18,
                FontFamily = Device.OnPlatform("OpenSans", "OpenSans-Regular", null)
            };

            Image image = new Image
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFill
            };

            texto.SetBinding(Label.TextProperty, "texto");
            image.SetBinding(Image.SourceProperty, "imagen");
            StackLayout Contenido = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {texto }
            };

            Contenido.SetBinding(StackLayout.PaddingProperty, "Padding");
            Content = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    image,
                    Contenido
                }
            };
        }
    }
}