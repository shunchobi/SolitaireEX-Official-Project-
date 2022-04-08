using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row8Blinker : MonoBehaviour {


    SpriteRenderer spRenderer;
	// Use this for initialization
	void Start () {
        spRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

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
        color.r = (spRenderer.color.r == 0) ? 1.0f : 0.0f;
        color.g = (spRenderer.color.g == 0) ? 1.0f : 0.0f;
        color.b = (spRenderer.color.b == 0) ? 1.0f : 0.0f;

        //color.a = (spRenderer.color.a == 0) ? 1.0f : 0.0f;
        spRenderer.color = color;
    }
    private void ColorAlpherTo(float a)
    {
        var color = spRenderer.color;
        color.a = a;
        spRenderer.color = color;
    }


}
