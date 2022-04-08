using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{


    private AudioSource audioSource;
    Cash cash;

    public AudioClip flipCard; //retuのカードがめくれる時
    public AudioClip movingCard; //カードがアニメーションで移動する時
    public AudioClip tapCard; //カードをタップした時
    public AudioClip button; //プレイ中の下のメニューを押した時
    public AudioClip selectContent; //buttonを押した後での、中身を選択した時に出る音
    public AudioClip selectFlipAmount1; //ホーム画面のめくる枚数を選択するボタンを押した時の音
    public AudioClip selectFlipAmount2; //ホーム画面のめくる枚数を選択するボタンを押した時の音
    public AudioClip flipCardToStart; //ゲームを始めるために一枚のカードを弾くときの音
    public AudioClip dealCard; //カードを配る時の音
    public AudioClip moveCardToHome;
    public AudioClip shine;
    public AudioClip cheer;
    public AudioClip popUp;
    public AudioClip riseNum;


    public bool startDealSound;
    float count;
    float soundSpan;





    public void InitilaizeSoundAllMember()
    {
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        audioSource = gameObject.GetComponent<AudioSource>();

        startDealSound = false;
        count = 0f;
        soundSpan = 0.1f;
        
        flipCard = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("flip2");
        movingCard = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("slice-flip");
        tapCard = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("click6");
        button = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("click7");
        selectContent = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("click9");
        selectFlipAmount1 = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("slice4");
        selectFlipAmount2 = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("tos1");
        flipCardToStart = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("slice5");
        dealCard = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("slice6");
        moveCardToHome = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("tap-slice");
        shine = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("shine");
        cheer = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("cheer");
        popUp = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("popUp");
        riseNum = cash.assetResourceManager.assetBundle.LoadAsset<AudioClip>("riseUpNum");

    }

    void FixedUpdate()
    {
        if (startDealSound == true)
        {
            count += Time.deltaTime;
            if (soundSpan < count)
            {
                GetDealCardSound();
                soundSpan += 0.15f;
            }
        }

        if (startAllCardMoveSound == true)
        {
            count += Time.deltaTime;
            if (span < count)
            {
                GetDealCardSound();
                span += 3f;
            }
        }
    }

    public bool startAllCardMoveSound = false;
    float span = 0.1f;


    public void StopUpdateSound()
    {
        startDealSound = false;
        startAllCardMoveSound = false;
        count = 0f;
        soundSpan = 0.1f;
        span = 0.1f;
    }



    public void GetDealCardSound()
    {
        audioSource.clip = dealCard;
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.Play();
    }



    public void GetFlipCardToStartSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = flipCardToStart;
        audioSource.Play();
    }



    public void GetSelectFlipAmount1Sound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = selectFlipAmount1;
        audioSource.Play();
    }

    public void GetSelectFlipAmount2Sound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = selectFlipAmount2;
        audioSource.Play();
    }


    public void GetShineSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = shine;
        audioSource.Play();
    }





    public void GetMovingCardSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = movingCard;
        audioSource.Play();
    }




    public void GetButtonSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = button;
        audioSource.Play();
    }


    public void GetSelectContentSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = button;
        audioSource.Play();
    }


    public void GetTapCardSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = tapCard;
        audioSource.Play();
    }


    public void GetFlipCardSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = flipCard;
        audioSource.Play();

    }



    public void GetMoveCardToHomeSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = moveCardToHome;
        audioSource.Play();
    }



    public void GetCheerSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = cheer;
        audioSource.Play();
    }

    public void GetPopUpSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = popUp;
        audioSource.Play();
    }

    public void GetRiseNumSound()
    {
        if (cash.gameDirector.beSound == false)
            return;
        audioSource.clip = riseNum;
        audioSource.Play();
    }


}

