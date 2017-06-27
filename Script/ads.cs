using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class Ads : MonoBehaviour {
	private BannerView bannerViewTop;
	private BannerView bannerViewBottom;
	private InterstitialAd interstitial;
	private string bannerID;
	private string interstitialID;
	private AdRequest bannerAdRequest;

	private int interstitialCounter;
	void Start () {
		bannerID = "ca-app-pub-8998944047411782/7716290158";
		interstitialID = "ca-app-pub-8998944047411782/9193023356";
		bannerAdRequest = new AdRequest.Builder().Build();
		interstitialCounter = 0;
		RequestInterstitial ();
	}

	public void showBannerBottom(){
		string adUnitId = bannerID;

		if (bannerViewTop!=null) {
			bannerViewTop.Hide ();
		}

		if (bannerViewBottom != null) {
			bannerViewBottom.Show ();
		} else {
			// Create a 320x50 banner at the top of the screen.
			bannerViewBottom = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
			// Load the banner with the request.
			bannerViewBottom.LoadAd(bannerAdRequest);
		}



	}

	public void showBannerTop(){
		string adUnitId = bannerID;

		if (bannerViewBottom != null) {
			bannerViewBottom.Hide ();
		
		}

		if (bannerViewTop != null) {
			bannerViewTop.Show ();
		} else {
			// Create a 320x50 banner at the top of the screen.
			bannerViewTop = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
			// Load the banner with the request.
			bannerViewTop.LoadAd(bannerAdRequest);
		}



	}

	public void RequestInterstitial(){
		if (interstitial != null) {
			interstitial.Destroy ();
		}
		string adUnitId = interstitialID;
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	public void showInterstitial(){
		if (interstitial.IsLoaded ()) {
			interstitial.Show ();
			RequestInterstitial ();
		} else {
			RequestInterstitial ();
		}
	}

	public int getInterstitialCounter(){
		return interstitialCounter;
	}

	public void addInterstitialCounter(){
		interstitialCounter++;
	}

	public void clearInterstitialCounter(){
		interstitialCounter = 0;
	}

}
