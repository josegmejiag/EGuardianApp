using System;
using Android.Content;
using Android.Graphics;
using EGuardian.Controls;
using EGuardian.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace EGuardian.Droid.Renderers
{
    public class ExtendedLabelRenderer : LabelRenderer
    {
        PaintFlags PaintFlagsOriginal;

        public ExtendedLabelRenderer(Context context) : base(context)
        {
            try
            {
                PaintFlagsOriginal = Control.PaintFlags;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            try
            {
                var view = (ExtendedLabel)Element;
                UpdateUi(view);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void UpdateUi(ExtendedLabel extendedElement)
        {
            if (extendedElement.IsStrikeThrough)
            {
                Control.PaintFlags = Control.PaintFlags | PaintFlags.StrikeThruText;
            }
            else
            {
                Control.PaintFlags = PaintFlagsOriginal;
            }
        }
    }
}