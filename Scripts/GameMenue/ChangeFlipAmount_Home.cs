using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFlipAmount_Home : MonoBehaviour {



    Cash cash;

	GameObject flip1Obj;
	GameObject flip3Obj;

	Button flip3Button;
	Button flip1Button;


	float flip1ButtonHeight;
	float flip1ButtonWidth;

    Vector3 flip1ObjPos;
    Vector3 flip3ObjPos;
    Vector3 kirikaesiPos;


    float elapsedTime = 0f;
	float elapsedTime_1 = 0f;
	float elapsedTime_2 = 0f;
	float timeToArrive = 0.4f;
	bool isChangeFlipButton = false;
	bool isMoveFrontButton = false;
	bool isMoveBackButton = false;
	bool isMoveAfterKirikaesi = false;

	public bool dealed = false;





	public void InitilaizeChangeFlipAmount_HomeAllMember()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		flip3Obj = GameObject.Find ("3FlipButton");
		flip1Obj = GameObject.Find ("1FlipButton");
		flip3Button = flip3Obj.GetComponent<Button> ();
		flip1Button = flip1Obj.GetComponent<Button> ();
		flip1ButtonHeight = flip1Obj.GetComponent<RectTransform> ().rect.height;
		flip1ButtonWidth = flip1Obj.GetComponent<RectTransform> ().rect.width;

        Text text = GameObject.Find("flipAmountText").GetComponent<Text>();
        text.text = L.Text.FromKey("flip one");
    }




    public void ChangeFlipButtonToThree()
	{
        flip1ObjPos = flip1Obj.transform.position;
        flip3ObjPos = flip3Obj.transform.position;
        flip1Obj.transform.position = flip3ObjPos;
		flip3Obj.transform.position = flip1ObjPos;
        flip3Obj.transform.SetSiblingIndex(1);
        cash.touchingDeck.flipAmountIs1 = false;
		flip1Button.enabled = false;
		flip3Button.enabled = true;

        Text text = GameObject.Find("flipAmountText").GetComponent<Text>();
        text.text = L.Text.FromKey("flip three");
    }



    void Update()
	{
		if (isChangeFlipButton == true) {
			if (cash.touchingDeck.flipAmountIs1 == true) {
				if(isMoveFrontButton == true){
			    	Vector3 startPos = flip1Obj.transform.position;
			    	elapsedTime += Time.deltaTime; 
			     	float t = elapsedTime / timeToArrive;
			    	if (t > 1.0f)
			    		t = 1.0f;
			    	float rate = t * t * (3.0f - 2.0f * t);
			    	flip1Obj.transform.position = flip1Obj.transform.position * (1.0f - rate) + flip3ObjPos * rate;
					if (elapsedTime >= timeToArrive) {
                        flip1Obj.transform.position = flip3ObjPos;
						isMoveFrontButton = false;
                        elapsedTime = 0f;
					}
				}

				if(isMoveBackButton == true){
		    		Vector3 startPos_1 = flip3Obj.transform.position;
		    		elapsedTime_1 += Time.deltaTime; 
			    	float c = elapsedTime_1 / timeToArrive; 
			    	if (c > 1.0f)
			    		c = 1.0f; 
			    	float rate_1 = c * c * (3.0f - 2.0f * c); 
			    	flip3Obj.transform.position = flip3Obj.transform.position * (1.0f - rate_1) + kirikaesiPos * rate_1;
					if (elapsedTime_1 >= timeToArrive) {
                        Text text = GameObject.Find("flipAmountText").GetComponent<Text>();
                        text.text = L.Text.FromKey("flip three");
                        flip3Obj.transform.position = kirikaesiPos;
						elapsedTime_1 = 0f;
                        //cash.sound.GetSelectFlipAmount2Sound();
                        flip3Obj.transform.SetSiblingIndex(1);
						isMoveBackButton = false;
						isMoveAfterKirikaesi = true;
					}

				}

				if(isMoveAfterKirikaesi == true){
					Vector3 startPos_2 = flip3Obj.transform.position;
					elapsedTime_2 += Time.deltaTime;  
					float f = elapsedTime_2 / timeToArrive;   
					if (f > 1.0f)
						f = 1.0f;   
					float rate_2 = f * f * (3.0f - 2.0f * f);  
					flip3Obj.transform.position = flip3Obj.transform.position * (1.0f - rate_2) + flip1ObjPos * rate_2;   // いわゆるLerp
					if (elapsedTime_2 >= timeToArrive) {
                        flip3Obj.transform.position = flip1ObjPos;
						elapsedTime_2 = 0f;
                        cash.touchingDeck.flipAmountIs1 = false;
                        isMoveAfterKirikaesi = false;
						isChangeFlipButton = false;
						flip1Button.enabled = false;
						flip3Button.enabled = true;
                        GameObject.Find("flipOne").GetComponent<Button>().enabled = true;
                        return;
					}

				}
			}




			if (cash.touchingDeck.flipAmountIs1 == false) { 
				if(isMoveFrontButton == true){
					Vector3 startPos = flip3Obj.transform.position;
					elapsedTime += Time.deltaTime; 
					float t = elapsedTime / timeToArrive; 
					if (t > 1.0f)
						t = 1.0f;   
					float rate = t * t * (3.0f - 2.0f * t); 
					flip3Obj.transform.position = flip3Obj.transform.position * (1.0f - rate) + flip1ObjPos * rate;   // いわゆるLerp
					if (elapsedTime >= timeToArrive) {
						flip3Obj.transform.position = flip1ObjPos;
						isMoveFrontButton = false;
						elapsedTime = 0f;
					}
				}

				if(isMoveBackButton == true){
					Vector3 startPos_1 = flip1Obj.transform.position;
					elapsedTime_1 += Time.deltaTime; 
					float c = elapsedTime_1 / timeToArrive;   
					if (c > 1.0f)
						c = 1.0f;   
					float rate_1 = c * c * (3.0f - 2.0f * c);  
					flip1Obj.transform.position = flip1Obj.transform.position * (1.0f - rate_1) + kirikaesiPos * rate_1;   // いわゆるLerp
					if (elapsedTime_1 >= timeToArrive) {
                        Text text = GameObject.Find("flipAmountText").GetComponent<Text>();
                        text.text = L.Text.FromKey("flip one");
                        flip1Obj.transform.position = kirikaesiPos;
						elapsedTime_1 = 0f;
                        //cash.sound.GetSelectFlipAmount2Sound();
                        flip1Obj.transform.SetSiblingIndex(1);
						isMoveBackButton = false;
						isMoveAfterKirikaesi = true;
					}
				}

				if(isMoveAfterKirikaesi == true){
					Vector3 startPos_2 = flip1Obj.transform.position;
					elapsedTime_2 += Time.deltaTime;  
					float f = elapsedTime_2 / timeToArrive; 
					if (f > 1.0f)
						f = 1.0f;   
					float rate_2 = f * f * (3.0f - 2.0f * f);  
					flip1Obj.transform.position = flip1Obj.transform.position * (1.0f - rate_2) + flip3ObjPos * rate_2;   // いわゆるLerp
					if (elapsedTime_2 >= timeToArrive) {
                        flip1Obj.transform.position = flip3ObjPos;
                        cash.touchingDeck.flipAmountIs1 = true;
                        elapsedTime_2 = 0f;
						isMoveAfterKirikaesi = false;
						isChangeFlipButton = false;
						flip3Button.enabled = false;
						flip1Button.enabled = true;
                        GameObject.Find("flipThree").GetComponent<Button>().enabled = true;
                        return;
					}
				}
			}
		}
	}
		

	public void OnMouseUp()
	{
        //cash.sound.GetSelectFlipAmount1Sound();

        if (cash.touchingDeck.flipAmountIs1 == true)
        {
            cash.settingContentThreeFlip.isFromFlipB = true;
            cash.settingContentThreeFlip.OnMouseUp();
        }
        else if (cash.touchingDeck.flipAmountIs1 == false)
        {
            cash.settingContentOneFlip.isFromFlipB = true;
            cash.settingContentOneFlip.OnMouseUp();
        }
	}


	public void ChangeFlipButton()
	{
        MakePos();

        isMoveFrontButton = true;
		isChangeFlipButton = true;
        isMoveBackButton = true;
    }


    void MakePos()
    {
        flip1ObjPos = new Vector3(flip1Obj.transform.position.x, flip1Obj.transform.position.y, 0f);
        flip3ObjPos = new Vector3(flip3Obj.transform.position.x, flip3Obj.transform.position.y, 0f);

        if (cash.touchingDeck.flipAmountIs1 == false) {
            float xKirikaesi = Mathf.Floor(flip1ObjPos.x + flip1ButtonWidth / 2.5f);
            float yKirikaesi = Mathf.Floor(flip1ObjPos.y + flip1ButtonHeight / 1.5f);
            kirikaesiPos = new Vector3(xKirikaesi, yKirikaesi, 0f);

        }
        else
        {
            float xKirikaesi = Mathf.Floor(flip3ObjPos.x + flip1ButtonWidth / 2.5f);
            float yKirikaesi = Mathf.Floor(flip3ObjPos.y + flip1ButtonHeight / 1.5f);
            kirikaesiPos = new Vector3(xKirikaesi, yKirikaesi, 0f);
        }
    }

}
