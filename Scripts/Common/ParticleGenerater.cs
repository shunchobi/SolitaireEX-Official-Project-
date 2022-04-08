using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGenerater : MonoBehaviour {

	public GameObject brightPref;
	public GameObject fireworksPref;


	public void GenerateBrightPref(Vector3 generatePos)
	{
		brightPref.transform.position = generatePos;
		Instantiate (brightPref);
	}

}
