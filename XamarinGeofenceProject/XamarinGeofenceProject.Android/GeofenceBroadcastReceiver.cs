using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;
using XamarinGeofenceProject.Services;

namespace XamarinGeofenceProject.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = true)]

    public class GeofenceBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            GeofencingEvent geofencingEvent = GeofencingEvent.FromIntent(intent);
            if (geofencingEvent.HasError)
            {
                string error = GeofenceStatusCodes.GetStatusCodeString(geofencingEvent.ErrorCode);
                Console.WriteLine($"Error Code: {geofencingEvent.ErrorCode}. Error: {error}");
                return;
            }

            // Get the transition type.
            int geofenceTransition = geofencingEvent.GeofenceTransition;
            // Get the geofences that were triggered. A single event can trigger
            // multiple geofences.
            IList<IGeofence> triggeringGeofences = geofencingEvent.TriggeringGeofences;
            NotificationsService notificationsService = new NotificationsService(context);

            switch (geofenceTransition)
            {
                case Geofence.GeofenceTransitionEnter:
                    notificationsService.SendNotification("들어옴", "들어왔어어어어어어!", 10000);
                    break;
                case Geofence.GeofenceTransitionExit:
                    notificationsService.SendNotification("나감", "나갓어어어어어ㅓ어", 10000);
                    break;
                case Geofence.GeofenceTransitionDwell:
                    notificationsService.SendNotification("머뭄", "머물러러러ㅓㄹ러ㅓ", 10000);
                    break;
                default:
                    // Log the error.
                    Console.WriteLine("Broadcast not implemented.");
                    break;
            }
        }
    }
}