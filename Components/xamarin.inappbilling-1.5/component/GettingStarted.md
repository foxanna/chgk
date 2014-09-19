##Xamarin.InAppBilling Component##

###Getting Started with Xamarin.InAppBilling###

To use an `Xamarin.InAppBilling` in your mobile application include the component in your project and reference the following using statements in your C# code:

```
using Xamarin.InAppBilling;
using Xamarin.InAppBilling.Utilities;
``` 

###A Special Note Security###

When developing Xamarin.Android applications that support In-App Billing there are several steps that should be taken to protect your app from being hacked by a malicious user and keep unlocked content safe.

While the best practice is to perform signature verification on a remote server and not on a device, this might not always be possible. Another technique is to obfuscate your Google Play public key and never store the assembled key in memory.

`Xamarin.InAppBilling` provides the **Unify** routine that can be used to break your Google Play public key into two or more pieces and to obfuscate those pieces using one or more key/value pairs. In addition, `Xamarin.InAppBilling` always encrypts your private key while it's in memory.

Here is an example of using **Unify** to obfuscate a private key:

```
string value = Security.Unify (
	new string[] { "X0X0-1c...", "+123+Jq...", "//w/2jANB...", "...Kl+/ID43" }, 
	new int[] { 2, 3, 1, 0 }, 
	new string[] {  "X0X0-1", "9V4XD", "+123+", "R9eGv", "//w/2", "MIIBI", "+/ID43", "9alu4" });
```
Where the first parameter is an array of strings containing your private key broken into two or more parts in a random order. The second parameter is an array of integers listing of order that the private key parts should be assembled in. The third, optional, parameter is a list of key/value pairs that will be used to replace sequences in the assembled key.

_**Note:** There are several more steps that should be taken to secure your application, please see Google's official [Security and Design](http://developer.android.com/google/play/billing/billing_best_practices.html) document for further details._

###Connecting to Google Play###

When your application's main activity first starts, you'll need to initially establish a connection to Google Play services to support In-App Billing.

The following is an example of using `Xamarin.InAppBilling` to connect to Google Play:

```
private InAppBillingServiceConnection _serviceConnection;
...

// Create a new connection to the Google Play Service
_serviceConnection = new InAppBillingServiceConnection (this, publicKey);
_serviceConnection.OnConnected += () => {
	// Load available products and any purchases
	...
};

// Attempt to connect to the service
_serviceConnection.Connect ();
```

The _OnConnected_ event will be raised once a successful connection to Google Play Services has been established.

_**Note:** Remember to unbind from the In-app Billing service when you are done with your Activity. If you don’t unbind, the open service connection could cause your device’s performance to degrade._

###Requesting Available Products###

Once you have an open connection you can request a list of available products that the user can purchase by providing a list of product ID. Here is an example:

```
private IList<Product> _products;
...

_products = await _serviceConnection.BillingHandler.QueryInventoryAsync (new List<string> {
	ReservedTestProductIDs.Purchased,
	ReservedTestProductIDs.Canceled,
	ReservedTestProductIDs.Refunded,
	ReservedTestProductIDs.Unavailable
}, ItemType.Product);

// Were any products returned?
if (_products == null) {
	// No, abort
	return;
}
```
In the above example we are requesting the four predefined reserved test Product ID provided by Google to test an application without actually making a purchase.

###Purchasing a Product###

Purchasing a product can be accomplished using the following:

```
// Ask the open connection's billing handler to purchase the selected product
_serviceConnection.BillingHandler.BuyProduct(_selectedProduct);
```
Where **_selectedProduct** is one of the products returned by the **QueryInventoryAsync** routine above.

###Handling the Purchase Result###

When the user attempts to purchase an In App Billing product from your application, the _OnProductPurchased_ event will be raised when the purchase is successfully handed off to the Google Play app to be processed.

To complete the purchase cycle, you will need to override the **OnActivityResult** method of the Activity of the app that initiated the purchase and pass the result to the _HandleActivityResult_ method of the _BillingHandler_ of your open _InAppBillingServiceConnection_:

