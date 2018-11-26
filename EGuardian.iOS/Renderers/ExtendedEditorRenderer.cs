using System;
using System.ComponentModel;
using EGuardian.Controls;
using EGuardian.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]
namespace EGuardian.iOS.Renderers
{
	public class ExtendedEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            var view = e.NewElement as ExtendedEditor;
            if (Control != null)
            {
                Control.ScrollEnabled = false;
                //SetFont(view);
                SetTextAlignment(view);

            }
        }
        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetTextAlignment(ExtendedEditor view)
        {
            try
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
            catch (Exception ex)
            {
                Control.TextAlignment = UITextAlignment.Left;
                System.Diagnostics.Debug.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetFont(ExtendedEditor view)
        {
            UIFont uiFont;
            if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
                Control.Font = uiFont;
            else if (view.Font == Font.Default)
                Control.Font = UIFont.SystemFontOfSize(12f);
        }
    }
}
