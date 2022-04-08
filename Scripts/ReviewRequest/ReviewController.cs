using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;
#if !UNITY_IOS
using Google.Play.Review;
#endif
using UnityEngine.Events;


/// <summary>
/// レビュー関連の処理を管理するクラス
/// </summary>
public class ReviewController : MonoBehaviour//: SingletonMonoBehaviour<ReviewController>
{



    public void RequestReview()
    {
        
#if UNITY_ANDROID
        StartCoroutine("ShowReviewCoroutine");
#endif　
#if UNITY_IOS 
        UnityEngine.iOS.Device.RequestStoreReview();
#endif 
    }

#if UNITY_ANDROID
    private IEnumerator ShowReviewCoroutine()
    {
        // https://developer.android.com/guide/playcore/in-app-review/unity
        var reviewManager = new ReviewManager();
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // エラーの場合はここで止まる.
            yield break;
        }
        var playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // エラーの場合はここで止まる.
            yield break;
        }
    }
#endif



    public bool GetIsTimeToRequestReview()
    {
        //レビューをリクエストするタイミングの数字を持つList、この数字はクリア回数を表す
        List<float> showTimingNum = new List<float> { 10f, 30f, 50f };
        GameDirector gameDirector = GameObject.Find("gameDirectorObject").GetComponent<GameDirector>();
        float totalGamesWonNum = gameDirector.GetTotalGamesWonNum();
        bool willShowReviewRequest = false;
        //レビューをリクエストするタイミングかを調べる
        for (int i = 0; i < showTimingNum.Count; i++)
        {
            float showTiming = showTimingNum[i];
            if (showTiming == totalGamesWonNum)
            {
                willShowReviewRequest = true;
                break;
            }
        }

        //if (gameDirector.GetBoolIsReviewRequested () == true)
        //	willShowReviewRequest = false;

        return willShowReviewRequest;
    }





}




//レビュー依頼を表示したいところで、下記の処理を記入
//→ 3回クリアしたとき?

//レビュー依頼は年に3回しか表示されず、1度レビューしたプレイヤーには少なくとも1年はレビュー依頼が表示されない。
//下記のコードは、ShowMassageBoxクラスに書いてある

//if (ReviewManager.Instance.CanReviewInApp)
//{
//    ReviewManager.Instance.RequestReview();
//}
////Android or ios10.3以下の場合
//else
//{
//    ShowMassageBox showMassageBox = GameObject.Find("callMassageBox").GetComponent<ShowMassageBox>();
//    showMassageBox.ShowReviewRequest();
//}

