using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class settingContent : MonoBehaviour {

	Color color;

	public Sprite soundOnSprite;
	public Sprite soundOffSprite;
	public Sprite timeOnSprite;
	public Sprite timeOffSprite;

    Cash cash;

	GameObject flipOne;
	GameObject flipThree;
	GameObject leftHand;
	GameObject rightHand;

	//GameObject flipOneCheckmark;
	//GameObject flipThreeCheckmark;
	//GameObject leftHandCheckmark;
	//GameObject rightHandCheckmark;


	public bool isChecked = false;




	public void InitilaizeSettingContentAllMember() 
	{
		flipOne = GameObject.Find ("flipOne");
		flipThree = GameObject.Find ("flipThree");
		leftHand = GameObject.Find ("leftHand");
		rightHand = GameObject.Find ("rightHand");

		//flipOneCheckmark = GameObject.Find ("flipOneCheckmark");
		//flipThreeCheckmark = GameObject.Find ("flipThreeCheckmark");
		//leftHandCheckmark = GameObject.Find ("leftHandCheckmark");
		//rightHandCheckmark = GameObject.Find ("rightHandCheckmark");

        cash = GameObject.Find("cashObj").GetComponent<Cash>();

        soundOnSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_sound_on");
        soundOffSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_sound_off");
        timeOnSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_timer_on");
        timeOffSprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("setting_timer_off");

	}


    public bool isFromFlipB = false;

	public void OnMouseUp()
	{
        if (isFromFlipB == false && gameObject.transform.name != "soundSetting")
            cash.sound.GetSelectContentSound();
        ProcessForCheckedSetting (gameObject);
        ChangeBoolAsIsChanged (gameObject);
        isFromFlipB = false;
    }





    public void ProcessForCheckedSetting(GameObject checkedSettingUi)
	{
		switch (checkedSettingUi.transform.name)
		{
            case "soundSetting": //サウンド設定
	     		SoundOrSilence (isChecked);
                cash.setting.SetBoolInfoOfPreferenceDate("soundSetting", isChecked);
    			break;
	    	case "timeSetting": //Time/Moveを消す設定
    			RemoveTimeAndMove (isChecked);
	    		cash.setting.SetBoolInfoOfPreferenceDate("timeSetting", isChecked);
	    		break;
	    	case "leftHand": //左利き用設定
       			if (isChecked == false)
                {
	    			ChangePlayStyleToLeftHand ();
	    			cash.setting.SetBoolInfoOfPreferenceDate ("isRightHand", false);
	    		}
	    		break;
    		case "rightHand": //右利き用設定
	    		if (isChecked == false) {
     				ChangePlayStyleToRightHand ();
	    			cash.setting.SetBoolInfoOfPreferenceDate ("isRightHand", true);
    			}
    			break;
    		case "flipThree": //3枚引く設定
	     		if (isChecked == false)
                {
                    if (cash.dealCard.dealFromBackToHome == true)
                    {
                        cash.showMassageBox.ChangeFlipBackedToHome();
                    }
                    else
                    {
                        cash.showMassageBox.FlipThreeMessage();
                    }
    			}
                break;
	    	case "flipOne": //1枚引く設定
                if (isChecked == false)
                {
                    if (cash.dealCard.dealFromBackToHome == true)
                    {
                        cash.showMassageBox.ChangeFlipBackedToHome();
                    }
                    else
                    {
                        cash.showMassageBox.FlipOneMessage();
                    }
	     		}
                break;
    		case "ruleSetting": //ルールを見る
     			ShowTheRules();
	     		break;

		}
	}


	/// <summary>
	/// サウンド
	/// </summary>
	public void SoundOrSilence(bool ischeckedPass)
	{
		if (ischeckedPass == false) {
			cash.gameDirector.beSound = false;
			gameObject.GetComponent<Image> ().sprite = soundOffSprite;
			return;
		}

		if (ischeckedPass == true) {
			cash.gameDirector.beSound = true;
			gameObject.GetComponent<Image> ().sprite = soundOnSprite;
		}
	}



	/// <summary>
	/// 左利きにする　deckとretuの場所を入れ替える
	/// </summary>
	public void ChangePlayStyleToLeftHand()
	{
		//ChangeGameObjectColor (leftHandCheckmark, 255f);
		//ChangeGameObjectColor(leftHandCheckmark, 0f);
		//ChangeGameObjectColor(rightHandCheckmark, 0f);
		rightHand.GetComponent<Image>().sprite
				  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_R_Off");
		leftHand.GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_L_On");


		rightHand.GetComponent<settingContent> ().isChecked = false;
		cash.gameDirector.rightHand = false;
        cash.gameDirector.MoveCardsToRightHand(false);
        if (cash.dealCard.didDeal == true)
            cash.gameDirector.MoveallCardToRightSide(false);
	}

	/// <summary>
	/// 右利きにする　deckとretuの場所を入れ替える
	/// </summary>
	public void ChangePlayStyleToRightHand()
	{
        //ChangeGameObjectColor(leftHandCheckmark, 0f);
		//ChangeGameObjectColor (rightHandCheckmark, 255f);
		//ChangeGameObjectColor(rightHandCheckmark, 0f);
		leftHand.GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_L_Off");
		rightHand.GetComponent<Image>().sprite
  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_R_On");


		leftHand.GetComponent<settingContent> ().isChecked = false;
		cash.gameDirector.rightHand = true;
        cash.gameDirector.MoveCardsToRightHand(true);
        if (cash.dealCard.didDeal == true)
            cash.gameDirector.MoveallCardToRightSide(true);
	}


	/// <summary>
	/// 1枚めくるにし、ゲームを新しく始める
	/// </summary>
	public void ChangeToOneFlip()
	{
		//ChangeGameObjectColor (flipOneCheckmark, 0f);
		//ChangeGameObjectColor (flipThreeCheckmark, 0f);
		flipThree.GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_R_Off");
		flipOne.GetComponent<Image>().sprite
  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_L_On");


		flipThree.GetComponent<settingContent> ().isChecked = false;		
		cash.changeFlipAmount_Home.ChangeFlipButton ();
        isChecked = !isChecked;
        cash.setting.SetBoolInfoOfPreferenceDate("isOneFlipChecked", true);
        GameObject.Find("flipThree").GetComponent<Button>().enabled = false;
    }

    /// <summary>
    /// 3枚めくるにし、ゲームを新しく始める
    /// </summary>
    public void ChangeToThreeFlip()
	{
		//ChangeGameObjectColor (flipOneCheckmark, 0f);
		//ChangeGameObjectColor (flipThreeCheckmark, 0f);
		flipOne.GetComponent<Image>().sprite
		  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_L_Off");
		flipThree.GetComponent<Image>().sprite
  = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("settingB_R_On");


		flipOne.GetComponent<settingContent> ().isChecked = false;
		cash.changeFlipAmount_Home.ChangeFlipButton ();
        isChecked = !isChecked;
        cash.setting.SetBoolInfoOfPreferenceDate("isOneFlipChecked", false);
        GameObject.Find("flipOne").GetComponent<Button>().enabled = false;
    }



    public void RemoveTimeAndMove(bool isCheckedPass)
	{
		if (isCheckedPass == true) {
			cash.scoreText_Playing.ShowUpTimeAndMove_ScoreText_Playing ();
			cash.scoreText_Playing.isShowingTimeAndMove = true;
			gameObject.GetComponent<Image> ().sprite = timeOnSprite;
			return;
		}

		if (isCheckedPass == false) {
			cash.scoreText_Playing.TransparentTimeAndMove_ScoreText_Playing ();
			cash.scoreText_Playing.isShowingTimeAndMove = false;
			gameObject.GetComponent<Image> ().sprite = timeOffSprite;
		}
	}



	void ShowTheRules()
	{
		cash.setting.TransparentEverySetting ();

		cash.rulesManager.SetActiveTrue ();
	}


	void ChangeGameObjectColor(GameObject obj, float numOfAColor)
	{
        color = obj.GetComponent<Image> ().color;
		color.a = numOfAColor;
		obj.GetComponent<Image> ().color = color;
	}



	void ChangeBoolAsIsChanged(GameObject obj)
	{
        if (obj.transform.name == "flipThree" || obj.transform.name == "flipOne")
            return;

        if (obj.transform.name == "timeSetting" || obj.transform.name == "soundSetting") {
				isChecked = !isChecked;
				return;
		}

        if (isChecked == false)
            isChecked = !isChecked;
	}
}
