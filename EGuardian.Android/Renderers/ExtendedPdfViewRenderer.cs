using EGuardian.Controls;
using EGuardian.Droid.Renderers;
using System;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedPdfView), typeof(ExtendedPdfViewRenderer))]
namespace EGuardian.Droid.Renderers
{
    public class ExtendedPdfViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var customWebView = Element as ExtendedPdfView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///android_asset/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
            }
        }
    }
}