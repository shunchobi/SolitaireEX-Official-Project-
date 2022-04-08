using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreText_Played : MonoBehaviour {


	Color color;

    GameObject playedScoreImage;

	Text numOfMinuteTime_ThisGame_Text;
	Text numOfSecondTime_ThisGame_Text;
	Text numOfMove_ThisGame_Text;
	Text numOfScore_ThisGame_Text;
	Text numOfMinuteTime_TheBest_Text;
	Text numOfSecondTime_TheBest_Text;
	Text numOfMove_TheBest_Text;
	Text numOfScore_TheBest_Text;

    Text numOfTotal_ThisGame_Text;
    Text numOfTotal_TheBest_Text;




    void Start () 
	{
        playedScoreImage = GameObject.Find("playedScoreImage");

        GameObject numOfMinuteTime_ThisGame_Obj = GameObject.Find("numOfMinuteTime_ThisGame");
        GameObject numOfSecondTime_ThisGame_Obj = GameObject.Find("numOfSecondTime_ThisGame");
        GameObject numOfMove_ThisGame_Obj = GameObject.Find("numOfMove_ThisGame");
        GameObject numOfScore_ThisGame_Obj = GameObject.Find("numOfScore_ThisGame");
        GameObject numOfMinuteTime_TheBest_Obj = GameObject.Find("numOfMinuteTime_TheBest");
        GameObject numOfSecondTime_TheBest_Obj = GameObject.Find("numOfSecondTime_TheBest");
        GameObject numOfMove_TheBest_Obj = GameObject.Find("numOfMove_TheBest");
        GameObject numOfScore_TheBest_Obj = GameObject.Find("numOfScore_TheBest");


		numOfMinuteTime_ThisGame_Text = numOfMinuteTime_ThisGame_Obj.GetComponent<Text> ();
		numOfSecondTime_ThisGame_Text = numOfSecondTime_ThisGame_Obj.GetComponent<Text> ();
		numOfMove_ThisGame_Text = numOfMove_ThisGame_Obj.GetComponent<Text>();
		numOfScore_ThisGame_Text = numOfScore_ThisGame_Obj.GetComponent<Text>();
		numOfMinuteTime_TheBest_Text = numOfMinuteTime_TheBest_Obj.GetComponent<Text>();
		numOfSecondTime_TheBest_Text = numOfSecondTime_TheBest_Obj.GetComponent<Text>();
		numOfMove_TheBest_Text = numOfMove_TheBest_Obj.GetComponent<Text> ();
		numOfScore_TheBest_Text = numOfScore_TheBest_Obj.GetComponent<Text>();

        GameObject numOfTotal_ThisGame_Obj = GameObject.Find("numOfTotal_ThisGame");
        GameObject numOfTotal_TheBest_Obj = GameObject.Find("numOfTotal_TheBest");
        numOfTotal_ThisGame_Text = numOfTotal_ThisGame_Obj.GetComponent<Text>();
        numOfTotal_TheBest_Text = numOfTotal_TheBest_Obj.GetComponent<Text>();

        TransparentEveryScoreText_Played();
		MovePlayedScoreDown ();
	}




	public void SetNum_ScoreTextPlayed(string textName, float num)
	{
		foreach (Transform child in playedScoreImage.transform) {
			if (textName == "numOfMinuteTime_ThisGame") 
				numOfMinuteTime_ThisGame_Text.text = num.ToString ();
			if (textName == "numOfSecondTime_ThisGame") {
				string secondTime = "";
				if (num < 10f)
					secondTime = "0" + num.ToString ();
				if(num >= 10f)
					secondTime = num.ToString ();
				numOfSecondTime_ThisGame_Text.text = secondTime;
			}
			if (textName == "numOfMove_ThisGame") 
				numOfMove_ThisGame_Text.text = num.ToString ();			
			if (textName == "numOfScore_ThisGame") 
				numOfScore_ThisGame_Text.text = num.ToString ();			
			if (textName == "numOfMinuteTime_TheBest") 
				numOfMinuteTime_TheBest_Text.text = num.ToString ();
			if (textName == "numOfSecondTime_TheBest") {
				string secondTime = "";
				if (num < 10f)
					secondTime = "0" + num.ToString ();
				else if(num >= 10f)
					secondTime = num.ToString ();
				numOfSecondTime_TheBest_Text.text = secondTime;
			}		
			if (textName == "numOfMove_TheBest") 
				numOfMove_TheBest_Text.text = num.ToString ();			
			if (textName == "numOfScore_TheBest") 
				numOfScore_TheBest_Text.text = num.ToString ();	
			if (textName == "numOfTotal_ThisGame") 
				numOfTotal_ThisGame_Text.text = num.ToString ();
			if (textName == "numOfTotal_TheBest") 
				numOfTotal_TheBest_Text.text = num.ToString ();
		}
	}



	public void MovePlayedScoreDown()
	{
		Vector2 closeDownPos = new Vector2 (0, 3000f);

        playedScoreImage.GetComponent<RectTransform> ().anchoredPosition = closeDownPos;
	}                                                                      
                                                                           

	public void MovePlayedScoreUp()
	{
		Vector2 showUpPos = new Vector2 (0, 280f);

        playedScoreImage.GetComponent<RectTransform> ().anchoredPosition = showUpPos;
	}                                                                      
                                                                           
                                                                           




	public void TransparentEveryScoreText_Played()
	{
		foreach (Transform child in playedScoreImage.transform) {
            if (child.transform.name != "finishClearedScore" &&
				child.transform.name != "star1" &&
				child.transform.name != "star2" &&
				child.transform.name != "star3" &&
				child.transform.name != "star4")
			{
                color = child.gameObject.GetComponent<Text>().color;
                color.a = 0f;
                child.gameObject.GetComponent<Text>().color = color;
            }
		}
	}

	public void ShowUpEveryScoreText_Played()
	{
		foreach (Transform child in playedScoreImage.transform) {
            if (child.transform.name != "finishClearedScore" &&
				child.transform.name != "star1" &&
				child.transform.name != "star2" &&
				child.transform.name != "star3" &&
				child.transform.name != "star4")
            {
                color = child.gameObject.GetComponent<Text>().color;
                color.a = 255f;
                child.gameObject.GetComponent<Text>().color = color;
            }
		}
	}

}
