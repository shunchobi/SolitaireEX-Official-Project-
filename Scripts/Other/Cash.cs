using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cash : MonoBehaviour{

    //        cash = GameObject.Find("cashObj").GetComponent<Cash>();


    public NewGame newGame;
    public GameDirector gameDirector;
    public GamesWon gamesWon;
    public Sound sound;
    public ShowBotton showBotton;
    public Setting setting;
    public PlacePos placePos;
    public Replay replay;
    public ScoreText_Playing scoreText_Playing;
    public TouchingDeck touchingDeck;
    public ArrowController arrowController;
    public SaveManager saveManager;
    public AutoComplete autoCom;
    public ScoreText_Played scoreText_Played;
    public DealCard dealCard;
    public FinishClearedScores finishClearedScores;
    public ShowMassageBox showMassageBox;
    public PanelSizeChanger panel;
    public RulesManager rulesManager;
    public AdsController adsController;
    public Hint hint;
    public Game game;
    public Design design;
    public Undo undo_C;
    public PriseWords priseWords;
    public ChangeFlipAmount_Home changeFlipAmount_Home;
    public settingContent settingContentOneFlip;
    public settingContent settingContentThreeFlip;
    //public UnityAdsController unityAdCont;
    //public CompleteProject.Purchaser purchaser;
    public AssetResourceManager assetResourceManager;
    public ReviewController reviewController;



    public GameObject row1_Obj;
    public GameObject row2_Obj;
    public GameObject row3_Obj;
    public GameObject row4_Obj;
    public GameObject row5_Obj;
    public GameObject row6_Obj;
    public GameObject row7_Obj;
    public GameObject row8_Obj;
    public GameObject row8_1Obj;
    public GameObject row8_2Obj;
    public GameObject row8_3Obj;
    public GameObject row9_Obj;
    public GameObject row10_Obj;
    public GameObject row11_Obj;
    public GameObject row12_Obj;

    public GameObject row8_1_Left;
    public GameObject row8_2_Left;
    public GameObject row8_3_Left;
    public GameObject row9_Left;
    public GameObject row10_Left;
    public GameObject row11_Left;
    public GameObject row12_Left;


    public GameObject undo;
    public GameObject hint_obj;
    public GameObject autoComplete_obj;
    public GameObject generateAndDealCards_Obj;
    public GameObject playmat_Obj;
    public GameObject currentContent;


    public GameObject newGameObj;
    public GameObject gameDirectorObj;
    public GameObject gamesWonObj;
    public GameObject soundObj;
    public GameObject showBottonObj;
    public GameObject settingObj;
    public GameObject placePosObj;
    public GameObject shrinkCardsObj;
    public GameObject replayObj;
    public GameObject scoreText_PlayingObj;
    public GameObject touchingDeckObj;
    public GameObject saveManagerObj;
    public GameObject arrowControllerObj;
    public GameObject autoComObj;
    public GameObject scoreText_PlayedObj;
    public GameObject dealCardObj;
    public GameObject finishClearedScoresObj;
    public GameObject showMassageBoxObj;
    public GameObject panelObj;
    public GameObject rulesManagerObj;
    public GameObject adsControllerObj;
    public GameObject currentContentManagerObj;
    public GameObject nextContentManagerObj;
    public GameObject hintObj;
    public GameObject gameObj;
    public GameObject designObj;
    public GameObject undo_CObj;
    public GameObject priseWordsObj;
    public GameObject changeFlipAmount_HomeObj;
    public GameObject settingContentOneFlipObj;
    public GameObject settingContentThreeFlipObj;
    //public GameObject storeObj;
    public GameObject unityAdContObj;
    public GameObject homeObj;
    public GameObject panelerObj;
    public GameObject purchaseObj;
    public GameObject statisticsPanelObj;



    public Button undoButton;
    public Button hintButton;


	public void Init () 
    {
        newGame = newGameObj.GetComponent<NewGame>();
        gameDirector = gameDirectorObj.GetComponent<GameDirector>();
        gamesWon = gamesWonObj.GetComponent<GamesWon>();
        sound = soundObj.GetComponent<Sound>();
        showBotton = showBottonObj.GetComponent<ShowBotton>();
        setting = settingObj.GetComponent<Setting>();
        placePos = placePosObj.GetComponent<PlacePos>();
        replay = replayObj.GetComponent<Replay>();
        scoreText_Playing = scoreText_PlayingObj.GetComponent<ScoreText_Playing>();
        touchingDeck = touchingDeckObj.GetComponent<TouchingDeck>();
        saveManager = saveManagerObj.GetComponent<SaveManager>();
        arrowController = arrowControllerObj.GetComponent<ArrowController>();
        autoCom = autoComObj.GetComponent<AutoComplete>();
        scoreText_Played = scoreText_PlayedObj.GetComponent<ScoreText_Played>();
        dealCard = dealCardObj.GetComponent<DealCard>();
        finishClearedScores = finishClearedScoresObj.GetComponent<FinishClearedScores>();
        showMassageBox = showMassageBoxObj.GetComponent<ShowMassageBox>();
        panel = panelObj.GetComponent<PanelSizeChanger>();
        rulesManager = rulesManagerObj.GetComponent<RulesManager>();
        adsController = adsControllerObj.GetComponent<AdsController>();
        hint = hint_obj.GetComponent<Hint>();
        assetResourceManager = gameDirectorObj.GetComponent<AssetResourceManager>();
        reviewController = GameObject.Find("review").GetComponent<ReviewController>();

        game = gameObj.GetComponent<Game>();
        design = designObj.GetComponent<Design>();
        undo_C = undo.GetComponent<Undo>();
        priseWords = priseWordsObj.GetComponent<PriseWords>();

        changeFlipAmount_Home = changeFlipAmount_HomeObj.GetComponent<ChangeFlipAmount_Home>();
        settingContentOneFlip = settingContentOneFlipObj.GetComponent<settingContent>();
        settingContentThreeFlip = settingContentThreeFlipObj.GetComponent<settingContent>();
        //unityAdCont = unityAdContObj.GetComponent<UnityAdsController>();
        //purchaser = purchaseObj.GetComponent<CompleteProject.Purchaser>();

        undoButton = undo.GetComponent<Button>();
        hintButton = hint_obj.GetComponent<Button>();
	}




    //public List<GameObject> allCards = new List<GameObject>();
    //List<BoxCollider2D> allCardsBoxColli = new List<BoxCollider2D>();
    //List<bool> allCardsIsTurned = new List<bool>();



    //public void SetCards(GameObject card){
    //    allCards.Add(card);
    //    allCardsBoxColli.Add(card.GetComponent<BoxCollider2D>());
    //    allCardsIsTurned.Add(card.GetComponent<Card>().isTurned);
    //}



    //public bool GetIsTurned(int num)
    //{
    //    return allCardsIsTurned[num];
    //}


    //public BoxCollider2D GetBoxCollider(int num){
    //    return allCardsBoxColli[num];
    //}
	
}
