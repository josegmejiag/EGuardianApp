using System;

using Android;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using CarouselView.FormsPlugin.Android;
using Plugin.Toasts;
using Xamarin.Forms;
using RoundedBoxView.Forms.Plugin.Droid;
using Refractored.XamForms.PullToRefresh.Droid;
using ImageCircle.Forms.Plugin.Droid;
using EGuardian.Droid.Services;

namespace EGuardian.Droid
{    
    //[Activity(Label = "EGuardian", Icon = "@mipmap/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    [Activity(Label = "EGuardian")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
			Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            App.DisplayScreenWidth = (double)Resources.DisplayMetrics.WidthPixels / (double)Resources.DisplayMetrics.Density;
            App.DisplayScreenHeight = (double)Resources.DisplayMetrics.HeightPixels / (double)Resources.DisplayMetrics.Density;
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();
            RoundedBoxViewRenderer.Init();
            ImageCircleRenderer.Init();
            PullToRefreshLayoutRenderer.Init();
            DependencyService.Register<ToastNotificatorImplementation>();
            ToastNotificatorImplementation.Init(this);
            LoadApplication(new App());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            App.CloseService();
        }
    }
}