using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GamesWon : MonoBehaviour {

	Color color;

    Cash cash;

	GameObject digitPrefab;
	GameObject gamesWonBackG;
	public float currentGamesWonNum;
	public static bool moveToHome = false;


	Vector2 oridinalPosAtHome;
	public static Vector2 clearedPos;
	public static Vector2 statisticsPos;
	public static Vector2 sizeDeltaAtCleared;
	public static Vector2 sizeDeltaAtHome;


	public static float animationMovedYPos = 233f; //sizeDeltaAtCleared.yの値の１.５倍

	//gamesWonオブジェクトのAnchorの値
	float xAnchor = 0f; //表示する桁数に依存する為、のちに変更される
	float yAnchor = 0.5f; //gamesWonオブジェクトの子になるため、yAnchorの値は必ず0.5になる

	//クリア後のスコア画面からホームへ戻るときの動きに必要
	float elapsedTime = 0f;
	public static float timeToArrive = 0.6f;
	public static float xDeltaScale;
	public static float yDeltaScale;

	//homeでScoreが表示される時
	bool isMoveToShowStatisticsPos = false;
	bool isMoveToBeOffStatisticsPos = false;
	float timeToArriveStatistics = 0.5f;

    //クリアした後に画面に表示するとき
    bool isMoveToclearedPos = false;
    float timeToArriveCleared = 1.5f;

    int acpect = 0;

    public static List<GameObject> showingGamesWonSprite = new List<GameObject> (); // 現在表示しているSprite
	private Dictionary<char, Sprite> dicGamesWonSprite = new Dictionary <char, Sprite>(); // スプライトディクショナリ

	string willGamesWonNumStr;

	public void initialize()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
		gamesWonBackG = GameObject.Find ("gamesWonBackG");
        ChangeGamesWonBackGColor(true);

		List<Sprite> numbers = new List<Sprite> ();
		string path = "";
		Sprite numberSprite;

		string prafabPath = "Prefab" + "/" + "digitPrefab";
		digitPrefab = Resources.Load<GameObject>(prafabPath);

		oridinalPosAtHome = new Vector2 (0, 100f);
		clearedPos = new Vector2 (0, 850f);
		statisticsPos = new Vector2 (0, 920f);
		sizeDeltaAtCleared = new Vector2 (130f, 155f);
		sizeDeltaAtHome = new Vector2 (80f, 100f);
		xDeltaScale = sizeDeltaAtCleared.x - sizeDeltaAtHome.x;
		yDeltaScale = sizeDeltaAtCleared.y - sizeDeltaAtHome.y;

        float height = Screen.height;
        float width = Screen.width;
        float _acpect = height / width;


        if (_acpect < 1.6f)
        {
            acpect = 1;
        }
        else if (_acpect > 1.6f && _acpect < 1.7f)
        {
            acpect = 2;
        }
        else if (_acpect > 1.7f && _acpect < 2f)
        {
            acpect = 3;
        }
        else if (_acpect > 2f)
        {
            acpect = 4;
        }


        for (int i = 0; i < 10; i++) {
			path = "number" + "_" + i;
            numberSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite> (path);
			numbers.Add (numberSprite);
		}

		dicGamesWonSprite = new Dictionary<char, Sprite>() {
			{ '0', numbers[0] },
			{ '1', numbers[1] },
			{ '2', numbers[2] },
			{ '3', numbers[3] },
			{ '4', numbers[4] },
			{ '5', numbers[5] },
			{ '6', numbers[6] },
			{ '7', numbers[7] },
			{ '8', numbers[8] },
			{ '9', numbers[9] },
		};
        //MovePopGamesWon();
    }

    bool isMovePop = false;
    bool isGo = false;
    bool isBack = false;
    bool isCount = false;
    float internPop = 1.8f;
    float goTime = 0.5f;
    float backTime = 1f;
    float internCount = 0f;
    Vector2 goPos = new Vector2(0f, 125f);



	/// <summary>
	/// GamesWonをホームに戻す
	/// </summary>
	void Update()
	{
        if (isMovePop == true)
        {
            if(isCount == true)
            {
                internCount += Time.deltaTime;
                if(internCount >= internPop)
                {
                    internCount = 0f;
                    elapsedTime = 0f;
                    isGo = true;
                }
            }

            if (isGo == true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / goTime;
                if (t > 1.0f) t = 1.0f;
                float rate = t * t * (3.0f - 2.0f * t);
                gameObject.GetComponent<RectTransform>().anchoredPosition
                    = gameObject.GetComponent<RectTransform>().anchoredPosition * (1.0f - rate) + goPos * rate;

                if(elapsedTime >= goTime)
                {
                    elapsedTime = 0f;
                    isGo = false;
                    isBack = true;
                }
            }

            if(isBack == true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / backTime;
                if (t > 1.0f) t = 1.0f;
                float rate = t * t * (3.0f - 2.0f * t);
                gameObject.GetComponent<RectTransform>().anchoredPosition
                    = gameObject.GetComponent<RectTransform>().anchoredPosition * (1.0f - rate) + oridinalPosAtHome * rate;

                if (elapsedTime >= goTime)
                {
                    elapsedTime = 0f;
                    isBack = false;
                    isCount = true;
                }
            }
        }

		if (moveToHome == true) {
			elapsedTime += Time.deltaTime;   // 経過時間
			float t = elapsedTime / timeToArrive;     // 時間を媒介変数に
			if ( t > 1.0f ) t = 1.0f;    // クランプ

            //移動処理
            float yDelta = clearedPos.y - oridinalPosAtHome.y;
            float yMoveDistance = yDelta * t; // 0 - 1
            Vector2 pos = gameObject.GetComponent<RectTransform>().anchoredPosition;
            pos.x = oridinalPosAtHome.x;
            pos.y = clearedPos.y - yMoveDistance;
            gameObject.GetComponent<RectTransform>().anchoredPosition = pos;

            //スケール処理
            if (elapsedTime >= timeToArrive)
				elapsedTime = timeToArrive;
			float timePercentage = elapsedTime / timeToArrive;
			float xNewScale = sizeDeltaAtCleared.x * showingGamesWonSprite.Count - timePercentage * xDeltaScale * showingGamesWonSprite.Count;
			float yNewScale = sizeDeltaAtCleared.y - timePercentage * yDeltaScale;
			if (xNewScale <= sizeDeltaAtHome.x)
				xNewScale = sizeDeltaAtHome.x;
			if (yNewScale <= sizeDeltaAtHome.y)
				yNewScale = sizeDeltaAtHome.y;
			gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (xNewScale, yNewScale);

			if (elapsedTime >= timeToArrive) {
				moveToHome = false;
				gameObject.GetComponent<RectTransform> ().anchoredPosition = oridinalPosAtHome;
				PlaceGamesWonNum (willGamesWonNumStr);
				ChangeWidthOfGamwsWonObj (showingGamesWonSprite.Count);
                gameObject.GetComponent<Button>().enabled = true;
				elapsedTime = 0f;
                FinishClearedScores finClearScore = GameObject.Find("finishClearedScore").GetComponent<FinishClearedScores>();
                finClearScore.CallMakeNewCardsAtHome();
            }
		}

		if (isMoveToShowStatisticsPos == true) {
			elapsedTime += Time.deltaTime;   // 経過時間
			float t = elapsedTime / timeToArriveStatistics;     // 時間を媒介変数に
			if ( t > 1.0f ) t = 1.0f;    // クランプ
			float rate = t * t * ( 3.0f - 2.0f * t );   // 3次関数補間値に変換
			gameObject.GetComponent<RectTransform> ().anchoredPosition 
			= gameObject.GetComponent<RectTransform> ().anchoredPosition * (1.0f - rate) + statisticsPos * rate;   // いわゆるLerp

			if (elapsedTime >= timeToArriveStatistics) {
				gameObject.GetComponent<RectTransform> ().anchoredPosition = statisticsPos;
				isMoveToShowStatisticsPos = false;
				elapsedTime = 0f;

            }
		}

		if (isMoveToBeOffStatisticsPos == true) {
			elapsedTime += Time.deltaTime;   // 経過時間
			float t = elapsedTime / timeToArriveStatistics;     // 時間を媒介変数に
			if ( t > 1.0f ) t = 1.0f;    // クランプ
			float rate = t * t * ( 3.0f - 2.0f * t );   // 3次関数補間値に変換
			gameObject.GetComponent<RectTransform> ().anchoredPosition 
			= gameObject.GetComponent<RectTransform> ().anchoredPosition * (1.0f - rate) + oridinalPosAtHome * rate;   // いわゆるLerp

			if (elapsedTime >= timeToArriveStatistics) {
				//ShowUpGamesWonObj ();
				gameObject.GetComponent<RectTransform> ().anchoredPosition = oridinalPosAtHome;
				isMoveToBeOffStatisticsPos = false;
				elapsedTime = 0f;
                //MovePopGamesWon();
            }
		}

        if (isMoveToclearedPos == true)
        {
            elapsedTime += Time.deltaTime;   // 経過時間
            float t = elapsedTime / timeToArriveStatistics;     // 時間を媒介変数に
            if (t > 1.0f) t = 1.0f;    // クランプ
            float rate = t * t * (3.0f - 2.0f * t);   // 3次関数補間値に変換
            gameObject.GetComponent<RectTransform>().anchoredPosition
            = gameObject.GetComponent<RectTransform>().anchoredPosition * (1.0f - rate) + clearedPos * rate;   // いわゆるLerp

            if (elapsedTime >= timeToArriveStatistics)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition = clearedPos;
                isMoveToclearedPos = false;
                elapsedTime = 0f;
            }
        }
    }


    //public void MovePopGamesWon()
    //{
    //    internCount = 0f;
    //    elapsedTime = 0f;
    //    isMovePop = true;
    //    isCount = true;
    //}

    //public void StopPopGamesWon()
    //{
    //    isMovePop = false;
    //    isGo = false;
    //    isBack = false;
    //    isCount = false;
    //}


    void ChangeTextColor(bool isTransparent)
    {
        Color textColor = GameObject.Find("gamesWonText").GetComponent<Text>().color;
        if(isTransparent == true)
            textColor.a = 0;
        if (isTransparent == false)
            textColor.a = 1f;
        GameObject.Find("gamesWonText").GetComponent<Text>().color = textColor;
    }



    public GameObject statisticsBackGPrefab;
    /// <summary>
    /// 成績表とgamesWonObjを上に出す
    /// </summary>
    public void OnMouseUp()
	{
        cash.sound.GetButtonSound();
        //StopPopGamesWon();
		gameObject.transform.SetAsLastSibling();
		TransparentGamesWonObj ();
		isMoveToShowStatisticsPos = true;
        GameObject statisticsBackG = Instantiate(statisticsBackGPrefab) as GameObject;
        statisticsBackG.GetComponent<StatisticsController>().Init();
        statisticsBackG.GetComponent<StatisticsController>().MoveToShow();
        ChangeTextColor(true);


        GameObject.Find("statisticsBackG2(Clone)").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("statistics_bg");
        GameObject.Find("statisticsCancel").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("statistics_quit_btn");
        GameObject.Find("statisticsFlipThree").GetComponent<Image>().sprite
			      = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("threeFlipStati");
		//= cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("statistics_tab2");
		GameObject.Find("statisticsFlipOne").GetComponent<Image>().sprite
			      = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("oneFlipStati");
		//= cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("statistics_tab1");
		GameObject.Find("statisticsTotal").GetComponent<Image>().sprite
                  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("statistics_total_bg");

		GameObject.Find("statisticsOneT").GetComponent<Text>().text = L.Text.FromKey("flip one");
		GameObject.Find("statisticsThreeT").GetComponent<Text>().text = L.Text.FromKey("flip three");

		GameObject.Find("homeMenu").transform.SetSiblingIndex(4);

		if (cash.setting.isShowingSetting == true)
        {
			cash.setting.CloseSetting();
        }

		if(cash.dealCard.didDeal == true)
        {
			//playingScoreを止める
			cash.scoreText_Playing.countingTime = false; //timeScoreを止める
														 //カードに触れられなくする
			cash.gameDirector.BeFalseOrTrueOpenCards_Retu(false);
			cash.gameDirector.BeFalseOrTrueOpenCards_Yama(false, true);
		}

		//WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.Statistics);

	}


	public void MoveToBeOffFormScoreShowing()
	{
		isMoveToBeOffStatisticsPos = true;
        ChangeTextColor(false);
        gameObject.GetComponent<Button>().enabled = true;
    }


	public Vector2 posAtOutOfScreen;
	public void SetPosAtOutOfScreen()
    {
		posAtOutOfScreen = new Vector2(gameObject.GetComponent<RectTransform>().anchoredPosition.x,
			gameObject.GetComponent<RectTransform>().anchoredPosition.y);

	}


	public void SetPosHomeOrOutOfScreen(bool isHomePos)
    {
		if (isHomePos == false)
			oridinalPosAtHome = posAtOutOfScreen;
		else if (isHomePos == true)
			oridinalPosAtHome = new Vector2(0, 100f);
	}




	public void ChangeGamesWonBackGColor(bool isTransparent)
	{
		color = gamesWonBackG.GetComponent<Image> ().color;
		if(isTransparent == true)
	    	color.a = 0f;
		if(isTransparent == false)
			color.a = 1;
		gamesWonBackG.GetComponent<Image> ().color = color;

		gamesWonBackG.SetActive (!isTransparent);
	}



	public static void ChangeScaleOfGamesWonNum()
	{
		foreach (GameObject obj in showingGamesWonSprite){
			obj.GetComponent<GamesWonNumController> ().isCahegingScale = true;
		}
	}

	public static void MoveToHomePos()
	{
		moveToHome = true;
        Color textColor = GameObject.Find("gamesWonText").GetComponent<Text>().color;
        textColor.a = 1;
        GameObject.Find("gamesWonText").GetComponent<Text>().color = textColor;
    }




	public void CallChangeGamesWonNumAsAnimation()
	{
		StartCoroutine ("ChangeGamesWonNumAsAnimation");  
	}


	/// <summary>
	/// クリア後に, アニメーションでgamesWonNumを１増加する
	/// </summary>
	IEnumerator ChangeGamesWonNumAsAnimation()
	{
		showUpGamesWonNumAtCleared (showingGamesWonSprite.Count);
        cash.sound.GetPopUpSound();
		ChangeGamesWonBackGColor (false);

		GameObject lastGamesWonNumObj = showingGamesWonSprite [showingGamesWonSprite.Count - 1];
		char lastNum = showingGamesWonSprite [showingGamesWonSprite.Count - 1].GetComponent<GamesWonNum> ().charNum;
		char willNextNum = GetNextChar (lastNum);
		float willGamesWonNum = currentGamesWonNum + 1f;
		willGamesWonNumStr = willGamesWonNum.ToString ();

		yield return new WaitForSeconds (1.2f);
        cash.sound.GetRiseNumSound();

        //クリア回数の最後の桁の数字が9ではない場合、charNumとspriteを変更するだけ
        if (lastNum != '9') {
			GetGamesWonNumObjJumpUp ();
			lastGamesWonNumObj.GetComponent<Image> ().sprite = dicGamesWonSprite[willNextNum];
			lastGamesWonNumObj.GetComponent<GamesWonNum>().charNum = willNextNum;
		}
		//クリア回数の最後の桁の数字が9だった場合、
		else if (lastNum == '9') {
			bool isRiseDigit = GetBoolIsRiseDigit (currentGamesWonNum);
			//桁が繰り上がる場合
			if(isRiseDigit == true){
	        	GenerateObj (); //オブジェクトをひとつ生成しリストへ追加
                ChangeScaleAtCleard (showingGamesWonSprite.Count); //scaleを桁数分に変更
				ArrangeObj(willGamesWonNumStr); //リスト内のオブジェクトのlocalPos, spriteを変更
			}
			//桁が繰り上がらない場合
			else if(isRiseDigit == false){
				//幾つの数字が0になるか
				int howManyNumWillBeZero = GetHowManyDigitsWillBeZero(willGamesWonNum);
				//1050だとしたら、ここには 5  が入る(後ろから数えて最後の0の隣にある数字)
				char lastNumIsNextToLastZero =  GetLastNumIsNextToLastZero(willGamesWonNum);
				for (int i = 0; i < howManyNumWillBeZero + 1; i++) { //0になる数字と、+1される数字の合計数が入る
					GameObject trgetGamesWonNumObj = showingGamesWonSprite [showingGamesWonSprite.Count - 1 - i];
		
					//最後の0の隣の数字
					if (i == howManyNumWillBeZero) {
						trgetGamesWonNumObj.GetComponent<Image> ().sprite = dicGamesWonSprite [lastNumIsNextToLastZero];
						trgetGamesWonNumObj.GetComponent<GamesWonNum> ().charNum = lastNumIsNextToLastZero;
					} else if(i != howManyNumWillBeZero){
						//0に変更される数字
						trgetGamesWonNumObj.GetComponent<Image> ().sprite = dicGamesWonSprite ['0'];
						trgetGamesWonNumObj.GetComponent<GamesWonNum> ().charNum = '0';

					}
				}
			}
			GetGamesWonNumObjJumpUp (); //ジャンプするアニメーションの開始
		}

		currentGamesWonNum += 1f;
        //WebAnalytics.GamesWon((int)currentGamesWonNum);
	}



	char GetLastNumIsNextToLastZero(float willGamesWonNum)
	{
		char aChar = new char ();
		string strGamesWonNum = willGamesWonNum.ToString ();

		for (int i = 0; i < strGamesWonNum.Length; i++) {
			char aStr = strGamesWonNum[strGamesWonNum.Length - 1 - i];
			if (aStr != '0') {
				aChar = aStr;
				break;
			}
		}
		return aChar;
	}


	/// <summary>
	/// 桁が上がるならtrueを返す
	/// </summary>
	bool GetBoolIsRiseDigit(float currentGamesWonNum)
	{
		bool isRiseUp = new bool ();
		if (currentGamesWonNum == 9f || currentGamesWonNum == 99f || currentGamesWonNum == 999f ||
			currentGamesWonNum == 9999f || currentGamesWonNum == 99999f || currentGamesWonNum == 99999f || currentGamesWonNum ==999999f)
			isRiseUp = true;
		else {			
			isRiseUp = false;
		}
		return isRiseUp; 
	}

	/// <summary>
	/// 下から何桁分の数字が 9 → 0 になるか
	/// </summary>
	int GetHowManyDigitsWillBeZero(float willGamesWonNum)
	{
		int zeroDigits = 0;
		string strGamesWonNum = willGamesWonNum.ToString ();

		for (int i = 0; i < strGamesWonNum.Length; i++) {
			char aStr = strGamesWonNum[strGamesWonNum.Length - 1 - i];
			//i == zeroDigitsは、1050のように数えたくない0が存在し、それを避けるため
			if (aStr == '0' && i == zeroDigits) {
				zeroDigits += 1;
			}
		}

		return zeroDigits;
	}



	void GetGamesWonNumObjJumpUp()
	{
		foreach (GameObject obj in showingGamesWonSprite){
			obj.GetComponent <GamesWonNumController> ().isJumpUp = true;
		}
	}


	void GenerateObj()
	{
		GameObject gamesWonNum = Instantiate (digitPrefab) as GameObject;
        gamesWonNum.transform.parent = gameObject.transform;
        gamesWonNum.transform.localScale = new Vector3(1f,1f,1f);
        gamesWonNum.GetComponent<GamesWonNumController>().ChangeScale (sizeDeltaAtCleared);
		showingGamesWonSprite.Add (gamesWonNum);
	}



	void ArrangeObj(string strValue)
	{
		for ( var i = 0 ; i < showingGamesWonSprite.Count ; ++i ) {
			xAnchor = GetXAnchor (strValue.Length, i + 1);
			// 表示するSpriteを指定
			showingGamesWonSprite[i].GetComponent<GamesWonNumController>().ChangeGamesWonSprite(dicGamesWonSprite[strValue[i]]);
			//gamesWonNumが持つGamesWonNumクラスのcharNumに値を持たせる
			showingGamesWonSprite[i].GetComponent<GamesWonNum>().charNum = strValue[i];
			//showingGamesWonSprite[i]を配置
			showingGamesWonSprite[i].GetComponent<GamesWonNumController>().MakePlace(xAnchor, yAnchor);
		}
	}




	/// <summary>
	/// GamesWonNumをホームに作る
	/// </summary>
	public void MakeGamesWonAtHomeScreen(float gamesWonNum)
	{
		currentGamesWonNum = gamesWonNum;
		// 表示文字列取得
		string strValue = gamesWonNum.ToString();
		int digit = strValue.Length;

		ChangeWidthOfGamwsWonObj (digit); //gamesWonオブジェクトのwidthを変更
		GenerateSpriteToList(digit); // 表示桁数分だけオブジェクト作成
		PlaceGamesWonNum (strValue); //gamesWonNumを配置
	}

	/// <summary>
	/// 渡した桁数ぶんのOBJを生成
	/// </summary>
	void GenerateSpriteToList(int digit)
	{
		if (showingGamesWonSprite != null)
			showingGamesWonSprite.Clear ();
		
		for(int i = 0; i < digit; i++){
			GameObject gamesWonNum = Instantiate (digitPrefab) as GameObject;
			showingGamesWonSprite.Add (gamesWonNum);
		}

        if (digit == 5 && acpect == 1)
            GamesWonNumSizeChange(70f, 86f);
        if (acpect == 2 && digit == 5)
            GamesWonNumSizeChange(66f, 73.8f);
        if (acpect == 3)
        {
            if (digit == 4)
                GamesWonNumSizeChange(70f, 86f);
            else if(digit == 5)
                GamesWonNumSizeChange(66f, 73.8f);
        }
        if (acpect == 4)
        {
            if (digit == 4)
                GamesWonNumSizeChange(70f, 86f);
            else if (digit == 5)
                GamesWonNumSizeChange(66f, 73.8f);
        }
    }


    public Vector2 GetNumSize()
    {
        Vector2 size = Vector2.zero;
        int digit = showingGamesWonSprite.Count;
        size = sizeDeltaAtHome;

        if (digit == 5 && acpect == 1)
            size = new Vector2(70f, 86f);
        if (acpect == 2 && digit == 5)
            size = new Vector2(66f, 73.8f);
        if (acpect == 3)
        {
            if (digit == 4)
                size = new Vector2(70f, 86f);
            else if (digit == 5)
                size = new Vector2(66f, 73.8f);
        }
        if (acpect == 4)
        {
            if (digit == 4)
                size = new Vector2(70f, 86f);
            else if (digit == 5)
                size = new Vector2(66f, 73.8f);
        }

        return size;
    }

    void GamesWonNumSizeChange(float x, float y)
    {
        for (int i = 0; i < showingGamesWonSprite.Count; i++)
        {
            GameObject obj = showingGamesWonSprite[i];
            obj.GetComponent<GamesWonNumController>().ChangeScale(x, y);
        }
    }


    /// <summary>
    /// 数字を表示
    /// </summary>
    void PlaceGamesWonNum(string strValue)
	{
		for ( var i = 0 ; i < showingGamesWonSprite.Count ; ++i ) {
			xAnchor = GetXAnchor (strValue.Length, i + 1);
			// 表示するSpriteを指定
			showingGamesWonSprite[i].GetComponent<GamesWonNumController>().ChangeGamesWonSprite(dicGamesWonSprite[strValue[i]]);
			//gamesWonNumが持つGamesWonNumクラスのcharNumに値を持たせる
			showingGamesWonSprite[i].GetComponent<GamesWonNum>().charNum = strValue[i];
			// 自身の子階層に移動
			showingGamesWonSprite [i].transform.SetParent (this.gameObject.transform);
			//showingGamesWonSprite[i]を配置
			showingGamesWonSprite[i].GetComponent<GamesWonNumController>().MakePlace(xAnchor, yAnchor);
		}
	}





	void FadeUpOfGamesWonNum(GameObject gamesWonNum)
	{
		gamesWonNum.GetComponent<GamesWonNumController> ().isJumpDown = true;
	}
	/// <summary>
	/// 表示している全てのGamesWonNumをフェードアウトで消す
	/// </summary>
	void FadeOutOfGamesWonNum(GameObject gamesWonNum)
	{
		showingGamesWonSprite.Remove (gamesWonNum);

		gamesWonNum.GetComponent<GamesWonNumController> ().isJumpUp = true;
	}
	/// <summary>
	/// クリア後にgamesWonオブジェクトを表示
	/// </summary>
	void showUpGamesWonNumAtCleared(int digit)
	{
        ChangeTextColor(true);

        TransparentGamesWonObj ();

        isMoveToclearedPos = true;

        ChangeScaleAtCleard(digit);

		foreach (Transform child in gameObject.transform){
			if(child.transform.name != "gamesWonBackG" && child.transform.name != "gamesWonText")
		    	child.GetComponent<GamesWonNumController> ().ChangeScale (sizeDeltaAtCleared);
		}
	}
	void ChangeScaleAtCleard(int digit)
	{
		float xWidthAtCleard = digit * sizeDeltaAtCleared.x;
		gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (xWidthAtCleard, sizeDeltaAtCleared.y);
	}
	void ChangeScaleAtHome(int digit)
	{
		float xWidthAtCleard = digit * sizeDeltaAtHome.x;
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (xWidthAtCleard, sizeDeltaAtHome.y);
	}

	/// <summary>
	/// gamesWonオブジェクトを表示させない
	/// </summary>
	void TransparentGamesWonObj()
	{
		this.gameObject.GetComponent<Button> ().enabled = false;
		color = gameObject.GetComponent<Image> ().color;
		color.a = 0f;
		this.gameObject.GetComponent<Image> ().color = color;
	}
	/// <summary>
	/// gamesWonオブジェクトを表示させる
	/// </summary>
	//void ShowUpGamesWonObj()
	//{
	//	this.gameObject.GetComponent<Button> ().enabled = true;
	//	color = gameObject.GetComponent<Image> ().color;
	//	color.a = 255f;
	//	this.gameObject.GetComponent<Image> ().color = color;
	//}
	/// <summary>
	/// gamesWonオブジェクトのwidthを変更
	/// </summary>
	void ChangeWidthOfGamwsWonObj(int digit)
	{
        bool isSpecialSize = GetIsSpecialGamesWonSize(digit);
        float widthOfGamesWonObj = 0f;

        if (isSpecialSize == false)
            widthOfGamesWonObj = 73f * (float)digit;
        else
            widthOfGamesWonObj = GetSpecialSize(digit);

        //gamesWonオブジェクトのwidthの値を桁数に合わせる
        Vector2 sd = gameObject.GetComponent<RectTransform>().sizeDelta;
        sd.x = widthOfGamesWonObj;
        gameObject.GetComponent<RectTransform>().sizeDelta = sd;
    }
	/// <summary>
	/// 指定した桁のxAnchorを取得
	/// digit = 表示するクリア回数の桁数, index(何桁目か) = 1から始まり、digit以下になる数字(1 = 左端の桁)
	/// </summary>
	float GetXAnchor(int digit, int index)
	{
		float result = 0f;
		float firstDigitXAnchorPos = 1f / digit / 2f;
		float xAnchorPosBtwDigits = 1f / digit;

		if (index == 1) {
			result = firstDigitXAnchorPos;
		}
		else if(index > 1){
			index -= 1;
			result = firstDigitXAnchorPos + xAnchorPosBtwDigits * (float)index;
		}
		return result;
	}
	/// <summary>
	/// 渡したcharの値よりひとつ大きいcharの値を返す
	/// </summary>
	char GetNextChar(char currentChar)
	{
		string s = currentChar.ToString ();
		int i = Convert.ToInt32(s);
		i += 1;
		string willS = i.ToString ();
		char returnChar = willS[0];

		return returnChar;
	}


    bool GetIsSpecialGamesWonSize(int deigit)
    {
        bool result = false;

        if (deigit == 5 && acpect == 1) //3:2
            result = true;
        if (acpect == 2)//5:3
        {
            if(deigit == 4 || deigit == 5)
                result = true;
        }
        if (acpect == 3)//5:3
        {
            if (deigit == 4 || deigit == 5)
                result = true;
        }
        if (acpect == 4)//5:3
        {
            if (deigit == 4 || deigit == 5)
                result = true;
        }

        return result;
    }


    float GetSpecialSize(int deigit)
    {
        float specialWidth = 0f;

        if (deigit >= 5 && acpect == 1)
            specialWidth = 330f;
        else if (acpect == 2)
        {
            if (deigit >= 4)
                specialWidth = 280f;
        }
        else if (acpect == 3)
        {
            if (deigit >= 4)
                specialWidth = 270f;
        }
        else if (acpect == 4)
        {
            if (deigit >= 4)
                specialWidth = 270f;
        }

        return specialWidth;
    }
}
