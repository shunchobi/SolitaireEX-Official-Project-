using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Hint : MonoBehaviour {


	public List<GameObject> hintCardsList = new List<GameObject>();

    Cash cash;

	public GameObject prefabCard;
	GameObject willMove_Copy;
	GameObject beAccepted_Obj;

	bool existOya;
	bool willBeYama;

    private float timeToWait;
    float movingSpeed_Hint;
    float rollingSpeed_Hint;



	void Start () 
	{
        timeToWait = 0.1f;
        movingSpeed_Hint = 2.5f;
        rollingSpeed_Hint = 0.8f;
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
	}



	public void OnMouseUp()
	{
        cash.sound.GetButtonSound();
		StartCoroutine("ShowHint");
	}


	public IEnumerator ShowHint()
	{
		GetComponent<Button>().enabled = false; //Playerが連続でHintボタンを押した時のエラー防止

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//山へ移動できるカードがあるかを探す
		for (int i = 1; i < 9; i++) {
			willMove_Copy = cash.gameDirector.GetLastCardInList (i); //for文でrow1〜row8_1のListの最後のカードを取得
			if (willMove_Copy != null) { //それぞれのListの最後のカードが存在する場合
				existOya = cash.gameDirector.CheckExistOya_1 (willMove_Copy); //取得したカードに移動先があるか調べる
				if (existOya == true) { //移動先がある場合
					beAccepted_Obj = cash.gameDirector.GetOya ();
					willBeYama = true;
					break;
				}
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//山へ移動できるカードがなかった場合、列へ移動できるカードがあるかを探す(空オブジェクトの上のカード → retu、deck → retu)
		if(existOya == false){
			for (int k = 1; k < 9; k++) {
				willMove_Copy = cash.gameDirector.GetLastCardInList_1 (k); //for文でrow8_1か、row1〜row7のListの裏側の下か空オブジェクトの上にある表のカードを渡す
				//空オブジェクトの上にあるKカードが、他の空オブジェクトに移動できることを知らせないようにする（意味のない移動だから）
				if(willMove_Copy != null){
					if (cash.gameDirector.GetIndexNum (willMove_Copy) == 0 && willMove_Copy.GetComponent<Card> ().numOfCard == 13) {
						willMove_Copy = null;
					}
				}
				if (willMove_Copy != null) {
					existOya = cash.gameDirector.CheckExistOya_2 (willMove_Copy); //取得したカードに移動先があるか調べる
					if (existOya == true) {
						beAccepted_Obj = cash.gameDirector.GetOya ();
						willBeYama = false;
					    break;
					}
				}
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//移動可能なカードがyamaかretuに存在する場合の処理
		if (existOya == true) {
            float interval_Acard = cash.placePos.intervalFrontCards;
            Vector3 oridinalPos = willMove_Copy.transform.position; //移動するカードの座標
			Vector3 endPos = new Vector3(0,0,0); //移動先の座標
			if(willBeYama == false) 
				endPos = new Vector3 (beAccepted_Obj.transform.position.x, beAccepted_Obj.transform.position.y - interval_Acard, beAccepted_Obj.transform.position.z);
			if(willBeYama == true || beAccepted_Obj.transform.tag == "retu_empty" || beAccepted_Obj.transform.tag == "yama_empty") 
				endPos = beAccepted_Obj.transform.position;

			AddAllTargetPrefabCardsToList (willMove_Copy, hintCardsList);//willMove_Copy自身、存在するなら子供もhintCardsListへ加える

			//hintCardsListの中身（子供も含むすべて）を移動させる、移動するカードの枠に線を引く
			if (hintCardsList.Count == 1) {
				hintCardsList [0].transform.tag = "hint";
				hintCardsList [0].GetComponent<SpriteRenderer> ().sortingOrder = 110;
				hintCardsList [0].GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed_Hint, false);
			}

			if (hintCardsList.Count > 1) {
				int lastIndex = hintCardsList.Count - 1;
			    for (int i = 0; i < hintCardsList.Count; i++) {
					hintCardsList [i].GetComponent<SpriteRenderer> ().sortingOrder = 110 + i;
					hintCardsList [i].transform.tag = "hint";
					hintCardsList [i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed_Hint, false);
					endPos.y -= interval_Acard;
			   }
			}
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//移動可能なカードがyamaかretuに存在せず、deckのカードがヒントの対象になる場合の処理
		} else if (existOya == false) { 
			int flipCardsAmount = 0; //row8からめくられる枚数
			bool flipamountIs1 = cash.touchingDeck.flipAmountIs1;

     		//デッキを点滅
			if (cash.gameDirector.row8_List.Count > 0) {
                for(int i = 0; i < cash.gameDirector.row8_List.Count; i++)
                {
                    GameObject aCard = cash.gameDirector.row8_List[i];
                    aCard.GetComponent<CardsDirector>().StartBlink(0.5f, 2f);
                }
			}
            //row8_1をdeckへ
            else if (cash.gameDirector.row8_List.Count == 0 && cash.gameDirector.openDeck_List.Count > 0)
                GameObject.Find("row8").GetComponent<Row8Blinker>().StartBlink(0.5f, 2f);
		} 
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		yield return new WaitForSeconds (1.4f); //hintカードが完全に移動する前に一行しての処理で消されてしまうため少し待てせる
        if(hintCardsList.Count > 0)
            TransparentToDestroyObjOfList (hintCardsList);
		beAccepted_Obj = null;
		willMove_Copy = null;
		existOya = false;
		GetComponent<Button>().enabled = true; //Playerが連続でHintボタンを押した時のエラー防止
	}



	/// <summary>
	/// 渡したListに存在するオブジェクトをTransparentToDestroyしListを空にする
	/// </summary>
	public void TransparentToDestroyObjOfList(List<GameObject> pass){
		for(int i = 0; i < pass.Count; i++){
			pass[i].GetComponent<CardsDirector> ().TransparentToDestroy (true);
		}
		pass.Clear ();
	}



	/// <summary>
	/// 渡したListに存在するオブジェクトをDestroyしListを空にする
	/// </summary>
	public void DestroyObjOfList(List<GameObject> pass){
		if(pass.Count > 0){
		 for(int i = 0; i < pass.Count; i++){
			Destroy (pass[i], 0.1f);
		}
			pass.Clear ();
		}
	}



	public void LetCardsBeTransparent(List<GameObject> pass)
	{
		for(int i = 0; i < pass.Count; i++){
			var color = pass [i].GetComponent<SpriteRenderer> ().color;
			color.a = 0;
			pass [i].GetComponent<SpriteRenderer> ().color = color;
		}
	}


	/// <summary>
	/// 渡したリストに、渡したカードとその子供達分のインスタンス化したカードを作り、インスタンス化したカードに情報を持たせる
	/// </summary>
	void AddAllTargetPrefabCardsToList(GameObject card, List<GameObject> list)
	{
		int indexNum = cash.gameDirector.GetIndexNum (card);
		int lastIndexNum = cash.gameDirector.GetLastIndexNum (card);
		List<GameObject> cardList = cash.gameDirector.GetListOfObj (card);

		if (indexNum != lastIndexNum) {
			for (int i = 0; i <= lastIndexNum - indexNum; i++) {
				GameObject card_Hint = Instantiate (prefabCard) as GameObject;
				AddInformationToCard (card_Hint, cardList [indexNum + i], 110 + i);
				list.Add (card_Hint);
			}
		}

		if (indexNum == lastIndexNum) {
			GameObject card_Hint = Instantiate (prefabCard) as GameObject;
			AddInformationToCard (card_Hint, cardList [cardList.Count - 1], 110);
			list.Add (card_Hint);
		}

	}



	/// <summary>
	/// hintカードがrow8からrow8_1へ移動する場合で、flipCardsAmountの枚数によってlistに加えるカードをかえる
	/// </summary>
	void AddDeckPrefabCradsToList(int flipCardsAmount, List<GameObject> list, bool flipamountIs1)
	{
		List<GameObject> row8_List = cash.gameDirector.row8_List;
		List<GameObject> row8_1List = cash.gameDirector.openDeck_List;
		int lastIndex_Row8_1List = row8_1List.Count - 1;

		//row8の最後、row8_1の最後、row8_1の最後から二番目
		if (flipCardsAmount == 1 && flipamountIs1 ==false) {
			for (int i = 0; i < 3; i++) {
				if (i == 0 || i == 1) {
					GameObject deckCard = Instantiate (prefabCard) as GameObject;
					AddInformationToCard (deckCard, row8_1List [lastIndex_Row8_1List - 1 + i], 110 + i);
					list.Add (deckCard);
				}
				if(i == 2){
					GameObject deckCard = Instantiate (prefabCard) as GameObject;
					AddInformationToCard (deckCard, row8_List [0], 110 + i);
					list.Add (deckCard);
				}
			}
		}
		//row8_1の最後、row8の最後、row8の最後から二番目
		if (flipCardsAmount == 2 && flipamountIs1 == false) {
			for (int i = 0; i < 3; i++) {
				if (i == 0) {
					GameObject deckCard = Instantiate (prefabCard) as GameObject;
					AddInformationToCard (deckCard, row8_1List [lastIndex_Row8_1List], 110);
					list.Add (deckCard);
				}
				if(i == 1 || i == 2){
					GameObject deckCard = Instantiate (prefabCard) as GameObject;
					AddInformationToCard (deckCard, row8_List [i - 1], 110 + i);
					list.Add (deckCard);
				}
			}
		}
		//row8の最後、row8の最後から二番目、row8の最後から三番目
		if (flipCardsAmount == 3 && flipamountIs1 == false) {
			for (int i = 0; i < 3; i++) {
				GameObject deckCard = Instantiate (prefabCard) as GameObject;
				AddInformationToCard (deckCard, row8_List [i], 110 + i);
				list.Add (deckCard);
			}
		}
		//flipamountIs1がtrueの場合
		if (flipCardsAmount == 1 && flipamountIs1 == true) {
			GameObject deckCard = Instantiate (prefabCard) as GameObject;
			AddInformationToCard (deckCard, row8_List [0], 110);
			list.Add (deckCard);
		}

	}


	/// <summary>
	/// hintカードがrow8_1からrow8へ移動する場合で、 row8_1の上三枚のカードをlistへ加える
	/// </summary>
	void AddRow8_1CardsToList(List<GameObject> list)
	{
		List<GameObject> row8_1List = cash.gameDirector.openDeck_List;
		int lastIndex_Row8_1List = row8_1List.Count - 1;

		if(row8_1List.Count >= 3){
	    	for (int i = 0; i < 3; i++) {
		    	GameObject child_Prefab = Instantiate (prefabCard) as GameObject;
				AddInformationToCard (child_Prefab, row8_1List [lastIndex_Row8_1List - i], 110 - i);
		    	list.Add (child_Prefab);
			}
		}
		if(row8_1List.Count <= 2){
			for (int i = 0; i < row8_1List.Count; i++) {
				GameObject child_Prefab = Instantiate (prefabCard) as GameObject;
				AddInformationToCard (child_Prefab, row8_1List [lastIndex_Row8_1List - i], 110 - i);
				list.Add (child_Prefab);
			}
		}
	}


	/// <summary>
	/// prefabにoriginalの情報を持たせる
	/// </summary>
	void AddInformationToCard(GameObject prefab, GameObject original, int sortingOrder)
	{
		prefab.GetComponent<BoxCollider2D> ().enabled = false;
		prefab.transform.position = original.transform.position;
		var color = prefab.GetComponent<SpriteRenderer> ().color;
		color.a = 255f;
		prefab.GetComponent<SpriteRenderer> ().color = color;
		prefab.GetComponent<SpriteRenderer> ().sprite = original.GetComponent<SpriteRenderer> ().sprite;
		prefab.GetComponent<Card> ().suit = original.GetComponent<Card> ().suit;
		prefab.GetComponent<Card> ().numOfCard = original.GetComponent<Card> ().numOfCard;
		prefab.transform.localScale = new Vector3 (cash.placePos.scaleCard.x, cash.placePos.scaleCard.y, 0);
		prefab.GetComponent<Card> ().height = cash.placePos.cardHeight;
		prefab.GetComponent<SpriteRenderer> ().sortingOrder = sortingOrder;
	}


	/// <summary>
	/// 渡したListのカードを全て透明にする
	/// </summary>
	void TransparentListCards(List<GameObject> list)
	{
		for(int i = 0; i < list.Count; i++){
			var color = list[i].GetComponent<SpriteRenderer>().color;
			color.a = 0;
			list[i].GetComponent<SpriteRenderer>().color = color;
		}

	}


	/// <summary>
	/// 透明になった渡したListのカードを全て元に戻す
	/// </summary>
	void TransparentListCards(List<GameObject> list, float originalColor)
	{
		for(int i = 0; i < list.Count; i++){
			var color = list[i].GetComponent<SpriteRenderer>().color;
			color.a = originalColor;
			list[i].GetComponent<SpriteRenderer>().color = color;
		}

	}


		
	
}
