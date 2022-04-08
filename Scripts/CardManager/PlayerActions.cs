using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Comon;

public class PlayerActions : MonoBehaviour {

	CardsDirector cardsDirector_C;
	Rigidbody2D rb2D;
    Cash cash;

	GameObject upperObj;

	private List<GameObject> childList = new List<GameObject> ();
	private GameObject obj_Empty;
	private GameObject oya;

	int indexNum_ThisObj;
	int lastIndexNum_ThisObj;
    float movingSpeed;
    float rollingSpeed;
    float timeToFlipNextRetuCard;

	private bool accepted;
	private bool oya_Exist = false;
	private bool isTap;
	private Vector3 oridinalPos = Vector3.zero;
	private Vector3 endPos = Vector3.zero;
	private List<GameObject> list_Oya = new List<GameObject> ();
	private List<GameObject> list_ThisCard = new List<GameObject> ();

	float toleranceDistance; 
	bool isDrag = false;

	void Start () 
	{
        movingSpeed = 0.3f;
        rollingSpeed = 0.1f;
        timeToFlipNextRetuCard = 0.1f;
		cardsDirector_C = gameObject.GetComponent<CardsDirector> ();
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        toleranceDistance = cash.placePos.cardWidth / 12f;

	}



	void OnMouseDown()
	{
        if (Input.touchCount > 1)
            return;

        isTap = true; 

		oridinalPos = Input.mousePosition;

        if (cash.hint.hintCardsList.Count > 0)
        {
            cash.hint.TransparentToDestroyObjOfList(cash.hint.hintCardsList);
        }
	}


	void OnMouseDrag()
	{
        if (Input.touchCount > 1)
            return;

        if (isDrag == false) {
			Vector3 movingPos = Input.mousePosition;
			
			float movedDistance = Vector3.Distance(oridinalPos, movingPos);
			if (movedDistance >= toleranceDistance) 
				isDrag = true;
		}

		if (isDrag == true) {
			isTap = false;
			GetComponent<SpriteRenderer> ().sortingOrder = 90;

			if (gameObject.tag != "deck") {
				int indexNum_ThisObj = cash.gameDirector.GetIndexNum (gameObject);
				int lastIndexNum_ThisObj = cash.gameDirector.GetLastIndexNum (gameObject);
				list_ThisCard = cash.gameDirector.GetListOfObj (gameObject);


				if (indexNum_ThisObj < lastIndexNum_ThisObj && childList.Count == 0) {
					for (int i = 0; i < lastIndexNum_ThisObj - indexNum_ThisObj; i++) {
						childList.Add (list_ThisCard [indexNum_ThisObj + 1 + i]);
					}
				}
				if (childList.Count != 0) {
					for (int i = 0; i < childList.Count; i++) {
						childList [i].GetComponent<SpriteRenderer> ().sortingOrder = 91 + i;
						childList [i].transform.parent = gameObject.transform;
					}
				}
			}
			Vector3 objectPointInScreen	= Camera.main.WorldToScreenPoint (this.rb2D.transform.position);
			Vector3 mousePointInScreen= new Vector3 (Input.mousePosition.x, Input.mousePosition.y, objectPointInScreen.z);
			Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint (mousePointInScreen);
			mousePointInWorld.z = this.rb2D.transform.position.z;
			this.rb2D.transform.position = mousePointInWorld;
		}
	}





	void OnTriggerEnter2D(Collider2D collider2D)
	{
        int oya_IndexNum = cash.gameDirector.GetIndexNum (collider2D.gameObject);
		int oya_LastIndexNum = cash.gameDirector.GetLastIndexNum(collider2D.gameObject);

		int indexNum = -1;
		int lastIndexNum = -1;
		if (gameObject.tag != "deck") {
			indexNum = cash.gameDirector.GetIndexNum (gameObject);
			lastIndexNum = cash.gameDirector.GetLastIndexNum (gameObject);
		}

		if (isTap == false) {

			if (collider2D.gameObject.tag == "retu" && oya_IndexNum == oya_LastIndexNum) {
				accepted = cash.gameDirector.AcceptedToBeChild_1 (gameObject, collider2D.gameObject);
				if (accepted == true) {
					oya = collider2D.gameObject;
					oya_Exist = true;
					accepted = false;
				}
			}

			if (collider2D.gameObject.tag == "retu_empty") {
				accepted = cash.gameDirector.AcceptedToBeChild_3 (gameObject, collider2D.gameObject);
				if (accepted == true) {
					oya = collider2D.gameObject;
					oya_Exist = true;
					accepted = false;
				}
			}

			if (collider2D.gameObject.tag == "yama" && indexNum == lastIndexNum) {
				accepted = cash.gameDirector.AcceptedToBeChild_2 (gameObject, collider2D.gameObject);
				if (accepted == true) {
					oya = collider2D.gameObject;
					oya_Exist = true;
					accepted = false;
				}
			}

			if (collider2D.gameObject.tag == "yama_empty") {
				accepted = cash.gameDirector.AcceptedToBeChild_3 (gameObject, collider2D.gameObject);
				if (accepted == true) {
					oya = collider2D.gameObject;
					oya_Exist = true;
					accepted = false;
				}
			}
		} 
	}


