using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace L {

	/// <summary>
	/// 文字列管理クラス(多言語対応)
	/// </summary>
	public static class Text
    {

		// 文字列格納、検索用ディクショナリー
		private static List<LocalizedText> _data;
		public static SystemLanguage _lang;
		private static bool hasInit = false;

		//初期化
		public static void Init(SystemLanguage lang)
        {
			if (hasInit)
				return;
			_lang = lang;
			_data = CSVData.GetData("Localization/Localization");
			hasInit = true;
		}


		//keyからテキストを取得
		public static string FromKey(string key)
        {
			Debug.Assert (hasInit, "TextManager has not called Init(SystemLanguage lang)");


			foreach (LocalizedText text in _data) {
				if (text.Key() == key) {
					return text.Text (_lang);
				}
			}
			return null;
		}
	}




	/// <summary>
	/// Localized text.
	/// </summary>
	/// 一つの単語を各言語で保持するクラス
	public class LocalizedText
    {
		private string key;
		private Dictionary<string, string> stringDict;


		public LocalizedText(string key) {
			this.key = key;
			this.stringDict = new Dictionary<string, string>();
		}

		public void AddLanguage(string lang, string value) {
			this.stringDict.Add (lang, value);
		}

		public string Text(SystemLanguage lang) {
			
			string langString = LanguageString.FromSystemLanguage (lang);
			return stringDict[langString];
		}

		public string Key() {

			return key;

		}
	}



	/// <summary>
	/// CSV data.
	/// </summary>
	/// CSV file を取得しLocalizedTextクラスへ変更
	public static class CSVData
    {
		public static List< LocalizedText> GetData(string filePath)
        {
			//データーの取得
			List< LocalizedText> data = new List< LocalizedText>();
			TextAsset csv = Resources.Load<TextAsset>(filePath);
			StringReader reader = new StringReader(csv.text);

			List<string> langList = new List<string>();
			int line = 0;

			while (reader.Peek () > -1) {
				List<string> valueList = new List<string> ();
				string row = reader.ReadLine ();

				//１行目の場合はkeyを取得
				if (line == 0)
                {
					langList.AddRange (row.Split (','));
				}

                else
                {
					//取得した文字列を"," で分割し配列Valuesに格納
					valueList.AddRange (row.Split (','));
					LocalizedText text = new LocalizedText (valueList [0]);

					for (int i = 1; i < valueList.Count; i++) {
                        string tempLang = langList [i];
						string tempValue = valueList [i];

						text.AddLanguage (tempLang, tempValue);
					}

					//Dictionary型に登録
					data.Add (text);
				}
				line++;
			}
			return data;
		}
	}


	/// <summary>
	/// Language string.
	/// </summary>
	/// SystemLanguageから言語名を返す
	public static class LanguageString
    {
		public static string FromSystemLanguage(SystemLanguage lang) {
			string langStr = "";

			if (lang == SystemLanguage.Japanese) {
				langStr = "Japanese";
			} else if (lang == SystemLanguage.ChineseSimplified || lang == SystemLanguage.Chinese) {
				langStr = "ChineseSimplified";
			} else if (lang == SystemLanguage.ChineseTraditional) {
				langStr = "ChineseTraditional";
			} else if (lang == SystemLanguage.Korean) {
				langStr = "Korean";
			//} else if (lang == SystemLanguage.Spanish) {
				//langStr = "Spanish";
			} else {
				langStr = "English";
			}

			return langStr;
		}
	}


}