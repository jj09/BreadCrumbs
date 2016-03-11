using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BreadCrumbs.Shared.ViewModels;
using BreadCrumbs.Shared.Models;
using GalaSoft.MvvmLight.Helpers;
using Plugin.Geolocator;
using System.Linq;
using Android.Locations;
using System.Collections.Generic;

namespace BreadCrumbs.Droid
{
    [Activity(Label = "BreadCrumbs.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
		public MainViewModel ViewModel { get; set; }

		private ListView _placesListView;

		private LayoutInflater _inflater;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			_inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);

			ViewModel = new MainViewModel();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            EditText placeNameEditText = FindViewById<EditText>(Resource.Id.PlaceNameEditText);
			Button savePlaceButton = FindViewById<Button>(Resource.Id.SavePlaceButton);
			_placesListView = FindViewById<ListView>(Resource.Id.PlacesListView);
			_placesListView.Adapter = ViewModel.SavedPlaces.GetAdapter(GetItemView);

			savePlaceButton.Click += delegate {
				ViewModel.SaveAsync(placeNameEditText.Text);

                placeNameEditText.Text = "";
            };

			_placesListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e) {
				var coordinates = this.ViewModel.SavedPlaces.ElementAt(e.Position).Coordinates;
				var gmmIntentUri = Android.Net.Uri.Parse($"google.navigation:q={coordinates.Lat},{coordinates.Long}");
				Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
				mapIntent.SetPackage("com.google.android.apps.maps");
				StartActivity(mapIntent);
			};
        }

		private View GetItemView(int position, Place item, View convertView)
		{
			var view = convertView ?? this._inflater.Inflate(Resource.Layout.RowItem, null);

			var title = view.FindViewById<TextView>(Resource.Id.Title);
			title.Text = item.Name + $"({item.Coordinates.Lat.ToString("#.##")}, {item.Coordinates.Long.ToString("#.##")})";

            return view;
		}
    }
}

