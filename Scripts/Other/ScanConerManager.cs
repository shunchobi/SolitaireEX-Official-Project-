using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanConerManager : MonoBehaviour {


    Cash cash;

    public Vector3 scanPos;
    Vector3 outPos;



	void Start ()
    {
        cash = GameObject.Find("cashObj").GetComponent<Cash>();

        float baseWidth = 640f;
        float baseHeight = 1136f;
        float thisWidth = Screen.width;
        float thisHeight = Screen.height;

        float xScale = thisWidth / baseWidth;
        float yScale = thisHeight / baseHeight;

        float width = GameObject.Find("GameObject (3)").GetComponent<SpriteRenderer>().bounds.size.x;
        float scaleNum_Card = cash.placePos.worldScreenWidth / width;
        
        Vector3 scale = gameObject.transform.localScale;
        scale.x = scaleNum_Card;
        scale.y = scaleNum_Card;
        scale.z = scaleNum_Card;
        gameObject.transform.localScale = scale;

        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        float acpect = screenHeight / screenWidth;
        if (acpect < 1.6f)
        {
            Vector3 smallScale = gameObject.transform.localScale;
            smallScale.x = 0.9f;
            smallScale.y = 0.9f;
            smallScale.z = 1f;
            gameObject.transform.localScale = smallScale;
        }

        outPos = new Vector3(0f, -1000f, 0f);
        CloseScanCover();
    }




    public void CloseScanCover()
    {
        gameObject.transform.position = outPos;
    }



    public void MoveScanCover()
    {
        scanPos = new Vector3(0f, Camera.main.ScreenToWorldPoint(GameObject.Find("scanFieldCurrent").transform.position).y, -10f);
        gameObject.transform.position = scanPos;
    }




}
