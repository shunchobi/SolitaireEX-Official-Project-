using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesManager : MonoBehaviour {


	GameObject rulesContent;
	GameObject rulesPanel;
    Cash cash;

	public bool isShowingRules = false;

	public void Init()
	{
		rulesPanel = GameObject.Find ("rulesPanel");
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        rulesContent = GameObject.Find("rulesContent");
        SetNumber(rulesContent);
        SetImage(rulesContent);
        SetTest(rulesContent);

        rulesPanel.SetActive (false);
	}




	public void SetActiveFalse()
	{
		isShowingRules = false;

        if (cash.dealCard.didDeal == false)
        {
            GameObject.Find("generateAndDealCards").GetComponent<BoxCollider2D>().enabled = true;
            cash.arrowController.MoveInstractionCard();
        }
        if (cash.dealCard.didDeal == true)
        {
            cash.scoreText_Playing.countingTime = true;
            cash.gameDirector.BeFalseOrTrueOpenCards_Retu(true);
            cash.gameDirector.BeFalseOrTrueOpenCards_Yama(true, true);
        }

        rulesPanel.SetActive (false);
	}



	public void SetActiveTrue()
	{
		isShowingRules = true;

        if (cash.dealCard.didDeal == false)
        {
            GameObject.Find("generateAndDealCards").GetComponent<BoxCollider2D>().enabled = false;
            cash.arrowController.StopInstractionCard();
        }
        if (cash.dealCard.didDeal == true)
        {
            //playingScoreを止める
            cash.scoreText_Playing.countingTime = false; //timeScoreを止める
            cash.gameDirector.BeFalseOrTrueOpenCards_Retu(false);
            cash.gameDirector.BeFalseOrTrueOpenCards_Yama(false, true);
        }

        rulesPanel.SetActive (true);
		rulesPanel.transform.SetAsLastSibling();
        //how to play
        UnityEngine.UI.Text howToPlay = GameObject.Find("ruleTab").transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        howToPlay.text = L.Text.FromKey("how to play");

        //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.HowTo);

    }




    void SetNumber(GameObject obj)
	{
		int ruleNum = 1;

		foreach (Transform child in obj.transform) {
			child.gameObject.GetComponent<RuleNumber> ().RuleNum = ruleNum;
			ruleNum++;
		}
	}


	void SetImage(GameObject obj)
	{
		foreach (Transform child in obj.transform) {
			child.gameObject.GetComponent<RuleContent> ().SetRuleImage ();
		}
	}

	void SetTest(GameObject obj)
	{
		foreach (Transform child in obj.transform) {
			child.gameObject.GetComponent<RuleContent> ().SetRuleText();
		}
	}
}
