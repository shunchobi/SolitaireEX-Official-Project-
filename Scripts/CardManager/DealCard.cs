using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Comon;

public class DealCard : MonoBehaviour {

	public GameObject cardPrefab;

	public List <GameObject> cardsList = new List<GameObject> ();
	List<GameObject> lastDealedCards = new List<GameObject> ();

	public bool showUpPlayingScoreText = true; 

	Vector3 interval = new Vector3 (0, 0, 0);
	float time = 0.04f;
	float movingspeed = 0.1f;
	float rollingSpeed = 0.15f;


	float startYPos;
	float endYPos;
	bool isMoving = false;
	bool deal = false;
	public bool didDeal = false;

	public bool dealFromBackToHome = false;
	bool isStartGame = false;
	bool isMouseUped = false;


	float elapsedTime = 0f;
	float timeToArrive = 0.1f;
	float flipDistance;
	Vector3 endPos = new Vector3 (0,0,0);
	public Vector3 oridinalPos = new Vector3 (0,0,0);


	Rigidbody2D rb2D;
    Cash cash;


	public void Init ()
	{
		rb2D = GetComponent<Rigidbody2D>();
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
	}





	void OnMouseDrag()
	{
        if (Input.touchCount > 1)
            return;

        Vector3 objectPointInScreen
		= Camera.main.WorldToScreenPoint (this.rb2D.transform.position);

		Vector3 mousePointInScreen
		= new Vector3(Input.mousePosition.x,
			Input.mousePosition.y,
			objectPointInScreen.z);

		Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
		mousePointInWorld.z = this.rb2D.transform.position.z;
		this.rb2D.transform.position = mousePointInWorld;
	}
		

	void OnMouseUp() 
	{
		flipDistance = cash.placePos.cardHeight * 0.5f;
		endYPos = gameObject.transform.position.y;

		if (oridinalPos.y + flipDistance < endYPos) 
			isStartGame = true;
		
		startYPos = 0;
		endYPos = 0;
	    isMouseUped = true;
	}


	void Update () 
	{
		if (isStartGame == true && isMouseUped == true) {
            cash.sound.GetFlipCardToStartSound();
			if (didDeal == false && dealFromBackToHome == false)
				StartDealCoroutine ();  
			if (didDeal == false && dealFromBackToHome == true)
				StartDealFromBackToHomeCoroutine ();  
			endPos = cash.row1_Obj.transform.position;
			deal = true;
			isMoving = true;
			isStartGame = false;
			isMouseUped = false;
			cash.arrowController.StopInstractionCard ();
		} 

		else if (isStartGame == false && isMouseUped == true) {
            cash.sound.GetMovingCardSound();
			endPos = oridinalPos;
			isMoving = true;
			isMouseUped = false;
		}

		if (isMoving == true) 
		{
			Vector3 startPos = transform.position;
			elapsedTime += Time.deltaTime;   
			float t = elapsedTime / timeToArrive; 
			if ( t > 1.0f ) t = 1.0f;   
			float rate = t * t * ( 3.0f - 2.0f * t );  
			rb2D.position = transform.position * (1.0f - rate) + endPos * rate;  

			if (rb2D.transform.position == endPos) {
				if (deal == true)
					TransparentGenerateAndDealObj ();
				deal = false;
				elapsedTime = 0f;
				isMoving = false;
			}
		}
	}





	public void CreatCardsFromPrefab (Vector3 scale, Vector3 pos)
	{
		dealFromBackToHome = false;

		for (int suit = 1; suit <= 4; suit++) { 
			for (int numOfCard = 1; numOfCard <= 13; numOfCard++) {
				GameObject aCard = Instantiate (cardPrefab) as GameObject;
				Card card = aCard.GetComponent<Card> ();
				aCard.transform.position = pos;
				aCard.transform.localScale = scale;
				card.suit = suit;
				card.numOfCard = numOfCard; 
				card.SetBackSprite();
				aCard.GetComponent<BoxCollider2D> ().enabled = false;
				cardsList.Add (aCard);
                //cash.SetCards(aCard);
			}
		}
	}    



	public void IsWhenBackToHome()
	{
	    isMoving = false;
		deal = false;
		didDeal = false;
	}


	public void TransparentGenerateAndDealObj()
	{
		GetComponent<BoxCollider2D> ().enabled = false;

		var color = GetComponent<SpriteRenderer> ().color;
		color.a = 0f;
		GetComponent<SpriteRenderer> ().color = color;
		gameObject.transform.position = oridinalPos;
	}

