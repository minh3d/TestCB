////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaymentManagerExample {
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public const string SMALL_PACK 	=  "your.product.id1.here";
	public const string NC_PACK 	=  "your.product.id2.here";
	


	private static bool IsInited = false;
	public static void init() {


		if(!IsInited) {

			//You do not have to add products by code if you already did it in seetings guid
			//Windows -> IOS Native -> Edit Settings
			//Billing tab.
			IOSInAppPurchaseManager.instance.addProductId(SMALL_PACK);
			IOSInAppPurchaseManager.instance.addProductId(NC_PACK);
			
			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.PRODUCT_BOUGHT, onProductBought);
			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.TRANSACTION_FAILED, onTransactionFailed);
			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.RESTORE_TRANSACTION_FAILED, onRestoreTransactionFailed);
			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.VERIFICATION_RESPONSE, onVerificationResponce);
			IOSInAppPurchaseManager.instance.addEventListener(IOSInAppPurchaseManager.STORE_KIT_INITIALIZED, OnStoreKitInited);
			


			IsInited = true;

		} 

		IOSInAppPurchaseManager.instance.loadStore();


	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public static void buyItem(string productId) {
		IOSInAppPurchaseManager.instance.buyProduct(productId);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private static void onProductBought(CEvent e) {
		
		IOSStoreKitResponse responce =  e.data as IOSStoreKitResponse;
		Debug.Log("STORE KIT GOT BUY: " + responce.productIdentifier);
		Debug.Log("RECIPT: " + responce.receipt);

		switch(responce.productIdentifier) {
		case SMALL_PACK:
			//code for adding small game money amount here
			break;
		case NC_PACK:
			//code for unlocking cool item here
			break;

		}

		
		IOSNativePopUpManager.showMessage("Success", "product " + responce.productIdentifier + " is purchased");
	}

	private static void onRestoreTransactionFailed() {
		IOSNativePopUpManager.showMessage("Fail", "Restore Failed");
	}

	private static void onTransactionFailed(CEvent e) {
		IOSStoreKitResponse responce =  e.data as IOSStoreKitResponse;
		IOSNativePopUpManager.showMessage("Fail", responce.error);
	}

	private static void onVerificationResponce(CEvent e) {
		IOSStoreKitVerificationResponse responce =  e.data as IOSStoreKitVerificationResponse;

		IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + responce.status.ToString());

		Debug.Log("ORIGINAL JSON ON: " + responce.originalJSON);
	}

	private static void OnStoreKitInited() {
		IOSInAppPurchaseManager.instance.removeEventListener(IOSInAppPurchaseManager.STORE_KIT_INITIALIZED, OnStoreKitInited);
		IOSNativePopUpManager.showMessage("StoreKit Inited", "Avaliable products cound: " + IOSInAppPurchaseManager.instance.products.Count.ToString());
	}

	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
