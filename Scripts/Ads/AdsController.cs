using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
using System;


public class AdsController : MonoBehaviour {

#if UNITY_ANDROID
	string bannerAdUnitId = "ca-app-pub-8709392240032722/2787916304";
	string iterstitialAdUnitId = "ca-app-pub-8709392240032722/1785483914";
    string rewardAdUnitId = "ca-app-pub-8709392240032722/2032279976";
#elif UNITY_IPHONE
    string bannerAdUnitId = "ca-app-pub-8709392240032722/4866735990";
	string iterstitialAdUnitId = "ca-app-pub-8709392240032722/9137360326";
    string rewardAdUnitId = "ca-app-pub-8709392240032722/4561583853";
    //string rewardAdUnitId = "ca-app-pub-3940256099942544/1712485313"; //test

#else
	string bannerAdUnitId = "unexpected_platform";
	string iterstitialAdUnitId = "unexpected_platform";
#endif

    private BannerView bannerView;
	private InterstitialAd interstitial;
    //private RewardedAd rewardedAd;

    Cash cash;
    NoAdsMinLeftController noAdsMinLeftController;

    bool wasOnNet = false;
	bool isDestroyedBanner = false;

    public void InitiAdmobAds()
    {
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        //noAdsMinLeftController = GameObject.Find("noAdsMinLeft").GetComponent<NoAdsMinLeftController>();
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return;


        MobileAds.Initialize(initStatus => { });


        wasOnNet = true;
        RequestBanner();
        RequestiIterstitial();
        //CreateAndLoadRewardedAd();
    }


