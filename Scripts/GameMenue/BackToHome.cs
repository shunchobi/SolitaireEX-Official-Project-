using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToHome : MonoBehaviour {



    Cash cash;


	void Start () 
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
    }


	public void OnMouseUp()
	{
        cash.sound.GetButtonSound();
        cash.gameDirector.isPlayingSecen = false;
        StartCoroutine("BeCardsBackToHome");
        //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.TopFromGame);
    }


    IEnumerator BeCardsBackToHome()
	{
		if (cash.game.showingThese == true)
			cash.game.OnMouseUp ();
		if (cash.design.showingThese == true)
			cash.design.OnMouseUp();
        
        cash.sound.GetMoveCardToHomeSound();
		cash.scoreText_Playing.countingTime = false; 
		cash.dealCard.dealFromBackToHome = true;
		cash.scoreText_Playing.TransparentEveryScoreText_Playing (); 
		cash.gameDirector.TransparentRetuYamaRow8BackG ();

        cash.gameDirector.ChangeAllCardsEnabled(false);

        cash.gameDirector.DealEveryCardsBackForHome ();
        cash.showBotton.UiMoving_PlayToHome();
        //cash.gamesWon.MovePopGamesWon();
        yield return new WaitForSeconds (0.5f);
        cash.dealCard.FixBoxCollider2DEnabledToHome();
        cash.showBotton.UiMoving_PlayToHome2();
		//cash.dealCard.ShowUpGenerateAndDealObj();
		//cash.dealCard.IsWhenBackToHome(); 
        //cash.arrowController.MoveInstractionCard();
		cash.saveManager.SaveFile_StatisticsDate ();
        ShowNewGameHome(true);
    }


    public void ShowNewGameHome(bool isShow)
    {
        GameObject newh = GameObject.Find("newGameHome");

        Color color = newh.GetComponent<Image>().color;
        if (isShow == true)
        {
            color.a = 1;
            newh.GetComponent<Button>().enabled = true;
        }
        if (isShow == false)
        {
            color.a = 0f;
            newh.GetComponent<Button>().enabled = false;
        }
        newh.GetComponent<Image>().color = color;
    }

}
