using UnityEngine;


// ScriptableObject的基础使用
// refs: https://docs.unity3d.com/Manual/class-ScriptableObject.html

public class Spawner : MonoBehaviour
{
	// The GameObject to instantiate.
	public GameObject entityToSpawn;

	// An instance of the ScriptableObject
	public SpawnManagerScriptableObject spawnManagerValues;	// 可以直接在Inspector中设置，也可以像本例子一样在运行时设置

	// This will be appended to the name of the created entities and increment when each is created.
	int instanceNumber = 1;

	void OnGUI()
	{
		if(GUI.Button(new Rect(10, 10, 150, 50), "SpawnEntities"))
		{
			SpawnEntities();
		}
	}

	void Start()
	{
		CreateScriptableObjectRunTime(1);
	}

	void CreateScriptableObjectRunTime(int way=0)
	{
		spawnManagerValues = ScriptableObject.CreateInstance<SpawnManagerScriptableObject>();

		if (way == 0)
		{
			// 可以用代码去设置
			spawnManagerValues.numberOfPrefabsToCreate = 1;
			spawnManagerValues.prefabName = "Cube";
			spawnManagerValues.spawnPoints = new Vector3[1] { Vector3.zero };
		}
		else
		{
			// 也可以读json文件来搞
			// https://docs.unity3d.com/Manual/JSONSerialization.html
			TextAsset configJson = Resources.Load("cube_spawn_config") as TextAsset;
			JsonUtility.FromJsonOverwrite(configJson.text, spawnManagerValues);
		}

	}

	void SpawnEntities()
	{
		Debug.Log("ToJson = " + JsonUtility.ToJson(spawnManagerValues, true));

		int currentSpawnPointIndex = 0;

		for (int i = 0; i < spawnManagerValues.numberOfPrefabsToCreate; i++)
		{
			// Creates an instance of the prefab at the current spawn point.
			GameObject currentEntity = Instantiate(entityToSpawn, spawnManagerValues.spawnPoints[currentSpawnPointIndex], Quaternion.identity);

			// Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
			currentEntity.name = spawnManagerValues.prefabName + instanceNumber;

			// Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
			currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManagerValues.spawnPoints.Length;

			instanceNumber++;
		}
	}
}
