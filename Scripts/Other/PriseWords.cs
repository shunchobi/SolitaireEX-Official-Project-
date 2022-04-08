using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PriseWords : MonoBehaviour {


    Cash cash;

	Text priseText;
	GameObject priseWords;
    Vector2 showPos;

	List<string> wordsList = new List<string> ();

    bool isOutOfScreen = false;


	bool isMoving = false;
	float elapsedTime = 0f;
	float timeToArrive = 0.8f;

	int calledCount = 0;

	void Start () 
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		LoadAllWords ();
		priseWords = GameObject.Find ("priseWords");	
        priseText = gameObject.GetComponent<Text> ();
        showPos = new Vector2(0f, 500f);
        ClosePriseWords ();
	}

	void LoadAllWords() 
	{
		string filePath = "Localization/" + L.Text.FromKey ("prize_words_file");
        TextAsset csv = Resources.Load<TextAsset>(filePath);
        StringReader reader = new StringReader(csv.text);
        while (reader.Peek () > -1) {
			string row = reader.ReadLine ();
			wordsList.AddRange (row.Split (','));
		}


    }

	void Update () 
	{
		if (isMoving == true) {
			elapsedTime += Time.deltaTime;   // 経過時間
			float t = elapsedTime / timeToArrive;     // 時間を媒介変数に
			if ( t > 1.0f ) t = 1.0f;    // クランプ
			float rate = t * t * ( 3.0f - 2.0f * t );   // 3次関数補間値に変換
			gameObject.GetComponent<RectTransform> ().anchoredPosition 
			= gameObject.GetComponent<RectTransform> ().anchoredPosition * (1.0f - rate) + showPos * rate;   // いわゆるLerp

			if (elapsedTime >= timeToArrive) {
				gameObject.GetComponent<RectTransform> ().anchoredPosition = showPos;
                cash.sound.GetCheerSound();
                cash.finishClearedScores.ShowUp();
                isMoving = false;
				elapsedTime = 0f;
            }
		}
	}


	public void ChangePrizeWord()
	{
		//クリア回数に対して「◯◯回クリアおめでとう！」と褒めるコード
		//GamesWon gamesWon = GameObject.Find("gamesWon").GetComponent<GamesWon>();
		//float gamesWonNum = gamesWon.currentGamesWonNum;
		//priseText.text = gamesWonNum.ToString() + L.Text.FromKey("celebrate wons");

        priseText.text = wordsList[Random.Range(0, wordsList.Count - 1)];
    }


	public void CallToShowPrise()
	{
		ChangePrizeWord ();
        if (isOutOfScreen == true)
        {
            isMoving = true;
            isOutOfScreen = false;
        }
    }
		

	public void ClosePriseWords()
	{
        if (isOutOfScreen == false)
        {
            Vector2 closeDownPos = new Vector2(-1500f, 528f);
            priseWords.GetComponent<RectTransform>().anchoredPosition = closeDownPos;
            isOutOfScreen = true;
        }
    }

}
