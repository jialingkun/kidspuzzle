using UnityEngine;
using System.Collections;
using admob;

public class Ads : MonoBehaviour {
	Admob ad;
	private int interstitialCounter;
	void Start () {
		initAdmob();
		interstitialCounter = 0;
	}

	void initAdmob()
	{
		ad = Admob.Instance();
		ad.setForChildren (true); //ads for children
		ad.interstitialEventHandler += onInterstitialEvent;
		ad.initAdmob("ca-app-pub-8998944047411782/7716290158", "ca-app-pub-8998944047411782/9193023356"); //(banner, interstitial)

	}

	void onInterstitialEvent(string eventName, string msg)
	{
		if (eventName == AdmobEvent.onAdLoaded)
		{
			Admob.Instance().showInterstitial();
		}
	}

	public void showBannerBottom(){
		Admob.Instance().removeBanner();
		Admob.Instance().showBannerRelative(AdSize.SmartBanner, AdPosition.BOTTOM_CENTER, 0);
	}

	public void showBannerTop(){
		Admob.Instance().removeBanner();
		Admob.Instance().showBannerRelative(AdSize.SmartBanner, AdPosition.TOP_CENTER, 0);
	}

	public void showInterstitial(){
		if (ad.isInterstitialReady())
		{
			ad.showInterstitial();
			ad.loadInterstitial();
		}
		else
		{
			ad.loadInterstitial();
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
