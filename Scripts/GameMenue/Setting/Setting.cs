using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Setting : MonoBehaviour {


	public PreferenceData preferenceDate;


	public Sprite backSprite;
	public Sprite playmatSprite;

	Color color;

	GameObject settingPanel;
	GameObject settingMenu_Obj;
    Cash cash;


	public bool isShowingSetting = false;

	Vector2 movedPos = new Vector2 (0, 1500f);

	public int cardBackDesign = 1;


	public void InitilaizeSettingAllMember()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		settingMenu_Obj = GameObject.Find ("settingMenu");
		settingPanel = GameObject.Find ("settingPanel");

		//flip one
		Text flipOne = GameObject.Find("flipOne").transform.GetChild(0).GetComponent<Text>();
		flipOne.text = L.Text.FromKey("flip one");

		//flip three
		Text flipThree = GameObject.Find("flipThree").transform.GetChild(0).GetComponent<Text>();
		flipThree.text = L.Text.FromKey("flip three");

		//left hand
		Text leftHand = GameObject.Find("leftHand").transform.GetChild(0).GetComponent<Text>();
		leftHand.text = L.Text.FromKey("left hand");

		//right hand
		Text rightHand = GameObject.Find("rightHand").transform.GetChild(0).GetComponent<Text>();
		rightHand.text = L.Text.FromKey("right hand");

		//how to play
		Text ruleSetting = GameObject.Find("ruleSetting").transform.GetChild(0).GetComponent<Text>();
		ruleSetting.text = L.Text.FromKey("how to play");

		Text statisticsInSetting = GameObject.Find("statisticsInSetting").transform.GetChild(0).GetComponent<Text>();
		statisticsInSetting.text = L.Text.FromKey("statistics");



		Text paymentServiceAct = GameObject.Find("paymentServiceAct").transform.GetChild(0).GetComponent<Text>();
        paymentServiceAct.text = L.Text.FromKey("payment service act");

        Text privacyPolicy = GameObject.Find("privacyPolicy").transform.GetChild(0).GetComponent<Text>();
        privacyPolicy.text = L.Text.FromKey("privacy policy");

        Text purchasePolicy = GameObject.Find("purchasePolicy").transform.GetChild(0).GetComponent<Text>();
        purchasePolicy.text = L.Text.FromKey("purchase policy");

        Text actOnSpecifiedCommercialTransaction = GameObject.Find("actOnSpecifiedCommercialTransaction").transform.GetChild(0).GetComponent<Text>();
        actOnSpecifiedCommercialTransaction.text = L.Text.FromKey("act on specified commercial transaction");

        TransparentEverySetting();
	}


	public void SetBoolInfoOfPreferenceDate(string name, bool result)
	{
		if(name == "soundSetting")
            preferenceDate.isSounding = result;
		if(name == "timeSetting")
			preferenceDate.isShowingTimeAndMove = result;
		if(name == "isRightHand")
			preferenceDate.isRightHand = result;
		if(name == "isOneFlipChecked")
			preferenceDate.isOneFlipChecked = result;

		cash.saveManager.SaveFile_PreferenceDate();
	}

	public void SetIntInfoOfPreferenceDate(string name, int result)
	{
		if(name == "backCrad")
			preferenceDate.backCardDesignNum = result;
		if(name == "playmat")
			preferenceDate.playmatDesignNum = result;

		cash.saveManager.SaveFile_PreferenceDate();
	}



	public void OnMouseUp()
	{
        cash.sound.GetButtonSound();
		GameObject.Find("playingMenu").transform.SetSiblingIndex(4);
		

		if (isShowingSetting == false) {
            if (cash.dealCard.didDeal == false)
            {
                GameObject.Find("generateAndDealCards").GetComponent<BoxCollider2D>().enabled = false;
                cash.arrowController.StopInstractionCard();
            }
			if (cash.dealCard.didDeal == true) {
				//playingScoreを止める
				cash.scoreText_Playing.countingTime = false; //timeScoreを止める
				//カードに触れられなくする
				cash.gameDirector.BeFalseOrTrueOpenCards_Retu(false);
				cash.gameDirector.BeFalseOrTrueOpenCards_Yama (false, true);
            }
			ShowUpEveryScoreSetting ();
		}
        //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.Setting);

    }


    public void CloseSetting()
	{
		if (isShowingSetting == true) {
            if (cash.dealCard.didDeal == false)
            {
                GameObject.Find("generateAndDealCards").GetComponent<BoxCollider2D>().enabled = true;
                cash.arrowController.MoveInstractionCard();
            }
			if (cash.dealCard.didDeal == true) {
				cash.scoreText_Playing.countingTime = true; 
				cash.gameDirector.BeFalseOrTrueOpenCards_Retu(true);
				cash.gameDirector.BeFalseOrTrueOpenCards_Yama (true, true);
            }
            cash.sound.GetSelectContentSound();
            TransparentEverySetting ();
		}
	}


	//GameDirectorクラスに、すべての表側のカードのBoxColliderをfalseにするメソッドを作る

	public void ShowUpEveryScoreSetting()
	{
		settingPanel.transform.SetAsLastSibling();
		settingPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
		ChangeGameObjectColor (settingMenu_Obj, 255f);

		foreach (Transform child in settingMenu_Obj.transform) {
			if (child.transform.tag == "buttonAndImage") {
				ChangeTransformColor (child, 255f);
				foreach (Transform graundChild in child.transform)
					if(child.GetComponent<settingContent>().isChecked == true)
					    ChangeTransformColor (graundChild, 255f);
			    	else if(child.GetComponent<settingContent>().isChecked == false)
				    	ChangeTransformColor (graundChild, 0f);
			}
			if (child.transform.tag == "button")
				ChangeTransformColor (child, 255f);
		}

		isShowingSetting = true;
	}



	public void TransparentEverySetting()
	{
		settingPanel.GetComponent<RectTransform> ().anchoredPosition = new Vector3(0, -1500f, 0);
		ChangeGameObjectColor (settingMenu_Obj, 0f);

		foreach (Transform child in settingMenu_Obj.transform) {
			if (child.transform.tag == "buttonAndImage") {
				ChangeTransformColor (child, 0f);
				foreach (Transform graundChild in child.transform)
					ChangeTransformColor (graundChild, 0f);
			}
			if (child.transform.tag == "button")
				ChangeTransformColor (child, 0f);
		}

		isShowingSetting = false;
	}



	void ChangeTransformColor(Transform obj, float numOfAColor)
	{
		if (obj.name == ("Text"))
        {
            color = obj.GetComponent<Text>().color;
            color.a = 255f;
            obj.GetComponent<Text>().color = color;
            return;
		}
		color = obj.GetComponent<Image> ().color;
		color.a = numOfAColor;
		obj.GetComponent<Image> ().color = color;
	}

	void ChangeGameObjectColor(GameObject obj, float numOfAColor)
	{
		if (obj.name == ("Text"))
		{
            color = obj.GetComponent<Text>().color;
            color.a = 255f;
            obj.GetComponent<Text>().color = color;
            return;
		}

		color = obj.GetComponent<Image> ().color;
		color.a = numOfAColor;
		obj.GetComponent<Image> ().color = color;
	}



    public void ChangePreferenceAtLoad()
    {
        InitilaizeSettingAllMember();

        settingContent soundSetting = GameObject.Find("soundSetting").GetComponent<settingContent>();
        settingContent timeSetting = GameObject.Find("timeSetting").GetComponent<settingContent>();
        settingContent flipOne = GameObject.Find("flipOne").GetComponent<settingContent>();
        settingContent flipThree = GameObject.Find("flipThree").GetComponent<settingContent>();
        settingContent leftHand = GameObject.Find("leftHand").GetComponent<settingContent>();
        settingContent rightHand = GameObject.Find("rightHand").GetComponent<settingContent>();
        settingContent ruleSetting = GameObject.Find("ruleSetting").GetComponent<settingContent>();
        settingContent cancelSetting = GameObject.Find("cancelSetting").GetComponent<settingContent>();
        ChangeFlipAmount_Home changeFlipAmount_Home = GameObject.Find("homeMenu").GetComponent<ChangeFlipAmount_Home>();
        TouchingDeck touchingDeck = GameObject.Find("row8").GetComponent<TouchingDeck>();
        Sound sound = GameObject.Find("gameDirectorObject").GetComponent<Sound>();
        soundSetting.InitilaizeSettingContentAllMember();
        timeSetting.InitilaizeSettingContentAllMember();
        flipOne.InitilaizeSettingContentAllMember();
        flipThree.InitilaizeSettingContentAllMember();
        leftHand.InitilaizeSettingContentAllMember();
        rightHand.InitilaizeSettingContentAllMember();
        ruleSetting.InitilaizeSettingContentAllMember();
        cancelSetting.InitilaizeSettingContentAllMember();
        changeFlipAmount_Home.InitilaizeChangeFlipAmount_HomeAllMember();
        touchingDeck.InitilaizeTouchingDeckAllMember();
        sound.InitilaizeSoundAllMember();
        ScoreText_Playing playingScore = GameObject.Find("playingScore").GetComponent<ScoreText_Playing>();
        playingScore.Initialize();

        soundSetting.SoundOrSilence(preferenceDate.isSounding);
        soundSetting.isChecked = !preferenceDate.isSounding; //isCheckedが変更されす前の値が入っているため

        timeSetting.RemoveTimeAndMove(preferenceDate.isShowingTimeAndMove);
        timeSetting.isChecked = !preferenceDate.isShowingTimeAndMove; //isCheckedが変更されす前の値が入っているため


        if (preferenceDate.isRightHand == false)
        {
            rightHand.isChecked = false;
            leftHand.isChecked = true;
            leftHand.ChangePlayStyleToLeftHand(); //どのSettingContentクラスを使っても大丈夫
        }
        else if (preferenceDate.isRightHand == true)
        {
            rightHand.isChecked = true;
            leftHand.isChecked = false;

            leftHand.ChangePlayStyleToRightHand();
        }


        if (preferenceDate.isOneFlipChecked == false)
        {
            //          flipThree.OnMouseUp ();
            flipOne.isChecked = false;
            flipThree.isChecked = true;                       //Play中に変更する場合は、settingContentクラスから変更する
            changeFlipAmount_Home.ChangeFlipButtonToThree(); //Loadするときにめくる枚数を変更するときは、changeFlipAmount_Homeクラスが持つメソッドで変更する
			GameObject.Find("flipOne").GetComponent<Image>().sprite
  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_L_Off");
			GameObject.Find("flipThree").GetComponent<Image>().sprite
	  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_R_On");

		}
		if (preferenceDate.isOneFlipChecked == true)
        {
            //          flipOne.OnMouseUp ();
            flipOne.isChecked = true;
            flipThree.isChecked = false;
			GameObject.Find("flipThree").GetComponent<Image>().sprite
  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_R_Off");
			GameObject.Find("flipOne").GetComponent<Image>().sprite
	  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_L_On");

		}

	}


	public void ChangeCardBackDesignAtLoad()
	{
		switch (preferenceDate.backCardDesignNum) {
		case 1:
                backSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite> ("back" + "_" + "1");
			break;
		case 2:
                backSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite> ("back" + "_" + "2");
			break;
		case 3:
                backSprite = cash.assetResourceManager.assetBundle.LoadAsset <Sprite > ("back" + "_" + "3");
			break;
		case 4:
                backSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite> ("back" + "_" + "4");
			break;
		}

		if(preferenceDate.backCardDesignNum > 4)
        {
			backSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("back_" + preferenceDate.backCardDesignNum.ToString());
		}

		SpriteRenderer generateAndDealCardsSprite = GameObject.Find ("generateAndDealCards").GetComponent<SpriteRenderer> ();
		generateAndDealCardsSprite.sprite = backSprite;
	}


	public void ChangePlaymatDesignAtLoad()
	{
		switch (preferenceDate.playmatDesignNum) {
		case 1:
                playmatSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite> ("playmat_1");
			break;
		case 2:
                playmatSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite> ("playmat_2");
			break;
		case 3:
                playmatSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite> ("playmat_3");
			break;
		case 4:
                playmatSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("playmat_4");
			break;
		}

		if (preferenceDate.playmatDesignNum > 4)
		{
			playmatSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("playmat_" + preferenceDate.playmatDesignNum.ToString()) ;
		}
		Debug.Log("load design = "+ "playmat_" + preferenceDate.playmatDesignNum.ToString());
		Debug.Log(playmatSprite);

		cash.playmat_Obj.GetComponent<SpriteRenderer> ().sprite = playmatSprite;
	}


	public void StartOrStopNoAdsByRewardedAtLoadApp()
    {

    }



	//時刻の代入用
	DateTime dt;
	TimeSpan dt2;
	string dt3;
	//秒数表示しないなら不要
	double dt4;

	bool isNoAdsTimeSaved = false;

	


 //   private void OnApplicationFocus(bool pause)
 //   {
	//	if (pause)//一時停止から再開時
	//	{
	//		Debug.Log("一時停止から再開");
	//		if (isNoAdsTimeSaved == true)
	//		{
	//			isNoAdsTimeSaved = false;

	//			RewardedIsTrueWhenCloseAppBefore();
				
	//		}
	//	}
	//	else//一時停止時
	//	{
	//		Debug.Log("一時停止");
	//		if (isNoAdsTimeSaved == false)
	//		{
	//			isNoAdsTimeSaved = true;

	//			if (preferenceDate.isRewarded == true)
	//			{
	//				NoAdsMinLeftController noAdsMinLeftController = GameObject.Find("noAdsMinLeft").GetComponent<NoAdsMinLeftController>();

	//				dt = DateTime.Now;
	//				preferenceDate.lastTime = dt.ToString();
	//				preferenceDate.noAdsCountdownResult = noAdsMinLeftController.countdownResult;
	//				cash.saveManager.SaveFile_PreferenceDate();
	//			}
	//		}

	//	}
	//}


	////アプリ終了時に呼び出されるメソッド
	//void OnApplicationQuit()
	//{
	//	Debug.Log("アプリを終了しました。");

	//	//preferenceDate.isRewardedがTrueだったら時刻を保存する
	//	if (isNoAdsTimeSaved == false)
	//	{
	//		if (preferenceDate.isRewarded == true)
	//		{
	//			NoAdsMinLeftController noAdsMinLeftController = GameObject.Find("noAdsMinLeft").GetComponent<NoAdsMinLeftController>();

	//			dt = DateTime.Now;
	//			preferenceDate.lastTime = dt.ToString();
	//			preferenceDate.noAdsCountdownResult = noAdsMinLeftController.countdownResult;
	//			cash.saveManager.SaveFile_PreferenceDate();
	//		}
	//	}


	//}


	//public void RewardedIsTrueWhenCloseAppBefore()
 //   {
	//	//保存した時刻から経過時間を計算し、30分経っていたらFalse
	//	//まだ経っていなかったらTrueにする

	//	if (preferenceDate.isRewarded == true)
	//	{
	//		Debug.Log("preferenceDate.isRewarded == true");
	//		NoAdsMinLeftController noAdsMinLeftController = GameObject.Find("noAdsMinLeft").GetComponent<NoAdsMinLeftController>();

	//		//保存した文字列設定の終了時刻をStringで取得
	//		dt3 = preferenceDate.lastTime;
	//		if (dt3 == null)
	//		{
	//			dt3 = DateTime.Now.ToString();
	//		}

	//		//String→DateTime→TimeSpan→doubleに変換
	//		dt = DateTime.Parse(dt3);
	//		dt2 = DateTime.Now - dt;
	//		//秒数表示しないなら不要
	//		dt4 = dt2.TotalSeconds;
	//		float pastSecFormCloseAppToOpenApp = (float)dt4;
	//		float leftNoAdsTime = preferenceDate.noAdsCountdownResult;
	//		//再起動時の広告なし時間の残り時間
	//		float a = leftNoAdsTime - pastSecFormCloseAppToOpenApp;

	//		//秒数表示しないなら「"経過時間は" + dt2 + "秒です";」に差し替え
	//		Debug.Log("経過時間は、" + pastSecFormCloseAppToOpenApp.ToString() + "　秒です");


	//		if (a <= 0f)
 //           {
	//			preferenceDate.isRewarded = false;
	//			cash.saveManager.SaveFile_PreferenceDate();

 //           }
 //           else if(a >= 0f)
 //           {
	//			float newNoAdsTime = preferenceDate.noAdsCountdownResult - pastSecFormCloseAppToOpenApp;
	//			noAdsMinLeftController.StartNoAdsByReward(newNoAdsTime);
	//		}

	//	}

	//}



}
