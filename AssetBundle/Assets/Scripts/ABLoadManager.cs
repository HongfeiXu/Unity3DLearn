using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 
/// </summary>
public class ABLoadManager
{
	/// <summary>
	/// 单例
	/// </summary>
	private static ABLoadManager m_Instance;
	public static ABLoadManager Instance
	{
		get
		{
			if (null == m_Instance)
            {
                m_Instance = new ABLoadManager();
                m_Instance.Init();
            }
            return m_Instance;
		}
	}


	private string m_abFolder = "";
	private Dictionary<string, AssetBundle> m_abDic = new Dictionary<string, AssetBundle>();

	private AssetBundleManifest m_manifest;

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
		AssetBundle target_ab = null;
		if (!m_abDic.ContainsKey(abName))
		{
			string abPath = Path.Combine(m_abFolder, abName);
            target_ab = AssetBundle.LoadFromFile(abPath);
			m_abDic[abName] = target_ab;
		}
		else
		{
            target_ab = m_abDic[abName];
		}

		// 加载依赖
		string[] dependencies = m_manifest.GetAllDependencies("mat_ab");
        AssetBundle ab = null;
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

		return target_ab;
	}

	public void UnLoadAssetBundle(string abName)
	{
		if (!m_abDic.ContainsKey(abName))
		{
			return;
		}
		m_abDic[abName].Unload(true);
		m_abDic.Remove(abName);
	}
}
