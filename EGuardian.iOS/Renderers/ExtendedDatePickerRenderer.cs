using System;
using System.ComponentModel;
using EGuardian.Controls;
using EGuardian.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]
namespace EGuardian.iOS.Renderers
{
	public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            var view = e.NewElement as ExtendedDatePicker;

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
            var view = (ExtendedDatePicker)Element;
            if (e.PropertyName == ExtendedDatePicker.HasBorderProperty.PropertyName)
                SetBorder(view);
            if (e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
                SetFont(view);
            if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
        }


        private void SetBorder(ExtendedDatePicker view)
        {
            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
        }

        private void SetTextAlignment(ExtendedDatePicker view)
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
        private void SetFont(ExtendedDatePicker view)
        {
            UIFont uiFont;
            if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
                Control.Font = uiFont;
            else if (view.Font == Font.Default)
                Control.Font = UIFont.SystemFontOfSize(14f);
        }
    }
}

