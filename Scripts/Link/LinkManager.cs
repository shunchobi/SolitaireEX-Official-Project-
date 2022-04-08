using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkManager : MonoBehaviour {


    const string PRIVACY_POLICY_URL = "http://one-wheat.com/app/solitaire/privacy_policy.htm";
    const string PARCHASE_POLICY_URL = "http://one-wheat.com/app/solitaire/purchase_policy.htm";
    const string PAYMENT_SERVICE_ACT_URL = "http://one-wheat.com/app/solitaire/payment_services_act.htm";
    const string ACT_ON_SPECIFIED_COMMERCIAL_TRANSACTION_URL = "http://one-wheat.com/app/solitaire/act_on_specified_commercial_transaction.htm";


    public void OnMouseDown()
    {
        TextRed(gameObject.transform.GetChild(0).GetComponent<Text>());
    }


    public void OnMouseUp()
    {
        TextBlue(gameObject.transform.GetChild(0).GetComponent<Text>());
    }


    public void VisitLink()
    {

        if (gameObject.transform.name == "purchasePolicy")
            Application.OpenURL(PARCHASE_POLICY_URL);

        if (gameObject.transform.name == "privacyPolicy")
            Application.OpenURL(PRIVACY_POLICY_URL);

        if (gameObject.transform.name == "paymentServiceAct")
            Application.OpenURL(PAYMENT_SERVICE_ACT_URL);

        if (gameObject.transform.name == "actOnSpecifiedCommercialTransaction")
            Application.OpenURL(ACT_ON_SPECIFIED_COMMERCIAL_TRANSACTION_URL);


    }



    void TextBlue(Text text)
    {
        Color color = text.color;
        color.r = 17/255f;
        color.g = 17/255f;
        color.b = 204/255f;
        color.a = 1;
        text.color = color;
    }


    void TextRed(Text text)
    {
        Color color = text.color;
        color.r = 1;
        color.g = 0;
        color.b = 0;
        color.a = 1;
        text.color = color;
    }



}
