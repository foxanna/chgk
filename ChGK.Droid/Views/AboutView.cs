using Android.App;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Xamarin.InAppBilling;
using Xamarin.InAppBilling.Utilities;

namespace ChGK.Droid.Views
{
	[Activity (Label = "", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]			
	public class AboutView : MenuItemIndependentView
	{
		protected override int LayoutId {
			get {
				return Resource.Layout.AboutView;
			}
		}

        InAppBillingServiceConnection _serviceConnection;

        Product _selectedProduct;

        View _donateButton;

		protected override void OnCreate (Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);

			FindViewById<TextView> (Resource.Id.a1).MovementMethod = LinkMovementMethod.Instance;
			FindViewById<TextView> (Resource.Id.a2).MovementMethod = LinkMovementMethod.Instance;

            _donateButton = FindViewById(Resource.Id.donate); 
            
            ConnectInAppBilling();
		}

        void ConnectInAppBilling()
        {
            string value = Security.Unify(Resources.GetStringArray(Resource.Array.billing_key), new int[] { 0, 1, 2, 3 });

            _serviceConnection = new InAppBillingServiceConnection(this, value);
            _serviceConnection.OnConnected += connected;
            _serviceConnection.Connect();
        }
        
        void ShowToast(string message)
        {
            var toast = Toast.MakeText(this, message, ToastLength.Short);
            toast.View.SetBackgroundResource(Resource.Drawable.undo_bar_bg);
            toast.Show();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            _serviceConnection.BillingHandler.HandleActivityResult(requestCode, resultCode, data);
        }

        async void connected()
        {
            _serviceConnection.BillingHandler.OnProductPurchaseCompleted += BillingHandler_OnProductPurchaseCompleted;
            var products = await _serviceConnection.BillingHandler.QueryInventoryAsync(new List<string> { "donate" }, ItemType.Product);

            if (products != null && products.Count > 0)
            {
                _donateButton.Visibility = ViewStates.Visible;
                _selectedProduct = products[0];

                _donateButton.Click += (e, s) => BuyProduct(_selectedProduct);
            }
        }

        void BuyProduct(Product product)
        {
            if (product == null)
            {
                return;
            }

            _serviceConnection.BillingHandler.BuyProduct(product);
        }


        void BillingHandler_OnProductPurchaseCompleted(int response, Purchase purchase)
        {
            bool result = _serviceConnection.BillingHandler.ConsumePurchase(purchase);
            ShowToast(GetString(Resource.String.donate_thanks));
        }
        
        protected override void OnDestroy()
        {
            if (_serviceConnection != null)
            {
                _serviceConnection.Disconnect();
            }

            base.OnDestroy();
        }
	}
}

