using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Gms.Location;
using System.Collections.Generic;
using Android.Content;
using AndroidX.Core.App;
using Android.Gms.Extensions;
using Xamarin.Essentials;
using static Android.Icu.Text.Transliterator;
using Xamarin.Forms;

namespace XamarinGeofenceProject.Droid
{
    [Activity(Label = "XamarinGeofenceProject", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            List<Point> list = GetGeofences();
            Location location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High));
            LocationService locationService = new LocationService();
            locationService.SetGeofences(list, new Point(location.Longitude, location.Latitude));
         
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private List<Point> GetGeofences()
        {
            return new List<Point>
            {
                new Point(-6.139933, 36.681062), // Alcazar, Jerez de la Frontera
				new Point(-3.703512, 40.416905), // Puerta del Sol, Madrid
                new Point(126.8862158, 37.5191507),
				new Point(0, 0),
            };
        }
    }
}