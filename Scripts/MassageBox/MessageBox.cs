using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Events;

public class MessageBox : ModalBox
{



    Cash cash;



    void Start()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
    }


    public static string PrefabResourceName = "Prefab/Message Box";
    public static Func<string, string> Localize = (sourceString) => { return sourceString; };
    public static bool LocalizeTitleAndMessage = false;

    DialogResult result;
    Action<DialogResult> onFinish;





    /// <summary>
    /// Display a message box.
    /// </summary>
    public static MessageBox Show(string message, string purpose, string title = null, Action<DialogResult> onFinished = null, MessageBoxButtons buttons = MessageBoxButtons.OK)
    {
        var box = (Instantiate(Resources.Load<GameObject>(PrefabResourceName)) as GameObject).GetComponent<MessageBox>();

        box.onFinish = onFinished;

		box.SetUpButtons(purpose);
        box.SetText(message, title);

        return box;
    }




	void SetUpButtons(string purpose)
    {
        var button = Button.gameObject;
		switch (purpose)
        {
		//GameMenuを選択し、『新しいゲームを始めます』を表示し、『はい』『いいえ』を選択した場合
		case DialogConst.PLAY_NEWGAME:
			button.GetComponentInChildren<Text>().text = Localize("YES");
			button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
            {
                result = DialogResult.Yes;
                cash.sound.GetSelectContentSound();
                cash.adsController.ShowInterstitiaAd();
                isEnable = false;
                Close();
            }));
			CreateButton(button, Localize("NO"), () => 
            {
                result = DialogResult.No;
                cash.sound.GetSelectContentSound();
                Close();
            });
			break;

		//Replayを選択し、『このゲームを始めからやり直します』を表示し、『はい』『いいえ』を選択した場合
		case DialogConst.REPLAY_GAME:
			button.GetComponentInChildren<Text>().text = Localize("YES");
			button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
            {
                result = DialogResult.Yes;
                cash.sound.GetSelectContentSound();
                cash.adsController.ShowInterstitiaAd();
                //cash.replay.OnMouseUp();
                isEnable = false;
                Close();
            }));
			CreateButton(button, Localize("NO"), () => 
            {
                result = DialogResult.No;
                cash.sound.GetSelectContentSound();
                Close();
            });
			break;

		//設定画面内のフリップ枚数を変更するボタンを選択し、『このゲームを終了し、" + flipNum + " 枚めくりで新しくゲームを始めます』を表示し、『はい』『いいえ』を選択した場合
		case DialogConst.THREE_FLIP:
			button.GetComponentInChildren<Text>().text = Localize("YES");
			button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
            {
                result = DialogResult.Yes;
                cash.settingContentThreeFlip.ChangeToThreeFlip ();
                    cash.newGame.MakeNewGameFromSetting ();
                isEnable = false;
                Close();
            }));
			CreateButton(button, Localize("NO"), () => 
            {
                result = DialogResult.No;
                cash.sound.GetSelectContentSound();
                Close();
            });
			break;

		case DialogConst.ONE_FLIP:
			button.GetComponentInChildren<Text>().text = Localize("YES");
			button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
            {
                result = DialogResult.Yes;
                cash.settingContentOneFlip.ChangeToOneFlip ();
                    cash.newGame.MakeNewGameFromSetting ();
                isEnable = false;
                Close();
            }));
			CreateButton(button, Localize("NO"), () =>
            {
                result = DialogResult.No;
                cash.sound.GetSelectContentSound();
                Close();
            });
			break;

		case DialogConst.CHANGE_FLIP_BACKED:
			button.GetComponentInChildren<Text>().text = Localize("YES");
			button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
            {
                result = DialogResult.Yes;

                if (cash.touchingDeck.flipAmountIs1 == true)
                {
                    cash.settingContentThreeFlip.ChangeToThreeFlip();
                }
                else
                {
                    cash.settingContentOneFlip.ChangeToOneFlip();
                }

                BackToHome backToHome = GameObject.Find("back").GetComponent<BackToHome>();
                backToHome.ShowNewGameHome(false);
                cash.newGame.MakeNewGameBacked();
                cash.sound.GetSelectContentSound();

                Close();
			}));
			CreateButton(button, Localize("NO"), () => 
            {
                result = DialogResult.No;
                cash.sound.GetSelectContentSound();
                Close();
            });
			break;


            case DialogConst.PLAY_NEWGAME_HOME:
                button.GetComponentInChildren<Text>().text = Localize("YES");
                button.GetComponent<Button>().onClick.AddListener((UnityAction)(() =>
                {
                    result = DialogResult.Yes;
                    cash.sound.GetSelectContentSound();
                    BackToHome backToHome = GameObject.Find("back").GetComponent<BackToHome>();
                    backToHome.ShowNewGameHome(false);
                    cash.newGame.MakeNewGameBacked();


                    Close();
                }));
                CreateButton(button, Localize("NO"), () => 
                {
                    result = DialogResult.No;
                    cash.sound.GetSelectContentSound();
                    Close();
                });
                break;

            case DialogConst.SUGGEST_HOW_TO_PLAY:
                RulesManager rulesManager = GameObject.Find("homeMenu").GetComponent<RulesManager>();
                button.GetComponentInChildren<Text>().text = Localize("YES");
                button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
                {
                    result = DialogResult.Yes;
                    cash.sound.GetSelectContentSound();
                    Close();
                }));
                CreateButton(button, Localize("NO"), () => 
                {
                    result = DialogResult.No;
                    cash.sound.GetSelectContentSound();
                    rulesManager.SetActiveTrue();
                    isEnable = false;
                    Close();
                });
                break;

        case DialogConst.PURCHASE_IS_PROCESSED:
            button.GetComponentInChildren<Text>().text = Localize("OK");
            button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
            {
                result = DialogResult.OK;
                cash.sound.GetSelectContentSound();
                isEnable = false;
                Close();
            }));
            break;

		//case DialogConst.REVIEW_REQUEST:
		//	button.GetComponentInChildren<Text>().text = Localize("YES");
		//	button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
		//	{
  //                  result = DialogResult.Yes; ReviewManager.Instance.RequestReview();
  //              cash.sound.GetSelectContentSound();
  //              PriseWords priseWords = GameObject.Find ("priseWords").GetComponent<PriseWords>();
		//		priseWords.ClosePriseWords();
  //              FinishClearedScores finishClearedScores = GameObject.Find("finishClearedScore").GetComponent<FinishClearedScores>();
		//		finishClearedScores.GoBackToHome();
		//		cash.gameDirector.ChangeBoolIsReviewRequested(true);
  //              Close();
		//	}));
		//	CreateButton(button, Localize("NO"), () =>
		//	{
  //                  result = DialogResult.No;
  //              cash.sound.GetSelectContentSound();
  //              PriseWords priseWords = GameObject.Find ("priseWords").GetComponent<PriseWords>();
		//		priseWords.ClosePriseWords();
		//		FinishClearedScores finishClearedScores = GameObject.Find("finishClearedScore").GetComponent<FinishClearedScores>();
		//		finishClearedScores.GoBackToHome();
		//		cash.gameDirector.ChangeBoolIsReviewRequested(true);
		//		Close();
		//	});
		//	break;

		//case DialogConst.DO_YOU_LIKE_THIS_APP:
		//	button.GetComponentInChildren<Text>().text = Localize("Yes");
		//	button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
		//	{
  //              result = DialogResult.Yes; 
		//		//ios10.3以上で、レビューする場合
		//		if (ReviewManager.Instance.CanReviewInApp == true)
		//		{
  //                  ReviewManager.Instance.RequestReview();
  //                  PriseWords priseWords = GameObject.Find ("priseWords").GetComponent<PriseWords>();
		//			priseWords.ClosePriseWords();
  //                  FinishClearedScores fcs = GameObject.Find("finishClearedScore").GetComponent<FinishClearedScores>();
		//			fcs.GoBackToHome();
		//			cash.gameDirector.ChangeBoolIsReviewRequested(true);
		//		}
		//		//Android or ios10.3以下の場合、ダイアログを出してストアに移動してレビューするか尋ねる
		//		else if(ReviewManager.Instance.CanReviewInApp == false)
		//		{
		//			cash.showMassageBox.ShowReviewRequest();
		//		}
  //              cash.sound.GetSelectContentSound();

  //              Close(); 
		//	}));

		//	CreateButton(button, Localize("No"), () => 
		//		{ 
		//			result = DialogResult.No; 
		//			PriseWords priseWords = GameObject.Find ("priseWords").GetComponent<PriseWords>();
		//			priseWords.ClosePriseWords();
		//			FinishClearedScores fcs = GameObject.Find("finishClearedScore").GetComponent<FinishClearedScores>();
		//			fcs.GoBackToHome();
		//			cash.gameDirector.ChangeBoolIsReviewRequested(true);
  //                  cash.sound.GetSelectContentSound();
  //                  Close(); 
		//		});
		//	break;

        case DialogConst.NO_INTERNET:
            button.GetComponentInChildren<Text>().text = Localize("OK");
            button.GetComponent<Button>().onClick.AddListener((UnityAction)(() => 
            {
                result = DialogResult.OK;
                cash.sound.GetSelectContentSound();
                Close();
            }));
            break;

        }
    }

    public static bool s = true;


    GameObject CreateButton(GameObject buttonToClone, string label, UnityAction target)
    {
        var button = Instantiate(buttonToClone) as GameObject;

        button.transform.SetParent(buttonToClone.transform.parent, false);

        button.GetComponentInChildren<Text>().text = label;
        button.GetComponent<Button>().onClick.AddListener(target);

        return button;
    }

    bool isEnable = true;
    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public override void Close()
    {
        if (onFinish != null)
            onFinish(result);
		cash.showMassageBox.isShowingMassageBox = false;
        if(isEnable == true)
            cash.showMassageBox.EnableBackGraund(true);
        isEnable = true;
        base.Close();
    }
}
