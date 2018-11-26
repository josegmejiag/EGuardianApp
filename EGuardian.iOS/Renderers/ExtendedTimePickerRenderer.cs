using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using EGuardian.Controls;
using EGuardian.iOS.Renderers;

[assembly: ExportRenderer(typeof(ExtendedTimePicker), typeof(ExtendedTimePickerRenderer))]
namespace EGuardian.iOS.Renderers
{
    public class ExtendedTimePickerRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            var view = e.NewElement as ExtendedTimePicker;

            if (view != null)
            {
                SetBorder(view);
                SetFont(view);

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var view = (ExtendedTimePicker)Element;
            if (e.PropertyName == ExtendedTimePicker.HasBorderProperty.PropertyName)
                SetBorder(view);
            if (e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
                SetFont(view);
        }


        private void SetBorder(ExtendedTimePicker view)
        {
            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
        }

        private void SetFont(ExtendedTimePicker view)
        {
            UIFont uiFont;
            if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
                Control.Font = uiFont;
            else if (view.Font == Font.Default)
                Control.Font = UIFont.SystemFontOfSize(14f);
        }
    }
}

