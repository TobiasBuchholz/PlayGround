using System;
using System.Diagnostics;
using System.Globalization;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Genesis.Logging;
using Plugin.CurrentActivity;
using Splat;

namespace PlayGround.UI.Droid
{
	[Application]
    public class PGApplication : Application, Application.IActivityLifecycleCallbacks
	{
		public PGApplication(IntPtr handle, JniHandleOwnership transfer)
			: base(handle,transfer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);

            ConfigureAmbientLoggerService();
            DirectLoggingOutputToConsole();

			new SplatRegistrar().Register(Locator.CurrentMutable, new AndroidCompositionRoute());
		}

        [Conditional("LOGGING")]
        private static void ConfigureAmbientLoggerService() 
        {
            LoggerService.Current = new DefaultLoggerService();
        }

        [Conditional("LOGGING")]
        private void DirectLoggingOutputToConsole() 
        {
            LoggerService
                .Current
                .Entries
                .SubscribeSafe(
                    entry =>
            {
                FormattableString message = $"#{entry.ThreadId} {entry.Message}";
                Log.WriteLine(ToLogPriority(entry.Level), GetLastCharacters(entry.Name, 64), message.ToString(CultureInfo.InvariantCulture));
            }); 
        }

        private static LogPriority ToLogPriority(Genesis.Logging.LogLevel level)
        {
            switch(level) {
                case Genesis.Logging.LogLevel.Debug:
                    return LogPriority.Debug;
                case Genesis.Logging.LogLevel.Info:
                case Genesis.Logging.LogLevel.Perf:
                    return LogPriority.Info;
                case Genesis.Logging.LogLevel.Warn:
                    return LogPriority.Warn;
                case Genesis.Logging.LogLevel.Error:
                    return LogPriority.Error;
                default:
                    return LogPriority.Verbose;
            }
        }

        private static string GetLastCharacters(string s, int characters)
        {
            if(s.Length <= characters) {
                return s;
            }
            return s.Substring(s.Length - characters - 1);
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}