    private void Update()
    {
        if (wasOnNet == false)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Debug.Log("ads updata 1");
                RequestBanner();
                RequestiIterstitial();
                //CreateAndLoadRewardedAd();
                wasOnNet = true;
            }
        }

        if (wasOnNet == true &&
            Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("ads updata 2");
            if (bannerView != null)
                bannerView.Destroy();
            if (interstitial != null)
                interstitial.Destroy();
            isDestroyedBanner = true;
            wasOnNet = false;
            return;
        }

		if (isDestroyedBanner == true){
			if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Debug.Log("ads updata 3");
                RequestBanner ();
                RequestiIterstitial();
                //if (this.rewardedAd.IsLoaded() == false)
                //    CreateAndLoadRewardedAd();
                wasOnNet = true;
				isDestroyedBanner = false;
			}
        }
    }

    //public void ShowRewardAd()
    //{
    //    if (this.rewardedAd.IsLoaded())
    //    {
    //        this.rewardedAd.Show();
    //    }
    //}


    //private void CreateAndLoadRewardedAd()
    //{
    //    this.rewardedAd = new RewardedAd(rewardAdUnitId);


    //    // 広告の読み込みが完了すると呼び出されます。
    //    this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
    //    // 広告の読み込みが失敗すると呼び出されます。
    //    this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
    //    // 広告がデバイスの画面いっぱいに表示されると呼び出されます。
    //    this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
    //    // 広告の表示に失敗すると呼び出されます。
    //    this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
    //    //動画を視聴したユーザーに報酬を付与するときに呼び出されます。
    //    this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    //    // 動画リワード広告を閉じると呼び出されます。
    //    this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;


    //    // Create an empty ad request.
    //    AdRequest request = new AdRequest.Builder().Build();
    //    // Load the rewarded ad with the request.
    //    this.rewardedAd.LoadAd(request);
    //}


    //// 広告の読み込みが完了すると呼び出されます。
    //public void HandleRewardedAdLoaded(object sender, EventArgs args)
    //{
    //    MonoBehaviour.print("HandleRewardedAdLoaded event received");
    //}

    //// 広告の読み込みが失敗すると呼び出されます。
    //public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    //{
    //    MonoBehaviour.print(
    //        "HandleRewardedAdFailedToLoad event received with message: "
    //                         + args.Message);
    //}

    //// 広告がデバイスの画面いっぱいに表示されると呼び出されます。
    //public void HandleRewardedAdOpening(object sender, EventArgs args)
    //{
    //    MonoBehaviour.print("HandleRewardedAdOpening event received");
    //}

    //// 広告の表示に失敗すると呼び出されます。
    //public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    //{
    //    MonoBehaviour.print(
    //        "HandleRewardedAdFailedToShow event received with message: "
    //                         + args.Message);
    //}

    //// 動画リワード広告を閉じると呼び出されます。
    //public void HandleRewardedAdClosed(object sender, EventArgs args)
    //{
    //    MonoBehaviour.print("HandleRewardedAdClosed event received");
    //    this.CreateAndLoadRewardedAd();
    //}

    ////動画を視聴したユーザーに報酬を付与するときに呼び出されます。
    //public void HandleUserEarnedReward(object sender, Reward args)
    //{
    //    //string type = args.Type;
    //    //double amount = args.Amount;
    //    MonoBehaviour.print("Reward");

    //    Debug.Log("noAdsMinLeftController.noAdsSecTime = " + noAdsMinLeftController.noAdsSecTime);
    //    noAdsMinLeftController.StartNoAdsByReward(noAdsMinLeftController.noAdsSecTime);
    //    if (bannerView != null)
    //        bannerView.Destroy();
    //    cash.setting.preferenceDate.isRewarded = true;
    //    cash.saveManager.SaveFile_PreferenceDate();
    //}



    public void RequestBanner()
	{
        if (cash.setting.preferenceDate.isPurchased == true ||
            cash.setting.preferenceDate.isRewarded == true)
        {
            return;
        }


        if (bannerView != null)
            bannerView.Destroy();

        AdSize adaptiveSize;

        //横画面でバナーのロードをリクエスト
        adaptiveSize = AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerView = new BannerView(bannerAdUnitId, adaptiveSize, AdPosition.Top);

        bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        bannerView.OnAdLoaded += (handler, EventArgs) =>
        {
            Debug.Log("バナーを読み込みました。");
        };


        AdRequest request = new AdRequest.Builder().Build(); //AddTestDevice(SystemInfo.deviceUniqueIdentifier)

        //if (Cash.preference.hasPurchased == true || Cash.preference.rewardWatched == true)
        //    return;

        bannerView.LoadAd(request);
    }


    public void HideBanner()
    {
        if (bannerView != null)
            bannerView.Hide();
    }

    public void ShowBanner()
    {
        //if (Cash.preference.hasPurchased == true || Cash.preference.rewardWatched == true)
        //    return;

        if (bannerView != null)
            bannerView.Show();
    }

    public void DestroyAds()
    {
        if (bannerView != null)
            bannerView.Destroy();
        if (interstitial != null)
            interstitial.Destroy();
    }


    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("バナーの読み込みに失敗しました。");

        MonoBehaviour.print(
                "HandleFailedToReceiveAd event received with message: " + args.LoadAdError.GetMessage().ToString());
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////




    public void ShowInterstitiaAd()
	{
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (interstitial != null)
                interstitial.Destroy();

            if (cash.gameDirector.isPlayingSecen == true)
            {
                if (cash.gameDirector.isCallAdsByNewGame == true)
                    cash.newGame.MakeNewGameFromSetting();
                else if (cash.gameDirector.isCallAdsByNewGame == false)
                    cash.replay.OnMouseUp();
            }
            else if (cash.gameDirector.isPlayingSecen == false)
                cash.gamesWon.CallChangeGamesWonNumAsAnimation();
         
			return;
        }

        if (interstitial != null && interstitial.IsLoaded())
        {
#if UNITY_EDITOR
            if (cash.gameDirector.isPlayingSecen == true)
                {
                    if(cash.gameDirector.isCallAdsByNewGame == true)
                        cash.newGame.MakeNewGameFromSetting();
                    else if(cash.gameDirector.isCallAdsByNewGame == false)
                        cash.replay.OnMouseUp();
                }
            else if (cash.gameDirector.isPlayingSecen == false)
                cash.gamesWon.CallChangeGamesWonNumAsAnimation();
#else 
            interstitial.Show();
           

#endif

        }
        else 
		{
            if (cash.gameDirector.isPlayingSecen == true)
            {
                if (cash.gameDirector.isCallAdsByNewGame == true)
                    cash.newGame.MakeNewGameFromSetting();
                else if (cash.gameDirector.isCallAdsByNewGame == false)
                    cash.replay.OnMouseUp();
            }
            else if (cash.gameDirector.isPlayingSecen == false)
                cash.gamesWon.CallChangeGamesWonNumAsAnimation();
         
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
                if (interstitial != null)
                    interstitial.Destroy ();
				RequestiIterstitial ();
			}
		}
    }


    public void RequestiIterstitial()
    {
        if (cash.setting.preferenceDate.isPurchased == true ||
            cash.setting.preferenceDate.isRewarded == true)
        {
            return;
        }


        interstitial = new InterstitialAd(iterstitialAdUnitId);
        AdRequest request = new AdRequest.Builder().Build();

        interstitial.OnAdFailedToLoad += (handler, EventArgs) =>
        {
            if (interstitial != null)
                interstitial.Destroy();

            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                RequestiIterstitial();
        };

        interstitial.OnAdClosed += (handler, EventArgs) =>
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (cash.gameDirector.isPlayingSecen == true)
                {
                    if(cash.gameDirector.isCallAdsByNewGame == true)
                        cash.newGame.MakeNewGameFromSetting();
                    else if(cash.gameDirector.isCallAdsByNewGame == false)
                        cash.replay.OnMouseUp();
                }
                else if (cash.gameDirector.isPlayingSecen == false)
                    cash.gamesWon.CallChangeGamesWonNumAsAnimation();
                
                if (interstitial != null)
                    interstitial.Destroy();
                return;
            }

            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                if (interstitial != null)
                    interstitial.Destroy();
                RequestiIterstitial();

                if (cash.gameDirector.isPlayingSecen == true)
                {
                    if (cash.gameDirector.isCallAdsByNewGame == true)
                        cash.newGame.MakeNewGameFromSetting();
                    else if (cash.gameDirector.isCallAdsByNewGame == false)
                        cash.replay.OnMouseUp();
                }
                else if (cash.gameDirector.isPlayingSecen == false)
                    cash.gamesWon.CallChangeGamesWonNumAsAnimation();
            }
        };


        //広告上に表示されているAppのインストース画面に行ったときに実行される
        //本アプリに戻ったときは、interstitial.OnAdClosedが実行され、広告が閉じる
        //interstitial. += (handler, EventArgs) =>
        //{
        //    if (cash.gameDirector.isPlayingSecen == true)
        //    {
        //        //WebAnalytics.InterstitialAd(WebAnalytics.touched_interstitial_game);
        //    }
        //    else if (cash.gameDirector.isPlayingSecen == false)
        //    {
        //        //WebAnalytics.InterstitialAd(WebAnalytics.touched_interstitial_clear);
        //    }
        //};

        //広告を表示したとき
        //interstitial.OnAdOpening += (handler, EventArgs) =>
        //{
        //    if (cash.gameDirector.isPlayingSecen == true)
        //    {
        //        //WebAnalytics.InterstitialAd(WebAnalytics.shown_interstitial_game);
        //    }
        //    else if (cash.gameDirector.isPlayingSecen == false)
        //    {
        //        //WebAnalytics.InterstitialAd(WebAnalytics.shown_interstitial_clear);
        //    }
        //};


        interstitial.LoadAd(request);
    }


}