using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour {


	Color color;
    public GameObject backGOfStorePrefab;
    Cash cash;
    GameObject storeObj;

	public bool isShowingSetting = false;


	void Start ()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
	}




	public void OnMouseUp()
	{
        cash.sound.GetButtonSound();
        ShowUpEveryShoppingList();

		if (isShowingSetting == false) {
            if (cash.dealCard.didDeal == false)
            {
                cash.generateAndDealCards_Obj.GetComponent<BoxCollider2D>().enabled = false;
                cash.arrowController.StopInstractionCard();
            }
            if (cash.dealCard.didDeal == true)
            {
                cash.scoreText_Playing.countingTime = false; //timeScoreを止める
                cash.gameDirector.BeFalseOrTrueOpenCards_Retu(false);
                cash.gameDirector.BeFalseOrTrueOpenCards_Yama(false, true);
            }
		}

        //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.Store);

    }


    public void ShowUpEveryShoppingList()
    {
        storeObj = Instantiate(backGOfStorePrefab) as GameObject;
        cash.panelerObj.SetActive(true);
        storeObj.transform.parent = cash.panelerObj.transform;
        storeObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //float delta = Screen.height / 1136f;
        Vector2 size = storeObj.GetComponent<RectTransform>().sizeDelta;
        size.x = 466f;
        size.y = 618f;
        storeObj.GetComponent<RectTransform>().sizeDelta = size;
        storeObj.transform.localScale = new Vector3(1f,1f,0f);

        Text scanRemaningText = GameObject.Find("scanRemaningText").GetComponent<Text>();
        int remaningScan = cash.gameDirector.GetRemainingScan();
        scanRemaningText.text = L.Text.FromKey("scan") + L.Text.FromKey("left scan") + " " + remaningScan + " " + L.Text.FromKey("times");

        GameObject.Find("freeScanText").GetComponent<Text>().text = L.Text.FromKey("free scan");
        GameObject.Find("cost10").GetComponent<Text>().text = L.Text.FromKey("cost10");
        GameObject.Find("cost24").GetComponent<Text>().text = L.Text.FromKey("cost24");
        GameObject.Find("cost80").GetComponent<Text>().text = L.Text.FromKey("cost80");
        GameObject.Find("cost200").GetComponent<Text>().text = L.Text.FromKey("cost200");

        GameObject.Find("cancelStore").GetComponent<Button>().onClick.AddListener(CloseStore);
      
        isShowingSetting = true;
        cash.showBotton.isUiShown = true;
    }





    void CloseStore()
	{
        cash.sound.GetSelectContentSound();

        if (cash.dealCard.didDeal == false)
        {
            cash.generateAndDealCards_Obj.GetComponent<BoxCollider2D>().enabled = true;
            cash.arrowController.MoveInstractionCard();
        }
        if (cash.dealCard.didDeal == true)
        {
            cash.scoreText_Playing.countingTime = true; //timeScoreを止める
            cash.gameDirector.BeFalseOrTrueOpenCards_Retu(true);
            cash.gameDirector.BeFalseOrTrueOpenCards_Yama(true, true);
        }

        cash.showBotton.isUiShown = false;
        Destroy(storeObj);
        cash.panelerObj.SetActive(false);
	}


	


}
