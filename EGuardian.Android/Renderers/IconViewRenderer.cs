using System;
using System.ComponentModel;
using Android.Graphics;
using Android.Widget;
using EGuardian.Controls;
using EGuardian.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRendererAttribute(typeof(IconView), typeof(IconViewRenderer))]

namespace EGuardian.Droid.Renderers
{
    public class IconViewRenderer : ViewRenderer<IconView, ImageView>
    {
        private bool _isDisposed;

        public IconViewRenderer()
        {
            base.AutoPackage = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<IconView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                SetNativeControl(new ImageView(Context));
            }
            UpdateBitmap(e.OldElement);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconView.SourceProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
            else if (e.PropertyName == IconView.ForegroundProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
        }

        private void UpdateBitmap(IconView previous = null)
        {
            try
            {
                if (!_isDisposed)
                {
                    var d = Resources.GetDrawable(Element.Source).Mutate();
                    //d.SetColorFilter(new ColorFilter());
                    d.SetColorFilter(Element.Foreground.ToAndroid(), PorterDuff.Mode.SrcIn);
                    //d.SetColorFilter(new LightingColorFilter(Element.Foreground.ToAndroid(), Element.Foreground.ToAndroid()));
                    //d.Alpha = Element.Foreground.ToAndroid().A;
                    Control.SetImageDrawable(d);
                    ((IVisualElementController)Element).NativeSizeChanged();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }
    }
}