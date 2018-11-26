using System.IO;
using EGuardian.Droid.Services;
using EGuardian.Interfaces;
using Xamarin.Forms;
using Android.Content;

[assembly: Dependency(typeof(ManejadorServicio))]
namespace EGuardian.Droid.Services
{
    public class ManejadorServicio : IService
    {
        Intent intent;
        public ManejadorServicio(){}
        public void Start()
        {
            intent = new Intent(Android.App.Application.Context, typeof(PeriodicBackgroundService));
            Android.App.Application.Context.StartService(intent);
        }

        public void Stop()
        {
            Android.App.Application.Context.StopService(intent);
        }
    }
}
