using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Replay : MonoBehaviour {



	DealCard dealCards_C;
	GameDirector gameDirector_C;
	Undo undo_C;
	Game game_C;
	ScoreText_Playing scoreText_Playing_C;
	Sound sound;
    PlacePos placePos;

	GameObject row1_Obj;
	GameObject row2_Obj;
	GameObject row3_Obj;
	GameObject row4_Obj;
	GameObject row5_Obj;
	GameObject row6_Obj;
	GameObject row7_Obj;
	GameObject row8_Obj;


    public List<List<GameObject>> replayLists = new List<List<GameObject>>();
    List<GameObject> rows;

	Vector3 interval = new Vector3 (0, 0, 0);
	float movingSpeed = 1f;
	float rollingSpeed = 0.2f;
    float interval_Acard;




    void Start () 
	{
		row1_Obj = GameObject.Find ("row1");
		row2_Obj = GameObject.Find ("row2");
		row3_Obj = GameObject.Find ("row3");
		row4_Obj = GameObject.Find ("row4");
		row5_Obj = GameObject.Find ("row5");
		row6_Obj = GameObject.Find ("row6");
		row7_Obj = GameObject.Find ("row7");
		row8_Obj = GameObject.Find ("row8");
        rows = new List<GameObject>()
        {
            row1_Obj,row2_Obj,row3_Obj,row4_Obj,row5_Obj,row6_Obj,row7_Obj,row8_Obj
        }
        ;

        GameObject deal_Obj = GameObject.Find ("generateAndDealCards");
		dealCards_C = deal_Obj.GetComponent<DealCard> ();
		GameObject gameDirector_Obj = GameObject.Find ("gameDirectorObject");
		gameDirector_C = gameDirector_Obj.GetComponent<GameDirector> ();
		GameObject undo_Obj = GameObject.Find ("undo");
		undo_C = undo_Obj.GetComponent<Undo> ();
		GameObject game_Obj = GameObject.Find ("game");
		game_C = game_Obj.GetComponent<Game> ();
		GameObject scoreText_Playing_Obj = GameObject.Find ("playingScore");
		scoreText_Playing_C = scoreText_Playing_Obj.GetComponent<ScoreText_Playing> ();
		sound = GameObject.Find ("gameDirectorObject").GetComponent<Sound> ();
        placePos = GameObject.Find("Main Camera").GetComponent<PlacePos>();

    }

    bool isNoReplay = true;
    private void Update()
    {
        if (scoreText_Playing_C.move == 0 && isNoReplay == true)
        {
            gameObject.GetComponent<Button>().enabled = false;
            Color color = gameObject.GetComponent<Image>().color;
            color.a = 0.6f;
            gameObject.GetComponent<Image>().color = color;
            isNoReplay = false;
        }
        else if(scoreText_Playing_C.move > 0 && isNoReplay == false)
        {
            gameObject.GetComponent<Button>().enabled = true;
            Color color = gameObject.GetComponent<Image>().color;
            color.a = 1f;
            gameObject.GetComponent<Image>().color = color;
            isNoReplay = true;
        }
            
    }


    public void OnMouseUp()
	{
        if (scoreText_Playing_C.move > 0)
        {
            GetComponent<Button>().enabled = false;
            interval_Acard = placePos.intervalBackCards;
            game_C.TransparentReplayAndNewGame();
            StartCoroutine("DealCards_Replay");
        }
	}



    IEnumerator DealCards_Replay()
	{
        ArrangeToBeggining();

        scoreText_Playing_C.ResetTime();
		scoreText_Playing_C.ResetMove();
		scoreText_Playing_C.ResetScore();
        undo_C.ResetUndo();
        dealCards_C.FixBoxCollider2DEnabledToPlay ();
        yield return new WaitForSeconds(1.5f);
        ShowMassageBox showMassageBox = GameObject.Find("callMassageBox").GetComponent<ShowMassageBox>();
        showMassageBox.EnableBackGraund(true);
        //WebAnalytics.ChangeScreen(WebAnalytics.ScreenName.Replay);
    }


    void ArrangeToBeggining()
    {
        for(int i = 0; i < replayLists.Count; i++)
        {
            List<GameObject> list = replayLists[i];

            for (int k = 0; k < list.Count; k++)
            {
                GameObject replayObj = list[k];
                GameObject directorObj = FindCard(replayObj);
                Vector3 willPos = GetPos(i, k);
                PlaceObj(directorObj, willPos, i, k);
                ArrangeList(directorObj, i, k);
            }
        }
    }


    void ArrangeList(GameObject obj, int _retuNum, int _indexNum)
    {
        List<GameObject> willList = gameDirector_C.GetList(_retuNum + 1);
        List<GameObject> nowList = gameDirector_C.GetListOfObj(obj);
        nowList.Remove(obj);
        willList.Insert(_indexNum, obj);
    }




    void PlaceObj(GameObject obj, Vector3 pos, int _retuNum, int _indexNum)
    {
        if (_retuNum == 7)
            obj.transform.tag = "deck";
        else
            obj.transform.tag = "retu";

        CardsDirector cd = obj.GetComponent<CardsDirector>();
        if (cd.isChangedBoxSize == true)
            cd.ResetSizeBC2D();

        if (obj.GetComponent<Card>().isTurned == true && _retuNum != _indexNum)
            cd.FlipToBack(true, rollingSpeed);

        if (_retuNum == _indexNum && _retuNum != 7)
            obj.GetComponent<BoxCollider2D>().enabled = true;
        else
            obj.GetComponent<BoxCollider2D>().enabled = false;

        obj.GetComponent<SpriteRenderer>().sortingOrder = _indexNum;
        cd.SetInfoToAnimation(true, pos, 1.2f, false);
    }

    
    Vector3 GetPos(int retuNum, int indexNum)
    {
        Vector3 pos = rows[retuNum].transform.position;
        float yPos = 0;
        if(retuNum != 7)
            yPos = placePos.intervalBackCards * indexNum;
        pos = new Vector3(pos.x, pos.y - yPos, pos.z);
        return pos;
    }


    GameObject FindCard(GameObject replayObj)
    {
        GameObject directorObj = null;
        int replaySuit = replayObj.GetComponent<Card>().suit;
        int replayNum = replayObj.GetComponent<Card>().numOfCard;

        for (int i = 1; i <= 13; i++)
        {
            List<GameObject> list = gameDirector_C.GetList(i);

            for(int k = 0; k < list.Count; k++)
            {
                GameObject obj = list[k];
                int suit = obj.GetComponent<Card>().suit;
                int num = obj.GetComponent<Card>().numOfCard;

                if(replaySuit == suit && replayNum == num)
                {
                    directorObj = obj;
                    break;
                }
            }
        }

        return directorObj;
    }


    public void AddObjToList_Replay(GameObject obj, int rowNum)
	{
        bool isListExist = GetBoolIsListExist(rowNum);
        List<GameObject> list;
        if (isListExist == false)
        {
            list = new List<GameObject>();
            replayLists.Add(list);
        }

        replayLists[rowNum - 1].Add(obj);
    }


    bool GetBoolIsListExist(int rowNum)
    {
        bool result = true;
        if (replayLists.Count < rowNum)
            result = false;

        return result;
    }


	public void ClearReplayList()
	{
        replayLists.Clear();
	}






}
