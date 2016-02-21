using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Xamarin.InAppBilling;
using Xamarin.InAppBilling.Utilities;

namespace ChGK.Droid.Views
{
    [Activity(Label = "", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class AboutView : MenuItemIndependentView
    {
        private View _donateButton;

        private Product _selectedProduct;

        private InAppBillingServiceConnection _serviceConnection;
        protected override int LayoutId => Resource.Layout.AboutView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FindViewById<TextView>(Resource.Id.a1).MovementMethod = LinkMovementMethod.Instance;
            FindViewById<TextView>(Resource.Id.a2).MovementMethod = LinkMovementMethod.Instance;

            _donateButton = FindViewById(Resource.Id.donate);
            _donateButton.Click += (e, s) => BuyProduct(_selectedProduct);

            ConnectInAppBilling();
        }

        private void ConnectInAppBilling()
        {
            var value = Security.Unify(Resources.GetStringArray(Resource.Array.billing_key), new[] {0, 1, 2, 3});

            _serviceConnection = new InAppBillingServiceConnection(this, value);

            _serviceConnection.OnConnected += Connected;
            _serviceConnection.Connect();
        }

        private void ShowToast(string message)
        {
            var toast = Toast.MakeText(this, message, ToastLength.Short);
            toast.View.SetBackgroundResource(Resource.Drawable.undo_bar_bg);
            toast.Show();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            _serviceConnection?.BillingHandler?.HandleActivityResult(requestCode, resultCode, data);
        }

        private async void Connected()
        {
            var products = await
                _serviceConnection.BillingHandler.QueryInventoryAsync(new List<string> {"donate"}, ItemType.Product);

            if (!products.Any())
                return;

            _serviceConnection.BillingHandler.OnProductPurchaseCompleted +=
                BillingHandler_OnProductPurchaseCompleted;

            _donateButton.Visibility = ViewStates.Visible;
            _selectedProduct = products[0];
        }

        private void BuyProduct(Product product)
        {
            if (product == null)
                return;

            _serviceConnection.BillingHandler.BuyProduct(product);
        }


        private void BillingHandler_OnProductPurchaseCompleted(int response, Purchase purchase)
        {
            var result = _serviceConnection.BillingHandler.ConsumePurchase(purchase);
            ShowToast(GetString(Resource.String.donate_thanks));
        }

        protected override void OnDestroy()
        {
            _serviceConnection?.Disconnect();

            base.OnDestroy();
        }
    }
}