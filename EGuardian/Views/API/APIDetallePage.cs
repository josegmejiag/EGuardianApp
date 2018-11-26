using System;
using Xamarin.Forms;

namespace EGuardian.Views.API
{
    public class APIDetallePage : ContentPage
    {
        public APIDetallePage()
        {
            Title = "API";
            Content = new WebView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Source = "http://68.183.17.17:8080/eguardian/swagger-ui.html#/"
            };
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
