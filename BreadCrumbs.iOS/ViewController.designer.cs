// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace BreadCrumbs.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField placeNameTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView PlacesTableView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton saveButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (placeNameTextField != null) {
				placeNameTextField.Dispose ();
				placeNameTextField = null;
			}
			if (PlacesTableView != null) {
				PlacesTableView.Dispose ();
				PlacesTableView = null;
			}
			if (saveButton != null) {
				saveButton.Dispose ();
				saveButton = null;
			}
		}
	}
}
