using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetBundleLoadWWW : MonoBehaviour
{
	private string localPath = "/ABs/mat_ab";
	private string manifestFileLocalPath = "/ABs/ABs";

	void OnGUI()
	{
		if (GUI.Button(new Rect(10, 10, 150, 50), "GetABFromFile"))
		{
			StartCoroutine(GetABFromFile());
		}
		if (GUI.Button(new Rect(10, 70, 150, 50), "GetABFromWWW"))
		{
			StartCoroutine(GetABFromWWW());
		}
	}

	IEnumerator GetABFromFile()
	{
		// 加载依赖ab包
		AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.dataPath + manifestFileLocalPath);
		AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		string[] dependencies = manifest.GetAllDependencies("mat_ab");
		foreach (string dependency in dependencies)
		{
			string dependencyFilePath = Path.Combine(Application.dataPath + "/ABs", dependency);
			Debug.Log("dependencyFilePath = " + dependencyFilePath);
			AssetBundle.LoadFromFile(dependencyFilePath);
		}
		// 加载目标ab包
		string file_prefab_bundle = "file://" + Application.dataPath + localPath;
		WWW data = new WWW(file_prefab_bundle);
        yield return data;
		Material mat = (Material)data.assetBundle.LoadAsset("Unlit_0");
		GetComponent<MeshRenderer>().material = mat;
	}

	IEnumerator GetABFromWWW()
	{
		// TODO: 加载依赖ab包 

		// 加载目标ab包
		string url_prefab_bundle = "http://127.0.0.1:8000" + localPath;
		WWW data = new WWW(url_prefab_bundle);
		yield return data;
		Material mat = (Material)data.assetBundle.LoadAsset("Unlit_0");
		GetComponent<MeshRenderer>().material = mat;
	}
}
