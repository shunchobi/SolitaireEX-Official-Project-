using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	Color color_Button;
	Color color_Text;

	GameObject newGame_Obj;
	GameObject backGOfGame_Obj;

    Cash cash;

	Vector3 oridinalPos = new Vector3 ();
	Vector3 movedPos = new Vector3 (0, -500f, 0);

	public bool showingThese = false;


	void Start () 
	{
		newGame_Obj = GameObject.Find ("newGame");
		backGOfGame_Obj = GameObject.Find ("backGOfGame");
		oridinalPos = backGOfGame_Obj.transform.position;
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		GameObject.Find("closeGameMenu").GetComponent<Image>().sprite
				  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("statistics_quit_btn");

		TransparentReplayAndNewGame ();
	}


	public void OnMouseUp()
	{
		cash.sound.GetButtonSound ();


        if (showingThese == false) {
			cash.design.ShowUpOrTranspaentEveryDesignMenu (false);
			ShowUpReplayAndNewGame ();
			return;
		}

		if (showingThese == true) {
			TransparentReplayAndNewGame ();
			return;
		}
	}



	public void TransparentReplayAndNewGame()
	{
		backGOfGame_Obj.transform.position = movedPos;
		
		showingThese = false;
		newGame_Obj.GetComponent<Button>().enabled = false;

		ChangeColor_Ui (newGame_Obj, 0f);
		ChangeColor_Ui (backGOfGame_Obj, 0f);
	}



	public void ShowUpReplayAndNewGame()
	{
		backGOfGame_Obj.transform.position = oridinalPos;

		showingThese = true;
		newGame_Obj.GetComponent<Button>().enabled = true;

		ChangeColor_Ui (newGame_Obj, 255f);
		ChangeColor_Ui (backGOfGame_Obj, 255f);
	}


	void ChangeColor_Ui(GameObject ui, float numOfColor)
	{
		color_Button = ui.GetComponent<Image> ().color;
		color_Button.a = numOfColor;
		ui.GetComponent<Image> ().color = color_Button;
	}
	
}
