using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Controls
{
    public class ExtendedPdfView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(nameof(Uri), typeof(string), typeof(ExtendedPdfView));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
    }
}