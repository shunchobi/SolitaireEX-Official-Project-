using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssetResourceManager : MonoBehaviour {



    public AssetBundle assetBundle;


    //pngの取り出し
    //image.GetComponent<Image>().sprite = cash.assetResourceManager.assetBundle.LoadAsset<Sprite>("1_1");




    public void Inti()
    {
        assetBundle = ReadAssetBundleAssetBundle("assetbundle");
       
    }



    private static Dictionary<string, AssetBundle> assetBundleCache = new Dictionary<string, AssetBundle>();

    private static AssetBundle ReadAssetBundleAssetBundle(string key)
    {
        AssetBundle assetBundle = null;


        if (!string.IsNullOrEmpty(key))
        {
            if (assetBundleCache != null)
            {
                if (assetBundleCache.ContainsKey(key))
                {
                    assetBundle = assetBundleCache[key];
                }
            }
            if (assetBundle == null)
            {

#if UNITY_EDITOR
                assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/iOS/" + key);
#elif UNITY_ANDROID
                assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Android/" + key);
#elif UNITY_IOS
                assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/iOS/" + key);
#else
                assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/iOS/" + key);
#endif


                if (assetBundleCache.ContainsKey(key))
                {
                    assetBundleCache[key] = assetBundle;
                }
                else
                {
                    assetBundleCache.Add(key, assetBundle);
                }
            }
        }
        return assetBundle;
    }
}



