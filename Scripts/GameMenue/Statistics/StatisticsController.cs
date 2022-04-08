using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatisticsController : MonoBehaviour {


    Cash cash;
    StatisticsTextController statisticsTextController;

	GameObject statisticsFlipOne;
	GameObject statisticsFlipThree;


	Vector2 endPos;
	public Vector2 statisticsOnPos;
	public Vector2 statisticsOffPos;

	bool isMoveToStatisticsPos = false;
	float timeToArriveStatistics = 0.5f;
	float elapsedTime = 0f;

	Sprite scanSprite;
	Sprite isntScanSprite;
	bool isScanScore = false;

	Button oneFlipButton;
	Button threeFlipButton;
	bool isthreeFlipScore = false;

	public bool isShowing = false;

	Button cancelButton;

	public void Init () 
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        statisticsTextController = gameObject.GetComponent<StatisticsTextController>();
        statisticsTextController.Init();
        statisticsFlipOne = GameObject.Find("statisticsFlipOne");
        statisticsFlipThree = GameObject.Find("statisticsFlipThree");
        statisticsOffPos = new Vector2(0, -600f);
        statisticsOnPos = new Vector2(0, 450f);
        scanSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("Images" + "/" + "statistics" + "/" + "statistics_scan_btn_on");
        isntScanSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("Images" + "/" + "statistics" + "/" + "statistics_scan_btn_off");
        oneFlipButton = GameObject.Find("statisticsFlipOneButton").GetComponent<Button>();
        threeFlipButton = GameObject.Find("statisticsFlipThreeButton").GetComponent<Button>();
        cancelButton = GameObject.Find("statisticsCancel").GetComponent<Button>();
        ChangeButtonEnabled(oneFlipButton, false);
        ChangeButtonEnabled(threeFlipButton, true);
        GameObject.Find("BestTime").GetComponent<Text>().text = L.Text.FromKey("best time");
        GameObject.Find("BestMove").GetComponent<Text>().text = L.Text.FromKey("best moves");
        GameObject.Find("BestScore").GetComponent<Text>().text = L.Text.FromKey("best score");
        GameObject.Find("BestTotalScore").GetComponent<Text>().text = L.Text.FromKey("best total score");
        GameObject.Find("WinningPercentage").GetComponent<Text>().text = L.Text.FromKey("percantage of won");
        GameObject.Find("GameWonCount").GetComponent<Text>().text = L.Text.FromKey("games won");
        GameObject.Find("GameCount").GetComponent<Text>().text = L.Text.FromKey("game played");

        GameObject.Find("TotalGameCount").GetComponent<Text>().text = L.Text.FromKey("total games played");
        GameObject.Find("TotalPlayTime").GetComponent<Text>().text = L.Text.FromKey("total played time");
    }


    void Update () 
	{

		if (isMoveToStatisticsPos == true) {
			elapsedTime += Time.deltaTime;   // 経過時間
			float t = elapsedTime / timeToArriveStatistics;     // 時間を媒介変数に
			if ( t > 1.0f ) t = 1.0f;    // クランプ
			float rate = t * t * ( 3.0f - 2.0f * t );   // 3次関数補間値に変換
			gameObject.GetComponent<RectTransform> ().anchoredPosition 
			= gameObject.GetComponent<RectTransform> ().anchoredPosition * (1.0f - rate) + endPos * rate;   // いわゆるLerp

			if (elapsedTime >= timeToArriveStatistics) {
				gameObject.GetComponent<RectTransform> ().anchoredPosition = endPos;
				ChangeButtonEnabled (cancelButton, true);
				elapsedTime = 0f;
				isMoveToStatisticsPos = false;
                if (isShowing == false)
                {
                    Destroy(gameObject);
                    cash.statisticsPanelObj.SetActive(false);
                }
			}
		}
	}


	public void MoveToShow()
    {
        cash.statisticsPanelObj.SetActive (true);
        gameObject.transform.parent = cash.statisticsPanelObj.transform;
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        size.x = 579f;
        size.y = 824f;
        gameObject.GetComponent<RectTransform>().sizeDelta = size;
        gameObject.transform.localScale = new Vector3(1f, 1f, 0f);

        gameObject.GetComponent<RectTransform>().anchoredPosition = statisticsOffPos;

        ChangeColor (true);
		isShowing = true;
        statisticsTextController.DisplayUpperSideStatistics (isthreeFlipScore, isScanScore);
        statisticsTextController.DisplayBelowSideStatistics ();
		endPos = statisticsOnPos;
		isMoveToStatisticsPos = true;

        GameObject.Find("generateAndDealCards").GetComponent<BoxCollider2D>().enabled = false;
        GameObject.Find("instractionCard").GetComponent<ArrowController>().StopInstractionCard();
        GameObject setting = GameObject.Find("setting");
        setting.GetComponent<Button>().enabled = false;
        Color color = setting.GetComponent<Image>().color;
        color.r = 154f / 255f;
        color.g = 154f / 255f;
        color.b = 154f / 255f;
        color.a = 179f / 255f;
        setting.GetComponent<Image>().color = color;

    }


    void MoveToBeOff()
	{
        gameObject.transform.parent = GameObject.Find("homeMenu").transform;
        gameObject.transform.SetSiblingIndex(5);
        isShowing = false;
		endPos = statisticsOffPos;
		isMoveToStatisticsPos = true;

        GameObject.Find("generateAndDealCards").GetComponent<BoxCollider2D>().enabled = true;
        GameObject.Find("instractionCard").GetComponent<ArrowController>().MoveInstractionCard();
        GameObject setting = GameObject.Find("setting");
        setting.GetComponent<Button>().enabled = true;
        Color color = setting.GetComponent<Image>().color;
        color.r = 255f / 255f;
        color.g = 255f / 255f;
        color.b = 255f / 255f;
        color.a = 255f / 255f;
        setting.GetComponent<Image>().color = color;
    }



    /// <summary>
    /// Scoreの中にある一枚めくる、三枚めくるのタブが押されたときの処理
    /// 表示するスコアの内容を、一枚めくる、三枚めくるに変更
    /// </summary>
    public void FlipButton()
	{
        cash.sound.GetTapCardSound();
		//3枚めくりのscoreを表示
		if (isthreeFlipScore == false) {
			isthreeFlipScore = true;
            statisticsTextController.DisplayUpperSideStatistics (isthreeFlipScore, isScanScore);
			ChangeButtonEnabled (oneFlipButton, true);
			ChangeButtonEnabled (threeFlipButton, false);
			statisticsFlipThree.transform.SetSiblingIndex(2);
			return;
		}
		//1枚めくりのscoreを表示
		if (isthreeFlipScore == true) {
			isthreeFlipScore = false;
            statisticsTextController.DisplayUpperSideStatistics (isthreeFlipScore, isScanScore);
			ChangeButtonEnabled (oneFlipButton, false);
			ChangeButtonEnabled (threeFlipButton, true);
			statisticsFlipOne.transform.SetSiblingIndex(2);
		}

	}


	/// <summary>
	/// Scoreの中にあるScanボタンが押されたときの処理
	/// 表示するスコアの内容を、スキャンあり、スキャンありに変更
	/// </summary>
	public void ScanButton()
	{
        cash.sound.GetSelectContentSound();
		//Scanありのscoreを表示
		if (isScanScore == false) {
			isScanScore = true;
            statisticsTextController.DisplayUpperSideStatistics (isthreeFlipScore, isScanScore);
            Text text = GameObject.Find("scanedText").GetComponent<Text>();
            text.text = L.Text.FromKey("scaned");
            return;
		}
		//Scanなしのscoreを表示
		if (isScanScore == true) {
			isScanScore = false;
            statisticsTextController.DisplayUpperSideStatistics (isthreeFlipScore, isScanScore);
            Text text = GameObject.Find("scanedText").GetComponent<Text>();
            text.text = L.Text.FromKey("no scaned");
        }
    }



	/// <summary>
	/// スコア画面を閉じる
	/// </summary>
	public void CancelButton()
	{
        cash.sound.GetSelectContentSound();
        ChangeColor (false);
		ChangeButtonEnabled (cancelButton, false);
		if (cash.dealCard.didDeal == false)
		{
			MoveToBeOff();
            cash.gamesWon.SetPosHomeOrOutOfScreen(true);
            cash.gamesWon.MoveToBeOffFormScoreShowing();
        }
        else
        {
            gameObject.transform.parent = GameObject.Find("homeMenu").transform;
            gameObject.transform.SetSiblingIndex(5);
            isShowing = false;
            endPos = statisticsOffPos;
            isMoveToStatisticsPos = true;
            GameObject setting = GameObject.Find("setting");
            setting.GetComponent<Button>().enabled = true;
            Color color = setting.GetComponent<Image>().color;
            color.r = 255f / 255f;
            color.g = 255f / 255f;
            color.b = 255f / 255f;
            color.a = 255f / 255f;
            setting.GetComponent<Image>().color = color;
            cash.gamesWon.SetPosHomeOrOutOfScreen(false);
            cash.gamesWon.MoveToBeOffFormScoreShowing();

            cash.scoreText_Playing.countingTime = true;
            cash.gameDirector.BeFalseOrTrueOpenCards_Retu(true);
            cash.gameDirector.BeFalseOrTrueOpenCards_Yama(true, true);

        }
    }


	/// <summary>
	/// 渡した buttonの enabledを変更
	/// </summary>
	void ChangeButtonEnabled(Button button, bool enabled)
	{
		button.enabled = enabled;
	}


	void ChangeColor(bool isShowUp)
	{
        Color color = cash.statisticsPanelObj.GetComponent<Image> ().color;
		if (isShowUp == true)
			color = new Color (0.3f,0.3f,0.3f,0.6f);
		if (isShowUp == false)
			color = new Color (0,0,0,0);
        cash.statisticsPanelObj.GetComponent<Image> ().color = color;
	}

}
