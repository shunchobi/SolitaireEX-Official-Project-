using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorChange {

	SaveManager saveManager;

	void Start () {
		saveManager = GameObject.Find ("gameDirectorObject").GetComponent<SaveManager> ();
	}



	void Awake ()
	{
		EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
	}


	private void OnPlayModeStateChanged ()
	{
		if (EditorApplication.isPaused) {
			saveManager.SaveFile_StatisticsDate ();
		}
		if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode) {
			saveManager.SaveFile_StatisticsDate ();
		}
	}



}
