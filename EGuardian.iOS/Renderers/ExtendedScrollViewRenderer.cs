using System;
using System.ComponentModel;
using EGuardian.Controls;
using EGuardian.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedScrollView), typeof(ExtendedScrollViewRenderer))]
namespace EGuardian.iOS.Renderers
{
	public class ExtendedScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            if (e.NewElement != null)
            {
                var view = e.NewElement as ExtendedScrollView;
                if (view != null)
                {
                    SetScrollBarStatus(view);
                    SetScrollStatus(view);

                }
                e.NewElement.PropertyChanged += OnElementPropertyChanged;
            }
        }

        private void SetScrollStatus(ExtendedScrollView view)
        {
            try
            {
                this.ScrollEnabled = view.ScrollingEnabled;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void SetScrollBarStatus(ExtendedScrollView view)
        {
            try
            {
                this.ShowsVerticalScrollIndicator = view.ScrollBarEnabled;
                this.ShowsHorizontalScrollIndicator = view.ScrollBarEnabled;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            /*var view = (ExtendedScrollView)Element;
            if (e.PropertyName == ExtendedScrollView.ScrollingEnabledProperty.PropertyName)
                SetScrollStatus(view);
            if (e.PropertyName == ExtendedScrollView.ScrollBarEnabledProperty.PropertyName)
                SetScrollBarStatus(view);
                */
        }
    }
}
