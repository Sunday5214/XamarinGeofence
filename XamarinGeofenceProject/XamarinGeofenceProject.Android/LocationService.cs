using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using static Android.Icu.Text.Transliterator;

namespace XamarinGeofenceProject.Droid
{
    public class LocationService
    {
        private GeofencingClient geofencingClient;
        private PendingIntent geofencePendingIntent;
        private int numberOfGeofences = 10;

        public LocationService()
        {
            geofencingClient = LocationServices.GetGeofencingClient(Xamarin.Essentials.Platform.AppContext);
        }

        public void SetGeofences(List<Point> targets, Point currentPosition)
        {
            List<Point> nearTargets = GetCloseTargets(targets, currentPosition);
            List<IGeofence> geofences = new List<IGeofence>();
            foreach (Point item in nearTargets)
            {
                IGeofence geofence = new GeofenceBuilder()
                    .SetRequestId($"{item.Y}_{item.X}")
                    .SetCircularRegion(item.Y, item.X, 1000)
                    .SetLoiteringDelay(60 * 1000)
                    .SetTransitionTypes(Geofence.GeofenceTransitionEnter | Geofence.GeofenceTransitionExit | Geofence.GeofenceTransitionDwell)
                    .SetExpirationDuration(Geofence.NeverExpire)
                    .Build();
                geofences.Add(geofence);
            }
            GeofencingRequest geofencingRequest = new GeofencingRequest.Builder()
                .AddGeofences(geofences)
                .Build();

            geofencingClient.AddGeofences(geofencingRequest, GetGeofencePendingIntent());
        }


        private PendingIntent GetGeofencePendingIntent()
        {
            if (geofencePendingIntent != null)
                return geofencePendingIntent;

            Intent intent = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(GeofenceBroadcastReceiver));
            // We use FLAG_UPDATE_CURRENT so that we get the same pending intent back when calling addGeofences() and removeGeofences().
            geofencePendingIntent = PendingIntent.GetBroadcast(Xamarin.Essentials.Platform.AppContext, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable);
            return geofencePendingIntent;
        }

        private List<Point> GetCloseTargets(List<Point> targets, Point position)
        {
            List<KeyValuePair<double, Point>> distanceAndTargetList = new List<KeyValuePair<double, Point>>();
            Location currentLocation = new Location(string.Empty)
            {
                Longitude = position.X,
                Latitude = position.Y
            };

            foreach (Point item in targets)
            {
                Location location = new Location(string.Empty)
                {
                    Latitude = item.Y,
                    Longitude = item.X
                };

                distanceAndTargetList.Add(new KeyValuePair<double, Point>(currentLocation.DistanceTo(location), item));
            }

            IEnumerable<Point> items = distanceAndTargetList.OrderBy(x => x.Key).Select(x => x.Value);

            return items.Count() > numberOfGeofences ? items.Take(numberOfGeofences).ToList() : items.ToList();
        }
    }
}