using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsTextController : MonoBehaviour {


    Cash cash;

	List<Text> upperStatisticsTextList = new List<Text> ();
	List<Text> belowStatisticsTextList = new List<Text> ();

	List<float> aupperStatisticsValueList = new List<float> ();
	List<float> belowStatisticsValueList = new List<float> ();



	public void Init () 
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		upperStatisticsTextList.Add (GameObject.Find ("BestTimeTex").GetComponent<Text>());
		upperStatisticsTextList.Add (GameObject.Find ("BestMoveTex").GetComponent<Text>());
		upperStatisticsTextList.Add (GameObject.Find ("BestScoreTex").GetComponent<Text>());
		upperStatisticsTextList.Add (GameObject.Find ("BestTotalScoreTex").GetComponent<Text>());
		upperStatisticsTextList.Add (GameObject.Find ("WinningPercentageTex").GetComponent<Text>());
		upperStatisticsTextList.Add (GameObject.Find ("GameWonCountTex").GetComponent<Text>());
		upperStatisticsTextList.Add (GameObject.Find ("GameCountTex").GetComponent<Text>());

		belowStatisticsTextList.Add (GameObject.Find ("TotalGameCountTex").GetComponent<Text>());
		belowStatisticsTextList.Add (GameObject.Find ("TotalPlayTimeTex").GetComponent<Text>());
    }

    


    /// <summary>
    /// それぞれのboolの条件にあった上のscoreを表示
    /// </summary>
    public void DisplayUpperSideStatistics(bool isthreeFlipScore, bool isScsnScore)
	{

		//Listに表示したい値を加える
		if(aupperStatisticsValueList.Count > 0)
	    	aupperStatisticsValueList.Clear ();
		aupperStatisticsValueList = cash.gameDirector.GetListUpperSideStatistics (!isthreeFlipScore, isScsnScore);

        //勝率の計算
        float a = aupperStatisticsValueList[4] / aupperStatisticsValueList[5] * 10000f;
        float b = Mathf.Floor (a) / 100f;
        string winningPercentageTex = " ";
        if (aupperStatisticsValueList[4] == 0)
            winningPercentageTex = "0%";
        else
            winningPercentageTex = b.ToString() + "%";
       
		//Listに表示
		for(int i = 0; i < upperStatisticsTextList.Count; i++){
			string targetScore = "";
			//BestTimeTex, BestMoveTex, BestScoreTex, BestTotalScoreTex
            if(i == 0)
            {
                float timeBest = aupperStatisticsValueList[i];
                float minuteTimeBest = Mathf.Floor(timeBest / 60f);
                float secondTimeBest = timeBest - minuteTimeBest * 60f;
                string secontSt = secondTimeBest.ToString();
                if (secondTimeBest < 10f)
                    secontSt = "0" + secontSt;
                upperStatisticsTextList[i].text = minuteTimeBest.ToString() + ":" + secontSt;
            }
			if (i != 0 && i <= 3) {
				targetScore = aupperStatisticsValueList [i].ToString ();
				upperStatisticsTextList [i].text = targetScore;
			}
			//WinningPercentageTex
			if (i == 4) {
				targetScore = winningPercentageTex;
				upperStatisticsTextList [i].text = targetScore;
			}
			//GameWonCountTex, GameCountTex
			if (i >= 5) {
				targetScore = aupperStatisticsValueList [i - 1].ToString ();
				upperStatisticsTextList [i].text = targetScore;
			}
		}
	}



	/// <summary>
	/// それぞれのboolの条件にあった下のscoreを表示
	/// </summary>
	public void DisplayBelowSideStatistics()
	{
		//Listに表示したい値を加える
		if(belowStatisticsValueList.Count != 0)
			belowStatisticsValueList.Clear ();
		belowStatisticsValueList = cash.gameDirector.GetListBelowSideStatistics();

		string targetScore = belowStatisticsValueList[0].ToString ();
		belowStatisticsTextList [0].text = targetScore;

        float totalSecondTime = belowStatisticsValueList[1];
		float hours = Mathf.Floor (totalSecondTime / 3600f);
		float m = Mathf.Floor (totalSecondTime - 3600f * hours);
        float minuts = Mathf.Floor(m / 60f);
        float second = Mathf.Floor (m - 60f * minuts);
        //belowStatisticsTextList [1].text = hours.ToString () + " : " + minuts.ToString () + " : " + second.ToString ();
        belowStatisticsTextList[1].text = hours.ToString() + "h " + minuts.ToString() + "m";

    }


}
