using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabChanger 
{
	public static void ExecuteChange(List<string> targets, System.Action<GameObject> onChange)
	{
		if (onChange == null) return;

		foreach (string path in targets)
		{
			var prefabAsset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
			if (prefabAsset == null) continue;

			var prefab = PrefabUtility.InstantiatePrefab (prefabAsset) as GameObject;
			if (prefab == null) continue;

			onChange(prefab);
			PrefabUtility.ReplacePrefab(prefab, prefabAsset);

			GameObject.DestroyImmediate(prefab);
		}
	}

	public static void ScanTree(GameObject gameObj, System.Action<GameObject> onExecute)
	{
		if (gameObj == null) return;
		if (onExecute == null) return;

		onExecute(gameObj);

		if (gameObj.transform.childCount > 0)
		{
			for (int i = 0; i < gameObj.transform.childCount; ++i)
			{
				var child = gameObj.transform.GetChild(i);
				ScanTree(child.gameObject, onExecute);
			}
		}
	}
}
