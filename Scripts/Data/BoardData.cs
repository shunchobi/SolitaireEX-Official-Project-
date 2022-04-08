using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class BoardData{

	public int remaningScan = 3;
    public bool isFirstTimeAsOpeningApp = true;
    public bool isFirstTimeAsOpeningScan = true;
    public bool isNoAds = false; 
	public bool isReviewRequested = false;
    public int scanedCount = 0;
    public float wonPointToPresent = 0f;
}
