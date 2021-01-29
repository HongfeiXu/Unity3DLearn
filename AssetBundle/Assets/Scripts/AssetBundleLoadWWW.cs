using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleLoadWWW : MonoBehaviour
{
	private string localPath = "/ABs/unlit_0_ab";

	// Start is called before the first frame update
	void Start()
    {
		//StartCoroutine(GetABFromFile());
		StartCoroutine(GetABFromWWW());
	}

	IEnumerator GetABFromFile()
	{
        string file_prefab_bundle = "file://" + Application.dataPath + localPath;
        WWW data = new WWW(file_prefab_bundle);
        yield return data;
		Material mat = (Material)data.assetBundle.LoadAsset("Unlit_0");
		GetComponent<MeshRenderer>().material = mat;
	}

	IEnumerator GetABFromWWW()
	{
		string url_prefab_bundle = "http://127.0.0.1:8000" + localPath;
		WWW data = new WWW(url_prefab_bundle);
		yield return data;
		Material mat = (Material)data.assetBundle.LoadAsset("Unlit_0");
		GetComponent<MeshRenderer>().material = mat;
	}
}
