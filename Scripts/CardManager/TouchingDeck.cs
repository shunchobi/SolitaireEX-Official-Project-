using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Comon;

public class TouchingDeck : MonoBehaviour {

    Cash cash;
	public List<Vector3> position = new List<Vector3>(); 

	GameObject lastObj;
    float timeToWait;
    float timeToWaitForButtonEnable;
	public bool flipAmountIs1 = true;

    float movingSpeed;
    float rollingSpeed;

	public bool rightHand = true;


	public void InitilaizeTouchingDeckAllMember()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        timeToWait = 0.12f;
        movingSpeed = 0.3f;
        rollingSpeed = 0.2f;
        timeToWaitForButtonEnable = 0.32f;
    }



    public void SetPosToList()
	{
        position.Clear ();
		position.Add (cash.row8_1Obj.transform.position);
		position.Add (cash.row8_2Obj.transform.position);
		position.Add (cash.row8_3Obj.transform.position);
	}



	IEnumerator OnMouseUpAsButton()
	{
        int list_Amount = cash.gameDirector.openDeck_List.Count;
        int flipAmount = cash.gameDirector.row8_List.Count;
        if(list_Amount == 0 && flipAmount == 0)
            yield break;

        if (cash.hint.hintCardsList.Count > 0)
        {
            //for (int i = 0; i < cash.hint.hintCardsList.Count; i++)
            //{
            //    cash.hint.hintCardsList[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
            //}
            cash.hint.TransparentToDestroyObjOfList(cash.hint.hintCardsList);
            //cash.hint.LetCardsBeTransparent(cash.hint.hintCardsList);
        }

        if (cash.gameDirector.row8_List.Count > 0)
        {
            if (cash.gameDirector.row8_List[0].GetComponent<CardsDirector>().isBlining == true)
            {
                for (int i = 0; i < cash.gameDirector.row8_List.Count; i++)
                {
                    GameObject aCard = cash.gameDirector.row8_List[i];
                    aCard.GetComponent<CardsDirector>().FinishBlink();
                }
            }
        }

        cash.row8_Obj.GetComponent<BoxCollider2D> ().enabled = false;

		if (flipAmountIs1 == false && flipAmount >= 3)
			flipAmount = 3;
		if (flipAmountIs1 == true && flipAmount != 0) 
			flipAmount = 1;

		if (flipAmount != 0)
			FixOpenDeck_1 (flipAmount);

		if (cash.gameDirector.openDeck_List.Contains (lastObj) == true)
			lastObj.GetComponent<BoxCollider2D> ().enabled = false;
		if (cash.gameDirector.openDeck_List.Count != 0) {
			if (cash.gameDirector.openDeck_List [list_Amount - 1].GetComponent<BoxCollider2D> ().enabled == true)
				cash.gameDirector.openDeck_List [list_Amount - 1].GetComponent<BoxCollider2D> ().enabled = false;
		}


		if (flipAmount == 0) {
            cash.sound.GetMovingCardSound();
			for (int i = 0; i < list_Amount; i++) {
				GameObject aCard_Row8_1 = cash.gameDirector.openDeck_List [i];
				cash.gameDirector.row8_List.Add (aCard_Row8_1);
				Vector3 pos = cash.row8_Obj.transform.position;
                aCard_Row8_1.GetComponent<CardsDirector> ().SetInfoToAnimation (true, pos, 0.25f, false);
				aCard_Row8_1.GetComponent<CardsDirector> ().FlipToBack (true, rollingSpeed);
			}
			cash.gameDirector.movingCard.Add (cash.row8_1Obj);
			cash.gameDirector.exOyaCard.Add(cash.row8_1Obj);
			cash.gameDirector.willFlip.Add(false);
			cash.gameDirector.openDeck_List.Clear ();

			GameObject currentContent = GameObject.Find("currentContent");


            if (cash.gameDirector.row8_List.Count > 0)
            {
                for (int k = 0; k < cash.gameDirector.row8_List.Count; k++)
                {
                    cash.gameDirector.row8_List[k].GetComponent<BoxCollider2D>().enabled = false;
                }
            }

			cash.scoreText_Playing.AddOneMoveText ();
            cash.scoreText_Playing.ChangeScoreText(cash.scoreText_Playing.isUsedAllYama);
            yield return new WaitForSeconds (0.28f);
			cash.row8_Obj.GetComponent<BoxCollider2D> ().enabled = true;
			yield break;
		}


		if (flipAmount == 1) {
            SetPosToList();
			for(int i = 0; i < flipAmount; i++){ 
				Vector3 FirstPos = new Vector3(0, 0, 0);
				if(list_Amount == 0) FirstPos = position [0];
				if(list_Amount == 1) FirstPos = position [1];
				if(list_Amount >= 2) FirstPos = position [2];

				GameObject willBeRow8_1 = cash.gameDirector.row8_List [0];
				cash.gameDirector.openDeck_List.Add (willBeRow8_1);
                willBeRow8_1.GetComponent<SpriteRenderer>().sortingOrder = 90 + i;
                willBeRow8_1.GetComponent<CardsDirector> ().SetInfoToAnimation (true, FirstPos, movingSpeed, false);
				willBeRow8_1.GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
                cash.sound.GetFlipCardSound();
				cash.gameDirector.row8_List.Remove (willBeRow8_1);
				if(i == flipAmount - 1)lastObj = willBeRow8_1;
				yield return new WaitForSeconds (timeToWait);
			}
		}

		if (flipAmount == 2) {
            SetPosToList();
			for(int i = 0; i < flipAmount; i++){
				Vector3 FirstPos = new Vector3(0, 0, 0);
				if(list_Amount == 0 && i == 0) FirstPos = position [0];
				if(list_Amount >= 1 && i == 0) FirstPos = position [1];
				if(list_Amount == 0 && i == 1) FirstPos = position [1];
				if(list_Amount >= 1 && i == 1) FirstPos = position [2];

				GameObject willBeRow8_1 = cash.gameDirector.row8_List [0];
				cash.gameDirector.openDeck_List.Add (willBeRow8_1);
                willBeRow8_1.GetComponent<SpriteRenderer>().sortingOrder = 90 + i;
                willBeRow8_1.GetComponent<CardsDirector> ().SetInfoToAnimation (true, FirstPos, movingSpeed, false);
				willBeRow8_1.GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
                cash.sound.GetFlipCardSound();
				cash.gameDirector.row8_List.Remove (willBeRow8_1);
				if(i == flipAmount - 1)lastObj = willBeRow8_1;
				yield return new WaitForSeconds (timeToWait);
			}
		}

		if (flipAmount == 3) {
            SetPosToList();
			for(int i = 0; i < flipAmount; i++){ 
				Vector3 pos = position [i];
				GameObject willBeRow8_1 = cash.gameDirector.row8_List [0];
				cash.gameDirector.openDeck_List.Add (willBeRow8_1);
                willBeRow8_1.GetComponent<SpriteRenderer>().sortingOrder = 90 + i;
                willBeRow8_1.GetComponent<CardsDirector> ().SetInfoToAnimation (true, pos, movingSpeed, false);
				willBeRow8_1.GetComponent<CardsDirector> ().FlipToFront (true, rollingSpeed);
                cash.sound.GetFlipCardSound();
				cash.gameDirector.row8_List.Remove (willBeRow8_1);
				if(i == flipAmount - 1)lastObj = willBeRow8_1;
				yield return new WaitForSeconds (timeToWait);
			}
		}
		cash.scoreText_Playing.AddOneMoveText (); 
		cash.gameDirector.movingCard.Add (cash.row8_Obj);
		cash.gameDirector.exOyaCard.Add(cash.row8_Obj);
		cash.gameDirector.willFlip.Add(false);
		cash.gameDirector.amount_BackToRow8.Add(flipAmount);
        lastObj.GetComponent<BoxCollider2D>().enabled = true;

        if (cash.gameDirector.row8_List.Count > 0)
        {
            for (int k = 0; k < cash.gameDirector.row8_List.Count; k++)
            {
                cash.gameDirector.row8_List[k].GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        yield return new WaitForSeconds (0.05f);
        cash.row8_Obj.GetComponent<BoxCollider2D> ().enabled = true;
    }



	public void FixOpenDeck()
	{
		int list_Amount = cash.gameDirector.openDeck_List.Count;
		int lastIndexNum_list = cash.gameDirector.openDeck_List.Count - 1;

		if (list_Amount == 0)
			return;

		if (list_Amount == 1) {
			cash.gameDirector.openDeck_List[lastIndexNum_list].GetComponent<BoxCollider2D> ().enabled = true;
			return;
		}

		GameObject lastCard = cash.gameDirector.openDeck_List[lastIndexNum_list];
        SetPosToList();

		if (list_Amount == 2) {
			for (int i = 0; i < 2; i++) {
				cash.gameDirector.openDeck_List [lastIndexNum_list - 1 + i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, position [0 + i], movingSpeed, false);
			}
		}

		if (list_Amount >= 3) {
			for (int i = 0; i < 3; i++) {
				cash.gameDirector.openDeck_List [lastIndexNum_list - 2 + i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, position [0 + i], movingSpeed, false);
			}
		}

		lastCard.GetComponent<BoxCollider2D> ().enabled = true;
	}



	void FixOpenDeck_1(int flipAmount)
	{
		int lastIndex = cash.gameDirector.openDeck_List.Count - 1;
		int row8_1Amount = cash.gameDirector.openDeck_List.Count;
        SetPosToList();

		if (flipAmount == 1 && row8_1Amount == 0) return;
		if (flipAmount == 1 && row8_1Amount == 1) return;
		if (flipAmount == 1 && row8_1Amount == 2) return;
		if (flipAmount == 1 && row8_1Amount >= 3) { 
			for (int i = 0; i < 2; i++) {
				cash.gameDirector.openDeck_List [lastIndex - 1 + i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, position [0 + i], movingSpeed, false);
			}
			return;
		}

		if (flipAmount == 2 && row8_1Amount == 0) return;
		if (flipAmount == 2 && row8_1Amount == 1) return;
		if (flipAmount == 2 && row8_1Amount >= 2) {			
			for (int i = 0; i < cash.gameDirector.openDeck_List.Count; i++) {
				cash.gameDirector.openDeck_List [i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, position [0], movingSpeed, false);
			}
		}


		if (flipAmount == 3 && row8_1Amount == 0) return;
		if (flipAmount == 3 && row8_1Amount == 1) return;
		if (flipAmount == 3 && row8_1Amount >= 2) {			
			for (int i = 0; i < cash.gameDirector.openDeck_List.Count; i++) {
				cash.gameDirector.openDeck_List [i].GetComponent<CardsDirector> ().SetInfoToAnimation (true, position [0], movingSpeed, false);
			}
		}
	}



}

	





