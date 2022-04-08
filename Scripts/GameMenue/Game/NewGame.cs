using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGame : MonoBehaviour {


    Cash cash;


    private void Start()
    {
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
    }


    public void OnMouseUp()
	{
        cash.sound.GetSelectContentSound();
        cash.dealCard.didDeal = false;
        cash.showBotton.ClosePlayingUia();
        StartCoroutine ("MakeNewGame");
	}

	public void MakeNewGameFromSetting()
	{
        cash.dealCard.didDeal = false;
        cash.showBotton.ClosePlayingUia();
        StartCoroutine ("MakeNewGame");
	}

	IEnumerator MakeNewGame()
	{
        GetComponent<Button> ().enabled = false;
		cash.game.TransparentReplayAndNewGame ();
		cash.gameDirector.GatherEveryCardsToPlayNewGame (1.8f);
        yield return new WaitForSeconds(1.3f);
        cash.dealCard.ChangeColor(true);
        cash.replay.ClearReplayList();
        cash.undo_C.ResetUndo();

        cash.gameDirector.ClearEveryLists();
        cash.gameDirector.DestroyEverCards();

        PlacePos placePos = GameObject.Find("Main Camera").GetComponent<PlacePos>();
        cash.dealCard.CreatCardsFromPrefab(placePos.scaleCard, GameObject.Find("generateAndDealCards").transform.position);

        cash.dealCard.showUpPlayingScoreText = false;
		cash.dealCard.isNewGameDealing = true;
        cash.gameDirector.isScaned = false;
        cash.dealCard.StartDealCoroutine ();
        cash.dealCard.ChangeColor(false);
        cash.gameDirector.AddPlaytimeStatistics();

        cash.scoreText_Playing.ResetMove();
		cash.scoreText_Playing.ResetTime();
        cash.scoreText_Playing.ResetScore();
        yield return new WaitForSeconds(2.7f);
        cash.showBotton.ShoUpPlayingUi();
    }



    public void MakeNewGameBacked()
    {
        StartCoroutine("CallMakeNewGameBacked");
    }

    IEnumerator CallMakeNewGameBacked()
	{
        cash.dealCard.ShowUpGenerateAndDealObj ();
		cash.dealCard.dealFromBackToHome = false;
		cash.dealCard.showUpPlayingScoreText = true;
        cash.gameDirector.isScaned = false;

        cash.replay.ClearReplayList();
        cash.undo_C.ResetUndo();
        cash.gameDirector.DestroyEverCards();
        yield return new WaitForSeconds(0.3f);
        cash.gameDirector.ClearEveryLists();

        PlacePos placePos = GameObject.Find("Main Camera").GetComponent<PlacePos>();
        cash.dealCard.CreatCardsFromPrefab(placePos.scaleCard, GameObject.Find("generateAndDealCards").transform.position);
        cash.gameDirector.AddPlaytimeStatistics();

        cash.dealCard.showUpPlayingScoreText = true;
		cash.scoreText_Playing.ResetMove();
		cash.scoreText_Playing.ResetTime();
		cash.scoreText_Playing.ResetScore();
	}

}
