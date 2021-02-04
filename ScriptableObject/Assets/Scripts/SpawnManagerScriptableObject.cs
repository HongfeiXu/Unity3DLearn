using UnityEngine;

// refs: https://docs.unity3d.com/Manual/class-ScriptableObject.html

[CreateAssetMenu(fileName ="Data", menuName ="ScriptableObjects/SpawnManagerScriptableObject")]
public class SpawnManagerScriptableObject : ScriptableObject
{
	public string prefabName;

	public int numberOfPrefabsToCreate;
	public Vector3[] spawnPoints;
}
