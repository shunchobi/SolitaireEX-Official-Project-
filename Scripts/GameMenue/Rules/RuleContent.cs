using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleContent : MonoBehaviour {


	int fontSize = 23;
    FontStyle fontStyle = FontStyle.Normal;
    


	public void SetRuleImage()
	{

		int ruleNum = gameObject.GetComponent<RuleNumber> ().RuleNum;
		string spritePath =  "rule" + "_" + ruleNum;
        Sprite ruleImage = GameObject.Find("cashObj").GetComponent<Cash>().assetResourceManager.assetBundle.LoadAsset<Sprite>(spritePath);
       
		Image image = null;
		foreach (Transform child in gameObject.transform) {
			if (child.GetComponent<Image> () != null)
				image = child.GetComponent<Image> ();
		}

		image.sprite = ruleImage;


        if (spritePath == "rule_1")
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.name == "Image")
                    child.GetComponent<Image>().sprite
                         = GameObject.Find("cashObj").GetComponent<Cash>().assetResourceManager.assetBundle.LoadAsset<Sprite>("rule_1");
            }
        }

	}


	public void SetRuleText()
	{
		int ruleNum = gameObject.GetComponent<RuleNumber> ().RuleNum;
        List<string> rules = GetRuleWordList(ruleNum);
        string ruleWords = "";

        Text text = null;
		foreach (Transform child in gameObject.transform) {
			if (child.GetComponent<Text> () != null)
				text = child.GetComponent<Text> ();
		}

        for(int i = 0; i < rules.Count; i++)
        {
            if (i >= 1)
                ruleWords = ruleWords + "\n";
            ruleWords = ruleWords + rules[i];
        }


        text.text = ruleWords;
        text.fontSize = fontSize;
        text.fontStyle = fontStyle;

        float height = Screen.height;
        float width = Screen.width;
        if (height / width >= 1.6f && Application.systemLanguage == SystemLanguage.English)
        {
            GameObject oya = GameObject.Find("rule_8");
            foreach(Transform child in oya.transform)
            {
                if(child.gameObject.transform.name == "Text")
                {
                    child.gameObject.GetComponent<Text>().fontSize = 21;
                }
            }
        }

    }


    List<string> GetRuleWordList(int ruleNum)
    {
        List<string> rules = new List<string>();

        switch (ruleNum)
        {
            case 1:
                rules.Add(L.Text.FromKey("rule1_2"));
                rules.Add(L.Text.FromKey("rule1_3"));
                break;
            case 2:
                rules.Add(L.Text.FromKey("rule2"));
                break;
            case 3:
                rules.Add(L.Text.FromKey("rule3"));
                break;
            case 4:
                rules.Add(L.Text.FromKey("rule4"));
                break;
            case 5:
                rules.Add(L.Text.FromKey("rule5_1"));
                rules.Add(L.Text.FromKey("rule5_2"));
                break;
            case 6:
                rules.Add(L.Text.FromKey("rule6_1"));
                rules.Add(L.Text.FromKey("rule6_2"));
                break;
            case 7:
                rules.Add(L.Text.FromKey("rule7"));
                break;
            case 8:
                rules.Add(L.Text.FromKey("rule8"));
                break;
        }
        return rules;
    }


}
