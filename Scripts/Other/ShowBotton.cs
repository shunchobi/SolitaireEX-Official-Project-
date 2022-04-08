using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBotton : MonoBehaviour {
	//playmatのスクリプト

	GameObject game;
	GameObject design;
	GameObject hint;
	GameObject undo;
	GameObject setting;
	GameObject back;
	GameObject gamesWon;
	GameObject gameTitle;
	GameObject flip3Button;
	GameObject flip1Button;
	GameObject scanFieldTabs;
	GameObject flipAmountText;
    //GameObject newGameHome;
    GameObject gamesWonText;
    //GameObject store;


    //UIObjectの画面内にいるときのオリジナルの座標を取得、保持しておき、動かして停止した際にオリジナルの座標に設置する。




    Cash cash;
    ChangeFlipAmount_Home changeFlipAmount_Home;


	public bool dealed = false;

	public bool uiAreClosedByTouching = false;

	bool homeToPlay = false;
	bool playToHome = false;
	bool playToClear = false;
	bool clearToHome = false;
	bool clearHome = false;
	bool showHome = false;
	bool uiAreOnScreen_Playing = false;

	bool uiAreOnScreen_Playing_Move = false;
	bool homeToPlay_Move = false;
	bool playToHome_Move = false;
	bool playToClear_Move = false;
	bool clearToHome_Move = false;


    Vector3 endPosBottomUi;
    Vector3 endPosUpperUi;
    Vector3 speedBottom;
    Vector3 speedUpper;
    Vector3 count;

	List<Button> allButton = new List<Button> ();

    bool isMoving = false;

    bool isShowNewGameHome = false;

	void Start () 
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

        endPosBottomUi = new Vector3(0f, 510f, 0f);
        endPosUpperUi = new Vector3(510f, 0f, 0f);
        speedBottom = new Vector3(0f, 30f, 0f);
        speedUpper = new Vector3(30f, 0f, 0f);
        count = Vector3.zero;


        game = GameObject.Find ("game");
		design = GameObject.Find ("design");
		hint = GameObject.Find ("hint");
		undo = GameObject.Find ("undo");
		setting = GameObject.Find ("setting");
		back = GameObject.Find ("back");
		gamesWon = GameObject.Find ("gamesWon");
		gameTitle = GameObject.Find ("gameTitle");
		flip3Button = GameObject.Find ("3FlipButton");
		flip1Button = GameObject.Find ("1FlipButton");
		scanFieldTabs = GameObject.Find ("scanFieldTabs");
		flipAmountText = GameObject.Find ("flipAmountText");
        //newGameHome = GameObject.Find("newGameHome");
        gamesWonText = GameObject.Find("gamesWonText");
        //store = GameObject.Find("store");








        changeFlipAmount_Home = GameObject.Find ("homeMenu").GetComponent <ChangeFlipAmount_Home> ();

        allButton.Add (game.GetComponent<Button> ());
		allButton.Add (design.GetComponent<Button> ());
		allButton.Add (hint.GetComponent<Button> ());
		allButton.Add (undo.GetComponent<Button> ());
		allButton.Add (setting.GetComponent<Button> ());
		allButton.Add (back.GetComponent<Button> ());
		allButton.Add (gamesWon.GetComponent<Button> ());
		allButton.Add (game.GetComponent<Button> ());
		allButton.Add (game.GetComponent<Button> ());
		allButton.Add (flip3Button.GetComponent<Button> ());
		allButton.Add (flip1Button.GetComponent<Button> ());

		OpenApp();
    }







	/// <summary>
	/// アプリを立ち上げる時のUiの座標を調整
	/// </summary>
	void OpenApp()
	{
		game.transform.position -= endPosBottomUi;
        design.transform.position -= endPosBottomUi;
        hint.transform.position -= endPosBottomUi;
        undo.transform.position -= endPosBottomUi;
        back.transform.position -= endPosUpperUi;
        //newGameHome.transform.position -= endPosBottomUi * 3;
    }



    public bool isButtonTouched = false;

    public void BeFalseIsButtonTouched()
    {
        isButtonTouched = false;
    }

    public void BeTrueIsButtonTouched()
    {
        isButtonTouched = true;
    }

    public bool isUiShown = false;

    void Update()
	{
        //if (Input.GetMouseButtonDown(0) && cash.game.showingThese == true && isButtonTouched == false)
        //{
        //    cash.game.TransparentReplayAndNewGame();
        //    return;
        //}
        //if (Input.GetMouseButtonDown(0) && cash.design.showingThese == true && isButtonTouched == false)
        //{
        //    cash.design.ShowUpOrTranspaentEveryDesignMenu(false);
        //    return;
        //}


  //      if (cash.setting.isShowingSetting == false && cash.game.showingThese == false && isUiShown == false && cash.dealCard.didDeal == true && cash.autoCom.isAutoComp == false &&
  //          cash.design.showingThese == false && cash.rulesManager.isShowingRules == false && cash.showMassageBox.isShowingMassageBox == false && cash.autoCom.isTouched == false) {


  //          if (isMoving == false)
  //          {
  //              //play中で、playingUiが無い時
  //              if (Input.GetMouseButtonDown(0) && cash.gameDirector.isPlayingSecen == true && uiAreClosedByTouching == false)
  //              {
  //                  bool isTouchedOnPlaymat = cash.gameDirector.GetBoolIsTouchedOnPlaymat(Camera.main.ScreenToWorldPoint(Input.mousePosition));
  //                  if (isTouchedOnPlaymat == true)
  //                  {
  //                      uiAreClosedByTouching = true;
  //                      ClosePlayingUia();
  //                      return;
  //                  }
  //              }

  //              //play中で、playingUiがある時
  //              if (Input.GetMouseButtonDown(0) && cash.gameDirector.isPlayingSecen == true && uiAreClosedByTouching == true)
  //              {
  //                  bool isTouchedOnPlaymat = cash.gameDirector.GetBoolIsTouchedOnPlaymat(Camera.main.ScreenToWorldPoint(Input.mousePosition));
  //                  if (isTouchedOnPlaymat == true)
  //                  {
  //                      uiAreClosedByTouching = false;
  //                      ShoUpPlayingUi();
  //                      return;
  //                  }
  //              }
  //          }
		//}

		/////////////////////////////////////////////////////////////////////////////////////
		//uiAreOnScreen_Playing or Not
		//Play中にすべてのUIを消す
		if (uiAreOnScreen_Playing == true && uiAreOnScreen_Playing_Move == true) {
            count += speedBottom;

            game.transform.position -= speedBottom;
			design.transform.position -= speedBottom;
			hint.transform.position -= speedBottom;
			undo.transform.position -= speedBottom;
			setting.transform.position += speedUpper;
			back.transform.position -= speedUpper;


            if (count.y >= endPosBottomUi.y)
            {
                count = Vector3.zero;
                uiAreOnScreen_Playing_Move = false;
                isMoving = false;
            }
        }

		//Play中に消したすべてのUIを出す
		if (uiAreOnScreen_Playing == false && uiAreOnScreen_Playing_Move == true) {
            count += speedBottom;

            game.transform.position += speedBottom;
			design.transform.position += speedBottom;
			hint.transform.position += speedBottom;
			undo.transform.position += speedBottom;
			setting.transform.position -= speedUpper;
			back.transform.position += speedUpper;


            if (count.y >= endPosBottomUi.y)
            {
                count = Vector3.zero;
                uiAreOnScreen_Playing_Move = false;
                isMoving = false;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////
		//homeToPlay
		if (homeToPlay == true && homeToPlay_Move == true) {
            count += speedBottom;

            game.transform.position += speedBottom;
			design.transform.position += speedBottom;
			hint.transform.position += speedBottom;
			undo.transform.position += speedBottom;
			back.transform.position += speedUpper;
            setting.transform.position -= speedUpper;
            //store.transform.position -= speedUpper;


            if (count.y >= endPosBottomUi.y)
            {
                uiAreClosedByTouching = false;
                count = Vector3.zero;
                homeToPlay_Move = false;
                homeToPlay = false;
            }
		}
		/////////////////////////////////////////////////////////////////////////////////////
		//playToHome1
		if (playToHome == true && playToHome_Move == true) {
            count += speedBottom;

				game.transform.position -= speedBottom;
				design.transform.position -= speedBottom;
				hint.transform.position -= speedBottom;
				undo.transform.position -= speedBottom;
				back.transform.position -= speedUpper;
                setting.transform.position += speedUpper;


            if (count.y >= endPosBottomUi.y)
            {
                count = Vector3.zero;
                playToHome_Move = false;
                playToHome = false;
            }
		}

        /////////////////////////////////////////////////////////////////////////////////////
        //playToHome2
        if (playToHome2 == true && playToHome_Move2 == true)
        {
            count += speedBottom;
                setting.transform.position -= speedUpper;
            //store.transform.position += speedUpper;

            
            gamesWon.transform.position += speedBottom * 2;
                flip3Button.transform.position += speedBottom * 3;
                flip1Button.transform.position += speedBottom * 3;
                flipAmountText.transform.position += speedBottom * 3;
                gameTitle.transform.position += speedUpper * 6f;
                //newGameHome.transform.position += speedBottom * 3;
                gamesWonText.transform.position += speedBottom * 3;

            if (count.y >= endPosBottomUi.y)
            {
                cash.dealCard.ShowUpGenerateAndDealObj();
                cash.dealCard.IsWhenBackToHome(); 
                cash.arrowController.MoveInstractionCard();

                count = Vector3.zero;
                playToHome_Move2 = false;
                playToHome2 = false;
                uiAreClosedByTouching = false;
                isShowNewGameHome = true;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //playToClear
        if (playToClear == true && playToClear_Move == true) {
            count += speedBottom;

            if (uiAreClosedByTouching == true) {
                count = Vector3.zero;
                playToClear_Move = false;
				playToClear = false;
                uiAreClosedByTouching = false;
                return;
			}

			if (uiAreClosedByTouching == false) {
				game.transform.position -= speedBottom;
				design.transform.position -= speedBottom;
				hint.transform.position -= speedBottom;
				undo.transform.position -= speedBottom;
				setting.transform.position += speedUpper;
				back.transform.position -= speedUpper;
			}

            if (count.y >= endPosBottomUi.y)
            {
                count = Vector3.zero;
                playToClear_Move = false;
                playToClear = false;
                uiAreClosedByTouching = false;
            }
			
		}
		/////////////////////////////////////////////////////////////////////////////////////
		//clearToHome
		if (clearToHome == true && clearToHome_Move == true) {
            count += speedBottom;

            setting.transform.position -= speedUpper;
            //store.transform.position += speedUpper;

            flip3Button.transform.position += speedBottom * 3;
			flip1Button.transform.position += speedBottom * 3;
			flipAmountText.transform.position += speedBottom * 3;
			gameTitle.transform.position += speedUpper * 6f;//6
            gamesWonText.transform.position += speedBottom * 3f;//3

            if (count.y >= endPosBottomUi.y)
            {
                count = Vector3.zero;
                clearToHome_Move = false;
                clearToHome = false;
                cash.dealCard.ShowUpGenerateAndDealObj ();
                cash.arrowController.MoveInstractionCard ();
            }
			
		}
		/////////////////////////////////////////////////////////////////////////////////////

		/////////////////////////////////////////////////////////////////////////////////////
		//clearHome
		if (clearHome == true && clearToHome_Move == true) {
            count += speedBottom;
            
            setting.transform.position += speedUpper;
            //store.transform.position -= speedUpper;

            flip3Button.transform.position -= speedBottom * 3;
			flip1Button.transform.position -= speedBottom * 3;
			flipAmountText.transform.position -= speedBottom * 3;
			gameTitle.transform.position -= speedUpper * 6f;
            gamesWon.transform.position -= speedBottom * 2;
            gamesWonText.transform.position -= speedBottom * 3;
            //if (isShowNewGameHome == true)
                //newGameHome.transform.position -= speedBottom * 3;


            if (count.y >= endPosBottomUi.y)
            {
                count = Vector3.zero;
                clearToHome_Move = false;
                clearHome = false;
                isShowNewGameHome = false;
            }
		}
		/////////////////////////////////////////////////////////////////////////////////////
	}




	public void ClearHome()
	{
		clearHome = true;
		clearToHome_Move = true;
	}


    public void ShoUpPlayingUi()
    {
        uiAreOnScreen_Playing = false;
        uiAreOnScreen_Playing_Move = true;
        isMoving = true;
    }

    public void ClosePlayingUia()
    {
        uiAreOnScreen_Playing = true;
        uiAreOnScreen_Playing_Move = true;
        isMoving = true;
    }


    /// <summary>
    /// home → play
    /// </summary>
    public void UiMoving_HomeToPlay()
	{
		homeToPlay = true;
		homeToPlay_Move = true;
	}

	/// <summary>
	/// play → home
	/// </summary>
	public void UiMoving_PlayToHome()
	{
		playToHome = true;
		playToHome_Move = true;
	}


    bool playToHome2 = false;
    bool playToHome_Move2 = false;
    /// <summary>
    /// play → home
    /// </summary>
    public void UiMoving_PlayToHome2()
    {
        playToHome2 = true;
        playToHome_Move2 = true;
    }


    /// <summary>
    /// play → clear
    /// </summary>
    public void UiMoving_PlayToClear()
	{
        if (uiAreClosedByTouching == false)
        {
            playToClear = true;
            playToClear_Move = true;
        }
	}




	public void PlaceGameTitle(bool isShowUp)
	{
		Color color = gameTitle.GetComponent<Image> ().color;
		if (isShowUp == true) {
			gameTitle.transform.position += pos;
			color.a = 255f;
		}
		if (isShowUp == false) {
			gameTitle.transform.position -= pos;
			color.a = 0f;
		}
		gameTitle.GetComponent<Image> ().color = color;
	}


	Vector3 pos = new Vector3 (1000, 1000f, 0);


	/// <summary>
	/// clear → home
	/// </summary>
	public void UiMoving_ClearToHome()
	{
		clearToHome = true;
		clearToHome_Move = true;
	}



	public void AllUiEnable(bool enabled)
	{
		for (int i = 0; i < allButton.Count; i++) {
			allButton [i].enabled = enabled;
		}
	}


}
