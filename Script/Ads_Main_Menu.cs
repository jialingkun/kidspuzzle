using UnityEngine;
using System.Collections;
using admob;

public class Ads_Main_Menu : MonoBehaviour {
	Admob ad;
	// Use this for initialization
	void Start () {
		initAdmob();
		Admob.Instance().showBannerRelative(AdSize.SmartBanner, AdPosition.BOTTOM_CENTER, 0);
	}

	void initAdmob()
	{
		ad = Admob.Instance();
		ad.initAdmob("ca-app-pub-3940256099942544/2934735716", "ca-app-pub-3940256099942544/4411468910");
		string[] keywords = { "animal","puzzle","children game"};
		ad.setKeywords(keywords);

	}
}
