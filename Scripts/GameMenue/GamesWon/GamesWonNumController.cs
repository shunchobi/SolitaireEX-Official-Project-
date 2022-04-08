using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamesWonNumController : MonoBehaviour {


    Cash cash;

    Color color;

	public bool isJumpUp = false;
	public bool isJumpDown = false;
	public bool isCahegingScale = false; 

	const float stopTiming = 60f;
	float count = 0f;
	float elapsedTime = 0f;

	const float aspect = 1.33f;
	const float jumpSpeed = 10f;


	void Start()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
    }


	void Update()
	{
		if (isJumpUp == true) {
			Vector2 sd = this.gameObject.GetComponent<RectTransform>().sizeDelta;
			sd.x += jumpSpeed;
			sd.y += jumpSpeed * aspect;
			gameObject.GetComponent<RectTransform>().sizeDelta = sd;

			count += jumpSpeed;
			if (count >= stopTiming) {
                cash.sound.GetRiseNumSound();
                isJumpUp = false;
				isJumpDown = true;
			}
		}
		//大きくしたオブジェクトを元のサイズに戻す
		if (isJumpDown == true) {
			Vector2 sd = this.gameObject.GetComponent<RectTransform>().sizeDelta;
			sd.x -= jumpSpeed;
			sd.y -= jumpSpeed * aspect;
			gameObject.GetComponent<RectTransform>().sizeDelta = sd;

			count -= jumpSpeed;
			if (count <= 0f) {
				ChangeScale (GamesWon.sizeDeltaAtCleared);
                cash.gameDirector.ProcessEndingAsScoring();
                count = 0f;
				isJumpDown = false;

			}
		}
		//クリア後のスコア画面から、ホーム画面に移る時に、オブジェクトのサイズをGamesWonObjと同じようにだんだん小さくする
		if(isCahegingScale == true){
			elapsedTime += Time.deltaTime; 
			if (elapsedTime >= GamesWon.timeToArrive)
				elapsedTime = GamesWon.timeToArrive;

			float timePercentage = elapsedTime / GamesWon.timeToArrive;
			float xNewScale = GamesWon.sizeDeltaAtCleared.x - timePercentage * GamesWon.xDeltaScale;
			float yNewScale = GamesWon.sizeDeltaAtCleared.y - timePercentage * GamesWon.yDeltaScale;

			if (xNewScale <= cash.gamesWon.GetNumSize().x)
				xNewScale = cash.gamesWon.GetNumSize().x;
			if (yNewScale <= cash.gamesWon.GetNumSize().y)
				yNewScale = cash.gamesWon.GetNumSize().y;

			ChangeScale (xNewScale, yNewScale);

			if(GamesWon.moveToHome == false){
                ChangeScale(cash.gamesWon.GetNumSize().x, cash.gamesWon.GetNumSize().y);
		        isCahegingScale = false; 
	         	elapsedTime = 0f;
			}
 			
		}
	}


	public void ChangeScale(float xScale, float yScale)
	{
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (xScale, yScale);
	}



	public void ChangeGamesWonSprite(Sprite sprite)
	{
		this.gameObject.GetComponent<Image> ().sprite = sprite;
	}


	public void MakePlace(float xAnchor, float yAnchor)
	{
		RectTransform rect = this.gameObject.GetComponent<RectTransform>();
		rect.anchorMin = new Vector2 (xAnchor, yAnchor);
		rect.anchorMax = new Vector2 (xAnchor, yAnchor);
		rect.anchoredPosition = new Vector3 (0f,0f,0f);
	}


	public void ChangeScale(Vector2 scale)
	{
		RectTransform rect = this.gameObject.GetComponent<RectTransform>();
		rect.sizeDelta = scale;
	}


}
