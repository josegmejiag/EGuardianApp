using System;
using CoreGraphics;
using EGuardian.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRendererAttribute(typeof(Frame), typeof(FrameViewRenderer))]
namespace EGuardian.iOS.Renderers
{
	public class FrameViewRenderer : FrameRenderer
    {
        public FrameViewRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            Layer.BorderColor = UIColor.White.CGColor;
            Layer.CornerRadius = 0;
            Layer.MasksToBounds = false;
            Layer.ShadowOffset = new CGSize(0, 3);
            Layer.ShadowRadius = 6;
            Layer.ShadowColor = Color.FromHex("000000").ToCGColor();
            Layer.ShadowOpacity = 0.16f;
        }
    }
}
