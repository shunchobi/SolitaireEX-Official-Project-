using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText_Playing : MonoBehaviour {

    Cash cash;
	Color color;

	GameObject numOfMove_Obj;
	GameObject numOfScore_Obj;
	GameObject numOfMinuteTime_Obj;
	GameObject playingScoreSafeAreaPanel;
	GameObject numOfSecondTime_Obj;

	Text numOfMove_Text;
	Text numOfScore_Text;
	Text numOfMinuteTime_Text;
	Text numOfSecondTime_Text;

	public bool isShowingTimeAndMove = true;

	//Move
	public float move = 0;
	public float aMoved = 1;
	//Score
	public float score = 0f;
	public float isToYama = 10f;
	public float isBackFromYama = -10f;
	public float isFromDeck = 5f;
	public float isFlipedRetu = 5f;
    public float isUsedAllYama = -5f;
	//Time
	public float time_Second = 0;
	public float time_Minute = 0;
	public bool countingTime = false;


	public void Initialize () 
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		numOfMove_Obj = GameObject.Find("numOfMove");
		numOfScore_Obj = GameObject.Find("numOfScore");
		numOfMinuteTime_Obj = GameObject.Find("numOfMinuteTime");
		numOfSecondTime_Obj = GameObject.Find("numOfSecondTime");
		playingScoreSafeAreaPanel = GameObject.Find ("playingScoreSafeAreaPanel");
		numOfMove_Text = numOfMove_Obj.GetComponent<Text>();
		numOfScore_Text = numOfScore_Obj.GetComponent<Text>();
		numOfMinuteTime_Text = numOfMinuteTime_Obj.GetComponent<Text>();
		numOfSecondTime_Text = numOfSecondTime_Obj.GetComponent<Text>();

		TransparentEveryScoreText_Playing ();
	}






	void Update () 
	{
		//他のスクリプトからは一切countingTime = trueにはせず、countingTime = falseを行う。
		//そしてcash.gameDirector.row8_List.Count == 24になるときをUpdateで常に調べつずける
		if(countingTime == true){
			
			time_Second += Time.deltaTime;
			float integerTime_Second = Mathf.FloorToInt (time_Second);

			if (integerTime_Second == 60) {
				integerTime_Second = 0f;
				time_Second = 0f;
				time_Minute += 1f;
			}
			//BoardDataに保存
//			cash.gameDirector.SetFloatOfBoardData("time", integerTime_Second + time_Minute * 60f); //秒単位で渡す
			//分単位の表示
			numOfMinuteTime_Text.text = time_Minute.ToString();
			//秒単位の表示
			if (integerTime_Second < 10f) {
				numOfSecondTime_Text.text = "0" + integerTime_Second.ToString ();
			}
			else if(integerTime_Second >= 10f)
				numOfSecondTime_Text.text = integerTime_Second.ToString();
		}
	}



	public void TransparentEveryScoreText_Playing()
	{
		foreach (Transform child in gameObject.transform) {
			color = child.gameObject.GetComponent<Text>().color;
			color.a = 0f;
			child.gameObject.GetComponent<Text> ().color = color;
		}
	}


	public void ShowUpScoreText_Playing()
	{
		if (isShowingTimeAndMove == true) {
			foreach (Transform child in gameObject.transform) {
				color = child.gameObject.GetComponent<Text> ().color;
				color.a = 255f;
				child.gameObject.GetComponent<Text> ().color = color;
			}
		}

		if (isShowingTimeAndMove == false) {
			foreach (Transform child in gameObject.transform) {
				if (child.transform.name == "score" || child.transform.name == "numOfScore") {
					color = child.gameObject.GetComponent<Text> ().color;
					color.a = 255f;
					child.gameObject.GetComponent<Text> ().color = color;
				}
			}
		}
	}


	public void TransparentTimeAndMove_ScoreText_Playing()
	{
		foreach (Transform child in gameObject.transform) {
			if (child.transform.name != "score") {
				if (child.transform.name != "numOfScore") {
					color = child.gameObject.GetComponent<Text> ().color;
					color.a = 0f;
					child.gameObject.GetComponent<Text> ().color = color;
				}
			}
		}
	}

	public void ShowUpTimeAndMove_ScoreText_Playing()
	{
		DealCard dealCard = GameObject.Find ("generateAndDealCards").GetComponent<DealCard> ();

		if (dealCard.didDeal == true) {
			foreach (Transform child in gameObject.transform) {
				if (child.transform.name != "score") {
					if (child.transform.name != "numOfScore") {
						color = child.gameObject.GetComponent<Text> ().color;
						color.a = 255f;
						child.gameObject.GetComponent<Text> ().color = color;
					}
				}
			}
		}
	}


	/// <summary>
	/// Move
	/// </summary>
	public void AddOneMoveText()
	{
		move += aMoved;
		numOfMove_Text.text = move.ToString ();
	}
	public void MinusOneMoveText()
	{
		move -= aMoved;
		numOfMove_Text.text = move.ToString ();
	}
	public void ResetMove()
	{
		move = 0;
		numOfMove_Text.text = move.ToString ();
	}


	/// <summary>
	/// Score
	/// </summary>
	public void ChangeScoreText(float addScore)
	{
		score += addScore;
		numOfScore_Text.text = score.ToString ();
	}


	public void ResetScore()
	{
		score = 0;
		numOfScore_Text.text = score.ToString ();
	}


	/// <summary>
	/// Time
	/// </summary>
	public void ResetTime()
	{
		countingTime = false;
		time_Second = 0f;
		time_Minute = 0f;
		numOfMinuteTime_Text.text = time_Minute.ToString ();
		numOfSecondTime_Text.text = "0" + time_Second.ToString ();
	}


}