	bool ProcessForTapOrDrag(bool tap)
	{
		bool processCardToMove = false;

		//タップ後に移動する場所がある場合
		if (tap == true) {
			oya_Exist = cash.gameDirector.CheckExistOya (gameObject); 
			if (oya_Exist == true) {
				oya = cash.gameDirector.GetOya ();
                cash.sound.GetTapCardSound();
				processCardToMove = true;
			}
			//タップ後に移動する場所がない場合
			else if(oya_Exist == false)
            {
				//タップしたが動かなかったカードに対して何か処理を書くならここに書く
			}

		}
		if (tap == false) {
			if (childList.Count != 0) { 
				for (int i = 0; i < childList.Count; i++) {
					childList [i].transform.parent = null;
				}
				childList.Clear ();
			}
			if (oya_Exist == true) {
				if (GetComponent<BoxCollider2D> ().IsTouching (oya.GetComponent<BoxCollider2D> ())) {
                    cash.sound.GetTapCardSound();
					processCardToMove = true;
				}
			}
		}
		return processCardToMove;
	}



	IEnumerator  OnMouseUp()
	{
		//移動できる場所があればtrue、なければfalseになる
		bool processCardToMove = ProcessForTapOrDrag (isTap);
		
        if (processCardToMove == true) {
			
		if (gameObject.tag != "deck")
			obj_Empty = cash.gameDirector.GetEmptyObj (gameObject);
		if (gameObject.tag == "deck")
			obj_Empty = cash.row8_1Obj;

		if (oya.transform.tag != "yama_empty" && oya.transform.tag != "retu_empty")
			list_Oya = cash.gameDirector.GetListOfObj (oya);
		if (oya.transform.tag == "yama_empty" || oya.transform.tag == "retu_empty")
			list_Oya = cash.gameDirector.GetListOfObj_1 (oya.transform.name);
		if (gameObject.tag != "deck")
			list_ThisCard = cash.gameDirector.GetListOfObj (gameObject);
		if (gameObject.tag == "deck")
			list_ThisCard = cash.gameDirector.openDeck_List;

		indexNum_ThisObj = -1;
		lastIndexNum_ThisObj = -1;
		if (gameObject.tag != "deck") {
			indexNum_ThisObj = cash.gameDirector.GetIndexNum (gameObject);
			lastIndexNum_ThisObj = cash.gameDirector.GetLastIndexNum (gameObject); 
		}

		upperObj = null;
		if (indexNum_ThisObj != 0 && gameObject.transform.tag != "deck")
			upperObj = list_ThisCard [indexNum_ThisObj - 1];

		GetComponent<SpriteRenderer> ().sortingOrder = 90;
		endPos = oya.transform.position;
		cash.gameDirector.movingCard.Add (gameObject);
		if (oya.transform.tag == "retu") {
			endPos.y -= cash.placePos.intervalFrontCards;
			oya.GetComponent<CardsDirector> ().ChangeSizeBC2D ();
		}
		if (oya.transform.tag == "yama")
			oya.GetComponent<BoxCollider2D> ().enabled = false;
		if (oya.transform.tag == "yama_empty" || oya.transform.tag == "retu_empty")
			cash.gameDirector.BeFalseBoxcolli (oya.transform.name);

	    	cash.scoreText_Playing.AddOneMoveText (); 
	    	isDrag = false;


        if (oya.transform.tag == "yama_empty") {
			list_Oya.Add (gameObject); 
			list_ThisCard.Remove (gameObject);
			cardsDirector_C.SetInfoToAnimation (true, endPos, movingSpeed, true);

			
			if (gameObject.tag == "deck")
            {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isToYama);
                cash.scoreText_Playing.ChangeScoreText(cash.scoreText_Playing.isFromDeck);
				AddInfoToListOfUndo (false, obj_Empty);
				yield return new WaitForSeconds (0.2f);
				if (list_ThisCard.Count != 0)
                        cash.touchingDeck.FixOpenDeck ();
				GameObject nextContent = GameObject.Find("nextContent");		
				GameObject currentContent = GameObject.Find("currentContent");
            }

			if (gameObject.tag == "yama") {
				obj_Empty.GetComponent<BoxCollider2D> ().enabled = true;
				AddInfoToListOfUndo (false, obj_Empty);
			}

			if (gameObject.tag == "retu" && upperObj == null) {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isToYama);
				isRetuAndUpperIsNull ("yama");
				yield return new WaitForSeconds (timeToFlipNextRetuCard);
                    yield break;
			}

				if (gameObject.tag == "retu" && upperObj.GetComponent<Card> ().isTurned == false) {
					cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isToYama);
					cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isFlipedRetu);
					yield return new WaitForSeconds (timeToFlipNextRetuCard);
					isRetuAndUpperIsBack ();
				} else if (gameObject.tag == "retu" && upperObj.GetComponent<Card> ().isTurned == true) {
					cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isToYama);
					upperObj.GetComponent<CardsDirector> ().ResetSizeBC2D ();
				}

			gameObject.tag = "yama";
                yield break;
		}

		if (oya.transform.tag == "retu_empty") {
			list_Oya.Add (gameObject);

			if (indexNum_ThisObj < lastIndexNum_ThisObj && gameObject.tag != "deck") {
				AddChild ();
				list_ThisCard.Remove (gameObject);
				cardsDirector_C.SetInfoToAnimation (true, endPos, movingSpeed, true);
				DoForChild ();
			} else {
				list_ThisCard.Remove (gameObject);
				cardsDirector_C.SetInfoToAnimation (true, endPos, movingSpeed, true);
			}


			if (gameObject.tag == "deck") {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isFromDeck);
				
				AddInfoToListOfUndo (false, obj_Empty);
				yield return new WaitForSeconds (0.2f);
				if (list_ThisCard.Count != 0)
                        cash.touchingDeck.FixOpenDeck ();
				GameObject nextContent = GameObject.Find("nextContent");		
				GameObject currentContent = GameObject.Find("currentContent");
                }

			if (gameObject.tag == "yama") {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isBackFromYama);
				AddInfoToListOfUndo (false, upperObj);
				upperObj.GetComponent<BoxCollider2D> ().enabled = true;
			}

			if (gameObject.tag == "retu" && upperObj == null) {
				isRetuAndUpperIsNull ("retu");
				yield return new WaitForSeconds (timeToFlipNextRetuCard);
                   
                    yield break;
			}

			if (gameObject.tag == "retu" && upperObj != null) {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isFlipedRetu);
				yield return new WaitForSeconds (timeToFlipNextRetuCard);
				isRetuAndUpperIsBack ();
			}
				
			gameObject.tag = "retu";
	    		yield return new WaitForSeconds (timeToFlipNextRetuCard);
               
                yield break;
		}

		if (oya.transform.tag == "yama") {
			list_Oya.Add (gameObject);
			list_ThisCard.Remove (gameObject);
			cardsDirector_C.SetInfoToAnimation (true, endPos, movingSpeed, true);
			cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isToYama);

			if (gameObject.tag == "deck") {
                cash.scoreText_Playing.ChangeScoreText(cash.scoreText_Playing.isFromDeck);
                
				AddInfoToListOfUndo (false, obj_Empty);
				yield return new WaitForSeconds (0.2f);
				if (list_ThisCard.Count != 0)
					cash.touchingDeck.FixOpenDeck ();
					GameObject nextContent = GameObject.Find("nextContent");		
					GameObject currentContent = GameObject.Find("currentContent");
                }

			if (gameObject.tag == "retu" && upperObj == null) {
				isRetuAndUpperIsNull (oya.transform.tag);
				yield return new WaitForSeconds (timeToFlipNextRetuCard);
                 
                    yield break;
			}

			if (gameObject.tag == "retu" && upperObj.GetComponent<Card> ().isTurned == true) {
				AddInfoToListOfUndo (false, upperObj);
				upperObj.GetComponent<CardsDirector> ().ResetSizeBC2D ();
			}

			if (gameObject.tag == "retu" && upperObj.GetComponent<Card> ().isTurned == false) {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isFlipedRetu);
				yield return new WaitForSeconds (timeToFlipNextRetuCard);
				isRetuAndUpperIsBack ();
			}
				
			gameObject.tag = "yama";
			yield return new WaitForSeconds (timeToFlipNextRetuCard);
               
                yield break;
		}

		if (oya.transform.tag == "retu") {
			list_Oya.Add (gameObject);

			if (indexNum_ThisObj < lastIndexNum_ThisObj && gameObject.tag != "deck") {
				AddChild ();
				list_ThisCard.Remove (gameObject);
				cardsDirector_C.SetInfoToAnimation (true, endPos, movingSpeed, true);
				DoForChild ();
			} else {
				list_ThisCard.Remove (gameObject);
				cardsDirector_C.SetInfoToAnimation (true, endPos, movingSpeed, true);
			}

			if (gameObject.tag == "deck") {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isFromDeck);
				AddInfoToListOfUndo (false, obj_Empty);
				yield return new WaitForSeconds (0.2f);

				if (list_ThisCard.Count != 0)
					cash.touchingDeck.FixOpenDeck ();
					GameObject nextContent = GameObject.Find("nextContent");		
					GameObject currentContent = GameObject.Find("currentContent");
                }

			if (gameObject.tag == "yama" && upperObj == null) { 
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isBackFromYama);
				AddInfoToListOfUndo (false, obj_Empty);
				obj_Empty.GetComponent<BoxCollider2D> ().enabled = true;
				gameObject.tag = "retu";
				yield return new WaitForSeconds (timeToFlipNextRetuCard);
                   
                    yield break;
			}

			if (gameObject.tag == "retu" && upperObj == null) {
				isRetuAndUpperIsNull (oya.transform.tag);
				yield return new WaitForSeconds (timeToFlipNextRetuCard);
                   
                    yield break;
			}

			if (gameObject.tag == "yama" && upperObj != null) {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isBackFromYama);
				AddInfoToListOfUndo (false, upperObj);
				upperObj.GetComponent<BoxCollider2D> ().enabled = true;
			}

			if (gameObject.tag == "retu" && upperObj.GetComponent<Card> ().isTurned == true) {
				AddInfoToListOfUndo (false, upperObj);
				upperObj.GetComponent<CardsDirector> ().ResetSizeBC2D ();
			}

			if (gameObject.tag == "retu" && upperObj.GetComponent<Card> ().isTurned == false) {
				cash.scoreText_Playing.ChangeScoreText (cash.scoreText_Playing.isFlipedRetu);
				yield return new WaitForSeconds (timeToFlipNextRetuCard);
				isRetuAndUpperIsBack ();
			}

			gameObject.tag = "retu";
			yield return new WaitForSeconds (timeToFlipNextRetuCard);
       
                yield break;
		}
	
		} else if (isTap == false) {
            cash.sound.GetMovingCardSound();

			isDrag = false;
			int indexNum_ThisObj = -1;
			int lastIndexNum_ThisObj = -1;
			if (gameObject.tag != "deck") {
				indexNum_ThisObj = cash.gameDirector.GetIndexNum (gameObject);
				lastIndexNum_ThisObj = cash.gameDirector.GetLastIndexNum (gameObject); 
			}
			float interval_Acard = cash.placePos.intervalFrontCards;
			obj_Empty = cash.gameDirector.GetEmptyObj (gameObject);
			Vector3 oridinalPos = cash.gameDirector.GetOridinalPos(list_ThisCard, indexNum_ThisObj, obj_Empty);
			
			GetComponent<SpriteRenderer> ().sortingOrder = 90;
			cardsDirector_C.SetInfoToAnimation (true, oridinalPos, movingSpeed, true);

			if (indexNum_ThisObj < lastIndexNum_ThisObj && gameObject.tag != "deck") {
				for(int i = 0; i < lastIndexNum_ThisObj - indexNum_ThisObj; i++){
					childList.Add(list_ThisCard[indexNum_ThisObj + 1 + i]);
				}
				for (int i = 0; i < childList.Count; i++) {
					childList [i].GetComponent<SpriteRenderer> ().sortingOrder = 91 + i;
					oridinalPos.y -= interval_Acard;
					childList [i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, oridinalPos, movingSpeed, true);
				}
				childList.Clear ();
			}
			yield return new WaitForSeconds (timeToFlipNextRetuCard);
     
        }
	}




	void AddChild()
	{
		for (int i = 0; i < lastIndexNum_ThisObj - indexNum_ThisObj; i++) {
			childList.Add (list_ThisCard [indexNum_ThisObj + 1 + i]);
		}
	}


	void DoForChild()
	{
		for (int i = 0; i < childList.Count; i++) {
			childList [i].GetComponent<SpriteRenderer> ().sortingOrder = 91 + i;
			endPos.y -= cash.placePos.intervalFrontCards;
			list_Oya.Add (childList [i]); 
			list_ThisCard.Remove (childList [i]);
			childList [i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed, true);
		}
		childList.Clear ();
	}


	void isRetuAndUpperIsBack()
	{
		GameObject lastCardOfOyaList = GameDirector.GetLastCardOfList (list_ThisCard);
        lastCardOfOyaList.GetComponent<CardsDirector>().isFlipedAtRetu = true;
        lastCardOfOyaList.GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
		AddInfoToListOfUndo (true, upperObj);
        cash.sound.GetFlipCardSound();
	}


	void isRetuAndUpperIsNull(string tag)
	{
		AddInfoToListOfUndo (false, obj_Empty);
		obj_Empty.GetComponent<BoxCollider2D> ().enabled = true;
		cash.gameDirector.ShowAutomaticCompleteObj (); 
		gameObject.tag = tag;
	}


	void AddInfoToListOfUndo(bool willFlip, GameObject obj)
	{
		cash.gameDirector.willFlip.Add(willFlip);
		cash.gameDirector.exOyaCard.Add (obj);
	}

}
















