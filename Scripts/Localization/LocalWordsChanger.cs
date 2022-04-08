using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalWordsChanger : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        SetUpGamesWonWord();
        SetUpGameMenuWord();
    }




    void SetUpGamesWonWord()
    {
        string s = L.Text.FromKey("games won");
        Text text = GameObject.Find("gamesWonText").GetComponent<Text>();
        text.text = s;
    }


    void SetUpGameMenuWord()
    {
        GameObject.Find("gameText").GetComponent<Text>().text = L.Text.FromKey("game");
        GameObject.Find("designText").GetComponent<Text>().text = L.Text.FromKey("design");
        GameObject.Find("hintText").GetComponent<Text>().text = L.Text.FromKey("hint");
        GameObject.Find("undoText").GetComponent<Text>().text = L.Text.FromKey("back");
    }




    void SetUpRuleWords(){
        GameObject.Find("MekurifudaT").GetComponent<Text>().text = L.Text.FromKey("draws");
        GameObject.Find("YamafudaT").GetComponent<Text>().text = L.Text.FromKey("stock");
        GameObject.Find("BafudaT").GetComponent<Text>().text = L.Text.FromKey("tableau");
        GameObject.Find("KumifudaT").GetComponent<Text>().text = L.Text.FromKey("foundations");

    }


}
