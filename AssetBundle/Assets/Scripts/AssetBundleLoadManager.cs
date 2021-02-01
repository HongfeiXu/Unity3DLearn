using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// AssetBundle加载示例：加载ab包中的材质，替换到当前gameobject上
/// </summary>
public class AssetBundleLoadManager : MonoBehaviour
{
	/// <summary>
	/// 单例
	/// </summary>
	private static AssetBundleLoadManager m_Instance;
	public static AssetBundleLoadManager Instance
	{
		get
		{
			if (null == m_Instance)
				m_Instance = new AssetBundleLoadManager();
			return m_Instance;
		}
	}


	private string m_abFolder = "";
	private Dictionary<string, AssetBundle> m_abDic = new Dictionary<string, AssetBundle>();

	private AssetBundleManifest m_manifest;

	void Start()
	{
		Init();
	}

	/// <summary>
	/// 初始化一些路径，以及ab包的manifest
	/// </summary>
	void Init()
	{
		m_abFolder = Path.Combine(Application.dataPath, "ABs");
		AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(m_abFolder, "ABs"));
		m_manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
	}

	public AssetBundle LoadAssetBundle(string abName)
	{
		// 加载目标ab包
		AssetBundle ab = null;
		if (!m_abDic.ContainsKey(abName))
		{
			string abPath = Path.Combine(m_abFolder, abName);
			ab = AssetBundle.LoadFromFile(abPath);
			m_abDic[abName] = ab;
		}
		else
		{
			ab = m_abDic[abName];
		}

		// 加载依赖
		string[] dependencies = m_manifest.GetAllDependencies("mat_ab");
		foreach (string dependency in dependencies)
		{
			Debug.Log("dependency = " + dependency);
			if (!m_abDic.ContainsKey(dependency))
			{
				string abPath = Path.Combine(m_abFolder, dependency);
				Debug.Log("abPath = " + abPath);
				ab = AssetBundle.LoadFromFile(abPath);
				m_abDic[dependency] = ab;
			}
		}

		return ab;
	}


	#region TEST

	void OnGUI()
	{
		if (GUI.Button(new Rect(10, 10, 150, 50), "GetABFromFileUseWWW"))
		{
			StartCoroutine(GetABFromFileUseWWW());
		}
		if (GUI.Button(new Rect(10, 70, 150, 50), "Test1"))
		{
			AssetBundleTest1();
		}
	}

	/// <summary>
	/// 从本地文件加载
	/// </summary>
	/// <returns></returns>
	IEnumerator GetABFromFileUseWWW()
	{
		// 加载依赖ab包
		// Not work!!!
		string[] dependencies = m_manifest.GetAllDependencies("mat_ab");
		foreach (string dependency in dependencies)
		{
			string dependencyFilePath = "file://" + Application.dataPath + "/ABs/" + dependency;
			Debug.Log("dependencyFilePath = " + dependencyFilePath);
			WWW data2 = new WWW(dependencyFilePath);
			yield return data2;
		}

		// 加载目标ab包
		string file_mat_bundle = "file://" + Path.Combine(m_abFolder, "mat_ab");
		WWW data3 = new WWW(file_mat_bundle);
		//WWW data3 = WWW.LoadFromCacheOrDownload(file_mat_bundle, 0);
		yield return data3;
		Material mat = (Material)data3.assetBundle.LoadAsset("Unlit_0");
		GetComponent<MeshRenderer>().material = mat;
	}

	void AssetBundleTest1()
	{
		// 加载目标ab包
		AssetBundle matAB = LoadAssetBundle("mat_ab");
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

	#endregion
}
