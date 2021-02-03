using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AssetBundle加载示例：加载ab包中的材质，替换到当前gameobject上
/// </summary>
public class ABLoaderTest : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "同步LoadAsset"))
        {
            AssetBundleTest1();
        }
        if (GUI.Button(new Rect(10, 70, 150, 50), "异步LoadAsset"))
        {
            StartCoroutine(AssetBundleTest2());
        }
		if (GUI.Button(new Rect(10, 130, 150, 50), "卸载AB"))
		{
            AssetBundleTest3();
		}
	}


    /// <summary>
    /// 同步加载 ab 包中的 asset
    /// </summary>
    void AssetBundleTest1()
    {
        // 加载目标ab包
        AssetBundle matAB = ABLoadManager.Instance.LoadAssetBundle("mat_ab");
        if (matAB == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
		// 设置材质
		Material mat = matAB.LoadAsset<Material>("Unlit_0");
		GetComponent<MeshRenderer>().material = mat;
    }

	/// <summary>
	/// 异步加载 ab 包中的 asset
	/// </summary>
	IEnumerator AssetBundleTest2()
	{
		// 加载目标ab包
		AssetBundle matAB = ABLoadManager.Instance.LoadAssetBundle("mat_ab");
		if (matAB == null)
		{
			Debug.Log("Failed to load AssetBundle!");
            yield break;
		}
		// 设置材质
		AssetBundleRequest request = matAB.LoadAssetAsync<Material>("Unlit_1");
		Debug.Log("request.progress = " + request.progress);
		yield return request;
        if (request.isDone)
		{
			GetComponent<MeshRenderer>().material = (Material)request.asset;
		}
	}

    void AssetBundleTest3()
	{
        ABLoadManager.Instance.UnLoadAssetBundle("mat_ab");
	}

	/// <summary>
	/// 从服务器下载
	/// </summary>
	/// <returns></returns>
	IEnumerator GetABFromWWW()
    {
        // TODO: 加载依赖ab包 


        // 加载目标ab包
        string url_mat_bundle = "http://127.0.0.1:8000" + "/ABs/mat_ab";
        WWW data = new WWW(url_mat_bundle);
        yield return data;
        Material mat = (Material)data.assetBundle.LoadAsset("Red");
        GetComponent<MeshRenderer>().material = mat;
    }

}
