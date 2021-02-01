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
        if (GUI.Button(new Rect(10, 10, 150, 50), "GetABFromWWW"))
        {
            StartCoroutine(GetABFromWWW());
        }
        if (GUI.Button(new Rect(10, 70, 150, 50), "Test1"))
        {
            AssetBundleTest1();
        }
    }

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
        Material mat = (Material)matAB.LoadAsset("Unlit_0");
        GetComponent<MeshRenderer>().material = mat;
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
