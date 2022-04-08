using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DesignMenu : MonoBehaviour
{


    Sprite sprite;
    Cash cash;

	GameObject targetIsToChangeBool;
	public bool isChosen = false;
	string backCardOrPlaymat = "";
	int backCardDesignNum;
	int playmatDesignNum;


	void Start ()
	{
        sprite = GetComponent<Image> ().sprite;
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		if (gameObject.transform.tag == "back_design") {
			backCardOrPlaymat = "backCrad";
		}
		if (gameObject.transform.tag == "playmat_design") {
			backCardOrPlaymat = "playmat";
		}
		
	}




    public void OnMouseUp()
	{
		cash.sound.GetSelectContentSound();
		if (backCardOrPlaymat == "backCrad")
			ChangeBackCardSprite ();
			
		if (backCardOrPlaymat == "playmat")
			ChangePlaymatSprite ();

		SetIntOfMyselfNum(this.gameObject);

		cash.showBotton.BeFalseIsButtonTouched();

	}


	void ChangeBackCardSprite()
	{
		isChosen = true;
		BeFalseToAnother (gameObject);
		cash.setting.backSprite = sprite;
        cash.gameDirector.ChangeBackCardSprite ();
	}


	void ChangePlaymatSprite()
	{
		isChosen = true;
		BeFalseToAnother (gameObject);
		cash.setting.playmatSprite = sprite;
        cash.gameDirector.ChangePlaymatSprite ();
	}


	void BeFalseToAnother(GameObject obj)
	{
		if (backCardOrPlaymat == "backCrad") {
			foreach (Transform child in GameObject.Find("BCContent").gameObject.transform)
			{
				if(child != this.gameObject)
					child.GetComponent<DesignMenu>().isChosen = false;
			}
		}

		if (backCardOrPlaymat == "playmat") {
			foreach (Transform child in GameObject.Find("BGContent").gameObject.transform)
			{
				if (child != this.gameObject)
					child.GetComponent<DesignMenu>().isChosen = false;
			}
		}
	}

	void SetIntOfMyselfNum(GameObject obj)
	{
		cash.setting.SetIntInfoOfPreferenceDate(backCardOrPlaymat, obj.transform.GetSiblingIndex()+1);
	
	}
	
}
