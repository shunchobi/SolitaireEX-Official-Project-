using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class PreferenceData  {


	public bool isSounding = true;
	public bool isRightHand = true;
	public bool isOneFlipChecked = true;
	public bool isShowingTimeAndMove = true;

	public bool isPurchased = false;

	public bool isRewarded = false;
	public string lastTime;
	public float noAdsCountdownResult;


	public int backCardDesignNum = 1;
	public int playmatDesignNum = 1;




}
