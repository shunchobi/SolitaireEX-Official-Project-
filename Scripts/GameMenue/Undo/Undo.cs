using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Comon;

public class Undo : MonoBehaviour {
    

    GameObject hint;
	
	List<GameObject> nowList = new List<GameObject>(); //戻るカードの、戻る前に所属していたリスト
	List<GameObject> willList = new List<GameObject>(); //戻った後に所属することになるリスト
	List<GameObject> childList = new List<GameObject>();
	private List<GameObject> list_ThisCard = new List<GameObject>();

	float timeToWait = 0.14f;


    Cash cash;

    float movingSpeed;
    float rollingSpeed;


	void Start()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        movingSpeed = 0.12f;
        rollingSpeed = 0.1f;
	}



    private void Update()
    {
        if (cash.gameDirector.movingCard.Count == 0 && isShownUi == true)
            ShowUpUndo(false);
        else if(cash.gameDirector.movingCard.Count > 0 && isShownUi == false)
            ShowUpUndo(true);

    }

    bool isShownUi = true;
    void ShowUpUndo(bool isShow)
    {
        Color color = gameObject.GetComponent<Image>().color;
        if(isShow == true)
        {
            color.a = 1;
            gameObject.GetComponent<Button>().enabled = true;
            isShownUi = true;
            gameObject.GetComponent<Image>().color = color;
            return;
        }
        if (isShow == false)
        {
            color.a = 0.6f/1f;
            gameObject.GetComponent<Button>().enabled = false;
            isShownUi = false;
            gameObject.GetComponent<Image>().color = color;

        }
    }



    public void OnMouseUp()
	{
        cash.sound.GetButtonSound();
        DoUndo();
	}


	public void DoUndo ()
	{
		if (cash.gameDirector.movingCard.Count > 0) { //移動するカードが存在する場合
            if (cash.hint.hintCardsList.Count > 0)
                cash.hint.TransparentToDestroyObjOfList(cash.hint.hintCardsList);
            
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//Undoのための材料
			int willLast = cash.gameDirector.willFlip.Count - 1;
			int exOyaLast = cash.gameDirector.exOyaCard.Count - 1;
			int movingLast = cash.gameDirector.movingCard.Count - 1;
			bool oyaFlip = cash.gameDirector.willFlip [willLast];

			GameObject exOya = cash.gameDirector.exOyaCard [exOyaLast];
			int exOyaIndex = cash.gameDirector.GetIndexNum (exOya);

			Vector3 endPos = new Vector3 (exOya.transform.position.x, exOya.transform.position.y, exOya.transform.position.z);
			int indexNum_Target = -1;
			int lastIndexNum_Target = -1;
			GameObject upperObj = null;

			if (exOya.transform.name == "row8_1") {
				nowList = cash.gameDirector.row8_List;
			} else if (exOya.transform.name == "row8") {
				nowList = cash.gameDirector.openDeck_List;
			} else if (exOya.transform.name != "row8_1" && exOya.transform.name != "row8") {
				nowList = cash.gameDirector.GetListOfObj (cash.gameDirector.movingCard [movingLast]);
			}
			if (exOya.transform.tag == "yama" || exOya.transform.tag == "retu") {
				willList = cash.gameDirector.GetListOfObj (exOya);
			} else if (exOya.transform.name == "row8_1") {
				willList = cash.gameDirector.openDeck_List;
			} else if (exOya.transform.name == "row8") {
				willList = cash.gameDirector.row8_List;
			} else if (exOya.transform.tag == "yama_empty" || exOya.transform.tag == "retu_empty") {
				willList = cash.gameDirector.GetListOfObj_1 (exOya.transform.name);
			}

			cash.scoreText_Playing.MinusOneMoveText ();
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			 
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (exOya.transform.tag == "retu") {
				GameObject target = cash.gameDirector.movingCard[movingLast]; //戻るカードを示す
				indexNum_Target = cash.gameDirector.GetIndexNum (target); //このカードが所属するリストでのIndexNum
				lastIndexNum_Target = cash.gameDirector.GetLastIndexNum (target); //このカードが所属するリストの最後の要素のIndexNum
				if(indexNum_Target != 0) upperObj = nowList[indexNum_Target - 1]; //戻るカードの上のカード
				bool isTurned = true;
				GameObject upperOfExOya = null;
				if (exOyaIndex != 0) {
					upperOfExOya = willList [exOyaIndex - 1];
					isTurned = upperOfExOya.GetComponent<Card> ().isTurned;
				}
                
                //ordeInLayer Add Removeの位置						
                if (target.transform.tag == "retu") {

					if(target.GetComponent<Card>().numOfCard == 1 && target.transform.tag == "yama"){
						cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
					}
					if (indexNum_Target == 0 && target.transform.tag == "retu") {
						cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
					} else{
						nowList [indexNum_Target - 1].GetComponent<CardsDirector> ().ResetSizeBC2D ();
					}
					//targetが戻った後にexOyaが裏返る必要がない場合
					if (oyaFlip == false) {
						endPos.y -= cash.placePos.intervalFrontCards; //Playerクラスで使うカードのズレの数字と同じ
						exOya.GetComponent<CardsDirector> ().ChangeSizeBC2D ();
					}
					//targetが戻った後にexOyaが裏返る必要がある場合
					if (oyaFlip == true) {
						if (exOyaIndex == 0 || isTurned == false) {
							cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isFlipedRetu);
							endPos.y -= cash.placePos.intervalBackCards; //DealCardクラスで使うカードのズレの数字と同じ
							exOya.GetComponent<CardsDirector> ().FlipToBack (true, rollingSpeed);
							exOya.GetComponent<BoxCollider2D> ().enabled = false;
						}
					}
				}

				if (target.transform.tag == "yama") {
                    cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isToYama);
					if (upperObj == null) {
						cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
					} else{
						upperObj.GetComponent<BoxCollider2D> ().enabled = true;
					}
					//targetが戻った後にexOyaが裏返る必要がない場合
					if (oyaFlip == false) {
						endPos.y -= cash.placePos.intervalFrontCards; //Playerクラスで使うカードのズレの数字と同じ
						exOya.GetComponent<CardsDirector> ().ChangeSizeBC2D ();
					}
					//targetが戻った後にexOyaが裏返る必要がある場合
					if (oyaFlip == true) {
						if (exOyaIndex == 0 || isTurned == false) {
							cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isFlipedRetu);
							endPos.y -= cash.placePos.intervalBackCards; //DealCardクラスで使うカードのズレの数字と同じ
							exOya.GetComponent<CardsDirector> ().FlipToBack (true, rollingSpeed);
							exOya.GetComponent<BoxCollider2D> ().enabled = false;
						}
					}
				}

				//targetに子供がいる場合
				if (indexNum_Target < lastIndexNum_Target)
					AddChildsToList (target, indexNum_Target, lastIndexNum_Target);

				if (childList.Count == 0) {
					willList.Add (target);
					nowList.Remove (target);
                    target.GetComponent<SpriteRenderer>().sortingOrder = 90;
                    target.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed, true);
				}
				if (childList.Count != 0) {
					willList.Add (target);
					nowList.Remove (target);
                    target.GetComponent<SpriteRenderer>().sortingOrder = 90;
                    target.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed, true);
                    DoForChild(endPos);
				}


				target.transform.tag = "retu";
				cash.gameDirector.movingCard.RemoveAt (movingLast);
				cash.gameDirector.willFlip.RemoveAt (willLast);
				cash.gameDirector.exOyaCard.RemoveAt (exOyaLast);
			}
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (exOya.transform.tag == "yama") {
				GameObject target = cash.gameDirector.movingCard [movingLast];
				indexNum_Target = cash.gameDirector.GetIndexNum (target); //このカードが所属するリストでのIndexNum
				lastIndexNum_Target = cash.gameDirector.GetLastIndexNum (target); //このカードが所属するリストの最後の要素のIndexNum
				if(indexNum_Target != 0) upperObj = nowList[indexNum_Target - 1];
				willList.Add (target);
				nowList.Remove (target);

				target.GetComponent<SpriteRenderer> ().sortingOrder = 90;
                target.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed, true);
				exOya.GetComponent<BoxCollider2D> ().enabled = false;
				cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isBackFromYama);

				if (target.transform.tag == "retu" && upperObj == null) {
					cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
				}

				if (target.transform.tag == "retu" && upperObj != null) {
					upperObj.GetComponent<CardsDirector>().ResetSizeBC2D ();
				}

				target.transform.tag = "yama";
				cash.gameDirector.willFlip.RemoveAt (willLast);
				cash.gameDirector.movingCard.RemoveAt (movingLast);
				cash.gameDirector.exOyaCard.RemoveAt (exOyaLast);
			}
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (exOya.transform.tag == "retu_empty") {
				GameObject target = cash.gameDirector.movingCard [movingLast];
				indexNum_Target = cash.gameDirector.GetIndexNum (target); //このカードが所属するリストでのIndexNum
				lastIndexNum_Target = cash.gameDirector.GetLastIndexNum (target); //このカードが所属するリストの最後の要素のIndexNum
				if(indexNum_Target != 0) upperObj = nowList[indexNum_Target - 1];
				target.GetComponent<SpriteRenderer> ().sortingOrder = 90;
				exOya.GetComponent<BoxCollider2D> ().enabled = false;

					
				if (target.transform.tag == "yama" && upperObj == null) {
					cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isToYama);
					cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
				}

				if (target.transform.tag == "yama" && upperObj != null) {
					cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isToYama);
					upperObj.GetComponent<BoxCollider2D> ().enabled = true;
				}

				if (target.transform.tag == "retu" && upperObj == null) {
					cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
				}

				if (target.transform.tag == "retu" && upperObj != null) {
					upperObj.GetComponent<CardsDirector>().ResetSizeBC2D ();
				}

				//targetに子供がいる場合
				if (indexNum_Target < lastIndexNum_Target)
					AddChildsToList (target, indexNum_Target, lastIndexNum_Target);

				if (childList.Count == 0) {
					willList.Add (target);
					nowList.Remove (target);
                    target.GetComponent<SpriteRenderer>().sortingOrder = 90;
                    target.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed, true);
				}
				if (childList.Count != 0) {
					willList.Add (target);
					nowList.Remove (target);
                    target.GetComponent<SpriteRenderer>().sortingOrder = 90;
                    target.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed, true);
					DoForChild (endPos);
				}
					
				target.transform.tag = "retu";
				cash.gameDirector.movingCard.RemoveAt (movingLast);
				cash.gameDirector.willFlip.RemoveAt (willLast);
				cash.gameDirector.exOyaCard.RemoveAt (exOyaLast);
			}
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (exOya.transform.tag == "yama_empty") {
				GameObject target = cash.gameDirector.movingCard [movingLast];
				indexNum_Target = cash.gameDirector.GetIndexNum (target); //このカードが所属するリストでのIndexNum
				lastIndexNum_Target = cash.gameDirector.GetLastIndexNum (target); //このカードが所属するリストの最後の要素のIndexNum
				if(indexNum_Target != 0) upperObj = nowList[indexNum_Target - 1];
				willList.Add (target);
				nowList.Remove (target);

				target.GetComponent<SpriteRenderer> ().sortingOrder = 90;
                target.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed, true);
				exOya.GetComponent<BoxCollider2D> ().enabled = false;

				if (target.transform.tag == "yama") {
					cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
				}

				if (target.transform.tag == "retu" && upperObj == null) {
					cash.scoreText_Playing.ChangeScoreText(cash.scoreText_Playing.isToYama);
					cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
				}

				if (target.transform.tag == "retu" && upperObj != null) {
					cash.scoreText_Playing.ChangeScoreText(cash.scoreText_Playing.isToYama);
					upperObj.GetComponent<CardsDirector> ().ResetSizeBC2D ();
				}

				target.transform.parent = null;
				target.transform.tag = "yama";
				cash.gameDirector.movingCard.RemoveAt (movingLast);
				cash.gameDirector.willFlip.RemoveAt (willLast);
				cash.gameDirector.exOyaCard.RemoveAt (exOyaLast);
			}
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (exOya.transform.name == "row8") { 
				int lastIndexNum = nowList.Count - 1;
				int amount_NowList = nowList.Count;
				int amount_WillList = willList.Count;
				int amount_BackToRow8Last = cash.gameDirector.amount_BackToRow8.Count - 1;
				int flipAmount = cash.gameDirector.amount_BackToRow8 [amount_BackToRow8Last];
				Vector3 endPos_Row8 = cash.row8_Obj.transform.position;

				nowList [lastIndexNum].GetComponent<BoxCollider2D> ().enabled = false;


				FlipRow8_1ToRow8 (flipAmount, lastIndexNum, endPos_Row8);
				//yield return new WaitForSeconds (movingSpeed+0.02f);
				cash.touchingDeck.FixOpenDeck ();

                if (willList.Count > 0)
                {
                    for (int i = 0; i < willList.Count; i++)
                    {
                        willList[i].GetComponent<BoxCollider2D>().enabled = false;
                    }
                }


				cash.gameDirector.movingCard.RemoveAt (movingLast);
				cash.gameDirector.willFlip.RemoveAt (willLast);
				cash.gameDirector.exOyaCard.RemoveAt (exOyaLast);
				cash.gameDirector.amount_BackToRow8.RemoveAt (amount_BackToRow8Last);
			}
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (exOya.transform.name == "row8_1") {
				GameObject target = cash.gameDirector.movingCard [movingLast];

				//////////////////////////row8からrow8_1へ戻される時の処理//////////////////////////
				if (target.transform.tag == "deck") {
					for(int i = 0; i < nowList.Count; i++){
						GameObject target_deck = nowList[i];
						willList.Add (target_deck);
					}
					nowList.Clear ();

                    cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isUsedAllYama);

                    if (willList.Count == 1) {
						Vector3 endPos_1 = cash.touchingDeck.position [0];
						GameObject firstCard_Row8_1 = willList [0];
                        firstCard_Row8_1.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos_1, movingSpeed, true);
						firstCard_Row8_1.GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
						firstCard_Row8_1.GetComponent<BoxCollider2D> ().enabled = true;
					}

					if (willList.Count == 2) {
						Vector3 endPos_1 = cash.touchingDeck.position [0];
						Vector3 endPos_2 = cash.touchingDeck.position [1];
						GameObject firstCard_Row8_1 = willList [0];
						GameObject secoundCard_Row8_1 = willList [1];
                        firstCard_Row8_1.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos_1, movingSpeed, true);
						firstCard_Row8_1.GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
                        secoundCard_Row8_1.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos_2, movingSpeed, false);
						secoundCard_Row8_1.GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
						secoundCard_Row8_1.GetComponent<BoxCollider2D> ().enabled = true;
					}

					if (willList.Count >= 3) {
						int lastIndexNum = willList.Count - 1;
						Vector3 endPos_1 = cash.touchingDeck.position [0];
						Vector3 endPos_2 = cash.touchingDeck.position [1];
						Vector3 endPos_3 = cash.touchingDeck.position [2];
						for (int i = 0; i < willList.Count; i++) {
                            willList[i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos_1, movingSpeed, true);
							willList[i].GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
							if (i == lastIndexNum - 1) {
                                willList [i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos_2, movingSpeed, false);
								willList[i].GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
							}
							if (i == lastIndexNum) {
                                willList [i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos_3, movingSpeed, false);
								willList[i].GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
								willList [i].GetComponent<BoxCollider2D> ().enabled = true;
							}
						}
					}

					cash.gameDirector.movingCard.RemoveAt (movingLast);
					cash.gameDirector.willFlip.RemoveAt (willLast);
					cash.gameDirector.exOyaCard.RemoveAt (exOyaLast);
                    return;
				}
				//////////////////////////row8からrow8_1へ戻される時の処理終了//////////////////////////

				//////////////////////////retuまたはyamaからrow8_1へ戻る時の処理//////////////////////////
				indexNum_Target = cash.gameDirector.GetIndexNum (target); //このカードが所属するリストでのIndexNum
				lastIndexNum_Target = cash.gameDirector.GetLastIndexNum (target); //このカードが所属するリストの最後の要素のIndexNum
				nowList = cash.gameDirector.GetListOfObj (target);
				if(indexNum_Target > 0) upperObj = nowList[indexNum_Target - 1];

				//retuかyamaからrow8_1に戻るときに、row8_1にある一番上のカードのBoxCollider2Dをfalseにする
				GameObject lastObj_list = null;
				if(willList.Count != 0) lastObj_list = willList[willList.Count - 1];
				if (lastObj_list != null) lastObj_list.GetComponent<BoxCollider2D> ().enabled = false;
				target.GetComponent<SpriteRenderer> ().sortingOrder = 90;
				Vector3 endpos_list = cash.touchingDeck.position [2];
				cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isFromDeck);

				if (target.transform.tag == "retu" && upperObj == null) {
					cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
				}

				if (target.transform.tag == "retu" && upperObj != null) {
					
					upperObj.GetComponent<CardsDirector> ().ResetSizeBC2D ();
				}
				if (target.transform.tag == "yama" && upperObj == null) {
					cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isToYama);
					cash.gameDirector.GetEmptyObj (target).GetComponent<BoxCollider2D> ().enabled = true;
				}

				if (target.transform.tag == "yama" && upperObj != null) {
					cash.scoreText_Playing.ChangeScoreText(-cash.scoreText_Playing.isToYama);
					upperObj.GetComponent<BoxCollider2D> ().enabled = true;
				}
				willList.Add (target);
				nowList.Remove (target);


				if(willList.Count == 1) endpos_list = cash.touchingDeck.position [0];
				if(willList.Count == 2) endpos_list = cash.touchingDeck.position [1];

				cash.touchingDeck.FixOpenDeck ();
				target.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endpos_list, movingSpeed, true);
				target.transform.tag = "deck";
				cash.gameDirector.movingCard.RemoveAt (movingLast);
				cash.gameDirector.willFlip.RemoveAt (willLast);
				cash.gameDirector.exOyaCard.RemoveAt (exOyaLast);
			}
            //////////////////////////retuまたはyamaからrow8_1へ戻る時の処理//////////////////////////

        } else {	
            return;
		}
	}









	void FlipRow8_1ToRow8(int amountToFlip, int lastIndexNum, Vector3 endPos_Row8)
	{
		for (int i = 0; i < amountToFlip; i++) {
			GameObject target = nowList [lastIndexNum - i];
			willList.Insert (0, target);
			nowList.Remove (target);
            target.GetComponent<SpriteRenderer>().sortingOrder = 90 + i;
			target.GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos_Row8, movingSpeed, false);
			target.GetComponent<CardsDirector> ().FlipToBack (true, rollingSpeed);
		}
	}



	void AddChildsToList(GameObject target, int indexNum_Target, int lastIndexNum_Target)
	{
		list_ThisCard = cash.gameDirector.GetListOfObj (target); //このカードが所属するListを取得
		//自身の子供をchildListへ追加
		for(int i = 0; i < lastIndexNum_Target - indexNum_Target; i++){
			childList.Add(list_ThisCard[indexNum_Target + 1 + i]);
		}
	}

	void DoForChild(Vector3 endPos)
	{
		//子供の処理
		for (int i = 0; i < childList.Count; i++) {
			childList [i].GetComponent<SpriteRenderer> ().sortingOrder = 91 + i;
			endPos.y -= cash.placePos.intervalFrontCards;
			childList [i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, endPos, movingSpeed, false);
			willList.Add (childList [i]); //修正する前は、list_Oya→cash.gameDirector.row7_List
			nowList.Remove (childList [i]);
		}
		childList.Clear ();
	}



	public void ResetUndo()
	{
        cash.gameDirector.willFlip.Clear();
        cash.gameDirector.amount_BackToRow8.Clear();
        cash.gameDirector.movingCard.Clear();
        cash.gameDirector.exOyaCard.Clear ();
	}



	/// <summary>
	/// 渡したオブジェクトの子、孫、ひ孫、、、を全てListへ加える
	/// </summary>
	public static List<GameObject>  GetAll (GameObject obj)
	{
		List<GameObject> allChildren = new List<GameObject> ();
		GetChildren (obj, ref allChildren);
		return allChildren;
	}
	//子要素を取得してリストに追加
	public static void GetChildren (GameObject obj, ref List<GameObject> allChildren)
	{
		Transform children = obj.GetComponentInChildren<Transform> ();
		//子要素がいなければ終了
		if (children.childCount == 0) {
			return;
		}
		foreach (Transform ob in children) {
			allChildren.Add (ob.gameObject);
			GetChildren (ob.gameObject, ref allChildren);
		}
	}

}


				


				
			




	






