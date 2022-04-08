using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishClearedScores : MonoBehaviour {

    Cash cash;

	float timeToWait = 0.05f;
    bool isPresented = false;

	void Start () 
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        Transparent();
    }



	public void OnMouseUp()
	{
        cash.sound.GetSelectContentSound();

        //レビュー依頼するときかを調べ、そおなら依頼し、違うならホームへ戻す
        //bool isTimeToRequestReview = ReviewManager.Instance.GetIsTimeToRequestReview();
        //if (isTimeToRequestReview == false)
        //{
        cash.priseWords.ClosePriseWords();

        Color color1 = GameObject.Find("star1").GetComponent<Image>().color;
        color1.a = 0;
        GameObject.Find("star1").GetComponent<Image>().color = color1;
        Color color2 = GameObject.Find("star2").GetComponent<Image>().color;
        color2.a = 0;
        GameObject.Find("star2").GetComponent<Image>().color = color2;
        Color color3 = GameObject.Find("star3").GetComponent<Image>().color;
        color3.a = 0;
        GameObject.Find("star3").GetComponent<Image>().color = color3;
        Color color4 = GameObject.Find("star4").GetComponent<Image>().color;
        color4.a = 0;
        GameObject.Find("star4").GetComponent<Image>().color = color4;

        cash.gamesWon.SetPosHomeOrOutOfScreen(true);
        GoBackToHome();
  //          return;
  //      }
		//else if (isTimeToRequestReview == true) {
            //ShowMassageBox showMassageBox = GameObject.Find("callMassageBox").GetComponent<ShowMassageBox>();
            //showMassageBox.DoYouLikeThisApp ();
            //ReviewManager.Instance.RequestReview();
            //PriseWords priseWords = GameObject.Find("priseWords").GetComponent<PriseWords>();
            //priseWords.ClosePriseWords();
            //FinishClearedScores finishClearedScores = GameObject.Find("finishClearedScore").GetComponent<FinishClearedScores>();
            //finishClearedScores.GoBackToHome();
            //cash.gameDirector.ChangeBoolIsReviewRequested(true);
        //}
    }



    void StartGameFromClearedGame()
    {
        cash.scoreText_Played.TransparentEveryScoreText_Played();
        cash.scoreText_Played.MovePlayedScoreDown();
        cash.gamesWon.ChangeGamesWonBackGColor(true);
        GameObject particle = GameObject.Find("particle");
        particle.GetComponent<ParticleManager>().DestroyFireWorks();
        Transparent();
        cash.gameDirector.isAutoCompShown = false;
        cash.autoCom.isAutoComp = false;
        isPresented = false;

        GamesWon.MoveToHomePos();
        GamesWon.ChangeScaleOfGamesWonNum();

    }




    public void GoBackToHome()
    {
        cash.scoreText_Played.TransparentEveryScoreText_Played();
        cash.scoreText_Played.MovePlayedScoreDown();
        cash.gamesWon.ChangeGamesWonBackGColor(true);
        GamesWon.MoveToHomePos();
        GamesWon.ChangeScaleOfGamesWonNum();
        GameObject particle = GameObject.Find("particle");
        particle.GetComponent<ParticleManager>().DestroyFireWorks();
        Transparent();
        cash.gameDirector.isAutoCompShown = false;
        cash.autoCom.isAutoComp = false;
        isPresented = false;
    }


    public void CallMakeNewCardsAtHome()
    {
        StartCoroutine("MakeNewCardsAtHome");
    }


    IEnumerator MakeNewCardsAtHome()
	{
        cash.sound.GetMoveCardToHomeSound();
        cash.dealCard.CreatCardsFromPrefab(cash.placePos.scaleCard,new Vector3 (Screen.width + 100f, 200f, 0f));
        cash.dealCard.PlaceDealCardsList(1f);//1.5
        yield return new WaitForSeconds (0.7f);//1.6
        //cash.dealCard.ShowUpGenerateAndDealObj();
        cash.showBotton.UiMoving_ClearToHome();
        //cash.gamesWon.MovePopGamesWon();

        cash.scoreText_Playing.ResetMove();
		cash.scoreText_Playing.ResetTime();
		cash.scoreText_Playing.ResetScore();
		cash.dealCard.IsWhenBackToHome ();
		cash.dealCard.dealFromBackToHome = false;
        //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.TopFromCleared);

    }





    public void Transparent()
	{
        Color color = gameObject.GetComponent<Image>().color;
        color.a = 0f;
        gameObject.GetComponent<Image>().color = color;
        gameObject.GetComponent<Button> ().enabled = false;
	}

	public void ShowUp()
	{
        Color color = gameObject.GetComponent<Image>().color;
        color.a = 1f;
        gameObject.GetComponent<Image>().color = color;
        gameObject.GetComponent<Button> ().enabled = true;
    }

	
}
