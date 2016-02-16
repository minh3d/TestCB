using UnityEngine;
using System.Collections;

public class UM_InAppPurchaseManager : SA_Singleton<UM_InAppPurchaseManager> {


	public const string ON_PURCHASE_FLOW_FINISHED   = "on_purchase_flow_finished";
	public const string ON_BILLING_CONNECT_FINISHED   = "on_billing_connect_finished";
	private bool _IsInited = false;

	public const string PREFS_KEY = "UM_InAppPurchaseManager";


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);


		switch(Application.platform) {
			
		case RuntimePlatform.Android:
			//listening for purchase and consume events
			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_PRODUCT_PURCHASED, Android_OnProductPurchased);
			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_PRODUCT_CONSUMED,  Android_OnProductConsumed);
			
			//initilaizing store
			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_BILLING_SETUP_FINISHED, Android_OnBillingConnected);
			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_RETRIEVE_PRODUC_FINISHED, Android_OnRetriveProductsFinised);

			break;
			
		case RuntimePlatform.IPhonePlayer:
			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.PRODUCT_BOUGHT, IOS_OnProductPurchased);
			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.TRANSACTION_FAILED, IOS_OnTransactionFailed);

			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.STORE_KIT_INITIALIZED, IOS_OnStoreKitInited);
			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.STORE_KIT_INITI_FAILED, IOS_OnStoreKitInitFailed);
			break;
			
			
		case RuntimePlatform.WP8Player:
			WP8InAppPurchasesManager.instance.addEventListener(WP8InAppPurchasesManager.INITIALIZED, WP8_OnInitComplete);
			WP8InAppPurchasesManager.instance.addEventListener(WP8InAppPurchasesManager.PRODUCT_PURCHASE_FINISHED, WP8_OnProductPurchased);
			break;
			
			
		}




	}


	public void Init() {
		switch(Application.platform) {
			
		case RuntimePlatform.Android:
			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				AndroidInAppPurchaseManager.instance.addProduct(product.AndroidId);
			}
			AndroidInAppPurchaseManager.instance.loadStore();
			break;
			
		case RuntimePlatform.IPhonePlayer:
			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				IOSInAppPurchaseManager.instance.addProductId(product.IOSId);
			}
			IOSInAppPurchaseManager.instance.loadStore();
			break;
			
			
		case RuntimePlatform.WP8Player:
			WP8InAppPurchasesManager.instance.init();
			break;
			
			
		}
	}

	//--------------------------------------
	// Methods
	//--------------------------------------




	public void Purchase(string productId) {

		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductById(productId);


		if(p != null) {
			switch(Application.platform) {
				
			case RuntimePlatform.Android:
				AndroidInAppPurchaseManager.instance.purchase(p.AndroidId);
				break;
				
			case RuntimePlatform.IPhonePlayer:

				IOSInAppPurchaseManager.instance.buyProduct(p.IOSId);
				break;
				
				
			case RuntimePlatform.WP8Player:
				WP8InAppPurchasesManager.instance.purchase(p.WP8Id);
				break;
			}


		} else {
			Debug.LogWarning("UM_InAppPurchaseManager product not found: " + productId);
		}

	}

	public bool IsProductPurchased(string id) {
		return IsProductPurchased(UltimateMobileSettings.Instance.GetProductById(id));
	}

	public bool IsProductPurchased(UM_InAppProduct product) {
		if(product == null) {
			return false;
		}

		switch(Application.platform) {
			
		case RuntimePlatform.Android:
			return AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(product.AndroidId);
		case RuntimePlatform.IPhonePlayer:
			return PlayerPrefs.HasKey(PREFS_KEY + product.IOSId);
		case RuntimePlatform.WP8Player:
			return product.WP8Template.isPurchased;
		}

		return false;

	}

	//--------------------------------------
	// Get / SET
	//--------------------------------------

	public bool IsInited {
		get {
			return _IsInited;
		}
	}

	//--------------------------------------
	// WP8 Listners
	//--------------------------------------

	private void WP8_OnInitComplete() {

		_IsInited = true;

		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = "Inited";
		r.isSuccess = true;


		foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {


			WP8ProductTemplate tpl =  WP8InAppPurchasesManager.instance.GetProductById(product.WP8Id);
			if(tpl != null) {
				product.SetTemplate(tpl);
			}
			
		}
		
		dispatch(ON_BILLING_CONNECT_FINISHED, r);
	}

	private void WP8_OnProductPurchased(CEvent e) {
		WP8PurchseResponce recponce = e.data as WP8PurchseResponce;

		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByWp8Id(recponce.productId);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.isSuccess = recponce.IsSuccses;
			r.product = p;
			dispatch(ON_PURCHASE_FLOW_FINISHED, r);
		} else {
			Debug.LogWarning("UM: Product tamplate not found");
			dispatch(ON_PURCHASE_FLOW_FINISHED, new UM_PurchaseResult());
		}
	}

	//--------------------------------------
	// IOS Listners
	//--------------------------------------


	private void IOS_OnProductPurchased(CEvent e) {
		IOSStoreKitResponse responce =  e.data as IOSStoreKitResponse;

		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByIOSId(responce.productIdentifier);
		if(p != null) {
			if(!p.IsConsumable) {
				PlayerPrefs.SetInt(PREFS_KEY + responce.productIdentifier, 1);
			}


			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.isSuccess = true;
			r.product = p;
			dispatch(ON_PURCHASE_FLOW_FINISHED, r);
		} else {
			Debug.LogWarning("UM: Product tamplate not found");
			dispatch(ON_PURCHASE_FLOW_FINISHED, new UM_PurchaseResult());
		}

	}

	private void IOS_OnTransactionFailed(CEvent e) {
		IOSStoreKitResponse responce =  e.data as IOSStoreKitResponse;
		
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByIOSId(responce.productIdentifier);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.isSuccess = false;
			r.product = p;
			dispatch(ON_PURCHASE_FLOW_FINISHED, r);
		} else {
			Debug.LogWarning("UM: Product tamplate not found");
			dispatch(ON_PURCHASE_FLOW_FINISHED, new UM_PurchaseResult());
		}
	}

	private void IOS_OnStoreKitInited() {
		_IsInited = true;


		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = "Inited";
		r.isSuccess = true;


		foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {

			ProductTemplate tpl = IOSInAppPurchaseManager.instance.GetProductById(product.IOSId); 
			if(tpl != null) {
				product.SetTemplate(tpl);
			}
			
		}
		
		dispatch(ON_BILLING_CONNECT_FINISHED, r);
	}

	private void IOS_OnStoreKitInitFailed(CEvent e) {

		ISN_Error er = e.data as ISN_Error;

		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = er.description;
		r.isSuccess = false;
		
		dispatch(ON_BILLING_CONNECT_FINISHED, r);
	}



	//--------------------------------------
	// Android Listners
	//--------------------------------------


	private void Android_OnProductPurchased(CEvent e) {
		BillingResult result = e.data as BillingResult;

		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByAndroidId(result.purchase.SKU);
		if(p != null) {
			if(p.IsConsumable) {
				AndroidInAppPurchaseManager.instance.consume(result.purchase.SKU);
			} else {
				Debug.Log("non cons");
				UM_PurchaseResult r =  new UM_PurchaseResult();
				r.isSuccess = result.isSuccess;
				r.product = p;
				dispatch(ON_PURCHASE_FLOW_FINISHED, r);
			}
		} else {
			Debug.LogWarning("UM: Product tamplate not found");
			dispatch(ON_PURCHASE_FLOW_FINISHED, new UM_PurchaseResult());
		}



	}
	
	
	private void Android_OnProductConsumed(CEvent e) {
		BillingResult result = e.data as BillingResult;

		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByAndroidId(result.purchase.SKU);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.isSuccess = result.isSuccess;
			r.product = p;
			dispatch(ON_PURCHASE_FLOW_FINISHED, r);
		} else {
			Debug.LogWarning("UM: Product tamplate not found");
			dispatch(ON_PURCHASE_FLOW_FINISHED, new UM_PurchaseResult());
		}
	}
	
	
	private void Android_OnBillingConnected(CEvent e) {
		BillingResult result = e.data as BillingResult;
		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		AndroidInAppPurchaseManager.instance.removeEventListener (AndroidInAppPurchaseManager.ON_BILLING_SETUP_FINISHED, Android_OnBillingConnected);
		
		
		if(result.isSuccess) {
			//Store connection is Successful. Next we loading product and customer purchasing details
			r.isSuccess = true;
			AndroidInAppPurchaseManager.instance.retrieveProducDetails();

		} else {
			r.isSuccess = false;
			dispatch(ON_BILLING_CONNECT_FINISHED, r);
		}


		r.message = result.message;

		
	}
	
	private void Android_OnRetriveProductsFinised(CEvent e) {
		_IsInited = true;

		BillingResult result = e.data as BillingResult;
		AndroidInAppPurchaseManager.instance.removeEventListener (AndroidInAppPurchaseManager.ON_RETRIEVE_PRODUC_FINISHED, Android_OnRetriveProductsFinised);

		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = result.message;
		r.isSuccess = result.isSuccess;

		if(r.isSuccess) {
			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				GoogleProductTemplate tpl = AndroidInAppPurchaseManager.instance.inventory.GetProductDetails(product.AndroidId);
				if(tpl != null) {
					product.SetTemplate(tpl);
					if(product.IsConsumable && IsProductPurchased(product)) {
						AndroidInAppPurchaseManager.instance.consume(product.AndroidId);
					}
				}

			}
		}

		dispatch(ON_BILLING_CONNECT_FINISHED, r);
		
	}

}