```
protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
{
	// Ask the open service connection's billing handler to process this request
	_serviceConnection.BillingHandler.HandleActivityResult (requestCode, resultCode, data);

	// TODO: Use a call back to update the purchased items
	// or listen to the OnProductPurchaseCompleted event to
	// handle a successful purchase
}
```

With the above code in place, you can listen to the _OnProductPurchaseCompleted_ event of the _BillingHandler_ to handle the successful purchase of the product or simply request the full list of purchased products again from the _BillingHandler_.

**PLEASE NOTE: The _OnProductPurchaseCompleted_ event will not be raised unless you have overridden the _OnActivityResult_ method of the Activity that initiated the purchase and passed the result to the _HandleActivityResult_ for processing!**

Any issues processing a purchase will raise either the _OnProductPurchasedError_, _OnPurchaseFailedValidation_ or _InAppBillingProcessingError_ events with details of the issue that stopped processing.

###Requesting a List of Previous Purchases###

After the user has purchased one or more products, the following code will request a list of those purchases:

```
// Ask the open connection's billing handler to get any purchases
var purchases = _serviceConnection.BillingHandler.GetPurchases (ItemType.Product);
```

###Consuming a Purchase###

For products that are consumable in your application, such as coins or tokens, use the following code to inform Google Play that a given product purchase has been consumed:

```
// Attempt to consume the given product
bool result = _serviceConnection.BillingHandler.ConsumePurchase (purchasedItem);

// Was the product consumed?
if (result) {
	// Yes, update interface
	...
}
```
Where **purchasedItem** is a purchase returned by the **GetPurchases** routine above. The _OnPurchaseConsumed_ event will be raised if the product is successfuly purchased else the _OnPurchaseConsumedError_ event will be raised.

###Disconnecting from Google Play###

When you are done with your Activity you must remember to unbind from the In-app Billing service. If you don’t unbind, the open service connection could cause your device’s performance to degrade.

To do this, override you Activity's **OnDestroy** method and call the **Disconnect** routine:

```
protected override void OnDestroy () {
			
	// Are we attached to the Google Play Service?
	if (_serviceConnection != null) {
		// Yes, disconnect
		_serviceConnection.Disconnect ();
	}

	// Call base method
	base.OnDestroy ();
}
```

###Events###

`Xamarin.InAppBilling` defines the following events that you can monitor and respond to:

* **OnConnected** - Raised when the component attaches to Google Play.
* **OnDisconnected** - Raised when the component detaches from Google Play.
* **OnInAppBillingError** - Raised when an error occurs inside the component.
* **OnProductPurchasedError** - Raised when there is an error purchasing a product or subscription.
* **OnProductPurchased** - Raised when the intent to purchase a product is successfully sent to Google Play. The _OnProductPurchaseCompleted_ is raised when the purchase intent is successfully processed by Google Play and the result returned.
* **OnProductPurchaseCompleted** - Raised when a product or subscription is fully processed by Google Play and returned. _NOTE: You should check the state of the response code as this event is raised for both successful and unsuccessful purchases._
* **OnPurchaseConsumedError** - Raised when there is an error consuming a purchase.
* **OnPurchaseConsumed** - Raised when a purchase is successfully consumed.
* **OnPurchaseFailedValidation** - Raised when a previous purchase fails validation. This is ususally caused when the application's Public Key does not match the Public Key in the Google Developer Console 100%.
* **OnGetProductsError** - Raised when a request to *GetProducts* fails.
* **OnInvalidOwnedItemsBundleReturned** - Raised when an invalid bundle of purchases is returned from Google Play.
* **InAppBillingProcessingError** - Raised when any other type of processing issue not covered by an existing event occurs.

##Examples##

For full examples of using `Xamarin.InAppBilling` in your mobile application, please see the  _Xamarin.InAppBilling_ example app included with this component.

See the API documentation for `Xamarin.InAppBilling` for a complete list of features and their usage.

## Other Resources

* [Xamarin Components](https://components.xamarin.com)
* [Implementing In-app Billing (IAB Version 3)](http://developer.android.com/google/play/billing/billing_integrate.html)
* [Security and Design](http://developer.android.com/google/play/billing/billing_best_practices.html)
* [Support](http://xamarin.com/support)