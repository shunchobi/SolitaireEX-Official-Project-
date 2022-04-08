using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentsManager : MonoBehaviour {

    Cash cash;


    private void Start()
    {
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
    }

    //この数字回クリアするごとにscanをプレゼント
    float presentTimingGamesWon = 10f;


    public bool GetIsPresentTimeGamesWon(int gamesWon, bool isFirstTimeWon)
    {
        bool result = false;

        if(isFirstTimeWon == true)
            result = true;
        if (cash.gameDirector.GetWonPoint() >= presentTimingGamesWon)
            result = true;

        if (result == true)
            cash.gameDirector.ResetWonPoint();

        return result;
    }


    public int GetPresentScanAmount(int gamesWon)
    {
        int scanAmount = 0;

        if (gamesWon == 1)//初クリア
            scanAmount = 3;
        else//presentTimingGamesWonごと
            scanAmount = 3;

        return scanAmount;
    }

	
	
}
