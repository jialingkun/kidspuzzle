using UnityEngine;
using System.Collections;
using admob;

public class Ads_Gameplay : MonoBehaviour {

	Admob ad;
	void Start () {
		initAdmob();
		Admob.Instance().showBannerRelative(AdSize.SmartBanner, AdPosition.TOP_CENTER, 0);
	}

	void initAdmob()
	{
		ad = Admob.Instance();
		ad.interstitialEventHandler += onInterstitialEvent;
		ad.initAdmob("ca-app-pub-3940256099942544/2934735716", "ca-app-pub-3940256099942544/4411468910");
		string[] keywords = { "animal","puzzle","children game"};
		ad.setKeywords(keywords);
		Debug.Log("admob inited -------------");

	}

	void onInterstitialEvent(string eventName, string msg)
	{
		Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
		if (eventName == AdmobEvent.onAdLoaded)
		{
			Admob.Instance().showInterstitial();
		}
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
}
