using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {



    Cash cash;
	Sprite frontSpite;

	public int suit;
	public int numOfCard;
	private bool isBlack;
	public bool isTurned;
	public float width;
	public float height;

    SpriteRenderer spriteRenderer;

	void Awake()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}





	
	public void SetFrontSprite()
	{
        
        string path = suit + "_" + numOfCard;


        //frontSpite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>(path);
        frontSpite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>(path);

        spriteRenderer.sprite = frontSpite;
		isTurned = true;
	}


	
	public void SetBackSprite()
	{
        spriteRenderer.sprite = cash.setting.backSprite;
		isTurned = false;
	}



	
	public bool GetBlackOrRed()
	{
		if (suit == 1 || suit == 4) isBlack = true;
		if (suit == 2 || suit == 3) isBlack = false;

		return isBlack;
	}





}
