using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;

namespace XamarinGeofenceProject.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        public static LocationService LocationService = null;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var center = UNUserNotificationCenter.Current;

            center.Delegate = new ForegroundNotificationDelegate();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

           
            center.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (result, error) =>
            {
                if(error != null)
                {

                }
            });

            LocationService = new LocationService();
            LocationService.RequestAlwaysLocation();
            return base.FinishedLaunching(app, options);
        }
    }

    public class ForegroundNotificationDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge | UNNotificationPresentationOptions.Sound);
        }
    }
}
