using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


namespace Statistics {
	
	public enum TypeOfGame {
		NONE = -1,
		FLIP1_NONE_SCAN =0,
		FLIP1_SCAND,
		FLIP3_NONE_SCAN,
		FLIP3_SCAND,
		NUM_OF_TYPES,
	};

	[System.Serializable]
	public class Data {
		
		public List<Statistics.GameScore> statisticsAllData;

		public GameScore DataFromType(TypeOfGame type)
		{
			GameScore currentData = null;

			foreach (GameScore data in statisticsAllData) {
				if (type == data.typeOfGame)
					currentData = data;
			}
			return currentData;
		}


		public Data() 
		{
			statisticsAllData = new List<Statistics.GameScore> ();

			for (int i = 0; i < (int)Statistics.TypeOfGame.NUM_OF_TYPES; i++) {
				Statistics.GameScore data = new Statistics.GameScore ();
				data.typeOfGame = (Statistics.TypeOfGame)i;
				statisticsAllData.Add (data);
			}
		}
	}


	[System.Serializable]
	public class GameScore {
    
		public TypeOfGame typeOfGame = TypeOfGame.NONE;

		public float gamesWonCount = 0;
		public float gamesCount = 0;
		public float bestTime = 0;
		public float bestScore = 0;
		public float bestMove = 0;
		public float bestTotalScore = 0;
		public float playtime = 0;

		//プレイしたゲームでベストスコアが出た場合、クリア後の成績が表示される際に、以前までのベストスコアではなく、
		//このゲームで出たベストスコアが”Best Score”として表示されていたので、それを防ぐために以下の4つの変数を設けた。
		//これ以上このゲームのコードをあちこちいじりたくないです。スパゲッティなので。
		public float bestTimePast = 0;
		public float bestScorePast = 0;
		public float bestMovePast = 0;
		public float bestTotalScorePast = 0;




		public static TypeOfGame getTypeOfGame (bool isScand, bool isFlip1) 
		{
			if (isScand == false && isFlip1) {
				return TypeOfGame.FLIP1_NONE_SCAN;
			}
			if (isScand && isFlip1) {
				return TypeOfGame.FLIP1_SCAND;
			}
			if (isScand == false && isFlip1 == false) {
				return TypeOfGame.FLIP3_NONE_SCAN;
			}
			if (isScand && isFlip1 == false) {
				return TypeOfGame.FLIP3_SCAND;
			}
			return TypeOfGame.NONE;
		}
	}
}