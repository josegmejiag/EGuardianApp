using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarouselView.FormsPlugin.iOS;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.Toasts;
using Refractored.XamForms.PullToRefresh.iOS;
using RoundedBoxView.Forms.Plugin.iOSUnified;
using SuaveControls.FloatingActionButton.iOS.Renderers;
using UIKit;
using Xamarin.Forms;

namespace EGuardian.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Calabash.Start();
			Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
			App.DisplayScreenWidth = (double)UIScreen.MainScreen.Bounds.Width;
            App.DisplayScreenHeight = (double)UIScreen.MainScreen.Bounds.Height;
            App.DisplayScaleFactor = (double)UIScreen.MainScreen.Scale;
            DependencyService.Register<ToastNotificatorImplementation>();
            ToastNotificatorImplementation.Init();
            FloatingActionButtonRenderer.InitRenderer();
            PullToRefreshLayoutRenderer.Init();
            ImageCircleRenderer.Init();
            CarouselViewRenderer.Init();
            RoundedBoxViewRenderer.Init();
            LoadApplication(new App());
            //imprimirFuentes();
            return base.FinishedLaunching(app, options);
        }

        void imprimirFuentes()
        {
            var fontList = new StringBuilder();
            var familyNames = UIFont.FamilyNames;
            foreach (var familyName in familyNames)
            {
                fontList.Append(String.Format("Family: {0}\n", familyName));
                Console.WriteLine("Family: {0}\n", familyName);
                var fontNames = UIFont.FontNamesForFamilyName(familyName);
                foreach (var fontName in fontNames)
                {
                    Console.WriteLine("\tFont: {0}\n", fontName);
                    fontList.Append(String.Format("\tFont: {0}\n", fontName));
                }
            };
        }
    }
}
