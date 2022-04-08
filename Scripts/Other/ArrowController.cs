using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour {

	Color color;

    Cash cash;

	GameObject arrow;
	GameObject generateAndDealCards_Obj;
	GameObject hand;

	//world座標
	Vector3 startPos = Vector3.zero;
	Vector3 finishPos = Vector3.zero;


	bool isMovingTofinish = false;
	bool isMovingToStart = false;

	float elapsedTime = 0;
	float timeToArriveToFinishPos = 0.5f;
	float timeToArriveToStartPos = 1.5f;



	void Start () 
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		arrow = GameObject.Find ("arrow");
		generateAndDealCards_Obj = GameObject.Find ("generateAndDealCards");
		hand = GameObject.Find ("hand");

		InitializePos(generateAndDealCards_Obj);
		MoveInstractionCard ();
	}




	void Update() 
	{
		if (isMovingTofinish == true) {
			elapsedTime += Time.deltaTime;   // 経過時間
			float t = elapsedTime / timeToArriveToFinishPos;     // 時間を媒介変数に
			if (t > 1.0f)
				t = 1.0f;    // クランプ
			float rate = t * t * (3.0f - 2.0f * t);   // 3次関数補間値に変換
			gameObject.GetComponent<RectTransform> ().position 
			= gameObject.GetComponent<RectTransform> ().position * (1.0f - rate) + finishPos * rate;

			if (elapsedTime >= timeToArriveToFinishPos) {
				elapsedTime = 0f;
				isMovingTofinish = false;
				isMovingToStart = true;
			}
		}

		if (isMovingToStart == true) {
			elapsedTime += Time.deltaTime;   // 経過時間
			float t = elapsedTime / timeToArriveToStartPos;     // 時間を媒介変数に
			if (t > 1.0f)
				t = 1.0f;    // クランプ
			float rate = t * t * (3.0f - 2.0f * t);   // 3次関数補間値に変換
			gameObject.GetComponent<RectTransform> ().position 
			= gameObject.GetComponent<RectTransform> ().position * (1.0f - rate) + startPos * rate;

			if (elapsedTime >= timeToArriveToStartPos) {
				elapsedTime = 0f;
				isMovingToStart = false;
				isMovingTofinish = true;
			}
		}

	}


	/// <summary>
	/// それぞれのscaleは、cash.placePosクラスで定義
	/// </summary>
	public void InitializePos(GameObject dealobj)
	{
		Vector3 dealObjScreenPos = RectTransformUtility.WorldToScreenPoint (Camera.main, dealobj.transform.position);
		startPos = dealObjScreenPos;
		float delta = (float)Screen.height / 9.5f;
		finishPos = new Vector3 (dealObjScreenPos.x, dealObjScreenPos.y + delta, 0);

		gameObject.GetComponent<RectTransform> ().position = startPos;
        gameObject.GetComponent<Image> ().sprite = cash.setting.backSprite;
	}




	public void MoveInstractionCard()
	{
        gameObject.GetComponent<Image> ().sprite = cash.setting.backSprite;
		gameObject.GetComponent<RectTransform> ().position = startPos;
		ChangeColor(true);
		isMovingTofinish = true;
	}


	public void StopInstractionCard()
	{
		ChangeColor(false);
		isMovingTofinish = false;
		isMovingToStart = false;
		elapsedTime = 0f;
	}



	void ChangeColor(bool isShowUp)
	{
		color = arrow.GetComponent<Image> ().color;
		if (isShowUp == true)
			color.a = 255f/255f;
		if (isShowUp == false)
			color.a = 0/255f;
		arrow.GetComponent<Image> ().color = color;

		color = hand.GetComponent<Image> ().color;
		if (isShowUp == true)
			color.a = 255f/255f;
		if (isShowUp == false)
			color.a = 0/255f;
		hand.GetComponent<Image> ().color = color;

	}

}
