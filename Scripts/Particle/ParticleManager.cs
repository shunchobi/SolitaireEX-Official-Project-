using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {


	GameObject fireWorksPref;
	GameObject brightPref;


	void Start()
	{
		string fireWorksPrafabPath = "Prefab" + "/" + "fireWorks";
		fireWorksPref = Resources.Load<GameObject>(fireWorksPrafabPath);
		string brightPrafabPath = "Prefab" + "/" + "bright";
		brightPref = Resources.Load<GameObject>(brightPrafabPath);
	}


	public void ShowFireWorks()
	{
		GameObject fireWorks = Instantiate (fireWorksPref) as GameObject;
		GameObject gamesWonBackG = GameObject.Find ("gamesWonBackG");
		float gamesWonBackGHeight = gamesWonBackG.GetComponent<RectTransform> ().rect.height;
		Vector3 gamesWonBackGPos = Camera.main.ScreenToWorldPoint(gamesWonBackG.transform.position);
		Vector3 fireWorksPos = new Vector3 (gamesWonBackGPos.x, gamesWonBackGPos.y - gamesWonBackGHeight/3f, 0f);
		fireWorks.transform.position = fireWorksPos;
	}


	public void DestroyFireWorks()
	{
		GameObject gamesWonBackG = GameObject.Find ("fireWorks(Clone)");
		Destroy (gamesWonBackG);
	}



	public void CreatBright(Vector3 pos)
	{
		GameObject bright = Instantiate (brightPref) as GameObject;
		bright.transform.position = pos;
	}

	
}
