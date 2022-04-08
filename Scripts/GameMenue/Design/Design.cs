using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Design : MonoBehaviour {

	GameObject designMenu_Obj;

	Color color;
    Cash cash;

	Vector3 oridinalPos = new Vector3 ();
	//Vector3 movedPos = new Vector3 (0, -500f, 0);
	Vector3 movedPos = new Vector3(0, -2000f, 0);


	public bool showingThese = false;

	void Start () 
	{
		//designMenu_Obj = GameObject.Find ("designMenu");
		designMenu_Obj = GameObject.Find("designScrollMenu");

		cash = GameObject.Find("cashObj").GetComponent<Cash>();
        oridinalPos = designMenu_Obj.transform.position;
        ShowUpOrTranspaentEveryDesignMenu (false);

		GameObject.Find("cardbackT").GetComponent<Text>().text
			= L.Text.FromKey("cardback");

		GameObject.Find("backgroundT").GetComponent<Text>().text
			= L.Text.FromKey("background");

		GameObject.Find("closeDesingScrollMenu").GetComponent<Image>().sprite
				  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("statistics_quit_btn");

		designMenu_Obj.GetComponent<Image>().sprite
				  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("DesignMenuBlack");

	}





	public void OnMouseUp()
	{
        cash.sound.GetButtonSound();

        if (showingThese == false) {
			cash.game.TransparentReplayAndNewGame ();
			ShowUpOrTranspaentEveryDesignMenu (true);
			return;
		}

		if (showingThese == true) {
			ShowUpOrTranspaentEveryDesignMenu (false);
			return;
		}
	}



	public void CloseDesignMenu()
    {
		ShowUpOrTranspaentEveryDesignMenu(false);
	}


	public void ShowUpOrTranspaentEveryDesignMenu(bool button)
	{
		showingThese = button;

		if (button == true)
		{
			designMenu_Obj.transform.position = oridinalPos;
			cash.gameDirector.BeFalseOrTrueOpenCards_Retu(false);
			cash.gameDirector.BeFalseOrTrueOpenCards_Yama(false, true);
			cash.scoreText_Playing.countingTime = false; //timeScoreを止める

		}
		if (button == false)
		{
			designMenu_Obj.transform.position = movedPos;
			cash.gameDirector.BeFalseOrTrueOpenCards_Retu(true);
			cash.gameDirector.BeFalseOrTrueOpenCards_Yama(true, true);
			cash.scoreText_Playing.countingTime = true; //timeScoreを止める

		}

		//color = designMenu_Obj.gameObject.GetComponent<Image> ().color;
		//if(button == true) 
		//	color.a = 255f;
		//if(button == false) 
		//	color.a = 0f;
		//designMenu_Obj.gameObject.GetComponent<Image> ().color = color;

		//foreach (Transform child in designMenu_Obj.transform) {
		//	if (child.transform.tag == "uiImage") {
		//		child.GetComponent<Button> ().enabled = button;
		//		color = child.gameObject.GetComponent<Image> ().color;
		//		if (button == true)
		//			color.a = 255f;
		//		if (button == false)
		//			color.a = 0f;
		//		child.gameObject.GetComponent<Image> ().color = color;
		//	}

		//	if (child.transform.tag == "text") {
		//		color = child.gameObject.GetComponent<Text> ().color;
		//		if (button == true)
		//			color.a = 255f;
		//		if (button == false)
		//			color.a = 0f;
		//		child.gameObject.GetComponent<Text> ().color = color;
		//	}
		//}
	}

}
