using System;
using System.ComponentModel;
using Android.Graphics;
using Android.Views;
using EGuardian.Controls;
using EGuardian.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace EGuardian.Droid.Renderers
{
    public class ExtendedPickerRenderer : PickerRenderer
    {
        public ExtendedPickerRenderer() : base()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            var view = e.NewElement as ExtendedPicker;

            if (view != null)
            {
                SetTextAlignment(view);
                SetFont(view);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var view = (ExtendedPicker)Element;
            if (e.PropertyName == ExtendedPicker.XAlignProperty.PropertyName)
            {
                SetTextAlignment(view);
                SetFont(view);
            }
        }

        private void SetTextAlignment(ExtendedPicker view)
        {
            switch (view.XAlign)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = GravityFlags.CenterVertical;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = GravityFlags.End;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = GravityFlags.Start;
                    break;
            }
        }

        private void SetFont(ExtendedPicker view)
        {
            if (view.Font != Font.Default)
            {
                Control.TextSize = (float)view.Font.FontSize;
                var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, view.Font.FontFamily + ".ttf");
                Control.Typeface = font;
            }
        }
    }
}