	public void ShowUpGenerateAndDealObj()
	{
		GetComponent<BoxCollider2D> ().enabled = true;

		gameObject.transform.position = oridinalPos;
		var color = GetComponent<SpriteRenderer> ().color;
		color.a = 255f;
		GetComponent<SpriteRenderer> ().color = color;
		gameObject.transform.position = oridinalPos;
	}


    public void ChangeColor(bool isShow)
    {
        var color = GetComponent<SpriteRenderer>().color;
        if(isShow == true)
            color.a = 1f;
        if(isShow == false)
            color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        gameObject.transform.position = oridinalPos;
    }



	public void StartDealCoroutine()
	{
		cash.gameDirector.SetGamesCountValue(Const.PLUS_GAME_COUNT);
        //WebAnalytics.GamePlayed(cash.gameDirector.GetTotalGamesCount());
        //WebAnalytics.PlayStyle(cash.touchingDeck.flipAmountIs1);
        cash.saveManager.SaveFile_StatisticsDate ();
		StartCoroutine ("DealCards");
	}


	public void StartDealFromBackToHomeCoroutine()
	{
		StartCoroutine ("DealCardsFromBackToHome");
    }

	public bool isNewGameDealing = false;

	

	IEnumerator DealCards()
	{
        cash.sound.startDealSound = true;

        if (isNewGameDealing == false)
        {
            //cash.gamesWon.StopPopGamesWon();
            cash.showBotton.ClearHome();
        }

        if (cash.placePos.isMakedPos == false)
        {
            cash.placePos.MakeRetuYamaPos();
            if(cash.gameDirector.rightHand == false)
                cash.gameDirector.MoveCardsToRightHand(false);
        }

        if(cash.replay.replayLists.Count > 0)
            cash.replay.ClearReplayList();


        for (int i = 1; i <= 8; i++) {
			for (int k = 1; k <= 24; k++) {
				GameObject aCard = GetAndRemoveRandomly (cardsList);
				AddObjToList (aCard, i);
                cash.replay.AddObjToList_Replay (aCard, i);
				AttachTag (aCard, i);
				PlaceCard (aCard, i);
				if(i != 1) yield return new WaitForSeconds (time);
				if (i == 8)
					continue;
				else if (i == k) {
					lastDealedCards.Add (aCard);
					break;
				}
			}
			interval = new Vector3 (0, 0, 0);
		}

        cash.sound.StopUpdateSound();

		
		for(int i = 0; i < lastDealedCards.Count; i++){
			lastDealedCards[i].GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
			lastDealedCards[i].GetComponent<BoxCollider2D> ().enabled = true;
		}
		lastDealedCards.Clear ();
        cash.sound.GetFlipCardSound();

        if (isNewGameDealing == false)
        {
            cash.showBotton.UiMoving_HomeToPlay();
            //cash.showBotton.AllUiEnable(false);
        }
        isNewGameDealing = false;

        cash.scoreText_Playing.ResetMove();
        cash.scoreText_Playing.ResetScore();
        cash.scoreText_Playing.ResetTime();
        //if (showUpPlayingScoreText == true) 
			cash.scoreText_Playing.ShowUpScoreText_Playing ();
		cash.showBotton.dealed = true;
		FixBoxCollider2DEnabledToPlay ();
		cash.gameDirector.ShowUpRetuYamaRow8BackG ();
		cash.scoreText_Playing.countingTime = true;
        //cash.showBotton.AllUiEnable (true);
        cash.gameDirector.isPlayingSecen = true;

        //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.Game);
        yield return new WaitForSeconds(1f);
        didDeal = true;
		cash.gamesWon.SetPosAtOutOfScreen();

        bool isFirstTimeAsOpeningApp = cash.gameDirector.GetIsFirstTimeAsOpeningApp();
        if(isFirstTimeAsOpeningApp == true)
        {
            yield return new WaitForSeconds(0.4f);
            ShowMassageBox showMassageBox = GameObject.Find("callMassageBox").GetComponent<ShowMassageBox>();
            showMassageBox.SuggestHowToPlay();
            cash.gameDirector.BeFalseIsFirstTimeAsOpeningApp();
        }
    }





