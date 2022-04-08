using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;


public class SaveManager : MonoBehaviour {

	public Statistics.Data statisticsData;
	public BoardData boardData;
	public PreferenceData preferenceData;

	public static String BOARD_FILE_NAME = "/current_board.dat";
	public static String PREFERENCE_FILE_NAME = "/preference.dat";
	public static String STATISTICS_FILE_NAME = "/satistics.dat";


    Cash cash;


	public void InitializeSaveManager()
	{
        cash = GameObject.Find("cashObj").GetComponent<Cash>();
        Debug.Log(Application.persistentDataPath + "/current_board.dat");
	}


	/// <summary>
	/// 使いたいファイルを取得
	/// </summary>
	static String GetFilePath (string fileName) 
	{
		string fullPath = "";
		string path = Application.persistentDataPath;
		fullPath = path + fileName;
		return fullPath;
	}







	/// <summary>
	/// Loads the json.
	/// </summary>
	public string LoadJson (string fileName)
	{
		string json = "";
		string path = GetFilePath (fileName);
		if (File.Exists (path)) {

			Debug.Log (fileName + " has file");

			// StreamReaderでファイルを読み込む
			//			StreamReader reader = (new StreamReader (GetFilePath (STATISTICS_FILE_NAME), ENCODE_UTF8));
			Byte[] bytes = File.ReadAllBytes(path);
			String encrypted = Convert.ToBase64String(bytes);
			// ファイルの最後まで読み込む
			//			string encrypted = reader.ReadToEnd ();
			json = EncryptScript.DecryptBase64AesToUTF8 (encrypted); //復号化

		} else {
			Debug.Log (fileName + " new file");
		}

		return json;
	}
	/// <summary>
	/// Saves the json.
	/// </summary>
	public void saveJson (string fileName, string json)
	{
		string path = GetFilePath(fileName);
		string encryoted = EncryptScript.EncryptUTF8AesToBase64(json); //暗号化

		Byte[] bytes = Convert.FromBase64String(encryoted);
		File.WriteAllBytes(path, bytes);
	}




	//StatisticsDate

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// SAVE StatisticsDate
	/// </summary>
	public void SaveFile_StatisticsDate () 
	{
		statisticsData = cash.gameDirector.statisticsData;

		string json = JsonUtility.ToJson (statisticsData); //json形式に書き換え

		saveJson (STATISTICS_FILE_NAME, json);
	}
	/// <summary>
	/// LOAD StatisticsDate
	/// </summary>
	public Statistics.Data LoadFile_StatisticsDate() 
	{		
		string json = LoadJson (STATISTICS_FILE_NAME);

		// ファイルの存在チェック
		if (json == "") {
			//jsonファイルがなかった場合、普通にインスタンス化
			statisticsData = new Statistics.Data();
		} 
		else {
			statisticsData = JsonUtility.FromJson<Statistics.Data> (json); //jsonファイルが存在した場合、それを書き換えて、インスタンス化
		}

		return statisticsData;
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	//PreferenceDate

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// SAVE PreferenceDate
	/// </summary>
	public void SaveFile_PreferenceDate () 
	{
		preferenceData = cash.setting.preferenceDate;

		string json = JsonUtility.ToJson (preferenceData); //json形式に書き換え

		saveJson (PREFERENCE_FILE_NAME, json);
	}
	/// <summary>
	/// LOAD PreferenceDate
	/// </summary>
	public PreferenceData LoadFile_PreferenceDate() 
	{
		string json = LoadJson (PREFERENCE_FILE_NAME);
		// ファイルの存在チェック
		if (json == "") {
			preferenceData = new PreferenceData (); //jsonファイルがなかった場合、普通にインスタンス化
		} else {
			preferenceData = JsonUtility.FromJson<PreferenceData> (json); //jsonファイルが存在した場合、それを書き換えて、インスタンス化
		}

		return preferenceData;
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	//BoardData

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public void SaveFile_BoardDate () 
	{
		boardData = cash.gameDirector.boardData;

		string json = JsonUtility.ToJson (boardData); //json形式に書き換え

		saveJson (BOARD_FILE_NAME, json);
	}
	/// <summary>
	/// LOAD BoardData
	/// </summary>
	public BoardData LoadFile_BoardDate() 
	{
		string json = LoadJson (BOARD_FILE_NAME);
		// ファイルの存在チェック
		if (json == "") {
			boardData = new BoardData (); //jsonファイルがなかった場合、普通にインスタンス化
		} else {
			boardData = JsonUtility.FromJson<BoardData> (json); //jsonファイルが存在した場合、それを書き換えて、インスタンス化
		}

		return boardData;
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////


		

}
