using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RewardImgChanger : MonoBehaviour
{

    Image rewardBtnImg;
    Cash cash;

    bool isRewared;

    // Start is called before the first frame update
    void Start()
    {
        rewardBtnImg = this.gameObject.GetComponent<Image>();
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        isRewared = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRewared != cash.setting.preferenceDate.isRewarded)
        {

            if (cash.setting.preferenceDate.isRewarded == false)
            {
                isRewared = false;
                this.gameObject.GetComponent<Button>().enabled = true;

                foreach (Transform child in this.gameObject.transform)
                {
                    if (child.transform.name == "selectedMark")
                    {
                        Color color = child.GetComponent<Image>().color;
                        color.a = 0f;
                        child.GetComponent<Image>().color = color;
                    }
                }
            }
            else
            {
                isRewared = true;
                this.gameObject.GetComponent<Button>().enabled = false;

                foreach (Transform child in this.gameObject.transform)
                {
                    if (child.transform.name == "selectedMark")
                    {
                        Color color = child.GetComponent<Image>().color;
                        color.a = 255f;
                        child.GetComponent<Image>().color = color;
                    }
                }
            }
        }
    }
}