    void PlaceCard (GameObject obj, int row_Num)
	{
		Vector3 endPos = GetPosToDeal (row_Num);
		obj.GetComponent<SpriteRenderer> ().sortingOrder = 90;
		obj.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingspeed, false);
		if(row_Num != 1 || row_Num != 8)
			interval += new Vector3 (0, cash.placePos.intervalBackCards, 0); 
	}



	
	void AddObjToList(GameObject obj, int row_Num)
	{
		switch (row_Num) 
		{
		case 1:
		    cash.gameDirector.row1_List.Add (obj); 
			break;
		case 2:
			cash.gameDirector.row2_List.Add (obj); 
			break;
		case 3:
			cash.gameDirector.row3_List.Add (obj); 
			break;
		case 4:
			cash.gameDirector.row4_List.Add (obj); 
			break;
		case 5:
			cash.gameDirector.row5_List.Add (obj); 
			break;
		case 6:
			cash.gameDirector.row6_List.Add (obj); 
			break;
		case 7:
			cash.gameDirector.row7_List.Add (obj); 
			break;
		case 8:
			cash.gameDirector.row8_List.Add (obj); 
			break;
		}
	}



	
	void AttachTag(GameObject obj, int tagPlace)
	{
		if (tagPlace != 8)
			obj.transform.tag = "retu";
		else 
			obj.transform.tag = "deck";
	}



	
	Vector3 GetPosToDeal(int row_Num)
	{
		Vector3 endPos = new Vector3 (0, 0, 0);
		switch (row_Num) 
		{
		case 1:
			endPos = cash.row1_Obj.transform.position;
			break;
		case 2:
			endPos = cash.row2_Obj.transform.position - interval;
			break;
		case 3:
			endPos = cash.row3_Obj.transform.position - interval;
			break;
		case 4:
			endPos = cash.row4_Obj.transform.position - interval;
			break;
		case 5:
			endPos = cash.row5_Obj.transform.position - interval;
			break;
		case 6:
			endPos = cash.row6_Obj.transform.position - interval;
			break;
		case 7:
			endPos = cash.row7_Obj.transform.position - interval;
			break;
		case 8:
			endPos = cash.row8_Obj.transform.position;
			break;
		}
		return endPos;
	}





	
	public static T GetAndRemoveRandomly<T>(List<T> list)
	{
		T target = list[Random.Range(0, list.Count)];
		list.Remove (target);
		return target;
	}



	
	public void FixBoxCollider2DEnabledToPlay()
	{
		cash.row1_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		cash.row2_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		cash.row3_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		cash.row4_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		cash.row5_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		cash.row6_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		cash.row7_Obj.GetComponent<BoxCollider2D> ().enabled = false;
		cash.row8_Obj.GetComponent<BoxCollider2D> ().enabled = true; 
        if(cash.gameDirector.row9_List.Count == 0)
    		cash.row9_Obj.GetComponent<BoxCollider2D> ().enabled = true;
        if (cash.gameDirector.row10_List.Count == 0)
            cash.row10_Obj.GetComponent<BoxCollider2D> ().enabled = true;
        if (cash.gameDirector.row11_List.Count == 0)
            cash.row11_Obj.GetComponent<BoxCollider2D> ().enabled = true;
        if (cash.gameDirector.row12_List.Count == 0)
            cash.row12_Obj.GetComponent<BoxCollider2D> ().enabled = true;
	}


    public void FixBoxCollider2DEnabledToHome()
    {
        cash.row1_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row2_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row3_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row4_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row5_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row6_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row7_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row8_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row9_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row10_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row11_Obj.GetComponent<BoxCollider2D>().enabled = false;
        cash.row12_Obj.GetComponent<BoxCollider2D>().enabled = false;
    }






    IEnumerator DealCardsFromBackToHome()
	{
        List<GameObject> enableTrueCards = new List<GameObject>();
		GetComponent<BoxCollider2D> ().enabled = false;
		//cash.sound.startDealSound = true;
		cash.sound.GetMoveCardToHomeSound();
		//cash.gamesWon.StopPopGamesWon();
        cash.showBotton.ClearHome();

        for (int i = 1; i <= 13; i++) {
			List<GameObject> list = GetList (i);
			Vector3 pos = GetPosToDealFromBackToHome (i);
			bool isToInterval_Acard = GetTrueOrFalse (i);

			if (list.Count != 0) {
				for(int k = 0; k < list.Count; k++){
					GameObject aCard = list [k];
                    bool isTurned = cash.gameDirector.isTurnedOrNot[0];
                    aCard.GetComponent<CardsDirector> ().SetInfoToAnimation (true, pos, 1.8f, false);　//movingspeed
					if (isTurned == true) 
						aCard.GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
					if (isToInterval_Acard == true) {
						if(isTurned == false) pos = new Vector3 (pos.x, pos.y - cash.placePos.intervalBackCards, pos.z);
						if(isTurned == true) pos = new Vector3 (pos.x, pos.y - cash.placePos.intervalFrontCards, pos.z);
					}
					cash.gameDirector.isTurnedOrNot.RemoveAt (0);

                    if(i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 7)
                    {
                        if (isTurned == true)
                            enableTrueCards.Add(aCard);
                    }
                    if (i == 9 || i == 10 || i == 11 || i == 12 || i == 13)
                    {
                        if (k == list.Count - 1)
                            enableTrueCards.Add(aCard);
                    }

                    //yield return new WaitForSeconds (time);
				}
			}
		}

        yield return new WaitForSeconds(0.5f);

        cash.touchingDeck.FixOpenDeck ();
        cash.sound.StopUpdateSound();
		cash.scoreText_Playing.ShowUpScoreText_Playing ();
		cash.gameDirector.ShowUpRetuYamaRow8BackG ();
		cash.scoreText_Playing.countingTime = true;
		dealFromBackToHome = false;
        FixBoxCollider2DEnabledToPlay();

        yield return new WaitForSeconds(movingspeed);
        for (int i = 0; i < enableTrueCards.Count; i++)
        {
            enableTrueCards[i].GetComponent<BoxCollider2D>().enabled = true;
        }
        enableTrueCards.Clear();

		yield return new WaitForSeconds(0.5f);
		cash.showBotton.UiMoving_HomeToPlay();
		yield return new WaitForSeconds(0.5f);
		cash.gameDirector.isPlayingSecen = true;

		didDeal = true;

    }


    bool GetTrueOrFalse(int num)
	{ 
		bool flag = new bool ();

		switch (num) 
		{
		case 1:
			flag = true; 
			break;
		case 2:
			flag = true; 
			break;
		case 3:
			flag = true; 
			break;
		case 4:
			flag = true; 
			break;
		case 5:
			flag = true; 
			break;
		case 6:
			flag = true; 
			break;
		case 7:
			flag = true; 
			break;
		case 8:
			flag = false; 
			break;
		case 9:
			flag = false; 
			break;
		case 10:
			flag = false; 
			break;
		case 11:
			flag = false; 
			break;
		case 12:
			flag = false; 
			break;
		case 13:
			flag = false; 
			break;
		}

		return flag;
	}



	List<GameObject> GetList(int listName)
	{
		List<GameObject> list = new List<GameObject> ();

		switch (listName) 
		{
		case 1:
			list = cash.gameDirector.row1_List; 
			break;
		case 2:
			list = cash.gameDirector.row2_List; 
			break;
		case 3:
			list = cash.gameDirector.row3_List; 
			break;
		case 4:
			list = cash.gameDirector.row4_List; 
			break;
		case 5:
			list = cash.gameDirector.row5_List; 
			break;
		case 6:
			list = cash.gameDirector.row6_List; 
			break;
		case 7:
			list = cash.gameDirector.row7_List; 
			break;
		case 8:
			list = cash.gameDirector.row8_List; 
			break;
		case 9:
			list = cash.gameDirector.row9_List; 
			break;
		case 10:
			list = cash.gameDirector.row10_List; 
			break;
		case 11:
			list = cash.gameDirector.row11_List; 
			break;
		case 12:
			list = cash.gameDirector.row12_List; 
			break;
		case 13:
			list = cash.gameDirector.openDeck_List; 
			break;
		}

		return list;
	}


	Vector3 GetPosToDealFromBackToHome(int row_Num)
	{
		Vector3 endPos = new Vector3 (0, 0, 0);

		switch (row_Num) 
		{
		case 1:
			endPos = cash.row1_Obj.transform.position;
			break;
		case 2:
			endPos = cash.row2_Obj.transform.position - interval;
			break;
		case 3:
			endPos = cash.row3_Obj.transform.position - interval;
			break;
		case 4:
			endPos = cash.row4_Obj.transform.position - interval;
			break;
		case 5:
			endPos = cash.row5_Obj.transform.position - interval;
			break;
		case 6:
			endPos = cash.row6_Obj.transform.position - interval;
			break;
		case 7:
			endPos = cash.row7_Obj.transform.position - interval;
			break;
		case 8:
			endPos = cash.row8_Obj.transform.position;
			break;
		case 9:
			endPos = cash.row9_Obj.transform.position;
			break;
		case 10:
			endPos = cash.row10_Obj.transform.position;
			break;
		case 11:
			endPos = cash.row11_Obj.transform.position;
			break;
		case 12:
			endPos = cash.row12_Obj.transform.position;
			break;
		case 13:
			endPos = cash.row8_1Obj.transform.position;
			break;
		}

		return endPos;
	}



    public void PlaceDealCardsList(float time)
    {
        for(int i = 0; i < cardsList.Count; i++)
        {
            GameObject obj = cardsList[i];
            obj.GetComponent<CardsDirector>().SetInfoToAnimation(true, gameObject.transform.position, time, false);
        }
    }



}