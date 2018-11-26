using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using EGuardian.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(ExtendedButtonRenderer))]
namespace EGuardian.Droid.Renderers
{
    public class ExtendedButtonRenderer : ButtonRenderer
    {
        public ExtendedButtonRenderer() : base()
        {
            SetWillNotDraw(false);
        }


        private GradientDrawable _normal,
                                        _pressed;


        // resolves: button text alignment lost after click or IsEnabled change
        //public override void ChildDrawableStateChanged(Android.Views.View child)
        //{
        //  base.ChildDrawableStateChanged(child);
        //  Control.Text = Control.Text; 
        //}

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
            {
                try
                {
                    var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".ttf");
                    Control.Typeface = font;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (Control != null)
            {
                SetAlignment();
                UpdatePadding();

                var density = Math.Max(1, Resources.DisplayMetrics.Density);
                var button = e.NewElement;
                var mode = MeasureSpec.GetMode((int)button.BorderRadius);
                var borderRadius = button.BorderRadius * density;
                var borderWidth = button.BorderWidth * density;

                // Create a drawable for the button's normal state
                _normal = new Android.Graphics.Drawables.GradientDrawable();

                if (button.BackgroundColor.R == -1.0 && button.BackgroundColor.G == -1.0 && button.BackgroundColor.B == -1.0)
                    _normal.SetColor(Android.Graphics.Color.ParseColor("#B2B2B2"));
                else
                    _normal.SetColor(button.BackgroundColor.ToAndroid());

                _normal.SetStroke((int)borderWidth, button.BorderColor.ToAndroid());
                _normal.SetCornerRadius(borderRadius);

                // Create a drawable for the button's pressed state
                _pressed = new Android.Graphics.Drawables.GradientDrawable();
                var highlight = Context.ObtainStyledAttributes(new int[]
                                    {
                                        Android.Resource.Attribute.ColorAccent  //  .ColorActivatedHighlight
                                    }).GetColor(0, Android.Graphics.Color.Gray);

                _pressed.SetColor(highlight);
                _pressed.SetStroke((int)borderWidth, button.BorderColor.ToAndroid());
                _pressed.SetCornerRadius(borderRadius);

                // Add the drawables to a state list and assign the state list to the button
                var sld = new StateListDrawable();
                sld.AddState(new int[] { Android.Resource.Attribute.StatePressed }, _pressed);
                sld.AddState(new int[] { }, _normal);
                //Control.SetBackground(sld);       //.SetBackgroundDrawable(sld); // deprecated
            }
        }
        private void UpdatePadding()
        {
            var element = this.Element as Button;
            if (element != null)
            {
                this.Control.SetPadding(
                    Control.PaddingLeft,
                    0,
                    Control.PaddingRight,
                    0
                );
            }
        }

        private void SetAlignment()
        {
            var element = this.Element as Button;

            if (element == null || this.Control == null)
            {
                return;
            }

            this.Control.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
            //element.VerticalAlignment.ToDroidVerticalGravity() |  
            //element.HorizontalAlignment.ToDroidHorizontalGravity();  
        }

        void DrawCustom(Button targetButton)
        {
            if (Control == null || targetButton == null)
                return;
        }
    }
}