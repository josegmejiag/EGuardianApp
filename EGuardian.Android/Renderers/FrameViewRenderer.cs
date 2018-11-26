using System;
using Android.Content;
using Android.Graphics.Drawables;
using EGuardian.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Frame), typeof(FrameViewRenderer))]

namespace EGuardian.Droid.Renderers
{
    public class FrameViewRenderer : FrameRenderer
    {
        public FrameViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                this.Elevation = 16f;
                //ViewGroup.SetBackground(Color.White.ToAndroid());
                //aqui//ViewGroup.SetBackgroundResource(Resource.Drawable.shadow);


                //ViewGroup.Elevation = 15;
                //ViewGroup.TranslationZ = 1;
                //ViewGroup.OffsetLeftAndRight(0);
                //ViewGroup.OffsetTopAndBottom(-10);
                //ViewGroup.opac

                //ViewGroup.Elevation = 8.0f;
                //ViewGroup.TranslationZ = 10.0f;


            }
        }
    }
}