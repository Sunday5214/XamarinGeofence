using System;
using Foundation;
using CoreLocation;
using System.Diagnostics;
using UserNotifications;

namespace XamarinGeofenceProject.iOS
{
	public class LocationService
	{
		private CLLocationManager LocationManager = new CLLocationManager();

		public LocationService()
		{
            LocationManager.DidStartMonitoringForRegion += LocationManager_DidStartMonitoringForRegion;
            LocationManager.DidDetermineState += LocationManager_DidDetermineState;
		}

        private void LocationManager_DidDetermineState(object sender, CLRegionStateDeterminedEventArgs e)
        {
            switch(e.State)
			{
				case CLRegionState.Inside:
					var InContent = new UNMutableNotificationContent();
                    InContent.Title = "들어옴";
                    InContent.Body = "들어어어어어ㅓ어옴";

				    Guid guid = Guid.NewGuid();
					var inTrigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
					var inRequest = UNNotificationRequest.FromIdentifier($"Test-{guid}", InContent, inTrigger);
					UNUserNotificationCenter.Current.AddNotificationRequest(
						inRequest, (error) =>
						{

						}
					);
					break;
				case CLRegionState.Outside:
                    var outContent = new UNMutableNotificationContent();
                    outContent.Title = "나감";
                    outContent.Body = "나아아아아ㅏ감";

                    Guid outGuid = Guid.NewGuid();
                    var outTrigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
                    var outRequest = UNNotificationRequest.FromIdentifier($"Test-{outGuid}", outContent, outTrigger);
                    UNUserNotificationCenter.Current.AddNotificationRequest(
                        outRequest, (error) =>
                        {

                        }
                    );
                    break;
				default:
					Debug.WriteLine("Holy..");
					break;
			}
        }

        private void LocationManager_DidStartMonitoringForRegion(object sender, CLRegionEventArgs e)
        {
			Debug.WriteLine("didStartMonitoringFor");
        }

        public void RegistLocation()
		{
			var location = new CLLocationCoordinate2D(37.5191507, 126.8862158);
			var region = new CLCircularRegion(location, 1.0, "test");

			region.NotifyOnEntry = true;
			region.NotifyOnExit = true;

			LocationManager.AllowsBackgroundLocationUpdates = true;
			LocationManager.PausesLocationUpdatesAutomatically = false;

			LocationManager.StartUpdatingLocation();
			LocationManager.StartMonitoring(region);

			Debug.WriteLine($"region regist: {region}");
		}

		public void RequestAlwaysLocation()
		{
			switch (LocationManager.AuthorizationStatus)
			{
				case CLAuthorizationStatus.NotDetermined:
					LocationManager.RequestAlwaysAuthorization();
					break;
				case CLAuthorizationStatus.AuthorizedWhenInUse:
					LocationManager.RequestAlwaysAuthorization();
					break;
				case CLAuthorizationStatus.AuthorizedAlways:
					RegistLocation();
					break;
				default:
					Debug.WriteLine("Location is not available");
					break;
			}
		}
	}
}

