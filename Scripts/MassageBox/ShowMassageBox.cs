using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;


public class ShowMassageBox : MonoBehaviour
{


    Cash cash;

	public bool isShowingMassageBox = false;

	public void Initilaze()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
    }





	public void PlayNewGameMessage()
	{
        cash.sound.GetSelectContentSound();
		cash.game.OnMouseUp ();
        cash.gameDirector.isCallAdsByNewGame = true;

        string yesOrNo = L.Text.FromKey("yes or no");
        string playNewGame = L.Text.FromKey("play new game");

        MessageBox.Show
		(
            yesOrNo,
            DialogConst.PLAY_NEWGAME,
            playNewGame,
			null,
			MessageBoxButtons.YesNo
		);
		isShowingMassageBox = true;
        EnableBackGraund(false);
    }


    public void PlayNewGameMessageHome()
    {
        cash.sound.GetSelectContentSound();

        string yesOrNo = L.Text.FromKey("yes or no");
        string playNewGame = L.Text.FromKey("play new game");

        MessageBox.Show
        (
            yesOrNo,
            DialogConst.PLAY_NEWGAME_HOME,
            playNewGame,
            null,
            MessageBoxButtons.YesNo
        );
        isShowingMassageBox = true;
        EnableBackGraund(false);
    }




    public void ReplayGameMessage()
	{
        cash.sound.GetSelectContentSound();
        cash.game.OnMouseUp ();
        cash.gameDirector.isCallAdsByNewGame = false;

        string yesOrNo = L.Text.FromKey("yes or no");
        string replayGame = L.Text.FromKey("replay game");

        MessageBox.Show
		(
            yesOrNo,
            DialogConst.REPLAY_GAME,
            replayGame,
			null,
			MessageBoxButtons.YesNo
		);
		isShowingMassageBox = true;
        EnableBackGraund(false);
    }



	public void FlipThreeMessage()
	{
		if (cash.dealCard.didDeal == false) {
			cash.settingContentThreeFlip.ChangeToThreeFlip ();

		} else if (cash.dealCard.didDeal == true) {
            cash.setting.CloseSetting ();
            EnableBackGraund(false);

            string yesOrNo = L.Text.FromKey("yes or no");
            string threeFlip = L.Text.FromKey("end game play 3 flip");

            MessageBox.Show
		(
                yesOrNo,
				DialogConst.THREE_FLIP,
                threeFlip,
				null,
				MessageBoxButtons.YesNo
			);
		}
		isShowingMassageBox = true;
    }


	public void FlipOneMessage()
	{
		if (cash.dealCard.didDeal == false) {
			cash.settingContentOneFlip.ChangeToOneFlip ();

		} else if (cash.dealCard.didDeal == true) {
            EnableBackGraund(false);

            cash.setting.CloseSetting ();
            string yesOrNo = L.Text.FromKey("yes or no");
            string oneFlip = L.Text.FromKey("end game play 1 flip");

            MessageBox.Show
		    (
                yesOrNo,
				DialogConst.ONE_FLIP,
                oneFlip,
				null,
				MessageBoxButtons.YesNo
			);
		}
		isShowingMassageBox = true;
    }


	/// <summary>
	/// backToHomeでホームに戻ってから、設定画面でめくる枚数を変更した時
	/// それまで遊んでいたゲームを破棄して、めくり枚数を変更する
	/// </summary>
	public void ChangeFlipBackedToHome()
	{
        cash.setting.CloseSetting ();

        string yesOrNo = L.Text.FromKey("yes or no");
        string changeFlip = L.Text.FromKey("end game change flip");

        MessageBox.Show
		(
            yesOrNo,
			DialogConst.CHANGE_FLIP_BACKED,
            changeFlip,
			null,
			MessageBoxButtons.YesNo
		);
		isShowingMassageBox = true;
        EnableBackGraund(false);
    }


	//public void DoYouLikeThisApp()
	//{
 //       string thankYouUseApp = L.Text.FromKey("thank you use app");
 //       string likeThisApp = L.Text.FromKey("like this app");

 //       MessageBox.Show
	//	(
 //           likeThisApp,
	//		DialogConst.DO_YOU_LIKE_THIS_APP,
 //           thankYouUseApp,
	//		null,
	//		MessageBoxButtons.YesNo
	//	);
	//	isShowingMassageBox = true;
 //       EnableBackGraund(false);
 //   }


    //public void ShowReviewRequest()
    //{
    //    string gladYouLike = L.Text.FromKey("glad you like");
    //    string goReview = L.Text.FromKey("go review");

    //    MessageBox.Show
    //    (
    //        goReview,
    //        DialogConst.REVIEW_REQUEST,
    //        gladYouLike,
    //        null,
    //        MessageBoxButtons.YesNo
    //    );
    //    isShowingMassageBox = true;
    //    EnableBackGraund(false);
    //}


    public void SuggestHowToPlay()
    {
        string howToPlay = "";
        string howIsSolitaire = L.Text.FromKey("how is solitaire");

        MessageBox.Show
        (
            howToPlay,
            DialogConst.SUGGEST_HOW_TO_PLAY,
            howIsSolitaire,
            null,
            MessageBoxButtons.YesNo
        );
        isShowingMassageBox = true;
        EnableBackGraund(false);
    }



    public void ShowScanPurchaseIsProcessed(string content)
    {
        string couldPurchase = L.Text.FromKey("could purchase");
        string obtainScan = L.Text.FromKey("scan times is") + " " + content + " " + L.Text.FromKey("you got");
        string removeAds = L.Text.FromKey("remove ads");

        MessageBox.Show
        (
            obtainScan,
            DialogConst.PURCHASE_IS_PROCESSED,
            couldPurchase,
            null,
            MessageBoxButtons.OK
        );
        isShowingMassageBox = true;
        EnableBackGraund(false);
    }


    public void ShowObtainScan(string content)
    {
        Debug.Log("content; " + content);
        string obtainScan = L.Text.FromKey("scan times is") + " " + content + " " + L.Text.FromKey("you got");

        MessageBox.Show
        (
            obtainScan,
            DialogConst.PURCHASE_IS_PROCESSED,
            "",
            null,
            MessageBoxButtons.OK
        );
        isShowingMassageBox = true;
    }



    public void ShowAdsPurchaseIsProcessed()
    {
        string removeAds = L.Text.FromKey("remove ads");
        string couldPurchase = L.Text.FromKey("could purchase");

        MessageBox.Show
        (
            removeAds,
            DialogConst.PURCHASE_IS_PROCESSED,
            couldPurchase,
            null,
            MessageBoxButtons.OK
        );
        isShowingMassageBox = true;
    }



    public void ShowPurchaseIsFailed(string content)
    {
        string couldntPurchase = L.Text.FromKey("couldnt purchase");

        MessageBox.Show
        (
            content,
            DialogConst.PURCHASE_IS_PROCESSED,
            couldntPurchase,
            null,
            MessageBoxButtons.OK
        );
        isShowingMassageBox = true;
        EnableBackGraund(false);
    }




    public void ShowHowToUseScan()
    {
        string howIsScan = L.Text.FromKey("how is scan");
        string firstScan = L.Text.FromKey("first scan");

        MessageBox.Show
        (
            howIsScan,
            DialogConst.THIS_IS_FIRST_TIME_USE_SCAN,
            firstScan,
            null,
            MessageBoxButtons.YesNo
        );
        isShowingMassageBox = true;
        EnableBackGraund(false);
    }



    public void ShowNoInternet()
    {
        MessageBox.Show
        (
            "No Internet",
            DialogConst.NO_INTERNET,
            "",
            null,
            MessageBoxButtons.OK
        );
        isShowingMassageBox = true;
        EnableBackGraund(false);
    }




    public void EnableBackGraund(bool enable)
    {
        if(enable == false)
        {
            if (cash.dealCard.didDeal == false)
            {
                GameObject.Find("generateAndDealCards").GetComponent<BoxCollider2D>().enabled = false;
                GameObject.Find("instractionCard").GetComponent<ArrowController>().StopInstractionCard();
            }
            if (cash.dealCard.didDeal == true)
            {
                ScoreText_Playing scoreText_Playing = GameObject.Find("playingScore").GetComponent<ScoreText_Playing>();
                scoreText_Playing.countingTime = false; //timeScoreを止める
                cash.gameDirector.BeFalseOrTrueOpenCards_Retu(false);
                cash.gameDirector.BeFalseOrTrueOpenCards_Yama(false, true);
            }
        }
        else if (enable == true)
        {
            if (cash.dealCard.didDeal == false)
            {
                GameObject.Find("generateAndDealCards").GetComponent<BoxCollider2D>().enabled = true;
                GameObject.Find("instractionCard").GetComponent<ArrowController>().MoveInstractionCard();
            }
            if (cash.dealCard.didDeal == true)
            {
                ScoreText_Playing scoreText_Playing = GameObject.Find("playingScore").GetComponent<ScoreText_Playing>();
                scoreText_Playing.countingTime = true; //timeScoreを止める
                cash.gameDirector.BeFalseOrTrueOpenCards_Retu(true);
                cash.gameDirector.BeFalseOrTrueOpenCards_Yama(true, true);
            }
        }
    }
}