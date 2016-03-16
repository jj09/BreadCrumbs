using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace BreadCrumbs.Droid
{

    public class AlertDialogFragment : DialogFragment
    {
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            EventHandler<DialogClickEventArgs> okhandler;
            var builder = new AlertDialog.Builder(Activity)
                .SetMessage("You need to install Google Maps in order to navigate to place")
                .SetPositiveButton("Ok", (sender, args) =>
                {
                    // Open Play Store to install Google Maps
                    // Not tested but this should never happen, as google maps cannot be uninstalled:
                    // https://community.republicwireless.com/thread/2364
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=com.google.android.apps.maps"));
                    StartActivity(intent);
                    
                })
                .SetNegativeButton("Cancel", (sender, args) => { })
                .SetTitle("Google Maps not installed");
            return builder.Create();
        }
    }
}