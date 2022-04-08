using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class GameDirector : MonoBehaviour {


	SaveManager saveManager;
	public BoardData boardData;
	public Statistics.Data statisticsData;


	public List <GameObject> row1_List = new List<GameObject> ();
	public List <GameObject> row2_List = new List<GameObject> ();
	public List <GameObject> row3_List = new List<GameObject> ();
	public List <GameObject> row4_List = new List<GameObject> ();
	public List <GameObject> row5_List = new List<GameObject> ();
	public List <GameObject> row6_List = new List<GameObject> ();
	public List <GameObject> row7_List = new List<GameObject> ();
	public List <GameObject> row8_List = new List<GameObject> ();
	public List <GameObject> row9_List = new List<GameObject> ();
	public List <GameObject> row10_List = new List<GameObject> ();
	public List <GameObject> row11_List = new List<GameObject> ();
	public List <GameObject> row12_List = new List<GameObject> ();
	public List <GameObject> openDeck_List = new List<GameObject> ();

	public List <GameObject> movingCard = new List<GameObject> ();
	public List <GameObject> exOyaCard = new List<GameObject> ();
	public List<bool> willFlip = new List<bool> ();
	public List<int> amount_BackToRow8 = new List<int> ();



	Color color;
    Cash cash;


	Statistics.GameScore currentPlaytimeData;

	private GameObject obj_Empty;
	private GameObject oya;
	private List<GameObject> wantingList;
    private int indexNum;
	private int indexNum_Last;
    float rollingSpeed;

	public bool beSound; 
	public bool rightHand;

    public bool isScaned;
    public bool isUsedScan;
    public int scanUsedCount;

    public bool isPlayingSecen;
	public bool isCallAdsByNewGame = false;


	void InitilaizeAllMember() {
        beSound = true;
        rightHand = true;
        isScaned = false;
        isPlayingSecen = false;
        isUsedScan = false;
        scanUsedCount = 0;
        indexNum = -1;
        rollingSpeed = 0.2f;
        saveManager = gameObject.GetComponent<SaveManager> ();
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
    }

	void LoadData() 
	{
		boardData = saveManager.LoadFile_BoardDate ();
		statisticsData = saveManager.LoadFile_StatisticsDate ();
		cash.setting.preferenceDate = saveManager.LoadFile_PreferenceDate ();
	}


	void Awake()
	{
        
        InitilaizeAllMember();
        cash.Init();
        cash.assetResourceManager.Inti();
        L.Text.Init (Application.systemLanguage);
        LoadData();
        cash.panel.Initi();
        Input.multiTouchEnabled = false;
        cash.placePos.MakePosAndScale();
        saveManager.InitializeSaveManager ();

		SetImageToSomeObj();

        InitilizaPlayTimeScore ();
  		cash.gamesWon.initialize ();
        cash.rulesManager.Init();
		cash.showMassageBox.Initilaze ();
		TransparentRetuYamaRow8BackG ();
        cash.touchingDeck.InitilaizeTouchingDeckAllMember();
        cash.dealCard.Init();
        cash.setting.ChangePreferenceAtLoad ();
		cash.setting.ChangeCardBackDesignAtLoad ();
		//GameObject.Find("noAdsMinLeft").GetComponent<NoAdsMinLeftController>().InitNoAdsMinLeftController();
		cash.setting.ChangePlaymatDesignAtLoad ();
		//cash.setting.RewardedIsTrueWhenCloseAppBefore();
		SetTotalGamesWonNum ();
        cash.adsController.InitiAdmobAds();
        cash.dealCard.CreatCardsFromPrefab (cash.placePos.scaleCard, GameObject.Find("generateAndDealCards").transform.position);
		ChangePlayedScoreAndPlayingScoreText();

	//cash.setting.preferenceDate.isPurchased = false;
	//cash.saveManager.SaveFile_PreferenceDate();


}


void ChangePlayedScoreAndPlayingScoreText()
    {
		GameObject.Find("timeText").GetComponent<Text>().text = L.Text.FromKey("time");
		GameObject.Find("moveText").GetComponent<Text>().text = L.Text.FromKey("move");
		GameObject.Find("scoreText").GetComponent<Text>().text = L.Text.FromKey("score");
		GameObject.Find("totalScoretext").GetComponent<Text>().text = L.Text.FromKey("total score");
		GameObject.Find("thisGameText").GetComponent<Text>().text = L.Text.FromKey("this game");
		GameObject.Find("theBestText").GetComponent<Text>().text = L.Text.FromKey("the best");

        GameObject.Find("move").GetComponent<Text>().text = L.Text.FromKey("move");
        GameObject.Find("score").GetComponent<Text>().text = L.Text.FromKey("score");

    }



	void SetImageToSomeObj()
    {
		
        GameObject.Find("gameTitle").GetComponent<Image>().sprite 
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("title");
        GameObject.Find("arrow").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("hand_arrow");
        GameObject.Find("hand").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("hand");
        //GameObject.Find("3FlipButton").GetComponent<Image>().sprite
        //          = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("flip_3");
        //GameObject.Find("1FlipButton").GetComponent<Image>().sprite
        //          = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("flip_1");
        //GameObject.Find("newGameHome").GetComponent<Image>().sprite
        //          = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("game_btn_newgame");

        //GameObject.Find("designMenu").GetComponent<Image>().sprite
        //          = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("design_BackG");

		for(int i = 1; i <= 16; i++)
        {
			GameObject.Find("BC"+i).GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("back_"+i);

		}

		for (int i = 1; i <= 10; i++)
		{
			GameObject.Find("BG"+i).GetComponent<Image>().sprite
					  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("playmat_"+i);
		}

		for (int i = 1; i <= 4; i++)
		{
			GameObject.Find("star" + i).GetComponent<Image>().sprite
					  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("star");
		}




		GameObject.Find("game").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("game");
        GameObject.Find("design").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("design");
        GameObject.Find("hint").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("hint");
        GameObject.Find("undo").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("undo");
        GameObject.Find("back").GetComponent<Image>().sprite
                  //= cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("backToHome");
				  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("backToHomeImg");
		GameObject.Find("setting").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting");

        GameObject.Find("settingMenu").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_bg");
		//GameObject.Find("flipOne").GetComponent<Image>().sprite
		//          = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_flip_one");
		GameObject.Find("flipOne").GetComponent<Image>().sprite
				  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_L_On");
		//GameObject.Find("flipOneCheckmark").GetComponent<Image>().sprite
  //                = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_check");
		//GameObject.Find("flipThree").GetComponent<Image>().sprite
		//          = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_flip_three");
		GameObject.Find("flipThree").GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_R_On");
		//GameObject.Find("flipThreeCheckmark").GetComponent<Image>().sprite
  //                = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_check");
		//GameObject.Find("leftHand").GetComponent<Image>().sprite
		//          = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_left_hand");
		GameObject.Find("leftHand").GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_L_On");

		//GameObject.Find("leftHandCheckmark").GetComponent<Image>().sprite
  //                = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_check");
		//GameObject.Find("rightHand").GetComponent<Image>().sprite
		//          = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_right_hand");
		GameObject.Find("rightHand").GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_R_On");

		//GameObject.Find("rightHandCheckmark").GetComponent<Image>().sprite
  //                = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_check");
        GameObject.Find("soundSetting").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_sound_on");
        GameObject.Find("timeSetting").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_timer_on");
        GameObject.Find("ruleSetting").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_howtoP");
		GameObject.Find("statisticsInSetting").GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_howtoP");

		GameObject.Find("cancelSetting").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_quit_btn");
        GameObject.Find("Image").GetComponent<Image>().sprite
                  //= cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_bg");
				  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("gameBG");

		GameObject.Find("autoComplete").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("complate_btn");

        GameObject.Find("backGOfGame").GetComponent<Image>().sprite
                  //= cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("game_btn_bg");
				  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("gameBG");
		GameObject.Find("replay").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("gameB");
		GameObject.Find("newGame").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("gameB");
		GameObject.Find("replayT").GetComponent<Text>().text = L.Text.FromKey("replay");
		GameObject.Find("gameT").GetComponent<Text>().text = L.Text.FromKey("newgame");


		GameObject.Find("row1").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("retuMaterial");
        GameObject.Find("row2").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("retuMaterial");
        GameObject.Find("row3").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("retuMaterial");
        GameObject.Find("row4").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("retuMaterial");
        GameObject.Find("row5").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("retuMaterial");
        GameObject.Find("row6").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("retuMaterial");
        GameObject.Find("row7").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("retuMaterial");
        GameObject.Find("row8").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("row8Material");
        
        GameObject.Find("row9").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("yamaMaterial");
        GameObject.Find("row10").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("yamaMaterial");
        GameObject.Find("row11").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("yamaMaterial");
        GameObject.Find("row12").GetComponent<SpriteRenderer>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("yamaMaterial");

        GameObject.Find("ruleTab").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_howtoP");
		GameObject.Find("cancelRules").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("statistics_quit_btn");
        GameObject.Find("finishClearedScore").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("OK");
        GameObject.Find("gamesWonBackG").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("clear_bg");

		//GameObject.Find("rewardInGameT").GetComponent<Text>().text = L.Text.FromKey("reward button word");
		//GameObject.Find("rewardInSettingT").GetComponent<Text>().text = L.Text.FromKey("reward button word");
		//GameObject.Find("purchaseInGameT").GetComponent<Text>().text = L.Text.FromKey("purchase button word");
		//GameObject.Find("purchaseInSettingT").GetComponent<Text>().text = L.Text.FromKey("purchase button word");

		//GameObject.Find("rewardInGame").GetComponent<Image>().sprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("rewardInGame");
		//GameObject.Find("rewardInSetting").GetComponent<Image>().sprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("rewardInGame");
		//GameObject.Find("purchaseInGame").GetComponent<Image>().sprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("purchaseInGame");
		//GameObject.Find("purchaseInSetting").GetComponent<Image>().sprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("purchaseInGame");
		//GameObject.Find("noAdsMinLeft").GetComponent<Image>().sprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("noAdsMinLeft");

		
	}



	void InitilizaPlayTimeScore()
	{
		Statistics.TypeOfGame typeOfGame = Statistics.GameScore.getTypeOfGame (true, true);
		currentPlaytimeData = statisticsData.DataFromType (typeOfGame);
	}



    public void AddScanedCount()
    {
        boardData.scanedCount++;
    }

    public int GetScanedCount()
    {
        return boardData.scanedCount;
    }



    public bool GetIsFirstTimeAsOpeningApp()
    {
		return boardData.isFirstTimeAsOpeningApp;
    }

    public bool GetIsFirstTimeAsOpeningScan()
    {
        return boardData.isFirstTimeAsOpeningScan;
    }

    public void BeFalseIsFirstTimeAsOpeningApp()
    {
        boardData.isFirstTimeAsOpeningApp = false;
        saveManager.SaveFile_BoardDate();
    }

    public void BeFalseIsFirstTimeAsOpeningScan()
    {
        boardData.isFirstTimeAsOpeningScan = false;
        saveManager.SaveFile_BoardDate();
    }



    public bool GetBoolIsReviewRequested()
	{
		return boardData.isReviewRequested;
	}

	public void ChangeBoolIsReviewRequested(bool _isReviewRequested)
	{
		boardData.isReviewRequested = _isReviewRequested;
        saveManager.SaveFile_BoardDate();
	}


    public void ChangeBoolIsNoAds(bool _isNoAds)
    {
        boardData.isNoAds = _isNoAds;
        saveManager.SaveFile_BoardDate();
    }


    public bool GetBoolIsNoAds()
    {
        return boardData.isNoAds;
    }




    public int GetRemainingScan()
	{
        int remainingScan = 0;
        remainingScan = boardData.remaningScan;
        return remainingScan;
	}



	public void AddRemainingScan(int addScanNum)
	{
		boardData.remaningScan += addScanNum;
        saveManager.SaveFile_BoardDate();
    }



	public void AddPlaytimeStatistics()
	{
		float playtime = cash.scoreText_Playing.time_Second + cash.scoreText_Playing.time_Minute * 60f;
		currentPlaytimeData.playtime += playtime;
	}


    public float GetTotalGamesWonNum()
    {
        float totalGamesWonNum = 0f;
        bool isScandFlag = false;
        bool flipAmoutIs1Flag = true;
        Statistics.TypeOfGame typeOfGame;

        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
                isScandFlag = false;
            if (i == 1)
                isScandFlag = true;
            for (int k = 0; k < 2; k++)
            {
                if (k == 0)
                    flipAmoutIs1Flag = true;
                if (k == 1)
                    flipAmoutIs1Flag = false;

                typeOfGame = Statistics.GameScore.getTypeOfGame(isScandFlag, flipAmoutIs1Flag);
                Statistics.GameScore currentData = statisticsData.DataFromType(typeOfGame);
                totalGamesWonNum += currentData.gamesWonCount;
            }
        }

        return totalGamesWonNum;
    }


    void SetTotalGamesWonNum()
	{
        float totalGamesWonNum = GetTotalGamesWonNum();
        cash.gamesWon.MakeGamesWonAtHomeScreen(totalGamesWonNum);
    }


    public void AddToWonPoint(float point){
        boardData.wonPointToPresent += point;
        saveManager.SaveFile_BoardDate();
    }

    public void ResetWonPoint(){
        boardData.wonPointToPresent = 0f;
        saveManager.SaveFile_BoardDate();
    } 

    public float GetWonPoint(){
        return boardData.wonPointToPresent;
    }




    List<float> upperSideStatisticsList = new List<float> ();
	public List<float> GetListUpperSideStatistics(bool isthreeFlipScore, bool isScsnScore)
	{
		if (upperSideStatisticsList.Count != 0)
			upperSideStatisticsList.Clear ();
		
		Statistics.TypeOfGame typeOfGame = Statistics.GameScore.getTypeOfGame(isScsnScore, isthreeFlipScore);
		Statistics.GameScore currentData = statisticsData.DataFromType(typeOfGame);
		upperSideStatisticsList.Add(currentData.bestTime);
		upperSideStatisticsList.Add(currentData.bestMove);
		upperSideStatisticsList.Add(currentData.bestScore);
		upperSideStatisticsList.Add(currentData.bestTotalScore);
		upperSideStatisticsList.Add(currentData.gamesWonCount);
		upperSideStatisticsList.Add(currentData.gamesCount);

		return upperSideStatisticsList;
	}


	List<float> belowSideStatisticsList = new List<float> ();
	public List<float> GetListBelowSideStatistics()
	{
		if (belowSideStatisticsList.Count > 0)
			belowSideStatisticsList.Clear ();

		float totalGamesCountNum = 0f;
		float totalPlayTimeNum = currentPlaytimeData.playtime;
		bool isScandFlag = false;
		bool flipAmoutIs1Flag = true;
		Statistics.TypeOfGame typeOfGame;

		for (int i = 0; i < 2; i++) {
			if (i == 0)
				isScandFlag = false;
			if (i == 1)
				isScandFlag = true;
			for (int k = 0; k < 2; k++) {
				if (k == 0)
					flipAmoutIs1Flag = true;
				if (k == 1)
					flipAmoutIs1Flag = false;

				typeOfGame = Statistics.GameScore.getTypeOfGame (isScandFlag, flipAmoutIs1Flag);
				Statistics.GameScore currentData = statisticsData.DataFromType (typeOfGame);
				totalGamesCountNum += currentData.gamesCount;
				totalPlayTimeNum += currentData.playtime;
			}
		}
		belowSideStatisticsList.Add(totalGamesCountNum);
		belowSideStatisticsList.Add(totalPlayTimeNum);

		return belowSideStatisticsList;
	}


    public int GetTotalGamesCount()
    {
        List<float> list = GetListBelowSideStatistics();
        float totalGamesCount = list[0];
        return (int)totalGamesCount;
    }







	public GameObject GetOya()
	{
		return oya;
	}


	public bool CheckExistOya(GameObject touched_Click)
	{
		bool accepted = false;
		int row1_LastIndex = GetIndexOfLastObj (row1_List);
		int row2_LastIndex = GetIndexOfLastObj (row2_List);
		int row3_LastIndex = GetIndexOfLastObj (row3_List);
		int row4_LastIndex = GetIndexOfLastObj (row4_List);
		int row5_LastIndex = GetIndexOfLastObj (row5_List);
		int row6_LastIndex = GetIndexOfLastObj (row6_List);
		int row7_LastIndex = GetIndexOfLastObj (row7_List);
		int row9_LastIndex = GetIndexOfLastObj (row9_List);
		int row10_LastIndex = GetIndexOfLastObj (row10_List);
		int row11_LastIndex = GetIndexOfLastObj (row11_List);
		int row12_LastIndex = GetIndexOfLastObj (row12_List);

		if (row12_LastIndex == -1) { 
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row12_Obj);
			if (accepted == true) oya = cash.row12_Obj;
		} if(row12_LastIndex != -1) { 
			accepted = AcceptedToBeChild_2 (touched_Click, row12_List [row12_LastIndex]);
			if(accepted == true) oya = row12_List [row12_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row11_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row11_Obj);
			if (accepted == true) oya = cash.row11_Obj; 
		} if(row11_LastIndex != -1) {
			accepted = AcceptedToBeChild_2 (touched_Click, row11_List [row11_LastIndex]);
			if(accepted == true) oya = row11_List [row11_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}		
		if (row10_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row10_Obj);
			if (accepted == true) oya = cash.row10_Obj; 
		} if(row10_LastIndex != -1) {
			accepted = AcceptedToBeChild_2 (touched_Click, row10_List [row10_LastIndex]);
			if(accepted == true) oya = row10_List [row10_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row9_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row9_Obj);
			if (accepted == true) oya = cash.row9_Obj; 
		} if(row9_LastIndex != -1) {
			accepted = AcceptedToBeChild_2 (touched_Click, row9_List [row9_LastIndex]);
			if(accepted == true) oya = row9_List [row9_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row1_LastIndex == -1){
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row1_Obj);
			if (accepted == true) oya = cash.row1_Obj; 
		} 
		if(row1_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (touched_Click, row1_List [row1_LastIndex]);
			if(accepted == true) oya = row1_List [row1_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}		
		if (row2_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row2_Obj);
			if (accepted == true) oya = cash.row2_Obj; 
		}
		if(row2_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (touched_Click, row2_List [row2_LastIndex]);
			if(accepted == true) oya = row2_List [row2_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row3_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row3_Obj);
			if (accepted == true) oya = cash.row3_Obj; 
		} if(row3_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (touched_Click, row3_List [row3_LastIndex]);
			if(accepted == true) oya = row3_List [row3_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row4_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row4_Obj);
			if (accepted == true) oya = cash.row4_Obj; 
		} if(row4_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (touched_Click, row4_List [row4_LastIndex]);
			if(accepted == true) oya = row4_List [row4_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row5_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row5_Obj);
			if (accepted == true) oya = cash.row5_Obj; 
		} if(row5_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (touched_Click, row5_List [row5_LastIndex]);
			if(accepted == true) oya = row5_List [row5_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row6_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row6_Obj);
			if (accepted == true) oya = cash.row6_Obj; 
		} if(row6_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (touched_Click, row6_List [row6_LastIndex]);
			if(accepted == true) oya = row6_List [row6_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row7_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (touched_Click, cash.row7_Obj);
			if (accepted == true) oya = cash.row7_Obj; 
		} if(row7_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (touched_Click, row7_List [row7_LastIndex]);
			if(accepted == true) oya = row7_List [row7_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		return accepted;
	}




	public bool CheckExistOya_1(GameObject willMove_Obj)
	{
		bool accepted = false;
		int row9_LastIndex = GetIndexOfLastObj (row9_List);
		int row10_LastIndex = GetIndexOfLastObj (row10_List);
		int row11_LastIndex = GetIndexOfLastObj (row11_List);
		int row12_LastIndex = GetIndexOfLastObj (row12_List);

		if (row12_LastIndex == -1) { 
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row12_Obj);
			if (accepted == true)
				oya = cash.row12_Obj;
		}
		if (row12_LastIndex != -1) { 
			accepted = AcceptedToBeChild_2 (willMove_Obj, row12_List [row12_LastIndex]);
			if (accepted == true)
				oya = row12_List [row12_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}

		if (row11_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row11_Obj);
			if (accepted == true) oya = cash.row11_Obj; 
		} if(row11_LastIndex != -1) {
			accepted = AcceptedToBeChild_2 (willMove_Obj, row11_List [row11_LastIndex]);
			if(accepted == true) oya = row11_List [row11_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}		
		if (row10_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row10_Obj);
			if (accepted == true) oya = cash.row10_Obj; 
		} if(row10_LastIndex != -1) {
			accepted = AcceptedToBeChild_2 (willMove_Obj, row10_List [row10_LastIndex]);
			if(accepted == true) oya = row10_List [row10_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row9_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row9_Obj);
			if (accepted == true) oya = cash.row9_Obj; 
		} if(row9_LastIndex != -1) {
			accepted = AcceptedToBeChild_2 (willMove_Obj, row9_List [row9_LastIndex]);
			if(accepted == true) oya = row9_List [row9_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		return accepted;
	}




	public bool CheckExistOya_2(GameObject willMove_Obj)
	{
		bool accepted = false;
		int row1_LastIndex = GetIndexOfLastObj (row1_List);
		int row2_LastIndex = GetIndexOfLastObj (row2_List);
		int row3_LastIndex = GetIndexOfLastObj (row3_List);
		int row4_LastIndex = GetIndexOfLastObj (row4_List);
		int row5_LastIndex = GetIndexOfLastObj (row5_List);
		int row6_LastIndex = GetIndexOfLastObj (row6_List);
		int row7_LastIndex = GetIndexOfLastObj (row7_List);

		if (row1_LastIndex == -1){
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row1_Obj);
			if (accepted == true) oya = cash.row1_Obj; 
		} 
		if(row1_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (willMove_Obj, row1_List [row1_LastIndex]);
			if(accepted == true) oya = row1_List [row1_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}		
		if (row2_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row2_Obj);
			if (accepted == true) oya = cash.row2_Obj; 
		}
		if(row2_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (willMove_Obj, row2_List [row2_LastIndex]);
			if(accepted == true) oya = row2_List [row2_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row3_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row3_Obj);
			if (accepted == true) oya = cash.row3_Obj; 
		} if(row3_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (willMove_Obj, row3_List [row3_LastIndex]);
			if(accepted == true) oya = row3_List [row3_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row4_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row4_Obj);
			if (accepted == true) oya = cash.row4_Obj; 
		} if(row4_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (willMove_Obj, row4_List [row4_LastIndex]);
			if(accepted == true) oya = row4_List [row4_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row5_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row5_Obj);
			if (accepted == true) oya = cash.row5_Obj; 
		} if(row5_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (willMove_Obj, row5_List [row5_LastIndex]);
			if(accepted == true) oya = row5_List [row5_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row6_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row6_Obj);
			if (accepted == true) oya = cash.row6_Obj; 
		} if(row6_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (willMove_Obj, row6_List [row6_LastIndex]);
			if(accepted == true) oya = row6_List [row6_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		if (row7_LastIndex == -1) {
			accepted = AcceptedToBeChild_3 (willMove_Obj, cash.row7_Obj);
			if (accepted == true) oya = cash.row7_Obj; 
		} if(row7_LastIndex != -1) {
			accepted = AcceptedToBeChild_1 (willMove_Obj, row7_List [row7_LastIndex]);
			if(accepted == true) oya = row7_List [row7_LastIndex];
		}
		if (accepted == true) {
			return accepted;
		}
		return accepted;
	}






	public bool AcceptedToBeChild_1(GameObject touched_Click_Pass, GameObject lastOfList_Pass)
	{
		bool accepted = false;

		GameObject touched_Click = touched_Click_Pass; 
		GameObject lastOfList = lastOfList_Pass;

		Card card_C_Touched_Click = touched_Click.GetComponent<Card> ();
		Card card_C_LastOfList = lastOfList.GetComponent<Card> ();

		bool isBlack_Touched_Click = card_C_Touched_Click.GetBlackOrRed ();
		bool isBlack_LastOfList = card_C_LastOfList.GetBlackOrRed ();

		int num_Touched_Click = card_C_Touched_Click.numOfCard; 
		int num_LastOfList = card_C_LastOfList.numOfCard; 

		if (isBlack_Touched_Click != isBlack_LastOfList && num_Touched_Click == num_LastOfList - 1)
		{
			accepted = true;
		} else 
		{
			accepted = false;
		}

		return accepted;
	}





	public bool AcceptedToBeChild_2(GameObject touched_Click_Pass, GameObject lastOfList_Pass)
	{
		bool accepted = false;

		GameObject touched_Click = touched_Click_Pass;
		GameObject lastOfList = lastOfList_Pass;

		Card card_C_Touched_Click = touched_Click.GetComponent<Card> ();
		Card card_C_LastOfList = lastOfList.GetComponent<Card> ();

		int isSuit_Touched_Click = card_C_Touched_Click.suit; 
		int isSuit_LastOfList = card_C_LastOfList.suit; 

		int num_Touched_Click = card_C_Touched_Click.numOfCard;
		int num_LastOfList = card_C_LastOfList.numOfCard; 

		int indexNum_Touched_Click = -1;
		int lastIndexNum_Touched_Click = -1;

		if (touched_Click.tag != "deck") {
			indexNum_Touched_Click = GetIndexNum (touched_Click);
			lastIndexNum_Touched_Click = GetLastIndexNum (touched_Click);
		}

		if (isSuit_Touched_Click == isSuit_LastOfList && num_Touched_Click - 1 == num_LastOfList && indexNum_Touched_Click == lastIndexNum_Touched_Click)
		{
			accepted = true;
		} else 
		{
			accepted = false;
		}

		return accepted;
	}





	public bool AcceptedToBeChild_3(GameObject touched_Click_Pass, GameObject emptyLine_Pass)
	{
		bool accepted = false;

		GameObject touched_Click = touched_Click_Pass; 
		Card card_C_Touched_Click = touched_Click.GetComponent<Card> ();
		int num_Touched_Click = card_C_Touched_Click.numOfCard; 

		if (num_Touched_Click == 13 && emptyLine_Pass.transform.name == "row1") accepted = true;
		if (num_Touched_Click == 13 && emptyLine_Pass.transform.name == "row2") accepted = true;
		if (num_Touched_Click == 13 && emptyLine_Pass.transform.name == "row3") accepted = true;
		if (num_Touched_Click == 13 && emptyLine_Pass.transform.name == "row4") accepted = true;
		if (num_Touched_Click == 13 && emptyLine_Pass.transform.name == "row5") accepted = true;
		if (num_Touched_Click == 13 && emptyLine_Pass.transform.name == "row6") accepted = true;
		if (num_Touched_Click == 13 && emptyLine_Pass.transform.name == "row7") accepted = true;

		if (num_Touched_Click == 1 && emptyLine_Pass.transform.name == "row12") accepted = true;
		if (num_Touched_Click == 1 && emptyLine_Pass.transform.name == "row11") accepted = true;
		if (num_Touched_Click == 1 && emptyLine_Pass.transform.name == "row10") accepted = true;
		if (num_Touched_Click == 1 && emptyLine_Pass.transform.name == "row9") accepted = true;

		return accepted;
	}


	public void TransparentRetuYamaRow8BackG()
	{
        GameObject cardBackG = null;
		for (int i = 1; i <= 12; i++) {
			cardBackG = GetCardBackG (i);
            color = cardBackG.GetComponent<SpriteRenderer> ().color;
			color.a = 0f;
			cardBackG.GetComponent<SpriteRenderer> ().color = color;
		}
	}

	public void ShowUpRetuYamaRow8BackG()
	{
        GameObject cardBackG = null;
		for (int i = 1; i <= 12; i++) {
			cardBackG = GetCardBackG (i);
            color = cardBackG.GetComponent<SpriteRenderer> ().color;
			color.a = 255f;
			cardBackG.GetComponent<SpriteRenderer> ().color = color;
		}
	}


	GameObject GetCardBackG(int num)
	{
        GameObject target = null;

        switch (num) {
		case 1:
			target = cash.row1_Obj;
			break;
		case 2:
			target = cash.row2_Obj;
			break;
		case 3:
			target = cash.row3_Obj;
			break;
		case 4:
			target = cash.row4_Obj;
			break;
		case 5:
			target = cash.row5_Obj;
			break;
		case 6:
			target = cash.row6_Obj;
			break;
		case 7:
			target = cash.row7_Obj;
			break;
		case 8:
			target = cash.row8_Obj;
			break;
		case 9:
			target = cash.row9_Obj;
			break;
		case 10:
			target = cash.row10_Obj;
			break;
		case 11:
			target = cash.row11_Obj;
			break;
		case 12:
			target = cash.row12_Obj;
			break;
		}
		return target;
	} 



	public int GetIndexNum(GameObject obj)
	{
		int existence_row1 = row1_List.IndexOf(obj);
		int existence_row2 = row2_List.IndexOf(obj);
		int existence_row3 = row3_List.IndexOf(obj);
		int existence_row4 = row4_List.IndexOf(obj);
		int existence_row5 = row5_List.IndexOf(obj);
		int existence_row6 = row6_List.IndexOf(obj);
		int existence_row7 = row7_List.IndexOf(obj);
		int existence_row8 = row8_List.IndexOf(obj);
		int existence_row8_1 = openDeck_List.IndexOf(obj);
		int existence_row9 = row9_List.IndexOf(obj);
		int existence_row10 = row10_List.IndexOf(obj);
		int existence_row11 = row11_List.IndexOf(obj);
		int existence_row12 = row12_List.IndexOf(obj);

		if (existence_row1 != -1) indexNum = existence_row1;
		if (existence_row2 != -1) indexNum = existence_row2;
		if (existence_row3 != -1) indexNum = existence_row3;
		if (existence_row4 != -1) indexNum = existence_row4;
		if (existence_row5 != -1) indexNum = existence_row5;
		if (existence_row6 != -1) indexNum = existence_row6;
		if (existence_row7 != -1) indexNum = existence_row7;
		if (existence_row8 != -1) indexNum = existence_row8;
		if (existence_row8_1 != -1) indexNum = existence_row8_1;
		if (existence_row9 != -1) indexNum = existence_row9;
		if (existence_row10 != -1) indexNum = existence_row10;
		if (existence_row11 != -1) indexNum = existence_row11;
		if (existence_row12 != -1) indexNum = existence_row12;

		return indexNum;
	}




	public Vector3 GetOridinalPos(List<GameObject> belongList, int indexNum, GameObject emptyObj)
	{
		Vector3 oridinalPos = Vector3.zero;
		int openDeckAmount = openDeck_List.Count;
	
		if (emptyObj == cash.row8_1Obj && openDeckAmount == 1) 
			oridinalPos = GameObject.Find ("row8_1").transform.position;
		else if (emptyObj == cash.row8_1Obj && openDeckAmount == 2)
			oridinalPos = GameObject.Find ("row8_2").transform.position;
		else if (emptyObj == cash.row8_1Obj && openDeckAmount >= 3)
			oridinalPos = GameObject.Find ("row8_3").transform.position;
		else if (belongList == row9_List)
			oridinalPos = cash.row9_Obj.transform.position;
		else if (belongList == row10_List)
			oridinalPos = cash.row10_Obj.transform.position;
		else if (belongList == row11_List)
			oridinalPos = cash.row11_Obj.transform.position;
		else if (belongList == row12_List)
			oridinalPos = cash.row12_Obj.transform.position;
		else {
			float oridinalYPos = 0;

			if (indexNum == 0)
				oridinalYPos = emptyObj.transform.position.y;
			if (indexNum > 0) {
				GameObject upperObj = belongList [indexNum - 1];
				bool isUpperObjTurned = upperObj.GetComponent<Card> ().isTurned;
				if (isUpperObjTurned == false)
					oridinalYPos = upperObj.transform.position.y - cash.placePos.intervalBackCards;
				if (isUpperObjTurned == true)
					oridinalYPos = upperObj.transform.position.y - cash.placePos.intervalFrontCards;
				
			}
			oridinalYPos = GetFloorFloat (oridinalYPos);
			oridinalPos = new Vector3 (emptyObj.transform.position.x, oridinalYPos, 0);
		}

		return oridinalPos;
	}


	public GameObject GetLastObjOfList(GameObject obj)
	{
		int lastIndexNum = GetLastIndexNum (obj);
		List<GameObject> list = GetListOfObj (obj);
		GameObject lastObj = list[lastIndexNum];

		return lastObj;
	}





	public int GetLastIndexNum(GameObject obj)
	{
		int existence_row1 = row1_List.IndexOf(obj);
		int existence_row2 = row2_List.IndexOf(obj);
		int existence_row3 = row3_List.IndexOf(obj);
		int existence_row4 = row4_List.IndexOf(obj);
		int existence_row5 = row5_List.IndexOf(obj);
		int existence_row6 = row6_List.IndexOf(obj);
		int existence_row7 = row7_List.IndexOf(obj);
		int existence_row8 = row8_List.IndexOf(obj);
		int existence_row9 = row9_List.IndexOf(obj);
		int existence_row10 = row10_List.IndexOf(obj);
		int existence_row11 = row11_List.IndexOf(obj);
		int existence_row12 = row12_List.IndexOf(obj);
		int existence_row13 = openDeck_List.IndexOf(obj);


		if (existence_row1 != -1) indexNum_Last = GetIndexOfLastObj(row1_List);
		if (existence_row2 != -1) indexNum_Last = GetIndexOfLastObj(row2_List);
		if (existence_row3 != -1) indexNum_Last = GetIndexOfLastObj(row3_List);
		if (existence_row4 != -1) indexNum_Last = GetIndexOfLastObj(row4_List);
		if (existence_row5 != -1) indexNum_Last = GetIndexOfLastObj(row5_List);
		if (existence_row6 != -1) indexNum_Last = GetIndexOfLastObj(row6_List);
		if (existence_row7 != -1) indexNum_Last = GetIndexOfLastObj(row7_List);
		if (existence_row8 != -1) indexNum_Last = GetIndexOfLastObj(row8_List);
		if (existence_row9 != -1) indexNum_Last = GetIndexOfLastObj(row9_List);
		if (existence_row10 != -1) indexNum_Last = GetIndexOfLastObj(row10_List);
		if (existence_row11 != -1) indexNum_Last = GetIndexOfLastObj(row11_List);
		if (existence_row12 != -1) indexNum_Last = GetIndexOfLastObj(row12_List);
		if (existence_row13 != -1) indexNum_Last = GetIndexOfLastObj(openDeck_List);

		return indexNum_Last;
	}



	public List<GameObject> GetListOfObj(GameObject obj)
	{

		int existence_row1 = row1_List.IndexOf(obj);
		int existence_row2 = row2_List.IndexOf(obj);
		int existence_row3 = row3_List.IndexOf(obj);
		int existence_row4 = row4_List.IndexOf(obj);
		int existence_row5 = row5_List.IndexOf(obj);
		int existence_row6 = row6_List.IndexOf(obj);
		int existence_row7 = row7_List.IndexOf(obj);
		int existence_row8 = row8_List.IndexOf(obj);
		int existence_row9 = row9_List.IndexOf(obj);
		int existence_row10 = row10_List.IndexOf(obj);
		int existence_row11 = row11_List.IndexOf(obj);
		int existence_row12 = row12_List.IndexOf(obj);
		int existence_row13 = openDeck_List.IndexOf(obj);

		if (existence_row1 != -1) wantingList = row1_List;
		if (existence_row2 != -1) wantingList = row2_List;
		if (existence_row3 != -1) wantingList = row3_List;
		if (existence_row4 != -1) wantingList = row4_List;
		if (existence_row5 != -1) wantingList = row5_List;
		if (existence_row6 != -1) wantingList = row6_List;
		if (existence_row7 != -1) wantingList = row7_List;
		if (existence_row8 != -1) wantingList = row8_List;
		if (existence_row9 != -1) wantingList = row9_List;
		if (existence_row10 != -1) wantingList = row10_List;
		if (existence_row11 != -1) wantingList = row11_List;
		if (existence_row12 != -1) wantingList = row12_List;
		if (existence_row13 != -1) wantingList = openDeck_List;

		return wantingList;

	}



	public List<GameObject> GetListOfObj_1 (string oyaName)
	{
		if (oyaName == "row1") wantingList = row1_List;
		if (oyaName == "row2") wantingList = row2_List;
		if (oyaName == "row3") wantingList = row3_List;
		if (oyaName == "row4") wantingList = row4_List;
		if (oyaName == "row5") wantingList = row5_List;
		if (oyaName == "row6") wantingList = row6_List;
		if (oyaName == "row7") wantingList = row7_List;
		if (oyaName == "row9") wantingList = row9_List;
		if (oyaName == "row10") wantingList = row10_List;
		if (oyaName == "row11") wantingList = row11_List;
		if (oyaName == "row12") wantingList = row12_List;

		return wantingList;

	}




	public GameObject GetEmptyObj(GameObject obj)
	{
		int existence_row1 = row1_List.IndexOf(obj);
		int existence_row2 = row2_List.IndexOf(obj);
		int existence_row3 = row3_List.IndexOf(obj);
		int existence_row4 = row4_List.IndexOf(obj);
		int existence_row5 = row5_List.IndexOf(obj);
		int existence_row6 = row6_List.IndexOf(obj);
		int existence_row7 = row7_List.IndexOf(obj);
		int existence_row8 = row8_List.IndexOf(obj);
		int existence_row8_1 = openDeck_List.IndexOf(obj);
		int existence_row9 = row9_List.IndexOf(obj);
		int existence_row10 = row10_List.IndexOf(obj);
		int existence_row11 = row11_List.IndexOf(obj);
		int existence_row12 = row12_List.IndexOf(obj);


		if (existence_row1 != -1) obj_Empty = cash.row1_Obj;
		if (existence_row2 != -1) obj_Empty = cash.row2_Obj;
		if (existence_row3 != -1) obj_Empty = cash.row3_Obj;
		if (existence_row4 != -1) obj_Empty = cash.row4_Obj;
		if (existence_row5 != -1) obj_Empty = cash.row5_Obj;
		if (existence_row6 != -1) obj_Empty = cash.row6_Obj;
		if (existence_row7 != -1) obj_Empty = cash.row7_Obj;
		if (existence_row8 != -1) obj_Empty = cash.row8_Obj;
		if (existence_row8_1 != -1) obj_Empty = cash.row8_1Obj;
		if (existence_row9 != -1) obj_Empty = cash.row9_Obj;
		if (existence_row10 != -1) obj_Empty = cash.row10_Obj;
		if (existence_row11 != -1) obj_Empty = cash.row11_Obj;
		if (existence_row12 != -1) obj_Empty = cash.row12_Obj;

		return obj_Empty;
	}



	public static int GetIndexOfLastObj<T>(List<T> list)
	{
		int lastIndexNum = list.Count - 1;
		return lastIndexNum;
	}



	public static T GetLastCardOfList<T>(List<T> list)
	{
		int lastIndexNum = GetIndexOfLastObj (list);
		T lastCard = list [lastIndexNum];

		return lastCard;
	}


	public void BeFalseBoxcolli(string oyaName)
	{
		if (oyaName == "row1") cash.row1_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row2") cash.row2_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row3") cash.row3_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row4") cash.row4_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row5") cash.row5_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row6") cash.row6_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row7") cash.row7_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row9") cash.row9_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row10") cash.row10_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row11") cash.row11_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		if (oyaName == "row12") cash.row12_Obj.GetComponent<BoxCollider2D> ().enabled = false;
	}



	GameObject lastCard;
	public GameObject GetLastCardInList(int numOfList)
	{
		if (numOfList == 1) {
			if(row1_List.Count != 0)
				lastCard = row1_List [row1_List.Count - 1];
			else if (row1_List.Count == 0)
				lastCard = null;
		}

		if (numOfList == 2) {
			if(row2_List.Count != 0)
				lastCard = row2_List [row2_List.Count - 1];
			else if (row2_List.Count == 0)
				lastCard = null;
		}

		if (numOfList == 3) {
			if(row3_List.Count != 0)
				lastCard = row3_List [row3_List.Count - 1];
			else if (row3_List.Count == 0)
				lastCard = null;
		}

		if (numOfList == 4) {
			if(row4_List.Count != 0)
				lastCard = row4_List [row4_List.Count - 1];
			else if (row4_List.Count == 0)
				lastCard = null;
		}

		if (numOfList == 5) {
			if(row5_List.Count != 0)
				lastCard = row5_List [row5_List.Count - 1];
			else if (row5_List.Count == 0)
				lastCard = null;
		}

		if (numOfList == 6) {
			if(row6_List.Count != 0)
				lastCard = row6_List [row6_List.Count - 1];
			else if (row6_List.Count == 0)
				lastCard = null;
		}

		if (numOfList == 7) {
			if(row7_List.Count != 0)
				lastCard = row7_List [row7_List.Count - 1];
			else if (row7_List.Count == 0)
				lastCard = null;
		}

		if (numOfList == 8) {
			if (openDeck_List.Count != 0)
				lastCard = openDeck_List [openDeck_List.Count - 1];
			else if (openDeck_List.Count == 0)
				lastCard = null;
		}

		return lastCard;
	}




	GameObject lastCard_1;
	public GameObject GetLastCardInList_1(int numOfList)
	{
		bool isTurned = false;

		if (numOfList == 1) {
			if (row1_List.Count != 0) {
				for (int i = 0; i < row1_List.Count; i++) {
					isTurned = row1_List [i].GetComponent<Card> ().isTurned;
					if (isTurned == true) {
						lastCard_1 = row1_List [i];
						break;
					}
				}
			}
			else if (row1_List.Count == 0)
				lastCard_1 = null;
		}

		if (numOfList == 2) {
			if (row2_List.Count != 0) {
				for (int i = 0; i < row2_List.Count; i++) {
					isTurned = row2_List [i].GetComponent<Card> ().isTurned;
					if (isTurned == true) {
						lastCard_1 = row2_List [i];
						break;
					}
				}
			}
			else if (row2_List.Count == 0)
				lastCard_1 = null;
		}

		if (numOfList == 3) {
			if (row3_List.Count != 0) {
				for (int i = 0; i < row3_List.Count; i++) {
					isTurned = row3_List [i].GetComponent<Card> ().isTurned;
					if (isTurned == true) {
						lastCard_1 = row3_List [i];
						break;
					}
				}
			}
			else if (row3_List.Count == 0)
				lastCard_1 = null;
		}

		if (numOfList == 4) {
			if (row4_List.Count != 0) {
				for (int i = 0; i < row4_List.Count; i++) {
					isTurned = row4_List [i].GetComponent<Card> ().isTurned;
					if (isTurned == true) {
						lastCard_1 = row4_List [i];
						break;
					}
				}
			}
			else if (row4_List.Count == 0)
				lastCard_1 = null;
		}

		if (numOfList == 5) {
			if (row5_List.Count != 0) {
				for (int i = 0; i < row5_List.Count; i++) {
					isTurned = row5_List [i].GetComponent<Card> ().isTurned;
					if (isTurned == true) {
						lastCard_1 = row5_List [i];
						break;
					}
				}
			}
			else if (row5_List.Count == 0)
				lastCard_1 = null;
		}

		if (numOfList == 6) {
			if (row6_List.Count != 0) {
				for (int i = 0; i < row6_List.Count; i++) {
					isTurned = row6_List [i].GetComponent<Card> ().isTurned;
					if (isTurned == true) {
						lastCard_1 = row6_List [i];
						break;
					}
				}
			}
			else if (row6_List.Count == 0)
				lastCard_1 = null;
		}

		if (numOfList == 7) {
			if (row7_List.Count != 0) {
				for (int i = 0; i < row7_List.Count; i++) {
					isTurned = row7_List [i].GetComponent<Card> ().isTurned;
					if (isTurned == true) {
						lastCard_1 = row7_List [i];
						break;
					}
				}
			}
			else if (row7_List.Count == 0)
				lastCard_1 = null;
		}

		if (numOfList == 8) {
			if (openDeck_List.Count != 0)
				lastCard_1 = openDeck_List [openDeck_List.Count - 1];
			else if (openDeck_List.Count == 0)
				lastCard_1 = null;
		}

		isTurned = false;

		return lastCard_1;
	}




	public int GetNumOfAmountChilds(GameObject oya)
	{
		int childsAmount = 0;

		int index_Oya = GetIndexNum (oya);
		int lastIndex_Oya = GetLastIndexNum (oya);
		childsAmount = lastIndex_Oya - index_Oya;

		return childsAmount;
	}
		




	public void AutomaticComplete_Call()
	{
		StartCoroutine ("AutomaticComplete");
	}



	GameObject willBeYama_Aut;
	IEnumerator AutomaticComplete()
	{
		SetLastYamaCardsBoxColliFalse (); 

		int amountOfAllOfRetuCards = GetAmountOfAllOfRetuCards ();
		bool canBeAccepted = false;

		for (int i = 0; i < amountOfAllOfRetuCards; i++) {
			canBeAccepted = GetACardWillBeYamaAuto (); 

			if (canBeAccepted == true) {
				MoveCardToYamaAutomatic (willBeYama_Aut, oya);
                cash.sound.GetDealCardSound();

            }
			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (0.2f);
		CheckAmountOfAllOfYamaIsForEnding ();
	}



	void SetLastYamaCardsBoxColliFalse()
	{
		row9_List [row9_List.Count - 1].GetComponent<BoxCollider2D> ().enabled = false;
		row10_List [row10_List.Count - 1].GetComponent<BoxCollider2D> ().enabled = false;
		row11_List [row11_List.Count - 1].GetComponent<BoxCollider2D> ().enabled = false;
		row12_List [row12_List.Count - 1].GetComponent<BoxCollider2D> ().enabled = false;
	}



    public bool isAutoCompShown = false;
    bool isUiMovedByClear = false;

	public void ShowAutomaticCompleteObj()
	{
		bool stillExistCard = CheckExistDeckCardAndBackRetuCard ();

		if (stillExistCard == true)
			return;

        if (isAutoCompShown == true)
            return;

        if (stillExistCard == false && isAutoCompShown == false) {
            cash.autoCom.ShowUpAutoCompleteObj ();
            cash.showBotton.UiMoving_PlayToClear();
            isUiMovedByClear = true;
            cash.dealCard.didDeal = false;
            isAutoCompShown = true;
		}
	}


    public bool GetIsRetuCard(GameObject obj)
    {
        List<GameObject> list = GetListOfObj(obj);
        bool result = false;

        if(list == row1_List)
            result = true;
        if (list == row2_List)
            result = true;
        if (list == row3_List)
            result = true;
        if (list == row4_List)
            result = true;
        if (list == row5_List)
            result = true;
        if (list == row6_List)
            result = true;
        if (list == row7_List)
            result = true;

        return result;
    }



    bool GetACardWillBeYamaAuto()
	{
		bool flag = false;

		if (row1_List.Count != 0) {
			willBeYama_Aut = row1_List [row1_List.Count - 1];
			flag = CheckExistOya_1 (willBeYama_Aut);
			if (flag == true)
				return flag;
		}
		if (row2_List.Count != 0) {
			willBeYama_Aut = row2_List [row2_List.Count - 1];
			flag = CheckExistOya_1 (willBeYama_Aut);
			if (flag == true)
				return flag;
		}
		if (row3_List.Count != 0) {
			willBeYama_Aut = row3_List [row3_List.Count - 1];
			flag = CheckExistOya_1 (willBeYama_Aut);
			if (flag == true)
				return flag;
		}
		if (row4_List.Count != 0) {
			willBeYama_Aut = row4_List [row4_List.Count - 1];
			flag = CheckExistOya_1 (willBeYama_Aut);
			if (flag == true)
				return flag;
		}
		if (row5_List.Count != 0) {
			willBeYama_Aut = row5_List [row5_List.Count - 1];
			flag = CheckExistOya_1 (willBeYama_Aut);
			if (flag == true)
				return flag;
		}
		if (row6_List.Count != 0) {
			willBeYama_Aut = row6_List [row6_List.Count - 1];
			flag = CheckExistOya_1 (willBeYama_Aut);
			if (flag == true)
				return flag;
		}
		if (row7_List.Count != 0) {
			willBeYama_Aut = row7_List [row7_List.Count - 1];
			flag = CheckExistOya_1 (willBeYama_Aut);
			if (flag == true)
				return flag;
		}

		return flag;
	}





	void MoveCardToYamaAutomatic(GameObject child, GameObject oya)
	{
		GameObject oya_Empty = GetEmptyObj (oya);
		Vector3 endPos = oya_Empty.transform.position;
		List<GameObject> childList = GetListOfObj (child);
		List<GameObject> oyaList = GetListOfObj (oya);

		cash.scoreText_Playing.AddOneMoveText();
		cash.scoreText_Playing.ChangeScoreText(cash.scoreText_Playing.isToYama);

		child.GetComponent<SpriteRenderer> ().sortingOrder = 90;
		child.GetComponent<BoxCollider2D> ().enabled = false;
		child.GetComponent<CardsDirector>().SetInfoToAnimation (true, endPos, 0.3f, false);
		childList.Remove (child);
		oyaList.Add (child);
	}




	int GetAmountOfAllOfRetuCards()
	{
		int amount = row1_List.Count + row2_List.Count + row3_List.Count + row4_List.Count + row5_List.Count + row6_List.Count + row7_List.Count;
		return amount;
	}





	bool CheckExistDeckCardAndBackRetuCard()
	{
		bool stiiExsist = true;

        if (row9_List.Count == 0)
            return stiiExsist;
        if (row10_List.Count == 0)
            return stiiExsist;
        if (row11_List.Count == 0)
            return stiiExsist;
        if (row12_List.Count == 0)
            return stiiExsist;


        stiiExsist = CheckExistBackCardInRetu(row1_List);
		if (stiiExsist == false)
			stiiExsist = CheckExistBackCardInRetu (row2_List);
     		else return stiiExsist;
	    		if(stiiExsist == false) 
	        		stiiExsist = CheckExistBackCardInRetu(row3_List);
	           	else return stiiExsist;
	        		if(stiiExsist == false)
	         		stiiExsist = CheckExistBackCardInRetu(row4_List);
    	        	else return stiiExsist;
    	        		if(stiiExsist == false) 
	            		stiiExsist = CheckExistBackCardInRetu(row5_List);
                  		else return stiiExsist;
                 			if(stiiExsist == false) 
	                		stiiExsist = CheckExistBackCardInRetu(row6_List);
	                    	else return stiiExsist;
	                    		if(stiiExsist == false) 
	                    		stiiExsist = CheckExistBackCardInRetu(row7_List);
	                        	else return stiiExsist;
                        			if(stiiExsist == false) 
		                        	stiiExsist = CheckExistBackCardInDeck(row8_List);
                             		else return stiiExsist;
		                            	if(stiiExsist == false) 
		                            	stiiExsist = CheckExistBackCardInDeck(openDeck_List);
                             	    	else return stiiExsist;
		return stiiExsist; 
	}


	bool CheckExistBackCardInDeck(List<GameObject> list)
	{
		bool existDeck = true; 

		if (list.Count == 0)
			existDeck = false;
		
		return existDeck;
	}



	bool CheckExistBackCardInRetu(List<GameObject> list)
	{
		bool existBack = true; 

		if (list.Count == 0) {
			existBack = false;
			return existBack;
		}
		
		for (int i = 0; i < list.Count; i++) {
			bool isTurned = list [i].GetComponent<Card> ().isTurned;

			if (isTurned == true) 
				existBack = false;
			else if (isTurned == false) {
				existBack = true;
				return existBack;
			}
		}
		return existBack;
	}




	public void GatherEveryCardsToPlayNewGame(float timeToArrive_Pass)
	{
		GameObject gatherAndDealObj = GameObject.Find ("generateAndDealCards");
		Vector3 gatherPos = new Vector3 (gatherAndDealObj.transform.position.x, gatherAndDealObj.transform.position.y, gatherAndDealObj.transform.position.z);
        cash.sound.GetMoveCardToHomeSound();

        if (row1_List.Count > 0) {
			for (int i = 0; i < row1_List.Count; i++) {
				GameObject aCard = row1_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row2_List.Count > 0) {
			for (int i = 0; i < row2_List.Count; i++) {
				GameObject aCard = row2_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row3_List.Count > 0) {
			for (int i = 0; i < row3_List.Count; i++) {
				GameObject aCard = row3_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row4_List.Count > 0) {
			for (int i = 0; i < row4_List.Count; i++) {
				GameObject aCard = row4_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row5_List.Count > 0) {
			for (int i = 0; i < row5_List.Count; i++) {
				GameObject aCard = row5_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row6_List.Count > 0) {
			for (int i = 0; i < row6_List.Count; i++) {
				GameObject aCard = row6_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row7_List.Count > 0) {
			for (int i = 0; i < row7_List.Count; i++) {
				GameObject aCard = row7_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row8_List.Count > 0) {
			for (int i = 0; i < row8_List.Count; i++) {
				GameObject aCard = row8_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (openDeck_List.Count > 0) {
			for (int i = 0; i < openDeck_List.Count; i++) {
				GameObject aCard = openDeck_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row9_List.Count > 0) {
			for (int i = 0; i < row9_List.Count; i++) {
				GameObject aCard = row9_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row10_List.Count > 0) {
			for (int i = 0; i < row10_List.Count; i++) {
				GameObject aCard = row10_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row11_List.Count > 0) {
			for (int i = 0; i < row11_List.Count; i++) {
				GameObject aCard = row11_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
		if (row12_List.Count > 0) {
			for (int i = 0; i < row12_List.Count; i++) {
				GameObject aCard = row12_List [i];
				SetCardInMotionBoxCollider2DAndMove (aCard, gatherPos, timeToArrive_Pass);
			}
		}
	}


	void SetCardInMotionBoxCollider2DAndMove(GameObject card, Vector3 pos, float timeToArrive_Pass)
	{
        CardsDirector cd = card.GetComponent<CardsDirector>();

        if (card.GetComponent<Card> ().isTurned == true)
            cd.FlipToBack (true, rollingSpeed);
        if (cd.isChangedBoxSize == true)
            cd.ResetSizeBC2D();
        if (card.GetComponent<BoxCollider2D>().enabled == true)
            card.GetComponent<BoxCollider2D>().enabled = false;

        cd.SetInfoToAnimation (true, pos, timeToArrive_Pass, false);
	}



	public void DestroyEverCards()
	{
        var retu = GameObject.FindGameObjectsWithTag("retu");
        foreach (var clone in retu)
             Destroy(clone, 0.1f);

        var yama = GameObject.FindGameObjectsWithTag("yama");
        foreach (var clone in yama)
            Destroy(clone, 0.1f);

        GameObject.Find("row8").transform.tag = "deck_1";
        GameObject.Find("row8_1").transform.tag = "deck_1";
        GameObject.Find("row8_2").transform.tag = "deck_1";
        GameObject.Find("row8_3").transform.tag = "deck_1";

        var deck = GameObject.FindGameObjectsWithTag("deck");
        foreach (var clone in deck)
            Destroy(clone, 0.1f);

        GameObject.Find("row8").transform.tag = "deck";
        GameObject.Find("row8_1").transform.tag = "deck";
        GameObject.Find("row8_2").transform.tag = "deck";
        GameObject.Find("row8_3").transform.tag = "deck";

    }


    public List<GameObject> GetList(int rowNum)
	{
        List<GameObject> targetList_Call = null;

		switch(rowNum){
		case 1:
			targetList_Call = row1_List;
			break;
		case 2:
			targetList_Call = row2_List;
			break;
		case 3:
			targetList_Call = row3_List;
			break;
		case 4:
			targetList_Call = row4_List;
			break;
		case 5:
			targetList_Call = row5_List;
			break;
		case 6:
			targetList_Call = row6_List;
			break;
		case 7:
			targetList_Call = row7_List;
			break;
		case 8:
			targetList_Call = row8_List;
			break;
		case 9:
			targetList_Call = row9_List;
			break;
		case 10:
			targetList_Call = row10_List;
			break;
		case 11:
			targetList_Call = row11_List;
			break;
		case 12:
			targetList_Call = row12_List;
			break;
		case 13:
			targetList_Call = openDeck_List;
			break;
		}

		return targetList_Call;
	}



	public void ClearEveryLists()
	{
        if (row1_List.Count > 0)
            row1_List.Clear ();
        if (row2_List.Count > 0)
            row2_List.Clear ();
        if (row3_List.Count > 0)
            row3_List.Clear ();
        if (row4_List.Count > 0)
            row4_List.Clear ();
        if (row5_List.Count > 0)
            row5_List.Clear ();
        if (row6_List.Count > 0)
            row6_List.Clear ();
        if (row7_List.Count > 0)
            row7_List.Clear ();
        if (row8_List.Count > 0)
            row8_List.Clear ();
        if (row9_List.Count > 0)
            row9_List.Clear ();
        if (row10_List.Count > 0)
            row10_List.Clear ();
        if (row11_List.Count > 0)
            row11_List.Clear ();
        if (row12_List.Count > 0)
            row12_List.Clear ();
        if (openDeck_List.Count > 0)
            openDeck_List.Clear ();
	}



	public void CheckAmountOfAllOfYamaIsForEnding()
	{
		int allOfYamaAmount = row9_List.Count + row10_List.Count + row11_List.Count + row12_List.Count;

		if (allOfYamaAmount == 52) {
            float secondTimeAtThisGame = Mathf.Floor(cash.scoreText_Playing.time_Second);
            float minuteTimeAtThisGame = Mathf.Floor(cash.scoreText_Playing.time_Minute);
            float timeAtThisGame = minuteTimeAtThisGame * 60f + secondTimeAtThisGame;
            float moveAtThisGame = cash.scoreText_Playing.move;
            float bonusScoreAtThisGame = Mathf.Floor(700000f / timeAtThisGame);
            float scoreAtThisGame = cash.scoreText_Playing.score;
            float totalScoreAtThisGame = bonusScoreAtThisGame + scoreAtThisGame;
            Statistics.TypeOfGame typeOfGame = Statistics.GameScore.getTypeOfGame(isScaned, cash.touchingDeck.flipAmountIs1);

            Statistics.GameScore currentData = statisticsData.DataFromType(typeOfGame);

			currentData.bestTimePast = currentData.bestTime;
			currentData.bestMovePast = currentData.bestMove;
			currentData.bestScorePast = currentData.bestScore;
			currentData.bestTotalScorePast = currentData.bestTotalScore;


			if (currentData.bestTime == 0 || currentData.bestTime > timeAtThisGame)
            {
                currentData.bestTime = timeAtThisGame;
            }

            if (currentData.bestMove == 0 || currentData.bestMove > moveAtThisGame)
            {
                currentData.bestMove = moveAtThisGame;
            }

            if (currentData.bestScore < scoreAtThisGame)
            {
                currentData.bestScore = scoreAtThisGame;
            }

            if (currentData.bestTotalScore < totalScoreAtThisGame)
            {
                currentData.bestTotalScore = totalScoreAtThisGame;
            }
            currentData.gamesWonCount += 1f;
            saveManager.SaveFile_StatisticsDate();

            if(isUiMovedByClear == false)
                cash.showBotton.UiMoving_PlayToClear();


            float wonPoint = 0f;
            if (cash.touchingDeck.flipAmountIs1 == true)
                wonPoint = 0.34f;
            else
                wonPoint = 1.25f;

            AddToWonPoint(wonPoint);


            isUiMovedByClear = false;
            cash.scoreText_Playing.countingTime = false;
            BeFalseYamaCard();
            AddPlaytimeStatistics();
            cash.scoreText_Playing.TransparentEveryScoreText_Playing ();
			cash.dealCard.IsWhenBackToHome ();
            isPlayingSecen = false;
            if (cash.autoCom.isAutoComp == false)
                cash.autoCom.TransparentAutoCompleteObj();
            ShowBright ();
            //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.JustCleared);
        }
    }


    void BeFalseYamaCard()
    {
        row9_List[row9_List.Count - 1].GetComponent<BoxCollider2D>().enabled = false;
        row10_List[row10_List.Count - 1].GetComponent<BoxCollider2D>().enabled = false;
        row11_List[row11_List.Count - 1].GetComponent<BoxCollider2D>().enabled = false;
        row12_List[row12_List.Count - 1].GetComponent<BoxCollider2D>().enabled = false;
    }


    void ShowBright()
	{
		GameObject particle = GameObject.Find ("particle");
		float cardWidth = cash.placePos.cardWidth;
		float cardHeight = cash.placePos.cardHeight;
        float zPos = -50f;
		Vector3 pos = Vector3.zero;
		pos = new Vector3 (cash.row9_Obj.transform.position.x + cardWidth/5f, cash.row9_Obj.transform.position.y - cardWidth /2 - cardHeight/3, zPos);
		particle.GetComponent<ParticleManager> ().CreatBright(pos);

		pos = new Vector3 (cash.row9_Obj.transform.position.x + cardWidth/2f + cardWidth/2f, cash.row9_Obj.transform.position.y - cardHeight/10f, zPos);
		particle.GetComponent<ParticleManager> ().CreatBright(pos);

		pos = new Vector3 (cash.row9_Obj.transform.position.x, cash.row9_Obj.transform.position.y + cardHeight/2f + cardHeight/5f, zPos);
		particle.GetComponent<ParticleManager> ().CreatBright(pos);

		pos = new Vector3 (cash.row11_Obj.transform.position.x + cardWidth/2f, cash.row11_Obj.transform.position.y + cardHeight/2f + cardHeight/9f, zPos);
		particle.GetComponent<ParticleManager> ().CreatBright(pos);

		pos = new Vector3 (cash.row12_Obj.transform.position.x - cardWidth/3f, cash.row12_Obj.transform.position.y + cardHeight/6f, zPos);
		particle.GetComponent<ParticleManager> ().CreatBright(pos);

		pos = new Vector3 (cash.row11_Obj.transform.position.x - cardWidth/2f, cash.row11_Obj.transform.position.y - cardHeight/2f - cardHeight/3f, zPos);
		particle.GetComponent<ParticleManager> ().CreatBright(pos);

		pos = new Vector3 (cash.row10_Obj.transform.position.x + cardWidth/2f, cash.row10_Obj.transform.position.y, zPos);
		particle.GetComponent<ParticleManager> ().CreatBright(pos);

        cash.sound.GetShineSound();
    }


    int brightCount = 0;
	public void CallToMoveCardsForEnding()
	{
		brightCount++;
		if (brightCount < 7)
			return;

		MoveCardsForEnding ();
		brightCount = 0;
    }




	bool isReviewedAtThisGame = false;
    void MoveCardsForEnding()
	{
        TransparentRetuYamaRow8BackG();

		//クリアしてキランと光り、光が消えたらレビュー依頼
		//レビュー依頼するときかを調べ、そおなら依頼し、違うならホームへ戻す
		bool isTimeToRequestReview = cash.reviewController.GetIsTimeToRequestReview();//ReviewController.Instance.GetIsTimeToRequestReview();
		if (isTimeToRequestReview == true)
		{
			cash.reviewController.RequestReview();
			ChangeBoolIsReviewRequested(true);
			isReviewedAtThisGame = true;
		}

		for (int i = 0; i < 4; i++) {
			for (int k = 0; k < 13; k++) {
				if (i == 0) {
					row9_List [k].GetComponent <CardsDirector> ().MoveCardRandomlly ();
				}
				if (i == 1) {
					row10_List [k].GetComponent <CardsDirector> ().MoveCardRandomlly ();
				}
				if (i == 2) {
					row11_List [k].GetComponent <CardsDirector> ().MoveCardRandomlly ();
				}
				if (i == 3) {
					row12_List [k].GetComponent <CardsDirector> ().MoveCardRandomlly ();
				}
			}
		}
    }


	int calledCount = 0;
	public void CallToShowScores()
	{
		calledCount++;
		if (calledCount < 52)
			return;

        calledCount = 0;
        Replay replay = GameObject.Find("replay").GetComponent<Replay>();
        Undo undo = GameObject.Find("undo").GetComponent<Undo>();

#if !UNITY_EDITOR
		if (isReviewedAtThisGame == false)
		{
			AdsController adsCont = GameObject.Find("adController").GetComponent<AdsController>();
			adsCont.ShowInterstitiaAd();
		}
		isReviewedAtThisGame = false;
        replay.ClearReplayList();
        undo.ResetUndo();
        DestroyEverCards();
        ClearEveryLists();
		cash.scoreText_Playing.TransparentEveryScoreText_Playing ();
#else
		cash.gamesWon.CallChangeGamesWonNumAsAnimation();
        replay.ClearReplayList();
        undo.ResetUndo();
        DestroyEverCards();
        ClearEveryLists();
        cash.scoreText_Playing.TransparentEveryScoreText_Playing();
#endif
	}


   

    public void ProcessEndingAsScoring()
    {
        calledCount++;
        if (calledCount != GamesWon.showingGamesWonSprite.Count)
            return;

        calledCount = 0;
        ShowScores();
        ShowFireWorks();
        GameObject.Find("priseWords").GetComponent<PriseWords>().CallToShowPrise();
    }




    void ShowFireWorks()
	{
		GameObject fireWorks = GameObject.Find ("particle");
		fireWorks.GetComponent<ParticleManager> ().ShowFireWorks();
	}


	public void ShowScores()
	{
		float secondTimeAtThisGame = Mathf.Floor (cash.scoreText_Playing.time_Second); 
		float minuteTimeAtThisGame = Mathf.Floor (cash.scoreText_Playing.time_Minute);
		float timeAtThisGame = minuteTimeAtThisGame * 60f + secondTimeAtThisGame; 
		float moveAtThisGame = cash.scoreText_Playing.move;
		float bonusScoreAtThisGame = Mathf.Floor (700000f / timeAtThisGame);
		float scoreAtThisGame = cash.scoreText_Playing.score; 
		float totalScoreAtThisGame = bonusScoreAtThisGame + scoreAtThisGame; 

		Statistics.TypeOfGame typeOfGame = Statistics.GameScore.getTypeOfGame (isScaned, cash.touchingDeck.flipAmountIs1);

		Statistics.GameScore currentData = statisticsData.DataFromType (typeOfGame);

		//if (currentData.bestTime == 0 || currentData.bestTime > timeAtThisGame) {
		//	currentData.bestTime = timeAtThisGame;
		//}

		//if (currentData.bestMove == 0 || currentData.bestMove > moveAtThisGame) {
		//	currentData.bestMove = moveAtThisGame;
		//}

		//if (currentData.bestScore < scoreAtThisGame) {
		//	currentData.bestScore = scoreAtThisGame;
		//}

		//if (currentData.bestTotalScore < totalScoreAtThisGame) {
		//	currentData.bestTotalScore = totalScoreAtThisGame;
		//}

		//saveManager.SaveFile_StatisticsDate ();

		float timeBest = currentData.bestTimePast;
		float minuteTimeBest = Mathf.Floor (timeBest / 60f);
		float secondTimeBest = timeBest - minuteTimeBest * 60f;

		cash.scoreText_Played.ShowUpEveryScoreText_Played ();
        cash.scoreText_Played. MovePlayedScoreUp();

        cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfScore_ThisGame", scoreAtThisGame);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfMove_ThisGame", moveAtThisGame);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfMinuteTime_ThisGame", minuteTimeAtThisGame);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfSecondTime_ThisGame", secondTimeAtThisGame);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfTotal_ThisGame", totalScoreAtThisGame);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfTotal_TheBest", currentData.bestTotalScorePast);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfScore_TheBest", currentData.bestScorePast);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfMove_TheBest", currentData.bestMovePast);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfMinuteTime_TheBest", minuteTimeBest);
		cash.scoreText_Played.SetNum_ScoreTextPlayed ("numOfSecondTime_TheBest", secondTimeBest);

		float bestTime = currentData.bestTimePast;
		float thisGameTime = timeAtThisGame;
        if (bestTime > thisGameTime && bestTime != 0)
        {
			Color color = GameObject.Find("star1").GetComponent<Image>().color;
			color.a = 255f;
			GameObject.Find("star1").GetComponent<Image>().color = color;
		}

		float bestMove = currentData.bestMovePast;
		float thisGameMove = moveAtThisGame;
		if (bestMove > thisGameMove && bestMove != 0)
		{
			Color color = GameObject.Find("star2").GetComponent<Image>().color;
			color.a = 255f;
			GameObject.Find("star2").GetComponent<Image>().color = color;
		}

		float bestScore = currentData.bestScorePast;
		float thisGameScore = scoreAtThisGame;
		if (thisGameScore > bestScore && bestScore != 0)
		{
			Color color = GameObject.Find("star3").GetComponent<Image>().color;
			color.a = 255f;
			GameObject.Find("star3").GetComponent<Image>().color = color;
		}

		float bestTotalScore = currentData.bestTotalScorePast;
		float thisGameTotalScore = totalScoreAtThisGame;
		if (thisGameTotalScore > bestTotalScore && bestTotalScore != 0)
		{
			Color color = GameObject.Find("star4").GetComponent<Image>().color;
			color.a = 255f;
			GameObject.Find("star4").GetComponent<Image>().color = color;
		}


		isScaned = false;
        //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.ScoreCleared);

    }






    public void SetGamesCountValue(float valuePlusOrMinus)
	{
		Statistics.TypeOfGame typeOfGame = Statistics.GameScore.getTypeOfGame(isScaned, cash.touchingDeck.flipAmountIs1);

		Statistics.GameScore currentData = statisticsData.DataFromType (typeOfGame);
		currentData.gamesCount += valuePlusOrMinus;
        
		saveManager.SaveFile_StatisticsDate ();
	}





	public void MoveCardsToRightHand(bool rightHand)
	{
		if (rightHand == true) {
			cash.row8_Obj.transform.position = cash.placePos.row8Pos;
			cash.row8_1Obj.transform.position = cash.placePos.row8_1Pos;
			cash.row8_2Obj.transform.position = cash.placePos.row8_2Pos;
			cash.row8_3Obj.transform.position = cash.placePos.row8_3Pos;
			cash.row9_Obj.transform.position = cash.placePos.row9Pos;
			cash.row10_Obj.transform.position = cash.placePos.row10Pos;
			cash.row11_Obj.transform.position = cash.placePos.row11Pos;
			cash.row12_Obj.transform.position = cash.placePos.row12Pos;

		}
		else if (rightHand == false) {
			cash.row8_Obj.transform.position = cash.placePos.row8_LeftPos;
			cash.row8_1Obj.transform.position = cash.placePos.row8_1_LeftPos;
			cash.row8_2Obj.transform.position = cash.placePos.row8_2_LeftPos;
			cash.row8_3Obj.transform.position = cash.placePos.row8_3_LeftPos;
			cash.row9_Obj.transform.position = cash.placePos.row9_LeftPos;
			cash.row10_Obj.transform.position = cash.placePos.row10_LeftPos;
			cash.row11_Obj.transform.position = cash.placePos.row11_LeftPos;
			cash.row12_Obj.transform.position = cash.placePos.row12_LeftPos;
		}
  	}


    public void MoveallCardToRightSide(bool rightHand)
    {
        int amountDeck = row8_List.Count;
        int amountDeck_1 = openDeck_List.Count;
        int amountRow9 = row9_List.Count;
        int amountRow10 = row10_List.Count;
        int amountRow11 = row11_List.Count;
        int amountRow12 = row12_List.Count;
        Vector3 deck = new Vector3();
        Vector3 deck_1 = new Vector3();
        Vector3 row9 = new Vector3();
        Vector3 row10 = new Vector3();
        Vector3 row11 = new Vector3();
        Vector3 row12 = new Vector3();

        if (rightHand == true)
        {
            deck = cash.placePos.row8Pos;
            deck_1 = cash.placePos.row8_1Pos;
            row9 = cash.placePos.row9Pos;
            row10 = cash.placePos.row10Pos;
            row11 = cash.placePos.row11Pos;
            row12 = cash.placePos.row12Pos;
        }
        else if (rightHand == false)
        {
            deck = cash.placePos.row8_LeftPos;
            deck_1 = cash.placePos.row8_1_LeftPos;
            row9 = cash.placePos.row9_LeftPos;
            row10 = cash.placePos.row10_LeftPos;
            row11 = cash.placePos.row11_LeftPos;
            row12 = cash.placePos.row12_LeftPos;
        }



        if (row8_List.Count != 0)
        {
            for (int i = amountDeck - 1; i >= 0; i--)
            {
                GameObject aDeckCard = row8_List[i];
                aDeckCard.transform.position = deck;
            }
        }
        if (openDeck_List.Count != 0)
        {
            for (int i = amountDeck_1 - 1; i >= 0; i--)
            {
                GameObject aDeck_1Card = openDeck_List[i];
                aDeck_1Card.transform.position = deck_1;
            }
        }
        if (row9_List.Count != 0)
        {
            for (int i = amountRow9 - 1; i >= 0; i--)
            {
                GameObject arow9Card = row9_List[i];
                arow9Card.transform.position = row9;
            }
        }
        if (row10_List.Count != 0)
        {
            for (int i = amountRow10 - 1; i >= 0; i--)
            {
                GameObject arow10Card = row10_List[i];
                arow10Card.transform.position = row10;
            }
        }
        if (row11_List.Count != 0)
        {
            for (int i = amountRow11 - 1; i >= 0; i--)
            {
                GameObject arow11Card = row11_List[i];
                arow11Card.transform.position = row11;
            }
        }
        if (row12_List.Count != 0)
        {
            for (int i = amountRow12 - 1; i >= 0; i--)
            {
                GameObject arow12Card = row12_List[i];
                arow12Card.transform.position = row12;
            }
        }

        cash.touchingDeck.SetPosToList();
        cash.touchingDeck.FixOpenDeck();

    }


    //public void BeFalseOrTrueOpenCards_Retu(bool result)
    //{
    //    for (int i = 0; i < 52; i++)
    //    {
    //        if (cash.allCards[i].transform.tag == "retu")
    //        {
    //            bool isTurned = cash.GetIsTurned(i);
    //            if (isTurned == true)
    //                cash.GetBoxCollider(i).enabled = result;
    //        }
    //    }
    //}


    public void BeFalseOrTrueOpenCards_Retu(bool result)
	{
		if (row1_List.Count > 0) {
			BeFalseOrTrueListCards_Retu (row1_List, result);
		}
		if (row2_List.Count > 0) {
			BeFalseOrTrueListCards_Retu (row2_List, result);
		}
		if (row3_List.Count > 0) {
			BeFalseOrTrueListCards_Retu (row3_List, result);
		}
		if (row4_List.Count > 0) {
			BeFalseOrTrueListCards_Retu (row4_List, result);
		}
		if (row5_List.Count > 0) {
			BeFalseOrTrueListCards_Retu (row5_List, result);
		}
		if (row6_List.Count > 0) {
			BeFalseOrTrueListCards_Retu (row6_List, result);
		}
		if (row7_List.Count > 0) {
			BeFalseOrTrueListCards_Retu (row7_List, result);
		}

	}


	void BeFalseOrTrueListCards_Retu(List<GameObject> list, bool result)
	{
		for (int i = 0; i < list.Count; i++) {
			GameObject aCard = list [i];
			bool isOpen = aCard.GetComponent<Card> ().isTurned;
			if (isOpen == true)
				aCard.GetComponent<BoxCollider2D> ().enabled = result;
		}
	}


	public void BeFalseOrTrueOpenCards_Yama(bool result, bool isToRow8)
	{
		if (row9_List.Count > 0) {
			BeFalseOrTrueListCards_Yama (row9_List, result);
		}
		if (row10_List.Count > 0) {
			BeFalseOrTrueListCards_Yama (row10_List, result);
		}
		if (row11_List.Count > 0) {
			BeFalseOrTrueListCards_Yama (row11_List, result);
		}
		if (row12_List.Count > 0) {
			BeFalseOrTrueListCards_Yama (row12_List, result);
		}
        if(openDeck_List.Count > 0)
        {
            openDeck_List[openDeck_List.Count - 1].GetComponent<BoxCollider2D>().enabled = result;
        }

        if(isToRow8 == true)
            GameObject.Find("row8").GetComponent<BoxCollider2D>().enabled = result;
	}
		

	void BeFalseOrTrueListCards_Yama(List<GameObject> list, bool result)
	{
		int laseIndexNum = list.Count - 1;
		list [laseIndexNum].GetComponent<BoxCollider2D> ().enabled = result;
	}



	public void ChangePlaymatSprite()
	{
		cash.playmat_Obj.GetComponent<SpriteRenderer> ().sprite = cash.setting.playmatSprite;
	}



	public void ChangeBackCardSprite()
	{
		if (row1_List.Count != 0) {
			for (int i = 0; i < row1_List.Count; i++) {
				GameObject aCard = row1_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row2_List.Count != 0) {
			for (int i = 0; i < row2_List.Count; i++) {
				GameObject aCard = row2_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row3_List.Count != 0) {
			for (int i = 0; i < row3_List.Count; i++) {
				GameObject aCard = row3_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row4_List.Count != 0) {
			for (int i = 0; i < row4_List.Count; i++) {
				GameObject aCard = row4_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row5_List.Count != 0) {
			for (int i = 0; i < row5_List.Count; i++) {
				GameObject aCard = row5_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row6_List.Count != 0) {
			for (int i = 0; i < row6_List.Count; i++) {
				GameObject aCard = row6_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row7_List.Count != 0) {
			for (int i = 0; i < row7_List.Count; i++) {
				GameObject aCard = row7_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row8_List.Count != 0) {
			for (int i = 0; i < row8_List.Count; i++) {
				GameObject aCard = row8_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row9_List.Count != 0) {
			for (int i = 0; i < row9_List.Count; i++) {
				GameObject aCard = row9_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row10_List.Count != 0) {
			for (int i = 0; i < row10_List.Count; i++) {
				GameObject aCard = row10_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row11_List.Count != 0) {
			for (int i = 0; i < row11_List.Count; i++) {
				GameObject aCard = row11_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}
		if (row12_List.Count != 0) {
			for (int i = 0; i < row12_List.Count; i++) {
				GameObject aCard = row12_List [i];
				Card card_C = aCard.GetComponent<Card> ();
				if (card_C.isTurned == false)
					card_C.SetBackSprite ();
			}
		}

		SpriteRenderer generateAndDealCardsSprite = GameObject.Find ("generateAndDealCards").GetComponent<SpriteRenderer> ();
		generateAndDealCardsSprite.sprite = cash.setting.backSprite;
	}



	public List<bool> isTurnedOrNot = new List<bool> ();
    float timeToWaitToHome = 0.02f;


    
    public void DealEveryCardsBackForHome()
	{
		if (row1_List.Count != 0) {
			for (int i = 0; i < row1_List.Count; i++) {
				GameObject aCard = row1_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row2_List.Count != 0) {
			for (int i = 0; i < row2_List.Count; i++) {
				GameObject aCard = row2_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
        }
		if (row3_List.Count != 0) {
			for (int i = 0; i < row3_List.Count; i++) {
				GameObject aCard = row3_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
        }
		if (row4_List.Count != 0) {
			for (int i = 0; i < row4_List.Count; i++) {
				GameObject aCard = row4_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row5_List.Count != 0) {
			for (int i = 0; i < row5_List.Count; i++) {
				GameObject aCard = row5_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row6_List.Count != 0) {
			for (int i = 0; i < row6_List.Count; i++) {
				GameObject aCard = row6_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row7_List.Count != 0) {
			for (int i = 0; i < row7_List.Count; i++) {
				GameObject aCard = row7_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row8_List.Count != 0) {
			for (int i = 0; i < row8_List.Count; i++) {
				GameObject aCard = row8_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (openDeck_List.Count != 0) {
			for (int i = 0; i < openDeck_List.Count; i++) {
				GameObject aCard = openDeck_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row9_List.Count != 0) {
			for (int i = 0; i < row9_List.Count; i++) {
				GameObject aCard = row9_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row10_List.Count != 0) {
			for (int i = 0; i < row10_List.Count; i++) {
				GameObject aCard = row10_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row11_List.Count != 0) {
			for (int i = 0; i < row11_List.Count; i++) {
				GameObject aCard = row11_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}
		if (row12_List.Count != 0) {
			for (int i = 0; i < row12_List.Count; i++) {
				GameObject aCard = row12_List [i];
				isTurnedOrNot.Add (aCard.GetComponent<Card>().isTurned);
				MoveAdnFlipToBack (aCard);
            }
		}

	}

	void MoveAdnFlipToBack(GameObject aCard)
	{
		aCard.GetComponent<CardsDirector> ().SetInfoToAnimation (true, cash.generateAndDealCards_Obj.transform.position, 1.8f, false);
		if (aCard.GetComponent<Card> ().isTurned == true) {
			aCard.GetComponent<CardsDirector> ().FlipToBack (true, rollingSpeed);
			aCard.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}



	public Vector3 GetPosOfGivenCard(int scanSuit, int scanNumOfCard)
	{
		Vector3 targetPos = Vector3.zero;
		int deckAmount = row8_List.Count + openDeck_List.Count;

		if (deckAmount > 0) {
			foreach (GameObject obj in row8_List) {
				Card card = obj.GetComponent<Card> ();
				int suit = card.suit;
				int numOfCard = card.numOfCard;
				if (suit == scanSuit && numOfCard == scanNumOfCard) {
					targetPos = obj.transform.position;
					break;
				}
			}
			if (targetPos == Vector3.zero) {
				foreach (GameObject obj in openDeck_List) {
					Card card = obj.GetComponent<Card> ();
					int suit = card.suit;
					int numOfCard = card.numOfCard;
					if (suit == scanSuit && numOfCard == scanNumOfCard) {
						targetPos = obj.transform.position;
						break;
					}
				}
			}
		}
		return targetPos;
	}




	public bool GetBoolIsTouchedOnPlaymat(Vector3 touchedPos)
	{
		bool isTouchedOnPlaymat = false;
		int upperListName = GetUpperListName (touchedPos.x); 
		float bottomYPosOfUpperCard = GetBottomYPosOfUpperCard(upperListName);
		float uiUpperYPos = 0f;

        if (cash.showBotton.uiAreClosedByTouching == false) {
			GameObject game = GameObject.Find ("game");
			float uiYPos = Camera.main.ScreenToWorldPoint (game.transform.position).y;
			float uiHeight = game.GetComponent<RectTransform> ().rect.height;
			uiUpperYPos = uiYPos + uiHeight;
		}
        if(cash.showBotton.uiAreClosedByTouching == true)
			uiUpperYPos = -cash.placePos.worldScreenHeight / 2f;
		
		if (bottomYPosOfUpperCard > touchedPos.y && uiUpperYPos < touchedPos.y)
			isTouchedOnPlaymat = true;

		return isTouchedOnPlaymat;
	}




	int GetUpperListName(float xPos)
	{
		int upperListName = 0;
		float halfCardWidth = cash.placePos.cardWidth / 2f;
		List<float> retuXPosList = new List<float> ();
		for (int i = 1; i <= 7; i++) {
			float retuXPos = GetRetuXPos(i);
			retuXPosList.Add (retuXPos);
		}
			
		for(int i = 0; i < retuXPosList.Count; i++){
			float leftRetuXPos = retuXPosList [i] - halfCardWidth;
			float rightRetuXPos = retuXPosList [i] + halfCardWidth;
			if (leftRetuXPos <= xPos && xPos <= rightRetuXPos) {
				upperListName = i + 1;
				break;
			}
		}
		return upperListName;
	}


	float GetBottomYPosOfUpperCard(int listNameNum)
	{
		float yPos = 0f;
		float halfCardHeight = cash.placePos.cardHeight / 2f;

		switch(listNameNum){
		case 1:
			if (row1_List.Count > 0)
				yPos = row1_List [row1_List.Count - 1].transform.position.y - halfCardHeight;
			else
				yPos = cash.row1_Obj.transform.position.y - halfCardHeight;
			break;
		case 2:
			if (row2_List.Count > 0)
				yPos = row2_List [row2_List.Count - 1].transform.position.y - halfCardHeight;
			else
				yPos = cash.row2_Obj.transform.position.y - halfCardHeight;
			break;
		case 3:
			if (row3_List.Count > 0)
				yPos = row3_List [row3_List.Count - 1].transform.position.y - halfCardHeight;
			else
				yPos = cash.row3_Obj.transform.position.y - halfCardHeight;
			break;
		case 4:
			if (row4_List.Count > 0)
				yPos = row4_List [row4_List.Count - 1].transform.position.y - halfCardHeight;
			else
				yPos = cash.row4_Obj.transform.position.y - halfCardHeight;
			break;
		case 5:
			if (row5_List.Count > 0)
				yPos = row5_List [row5_List.Count - 1].transform.position.y - halfCardHeight;
			else
				yPos = cash.row5_Obj.transform.position.y - halfCardHeight;
			break;
		case 6:
			if (row6_List.Count > 0)
				yPos = row6_List [row6_List.Count - 1].transform.position.y - halfCardHeight;
			else
				yPos = cash.row6_Obj.transform.position.y - halfCardHeight;
			break;
		case 7:
			if (row7_List.Count > 0)
				yPos = row7_List [row7_List.Count - 1].transform.position.y - halfCardHeight;
			else
				yPos = cash.row7_Obj.transform.position.y - halfCardHeight;
			break;
		}
		return yPos;
	}


	float GetRetuXPos(int listName)
	{
		float xPos = 0f;

		switch(listName){
		case 1:
			xPos = cash.row1_Obj.transform.position.x;
			break;
		case 2:
			xPos = cash.row2_Obj.transform.position.x;
			break;
		case 3:
			xPos = cash.row3_Obj.transform.position.x;
			break;
		case 4:
			xPos = cash.row4_Obj.transform.position.x;
			break;
		case 5:
			xPos = cash.row5_Obj.transform.position.x;
			break;
		case 6:
			xPos = cash.row6_Obj.transform.position.x;
			break;
		case 7:
			xPos = cash.row7_Obj.transform.position.x;
			break;
		}
		return xPos;
	}



	public void UiEnabled(GameObject ui, bool enabled)
	{
		ui.GetComponent<Button> ().enabled = enabled;
		Color color = ui.GetComponent<Image> ().color;
		if(enabled == true)
			color.a = 1f;
		if(enabled == false)
	    	color.a = 0.6f;
		ui.GetComponent<Image> ().color = color;
	}


	public void ChangeAllCardsEnabled(bool enabled)
	{
		if (row1_List.Count != 0) {
			for (int i = 0; i < row1_List.Count; i++) {
				GameObject aCard = row1_List [i];
				aCard.GetComponent<BoxCollider2D> ().enabled = enabled;
			}
		}
		if (row2_List.Count != 0) {
			for (int i = 0; i < row2_List.Count; i++) {
				GameObject aCard = row2_List [i];
				aCard.GetComponent<BoxCollider2D> ().enabled = enabled;
			}
		}
		if (row3_List.Count != 0) {
			for (int i = 0; i < row3_List.Count; i++) {
				GameObject aCard = row3_List [i];
				aCard.GetComponent<BoxCollider2D> ().enabled = enabled;
			}
		}
		if (row4_List.Count != 0) {
			for (int i = 0; i < row4_List.Count; i++) {
				GameObject aCard = row4_List [i];
				aCard.GetComponent<BoxCollider2D> ().enabled = enabled;
			}
		}
		if (row5_List.Count != 0) {
			for (int i = 0; i < row5_List.Count; i++) {
				GameObject aCard = row5_List [i];
				aCard.GetComponent<BoxCollider2D> ().enabled = enabled;
			}
		}
		if (row6_List.Count != 0) {
			for (int i = 0; i < row6_List.Count; i++) {
				GameObject aCard = row6_List [i];
				aCard.GetComponent<BoxCollider2D> ().enabled = enabled;
			}
		}
		if (row7_List.Count != 0) {
			for (int i = 0; i < row7_List.Count; i++) {
				GameObject aCard = row7_List [i];
				aCard.GetComponent<BoxCollider2D> ().enabled = enabled;
			}
		}

		if (openDeck_List.Count != 0)
            openDeck_List[openDeck_List.Count - 1].GetComponent<BoxCollider2D>().enabled = enabled;

        if (row9_List.Count != 0) 
            row9_List[row9_List.Count - 1].GetComponent<BoxCollider2D> ().enabled = enabled;

        if (row10_List.Count != 0)
            row10_List[row10_List.Count - 1].GetComponent<BoxCollider2D>().enabled = enabled;

        if (row11_List.Count != 0)
            row11_List[row11_List.Count - 1].GetComponent<BoxCollider2D>().enabled = enabled;

        if (row12_List.Count != 0)
            row12_List[row12_List.Count - 1].GetComponent<BoxCollider2D>().enabled = enabled;
    }


    public void AddAllCardsToDealList()
	{
		if (row1_List.Count > 0) {
			for (int i = 0; i < row1_List.Count; i++) {
				GameObject aCard = row1_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row2_List.Count > 0) {
			for (int i = 0; i < row2_List.Count; i++) {
				GameObject aCard = row2_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row3_List.Count > 0) {
			for (int i = 0; i < row3_List.Count; i++) {
				GameObject aCard = row3_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row4_List.Count > 0) {
			for (int i = 0; i < row4_List.Count; i++) {
				GameObject aCard = row4_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row5_List.Count > 0) {
			for (int i = 0; i < row5_List.Count; i++) {
				GameObject aCard = row5_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row6_List.Count > 0) {
			for (int i = 0; i < row6_List.Count; i++) {
				GameObject aCard = row6_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row7_List.Count > 0) {
			for (int i = 0; i < row7_List.Count; i++) {
				GameObject aCard = row7_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row8_List.Count > 0) {
			for (int i = 0; i < row8_List.Count; i++) {
				GameObject aCard = row8_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (openDeck_List.Count > 0) {
			for (int i = 0; i < openDeck_List.Count; i++) {
				GameObject aCard = openDeck_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row9_List.Count > 0) {
			for (int i = 0; i < row9_List.Count; i++) {
				GameObject aCard = row9_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row10_List.Count > 0) {
			for (int i = 0; i < row10_List.Count; i++) {
				GameObject aCard = row10_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row11_List.Count > 0) {
			for (int i = 0; i < row11_List.Count; i++) {
				GameObject aCard = row11_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
		if (row12_List.Count > 0) {
			for (int i = 0; i < row12_List.Count; i++) {
				GameObject aCard = row12_List [i];
				cash.dealCard.cardsList.Add (aCard);
			}
		}
	}



	float moveToShrink = 0.4f;
    public float shrinkLength = 3f;

    public void ShrinkListCards(List<GameObject> list, int howManyTimeToShrink, float delta)
    {
        float length = shrinkLength * howManyTimeToShrink;
        int frontCounter = 0;
        GameObject firstFrontCard = null;

        for(int i = 0; i < list.Count; i++)
        {
            bool isTurned = list[i].GetComponent<Card>().isTurned;
            if(isTurned == true)
            {
                if(frontCounter == 0)
                    firstFrontCard = list[i];

                if(frontCounter > 0)
                {
                    float yFirstCardPos = firstFrontCard.transform.position.y;
                    float yThisCardPos = list[i].transform.position.y;
                    float a = length * frontCounter;
                    float yPos = yThisCardPos + a;
                    Vector3 pos = new Vector3(list[i].transform.position.x, yPos, 0f);
                    list[i].GetComponent<CardsDirector>().isToShrink = false;
                    list[i].GetComponent<CardsDirector>().SetInfoToAnimation(true, pos, moveToShrink, false);
                }
                frontCounter++;
            }
        }

    }



    public void StrechListCards(List<GameObject> list, int howManyTimeToStrech, float delta)
    {
        if (howManyTimeToStrech == 0)
            return;

        float length = shrinkLength * howManyTimeToStrech;
        int frontCounter = 0;
        GameObject firstFrontCard = null;

        for (int i = 0; i < list.Count; i++)
        {
            bool isTurned = list[i].GetComponent<Card>().isTurned;
            if (isTurned == true)
            {
                if (frontCounter == 0)
                    firstFrontCard = list[i];

                if (frontCounter > 0)
                {
                    float yFirstCardPos = firstFrontCard.transform.position.y;
                    float yThisCardPos = list[i].transform.position.y;
                    float a = length * frontCounter;
                    float yPos = yThisCardPos - a;
                    Vector3 pos = new Vector3(list[i].transform.position.x, yPos, 0f);
                    list[i].GetComponent<CardsDirector>().isToShrink = false;
                    list[i].GetComponent<CardsDirector>().SetInfoToAnimation(true, pos, moveToShrink, false);
                }
                frontCounter++;
            }
        }

    }


    public void PlaceCards(List<GameObject> list)
	{
		if (list.Count >= 2)
			return;
		
		int backCardCounter = 0;
		for (int i = 0; i < list.Count; i++) {
			GameObject obj = list [i];
			bool isTurned = obj.GetComponent<Card> ().isTurned;
			if (isTurned == false)
				backCardCounter += 1;
		}

		GameObject emptyObj = GetEmptyObj (list[list.Count - 1]);
		Vector3 endPos = emptyObj.transform.position;
		float newYPos = GetFloorFloat(endPos.y);

		for (int i = 0; i < list.Count; i++) {
			GameObject obj = list [i];
			bool isTurned = obj.GetComponent<Card> ().isTurned;

			if(i == 0)
				obj.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, moveToShrink, false);

			if (isTurned == false && i > 0) {
				newYPos = endPos.y - cash.placePos.intervalBackCards;
				endPos = new Vector3 (endPos.x, newYPos, endPos.z);
				obj.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, moveToShrink, false);
			}

			if (isTurned == true && i > 0 && backCardCounter == i) {
				newYPos = endPos.y - cash.placePos.intervalBackCards;
				endPos = new Vector3 (endPos.x, newYPos, endPos.z);
				obj.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, moveToShrink, false);
			}

			if (isTurned == true && i > 0 && backCardCounter < i) {
				newYPos = endPos.y - cash.placePos.intervalFrontCards;
				endPos = new Vector3 (endPos.x, newYPos, endPos.z);
				obj.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, moveToShrink, false);
			}
		}
	}



	float GetFloorFloat(float num)
	{
		float a = num * 10f;
		float b = Mathf.Floor(a);
		float newNum = b / 10f;
		return newNum;
	}



	public void ChangeRetuBackCardBC2DToFalse()
	{
		if (row1_List.Count != 0)
		{
			for (int i = 0; i < row1_List.Count; i++)
			{
				GameObject aCard = row1_List[i];
				Card card_C = aCard.GetComponent<Card>();
				if (card_C.isTurned == false)
					card_C.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		if (row2_List.Count != 0)
		{
			for (int i = 0; i < row2_List.Count; i++)
			{
				GameObject aCard = row2_List[i];
				Card card_C = aCard.GetComponent<Card>();
				if (card_C.isTurned == false)
					card_C.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		if (row3_List.Count != 0)
		{
			for (int i = 0; i < row3_List.Count; i++)
			{
				GameObject aCard = row3_List[i];
				Card card_C = aCard.GetComponent<Card>();
				if (card_C.isTurned == false)
					card_C.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		if (row4_List.Count != 0)
		{
			for (int i = 0; i < row4_List.Count; i++)
			{
				GameObject aCard = row4_List[i];
				Card card_C = aCard.GetComponent<Card>();
				if (card_C.isTurned == false)
					card_C.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		if (row5_List.Count != 0)
		{
			for (int i = 0; i < row5_List.Count; i++)
			{
				GameObject aCard = row5_List[i];
				Card card_C = aCard.GetComponent<Card>();
				if (card_C.isTurned == false)
					card_C.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		if (row6_List.Count != 0)
		{
			for (int i = 0; i < row6_List.Count; i++)
			{
				GameObject aCard = row6_List[i];
				Card card_C = aCard.GetComponent<Card>();
				if (card_C.isTurned == false)
					card_C.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		if (row7_List.Count != 0)
		{
			for (int i = 0; i < row7_List.Count; i++)
			{
				GameObject aCard = row7_List[i];
				Card card_C = aCard.GetComponent<Card>();
				if (card_C.isTurned == false)
					card_C.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		//if (row8_List.Count != 0)
		//{
		//	for (int i = 0; i < row8_List.Count; i++)
		//	{
		//		GameObject aCard = row8_List[i];
		//		Card card_C = aCard.GetComponent<Card>();
		//		if (card_C.isTurned == false)
		//			card_C.GetComponent<BoxCollider2D>().enabled = false;
		//	}
		//}
		//if (row9_List.Count != 0)
		//{
		//	for (int i = 0; i < row9_List.Count; i++)
		//	{
		//		GameObject aCard = row9_List[i];
		//		Card card_C = aCard.GetComponent<Card>();
		//		if (card_C.isTurned == false)
		//			card_C.GetComponent<BoxCollider2D>().enabled = false;
		//	}
		//}
		//if (row10_List.Count != 0)
		//{
		//	for (int i = 0; i < row10_List.Count; i++)
		//	{
		//		GameObject aCard = row10_List[i];
		//		Card card_C = aCard.GetComponent<Card>();
		//		if (card_C.isTurned == false)
		//			card_C.GetComponent<BoxCollider2D>().enabled = false;
		//	}
		//}
		//if (row11_List.Count != 0)
		//{
		//	for (int i = 0; i < row11_List.Count; i++)
		//	{
		//		GameObject aCard = row11_List[i];
		//		Card card_C = aCard.GetComponent<Card>();
		//		if (card_C.isTurned == false)
		//			card_C.GetComponent<BoxCollider2D>().enabled = false;
		//	}
		//}
		//if (row12_List.Count != 0)
		//{
		//	for (int i = 0; i < row12_List.Count; i++)
		//	{
		//		GameObject aCard = row12_List[i];
		//		Card card_C = aCard.GetComponent<Card>();
		//		if (card_C.isTurned == false)
		//			card_C.GetComponent<BoxCollider2D>().enabled = false;
		//	}
		//}

		//SpriteRenderer generateAndDealCardsSprite = GameObject.Find("generateAndDealCards").GetComponent<SpriteRenderer>();
		//generateAndDealCardsSprite.sprite = cash.setting.backSprite;
	}



	/// //////////////////////////////////////////////////////////////////////////
	/// Auto Clear
	/// AoutClearGame() を呼ぶだけ
	public void AoutClearGame()
	{
		for (int i = 0; i < row1_List.Count; i++) {
			GameObject aCard = row1_List [i];
			GetAmountOfList ();
			returnList.Add (aCard);

		}
		row1_List.Clear ();
		for (int i = 0; i < row2_List.Count; i++) {
			GameObject aCard = row2_List [i];
			GetAmountOfList ();
			returnList.Add (aCard);
		}
		row2_List.Clear ();
		for (int i = 0; i < row3_List.Count; i++) {
			GameObject aCard = row3_List [i];
			GetAmountOfList ();
			returnList.Add (aCard);
		}
		row3_List.Clear ();
		for (int i = 0; i < row4_List.Count; i++) {
			GameObject aCard = row4_List [i];
			GetAmountOfList ();
			returnList.Add (aCard);
		}
		row4_List.Clear ();
		for (int i = 0; i < row5_List.Count; i++) {
			GameObject aCard = row5_List [i];
			GetAmountOfList ();
			returnList.Add (aCard);
		}
		row5_List.Clear ();
		for (int i = 0; i < row6_List.Count; i++) {
			GameObject aCard = row6_List [i];
			GetAmountOfList ();
			returnList.Add (aCard);
		}
		row6_List.Clear ();
		for (int i = 0; i < row7_List.Count; i++) {
			GameObject aCard = row7_List [i];
			GetAmountOfList ();
			returnList.Add (aCard);
		}
		row7_List.Clear ();
		for (int i = 0; i < row8_List.Count; i++) {
			GameObject aCard = row8_List [i];
			GetAmountOfList ();
			returnList.Add (aCard);
		}
		row8_List.Clear ();

		CheckAmountOfAllOfYamaIsForEnding ();
	}

	List<GameObject> returnList = new List<GameObject> ();

	void GetAmountOfList()
	{
		if (row9_List.Count != 13)
			returnList = row9_List;
		else if (row10_List.Count != 13)
			returnList = row10_List;
		else if (row11_List.Count != 13)
			returnList = row11_List;
		else if (row12_List.Count != 13)
			returnList = row12_List;
	}
    //////////////////////////////////////////////////////////////////////////

}





