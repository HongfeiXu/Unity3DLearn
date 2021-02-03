using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildAssetBundle
{
	[MenuItem("AssetBundle/Build Windows AB")]
	static void BuildWindowsAssetBundles()
	{
		BuildAssetBundles(BuildTarget.StandaloneWindows);
	}

	[MenuItem("AssetBundle/Build Android AB")]
	static void BuildAndroidAssetBundles()
	{
		BuildAssetBundles(BuildTarget.Android);
	}

	[MenuItem("AssetBundle/Build iOS AB")]
	static void BuildIOSAssetBundles()
	{
		BuildAssetBundles(BuildTarget.iOS);
	}


	static void BuildAssetBundles(BuildTarget buildTarget)
	{
		// Put the bundles in a folder called "ABs" within the Assets folder.
		BuildPipeline.BuildAssetBundles("Assets/ABs", BuildAssetBundleOptions.None,buildTarget);
	}
}
