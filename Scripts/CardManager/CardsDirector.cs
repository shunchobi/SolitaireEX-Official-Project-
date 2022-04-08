using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsDirector : MonoBehaviour {

	Card card_C;
	Rigidbody2D rb2D;
	BoxCollider2D bc2D;

    Cash cash;

	public bool isChangedBoxSize = false;
	private bool isMoving = false;
	private Vector3 endPos;
	private float rolling = 0f;
	private float speed_Rolling = 0f;
	float elapsedTime;
	float timeToArrive = 0f;

	private bool beFront = false;
	private bool beBack = false;
	private bool transparentToDestroy = false;
	private bool transparentToShowUp = false;
	private bool rollingAndMovingRandmlly = false;

	float timeToChangedColor = 1.5f;
	private float currentRemainTime = 1f;
	private float currentRemainTime_1 = 0;
	private SpriteRenderer spRenderer;

	public bool isMovingByPlayer = false;

	float randomTimeToArrive = 0f;
	Vector3 randomEndPos = Vector3.zero;



	void Start()
	{
		card_C = gameObject.GetComponent<Card>();
		rb2D = gameObject.GetComponent<Rigidbody2D>();
		bc2D = gameObject.GetComponent<BoxCollider2D>();
		spRenderer = gameObject.GetComponent<SpriteRenderer>();
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

		exBC2DFlag = bc2D.enabled;

	}



    public bool isFlipedAtRetu = false;
    public int layer;
    public bool isToShrink = true;

	bool BC2DFlag;
	bool exBC2DFlag;


	void Update()
	{
		//if(exBC2DFlag != bc2D.enabled)
  //      {
		//	exBC2DFlag = bc2D.enabled;
		//	Debug.Log(card_C.suit + "_" + card_C.numOfCard + " = " + "bc2D.enabled is changed");

		//}


		if (isMovingByPlayer == true){
			if (isMoving == true) {
				elapsedTime += Time.deltaTime;  
				float t = elapsedTime / timeToArrive; 
				if (t > 1.0f)
					t = 1.0f;  
				float rate = t * t * (3.0f - 2.0f * t); 
				transform.position = transform.position * (1.0f - rate) + endPos * rate;

                if (elapsedTime >= timeToArrive)
                {
			        if (gameObject.transform.tag != "hint")
					    SetNumOfOrderInLayer ();


                    if (gameObject.transform.tag != "hint" && isFromPlaying == true)
                    {
                        cash.gameDirector.BeFalseOrTrueOpenCards_Retu(true);
                        cash.gameDirector.BeFalseOrTrueOpenCards_Yama(true, false);
                        cash.undoButton.enabled = true;
                        cash.hintButton.enabled = true;
                        cash.gameDirector.ShowAutomaticCompleteObj();
                        cash.gameDirector.CheckAmountOfAllOfYamaIsForEnding();
                    }
                    isFromPlaying = false;
                    transform.position = endPos;
					elapsedTime = 0f;
					isMovingByPlayer = false;
					isMoving = false;
				}
			}
		}


        if (beFront == true)
        {
            if (isFlipedAtRetu == true)
                bc2D.enabled = false;

            if (isToRoll1 == true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / speed_Rolling;

                float yRoll = 90f * t;
                rb2D.transform.rotation = Quaternion.Euler(new Vector3(0, yRoll, 0));

                if (yRoll >= 90f)
                {
                    card_C.SetFrontSprite();
                    card_C.isTurned = true;
                    elapsedTime = 0f;
                    isToRoll2 = true;
                    isToRoll1 = false;
                }
            }

            if (isToRoll2 == true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / speed_Rolling;

                float yRoll = 90f - 90f * t;
                rb2D.transform.rotation = Quaternion.Euler(new Vector3(0, yRoll, 0));


                if (yRoll <= 0f)
                {
                    rb2D.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    if (isFlipedAtRetu == true)
                    {
                        bc2D.enabled = true;
                        isFlipedAtRetu = false;
                    }
                    beFront = false;
                    isToRoll2 = false;
                }
            }
        }

        if (beBack == true)
		{
            if (isToRoll1 == true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / speed_Rolling;
                float yRoll = 90f * t;
                rb2D.transform.rotation = Quaternion.Euler(new Vector3(0, yRoll, 0));

                if (yRoll >= 90f)
                {
                    card_C.SetBackSprite();
                    card_C.isTurned = false;
                    elapsedTime = 0f;
                    isToRoll2 = true;
                    isToRoll1 = false;
                }
            }

            if (isToRoll2 == true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / speed_Rolling;

                float yRoll = 90f - 90f * t;
                rb2D.transform.rotation = Quaternion.Euler(new Vector3(0, yRoll, 0));


                if (yRoll <= 0f)
                {
                    rb2D.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    rolling = 0f;

					cash.gameDirector.ChangeRetuBackCardBC2DToFalse();

					beBack = false;
                    isToRoll2 = false;
                }
            }
		}

		if (transparentToDestroy == true) {
			
			currentRemainTime += Time.deltaTime;
			float colorPercentage = currentRemainTime / timeToChangedColor;
			float aColor = 255f - 255f * colorPercentage;

			var color = spRenderer.color;
			color.a = (int)aColor/255f;
			spRenderer.color = color;

			if (aColor <= 15f){
				currentRemainTime = 0f;
				GameObject.Destroy (gameObject);
				transparentToDestroy = false;
				return;
			}
		}
		if (transparentToShowUp == true) {
			
			currentRemainTime_1 += Time.deltaTime;
			float colorPercentage = currentRemainTime_1 / timeToChangedColor;
			float aColor = 255f * colorPercentage;

			var color = spRenderer.color;
			color.a = (int)aColor/255f;
			spRenderer.color = color;
		
			if (aColor >= 255f){
				currentRemainTime_1 = 0;
				transparentToShowUp = false;
     		}
		}

		if (rollingAndMovingRandmlly == true) {
			elapsedTime += Time.deltaTime; 
			float t = elapsedTime / randomTimeToArrive;
			if (t > 1.0f)
				t = 1.0f;    
			float rate = t * t * (3.0f - 2.0f * t);  
			rb2D.position = transform.position * (1.0f - rate) + randomEndPos * rate;  

			if (elapsedTime >= randomTimeToArrive) {
				elapsedTime = 0f;
				rollingAndMovingRandmlly = false;
				cash.gameDirector.CallToShowScores ();
			}
		}

		//	bool isExpand = false;
		//float expandStepPercent = 0.02f;
		//float expandMaxPercent = 1.1f;//カードのScaleの値に対して何％カードを大きくするか

		//if (isExpand == true)
		//{
		//	elapsedTime += Time.deltaTime;
		//	float t = elapsedTime / expandTime;
		//	if (t > 1.0f)
		//		t = 1.0f;
		//	float rate = t * t * (3.0f - 2.0f * t);
		//	this.gameObject.transform.localScale
		//		= new Vector3(this.gameObject.transform.localScale.x * (1.0f - rate) + expandEndScale * rate,
		//					  this.gameObject.transform.localScale.y * (1.0f - rate) + expandEndScale * rate,
		//					  0);

		//	if (elapsedTime >= expandTime)
		//	{
		//		elapsedTime = 0f;
		//		isExpand = false;
  //              reducs = true;
  //          }
		//}


		//if (reducs == true)
		//{
		//	elapsedTime += Time.deltaTime;
		//	float t = elapsedTime / expandTime;
		//	if (t > 1.0f)
		//		t = 1.0f;
		//	float rate = t * t * (3.0f - 2.0f * t);
		//	this.gameObject.transform.localScale
		//		= new Vector3(this.gameObject.transform.localScale.x * (1.0f - rate) + expandOridinalScale * rate,
		//					  this.gameObject.transform.localScale.y * (1.0f - rate) + expandOridinalScale * rate,
		//					  0);

		//	if (elapsedTime >= expandTime)
		//	{
		//		elapsedTime = 0f;
  //              bc2D.enabled = true;
  //              reducs = false;
		//	}
		//}
		
		



		if (isCircularMotion == true){
	        MoveToCircle();
			MoveToFigureOfEight();
		}

        if (isBlining == true)
        {
            if (Time.time > _nextTime)
            {
                SwichColorAlpher();
                _nextTime += _interval;
                //finished
                if (Time.time > _blinkingTime)
                {
                    FinishBlink();
                }

            }

        }

    }


	//bool isExpand = false;
	//bool reducs = false;
	//float expandStepPercent = 0.02f;
	//float expandMaxPercent = 1.2f;//カードのScaleの値に対して何％カードを大きくするか
	//float expandTime = 0.2f;
	//float expandEndScale = 0;
	//float expandOridinalScale = 0;

	//public void ExpandCard()
 //   {
 //       bc2D.enabled = false;
 //       expandOridinalScale = this.gameObject.transform.localScale.x;
	//	expandEndScale = this.gameObject.transform.localScale.x * expandMaxPercent;
	//	isExpand = true;
	//}



    public bool isBlining = false;
    private float _nextTime;
    private float _interval = 2f;   // 点滅周期
    private float _blinkingTime = 0f;

    public void FinishBlink()
    {
        isBlining = false;
        ColorAlpherTo(1.0f);
    }

    public void StartBlink(float interval, float blinkingTime)
    {
        if (interval != 0)
            _interval = interval;
        if (blinkingTime != 0)
            _blinkingTime = Time.time + blinkingTime;
        _nextTime = Time.time + _interval; 
        isBlining = true;
    }


    private void SwichColorAlpher()
    {
        var color = spRenderer.color;
        color.a = (spRenderer.color.a == 0) ? 1.0f : 0.0f;
        spRenderer.color = color;
    }
    private void ColorAlpherTo(float a)
    {
        var color = spRenderer.color;
        color.a = a;
        spRenderer.color = color;
    }



    public bool isCircularMotion = false;
	public float m_moveSpeed = 2f;
	public float m_radius = 4f;
	public float m_period = 2f;

	void MoveToCircle()
	{
		float time = Time.time;
		float x = Mathf.Sin(time);
		float y = 0.0f;
		float z = Mathf.Cos(time);
		transform.position = new Vector3(x, y, z);
	}
	void MoveToFigureOfEight()
	{
		float time = Time.time;
		float x = Mathf.Cos(m_period) * m_radius;
		float y = 0.0f;             
		float z = Mathf.Sin(m_period) * m_radius;
		transform.position = new Vector3(x, y, z);
	}








	public void TransparentToDestroy(bool transparentToDestroy_call)
	{
		transparentToDestroy = transparentToDestroy_call;
	}



	public void TransparentToShowUp(bool transparentToShowUp_call)
	{
		transparentToShowUp = transparentToShowUp_call;
	}

    bool isFromPlaying = false;


	public void SetInfoToAnimation(bool isMoving_call, Vector3 endPos_call, float timeToArrive_Pass, bool _isFromPlaying)
	{
        if (gameObject.transform.tag != "hint" && _isFromPlaying == true)
        {
            cash.gameDirector.BeFalseOrTrueOpenCards_Retu(false);
            cash.gameDirector.BeFalseOrTrueOpenCards_Yama(false, false);
            cash.undo.GetComponent<UnityEngine.UI.Button>().enabled = false;
            cash.hint.GetComponent<UnityEngine.UI.Button>().enabled = false;
        }
        isFromPlaying = _isFromPlaying;
        elapsedTime = 0f;
        timeToArrive = timeToArrive_Pass;
		isMoving = isMoving_call;
		endPos = endPos_call;
		isMovingByPlayer = true;
	}


    bool isToRoll1 = false;
    bool isToRoll2 = false;
	public void FlipToFront(bool front_call, float speed_Rolling_Pass)
	{
        elapsedTime = 0f;
        speed_Rolling = speed_Rolling_Pass;
		beFront = front_call;
        isToRoll1 = true;
    }


	public void FlipToBack(bool back_call, float speed_Rolling_Pass)
	{
        elapsedTime = 0f;
        speed_Rolling = speed_Rolling_Pass;
        isToRoll1 = true;
        beBack = back_call;

        bc2D.enabled = false;
        //Debug.Log(card_C.suit+"_"+ card_C.numOfCard+" = "+"FlipToBack bc2D.enabled = false;");
    }


	public void MoveCardRandomlly()
	{
		bc2D.enabled = false;
		MakeRandomValue ();
		rollingAndMovingRandmlly = true;
	}

	void MakeRandomValue()
	{
		float xPos = cash.placePos.worldScreenWidth / 2f + 200f;
		float randomXF = GetRandomValue (0f, 10f);
		if (randomXF <= 5f) 
			xPos = -cash.placePos.worldScreenWidth / 2f - 200f;
		else if (randomXF >= 6f) 
			xPos = cash.placePos.worldScreenWidth / 2f + 200f;
		float yamaYPos = GameObject.Find ("row9").transform.position.y;
		float yPos = GetRandomValue (-cash.placePos.worldScreenHeight / 2 , yamaYPos);
		randomEndPos = new Vector3 (xPos, yPos, 0f);
		randomTimeToArrive = GetRandomValue (0.5f, 1.5f);
	}
	float GetRandomValue(float min, float max)
	{
		float randomValue = Random.Range(min, max);
		return randomValue;
	}






	public void SetNumOfOrderInLayer ()
	{
		int indexNum_ThisOj = cash.gameDirector.GetIndexNum(gameObject);
		if(indexNum_ThisOj != -1)
	    	GetComponent<SpriteRenderer> ().sortingOrder = indexNum_ThisOj;
	}




	public void ChangeSizeBC2D()
	{
		isChangedBoxSize = true;

		bc2D.size = new Vector2 (0.88f, 0.3f);
		bc2D.offset = new Vector2 (0f, 0.45f);
	}


	public void ResetSizeBC2D()
	{
		isChangedBoxSize = false;

		bc2D.size = new Vector2 (0.88f, 1.22f);
		bc2D.offset = new Vector2 (0f,0f);
	}





}