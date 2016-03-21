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
using Android.Views.InputMethods;
using Android.Content.PM;

namespace BreadCrumbs.Droid
{
    [Activity(Label = "Bread Crumbs", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
		public MainViewModel ViewModel { get; set; }

		private ListView _placesListView;
        private EditText _placeNameEditText;

        private LayoutInflater _inflater;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			_inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);

			ViewModel = new MainViewModel();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            _placeNameEditText = FindViewById<EditText>(Resource.Id.PlaceNameEditText);
            _placeNameEditText.KeyPress += (sender, e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    SavePlaceAndClearTextBox();
                    e.Handled = true;
                }
            };

            Button savePlaceButton = FindViewById<Button>(Resource.Id.SavePlaceButton);
            savePlaceButton.Click += (sender, e) => {
                SavePlaceAndClearTextBox();
            };

            _placesListView = FindViewById<ListView>(Resource.Id.PlacesListView);
			_placesListView.Adapter = ViewModel.SavedPlaces.GetAdapter(GetItemView);

			_placesListView.ItemClick += (sender, e) =>
            {
                if (IsGoogleMapsInstalled())
                {
                    var coordinates = this.ViewModel.SavedPlaces.ElementAt(e.Position).Coordinates;
                    var gmmIntentUri = Android.Net.Uri.Parse($"google.navigation:q={coordinates.Lat},{coordinates.Long}&directionsmode=walking");
                    Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
                    mapIntent.SetPackage("com.google.android.apps.maps");
                    StartActivity(mapIntent);
                }
                else
                {
                    var transaction = FragmentManager.BeginTransaction();
                    var dialogFragment = new AlertDialogFragment();
                    dialogFragment.Show(transaction, "dialog_fragment");
                }
			};

            _placesListView.ItemLongClick += (sender, e) =>
            {
                ViewModel.Remove(ViewModel.SavedPlaces.ElementAt(e.Position));
            };
        }

        private bool IsGoogleMapsInstalled()
        {
            try
            {
                ApplicationInfo info = PackageManager.GetApplicationInfo("com.google.android.apps.maps", 0);
                return true;
            }
            catch (PackageManager.NameNotFoundException e)
            {
                return false;
            }
        }

        private void _placesListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SavePlaceAndClearTextBox()
        {
            ViewModel.SaveAsync(_placeNameEditText.Text);

            _placeNameEditText.Text = "";
        }

        private View GetItemView(int position, Place item, View convertView)
		{
			var view = convertView ?? this._inflater.Inflate(Resource.Layout.RowItem, null);

			var title = view.FindViewById<TextView>(Resource.Id.Title);
			title.Text = item.DisplayName;

            return view;
		}
    }
}

