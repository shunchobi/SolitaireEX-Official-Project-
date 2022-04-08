using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NoAdsMinLeftController : MonoBehaviour
{

    GameObject noAdsMinLeft;
    Text noAdsMinLeftT;
    Cash cash;

    public float noAdsSecTime = 1800f;//1800f; //30分  (60sec*30min)
    public float countdownResult;

    bool isStartedCountdown = false;


   


    // Start is called before the first frame update
    public void InitNoAdsMinLeftController()
    {
        noAdsMinLeft = GameObject.Find("noAdsMinLeft");
        noAdsMinLeftT = GameObject.Find("noAdsMinLeftT").GetComponent<Text>();
        cash = GameObject.Find("cashObj").GetComponent<Cash>();


        Non_ShowObj();
    }

   

    // Update is called once per frame
    void Update()
    {
        if (isStartedCountdown == true)
        {
            countdownResult = countdownResult - Time.deltaTime;
            DisplayLeftTime(countdownResult);

            if(countdownResult <= 0)
            {
                cash.setting.preferenceDate.isRewarded = false;
                cash.saveManager.SaveFile_PreferenceDate();
                isStartedCountdown = false;
                Non_ShowObj();

                if (Application.internetReachability == NetworkReachability.NotReachable)
                    return;
                cash.adsController.RequestBanner();
                cash.adsController.RequestiIterstitial();
            }
        }
    }


    public void StartNoAdsByReward(float _noAdsSecTime)
    {
        ShowObj();
        countdownResult = _noAdsSecTime;
        isStartedCountdown = true;
    }





    void DisplayLeftTime(float _noAdsSecTime)
    {
        int a = (int)_noAdsSecTime;
        float min_f = Mathf.Floor(a / 60);
        float sec_f = a - min_f * 60;

        string min_s = min_f.ToString();
        if (min_f < 10)
            min_s = 0 + min_s;
        string sec_s = sec_f.ToString();
        if (sec_f < 10)
            sec_s = 0 + sec_s;

        string time_s = min_s + ":" + sec_s;

        noAdsMinLeftT.text = time_s;
    }



    void Non_ShowObj()
    {
        noAdsMinLeft.GetComponent<Image>().enabled = false;
        noAdsMinLeftT.enabled = false;
    }


    void ShowObj()
    {
        noAdsMinLeft.GetComponent<Image>().enabled = true;
        noAdsMinLeftT.enabled = true;
    }


}
