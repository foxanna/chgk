using Android.App;
using Android.Gms.Ads;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.InAppBilling;
using Xamarin.InAppBilling.Utilities;

namespace ChGK.Droid.Views
{
    [Activity(Label = "")]
    public class AdvertView : MenuItemIndependentView
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.AdvertView;
            }
        }

        readonly List<AdView> _banners = new List<AdView>();
        Button _donateButton;
        TextView _nameTextView, _priceTextView;

        InAppBillingServiceConnection _serviceConnection;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var adContainer = FindViewById<LinearLayout>(Resource.Id.ad_container);

            foreach (var adId in Resources.GetStringArray(Resource.Array.ads_ids))
            {
                var adView = new AdView(this)
                {
                    AdSize = AdSize.SmartBanner,
                    AdUnitId = adId,
                };

                adContainer.AddView(adView);

                _banners.Add(adView);
            }

            foreach (var banner in _banners)
            {
                var adRequestBuilder = new AdRequest.Builder();
                banner.LoadAd(adRequestBuilder.Build());
            }

            _donateButton = FindViewById<Button>(Resource.Id.donate_button);
            _donateButton.Enabled = false;
            _donateButton.Click += (s, e) => BuyProduct(_selectedProduct);

            _nameTextView = FindViewById<TextView>(Resource.Id.name_text);
            _priceTextView = FindViewById<TextView>(Resource.Id.price_text);

            ConnectInAppBilling();
        }

        void ConnectInAppBilling()
        {
            string value = Security.Unify(Resources.GetStringArray(Resource.Array.billing_key), new int[] { 0, 1, 2, 3 });

            _serviceConnection = new InAppBillingServiceConnection(this, value);
            _serviceConnection.OnConnected += connected;
            _serviceConnection.OnInAppBillingError += OnInAppBillingError;
            _serviceConnection.Connect();
        }

        void BillingHandler_OnProductPurchaseCompleted(int response, Purchase purchase)
        {           
            bool result = _serviceConnection.BillingHandler.ConsumePurchase(purchase);
            ShowToast(GetString(Resource.String.donate_thanks));
        }

        IList<Product> _products;
        Product _selectedProduct;

        async void connected()
        {
            _serviceConnection.BillingHandler.OnProductPurchaseCompleted += BillingHandler_OnProductPurchaseCompleted;
            _products = await _serviceConnection.BillingHandler.QueryInventoryAsync(new List<string> { "donate" }, ItemType.Product);

            if (_products != null && _products.Count > 0)
            {
                _donateButton.Enabled = true;
                _selectedProduct = _products[0];

                _nameTextView.Text = _selectedProduct.Title;
                _priceTextView.Text = _selectedProduct.Price;
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

        void OnInAppBillingError(InAppBillingErrorType error, string message)
        {
            ShowToast("Oops: " + message);
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

        protected override void OnResume()
        {
            base.OnResume();

            foreach (var banner in _banners)
            {
                banner.Resume();
            }
        }

        protected override void OnPause()
        {
            foreach (var banner in _banners)
            {
                banner.Pause();
            }

            base.OnPause();
        }

        protected override void OnDestroy()
        {
            foreach (var banner in _banners)
            {
                banner.Destroy();
            }

            if (_serviceConnection != null)
            {
                _serviceConnection.Disconnect();
            }

            base.OnDestroy();
        }
    }
}
