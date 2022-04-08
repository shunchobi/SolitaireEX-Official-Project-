using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoComplete : MonoBehaviour {

    Cash cash;

    Vector3 movePos;

    public bool isTouched = false;
    public bool isAutoComp = false;

    Image image;
    Button button;


	void Start ()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        movePos = new Vector3(0f, 2000, 0f);
        image = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();

        TransparentAutoCompleteObj ();
	}

	public void TransparentAutoCompleteObj()
	{
        button.enabled = false;
        gameObject.transform.position -= movePos;

        var color = image.color;
		color.a = 0f;
        image.color = color;
	}


	public void ShowUpAutoCompleteObj()
	{
        gameObject.transform.position += movePos;
        button.enabled = true;
        var color = image.color;
		color.a = 255f;
        image.color = color;
	}



	public void OnMouseUpAsButton()
	{
        cash.gameDirector.BeFalseOrTrueOpenCards_Retu(false);
        cash.gameDirector.BeFalseOrTrueOpenCards_Yama(false, true);
        isAutoComp = true;
        cash.gameDirector.AutomaticComplete_Call ();
		TransparentAutoCompleteObj ();
    }


    public void ChangeBoolIsFalse()
    {
        isTouched = false;
    }


    public void ChangeBoolIsTrue()
    {
        isTouched = true;
    }

}
