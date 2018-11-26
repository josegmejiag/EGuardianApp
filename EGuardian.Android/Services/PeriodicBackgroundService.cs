using System;
using System.Threading.Tasks;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Util;
using System.Collections.Generic;
using Android.App.Usage;
using Android.Icu.Util;
using Android.Widget;
using System.Linq;
using System.Timers;
using Android.Media;
using Android.Support.V4.App;
using Xamarin.Forms;
using EGuardian.Data;

namespace EGuardian.Droid.Services
{
    [Service]
    class PeriodicBackgroundService : Service
    {
        UsageStatsManager mUsageStatsManager;
        private const string Tag = "[PeriodicBackgroundService]";
        const int NOTIFICATION_ID = 9000;
        private bool _isRunning;
        private Context _context;
        private Task _task;

        #region overrides

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            _context = this;
            _isRunning = false;
            _task = new Task(DoWork);
            mUsageStatsManager = (UsageStatsManager)this
                .GetSystemService("usagestats"); //Context.USAGE_STATS_SERVICE
        }

        public override void OnDestroy()
        {
            _isRunning = false;

            if (_task != null && _task.Status == TaskStatus.RanToCompletion)
            {
                _task.Dispose();
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            if (!_isRunning)
            {
                _isRunning = true;
                _task.Start();
            }

            Notification.Builder notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Mipmap.icon)
            .SetContentTitle(Resources.GetString(Resource.String.notification_content_title))
            .SetContentText(Resources.GetString(Resource.String.notification_content_text));

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(NOTIFICATION_ID, notificationBuilder.Build());

            return StartCommandResult.Sticky;
        }

        #endregion


        public IDictionary<string, UsageStats> GetUsageStatistics()
        {
            Log.Info("TAG", "Leyendo estadisticas");
            IDictionary<string, UsageStats> queryUsageStats = null;
            try
            {
                var cal = Calendar.GetInstance(Java.Util.Locale.Default);
                cal.Add(CalendarField.Year, -1);

                // Query stats beginning one year ago to the current date.
                queryUsageStats = mUsageStatsManager.QueryAndAggregateUsageStats(cal.TimeInMillis,
                                          Java.Lang.JavaSystem.CurrentTimeMillis());

                if (queryUsageStats.Count == 0)
                {
                    Log.Info("TAG", "The user may not allow the access to apps usage. ");
                    Toast.MakeText(this,
                        GetString("Dar permisos para lectura de estadisticas de uso"),
                        ToastLength.Long).Show();
                        //StartActivity(new Intent(Settings.ActionUsageAccessSettings));
                }
            }
            catch(Exception ex)
            {
                Log.Info("TAG2", ex.Message+" "+ex.StackTrace);
            }
            return queryUsageStats;
        }

        private string GetString(object explanation_access_to_appusage_is_not_enabled)
        {
            throw new NotImplementedException();
        }

        private void DoWork()
        {

            /*var Handler = new Handler(Looper.MainLooper);
            Handler.Post(() => { MessagingCenter.Send("From Service", "hello"); });*/


            Notification.Builder notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Mipmap.icon)
            .SetContentTitle(Resources.GetString(Resource.String.notification_content_title))
            .SetContentText(Resources.GetString(Resource.String.notification_content_text));

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(NOTIFICATION_ID, notificationBuilder.Build());

            try
            {
                Log.WriteLine(LogPriority.Info, Tag, "---------------------");
                Log.WriteLine(LogPriority.Info, Tag, "SERVICIO REINICIADO");

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 10000;
                timer.Elapsed += OnTimedEvent;
                timer.Enabled = true;
            }
            catch (Exception e)
            {
                Log.WriteLine(LogPriority.Error, Tag, e.ToString());
            }
            finally
            {
                StopSelf();
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Log.WriteLine(LogPriority.Info, Tag, "En el servicio! DoWork");
            IDictionary<string, UsageStats> usageStatsList = GetUsageStatistics();

            foreach (KeyValuePair<string, UsageStats> kvp in usageStatsList)
            {
                if(kvp.Key.Equals("com.facebook.katana")
                  || kvp.Key.Equals("com.android.chrome")
                  || kvp.Key.Equals("com.twitter.android")
                  || kvp.Key.Equals("com.whatsapp")
                  || kvp.Key.Equals("com.google.android.youtube"))
                {
                    var estadistica = ((UsageStats)kvp.Value);
                    var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .AddMilliseconds((long)estadistica.LastTimeUsed)
                     .ToLocalTime();

                    if (time >= EGuardian.Data.Constants.FechaInicio && time < EGuardian.Data.Constants.FechaFinal)
                    {
                        Log.WriteLine(LogPriority.Info, Tag, "CACHADO USANDO EL APP:" + kvp.Key + time.ToString("g"));


                        string package = "1";
                        switch (kvp.Key)
                        {
                            case "com.facebook.katana":
                                package = "1";
                                break;
                            case "com.android.chrome":
                                package = "2";
                                break;
                            case "com.twitter.android":
                                package = "3";
                                break;
                            case "com.whatsapp":
                                package = "4";
                                break;
                            case "com.google.android.youtube":
                                package = "5";
                                break;
                        }

                        Constants.idAplicacionIncidencia = Convert.ToInt32(package);
                        Constants.fechaInicioIncidencia = time.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                        var Handler = new Handler(Looper.MainLooper);
                        Handler.Post(() => { MessagingCenter.Send("From Service", "hello"); });






                        Vibrator vibrator = (Vibrator)this.GetSystemService(Context.VibratorService);
                        vibrator.Vibrate(600);

                        try
                        {
                            MediaPlayer _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.ding_persevy);
                            _mediaPlayer.Start();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }



                        Intent intent = new Intent(this, typeof(MainActivity));
                        intent.AddFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                    }
                }
            }
        }
    }
}

