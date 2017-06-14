﻿using UnityEngine;
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
		ad.interstitialEventHandler += onInterstitialEvent;
		ad.initAdmob("ca-app-pub-3940256099942544/2934735716", "ca-app-pub-3940256099942544/4411468910");
		string[] keywords = { "animal","puzzle","children game"};
		ad.setKeywords(keywords);

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
