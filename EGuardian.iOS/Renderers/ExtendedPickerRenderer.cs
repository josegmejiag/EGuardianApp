using System.ComponentModel;
using EGuardian.Controls;
using EGuardian.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace EGuardian.iOS.Renderers
{
	public class ExtendedPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            var view = e.NewElement as ExtendedPicker;

            if (view != null)
            {
                SetBorder(view);
                SetFont(view);
                SetTextAlignment(view);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var view = (ExtendedPicker)Element;
            if (e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
                SetFont(view);
            if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
            if (e.PropertyName == ExtendedPicker.HasBorderProperty.PropertyName)
                SetBorder(view);
        }


        private void SetBorder(ExtendedPicker view)
        {
            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
        }

        private void SetTextAlignment(ExtendedPicker view)
        {
            switch (view.XAlign)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }


        private void SetFont(ExtendedPicker view)
        {
            UIFont uiFont;
            if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
                Control.Font = uiFont;
            else if (view.Font == Font.Default)
                Control.Font = UIFont.SystemFontOfSize(14f);
        }
    }
}

